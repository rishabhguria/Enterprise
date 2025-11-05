namespace Prana.Import.PL
{
    partial class frmImport
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (bgRunBatch != null)
                {
                    bgRunBatch.Dispose();
                }
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cntrlBatchSetup1 = new Prana.Admin.CommonControls.CntrlBatchSetup();
            this.btnRunBatch = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.tbReports = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.frmImport_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.tbImport = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._frmImport_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmImport_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmImport_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmImport_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ctrlImportDashboard1 = new Prana.Import.Controls.CtrlImportDashboard();
            this.ctrlHistoricalUpload1 = new Prana.Import.Controls.CtrlHistoricalUpload();
            this.ctrlArchivedData1 = new Prana.Import.Controls.CtrlArchivedData();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.tbReports.SuspendLayout();
            this.ultraTabPageControl5.SuspendLayout();
            this.frmImport_Fill_Panel.ClientArea.SuspendLayout();
            this.frmImport_Fill_Panel.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbImport)).BeginInit();
            this.tbImport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.splitContainer1);
            this.ultraTabPageControl1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(880, 485);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cntrlBatchSetup1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnRunBatch);
            this.splitContainer1.Size = new System.Drawing.Size(880, 485);
            this.splitContainer1.SplitterDistance = 433;
            this.splitContainer1.TabIndex = 0;
            // 
            // cntrlBatchSetup1
            // 
            this.cntrlBatchSetup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cntrlBatchSetup1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cntrlBatchSetup1.Location = new System.Drawing.Point(0, 0);
            this.cntrlBatchSetup1.Margin = new System.Windows.Forms.Padding(4);
            this.cntrlBatchSetup1.Name = "cntrlBatchSetup1";
            this.cntrlBatchSetup1.Size = new System.Drawing.Size(880, 433);
            this.cntrlBatchSetup1.TabIndex = 0;
            // 
            // btnRunBatch
            // 
            this.btnRunBatch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRunBatch.Location = new System.Drawing.Point(350, 13);
            this.btnRunBatch.MaximumSize = new System.Drawing.Size(114, 23);
            this.btnRunBatch.Name = "btnRunBatch";
            this.btnRunBatch.Size = new System.Drawing.Size(114, 23);
            this.btnRunBatch.TabIndex = 0;
            this.btnRunBatch.Text = "Run Batch";
            this.btnRunBatch.Click += new System.EventHandler(this.btnRunBatch_Click);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.ctrlImportDashboard1);
            this.ultraTabPageControl2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(880, 485);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.ultraTabControl1);
            this.ultraTabPageControl3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(880, 485);
            // 
            // ultraTabControl1
            // 
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage2);
            this.ultraTabControl1.Controls.Add(this.tbReports);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl5);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage2;
            this.ultraTabControl1.Size = new System.Drawing.Size(880, 485);
            this.ultraTabControl1.TabIndex = 0;
            ultraTab4.TabPage = this.tbReports;
            ultraTab4.Text = "View Historical Uploads";
            ultraTab5.TabPage = this.ultraTabPageControl5;
            ultraTab5.Text = "View Archived Files";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab4,
            ultraTab5});
            // 
            // ultraTabSharedControlsPage2
            // 
            this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
            this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(876, 459);
            // 
            // tbReports
            // 
            this.tbReports.Controls.Add(this.ctrlHistoricalUpload1);
            this.tbReports.Location = new System.Drawing.Point(1, 23);
            this.tbReports.Name = "tbReports";
            this.tbReports.Size = new System.Drawing.Size(876, 459);
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Controls.Add(this.ctrlArchivedData1);
            this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(876, 459);
            // 
            // frmImport_Fill_Panel
            // 
            // 
            // frmImport_Fill_Panel.ClientArea
            // 
            this.frmImport_Fill_Panel.ClientArea.Controls.Add(this.ultraPanel1);
            this.frmImport_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.frmImport_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frmImport_Fill_Panel.Location = new System.Drawing.Point(8, 31);
            this.frmImport_Fill_Panel.Name = "frmImport_Fill_Panel";
            this.frmImport_Fill_Panel.Size = new System.Drawing.Size(884, 511);
            this.frmImport_Fill_Panel.TabIndex = 0;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.tbImport);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(884, 511);
            this.ultraPanel1.TabIndex = 0;
            // 
            // tbImport
            // 
            this.tbImport.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tbImport.Controls.Add(this.ultraTabPageControl1);
            this.tbImport.Controls.Add(this.ultraTabPageControl2);
            this.tbImport.Controls.Add(this.ultraTabPageControl3);
            this.tbImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbImport.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.tbImport.Location = new System.Drawing.Point(0, 0);
            this.tbImport.Name = "tbImport";
            this.tbImport.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tbImport.Size = new System.Drawing.Size(884, 511);
            this.tbImport.TabIndex = 0;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Setup and Run Batch";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Upload and Import";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Reports";
            this.tbImport.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(880, 485);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _frmImport_UltraFormManager_Dock_Area_Left
            // 
            this._frmImport_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmImport_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmImport_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._frmImport_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmImport_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._frmImport_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._frmImport_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._frmImport_UltraFormManager_Dock_Area_Left.Name = "_frmImport_UltraFormManager_Dock_Area_Left";
            this._frmImport_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 511);
            // 
            // _frmImport_UltraFormManager_Dock_Area_Right
            // 
            this._frmImport_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmImport_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmImport_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._frmImport_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmImport_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._frmImport_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._frmImport_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(892, 31);
            this._frmImport_UltraFormManager_Dock_Area_Right.Name = "_frmImport_UltraFormManager_Dock_Area_Right";
            this._frmImport_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 511);
            // 
            // _frmImport_UltraFormManager_Dock_Area_Top
            // 
            this._frmImport_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmImport_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmImport_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._frmImport_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmImport_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._frmImport_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmImport_UltraFormManager_Dock_Area_Top.Name = "_frmImport_UltraFormManager_Dock_Area_Top";
            this._frmImport_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(900, 31);
            // 
            // _frmImport_UltraFormManager_Dock_Area_Bottom
            // 
            this._frmImport_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmImport_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmImport_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._frmImport_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmImport_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._frmImport_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._frmImport_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 542);
            this._frmImport_UltraFormManager_Dock_Area_Bottom.Name = "_frmImport_UltraFormManager_Dock_Area_Bottom";
            this._frmImport_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(900, 8);
            // 
            // ctrlImportDashboard1
            // 
            this.ctrlImportDashboard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlImportDashboard1.Location = new System.Drawing.Point(0, 0);
            this.ctrlImportDashboard1.Name = "ctrlImportDashboard1";
            this.ctrlImportDashboard1.Size = new System.Drawing.Size(880, 485);
            this.ctrlImportDashboard1.TabIndex = 0;
            // 
            // ctrlHistoricalUpload1
            // 
            this.ctrlHistoricalUpload1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlHistoricalUpload1.Location = new System.Drawing.Point(0, 0);
            this.ctrlHistoricalUpload1.Name = "ctrlHistoricalUpload1";
            this.ctrlHistoricalUpload1.Size = new System.Drawing.Size(876, 459);
            this.ctrlHistoricalUpload1.TabIndex = 0;
            // 
            // ctrlArchivedData1
            // 
            this.ctrlArchivedData1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlArchivedData1.Location = new System.Drawing.Point(0, 0);
            this.ctrlArchivedData1.Name = "ctrlArchivedData1";
            this.ctrlArchivedData1.Size = new System.Drawing.Size(876, 459);
            this.ctrlArchivedData1.TabIndex = 0;
            // 
            // frmImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 550);
            this.Controls.Add(this.frmImport_Fill_Panel);
            this.Controls.Add(this._frmImport_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._frmImport_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._frmImport_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._frmImport_UltraFormManager_Dock_Area_Bottom);
            this.MinimumSize = new System.Drawing.Size(900, 550);
            this.Name = "frmImport";
            this.ShowIcon = false;
            this.Text = "Import Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportData_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImportData_FormClosed);
            this.Load += new System.EventHandler(this.frmImport_Load);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.tbReports.ResumeLayout(false);
            this.ultraTabPageControl5.ResumeLayout(false);
            this.frmImport_Fill_Panel.ClientArea.ResumeLayout(false);
            this.frmImport_Fill_Panel.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbImport)).EndInit();
            this.tbImport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel frmImport_Fill_Panel;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbImport;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Prana.Admin.CommonControls.CntrlBatchSetup cntrlBatchSetup1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbReports;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
        private Infragistics.Win.Misc.UltraButton btnRunBatch;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Controls.CtrlImportDashboard ctrlImportDashboard1;
        private Controls.CtrlHistoricalUpload ctrlHistoricalUpload1;
        private Prana.Import.Controls.CtrlArchivedData ctrlArchivedData1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmImport_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmImport_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmImport_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmImport_UltraFormManager_Dock_Area_Bottom;
    }
}