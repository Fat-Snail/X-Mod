using NewLife.SyncDB.Entity;
using NewLife;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewLife.SyncDB
{
    public partial class FrmSubTable : UIForm
    {
        private TableModel _model = null;
        private Action _action= null;
        public FrmSubTable(Action reflashAct, TableModel model)
        {
            InitializeComponent();
            _model = model;

            var tables = SyncSetting.Current.Tables.FindAll(x => x.IsSubTable);

            var tableName = model.RealName;
            var subRegex = string.Empty;
            foreach (var table in tables)
            {
                if (Regex.IsMatch(tableName, table.SubRegex))
                {
                    tableName = table.Name;
                    subRegex = table.SubRegex;
                    break;
                }
            }

            if (!tableName.IsNullOrEmpty())
            {
                txtTableName.Text = tableName;
            }

            if (!subRegex.IsNullOrEmpty())
            {
                txtSubRegex.Text = subRegex;
            }
            else
            {
                txtSubRegex.Text = @"(?<name>User)(?<i>\d+)";
            }

            this._action = reflashAct;

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();



        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (txtTableName.Text.IsNullOrEmpty())
            {
                UIMessageBox.ShowWarning("表名不能为空");
                return;
            }

            if (txtSubRegex.Text.IsNullOrEmpty())
            {
                UIMessageBox.ShowWarning("规则不能为空");
                return;
            }

            var table = SyncSetting.Current.Tables.Find(x => x.Name == txtTableName.Text);

            if (table == null)
            {
                table = new SyncSetting.TableSet { Name = txtTableName.Text };
                SyncSetting.Current.Tables.Add(table);
            }
            table.IsSubTable = true;
            table.SubRegex=txtSubRegex.Text;

            SyncSetting.Current.Save();

            if (this._action != null)
            {
                this._action.Invoke();
            }

            UIMessageBox.ShowWarning("保存成功！");
            this.Close();
        }
    }
}
