using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewLife.SyncDB.Entity
{
    /// <summary>
    /// 表模型
    /// </summary>
    public class TableModel
    {
        [DisplayName("名称")]
        public String Name { get; set; }

        [DisplayName("昵称")]
        public String DisplayName { get; set; }

        /// <summary>启用同步</summary>
        [DisplayName("同步")]
        public Boolean EnableSync { get; set; }

        [DisplayName("源表行数")]
        public Int32 Total { get; set; } = -1;

        [DisplayName("已同步")]
        public Int32 Finish { get; set; } = -1;

        [DisplayName("目标行数")]
        public Int32 Total2 { get; set; } = -1;

        [DisplayName("备注")]
        public String Description { get; set; }
        //需不需要配置
        public Image NeedManual { get; set; }
        public bool IsNeedManual { get; set; }
        //为了区别同步时，分表造成混乱
        public string RealName { get; set; }
        public string MasterName { get; set; }//主表名称，一般子表才会有的属性
    }
}
