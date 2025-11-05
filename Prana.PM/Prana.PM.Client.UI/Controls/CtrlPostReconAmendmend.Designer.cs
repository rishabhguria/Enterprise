using Prana.Utilities.UI.UIUtilities;

namespace Prana.PM.Client.UI.Controls
{
    partial class CtrlPostReconAmendmend
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
                if (headerCheckBoxUnWind != null)
                {
                    headerCheckBoxUnWind.Dispose();
                }
                if (_pricingServicesProxy != null)
                {
                    _pricingServicesProxy.Dispose();
                }
                if (_lastModifiedCell != null)
                {
                    _lastModifiedCell.Dispose();
                }
                if (dtMarkPrice != null)
                {
                    dtMarkPrice.Dispose();
                }
                if (_allocationServices != null)
                {
                    _allocationServices.Dispose();
                }
                if (backgroundWorker != null)
                {
                    backgroundWorker.Dispose();
                }
                if (_closingServices != null)
                {
                    _closingServices.Dispose();
                }
                if (preferences != null)
                {
                    preferences.Dispose();
                }
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chkBxIsAutoCloseStrategy = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.btnRestoreDefaultAlgo = new Infragistics.Win.Misc.UltraButton();
            this.lblAccountPNLPostAmendment = new Infragistics.Win.Misc.UltraLabel();
            this.lblAccountPNLPostAmendmentValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblAccountPNLPreAmendment = new Infragistics.Win.Misc.UltraLabel();
            this.lblAccountPNLPreAmendmentValue = new Infragistics.Win.Misc.UltraLabel();
            this.btnGetData = new Infragistics.Win.Misc.UltraButton();
            this.lblStartDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblEndDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtEndDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtStartDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblSecondaySortValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblSeconadySort = new Infragistics.Win.Misc.UltraLabel();
            this.btnPostTransaction = new Infragistics.Win.Misc.UltraButton();
            this.lblNAVPeriodValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblLockDateValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblAccountingRuleValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblHeader = new Infragistics.Win.Misc.UltraLabel();
            this.lblSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.lblAccount = new Infragistics.Win.Misc.UltraLabel();
            this.lblNAVPeriod = new Infragistics.Win.Misc.UltraLabel();
            this.lblLockDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblAccountingRule = new Infragistics.Win.Misc.UltraLabel();
            this.lblSymbolPNLPostAmendment = new Infragistics.Win.Misc.UltraLabel();
            this.lblAccountValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblSymbolValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblSymbolPNLPostAmendmentValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblSymbolPNLPreAmendment = new Infragistics.Win.Misc.UltraLabel();
            this.lblSymbolPNLPreAmendmentValue = new Infragistics.Win.Misc.UltraLabel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.grdData = new PranaUltraGrid();
            this.ultraCalcManager1 = new Infragistics.Win.UltraWinCalcManager.UltraCalcManager(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unwindToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grdClosed = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grpBoxComments = new Infragistics.Win.Misc.UltraGroupBox();
            this.tbComments = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraStatusBar1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnReverse = new Infragistics.Win.Misc.UltraButton();
            this.btnPreview = new Infragistics.Win.Misc.UltraButton();
            this.bgWorkerUnwindingClosing = new System.ComponentModel.BackgroundWorker();
            this._bgUnwindClosing = new System.ComponentModel.BackgroundWorker();
            this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.ultraToolTipManager2 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkBxIsAutoCloseStrategy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCalcManager1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdClosed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxComments)).BeginInit();
            this.grpBoxComments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbComments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.splitContainer1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1175, 526);
            this.ultraPanel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chkBxIsAutoCloseStrategy);
            this.splitContainer1.Panel1.Controls.Add(this.btnRestoreDefaultAlgo);
            this.splitContainer1.Panel1.Controls.Add(this.lblAccountPNLPostAmendment);
            this.splitContainer1.Panel1.Controls.Add(this.lblAccountPNLPostAmendmentValue);
            this.splitContainer1.Panel1.Controls.Add(this.lblAccountPNLPreAmendment);
            this.splitContainer1.Panel1.Controls.Add(this.lblAccountPNLPreAmendmentValue);
            this.splitContainer1.Panel1.Controls.Add(this.btnGetData);
            this.splitContainer1.Panel1.Controls.Add(this.lblStartDate);
            this.splitContainer1.Panel1.Controls.Add(this.lblEndDate);
            this.splitContainer1.Panel1.Controls.Add(this.dtEndDate);
            this.splitContainer1.Panel1.Controls.Add(this.dtStartDate);
            this.splitContainer1.Panel1.Controls.Add(this.lblSecondaySortValue);
            this.splitContainer1.Panel1.Controls.Add(this.lblSeconadySort);
            this.splitContainer1.Panel1.Controls.Add(this.btnPostTransaction);
            this.splitContainer1.Panel1.Controls.Add(this.lblNAVPeriodValue);
            this.splitContainer1.Panel1.Controls.Add(this.lblLockDateValue);
            this.splitContainer1.Panel1.Controls.Add(this.lblAccountingRuleValue);
            this.splitContainer1.Panel1.Controls.Add(this.lblHeader);
            this.splitContainer1.Panel1.Controls.Add(this.lblSymbol);
            this.splitContainer1.Panel1.Controls.Add(this.lblAccount);
            this.splitContainer1.Panel1.Controls.Add(this.lblNAVPeriod);
            this.splitContainer1.Panel1.Controls.Add(this.lblLockDate);
            this.splitContainer1.Panel1.Controls.Add(this.lblAccountingRule);
            this.splitContainer1.Panel1.Controls.Add(this.lblSymbolPNLPostAmendment);
            this.splitContainer1.Panel1.Controls.Add(this.lblAccountValue);
            this.splitContainer1.Panel1.Controls.Add(this.lblSymbolValue);
            this.splitContainer1.Panel1.Controls.Add(this.lblSymbolPNLPostAmendmentValue);
            this.splitContainer1.Panel1.Controls.Add(this.lblSymbolPNLPreAmendment);
            this.splitContainer1.Panel1.Controls.Add(this.lblSymbolPNLPreAmendmentValue);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1175, 526);
            this.splitContainer1.SplitterDistance = 121;
            this.splitContainer1.TabIndex = 0;
            // 
            // chkBxIsAutoCloseStrategy
            // 
            this.chkBxIsAutoCloseStrategy.Location = new System.Drawing.Point(287, 97);
            this.chkBxIsAutoCloseStrategy.Name = "chkBxIsAutoCloseStrategy";
            this.chkBxIsAutoCloseStrategy.Size = new System.Drawing.Size(157, 17);
            this.chkBxIsAutoCloseStrategy.TabIndex = 118;
            this.chkBxIsAutoCloseStrategy.Text = "Auto Close Strategy";
            // 
            // btnRestoreDefaultAlgo
            // 
            this.btnRestoreDefaultAlgo.Location = new System.Drawing.Point(450, 93);
            this.btnRestoreDefaultAlgo.Name = "btnRestoreDefaultAlgo";
            this.btnRestoreDefaultAlgo.Size = new System.Drawing.Size(137, 26);
            this.btnRestoreDefaultAlgo.TabIndex = 27;
            this.btnRestoreDefaultAlgo.Text = "Restore Default Algo";
            this.btnRestoreDefaultAlgo.Click += new System.EventHandler(this.btnRestoreDefaultAlgo_Click);
            // 
            // lblAccountPNLPostAmendment
            // 
            this.lblAccountPNLPostAmendment.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblAccountPNLPostAmendment.Location = new System.Drawing.Point(620, 97);
            this.lblAccountPNLPostAmendment.Name = "lblAccountPNLPostAmendment";
            this.lblAccountPNLPostAmendment.Size = new System.Drawing.Size(182, 15);
            this.lblAccountPNLPostAmendment.TabIndex = 26;
            this.lblAccountPNLPostAmendment.Text = "Account PNL Post Amendment";
            // 
            // lblAccountPNLPostAmendmentValue
            // 
            this.lblAccountPNLPostAmendmentValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblAccountPNLPostAmendmentValue.Location = new System.Drawing.Point(817, 97);
            this.lblAccountPNLPostAmendmentValue.MaximumSize = new System.Drawing.Size(120, 15);
            this.lblAccountPNLPostAmendmentValue.Name = "lblAccountPNLPostAmendmentValue";
            this.lblAccountPNLPostAmendmentValue.Size = new System.Drawing.Size(120, 15);
            this.lblAccountPNLPostAmendmentValue.TabIndex = 25;
            this.lblAccountPNLPostAmendmentValue.Text = "0";
            // 
            // lblAccountPNLPreAmendment
            // 
            this.lblAccountPNLPreAmendment.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblAccountPNLPreAmendment.Location = new System.Drawing.Point(620, 76);
            this.lblAccountPNLPreAmendment.Name = "lblAccountPNLPreAmendment";
            this.lblAccountPNLPreAmendment.Size = new System.Drawing.Size(182, 15);
            this.lblAccountPNLPreAmendment.TabIndex = 24;
            this.lblAccountPNLPreAmendment.Text = "Account PNL Pre Amendment";
            // 
            // lblAccountPNLPreAmendmentValue
            // 
            this.lblAccountPNLPreAmendmentValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblAccountPNLPreAmendmentValue.Location = new System.Drawing.Point(817, 76);
            this.lblAccountPNLPreAmendmentValue.MaximumSize = new System.Drawing.Size(120, 15);
            this.lblAccountPNLPreAmendmentValue.Name = "lblAccountPNLPreAmendmentValue";
            this.lblAccountPNLPreAmendmentValue.Size = new System.Drawing.Size(120, 15);
            this.lblAccountPNLPreAmendmentValue.TabIndex = 23;
            this.lblAccountPNLPreAmendmentValue.Text = "0";
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(1052, 92);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(100, 26);
            this.btnGetData.TabIndex = 22;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // lblStartDate
            // 
            this.lblStartDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblStartDate.Location = new System.Drawing.Point(946, 34);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(100, 15);
            this.lblStartDate.TabIndex = 21;
            this.lblStartDate.Text = "Start Date";
            this.lblStartDate.Visible = false;
            // 
            // lblEndDate
            // 
            this.lblEndDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblEndDate.Location = new System.Drawing.Point(963, 55);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(50, 17);
            this.lblEndDate.TabIndex = 20;
            this.lblEndDate.Text = "To Date";
            // 
            // dtEndDate
            // 
            this.dtEndDate.Location = new System.Drawing.Point(1052, 53);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(101, 22);
            this.dtEndDate.TabIndex = 19;
            // 
            // dtStartDate
            // 
            this.dtStartDate.Location = new System.Drawing.Point(1052, 30);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(101, 22);
            this.dtStartDate.TabIndex = 18;
            this.dtStartDate.Visible = false;
            // 
            // lblSecondaySortValue
            // 
            this.lblSecondaySortValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSecondaySortValue.Location = new System.Drawing.Point(467, 76);
            this.lblSecondaySortValue.Name = "lblSecondaySortValue";
            this.lblSecondaySortValue.Size = new System.Drawing.Size(120, 15);
            this.lblSecondaySortValue.TabIndex = 17;
            this.lblSecondaySortValue.Text = "Secondary Sort";
            // 
            // lblSeconadySort
            // 
            this.lblSeconadySort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSeconadySort.Location = new System.Drawing.Point(332, 76);
            this.lblSeconadySort.Name = "lblSeconadySort";
            this.lblSeconadySort.Size = new System.Drawing.Size(100, 15);
            this.lblSeconadySort.TabIndex = 16;
            this.lblSeconadySort.Text = "Secondary Sort";
            // 
            // btnPostTransaction
            // 
            this.btnPostTransaction.Location = new System.Drawing.Point(148, 92);
            this.btnPostTransaction.Name = "btnPostTransaction";
            this.btnPostTransaction.Size = new System.Drawing.Size(125, 26);
            this.btnPostTransaction.TabIndex = 15;
            this.btnPostTransaction.Text = "Post Transaction";
            this.btnPostTransaction.Click += new System.EventHandler(this.btnPostTransaction_Click);
            // 
            // lblNAVPeriodValue
            // 
            this.lblNAVPeriodValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblNAVPeriodValue.Location = new System.Drawing.Point(148, 76);
            this.lblNAVPeriodValue.Name = "lblNAVPeriodValue";
            this.lblNAVPeriodValue.Size = new System.Drawing.Size(100, 15);
            this.lblNAVPeriodValue.TabIndex = 14;
            this.lblNAVPeriodValue.Text = "Nav Period";
            // 
            // lblLockDateValue
            // 
            this.lblLockDateValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblLockDateValue.Location = new System.Drawing.Point(467, 34);
            this.lblLockDateValue.Name = "lblLockDateValue";
            this.lblLockDateValue.Size = new System.Drawing.Size(100, 15);
            this.lblLockDateValue.TabIndex = 13;
            this.lblLockDateValue.Text = "Lock Date";
            // 
            // lblAccountingRuleValue
            // 
            this.lblAccountingRuleValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblAccountingRuleValue.Location = new System.Drawing.Point(467, 55);
            this.lblAccountingRuleValue.Name = "lblAccountingRuleValue";
            this.lblAccountingRuleValue.Size = new System.Drawing.Size(100, 15);
            this.lblAccountingRuleValue.TabIndex = 12;
            this.lblAccountingRuleValue.Text = "Accounting Rule";
            // 
            // lblHeader
            // 
            this.lblHeader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblHeader.Location = new System.Drawing.Point(13, 12);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(197, 15);
            this.lblHeader.TabIndex = 11;
            this.lblHeader.Text = "View/Amend - NAV Period Lock";
            // 
            // lblSymbol
            // 
            this.lblSymbol.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSymbol.Location = new System.Drawing.Point(25, 34);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(79, 15);
            this.lblSymbol.TabIndex = 10;
            this.lblSymbol.Text = "Symbol";
            // 
            // lblAccount
            // 
            this.lblAccount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblAccount.Location = new System.Drawing.Point(25, 55);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(88, 15);
            this.lblAccount.TabIndex = 9;
            this.lblAccount.Text = "Account";
            // 
            // lblNAVPeriod
            // 
            this.lblNAVPeriod.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblNAVPeriod.Location = new System.Drawing.Point(25, 76);
            this.lblNAVPeriod.Name = "lblNAVPeriod";
            this.lblNAVPeriod.Size = new System.Drawing.Size(79, 15);
            this.lblNAVPeriod.TabIndex = 8;
            this.lblNAVPeriod.Text = "NAVPeriod";
            // 
            // lblLockDate
            // 
            this.lblLockDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblLockDate.Location = new System.Drawing.Point(332, 34);
            this.lblLockDate.Name = "lblLockDate";
            this.lblLockDate.Size = new System.Drawing.Size(100, 15);
            this.lblLockDate.TabIndex = 7;
            this.lblLockDate.Text = "Lock Date";
            // 
            // lblAccountingRule
            // 
            this.lblAccountingRule.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblAccountingRule.Location = new System.Drawing.Point(332, 55);
            this.lblAccountingRule.Name = "lblAccountingRule";
            this.lblAccountingRule.Size = new System.Drawing.Size(100, 15);
            this.lblAccountingRule.TabIndex = 6;
            this.lblAccountingRule.Text = "Accounting Rule";
            // 
            // lblSymbolPNLPostAmendment
            // 
            this.lblSymbolPNLPostAmendment.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSymbolPNLPostAmendment.Location = new System.Drawing.Point(620, 55);
            this.lblSymbolPNLPostAmendment.Name = "lblSymbolPNLPostAmendment";
            this.lblSymbolPNLPostAmendment.Size = new System.Drawing.Size(182, 15);
            this.lblSymbolPNLPostAmendment.TabIndex = 5;
            this.lblSymbolPNLPostAmendment.Text = "Symbol PNL Post Amendment";
            // 
            // lblAccountValue
            // 
            this.lblAccountValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblAccountValue.Location = new System.Drawing.Point(148, 55);
            this.lblAccountValue.Name = "lblAccountValue";
            this.lblAccountValue.Size = new System.Drawing.Size(171, 15);
            this.lblAccountValue.TabIndex = 4;
            this.lblAccountValue.Text = "Account";
            // 
            // lblSymbolValue
            // 
            this.lblSymbolValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSymbolValue.Location = new System.Drawing.Point(148, 34);
            this.lblSymbolValue.Name = "lblSymbolValue";
            this.lblSymbolValue.Size = new System.Drawing.Size(171, 15);
            this.lblSymbolValue.TabIndex = 3;
            this.lblSymbolValue.Text = "Symbol";
            // 
            // lblSymbolPNLPostAmendmentValue
            // 
            this.lblSymbolPNLPostAmendmentValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSymbolPNLPostAmendmentValue.Location = new System.Drawing.Point(817, 55);
            this.lblSymbolPNLPostAmendmentValue.MaximumSize = new System.Drawing.Size(120, 15);
            this.lblSymbolPNLPostAmendmentValue.Name = "lblSymbolPNLPostAmendmentValue";
            this.lblSymbolPNLPostAmendmentValue.Size = new System.Drawing.Size(120, 15);
            this.lblSymbolPNLPostAmendmentValue.TabIndex = 2;
            this.lblSymbolPNLPostAmendmentValue.Text = "0";
            // 
            // lblSymbolPNLPreAmendment
            // 
            this.lblSymbolPNLPreAmendment.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSymbolPNLPreAmendment.Location = new System.Drawing.Point(620, 34);
            this.lblSymbolPNLPreAmendment.Name = "lblSymbolPNLPreAmendment";
            this.lblSymbolPNLPreAmendment.Size = new System.Drawing.Size(182, 15);
            this.lblSymbolPNLPreAmendment.TabIndex = 1;
            this.lblSymbolPNLPreAmendment.Text = "Symbol PNL Pre Amendment";
            // 
            // lblSymbolPNLPreAmendmentValue
            // 
            this.lblSymbolPNLPreAmendmentValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblSymbolPNLPreAmendmentValue.Location = new System.Drawing.Point(817, 34);
            this.lblSymbolPNLPreAmendmentValue.MaximumSize = new System.Drawing.Size(120, 15);
            this.lblSymbolPNLPreAmendmentValue.Name = "lblSymbolPNLPreAmendmentValue";
            this.lblSymbolPNLPreAmendmentValue.Size = new System.Drawing.Size(120, 15);
            this.lblSymbolPNLPreAmendmentValue.TabIndex = 0;
            this.lblSymbolPNLPreAmendmentValue.Text = "0";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ultraSplitter1);
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            this.splitContainer2.Panel1.Controls.Add(this.grpBoxComments);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ultraStatusBar1);
            this.splitContainer2.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer2.Panel2.Controls.Add(this.btnSave);
            this.splitContainer2.Panel2.Controls.Add(this.btnReverse);
            this.splitContainer2.Panel2.Controls.Add(this.btnPreview);
            this.splitContainer2.Size = new System.Drawing.Size(1175, 401);
            this.splitContainer2.SplitterDistance = 336;
            this.splitContainer2.TabIndex = 0;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter1.Enabled = false;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 285);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 45;
            this.ultraSplitter1.Size = new System.Drawing.Size(1175, 6);
            this.ultraSplitter1.TabIndex = 29;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.grdData);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.grdClosed);
            this.splitContainer3.Size = new System.Drawing.Size(1175, 291);
            this.splitContainer3.SplitterDistance = 144;
            this.splitContainer3.TabIndex = 1;
            // 
            // grdData
            // 
            this.grdData.CalcManager = this.ultraCalcManager1;
            this.grdData.ContextMenuStrip = this.contextMenuStrip1;
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdData.DisplayLayout.Appearance = appearance1;
            this.grdData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdData.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdData.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdData.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdData.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdData.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdData.DisplayLayout.MaxColScrollRegions = 1;
            this.grdData.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdData.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdData.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdData.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdData.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdData.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdData.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdData.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdData.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdData.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdData.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grdData.Location = new System.Drawing.Point(0, 0);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(1175, 144);
            this.grdData.TabIndex = 0;
            this.grdData.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdData_AfterCellUpdate);
            this.grdData.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdData_InitializeLayout);
            this.grdData.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdData_InitializeRow);
            this.grdData.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdData_CellChange);
            this.grdData.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdData_ClickCellButton);
            this.grdData.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdData_BeforeCellUpdate);
            this.grdData.FilterCellValueChanged += new Infragistics.Win.UltraWinGrid.FilterCellValueChangedEventHandler(this.grdData_FilterCellValueChanged);
            this.grdData.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdData_DragDrop);
            this.grdData.DragEnter += new System.Windows.Forms.DragEventHandler(this.grdData_DragEnter);
            this.grdData.DragOver += new System.Windows.Forms.DragEventHandler(this.grdData_DragOver);
            this.grdData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdData_MouseDown);
            this.grdData.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdData_MouseMove);
            // 
            // ultraCalcManager1
            // 
            this.ultraCalcManager1.ContainingControl = this;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.unwindToolStripMenuItem,
            this.saveLayoutToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 92);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Visible = false;
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // unwindToolStripMenuItem
            // 
            this.unwindToolStripMenuItem.Name = "unwindToolStripMenuItem";
            this.unwindToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.unwindToolStripMenuItem.Text = "Unwind";
            this.unwindToolStripMenuItem.Visible = false;
            this.unwindToolStripMenuItem.Click += new System.EventHandler(this.unwindToolStripMenuItem_Click);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // grdClosed
            // 
            this.grdClosed.CalcManager = this.ultraCalcManager1;
            this.grdClosed.ContextMenuStrip = this.contextMenuStrip1;
            appearance13.BackColor = System.Drawing.Color.Black;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdClosed.DisplayLayout.Appearance = appearance13;
            this.grdClosed.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdClosed.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.grdClosed.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdClosed.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.grdClosed.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdClosed.DisplayLayout.GroupByBox.Hidden = true;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdClosed.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.grdClosed.DisplayLayout.MaxColScrollRegions = 1;
            this.grdClosed.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdClosed.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdClosed.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.grdClosed.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdClosed.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdClosed.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.grdClosed.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdClosed.DisplayLayout.Override.CellAppearance = appearance20;
            this.grdClosed.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdClosed.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.grdClosed.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.grdClosed.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.grdClosed.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdClosed.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.grdClosed.DisplayLayout.Override.RowAppearance = appearance23;
            this.grdClosed.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdClosed.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.grdClosed.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdClosed.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdClosed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdClosed.Location = new System.Drawing.Point(0, 0);
            this.grdClosed.Name = "grdClosed";
            this.grdClosed.Size = new System.Drawing.Size(1175, 143);
            this.grdClosed.TabIndex = 0;
            this.grdClosed.Text = "ultraGrid1";
            this.grdClosed.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdClosed_InitializeLayout);
            this.grdClosed.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdClosed_InitializeRow);
            this.grdClosed.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdClosed_MouseDown);
            // 
            // grpBoxComments
            // 
            this.grpBoxComments.Controls.Add(this.tbComments);
            this.grpBoxComments.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpBoxComments.Location = new System.Drawing.Point(0, 291);
            this.grpBoxComments.Name = "grpBoxComments";
            this.grpBoxComments.Size = new System.Drawing.Size(1175, 45);
            this.grpBoxComments.TabIndex = 28;
            this.grpBoxComments.Text = "Comments";
            // 
            // tbComments
            // 
            this.tbComments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbComments.Location = new System.Drawing.Point(3, 17);
            this.tbComments.Name = "tbComments";
            this.tbComments.Size = new System.Drawing.Size(1169, 22);
            this.tbComments.TabIndex = 3;
            this.tbComments.ValueChanged += new System.EventHandler(this.tbComments_ValueChanged);
            // 
            // ultraStatusBar1
            // 
            this.ultraStatusBar1.Location = new System.Drawing.Point(0, 38);
            this.ultraStatusBar1.Name = "ultraStatusBar1";
            this.ultraStatusBar1.Size = new System.Drawing.Size(1175, 23);
            this.ultraStatusBar1.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnCancel.Location = new System.Drawing.Point(682, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(573, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(81, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnReverse
            // 
            this.btnReverse.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnReverse.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnReverse.Location = new System.Drawing.Point(464, 9);
            this.btnReverse.Name = "btnReverse";
            this.btnReverse.Size = new System.Drawing.Size(81, 23);
            this.btnReverse.TabIndex = 0;
            this.btnReverse.Text = "Reverse";
            this.btnReverse.Click += new System.EventHandler(this.btnReverse_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPreview.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnPreview.Location = new System.Drawing.Point(355, 9);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(81, 23);
            this.btnPreview.TabIndex = 1;
            this.btnPreview.Text = "Preview";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // bgWorkerUnwindingClosing
            // 
            this.bgWorkerUnwindingClosing.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerUnwindingClosing_DoWork);
            this.bgWorkerUnwindingClosing.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerUnwindingClosing_RunWorkerCompleted);
            // 
            // _bgUnwindClosing
            // 
            this._bgUnwindClosing.DoWork += new System.ComponentModel.DoWorkEventHandler(this._bgUnwindClosing_DoWork);
            this._bgUnwindClosing.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this._bgUnwindClosing_RunWorkerCompleted);
            // 
            // ultraToolTipManager1
            // 
            this.ultraToolTipManager1.ContainingControl = this;
            // 
            // ultraToolTipManager2
            // 
            this.ultraToolTipManager2.ContainingControl = this;
            // 
            // CtrlPostReconAmendmend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "CtrlPostReconAmendmend";
            this.Size = new System.Drawing.Size(1175, 526);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkBxIsAutoCloseStrategy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCalcManager1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdClosed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxComments)).EndInit();
            this.grpBoxComments.ResumeLayout(false);
            this.grpBoxComments.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbComments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).EndInit();
            this.ResumeLayout(false);

        }

        
        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnReverse;
        private Infragistics.Win.Misc.UltraButton btnPreview;
        private Infragistics.Win.Misc.UltraButton btnPostTransaction;
        private Infragistics.Win.Misc.UltraLabel lblNAVPeriodValue;
        private Infragistics.Win.Misc.UltraLabel lblLockDateValue;
        private Infragistics.Win.Misc.UltraLabel lblAccountingRuleValue;
        private Infragistics.Win.Misc.UltraLabel lblHeader;
        private Infragistics.Win.Misc.UltraLabel lblSymbol;
        private Infragistics.Win.Misc.UltraLabel lblAccount;
        private Infragistics.Win.Misc.UltraLabel lblNAVPeriod;
        private Infragistics.Win.Misc.UltraLabel lblLockDate;
        private Infragistics.Win.Misc.UltraLabel lblAccountingRule;
        private Infragistics.Win.Misc.UltraLabel lblSymbolPNLPostAmendment;
        private Infragistics.Win.Misc.UltraLabel lblAccountValue;
        private Infragistics.Win.Misc.UltraLabel lblSymbolValue;
        private Infragistics.Win.Misc.UltraLabel lblSymbolPNLPostAmendmentValue;
        private Infragistics.Win.Misc.UltraLabel lblSymbolPNLPreAmendment;
        private Infragistics.Win.Misc.UltraLabel lblSymbolPNLPreAmendmentValue;
        private PranaUltraGrid grdData;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private Infragistics.Win.Misc.UltraLabel lblSecondaySortValue;
        private Infragistics.Win.Misc.UltraLabel lblSeconadySort;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdClosed;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private Infragistics.Win.Misc.UltraLabel lblStartDate;
        private Infragistics.Win.Misc.UltraLabel lblEndDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtEndDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtStartDate;
        private Infragistics.Win.Misc.UltraButton btnGetData;
        private Infragistics.Win.UltraWinCalcManager.UltraCalcManager ultraCalcManager1;
        private System.ComponentModel.BackgroundWorker bgWorkerUnwindingClosing;
        private System.Windows.Forms.ToolStripMenuItem unwindToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker _bgUnwindClosing;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        internal Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar1;
        private Infragistics.Win.Misc.UltraLabel lblAccountPNLPostAmendment;
        private Infragistics.Win.Misc.UltraLabel lblAccountPNLPostAmendmentValue;
        private Infragistics.Win.Misc.UltraLabel lblAccountPNLPreAmendment;
        private Infragistics.Win.Misc.UltraLabel lblAccountPNLPreAmendmentValue;
        private Infragistics.Win.Misc.UltraButton btnRestoreDefaultAlgo;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager1;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager2;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxComments;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor tbComments;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBxIsAutoCloseStrategy;
    }
}
