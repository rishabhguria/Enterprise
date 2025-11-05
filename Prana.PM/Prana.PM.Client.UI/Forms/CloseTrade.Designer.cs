using Prana.BusinessObjects.Constants;
using Prana.Global;
namespace Prana.PM.Client.UI.Forms
{
    partial class CloseTrade
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
                //_proxy.InnerChannel.re
                if (components != null)
                {
                    components.Dispose();
                }
                if (_allocationServices != null)
                {
                    _allocationServices.Dispose();
                }
                if (_proxy != null)
                {
                    _proxy.Dispose();
                }
                if (_closingServices != null)
                {
                    _closingServices.Dispose();
                }
                if (mainStatusBar != null)
                {
                    mainStatusBar.Dispose();
                }
                if (statusPanel != null)
                {
                    statusPanel.Dispose();
                }
                if (datetimePanel != null)
                {
                    datetimePanel.Dispose();
                }
                if (ProgressTimer != null)
                {
                    ProgressTimer.Dispose();
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
            this.ControlBox = true;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.closeTradeWithSplitter1 = new Prana.PM.Client.UI.Controls.CtrlCloseTrade();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlExpiryandSettlementNew1 = new Prana.PM.Client.UI.Controls.ctrlExpiryandSettlementNew();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlCloseTradefromAllocation1 = new Prana.PM.Client.UI.Controls.ctrlCloseTradefromAllocation();
            this.Form1_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnRefresh = new Infragistics.Win.Misc.UltraButton();
            this.btnAdvanceOptions = new Infragistics.Win.Misc.UltraButton();
            this.dtToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.rbHistorical = new System.Windows.Forms.RadioButton();
            this.rbCurrent = new System.Windows.Forms.RadioButton();
            this.lblAccountFilter = new Infragistics.Win.Misc.UltraLabel();
            this.MultiSelectDropDown1 = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this.tabCloseTradeMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._CloseTrade_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CloseTrade_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CloseTrade_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CloseTrade_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            this.Form1_Fill_Panel.ClientArea.SuspendLayout();
            this.Form1_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabCloseTradeMain)).BeginInit();
            this.tabCloseTradeMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.closeTradeWithSplitter1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1098, 511);
            // 
            // closeTradeWithSplitter1
            // 
            this.closeTradeWithSplitter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.closeTradeWithSplitter1.Location = new System.Drawing.Point(0, 0);
            this.closeTradeWithSplitter1.Name = "closeTradeWithSplitter1";
            this.closeTradeWithSplitter1.Size = new System.Drawing.Size(1098, 511);
            this.closeTradeWithSplitter1.TabIndex = 0;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.ctrlExpiryandSettlementNew1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(1098, 511);
            // 
            // ctrlExpiryandSettlementNew1
            // 
            this.ctrlExpiryandSettlementNew1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlExpiryandSettlementNew1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlExpiryandSettlementNew1.Location = new System.Drawing.Point(0, 0);
            this.ctrlExpiryandSettlementNew1.Name = "ctrlExpiryandSettlementNew1";
            this.ctrlExpiryandSettlementNew1.Size = new System.Drawing.Size(1098, 511);
            this.ctrlExpiryandSettlementNew1.TabIndex = 0;
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.ctrlCloseTradefromAllocation1);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(1098, 511);
            // 
            // ctrlCloseTradefromAllocation1
            // 
            this.ctrlCloseTradefromAllocation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCloseTradefromAllocation1.Location = new System.Drawing.Point(0, 0);
            this.ctrlCloseTradefromAllocation1.Name = "ctrlCloseTradefromAllocation1";
            this.ctrlCloseTradefromAllocation1.Size = new System.Drawing.Size(1098, 511);
            this.ctrlCloseTradefromAllocation1.TabIndex = 0;
            // 
            // Form1_Fill_Panel
            // 
            // 
            // Form1_Fill_Panel.ClientArea
            // 
            this.Form1_Fill_Panel.ClientArea.Controls.Add(this.splitContainer1);
            this.Form1_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.Form1_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Form1_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.Form1_Fill_Panel.Name = "Form1_Fill_Panel";
            this.Form1_Fill_Panel.Size = new System.Drawing.Size(1102, 579);
            this.Form1_Fill_Panel.TabIndex = 0;
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
            this.splitContainer1.Panel1.Controls.Add(this.ultraLabel2);
            this.splitContainer1.Panel1.Controls.Add(this.ultraLabel1);
            this.splitContainer1.Panel1.Controls.Add(this.btnRefresh);
            this.splitContainer1.Panel1.Controls.Add(this.btnAdvanceOptions);
            this.splitContainer1.Panel1.Controls.Add(this.dtToDate);
            this.splitContainer1.Panel1.Controls.Add(this.dtFromDate);
            this.splitContainer1.Panel1.Controls.Add(this.rbHistorical);
            this.splitContainer1.Panel1.Controls.Add(this.rbCurrent);
            this.splitContainer1.Panel1.Controls.Add(this.lblAccountFilter);
            this.splitContainer1.Panel1.Controls.Add(this.MultiSelectDropDown1);
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1.Panel1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabCloseTradeMain);
            this.splitContainer1.Size = new System.Drawing.Size(1102, 579);
            this.splitContainer1.SplitterDistance = 38;
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer1.TabIndex = 2;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Location = new System.Drawing.Point(147, 12);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(40, 23);
            this.ultraLabel2.TabIndex = 11;
            this.ultraLabel2.Text = "From:";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(297, 12);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(29, 23);
            this.ultraLabel1.TabIndex = 12;
            this.ultraLabel1.Text = "To:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(666, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 24);
            this.btnRefresh.TabIndex = 13;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnAdvanceOptions
            // 
            this.btnAdvanceOptions.Location = new System.Drawing.Point(750, 10);
            this.btnAdvanceOptions.Name = "btnAdvanceOptions";
            this.btnAdvanceOptions.Size = new System.Drawing.Size(153, 24);
            this.btnAdvanceOptions.TabIndex = 14;
            this.btnAdvanceOptions.Text = "Advance Options";
            this.btnAdvanceOptions.Click += new System.EventHandler(this.btnAdvanceOptions_Click);
            // 
            // dtToDate
            // 
            this.dtToDate.DateTime = new System.DateTime(2015, 1, 4, 0, 0, 0, 0);
            this.dtToDate.Location = new System.Drawing.Point(327, 10);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(100, 21);
            this.dtToDate.TabIndex = 3;
            this.dtToDate.Value = new System.DateTime(2015, 1, 4, 0, 0, 0, 0);
            this.dtToDate.ValueChanged += new System.EventHandler(this.dtToDate_ValueChanged);
            // 
            // dtFromDate
            // 
            this.dtFromDate.DateTime = new System.DateTime(2015, 1, 4, 0, 0, 0, 0);
            this.dtFromDate.Location = new System.Drawing.Point(187, 10);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(100, 21);
            this.dtFromDate.TabIndex = 2;
            this.dtFromDate.Value = new System.DateTime(2015, 1, 4, 0, 0, 0, 0);
            this.dtFromDate.ValueChanged += new System.EventHandler(this.dtFromDate_ValueChanged);
            // 
            // rbHistorical
            // 
            this.rbHistorical.AutoSize = true;
            this.rbHistorical.Location = new System.Drawing.Point(75, 10);
            this.rbHistorical.Name = "rbHistorical";
            this.rbHistorical.Size = new System.Drawing.Size(68, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbHistorical, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbHistorical.TabIndex = 1;
            this.rbHistorical.Text = "Historical";
            this.rbHistorical.UseVisualStyleBackColor = true;
            this.rbHistorical.CheckedChanged += new System.EventHandler(this.rbHistorical_CheckedChanged);
            // 
            // rbCurrent
            // 
            this.rbCurrent.AutoSize = true;
            this.rbCurrent.Checked = true;
            this.rbCurrent.Location = new System.Drawing.Point(8, 10);
            this.rbCurrent.Name = "rbCurrent";
            this.rbCurrent.Size = new System.Drawing.Size(59, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rbCurrent, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rbCurrent.TabIndex = 0;
            this.rbCurrent.TabStop = true;
            this.rbCurrent.Text = "Current";
            this.rbCurrent.UseVisualStyleBackColor = true;
            this.rbCurrent.CheckedChanged += new System.EventHandler(this.rbCurrent_CheckedChanged);
            // 
            // lblAccountFilter
            // 
            this.lblAccountFilter.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblAccountFilter.Location = new System.Drawing.Point(430, 12);
            this.lblAccountFilter.Name = "lblAccountFilter";
            this.lblAccountFilter.Size = new System.Drawing.Size(86, 23);
            this.lblAccountFilter.TabIndex = 15;
            this.lblAccountFilter.Text = "Account Filter: ";
            // 
            // MultiSelectDropDown1
            // 
            this.MultiSelectDropDown1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.MultiSelectDropDown1.Location = new System.Drawing.Point(514, 10);
            this.MultiSelectDropDown1.Name = "MultiSelectDropDown1";
            this.MultiSelectDropDown1.Size = new System.Drawing.Size(150, 21);
            this.MultiSelectDropDown1.TabIndex = 0;
            this.MultiSelectDropDown1.TitleText = "";
            // 
            // tabCloseTradeMain
            // 
            this.tabCloseTradeMain.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabCloseTradeMain.Controls.Add(this.ultraTabPageControl1);
            this.tabCloseTradeMain.Controls.Add(this.ultraTabPageControl2);
            this.tabCloseTradeMain.Controls.Add(this.ultraTabPageControl3);
            this.tabCloseTradeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCloseTradeMain.Location = new System.Drawing.Point(0, 0);
            this.tabCloseTradeMain.Name = "tabCloseTradeMain";
            this.tabCloseTradeMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabCloseTradeMain.Size = new System.Drawing.Size(1102, 537);
            this.tabCloseTradeMain.TabIndex = 0;
            ultraTab1.Key = "ClosedAmend";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Closed Amend";
            ultraTab2.Key = "Expiration/Settlement";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Expiration/Settlement";
            ultraTab3.Key = "CloseOrder";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Close Order";
            this.tabCloseTradeMain.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3});
            this.tabCloseTradeMain.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabCloseTradeMain_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1098, 511);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _CloseTrade_UltraFormManager_Dock_Area_Left
            // 
            this._CloseTrade_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CloseTrade_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CloseTrade_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._CloseTrade_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CloseTrade_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._CloseTrade_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._CloseTrade_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._CloseTrade_UltraFormManager_Dock_Area_Left.Name = "_CloseTrade_UltraFormManager_Dock_Area_Left";
            this._CloseTrade_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 579);
            // 
            // _CloseTrade_UltraFormManager_Dock_Area_Right
            // 
            this._CloseTrade_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CloseTrade_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CloseTrade_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._CloseTrade_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CloseTrade_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._CloseTrade_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._CloseTrade_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1106, 27);
            this._CloseTrade_UltraFormManager_Dock_Area_Right.Name = "_CloseTrade_UltraFormManager_Dock_Area_Right";
            this._CloseTrade_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 579);
            // 
            // _CloseTrade_UltraFormManager_Dock_Area_Top
            // 
            this._CloseTrade_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CloseTrade_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CloseTrade_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._CloseTrade_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CloseTrade_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._CloseTrade_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._CloseTrade_UltraFormManager_Dock_Area_Top.Name = "_CloseTrade_UltraFormManager_Dock_Area_Top";
            this._CloseTrade_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1110, 27);
            // 
            // _CloseTrade_UltraFormManager_Dock_Area_Bottom
            // 
            this._CloseTrade_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CloseTrade_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CloseTrade_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._CloseTrade_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CloseTrade_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._CloseTrade_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._CloseTrade_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 606);
            this._CloseTrade_UltraFormManager_Dock_Area_Bottom.Name = "_CloseTrade_UltraFormManager_Dock_Area_Bottom";
            this._CloseTrade_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1110, 4);
            // 
            // CloseTrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 610);
            this.Controls.Add(this.Form1_Fill_Panel);
            this.Controls.Add(this._CloseTrade_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._CloseTrade_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._CloseTrade_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._CloseTrade_UltraFormManager_Dock_Area_Bottom);
            this.MinimumSize = new System.Drawing.Size(1165, 610);
            this.Name = "CloseTrade";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Close Trade";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CloseTrade_FormClosing);
            this.FormClosed += new System.EventHandler(this.CloseTrade_FormClosed);
            this.Load += new System.EventHandler(this.CloseTrade_Load);
            this.Disposed += new System.EventHandler(this.CloseTrade_Disposed);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            this.Form1_Fill_Panel.ClientArea.ResumeLayout(false);
            this.Form1_Fill_Panel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabCloseTradeMain)).EndInit();
            this.tabCloseTradeMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }
    
        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraButton btnRefresh;
        private Infragistics.Win.Misc.UltraButton btnScreenshot;
        private Infragistics.Win.Misc.UltraButton btnAdvanceOptions;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDate;
        private System.Windows.Forms.RadioButton rbHistorical;
        private System.Windows.Forms.RadioButton rbCurrent;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel lblAccountFilter;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCloseTradeMain;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Prana.PM.Client.UI.Controls.CtrlCloseTrade closeTradeWithSplitter1;
        private Prana.PM.Client.UI.Controls.ctrlExpiryandSettlementNew ctrlExpiryandSettlementNew1;
        private Prana.PM.Client.UI.Controls.ctrlCloseTradefromAllocation ctrlCloseTradefromAllocation1;
        private Prana.Utilities.UI.UIUtilities.MultiSelectDropDown MultiSelectDropDown1;
        private Infragistics.Win.Misc.UltraPanel Form1_Fill_Panel;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CloseTrade_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CloseTrade_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CloseTrade_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CloseTrade_UltraFormManager_Dock_Area_Bottom;
        private Utilities.UI.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;

    }
}