using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using NewLife.SyncDB.Entity;
using XCode.DataAccessLayer;

namespace NewLife.SyncDB.Ava.Views;

public partial class MainWindow : Window
{
    private List<string> _excludeTables = new List<string>();
    public MainWindow()
    {
        InitializeComponent();
        
        //排除掉系统表名
        _excludeTables.AddRange(new string[] { "sqlite_sequence", "SqliteSequence" });

        this.Title = SyncSetting.Current.ServiceName;
        
        this.btnSetting.Click+=BtnSettingOnClick;
        this.btnReadStruct.Click+= BtnReadStructOnClick;
        
        DAL.AddConnStr("BakDB", SyncSetting.Current.MainConn, null, null);
        DAL.AddConnStr("ToBakDB", SyncSetting.Current.BakConn, null, null);
    }

    private void BtnReadStructOnClick(object? sender, RoutedEventArgs e)
    {
        this.btnReadStruct.IsEnabled = false;
        
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
                //NeedManual = isNeedManual ? GetResourcesImg("warn_16.Image") : GetResourcesImg("right_16.Image"),
            };
        }).OrderBy(e => e.Name).ToList();


        //异步刷新条数
        Task.Run(() => FetchRows(models));

        //dataGrid.AutoGenerateColumns = true;
        
        
        
        dataGrid.ItemsSource = new ObservableCollection<TableModel>(models);
    }
    
     #region >设计列<
        private void CreateColumns()
        {
            DataGridBoundColumn col = new DataGridTextColumn { Header = "状态", Binding = new Binding($"NeedManual")};

            // col.DataPropertyName = "NeedManual";
            // col.HeaderText = "状态";
            // col.MinimumWidth = 6;
            // col.Name = "needFixImg";
            // col.DisplayIndex = 0;
            // col.Width = 64;

            this.dataGrid.Columns.Add(col);

            col = new DataGridTextColumn { Header = "名称", Binding = new Binding($"Name")};
            // col.DataPropertyName = "Name";
            // col.HeaderText = "名称";
            // col.MinimumWidth = 6;
            // col.Name = "name";
            // col.Width = 180;
            // col.DisplayIndex = 1;
            this.dataGrid.Columns.Add(col);

            col = new DataGridTextColumn { Header = "昵称", Binding = new Binding($"DisplayName")};
            // col.DataPropertyName = "DisplayName";
            // col.HeaderText = "昵称";
            // col.MinimumWidth = 6;
            // col.Name = "displayName";
            // col.Width = 180;
            // col.DisplayIndex = 2;
             this.dataGrid.Columns.Add(col);
            //
            col = new DataGridCheckBoxColumn { Header = "是否同步", Binding = new Binding($"EnableSync")};
            // col.DataPropertyName = "EnableSync";
            // col.HeaderText = "是否同步";
            // col.MinimumWidth = 6;
            // col.Name = "enableSync";
            // col.Width = 80;
            // col.DisplayIndex = 3;
            this.dataGrid.Columns.Add(col);
            //
            col = new DataGridTextColumn { Header = "源表行数", Binding = new Binding($"Total")};
            // col.DataPropertyName = "Total";
            // col.HeaderText = "源表行数";
            // col.MinimumWidth = 6;
            // col.Name = "total";
            // col.Width = 100;
            // col.DisplayIndex = 4;
            this.dataGrid.Columns.Add(col);
            //
            col = new DataGridTextColumn { Header = "已同步", Binding = new Binding($"Finish")};
            // col.DataPropertyName = "Finish";
            // col.HeaderText = "已同步";
            // col.MinimumWidth = 6;
            // col.Name = "finish";
            // col.Width = 100;
            // col.DisplayIndex = 5;
            this.dataGrid.Columns.Add(col);
            //
            col = new DataGridTextColumn { Header = "目标行数", Binding = new Binding($"Total2")};
            // col.DataPropertyName = "Total2";
            // col.HeaderText = "目标行数";
            // col.MinimumWidth = 6;
            // col.Name = "total2";
            // col.Width = 100;
            // col.DisplayIndex = 6;
            this.dataGrid.Columns.Add(col);
            //
            col = new DataGridTextColumn { Header = "描述", Binding = new Binding($"Description"),Width =new DataGridLength(200) };
            // col.DataPropertyName = "Description";
            // col.HeaderText = "描述";
            // col.MinimumWidth = 6;
            // col.Name = "description";
            // col.Width = 200;
            // col.DisplayIndex = 7;
            this.dataGrid.Columns.Add(col);
            
        }

        #endregion
    
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
            return true; 
        }

        return false; 
    }

    private void FetchRows(List<Entity.TableModel> models)
    {
        var dal = DAL.Create("BakDB");
        foreach (var item in models)
        {
            var sb = new SelectBuilder { Table = item.RealName };
            item.Total = dal.SelectCount(sb);
        }

        Dispatcher.UIThread.Invoke(() =>
        {
            dataGrid.ItemsSource = models;
        });
        //this.Invoke(() => { dataGridView1.Refresh(); });

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
            //this.Invoke(() => { dataGridView1.Refresh(); });
        }

        //return Task.CompletedTask;
    }
    
    private void BtnSettingOnClick(object? sender, RoutedEventArgs e)
    {
        btnSetting.IsEnabled = false;

        var win = new WinConfig();

        win.ShowDialog(this);

        btnSetting.IsEnabled = true;
    }
}