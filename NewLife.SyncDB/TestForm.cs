using NewLife.SyncDB.Properties;
using NewLife;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCode.DataAccessLayer;

namespace NewLife.SyncDB
{
    public partial class TestForm : Sunny.UI.UIForm
    {
        //List<ActionButtons> mButtons = new List<ActionButtons>();
        private List<string> _excludeTables = new List<string>();
        public TestForm()
        {
            InitializeComponent();

            var styles = UIStyles.PopularStyles();
            foreach (UIStyle style in styles)
            {
                this.uiContextMenuStrip1.Items.Add(style.ToString());
                //uiNavBar1.CreateChildNode(uiNavBar1.Nodes[0], style.DisplayText(), style.Value());
            }

            if (!SyncSetting.Current.ServiceName.IsNullOrEmpty())
                this.Text = SyncSetting.Current.ServiceName;

            //this.dataGridView1.RowsAdded += DataGridView1_RowsAdded;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    this.dataGridView1.RowsAdded += DataGridView1_RowsAdded;

        //    CreateColumns();

        //    this.dataGridView1.Rows.Add();
        //}

        //private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        //{
        //    AddButtons();
        //    RepositionActionButtons();
        //}

        //private void AddButtons()
        //{
        //    Button btnAdd = new Button();
        //    btnAdd.Text = "配置";
        //    //btnAdd.Click += onAddButtonClick;
        //    dataGridView1.Controls.Add(btnAdd);

        //    Button btnRemove = new Button();
        //    btnRemove.Text = "忽略";
        //    //btnRemove.Click += onRemoveButtonClick;
        //    dataGridView1.Controls.Add(btnRemove);

        //    mButtons.Add(new ActionButtons(btnAdd, btnRemove));
        //}

        //private void UpdateActionButtonsPosition(ActionButtons ab, Rectangle rect, int rowIndex)
        //{
        //    dataGridView1.Invoke(() =>
        //    {
        //        ab.AddButton.Location = new Point(rect.Left + 5, rect.Top + 5);
        //        ab.AddButton.Size = new Size(rect.Width / 2 - 10, rect.Height - 10);
        //        ab.AddButton.Visible = true;

        //        ab.RemoveButton.Location = new Point(ab.AddButton.Left + ab.AddButton.Width + 5, rect.Top + 5);
        //        ab.RemoveButton.Size = ab.AddButton.Size;
        //        ab.RemoveButton.Visible = rowIndex > 0;
        //    });
        //}

        //private void RepositionActionButtons()
        //{
        //    Task.Run(() =>
        //    {
        //        Thread.Sleep(100);

        //        int firstRow = dataGridView1.FirstDisplayedScrollingRowIndex;
        //        int lastRow = firstRow + dataGridView1.DisplayedRowCount(false);
        //        int firstCol = dataGridView1.FirstDisplayedScrollingColumnIndex;
        //        int lastCol = firstCol + dataGridView1.DisplayedColumnCount(true);

        //        var col = this.dataGridView1.Columns["operate"];

        //        for (int i = 0; i < mButtons.Count; i++)
        //        {
        //            ActionButtons ab = mButtons[i];
        //            ab.AddButton.Tag = i;
        //            ab.RemoveButton.Tag = i;

        //            if (i >= firstRow && i <= lastRow &&
        //                col.Index >= firstCol && col.Index <= lastCol)
        //            {
        //                Rectangle rect = this.dataGridView1.GetCellDisplayRectangle(col.Index, i, false);
        //                UpdateActionButtonsPosition(ab, rect, i);
        //            }
        //            //else
        //            //{
        //            //    HideAllActionButtons(ab);
        //            //}
        //        }
        //    });
        //}


        private void toolReadStruct_Click(object sender, EventArgs e)
        {
            //this.dataGridView1.RowsAdded += dataGridView1_RowsAdded;
            this.dataGridView1.AutoGenerateColumns = false;

            CreateColumns();

            DAL.AddConnStr("BakDB", SyncSetting.Current.MainConn, null, null);
            DAL.AddConnStr("ToBakDB", SyncSetting.Current.BakConn, null, null);

            var dal = DAL.Create("BakDB");
            // 获取数据表
            var tables = dal.Tables;
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
            }).Where(e => !_excludeTables.Contains(e.Name))
              .OrderBy(e => e.Name).ToList();

            //models.re
            //异步刷新条数
            Task.Run(() =>
            {
                FetchRows(models);
            });

            dataGridView1.DataSource = models;
            dataGridView1.Tag = tables;

            ////新增后置的操作列，需要一列加所有操作，所以只能代码生成
            //CreateOperateColumns();
            ////隐藏没必要显示的列
            //HideColumns();
            //AddToolTip();

            //this.dataGridView1.Rows.Add();
        }

        private void CreateOperateColumns()
        {
            var col = new DataGridViewLinkColumn();

            col.HeaderText = "操 作";
            col.Name = "operate";
            //col.MinimumWidth = 230;
            col.Width = 100;


            this.dataGridView1.Columns.Add(col);
        }

        private void CreateOperateRow(int rowIndex)
        {
            var col = this.dataGridView1.Columns["operate"];

            if (col.Width < 300)
                col.Width = 300;

            var colIndex = col.Index;//这个会变，所以还是动态获取一下

            var rectangle = this.dataGridView1.GetCellDisplayRectangle(colIndex, rowIndex, true);//获取当前单元格的绘制区域
            var btnSize = new Size(rectangle.Width / 2 - 10, rectangle.Height - 10); //new Size(rectangle.Width / 3 + 1, rectangle.Height);
            var btnLeft = rectangle.Left;

            //配置按钮
            var btn1 = GetBtnByType("btnConfig", "配置", rowIndex);
            this.dataGridView1.Controls.Add(btn1);
            btn1.Size = btnSize;
            btn1.Location = new Point(btnLeft, rectangle.Top + 5);

            //忽略按钮
            //btnLeft += btn.Width;//为下一个绘制增加起点
            var btn2 = GetBtnByType("btnIgnore", "忽略", rowIndex, Color.Red);
            this.dataGridView1.Controls.Add(btn2);
            btn2.Size = btnSize;
            btn2.Location = new Point(btn1.Left + btn1.Width + 5, rectangle.Top + 5); //new Point(btnLeft, rectangle.Top + 5);

        }
        private UILabel GetBtnByType(string btnName, string btnText, int rowIndex, Color? fontColor = null)
        {
            if (fontColor == null)
            {
                fontColor = Color.FromArgb(64, 158, 255);
            }

            var lab = new UILabel();
            lab.Name = btnName;
            lab.Text = btnText;
            lab.ForeColor = fontColor.Value;
            lab.Font = new Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            lab.BackColor = Color.Transparent;
            lab.Tag = rowIndex;
            lab.AutoSize = true;
            lab.Click += Lab_Click;

            return lab;
        }

        private void Lab_Click(object sender, EventArgs e)
        {
            var btn = sender as UILabel;
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

        private void AddToolTip()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var item = dataGridView1.Rows[i].DataBoundItem as Entity.TableModel;

                if (item != null && item.IsNeedManual)
                {
                    //dataGridView1[i, 0].ToolTipText = "需要配置主键或排序列";
                    //dataGridView1.Rows[i].HeaderCell.ToolTipText = "需要配置主键或排序列";
                    dataGridView1.Rows[i].Cells["needFixImg"].ToolTipText = "需要配置主键或排序列[单击配置]";
                }

                //CreateOperateRow(i);

                //if (i < dataGridView1.Rows.Count - 1)//i=最后一行时，那是虚拟的行，这是用datagridview常犯的错
                //{
                //    //新增操作按钮
                //    CreateOperateRow(i);
                //}
            }
        }

        private void HideColumns()
        {
            if (dataGridView1.Rows?.Count > 0)
            {
                dataGridView1.Columns["IsNeedManual"].Visible = false;
            }
        }

        private bool GetNeedManual(IDataTable table)
        {
            var tableCnf = SyncSetting.Current.Tables.Find(x => x.Name == table.Name);

            //已经配置了就不提示了
            if (tableCnf != null)
            {
                return false;
            }

            var id = table.Columns.FirstOrDefault(e => e.Identity);
            if (id == null)
            {
                var pks = table.PrimaryKeys;
                if (pks != null && pks.Length == 1)
                    id = pks[0];

                //if (id == null)
                //{
                //    return ((System.Drawing.Image)(Resources.ResourceManager.GetObject("warn_16.Image")));
                //}
            }


            var pk = table.Columns.FirstOrDefault(e => e.PrimaryKey);
            // 没有排序的自动
            var dc = table.Columns.FirstOrDefault(e => e.ColumnName.ToLower().Contains("time"));

            if (id == null || pk == null || dc == null)
            {
                return true; //((System.Drawing.Image)(Resources.ResourceManager.GetObject("warn_16.Image")));
            }

            return false; //((System.Drawing.Image)(Resources.ResourceManager.GetObject("right_16.Image")));
        }
        private System.Drawing.Image GetResourcesImg(string sourceName)
        {
            return ((System.Drawing.Image)(Resources.ResourceManager.GetObject(sourceName)));
        }

        private void CreateColumns()
        {
            DataGridViewColumn col = new DataGridViewImageColumn();

            col.DataPropertyName = "NeedManual";
            col.HeaderText = "状态";
            col.MinimumWidth = 6;
            col.Name = "needFixImg";
            col.Width = 40;
            //col.Index = 0;
            //col.ImageLayout = DataGridViewImageCellLayout.Normal;

            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Name";
            col.HeaderText = "名称";
            col.MinimumWidth = 6;
            col.Name = "name";
            col.Width = 125;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "DisplayName";
            col.HeaderText = "昵称";
            col.MinimumWidth = 6;
            col.Name = "displayName";
            col.Width = 125;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewCheckBoxColumn();
            col.DataPropertyName = "EnableSync";
            col.HeaderText = "是否同步";
            col.MinimumWidth = 6;
            col.Name = "enableSync";
            col.Width = 125;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Total";
            col.HeaderText = "源表行数";
            col.MinimumWidth = 6;
            col.Name = "total";
            col.Width = 125;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Finish";
            col.HeaderText = "已同步";
            col.MinimumWidth = 6;
            col.Name = "finish";
            col.Width = 125;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Total2";
            col.HeaderText = "目标行数";
            col.MinimumWidth = 6;
            col.Name = "total2";
            col.Width = 125;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.DataPropertyName = "Description";
            col.HeaderText = "描述";
            col.MinimumWidth = 6;
            col.Name = "description";
            col.Width = 125;
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewCheckBoxColumn();
            col.DataPropertyName = "IsNeedManual";
            col.HeaderText = "是否配置";
            col.MinimumWidth = 6;
            col.Name = "isNeedManual";
            col.Width = 125;
            col.Visible = false;//已经通过图标显示
            this.dataGridView1.Columns.Add(col);

            col = new DataGridViewLinkColumn();
            //col.DataPropertyName = "Description";
            col.HeaderText = "操作";
            col.MinimumWidth = 6;
            col.Name = "operate";
            col.Width = 125;
            col.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridView1.Columns.Add(col);
        }

        void FetchRows(List<Entity.TableModel> models)
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

        private void toolSetting_Click(object sender, EventArgs e)
        {
            new FrmConfig().ShowDialog();
        }



        //private void AddButtons()
        //{
        //    Button btnAdd = new Button();
        //    btnAdd.Text = "配置";
        //    btnAdd.Click += onAddButtonClick;
        //    dataGridView1.Controls.Add(btnAdd);

        //    Button btnRemove = new Button();
        //    btnRemove.Text = "忽略";
        //    btnRemove.Click += onRemoveButtonClick;
        //    dataGridView1.Controls.Add(btnRemove);

        //    mButtons.Add(new ActionButtons(btnAdd, btnRemove));
        //}

        //private void RemoveButtons(int rowIndex)
        //{
        //    if (rowIndex >= 0 && rowIndex < mButtons.Count)
        //    {
        //        ActionButtons ab = mButtons[rowIndex];
        //        ab.AddButton.Visible = false;
        //        ab.AddButton.Dispose();
        //        ab.RemoveButton.Visible = false;
        //        ab.RemoveButton.Dispose();

        //        mButtons.RemoveAt(rowIndex);
        //    }
        //}

        //private void HideAllActionButtons(ActionButtons ab)
        //{
        //    dataGridView1.Invoke(() =>
        //    {
        //        ab.AddButton.Visible = ab.RemoveButton.Visible = false;
        //    });
        //}

        //private void UpdateActionButtonsPosition(ActionButtons ab, Rectangle rect, int rowIndex)
        //{
        //    dataGridView1.Invoke(() =>
        //    {
        //        ab.AddButton.Location = new Point(rect.Left + 5, rect.Top + 5);
        //        ab.AddButton.Size = new Size(rect.Width / 2 - 10, rect.Height - 10);
        //        ab.AddButton.Visible = true;

        //        ab.RemoveButton.Location = new Point(ab.AddButton.Left + ab.AddButton.Width + 5, rect.Top + 5);
        //        ab.RemoveButton.Size = ab.AddButton.Size;
        //        ab.RemoveButton.Visible = true;
        //    });
        //}

        //private void RepositionActionButtons()
        //{
        //    Task.Run(() =>
        //    {
        //        Thread.Sleep(100);

        //        var btnColIndex = this.dataGridView1.Columns["operate"].DisplayIndex;

        //        int firstRow = dataGridView1.FirstDisplayedScrollingRowIndex;
        //        int lastRow = firstRow + dataGridView1.DisplayedRowCount(false);
        //        int firstCol = dataGridView1.FirstDisplayedScrollingColumnIndex;
        //        int lastCol = firstCol + dataGridView1.DisplayedColumnCount(true);

        //        for (int i = 0; i < mButtons.Count; i++)
        //        {
        //            ActionButtons ab = mButtons[i];
        //            ab.AddButton.Tag = i;
        //            ab.RemoveButton.Tag = i;

        //            if (i >= firstRow && i <= lastRow &&
        //                btnColIndex >= firstCol && btnColIndex <= lastCol)
        //            {
        //                Rectangle rect = this.dataGridView1.GetCellDisplayRectangle(btnColIndex, i, false);
        //                UpdateActionButtonsPosition(ab, rect, i);
        //            }
        //            else
        //            {
        //                HideAllActionButtons(ab);
        //            }
        //        }
        //    });
        //}

        //private void onAddButtonClick(object? sender, EventArgs e)
        //{
        //    if (sender != null)
        //    {
        //        Button btn = (Button)sender;
        //        int rowIndex = (int)btn.Tag;
        //        if (rowIndex == dataGridView1.Rows.Count - 1)
        //        {
        //            dataGridView1.Rows.Add();
        //        }
        //        else
        //        {
        //            dataGridView1.Rows.Insert(rowIndex + 1, 1);
        //        }
        //    }
        //}

        //private void onRemoveButtonClick(object? sender, EventArgs e)
        //{
        //    if (sender != null)
        //    {
        //        Button btn = (Button)sender;
        //        int rowIndex = (int)btn.Tag;
        //        dataGridView1.Rows.RemoveAt(rowIndex);
        //    }
        //}

        //private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        //{
        //    AddButtons();
        //    RepositionActionButtons();
        //}

        //private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        //{
        //    RemoveButtons(e.RowIndex);
        //    RepositionActionButtons();
        //}

        //private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        //{
        //    RepositionActionButtons();
        //}

        //private void dataGridView1_SizeChanged(object sender, EventArgs e)
        //{
        //    RepositionActionButtons();
        //}

        //private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        //{
        //    RepositionActionButtons();
        //    this.Refresh();
        //}
    }
}
