namespace Prana.PM.Client.UI
{
    partial class CtrlImportPreferences
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblInfo = new Infragistics.Win.Misc.UltraLabel();
            this.btnBrowseExpReport = new Infragistics.Win.Misc.UltraButton();
            this.txtReportSavePath = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblSaveDir = new Infragistics.Win.Misc.UltraLabel();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtReportSavePath)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox1.Controls.Add(this.lblInfo);
            this.ultraGroupBox1.Controls.Add(this.btnBrowseExpReport);
            this.ultraGroupBox1.Controls.Add(this.txtReportSavePath);
            this.ultraGroupBox1.Controls.Add(this.lblSaveDir);
            this.ultraGroupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox1.Location = new System.Drawing.Point(6, 3);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(473, 232);
            this.ultraGroupBox1.TabIndex = 1;
            this.ultraGroupBox1.Text = "Exception Records";
            // 
            // lblInfo
            // 
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(8, 143);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(416, 43);
            this.lblInfo.TabIndex = 12;
            this.lblInfo.Text = "* Please enter the location for the file to be dumped in case of any error while " +
    "importing.";
            // 
            // btnBrowseExpReport
            // 
            this.btnBrowseExpReport.BackColorInternal = System.Drawing.Color.White;
            this.btnBrowseExpReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.btnBrowseExpReport.Location = new System.Drawing.Point(387, 78);
            this.btnBrowseExpReport.Name = "btnBrowseExpReport";
            this.btnBrowseExpReport.Size = new System.Drawing.Size(76, 22);
            this.btnBrowseExpReport.TabIndex = 11;
            this.btnBrowseExpReport.Text = "Browse";
            this.btnBrowseExpReport.Click += new System.EventHandler(this.btnBrowseExpReport_Click);
            // 
            // txtReportSavePath
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            this.txtReportSavePath.Appearance = appearance1;
            this.txtReportSavePath.BackColor = System.Drawing.Color.White;
            this.txtReportSavePath.Location = new System.Drawing.Point(179, 77);
            this.txtReportSavePath.MaxLength = 50;
            this.txtReportSavePath.Name = "txtReportSavePath";
            this.txtReportSavePath.Size = new System.Drawing.Size(202, 23);
            this.txtReportSavePath.TabIndex = 10;
            this.txtReportSavePath.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // lblSaveDir
            // 
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.lblSaveDir.Appearance = appearance2;
            this.lblSaveDir.Location = new System.Drawing.Point(8, 74);
            this.lblSaveDir.Name = "lblSaveDir";
            this.lblSaveDir.Size = new System.Drawing.Size(149, 26);
            this.lblSaveDir.TabIndex = 0;
            this.lblSaveDir.Text = "Directory Location *";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(163, 244);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(158, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save Preferences";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 270);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(485, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.statusStrip1);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraGroupBox1);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnSave);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(485, 292);
            this.ultraPanel1.TabIndex = 13;
            // 
            // CtrlImportPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "CtrlImportPreferences";
            this.Size = new System.Drawing.Size(485, 292);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.CtrlImportPreferences_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtReportSavePath)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraButton btnBrowseExpReport;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtReportSavePath;
        private Infragistics.Win.Misc.UltraLabel lblSaveDir;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private Infragistics.Win.Misc.UltraLabel lblInfo;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;

    }
}
