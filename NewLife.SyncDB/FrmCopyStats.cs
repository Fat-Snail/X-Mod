using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewLife.SyncDB
{
    public partial class FrmCopyStats : UIForm
    {
        public FrmCopyStats(int totalTable,int totalMins,int totalRow,int rowPerMin)
        {
            InitializeComponent();

            this.labTotalTable.Text = totalTable.ToString("N0");
            this.labTotalRow.Text = totalRow.ToString("N0");
            this.labTotalMins.Text = totalMins.ToString("N0");
            this.labPerMin.Text=rowPerMin.ToString("N0");

        }
    }
}
