namespace NewLife.SyncDB
{
    partial class FrmTableConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.cbbPrimaryKey = new Sunny.UI.UIComboBox();
            this.cbbIdentity = new Sunny.UI.UIComboBox();
            this.clbOrderKeys = new System.Windows.Forms.CheckedListBox();
            this.btnSave = new Sunny.UI.UISymbolButton();
            this.SuspendLayout();
            // 
            // uiLabel1
            // 
            this.uiLabel1.AutoSize = true;
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(5, 60);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(69, 24);
            this.uiLabel1.TabIndex = 35;
            this.uiLabel1.Text = "主 键：";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel2
            // 
            this.uiLabel2.AutoSize = true;
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(5, 95);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(69, 24);
            this.uiLabel2.TabIndex = 36;
            this.uiLabel2.Text = "自 增：";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel3
            // 
            this.uiLabel3.AutoSize = true;
            this.uiLabel3.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel3.Location = new System.Drawing.Point(5, 135);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(69, 24);
            this.uiLabel3.TabIndex = 37;
            this.uiLabel3.Text = "排 序：";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbbPrimaryKey
            // 
            this.cbbPrimaryKey.DataSource = null;
            this.cbbPrimaryKey.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cbbPrimaryKey.DropDownWidth = 300;
            this.cbbPrimaryKey.FillColor = System.Drawing.Color.White;
            this.cbbPrimaryKey.FilterMaxCount = 50;
            this.cbbPrimaryKey.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbbPrimaryKey.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cbbPrimaryKey.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cbbPrimaryKey.Location = new System.Drawing.Point(64, 57);
            this.cbbPrimaryKey.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbbPrimaryKey.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbbPrimaryKey.Name = "cbbPrimaryKey";
            this.cbbPrimaryKey.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbbPrimaryKey.ShowClearButton = true;
            this.cbbPrimaryKey.Size = new System.Drawing.Size(120, 31);
            this.cbbPrimaryKey.TabIndex = 54;
            this.cbbPrimaryKey.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbbPrimaryKey.Watermark = "必 选";
            // 
            // cbbIdentity
            // 
            this.cbbIdentity.DataSource = null;
            this.cbbIdentity.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cbbIdentity.DropDownWidth = 300;
            this.cbbIdentity.FillColor = System.Drawing.Color.White;
            this.cbbIdentity.FilterMaxCount = 50;
            this.cbbIdentity.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbbIdentity.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cbbIdentity.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cbbIdentity.Location = new System.Drawing.Point(64, 97);
            this.cbbIdentity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbbIdentity.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbbIdentity.Name = "cbbIdentity";
            this.cbbIdentity.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbbIdentity.ShowClearButton = true;
            this.cbbIdentity.Size = new System.Drawing.Size(120, 29);
            this.cbbIdentity.TabIndex = 55;
            this.cbbIdentity.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbbIdentity.Watermark = "可 选";
            // 
            // clbOrderKeys
            // 
            this.clbOrderKeys.Font = new System.Drawing.Font("微软雅黑", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clbOrderKeys.FormattingEnabled = true;
            this.clbOrderKeys.Location = new System.Drawing.Point(64, 138);
            this.clbOrderKeys.Name = "clbOrderKeys";
            this.clbOrderKeys.Size = new System.Drawing.Size(120, 84);
            this.clbOrderKeys.TabIndex = 56;
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("宋体", 12F);
            this.btnSave.Location = new System.Drawing.Point(123, 239);
            this.btnSave.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(61, 23);
            this.btnSave.Style = Sunny.UI.UIStyle.Custom;
            this.btnSave.StyleCustomMode = true;
            this.btnSave.TabIndex = 80;
            this.btnSave.Text = "OK";
            this.btnSave.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FrmTableConfig
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(217, 281);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.clbOrderKeys);
            this.Controls.Add(this.cbbIdentity);
            this.Controls.Add(this.cbbPrimaryKey);
            this.Controls.Add(this.uiLabel3);
            this.Controls.Add(this.uiLabel2);
            this.Controls.Add(this.uiLabel1);
            this.Font = new System.Drawing.Font("微软雅黑", 6.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTableConfig";
            this.Padding = new System.Windows.Forms.Padding(2, 29, 2, 2);
            this.ShowDragStretch = true;
            this.ShowRadius = true;
            this.ShowShadow = false;
            this.Text = "同步表配置";
            this.TitleFont = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TitleHeight = 29;
            this.ZoomScaleRect = new System.Drawing.Rectangle(19, 19, 800, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UIComboBox cbbPrimaryKey;
        private Sunny.UI.UIComboBox cbbIdentity;
        private System.Windows.Forms.CheckedListBox clbOrderKeys;
        private Sunny.UI.UISymbolButton btnSave;
    }
}