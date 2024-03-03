using NewLife.SyncDB.Ext;
using NewLife;
using NewLife.Caching;
using NewLife.Data;
using NewLife.Serialization;
using NewLife.Threading;
using Sunny.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCode.DataAccessLayer;

namespace NewLife.SyncDB
{
    public partial class FrmSync : UIForm
    {
        private int _round = 1;
        private Action _closeAct = null;

        private List<SyncSetting.TableSet> _syncTables = null;

        private IList<IDataTable> _dbTables = null;
        private ConcurrentQueue<SyncSetting.TableSet> _tablesQueue = new ConcurrentQueue<SyncSetting.TableSet>();
        private List<SyncSetting.TableSet> _remainTables = new List<SyncSetting.TableSet>();

        private List<TimerX> _timers = new List<TimerX>();//处理同步线程
        private TimerX _nextTimer = null;

        private Stopwatch _stopwatch = new Stopwatch();

        private MemoryCache _orderColCache = new MemoryCache();//预防没设置排序表

        public FrmSync(Action closeAct)
        {
            InitializeComponent();

            this._closeAct = closeAct;
            var allTablesCnf = SyncSetting.Current.Tables;//前面判断一下到底有没有需要同步的表
            var syncTables = allTablesCnf.FindAll(x =>
            {
                if (x.SyncTime.IsNullOrEmpty())
                    return false;

                if (x.IsSync.HasValue && !x.IsSync.Value)//不需要同步
                    return false;

                return true;
            });


            if (syncTables.Count == 0)
            {
                UIMessageTip.ShowError("没有可同步的表！", 5000);
                return;
            }

            this._syncTables = syncTables;

            var dal = DAL.Create("BakDB");

            _dbTables = dal.Tables;



        }

        private void DoCheck(object state)
        {
            if (_remainTables.Count > 0)
            {
                this.Invoke(() =>
                {
                    this.labRemainTables.Text = _remainTables.Count.ToString();
                });

                return;
            }

            _stopwatch.Stop();

            var totalMins = _stopwatch.Elapsed.TotalMinutes;

            //如果不够一分钟得算一分钟，不然下面算均值会很大
            if (totalMins < 1)
                totalMins = 1;

            _round++;

            //var totalTable = _syncTables.Count;

            //var totalRow = _syncModels.Sum(x => x.Total);//会有-1的列，忽略不计吧

            //var rowPerMin = (int)Math.Ceiling(totalRow / totalMins);
            RestQueue();



            this.Invoke(() =>
            {
                this.Text = $"正在同步第{_round}轮";
                this.labTotalSpan.Text = totalMins.ToString();
            });


        }

        private void DoSyncTable(object state)
        {
            if (_tablesQueue.Count == 0)
            {
                return;
            }

            _tablesQueue.TryDequeue(out var table);

            if (table == null)
            {
                //预防多线程拿不到值
                return;
            }

            var endTime= DateTime.Now;

            WriteStatuLog($"正在同步表[{table.Name}]".LogFormat());


            //寻找主键和排序
            var dbTable = _dbTables.Find(x => x.TableName == table.Name);


            #region >寻找排序键<
            var orderCol = table.OrderColumns;
            if (orderCol.IsNullOrEmpty())
            {
                WriteStatuLog($"表[{table.Name}]没有时间序列，会默认使用时间字段，请及时配置！".LogFormat());

                orderCol = _orderColCache.GetOrAdd<string>(table.Name, k =>
                {
                    if (dbTable == null)
                        return string.Empty;

                    var timeCols = dbTable.Columns.Where(w => w.Name.ToLower().Contains("time")).Select(x => x.Name).ToList();

                    var col = timeCols.Find(x => x.ToLower() == "updatetime");//如果有updatetime优先使用updatetime
                    if (!col.IsNullOrEmpty())
                        return col;

                    return timeCols.FirstOrDefault();


                    //return string.Empty;
                });
            }

            if (orderCol.IsNullOrEmpty())
            {
                WriteStatuLog($"表[{table.Name}]没有可用的排序列，跳过同步！".LogFormat());
                _remainTables.Remove(table);
                return;

            }

            #endregion

            #region >寻找主键或者自增键<
            IDataColumn idKeyCol = null;//寻找唯一键

            if (table.IdentityKey.IsNullOrEmpty() && table.PrimaryKey.IsNullOrEmpty())
            {
                WriteStatuLog($"表[{table.Name}]没有主键或者自增键，会默认使用唯一字段，请及时配置！".LogFormat());

                var pk = dbTable.Columns.FirstOrDefault(e => e.PrimaryKey);
                if (pk != null)
                {
                    idKeyCol = pk;
                }

                if (idKeyCol == null)//继续查找自增键
                {
                    var id = dbTable.Columns.FirstOrDefault(e => e.Identity);
                    if (id != null)
                    {
                        idKeyCol = id;
                    }
                }

                if (idKeyCol == null)
                {
                    WriteStatuLog($"表[{table.Name}]没有可用的唯一键，跳过同步！".LogFormat());
                    _remainTables.Remove(table);
                    return;
                }

            }
            else//有主键优先拿主键
            {
                if (!table.PrimaryKey.IsNullOrEmpty())
                {
                    var col = dbTable.Columns.FirstOrDefault(e => e.Name == table.PrimaryKey);
                    if (col != null)
                    {
                        idKeyCol = col;
                    }
                }

                if (idKeyCol == null && !table.IdentityKey.IsNullOrEmpty())
                {
                    var col = dbTable.Columns.FirstOrDefault(e => e.Name == table.IdentityKey);
                    if (col != null)
                    {
                        idKeyCol = col;
                    }
                }

                if (idKeyCol == null)
                {
                    throw new Exception("配置的唯一键值有误！这会影响同步的准确性");
                }
            }

            #endregion

            //var sb = new SelectBuilder { Table = table.Name, Where = $"{orderCol}>='{table.SyncTime}'", Limit = "Limit 1" };
            var sb = new SelectBuilder { Table = table.Name, Where = $"{orderCol}>='{table.SyncTime}'" };

            var dal = DAL.Create("BakDB");
            var dt = dal.Query(sb);

            if (dt == null || dt.Rows.Count == 0)
            {
                WriteStatuLog($"表[{table.Name}]没有可同步的数据，跳过同步！".LogFormat());
                _remainTables.Remove(table);
                return;
            }

            //var list = dt.Cast<IModel>();

            //其实下面的逻辑都可以用使用Upsert搞定，但是觉得批量提交的数据库语句有点大，而且量大的时候会超时，感觉

            var idKeyIndex = dbTable.Columns.FindIndex(x => x == idKeyCol);

            if (idKeyIndex == -1)
            {
                throw new Exception("不可能找不到唯一键的索引！");
            }

            var toDal = DAL.Create("ToBakDB");

            var insCols = dbTable.Columns.ToArray();
            var c = insCols.Find(x => x.Identity);
            //把自增键去掉，不然会插入不了自增键的数据
            if (c != null) c.Identity = false;


            var udCols = insCols.Select(x => x.Name).ToArray();

            foreach (var row in dt.Rows)
            {
                var value = row[idKeyIndex];

                var str = row.ToJson();

                //var m = row.Cast<IModel>();

                sb = new SelectBuilder { Table = table.Name, Where = $"{idKeyCol.Name}='{value}'" };

                var toDt = toDal.Query(sb);

                if (toDt != null && toDt.Rows.Count > 0)
                {
                    var oldData = toDt.Rows[0].ToJson();
                    var newData = row.ToJson();

                    if (oldData == newData)
                    {
                        WriteStatuLog($"表[{table.Name}] - [{idKeyCol.Name}] :值：{value} 没变化，跳过更新！".LogFormat());

                        continue;
                    }



                    var uDt = dt.Clone(); //new DbTable();
                    uDt.Rows = new List<object[]>();
                    uDt.Rows.Add(row);

                    var list = uDt.Cast<IModel>();

                    toDal.Session.Upsert(dbTable, dbTable.Columns.ToArray(), udCols, null, list);

                    toDt = toDal.Query(sb);

                    str = toDt.Rows[0].ToJson();

                    WriteStatuLog($"表[{table.Name}] - [{idKeyCol.Name}] :值：{value} 已更新！".LogFormat());
                }
                else
                {
                    var uDt = dt.Clone();
                    uDt.Rows = new List<object[]>();
                    uDt.Rows.Add(row);

                    toDal.Insert(uDt, dbTable,insCols);
                    WriteStatuLog($"表[{table.Name}] - [{idKeyCol.Name}] :值：{value} 不存在！".LogFormat());
                }

            }




            ////把自增键去掉，不然会影响效率，作为备份库，自增键也不是很重要
            //var col = table.Columns.FirstOrDefault(e => e.Identity);
            //if (col != null)
            //{
            //    col.Identity = false;
            //}

            //var dal = DAL.Create("BakDB");

            //var toDal = DAL.Create("ToBakDB");
            //var toTables = toDal.Tables;



            table.SyncTime=endTime.ToString("yyyy-MM-dd HH:mm:ss");
            SyncSetting.Current.Save();

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
        }

        private void RestQueue()
        {
            foreach (var table in this._syncTables)
            {
                _tablesQueue.Enqueue(table);
                _remainTables.Add(table);
            }
            _stopwatch.Restart();

            this.Invoke(new Action(() =>
            {
                this.labRemainTables.Text = _remainTables.Count.ToString();
            }));
        }

        private void WriteStatuLog(string message)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                if (txtLog.Text.Length > 3000)
                    txtLog.Text = txtLog.Text.Substring(1000, txtLog.Text.Length - 1000);
                txtLog.Text += Environment.NewLine + message;
                txtLog.SelectionStart = txtLog.TextLength;
                txtLog.ScrollToCaret();

            });

            Invoke(mi);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_closeAct != null)
            {
                _closeAct.Invoke();
            }
            this.Close();
        }

        private void FrmSync_Shown(object sender, EventArgs e)
        {
            RestQueue();

            //启动同步线程，跟复制线程的数量一样
            for (int i = 0; i < SyncSetting.Current.ThreadCount; i++)
            {
                _timers.Add(new TimerX(DoSyncTable, null, i * 1_000, 1_000) { Async = true });
            }

            _nextTimer = new TimerX(DoCheck, null, 5_000, 500) { Async = true };//应该不会半分钟同步完的
        }
    }
}
