using NewLife.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewLife.SyncDB
{
    internal class SyncSetting : Config<SyncSetting>
    {
        [Description("同步名称")]
        public string ServiceName { get; set; } = "订单同步";
        [Description("旧数据库连接")]
        public string MainConn { get; set; }
        [Description("新数据库连接")]
        public string BakConn { get; set; }
        [Description("线程数")]
        public int ThreadCount { get; set; } = 3;
        [Description("原服务器IP")]
        public string ServiceIP { get; set; }
        [Description("表配置")]
        public List<TableSet> Tables { get; set; }=new List<TableSet>();

        public class TableSet
        {
            [Description("表名")]
            public string Name { get; set; }
            [Description("自增键（可选）")]
            public string IdentityKey { get; set; }
            [Description("主键")]
            public string PrimaryKey { get; set; }
            [Description("表排序，一般推荐UpdateTime")]
            public string OrderColumns { get; set; }
            [Description("是否是分表")]
            public bool IsSubTable { get; set; }
            [Description("分表正则")]
            public string SubRegex { get; set; }
            [Description("同步的起始时间")]
            public bool? IsSync { get; set; }
            [Description("同步的起始时间")]
            public string SyncTime { get; set; }
        }

    }


    [Config("ClientUI", null)]//其实可以一起配置，主要是为了以后能单独剥离ui
    [DisplayName("Win客户端皮肤设置")]
    internal class ClientUISetting : Config<ClientUISetting>
    {
        [Description("客户端皮肤")]
        public int UIStyle { get; set; } = 1;
    }

}
