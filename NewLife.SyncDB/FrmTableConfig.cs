using NewLife;
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
    public partial class FrmTableConfig : Sunny.UI.UIForm
    {
        private IDataTable _table = null;

        public FrmTableConfig(IDataTable table)
        {
            InitializeComponent();

            _table = table;
            this.Text = $"[{table.Name}]表配置";

            var tableCnf = SyncSetting.Current.Tables.Find(x => x.Name == table.Name);

            cbbPrimaryKey.Items.AddRange(table.Columns.Select(x => x.Name).ToArray());

            if (tableCnf != null && !tableCnf.PrimaryKey.IsNullOrEmpty())
            {
                cbbPrimaryKey.SelectedItem = tableCnf.PrimaryKey;
            }
            else
            {
                var pk = table.Columns.FirstOrDefault(e => e.PrimaryKey);
                if (pk != null)
                {
                    cbbPrimaryKey.SelectedItem = pk.Name;
                }
            }

            cbbIdentity.Items.AddRange(table.Columns.Select(x => x.Name).ToArray());
            if (tableCnf != null && !tableCnf.IdentityKey.IsNullOrEmpty())
            {
                cbbIdentity.SelectedItem = tableCnf.IdentityKey;
            }
            else
            {
                var id = table.Columns.FirstOrDefault(e => e.Identity);
                if (id != null)
                {
                    cbbIdentity.SelectedItem = id.Name;
                }
            }

            var orderKeys = table.Columns.Where(w => w.Name.ToLower().Contains("time")).Select(x => x.Name).ToList();
            if (orderKeys.Count > 0)
            {
                foreach (var key in orderKeys)
                {
                    clbOrderKeys.Items.Add(key);
                }
                //clbOrderKeys.Items.Add(orderKeys);
            }

            if (tableCnf != null && !tableCnf.OrderColumns.IsNullOrEmpty())
            {
                for (int i = 0; i < clbOrderKeys.Items.Count; i++)
                {
                    if (tableCnf.OrderColumns.Contains(clbOrderKeys.Items[i].ToString()))
                    {
                        clbOrderKeys.SetItemChecked(i, true);
                    }
                }

                //foreach (var key in tableCnf.OrderColumns.Split(','))
                //{
                //    clbOrderKeys.SetItemChecked()
                //}
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbbPrimaryKey.SelectedItem == null || cbbPrimaryKey.SelectedItem.ToString().IsNullOrEmpty())
            {
                Sunny.UI.UIMessageTip.ShowWarning("主键不能为空", 2000);
                return;
            }

            var tableCnf = SyncSetting.Current.Tables.Find(x => x.Name == _table.Name);
            if (tableCnf == null)
            {
                tableCnf = new SyncSetting.TableSet();
                SyncSetting.Current.Tables.Add(tableCnf);
            }

            tableCnf.Name = _table.Name;
            tableCnf.PrimaryKey = cbbPrimaryKey.SelectedItem.ToString();
            if (cbbIdentity.SelectedItem != null)
            {
                tableCnf.IdentityKey = cbbIdentity.SelectedItem.ToString();
            }

            if (clbOrderKeys.CheckedItems.Count > 0)
            {
                var chkList = new List<string>();

                foreach (var item in clbOrderKeys.CheckedItems)
                {
                    chkList.Add(item.ToString());
                }

                if (chkList.Count > 0)
                {
                    tableCnf.OrderColumns = string.Join(",", chkList);
                }
            }

            SyncSetting.Current.Save();

            Sunny.UI.UIMessageTip.ShowOk("保存成功！", 2000);

            this.Close();

        }
    }
}
