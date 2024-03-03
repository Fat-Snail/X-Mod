namespace NewLife.SyncDB
{
    partial class FrmSubTable
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
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.txtTableName = new Sunny.UI.UITextBox();
            this.txtSubRegex = new Sunny.UI.UITextBox();
            this.saveBtn = new Sunny.UI.UIButton();
            this.cancelBtn = new Sunny.UI.UIButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // uiLabel2
            // 
            this.uiLabel2.AutoSize = true;
            this.uiLabel2.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(3, 83);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(100, 24);
            this.uiLabel2.TabIndex = 38;
            this.uiLabel2.Text = "分表规则：";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel1
            // 
            this.uiLabel1.AutoSize = true;
            this.uiLabel1.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(34, 48);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(69, 24);
            this.uiLabel1.TabIndex = 37;
            this.uiLabel1.Text = "表 名：";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTableName
            // 
            this.txtTableName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTableName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTableName.Location = new System.Drawing.Point(95, 48);
            this.txtTableName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTableName.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Padding = new System.Windows.Forms.Padding(5);
            this.txtTableName.ShowText = false;
            this.txtTableName.Size = new System.Drawing.Size(201, 30);
            this.txtTableName.TabIndex = 39;
            this.txtTableName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtTableName.Watermark = "";
            // 
            // txtSubRegex
            // 
            this.txtSubRegex.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSubRegex.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSubRegex.Location = new System.Drawing.Point(95, 83);
            this.txtSubRegex.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSubRegex.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtSubRegex.Name = "txtSubRegex";
            this.txtSubRegex.Padding = new System.Windows.Forms.Padding(5);
            this.txtSubRegex.ShowText = false;
            this.txtSubRegex.Size = new System.Drawing.Size(201, 30);
            this.txtSubRegex.TabIndex = 40;
            this.txtSubRegex.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSubRegex.Watermark = "";
            // 
            // saveBtn
            // 
            this.saveBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.saveBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.saveBtn.Location = new System.Drawing.Point(219, 187);
            this.saveBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(100, 28);
            this.saveBtn.TabIndex = 42;
            this.saveBtn.Text = "保存";
            this.saveBtn.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cancelBtn.Location = new System.Drawing.Point(95, 187);
            this.cancelBtn.MinimumSize = new System.Drawing.Size(1, 1);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(100, 28);
            this.cancelBtn.TabIndex = 41;
            this.cancelBtn.Text = "取消";
            this.cancelBtn.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(100, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 57);
            this.label1.TabIndex = 43;
            this.label1.Text = "一般分表是以表名加数字，譬如User1^N,所以正则一般为：(?<name>\\w+)(?<i>\\d+)";
            // 
            // FrmSubTable
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(358, 239);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.txtSubRegex);
            this.Controls.Add(this.txtTableName);
            this.Controls.Add(this.uiLabel2);
            this.Controls.Add(this.uiLabel1);
            this.Name = "FrmSubTable";
            this.Text = "分表";
            this.ZoomScaleRect = new System.Drawing.Rectangle(19, 19, 800, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UITextBox txtTableName;
        private Sunny.UI.UITextBox txtSubRegex;
        private Sunny.UI.UIButton saveBtn;
        private Sunny.UI.UIButton cancelBtn;
        private System.Windows.Forms.Label label1;
    }
}