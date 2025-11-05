namespace Prana.Tools
{
    partial class SecurityMasterManagement
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlSMBatchSetup1 = new Prana.Tools.ctrlSMBatchSetup();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlReportTemplate1 = new Prana.Tools.ctrlReportTemplate();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ctrlSMBatchSetup1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(984, 435);
            // 
            // ctrlSMBatchSetup1
            // 
            this.ctrlSMBatchSetup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlSMBatchSetup1.Location = new System.Drawing.Point(0, 0);
            this.ctrlSMBatchSetup1.Name = "ctrlSMBatchSetup1";
            this.ctrlSMBatchSetup1.SecurityMaster = null;
            this.ctrlSMBatchSetup1.Size = new System.Drawing.Size(984, 435);
            this.ctrlSMBatchSetup1.TabIndex = 1;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(992, 466);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.ctrlReportTemplate1);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(992, 466);
            // 
            // ctrlReportTemplate1
            // 
            this.ctrlReportTemplate1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlReportTemplate1.Location = new System.Drawing.Point(0, 0);
            this.ctrlReportTemplate1.Name = "ctrlReportTemplate1";
            this.ctrlReportTemplate1.Size = new System.Drawing.Size(992, 466);
            this.ctrlReportTemplate1.TabIndex = 0;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(984, 435);
            // 
            // ultraTabControl1
            // 
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl3);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Location = new System.Drawing.Point(4, 27);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(988, 461);
            this.ultraTabControl1.TabIndex = 0;
            ultraTab1.Key = "tabDataBatch";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Setup and Run Data Batch";
            ultraTab2.Key = "tabLookup";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Look up and Validate";
            ultraTab3.Key = "tabReports";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Reports";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3});
            this.ultraTabControl1.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Standard;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _SecurityMasterManagement_UltraFormManager_Dock_Area_Left
            // 
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Left.Name = "_SecurityMasterManagement_UltraFormManager_Dock_Area_Left";
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 461);
            // 
            // _SecurityMasterManagement_UltraFormManager_Dock_Area_Right
            // 
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(992, 27);
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Right.Name = "_SecurityMasterManagement_UltraFormManager_Dock_Area_Right";
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 461);
            // 
            // _SecurityMasterManagement_UltraFormManager_Dock_Area_Top
            // 
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Top.Name = "_SecurityMasterManagement_UltraFormManager_Dock_Area_Top";
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(996, 27);
            // 
            // _SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom
            // 
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 488);
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom.Name = "_SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom";
            this._SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(996, 4);
            // 
            // SecurityMasterManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 492);
            this.Controls.Add(this.ultraTabControl1);
            this.Controls.Add(this._SecurityMasterManagement_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._SecurityMasterManagement_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._SecurityMasterManagement_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom);
            this.Name = "SecurityMasterManagement";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Security Master Management";
            this.Load += new System.EventHandler(this.SecurityMasterManagement_Load);
            this.FormClosing += SecurityMasterManagement_FormClosing;
            this.FormClosed += SecurityMasterManagement_FormClosed;
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

      

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private ctrlReportTemplate ctrlReportTemplate1;
        private ctrlSMBatchSetup ctrlSMBatchSetup1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SecurityMasterManagement_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SecurityMasterManagement_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SecurityMasterManagement_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SecurityMasterManagement_UltraFormManager_Dock_Area_Bottom;

    }
}