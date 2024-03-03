using NewLife;
using NewLife.Security;
using NewLife.Threading;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using XCode.DataAccessLayer;

namespace NewLife.SyncDB
{
    public partial class FrmCopy : Sunny.UI.UIForm
    {
        private Action _closeAct = null;
        private IList<IDataTable> _syncTables = null;
        private List<Entity.TableModel> _syncModels = null;
        public event EventHandler<EventArgs> OnRefreshData;

        private Stopwatch _stopwatch = new Stopwatch();
        private ConcurrentQueue<IDataTable> _tablesQueue = new ConcurrentQueue<IDataTable>();
        private List<IDataTable> _remainTables = new List<IDataTable>();

        private List<TimerX> _timers = new List<TimerX>();//处理迁移线程
        private TimerX _stopTimer = null;

        public FrmCopy(Action closeAct, IList<IDataTable> syncTables, List<Entity.TableModel> syncModels)
        {
            InitializeComponent();



            this._closeAct = closeAct;
            this._syncTables = syncTables;

            foreach (IDataTable table in syncTables)
            {
                _tablesQueue.Enqueue(table);
                _remainTables.Add(table);
            }

            this._syncModels = syncModels;

            for (int i = 0; i < SyncSetting.Current.ThreadCount; i++)
            {
                _timers.Add(new TimerX(DoSyncTable, null, i * 1_000, 1_000) { Async = true });
            }

            _stopTimer = new TimerX(DoCheck, null, 5_000, 500) { Async = true };//应该不会半分钟同步完的

            _stopwatch.Start();
        }

        private void DoCheck(object state)
        {
            if (_remainTables.Count > 0)
                return;

            _stopwatch.Stop();

            var totalMins = _stopwatch.Elapsed.TotalMinutes;

            //如果不够一分钟得算一分钟，不然下面算均值会很大
            if (totalMins < 1)
                totalMins = 1;
            var totalTable = _syncTables.Count;

            var totalRow = _syncModels.Sum(x => x.Total);//会有-1的列，忽略不计吧

            var rowPerMin = (int)Math.Ceiling(totalRow / totalMins);

            this.Invoke(() =>
            {
                _stopTimer.Dispose();
                var frm = new FrmCopyStats(totalTable, Convert.ToInt32(totalMins), totalRow, rowPerMin);
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            });
        }

        private void DoSyncTable(object state)
        {
            if (_syncTables.Count == 0)
            {
                //这里写入同步完毕逻辑，但是有可能还是没有完全同步完
                return;
            }

            _tablesQueue.TryDequeue(out var table);

            if (table == null)
            {
                //预防多线程拿不到值
                return;
            }

            //把自增键去掉，不然会影响效率，作为备份库，自增键也不是很重要
            var col = table.Columns.FirstOrDefault(e => e.Identity);
            if (col != null)
            {
                col.Identity = false;
            }

            var dal = DAL.Create("BakDB");

            var toDal = DAL.Create("ToBakDB");
            var toTables = toDal.Tables;

            //dal.Insert(new { Id = Rand.Next(), Name = Rand.NextString(8) }, "user");

            if (toTables != null && toTables.FirstOrDefault(e => e.TableName == table.TableName) != null)
            {
                toDal.Execute($"DELETE FROM [{table.TableName}]");//删除原有数据
            }
            //else
            //{
            //    toDal.SetTables(table);//先建个表，然后把自增键去掉就能同步自增键的值了
            //}



            //logWriter.Invoke($"正在同步[{syncTable}]表".LogFormat());

            var dpk = new DbPackage
            {
                Dal = dal,
                IgnoreError = true,
                Log = NewLife.Log.XTrace.Log,
                BatchInsert = true,
                //BatchSize = 200,
            };

            dpk.OnPage += Dpk_OnPage;
            //if (toDal.DbType == DatabaseType.SqlServer)
            //{
            //    toDal.Execute($"SET IDENTITY_INSERT {table.TableName} ON");
            //}

            var rs = dpk.Sync(table, toDal.ConnName);

            var model = _syncModels.Find(x => x.RealName == table.Name);


            if (model != null)
            {
                var isSub = false;
                var subTableName = string.Empty;
                if (!model.MasterName.IsNullOrEmpty())
                {
                    isSub = true;
                    subTableName = model.RealName;
                    model = _syncModels.Find(x => x.Name == model.MasterName && !x.MasterName.IsNullOrEmpty());//找到他的主表
                    if (model == null)
                    {
                        throw new Exception("找不到所属的主表，是否有误");
                    }
                }

                var sb = new SelectBuilder { Table = model.RealName };
                model.Finish = toDal.SelectCount(sb);
                if (isSub)
                {
                    model.Description = $"子表({subTableName})已复制完成【100%】";
                }
                else
                {
                    model.Description = "已复制完成【100%】";
                }

                OnRefreshData?.Invoke(this, null);
            }

            _remainTables.Remove(table);

            var setTable = SyncSetting.Current.Tables.Find(x => x.Name == table.Name);
            if (setTable == null)
            {
                setTable = new SyncSetting.TableSet
                {
                    Name = table.Name,

                };
                SyncSetting.Current.Tables.Add(setTable);
            }
            setTable.SyncTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SyncSetting.Current.Save();

            //if (toDal.DbType == DatabaseType.SqlServer)
            //{
            //    toDal.Execute($"SET IDENTITY_INSERT {table.TableName} OFF");
            //}

        }

        private void Dpk_OnPage(object sender, PageEventArgs e)
        {
            var table = e.Table;

            var model = _syncModels.Find(x => x.RealName == table.Name);

            if (model != null)
            {

                var all = model.Total;
                //var count = (int)e.Row;//这是错的，Row是返回最大Id
                var count = model.Finish;
                count += e.Page.Total;
                var rate = Convert.ToInt32(count * 100 / all);

                var isSub = false;
                var subTableName = string.Empty;
                var masterName = string.Empty;
                if (!model.MasterName.IsNullOrEmpty())
                {
                    isSub = true;
                    subTableName = model.RealName;
                    masterName = model.MasterName;
                    model = _syncModels.Find(x => x.Name == model.MasterName && !x.MasterName.IsNullOrEmpty());//找到他的主表
                    if (model == null)
                    {
                        throw new Exception("找不到所属的主表，是否有误");
                    }
                }

                model.Finish = count;
                if (isSub)
                {
                    var shortName = subTableName.Replace(masterName, "");//没有正则，为了简短一点，没办法了

                    model.Description = $"子表({shortName})进度【{rate}%】";
                }
                else
                {
                    model.Description = $"进度【{rate}%】";
                }

                if (OnRefreshData != null)
                    OnRefreshData?.Invoke(this, null);
            }


            //e.
            //throw new NotImplementedException();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_closeAct != null)
            {
                _closeAct.Invoke();
            }
            this.Close();
        }
    }
}
