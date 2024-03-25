using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewLife.SyncDB.Ext;
using NewLife.SyncDB.Properties;
using NewLife;
using NewLife.Data;
using NewLife.Security;
using Sunny.UI;
using XCode.DataAccessLayer;
using XCode.Transform;

namespace NewLife.SyncDB
{
    public partial class MainForm : UIForm
    {
        private List<string> _excludeTables = new List<string>();


        private DataGridViewExt _dataGridViewExt = null;

        public MainForm()
        {
            InitializeComponent();

            //排除掉系统表名
            _excludeTables.AddRange(new string[] { "sqlite_sequence", "SqliteSequence" });

            //var a= System.Configuration.ConfigurationManager.AppSettings.Get("AAA");

            //如果不是默认皮肤
            if (ClientUISetting.Current.UIStyle != 1)
            {
                StyleManager.Style = (UIStyle)ClientUISetting.Current.UIStyle;
            }

            DAL.AddConnStr("BakDB", SyncSetting.Current.MainConn, null, null);
            DAL.AddConnStr("ToBakDB", SyncSetting.Current.BakConn, null, null);
            

            var styles = UIStyles.PopularStyles();
            foreach (UIStyle style in styles)
            {
                this.uiContextMenuStrip1.Items.Add(style.ToString());
                
            }

            if (!SyncSetting.Current.ServiceName.IsNullOrEmpty())
                this.Text = SyncSetting.Current.ServiceName;

            _dataGridViewExt = new DataGridViewExt(this.dataGridView1, "operate");//把多按钮扩展实例一下
            
        }

        private void uiContextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var styleTxt = e.ClickedItem.Text;

            if (styleTxt.IsNullOrEmpty())
            {
                return;
            }
            UIStyle style;
            if (Enum.TryParse<UIStyle>(styleTxt, out style))
            {
                StyleManager.Style = style;
            }
            ClientUISetting.Current.UIStyle = (int)style;
            ClientUISetting.Current.Save();

        }

        private void toolSetting_Click(object sender, EventArgs e)
        {
            new FrmConfig().ShowDialog();
        }

        private async void toolReadStruct_Click(object sender, EventArgs _)
        {
            this.dataGridView1.AutoGenerateColumns = false;
            this.toolReadStruct.Enabled = false;
            //this.dataGridView1.RowHeadersVisible = false;

            var frm = new UIProcessIndicatorForm();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Size = new Size(50, 50);
            frm.Show();



            //新增前置的检测列
            CreateColumns();



            var dal = DAL.Create("BakDB");
            var toDal = DAL.Create("ToBakDB");
            // 获取数据表
            var tables = dal.Tables?.Where(x => !_excludeTables.Contains(x.Name)).ToList();
            

            //var t = toDal.GetCreateTableSql(ts);

            var models = tables.Select(e =>
            {
                var isNeedManual = GetNeedManual(e);
                return new Entity.TableModel
                {
                    Name = e.TableName,
                    RealName = e.TableName,
                    DisplayName = e.DisplayName,
                    EnableSync = true,
                    IsNeedManual = isNeedManual,
                    NeedManual = isNeedManual ? GetResourcesImg("warn_16.Image") : GetResourcesImg("right_16.Image"),
                };
            }).OrderBy(e => e.Name).ToList();


            //异步刷新条数
            //Task.Run(() =>
            //{
            await Task.Run(() => FetchRows(models));
            //});

            dataGridView1.DataSource = models;
            dataGridView1.Tag = tables;

            //最后遍历所有行，注册事件或者增添提示
            ModifyRowsAct();
            //隐藏分表
            HiddenSubTables();

            frm.Hide();

        }

        private void HiddenSubTables()
        {
            var tables = SyncSetting.Current.Tables.FindAll(x => x.IsSubTable);

            //var tableName = model.Name;
            var subRegex = string.Empty;
            foreach (var table in tables)
            {
                Entity.TableModel firstOne = null;
                var total = 0;
                var finish = 0;
                var count = 0;
                for (var i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    var rowModel = dataGridView1.Rows[i].DataBoundItem as Entity.TableModel;

                    if (Regex.IsMatch(rowModel.RealName, table.SubRegex))
                    {
                        if (rowModel.Total > 0)
                            total += rowModel.Total;
                        if (rowModel.Finish > 0)
                            finish += rowModel.Finish;
                        count++;
                        //第一行不用隐藏
                        if (firstOne == null)
                        {
                            firstOne = rowModel;
                            firstOne.Name = table.Name;
                        }
                        else
                        {
                            rowModel.MasterName = table.Name;
                            this.dataGridView1.Rows[i].Visible = false;
                        }
                    }
                }

                if (firstOne != null)
                {
                    firstOne.Total = total;
                    firstOne.Finish = finish;
                    firstOne.DisplayName = $"{table.Name}[1-{count}]";
                    firstOne.MasterName = table.Name;
                }

                //if (total > 0)
                //{

                //}

                //if (Regex.IsMatch(tableName, table.SubRegex))
                //{
                //    tableName = table.Name;
                //    subRegex = table.SubRegex;
                //    break;
                //}
            }
        }

        private void ModifyRowsAct()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var item = dataGridView1.Rows[i].DataBoundItem as Entity.TableModel;

                if (item != null && item.IsNeedManual)
                {
                    dataGridView1.Rows[i].Cells["needFixImg"].ToolTipText = "需要配置主键或排序列[单击配置]";
                }


                #region >一列新增多个按钮<
                //新增操作按钮
                var rowLabels = new List<Control>();

                var btn = _dataGridViewExt.GetBtnByType("btnConfig", "配置", i);
                btn.Tag = i;
                btn.Click += Lab_Click;
                rowLabels.Add(btn);

                btn = _dataGridViewExt.GetBtnByType("btnIgnore", "忽略", i, Color.Red);
                btn.Tag = i;
                rowLabels.Add(btn);

                _dataGridViewExt.CreateCustomOperateRow(i, rowLabels); //CreateOperateRow(i);

                #endregion

            }
        }

        private bool GetNeedManual(IDataTable table)
        {
            var tableCnf = SyncSetting.Current.Tables.Find(x => x.Name == table.Name);

            //已经配置了就不提示了
            if (tableCnf != null)
            {
                if (tableCnf.IdentityKey.IsNullOrEmpty() && tableCnf.PrimaryKey.IsNullOrEmpty())
                {
                    goto Check;
                }
                else
                {
                    return false;
                }
            }

        Check:
            var id = table.Columns.FirstOrDefault(e => e.Identity);
            if (id == null)
            {
                var pks = table.PrimaryKeys;
                if (pks != null && pks.Length == 1)
                    id = pks[0];

            }

            var pk = table.Columns.FirstOrDefault(e => e.PrimaryKey);
            // 没有排序的自动
            var dc = table.Columns.FirstOrDefault(e => e.DataType == typeof(DateTime));

            if (id == null || pk == null || dc == null)
            {
                return true; //((System.Drawing.Image)(Resources.ResourceManager.GetObject("warn_16.Image")));
            }

            return false; //((System.Drawing.Image)(Resources.ResourceManager.GetObject("right_16.Image")));
        }
        private Image GetResourcesImg(string sourceName)
        {
            return ((Image)(Resources.ResourceManager.GetObject(sourceName)));
        }

        #region >设计列<
        private void CreateColumns()
        {
            DataGridViewColumn col = new DataGridViewImageColumn();

            col.DataPropertyName = "NeedManual";
            col.HeaderText = "状态";
            col.MinimumWidth = 6;
            col.Name = "needFixImg";
            col.DisplayIndex = 0;
            col.Width = 64;

            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Name";
            col.HeaderText = "名称";
            col.MinimumWidth = 6;
            col.Name = "name";
            col.Width = 180;
            col.DisplayIndex = 1;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "DisplayName";
            col.HeaderText = "昵称";
            col.MinimumWidth = 6;
            col.Name = "displayName";
            col.Width = 180;
            col.DisplayIndex = 2;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewCheckBoxColumn();
            col.DataPropertyName = "EnableSync";
            col.HeaderText = "是否同步";
            col.MinimumWidth = 6;
            col.Name = "enableSync";
            col.Width = 80;
            col.DisplayIndex = 3;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Total";
            col.HeaderText = "源表行数";
            col.MinimumWidth = 6;
            col.Name = "total";
            col.Width = 100;
            col.DisplayIndex = 4;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Finish";
            col.HeaderText = "已同步";
            col.MinimumWidth = 6;
            col.Name = "finish";
            col.Width = 100;
            col.DisplayIndex = 5;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Total2";
            col.HeaderText = "目标行数";
            col.MinimumWidth = 6;
            col.Name = "total2";
            col.Width = 100;
            col.DisplayIndex = 6;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Description";
            col.HeaderText = "描述";
            col.MinimumWidth = 6;
            col.Name = "description";
            col.Width = 200;
            col.DisplayIndex = 7;
            this.dataGridView1.Columns.Add(col);



            col = new DataGridViewLinkColumn();
            //col.DataPropertyName = "Description";
            col.HeaderText = "操作";
            col.MinimumWidth = 6;
            col.Name = "operate";
            col.Width = 85;
            col.DisplayIndex = 8;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewCheckBoxColumn();
            col.DataPropertyName = "IsNeedManual";
            col.Name = "isNeedManual";
            col.Visible = false;//已经通过图标显示
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "RealName";
            col.Name = "realName";
            col.Visible = false;//已经通过图标显示
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "MasterName";
            col.Name = "masterName";
            col.Visible = false;//已经通过图标显示
            this.dataGridView1.Columns.Add(col);
        }

        #endregion

        private void FetchRows(List<Entity.TableModel> models)
        {
            var dal = DAL.Create("BakDB");
            foreach (var item in models)
            {
                var sb = new SelectBuilder { Table = item.RealName };
                item.Total = dal.SelectCount(sb);
            }

            this.Invoke(() => { dataGridView1.Refresh(); });

            var toDal = DAL.Create("ToBakDB");

            var toTables = toDal.Tables;

            if (toTables?.Count > 0)
            {
                foreach (var item in models)
                {
                    //判断存不存在
                    var toTable = toTables.Find(x => x.TableName == item.RealName);
                    if (toTable != null)
                    {
                        var sb = new SelectBuilder { Table = item.RealName };
                        item.Finish = toDal.SelectCount(sb);
                    }
                }
                this.Invoke(() => { dataGridView1.Refresh(); });
            }

            //return Task.CompletedTask;
        }

        private void toolReplication_Click(object sender, EventArgs e)
        {
            toolStrip1.Enabled = false;

            var models = this.dataGridView1.DataSource as List<Entity.TableModel>;
            var tables = this.dataGridView1.Tag as IList<IDataTable>;

            if (tables == null)
            {
                UIMessageTip.ShowError("请先【读取】表结构和数据！");
                toolStrip1.Enabled = true;
                return;
            }

            var toDal = DAL.Create("ToBakDB");
            var toTables = toDal.Tables;

            if (tables?.Count > 0 && toTables?.Count > 0)
            {
                var ft = tables[0];

                var m = toTables.Find(x => x.TableName == ft.TableName);
                if (m != null)
                {
                    if (!UIMessageBox.Show("已经复制过，是否要重新复制？", "警告", UIStyle.Blue, UIMessageBoxButtons.OKCancel))
                    {
                        return;//已经复制过就不复制，以防无触碰
                    }
                    //如果还是坚持要同步，把已同步的数量清零，不然会累加
                    models.ForEach(x =>
                    {
                        x.Finish = 0;
                    });

                }
            }

            ////检查一下数据库是否复制过
            //if (tables?.Count > 0)
            //{
            //    var first = tables.FirstOrDefault();
            //    var toDal = DAL.Create("ToBakDB");
            //    toDal.
            //}

            var frm = new FrmCopy(() =>
            {
                toolStrip1.Enabled = true;
            }, tables, models);
            frm.OnRefreshData += Frm_OnRefreshData;
            frm.StartPosition = FormStartPosition.Manual;

            var mousePos = Control.MousePosition;
            frm.Location = new Point(mousePos.X + 120, mousePos.Y - 10);
            frm.ShowDialog();
        }

        private void Frm_OnRefreshData(object sender, EventArgs e)
        {
            this.Invoke(() =>
            {
                this.dataGridView1.Refresh();
            });
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0) return;//排除掉首列
            var rowIndex = e.RowIndex;
            if (rowIndex == -1) return;

            var item = dataGridView1.Rows[rowIndex].DataBoundItem as Entity.TableModel;
            //每次点击都会引起计算，但是就是省两行代码
            var tables = dataGridView1.Tag as IList<IDataTable>;
            var table = tables.Find(x => x.Name == item.Name);

            //首列配置
            if (this.dataGridView1.Columns[e.ColumnIndex].Name == "needFixImg" &&
                item != null && item.IsNeedManual)
            {
                var mousePos = Control.MousePosition;
                var frm = new FrmTableConfig(table);
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(mousePos.X - 5, mousePos.Y - 10);

                frm.ShowDialog(this);
            }
            //是否同步列
            if (this.dataGridView1.Columns[e.ColumnIndex].Name == "enableSync")
            {
                if (table != null)
                {
                    var setTable = SyncSetting.Current.Tables.Find(x => x.Name == table.Name);
                    if (setTable == null)
                    {
                        setTable = new SyncSetting.TableSet
                        {
                            Name = table.Name,
                        };
                        SyncSetting.Current.Tables.Add(setTable);
                    }
                    //setTable.IsSync = this.dataGridView1.Rows[rowIndex].Cells[e.ColumnIndex].Value.ToBoolean();//不知道为啥value永远时true
                    setTable.IsSync = !this.dataGridView1.Rows[rowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToBoolean();
                    SyncSetting.Current.Save();
                }
            }

        }

        private void Lab_Click(object sender, EventArgs e)
        {
            var btn = sender as Label;
            var rowIndex = btn.Tag.ToInt();

            var item = dataGridView1.Rows[rowIndex].DataBoundItem as Entity.TableModel;

            if (item != null)
            {
                var tables = dataGridView1.Tag as IList<IDataTable>;

                var table = tables.Find(x => x.Name == item.RealName);

                var mousePos = Control.MousePosition;
                var frm = new FrmTableConfig(table);
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(mousePos.X - 5, mousePos.Y - 10);

                frm.ShowDialog(this);
            }

            //int rowIndex = ;
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            _dataGridViewExt.ReDrawOperateCtl(e);
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)//标题栏跳过
                return;

            switch (e.ColumnIndex)
            {
                case 1://表名列
                    if (e.Button == MouseButtons.Right)
                    {
                        ContextMenuStripUseManyTable.Tag = dataGridView1.Rows[e.RowIndex].DataBoundItem as Entity.TableModel; ;
                        ContextMenuStripUseManyTable.Show(MousePosition.X, MousePosition.Y);
                    }
                    break;
            }

            //switch (e.ColumnIndex)
            //{
            //    case dataGridView1.Columns[""].Index:
            //}

            //if (e.ColumnIndex < 0)


        }

        private void ContextMenuStripUseManyTable_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "设为分表")
            {
                if (ContextMenuStripUseManyTable.Tag != null)
                {
                    var model = ContextMenuStripUseManyTable.Tag as Entity.TableModel;

                    var frm = new FrmSubTable(() =>
                    {
                        HiddenSubTables();
                    }, model);

                    var mousePos = Control.MousePosition;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = new Point(mousePos.X - 5, mousePos.Y - 10);
                    frm.ShowDialog();

                }
            }
        }

        private void toolSyncData_Click(object sender, EventArgs e)
        {
            this.toolStrip1.Enabled = false;

            var frm = new FrmSync(() =>
            {
                this.toolStrip1.Enabled = true;
            });
            frm.ShowDialog();


        }

        //private void DataGridView1_RowsAdded(object sender, System.Windows.Forms.DataGridViewRowsAddedEventArgs e)
        //{
        //    var rowIndex = e.RowIndex;
        //    var item = dataGridView1.Rows[rowIndex].DataBoundItem as Entity.TableModel;
        //    if (item.IsNeedManual)
        //    {
        //        dataGridView1[rowIndex, 1].ToolTipText = "需要配置主键或排序列";
        //    }
        //}
    }


}
