namespace Prana.ReportingServer
{
    partial class ReportingServer
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
            this.btnStartReportingServer = new System.Windows.Forms.Button();
            this.btnClearMessageBox = new System.Windows.Forms.Button();
            this.lstbxErrorLog = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.udtStructureDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.cmbxClientSettingNames = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAdmin = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.udtStructureDate)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartReportingServer
            // 
            this.btnStartReportingServer.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnStartReportingServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartReportingServer.Location = new System.Drawing.Point(132, 234);
            this.btnStartReportingServer.Name = "btnStartReportingServer";
            this.btnStartReportingServer.Size = new System.Drawing.Size(94, 23);
            this.btnStartReportingServer.TabIndex = 0;
            this.btnStartReportingServer.Text = "Start Server";
            this.btnStartReportingServer.UseVisualStyleBackColor = true;
            this.btnStartReportingServer.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnClearMessageBox
            // 
            this.btnClearMessageBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClearMessageBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearMessageBox.Location = new System.Drawing.Point(132, 263);
            this.btnClearMessageBox.Name = "btnClearMessageBox";
            this.btnClearMessageBox.Size = new System.Drawing.Size(94, 23);
            this.btnClearMessageBox.TabIndex = 4;
            this.btnClearMessageBox.Text = "Clear Logging Window";
            this.btnClearMessageBox.Click += new System.EventHandler(this.btnClearMessageBox_Click);
            // 
            // lstbxErrorLog
            // 
            this.lstbxErrorLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstbxErrorLog.ForeColor = System.Drawing.Color.Red;
            this.lstbxErrorLog.HorizontalScrollbar = true;
            this.lstbxErrorLog.Items.AddRange(new object[] {
            ""});
            this.lstbxErrorLog.Location = new System.Drawing.Point(12, 40);
            this.lstbxErrorLog.Name = "lstbxErrorLog";
            this.lstbxErrorLog.Size = new System.Drawing.Size(558, 173);
            this.lstbxErrorLog.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(270, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Logs";
            // 
            // btnCalculate
            // 
            this.btnCalculate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCalculate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalculate.Location = new System.Drawing.Point(379, 263);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(102, 23);
            this.btnCalculate.TabIndex = 6;
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // udtStructureDate
            // 
            this.udtStructureDate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.udtStructureDate.Location = new System.Drawing.Point(231, 234);
            this.udtStructureDate.Name = "udtStructureDate";
            this.udtStructureDate.Size = new System.Drawing.Size(142, 21);
            this.udtStructureDate.TabIndex = 1;
            // 
            // cmbxClientSettingNames
            // 
            this.cmbxClientSettingNames.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmbxClientSettingNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxClientSettingNames.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbxClientSettingNames.FormattingEnabled = true;
            this.cmbxClientSettingNames.Location = new System.Drawing.Point(231, 263);
            this.cmbxClientSettingNames.Name = "cmbxClientSettingNames";
            this.cmbxClientSettingNames.Size = new System.Drawing.Size(142, 22);
            this.cmbxClientSettingNames.TabIndex = 5;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(379, 233);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(102, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnAdmin
            // 
            this.btnAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdmin.Location = new System.Drawing.Point(467, 9);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(102, 23);
            this.btnAdmin.TabIndex = 118;
            this.btnAdmin.Text = "Admin";
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);
            // 
            // ReportingServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 311);
            this.Controls.Add(this.btnAdmin);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.cmbxClientSettingNames);
            this.Controls.Add(this.udtStructureDate);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstbxErrorLog);
            this.Controls.Add(this.btnClearMessageBox);
            this.Controls.Add(this.btnStartReportingServer);
            this.Name = "ReportingServer";
            this.Text = "Reporting Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReportingServer_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.udtStructureDate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartReportingServer;
       // private System.Windows.Forms.Button  btnOpenLog;
        private System.Windows.Forms.Button btnClearMessageBox;
        private System.Windows.Forms.ListBox lstbxErrorLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCalculate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor udtStructureDate;
        private System.Windows.Forms.ComboBox cmbxClientSettingNames;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAdmin;
    }
}

