using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCode.DataAccessLayer;

namespace NewLife.SyncDB
{
    public partial class FrmConfig : Sunny.UI.UIForm
    {
        public FrmConfig()
        {
            InitializeComponent();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            bool conChanage=false;

            SyncSetting.Current.ServiceName = txtServiceName.Text;
            if (SyncSetting.Current.MainConn != txtMainConn.Text)
            {
                SyncSetting.Current.MainConn = txtMainConn.Text;
                conChanage = true;
            }

            if (SyncSetting.Current.BakConn != txtBakConn.Text)
            {
                SyncSetting.Current.BakConn = txtBakConn.Text;
                conChanage = true;
            }
            
            SyncSetting.Current.ThreadCount = txtThreadCount.Text.ToInt();

            if (conChanage)
            {
                DAL.AddConnStr("BakDB", SyncSetting.Current.MainConn, null, null);
                DAL.AddConnStr("ToBakDB", SyncSetting.Current.BakConn, null, null);
            }

            SyncSetting.Current.Save();
            Hide();
        }

        private void ConfigForm_Shown(object sender, EventArgs e)
        {
            this.txtServiceName.Text = SyncSetting.Current.ServiceName;
            this.txtMainConn.Text = SyncSetting.Current.MainConn;
            this.txtThreadCount.Text = SyncSetting.Current.ThreadCount.ToString();
            this.txtBakConn.Text = SyncSetting.Current.BakConn;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
