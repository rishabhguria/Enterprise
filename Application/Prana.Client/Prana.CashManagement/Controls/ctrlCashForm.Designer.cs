using Prana.LogManager;
using Prana.BusinessObjects.Constants;
using Prana.CashManagement.Classes;
using Prana.Global;
using Prana.Interfaces;
using Prana.WCFConnectionMgr;
using System;
namespace Prana.CashManagement
{
    partial class ctrlCashForm
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (noOfDayEndCashTab != null && noOfDayEndCashTab.Count > 0)
                    {
                        for (int counter = 0; counter < noOfDayEndCashTab.Count; counter++)
                        {
                            noOfDayEndCashTab[counter].CashLayout -= new EventHandler<EventArgs<CashManagementLayout>>(SetLayoutForDayEndCashGrid);
                            noOfDayEndCashTab[counter].Dispose();
                            noOfDayEndCashTab[counter] = null;
                        }
                        noOfDayEndCashTab = null;
                    }
                    if (components != null)
                    {
                        components.Dispose();
                        CashDataManager.CashManagementServices.ConnectedEvent -= new Proxy<ICashManagementService>.ConnectionEventHandler(CashManagementServices_ConnectedEvent);
                        CashDataManager.CashManagementServices.DisconnectedEvent -= new Proxy<ICashManagementService>.ConnectionEventHandler(CashManagementServices_DisconnectedEvent);
                    }
                    if (ctrlMasterFundAndAccountsDropdown1 != null)
                    {
                        ctrlMasterFundAndAccountsDropdown1.CheckStateChanged -= ctrlMasterFundAndAccountsDropdown1_CheckStateChanged;
                    }
                    if (_proxy != null)
                    {
                        _proxy.InnerChannel.UnSubscribe(Topics.Topic_DayEndCash);
                        _proxy.InnerChannel.UnSubscribe(Topics.Topic_CashData);
                        _proxy.Dispose();
                        _proxy = null;
                    }

                    if (tabCntlDayEndData != null)
                    {
                        tabCntlDayEndData.Tabs.Clear();
                        tabCntlDayEndData.Dispose();
                    }
                    if (transactionDetails != null)
                    {
                        transactionDetails.Close();
                        transactionDetails = null;
                    }

                    if (ctrlDayEndDataGrid1 != null)
                    {
                        ctrlDayEndDataGrid1.Dispose();
                        ctrlDayEndDataGrid1 = null;
                    }
                    if (_cashImpactToBind != null)
                    {
                        _cashImpactToBind.Clear();
                        _cashImpactToBind = null;
                    }
                    if (_getedTransactions != null)
                    {
                        _getedTransactions.Clear();
                        _getedTransactions = null;
                    }
                    if (_dayEndDataToBind != null)
                    {
                        _dayEndDataToBind.Clear();
                        _dayEndDataToBind = null;
                    }
                    if (_dateWiseDayEndDataDictionary != null)
                    {
                        _dateWiseDayEndDataDictionary.Clear();
                        _dateWiseDayEndDataDictionary = null;
                    }
                    if (_dictforexGivenDateRange != null)
                    {
                        _dictforexGivenDateRange.Clear();
                        _dictforexGivenDateRange = null;
                    }
                    if(ctrlgrdTodayDayEnd != null)
                    {
                        ctrlgrdTodayDayEnd.Dispose();
                    }
                }
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlDayEndDataGrid1 = new Prana.CashManagement.Controls.ctrlDataGrid();
            this.menuStripGetLots = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.dtToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnRunBatch = new Infragistics.Win.Misc.UltraButton();
            this.lblDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnGet = new Infragistics.Win.Misc.UltraButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlYesterdayDayEnd = new Infragistics.Win.Misc.UltraGroupBox();
            this.grdYesterdayEnd = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStripItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnltabDayEnd = new System.Windows.Forms.Panel();
            this.tabCntlDayEndData = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.lblTodayDayEnd = new System.Windows.Forms.Label();
            this.ultraExpnGrpBxAllTransactions = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.grdGetLots = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ugbxDayEndParams = new Infragistics.Win.Misc.UltraGroupBox();
            this.ctrlMasterFundAndAccountsDropdown1 = new Prana.CashManagement.Controls.ctrlMasterFundAndAccountsDropdown();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraTabPageControl1.SuspendLayout();
            this.menuStripGetLots.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlYesterdayDayEnd)).BeginInit();
            this.pnlYesterdayDayEnd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdYesterdayEnd)).BeginInit();
            this.contextMenuStripItems.SuspendLayout();
            this.pnltabDayEnd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabCntlDayEndData)).BeginInit();
            this.tabCntlDayEndData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpnGrpBxAllTransactions)).BeginInit();
            this.ultraExpnGrpBxAllTransactions.SuspendLayout();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdGetLots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxDayEndParams)).BeginInit();
            this.ugbxDayEndParams.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ctrlDayEndDataGrid1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 25);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(622, 112);
            // 
            // ctrlDayEndDataGrid1
            // 
            this.ctrlDayEndDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlDayEndDataGrid1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ctrlDayEndDataGrid1.Location = new System.Drawing.Point(0, 0);
            this.ctrlDayEndDataGrid1.Margin = new System.Windows.Forms.Padding(4);
            this.ctrlDayEndDataGrid1.Name = "ctrlDayEndDataGrid1";
            this.ctrlDayEndDataGrid1.Size = new System.Drawing.Size(622, 112);
            this.ctrlDayEndDataGrid1.TabIndex = 5;
            // 
            // menuStripGetLots
            // 
            this.menuStripGetLots.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRowToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.menuStripGetLots.Name = "menuStripCashValue";
            this.menuStripGetLots.Size = new System.Drawing.Size(68, 48);
            this.inboxControlStyler1.SetStyleSettings(this.menuStripGetLots, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // addRowToolStripMenuItem
            // 
            this.addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
            this.addRowToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // ultraLabel1
            // 
            appearance39.FontData.SizeInPoints = 9F;
            appearance39.TextHAlignAsString = "Left";
            appearance39.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance39;
            this.ultraLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(660, 14);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(21, 23);
            this.ultraLabel1.TabIndex = 102;
            this.ultraLabel1.Text = "To";
            // 
            // dtToDate
            // 
            appearance40.FontData.SizeInPoints = 9F;
            this.dtToDate.Appearance = appearance40;
            this.dtToDate.AutoSize = false;
            this.dtToDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtToDate.Location = new System.Drawing.Point(690, 14);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(109, 23);
            this.dtToDate.TabIndex = 101;
            this.dtToDate.ValueChanged += new System.EventHandler(this.ultraDateTimeEditor1_ValueChanged);
            // 
            // btnRunBatch
            // 
            appearance41.FontData.SizeInPoints = 9F;
            this.btnRunBatch.Appearance = appearance41;
            this.btnRunBatch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunBatch.Location = new System.Drawing.Point(896, 14);
            this.btnRunBatch.Name = "btnRunBatch";
            this.btnRunBatch.Size = new System.Drawing.Size(78, 23);
            this.btnRunBatch.TabIndex = 2;
            this.btnRunBatch.Text = "Calculate";
            this.btnRunBatch.Click += new System.EventHandler(this.btnRunBatch_Click);
            // 
            // lblDate
            // 
            appearance42.FontData.SizeInPoints = 9F;
            appearance42.TextHAlignAsString = "Left";
            appearance42.TextVAlignAsString = "Middle";
            this.lblDate.Appearance = appearance42;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(493, 14);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(56, 23);
            this.lblDate.TabIndex = 100;
            this.lblDate.Text = "From";
            // 
            // dtFromDate
            // 
            appearance43.FontData.SizeInPoints = 9F;
            this.dtFromDate.Appearance = appearance43;
            this.dtFromDate.AutoSize = false;
            this.dtFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFromDate.Location = new System.Drawing.Point(530, 14);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(109, 23);
            this.dtFromDate.TabIndex = 0;
            this.dtFromDate.ValueChanged += new System.EventHandler(this.dtDate_ValueChanged);
            // 
            // btnSave
            // 
            appearance44.FontData.SizeInPoints = 9F;
            this.btnSave.Appearance = appearance44;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(982, 14);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGet
            // 
            appearance45.FontData.SizeInPoints = 9F;
            this.btnGet.Appearance = appearance45;
            this.btnGet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGet.Location = new System.Drawing.Point(810, 14);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(78, 23);
            this.btnGet.TabIndex = 1;
            this.btnGet.Text = "Get Data";
            this.btnGet.Click += new System.EventHandler(this.btnGetFx_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(2, 44);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer2.Panel1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ultraExpnGrpBxAllTransactions);
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer2.Panel2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer2.Size = new System.Drawing.Size(1092, 388);
            this.splitContainer2.SplitterDistance = 140;
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer2.TabIndex = 110;
            this.splitContainer2.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlYesterdayDayEnd);
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1.Panel1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnltabDayEnd);
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1.Panel2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer1.Size = new System.Drawing.Size(1092, 140);
            this.splitContainer1.SplitterDistance = 462;
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer1.TabIndex = 108;
            // 
            // pnlYesterdayDayEnd
            // 
            appearance46.FontData.SizeInPoints = 9F;
            this.pnlYesterdayDayEnd.Appearance = appearance46;
            this.pnlYesterdayDayEnd.Controls.Add(this.grdYesterdayEnd);
            this.pnlYesterdayDayEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlYesterdayDayEnd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlYesterdayDayEnd.Location = new System.Drawing.Point(0, 0);
            this.pnlYesterdayDayEnd.Name = "pnlYesterdayDayEnd";
            this.pnlYesterdayDayEnd.Size = new System.Drawing.Size(462, 140);
            this.pnlYesterdayDayEnd.TabIndex = 0;
            this.pnlYesterdayDayEnd.Text = "Last Day End Cash";
            // 
            // grdYesterdayEnd
            // 
            this.grdYesterdayEnd.ContextMenuStrip = this.contextMenuStripItems;
            appearance9.BackColor = System.Drawing.Color.Black;
            this.grdYesterdayEnd.DisplayLayout.Appearance = appearance9;
            this.grdYesterdayEnd.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdYesterdayEnd.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdYesterdayEnd.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdYesterdayEnd.DisplayLayout.EmptyRowSettings.Style = Infragistics.Win.UltraWinGrid.EmptyRowStyle.HideRowSelector;
            this.grdYesterdayEnd.DisplayLayout.MaxColScrollRegions = 1;
            this.grdYesterdayEnd.DisplayLayout.MaxRowScrollRegions = 1;
            appearance10.BackColor = System.Drawing.Color.LightSlateGray;
            appearance10.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance10.BorderColor = System.Drawing.Color.DimGray;
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.White;
            this.grdYesterdayEnd.DisplayLayout.Override.ActiveRowAppearance = appearance10;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdYesterdayEnd.DisplayLayout.Override.BorderStyleRowSelector = Infragistics.Win.UIElementBorderStyle.None;
            this.grdYesterdayEnd.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdYesterdayEnd.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
            this.grdYesterdayEnd.DisplayLayout.Override.CellPadding = 0;
            this.grdYesterdayEnd.DisplayLayout.Override.CellSpacing = 0;
            this.grdYesterdayEnd.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            appearance11.BackColor = System.Drawing.Color.Gray;
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance11.BorderColor = System.Drawing.Color.Black;
            appearance11.FontData.BoldAsString = "True";
            appearance11.ForeColor = System.Drawing.Color.White;
            this.grdYesterdayEnd.DisplayLayout.Override.GroupByRowAppearance = appearance11;
            this.grdYesterdayEnd.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
            this.grdYesterdayEnd.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.SummaryCells;
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            appearance12.TextHAlignAsString = "Right";
            appearance12.TextVAlignAsString = "Middle";
            this.grdYesterdayEnd.DisplayLayout.Override.GroupBySummaryValueAppearance = appearance12;
            appearance13.FontData.BoldAsString = "False";
            appearance13.FontData.Name = "Segoe UI";
            appearance13.FontData.SizeInPoints = 8F;
            appearance13.TextHAlignAsString = "Center";
            appearance13.TextVAlignAsString = "Middle";
            this.grdYesterdayEnd.DisplayLayout.Override.HeaderAppearance = appearance13;
            this.grdYesterdayEnd.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdYesterdayEnd.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.grdYesterdayEnd.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance14.TextHAlignAsString = "Right";
            appearance14.TextVAlignAsString = "Middle";
            this.grdYesterdayEnd.DisplayLayout.Override.RowAlternateAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.Black;
            appearance15.TextHAlignAsString = "Right";
            appearance15.TextVAlignAsString = "Middle";
            this.grdYesterdayEnd.DisplayLayout.Override.RowAppearance = appearance15;
            this.grdYesterdayEnd.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdYesterdayEnd.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdYesterdayEnd.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance16.BackColor = System.Drawing.Color.Transparent;
            appearance16.BorderColor = System.Drawing.Color.Transparent;
            appearance16.FontData.BoldAsString = "True";
            this.grdYesterdayEnd.DisplayLayout.Override.SelectedRowAppearance = appearance16;
            this.grdYesterdayEnd.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdYesterdayEnd.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdYesterdayEnd.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdYesterdayEnd.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdYesterdayEnd.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdYesterdayEnd.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdYesterdayEnd.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            appearance17.BorderColor = System.Drawing.Color.Transparent;
            this.grdYesterdayEnd.DisplayLayout.Override.SummaryFooterAppearance = appearance17;
            this.grdYesterdayEnd.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance18.BackColor = System.Drawing.Color.Transparent;
            appearance18.BorderColor = System.Drawing.Color.Transparent;
            appearance18.FontData.BoldAsString = "False";
            appearance18.ForeColor = System.Drawing.Color.White;
            this.grdYesterdayEnd.DisplayLayout.Override.SummaryValueAppearance = appearance18;
            appearance19.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdYesterdayEnd.DisplayLayout.Override.TemplateAddRowAppearance = appearance19;
            this.grdYesterdayEnd.DisplayLayout.PriorityScrolling = true;
            this.grdYesterdayEnd.DisplayLayout.RowConnectorColor = System.Drawing.Color.Transparent;
            this.grdYesterdayEnd.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdYesterdayEnd.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdYesterdayEnd.DisplayLayout.UseFixedHeaders = true;
            this.grdYesterdayEnd.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdYesterdayEnd.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdYesterdayEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdYesterdayEnd.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grdYesterdayEnd.Location = new System.Drawing.Point(3, 20);
            this.grdYesterdayEnd.Name = "grdYesterdayEnd";
            this.grdYesterdayEnd.Size = new System.Drawing.Size(456, 117);
            this.grdYesterdayEnd.TabIndex = 4;
            this.grdYesterdayEnd.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdYesterdayEnd.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdYesterdayEnd_InitializeRow);
            this.grdYesterdayEnd.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdYesterdayEnd_AfterSortChange);
            this.grdYesterdayEnd.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdYesterdayEnd_BeforeColumnChooserDisplayed);
            this.grdYesterdayEnd.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdYesterdayEnd_BeforeCustomRowFilterDialog);
            this.grdYesterdayEnd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdYesterdayEnd_MouseUp);
            this.grdYesterdayEnd.BeforeRowFilterDropDown += grd_BeforeRowFilterDropDown;
            this.grdYesterdayEnd.AfterRowFilterChanged += grdYesterdayEnd_AfterRowFilterChanged;
            // 
            // toolStripMenuItemSaveLayout
            // 
            this.contextMenuStripItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.contextMenuStripItems.Name = "menuStripCashValue";
            this.contextMenuStripItems.Size = new System.Drawing.Size(108, 48);
            this.inboxControlStyler1.SetStyleSettings(this.contextMenuStripItems, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // toolStripItemSave
            // 
            this.saveLayoutToolStripMenuItem.Name = "toolStripItemSave";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // pnltabDayEnd
            // 
            this.pnltabDayEnd.Controls.Add(this.tabCntlDayEndData);
            this.pnltabDayEnd.Controls.Add(this.lblTodayDayEnd);
            this.pnltabDayEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnltabDayEnd.Location = new System.Drawing.Point(0, 0);
            this.pnltabDayEnd.Name = "pnltabDayEnd";
            this.pnltabDayEnd.Size = new System.Drawing.Size(626, 140);
            this.inboxControlStyler1.SetStyleSettings(this.pnltabDayEnd, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.pnltabDayEnd.TabIndex = 109;
            // 
            // tabCntlDayEndData
            // 
            appearance20.FontData.SizeInPoints = 9F;
            this.tabCntlDayEndData.Appearance = appearance20;
            appearance21.FontData.SizeInPoints = 9F;
            this.tabCntlDayEndData.ClientAreaAppearance = appearance21;
            this.tabCntlDayEndData.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabCntlDayEndData.Controls.Add(this.ultraTabPageControl1);
            this.tabCntlDayEndData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCntlDayEndData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance22.FontData.SizeInPoints = 9F;
            this.tabCntlDayEndData.HotTrackAppearance = appearance22;
            this.tabCntlDayEndData.Location = new System.Drawing.Point(0, 0);
            this.tabCntlDayEndData.Name = "tabCntlDayEndData";
            appearance23.FontData.SizeInPoints = 9F;
            this.tabCntlDayEndData.PressedCloseButtonAppearance = appearance23;
            appearance24.FontData.SizeInPoints = 9F;
            this.tabCntlDayEndData.ScrollButtonAppearance = appearance24;
            appearance25.FontData.SizeInPoints = 9F;
            this.tabCntlDayEndData.ScrollTrackAppearance = appearance25;
            this.tabCntlDayEndData.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabCntlDayEndData.Size = new System.Drawing.Size(626, 140);
            appearance26.FontData.SizeInPoints = 9F;
            this.tabCntlDayEndData.TabHeaderAreaAppearance = appearance26;
            this.tabCntlDayEndData.TabIndex = 104;
            ultraTab2.TabPage = this.ultraTabPageControl1;
            ultraTab2.Text = "Day End Cash";
            this.tabCntlDayEndData.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab2});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(622, 112);
            // 
            // lblTodayDayEnd
            // 
            this.lblTodayDayEnd.AutoSize = true;
            this.lblTodayDayEnd.Location = new System.Drawing.Point(3, 5);
            this.lblTodayDayEnd.Name = "lblTodayDayEnd";
            this.lblTodayDayEnd.Size = new System.Drawing.Size(81, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblTodayDayEnd, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblTodayDayEnd.TabIndex = 103;
            this.lblTodayDayEnd.Text = "Today Day End";
            // 
            // ultraExpnGrpBxAllTransactions
            // 
            appearance47.FontData.SizeInPoints = 9F;
            this.ultraExpnGrpBxAllTransactions.Appearance = appearance47;
            this.ultraExpnGrpBxAllTransactions.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.ultraExpnGrpBxAllTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpnGrpBxAllTransactions.ExpandedSize = new System.Drawing.Size(1092, 244);
            this.ultraExpnGrpBxAllTransactions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraExpnGrpBxAllTransactions.Location = new System.Drawing.Point(0, 0);
            this.ultraExpnGrpBxAllTransactions.Name = "ultraExpnGrpBxAllTransactions";
            this.ultraExpnGrpBxAllTransactions.Size = new System.Drawing.Size(1092, 244);
            this.ultraExpnGrpBxAllTransactions.TabIndex = 108;
            this.ultraExpnGrpBxAllTransactions.Text = "Transaction Details";
            this.ultraExpnGrpBxAllTransactions.ExpandedStateChanged += new System.EventHandler(this.ultraExpnGrpBxAllTransactions_ExpandedStateChanged);
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.grdGetLots);
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(3, 20);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(1086, 221);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // grdGetLots
            // 
            this.grdGetLots.ContextMenuStrip = this.contextMenuStripItems;
            appearance28.BackColor = System.Drawing.Color.Black;
            this.grdGetLots.DisplayLayout.Appearance = appearance28;
            this.grdGetLots.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdGetLots.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdGetLots.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.EmptyRowSettings.Style = Infragistics.Win.UltraWinGrid.EmptyRowStyle.HideRowSelector;
            this.grdGetLots.DisplayLayout.MaxColScrollRegions = 1;
            this.grdGetLots.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.Color.LightSlateGray;
            appearance29.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance29.BorderColor = System.Drawing.Color.DimGray;
            appearance29.FontData.BoldAsString = "True";
            appearance29.ForeColor = System.Drawing.Color.White;
            this.grdGetLots.DisplayLayout.Override.ActiveRowAppearance = appearance29;
            this.grdGetLots.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdGetLots.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdGetLots.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdGetLots.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdGetLots.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdGetLots.DisplayLayout.Override.BorderStyleRowSelector = Infragistics.Win.UIElementBorderStyle.None;
            this.grdGetLots.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdGetLots.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
            this.grdGetLots.DisplayLayout.Override.CellPadding = 0;
            this.grdGetLots.DisplayLayout.Override.CellSpacing = 0;
            this.grdGetLots.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            appearance30.BackColor = System.Drawing.Color.Gray;
            appearance30.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance30.BorderColor = System.Drawing.Color.Black;
            appearance30.FontData.BoldAsString = "True";
            appearance30.ForeColor = System.Drawing.Color.White;
            this.grdGetLots.DisplayLayout.Override.GroupByRowAppearance = appearance30;
            this.grdGetLots.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
            this.grdGetLots.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.SummaryCells;
            appearance31.BorderColor = System.Drawing.Color.Transparent;
            appearance31.TextHAlignAsString = "Right";
            appearance31.TextVAlignAsString = "Middle";
            this.grdGetLots.DisplayLayout.Override.GroupBySummaryValueAppearance = appearance31;
            appearance32.FontData.BoldAsString = "False";
            appearance32.FontData.Name = "Segoe UI";
            appearance32.FontData.SizeInPoints = 8F;
            appearance32.TextHAlignAsString = "Center";
            appearance32.TextVAlignAsString = "Middle";
            this.grdGetLots.DisplayLayout.Override.HeaderAppearance = appearance32;
            this.grdGetLots.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdGetLots.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.grdGetLots.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance33.TextHAlignAsString = "Right";
            appearance33.TextVAlignAsString = "Middle";
            this.grdGetLots.DisplayLayout.Override.RowAlternateAppearance = appearance33;
            appearance34.BackColor = System.Drawing.Color.Black;
            appearance34.ForeColor = System.Drawing.Color.White;
            appearance34.TextHAlignAsString = "Right";
            appearance34.TextVAlignAsString = "Middle";
            this.grdGetLots.DisplayLayout.Override.RowAppearance = appearance34;
            this.grdGetLots.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdGetLots.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance35.BackColor = System.Drawing.Color.Transparent;
            appearance35.BorderColor = System.Drawing.Color.Transparent;
            appearance35.FontData.BoldAsString = "True";
            this.grdGetLots.DisplayLayout.Override.SelectedRowAppearance = appearance35;
            this.grdGetLots.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdGetLots.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdGetLots.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdGetLots.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdGetLots.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdGetLots.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdGetLots.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            appearance36.BorderColor = System.Drawing.Color.Transparent;
            this.grdGetLots.DisplayLayout.Override.SummaryFooterAppearance = appearance36;
            this.grdGetLots.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance37.BackColor = System.Drawing.Color.Transparent;
            appearance37.BorderColor = System.Drawing.Color.Transparent;
            appearance37.FontData.BoldAsString = "False";
            appearance37.ForeColor = System.Drawing.Color.White;
            this.grdGetLots.DisplayLayout.Override.SummaryValueAppearance = appearance37;
            appearance38.TextHAlignAsString = "Center";
            this.grdGetLots.DisplayLayout.Override.TemplateAddRowAppearance = appearance38;
            this.grdGetLots.DisplayLayout.PriorityScrolling = true;
            this.grdGetLots.DisplayLayout.RowConnectorColor = System.Drawing.Color.Transparent;
            this.grdGetLots.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdGetLots.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdGetLots.DisplayLayout.UseFixedHeaders = true;
            this.grdGetLots.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdGetLots.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdGetLots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdGetLots.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grdGetLots.Location = new System.Drawing.Point(0, 0);
            this.grdGetLots.Name = "grdGetLots";
            this.grdGetLots.Size = new System.Drawing.Size(1086, 221);
            this.grdGetLots.TabIndex = 6;
            this.grdGetLots.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdGetLots_InitializeLayout);
            this.grdGetLots.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdGetLots_InitializeRow);
            this.grdGetLots.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdGetLots_InitializeGroupByRow);
            this.grdGetLots.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdGetLots_AfterSortChange);
            this.grdGetLots.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdGetLots_BeforeColumnChooserDisplayed);
            this.grdGetLots.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdGetLots_BeforeCustomRowFilterDialog);
            this.grdGetLots.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdGetLots_MouseUp);
            this.grdGetLots.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(grdGetLots_DoubleClickRow);
            this.grdGetLots.AfterRowFilterChanged += grdGetLots_AfterRowFilterChanged;
            this.grdGetLots.BeforeRowFilterDropDown += grd_BeforeRowFilterDropDown;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 435);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1092, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 111;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // ugbxDayEndParams
            // 
            this.ugbxDayEndParams.Controls.Add(this.ctrlMasterFundAndAccountsDropdown1);
            this.ugbxDayEndParams.Controls.Add(this.dtFromDate);
            this.ugbxDayEndParams.Controls.Add(this.btnSave);
            this.ugbxDayEndParams.Controls.Add(this.lblDate);
            this.ugbxDayEndParams.Controls.Add(this.dtToDate);
            this.ugbxDayEndParams.Controls.Add(this.ultraLabel1);
            this.ugbxDayEndParams.Controls.Add(this.btnRunBatch);
            this.ugbxDayEndParams.Controls.Add(this.btnGet);
            this.ugbxDayEndParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.ugbxDayEndParams.Location = new System.Drawing.Point(0, 0);
            this.ugbxDayEndParams.Name = "ugbxDayEndParams";
            this.ugbxDayEndParams.Size = new System.Drawing.Size(1092, 46);
            this.ugbxDayEndParams.TabIndex = 112;
            // 
            // ctrlMasterFundAndAccountsDropdown1
            // 
            this.ctrlMasterFundAndAccountsDropdown1.Location = new System.Drawing.Point(6, 4);
            this.ctrlMasterFundAndAccountsDropdown1.Name = "ctrlMasterFundAndAccountsDropdown1";
            this.ctrlMasterFundAndAccountsDropdown1.Size = new System.Drawing.Size(482, 36);
            this.ctrlMasterFundAndAccountsDropdown1.TabIndex = 103;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.splitContainer2);
            this.ultraPanel1.ClientArea.Controls.Add(this.statusStrip1);
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxDayEndParams);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1092, 457);
            this.ultraPanel1.TabIndex = 105;
            // 
            // exportCurrentToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportCurrentToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportCurrentToolStripMenuItem_Click);
            // 
            // ctrlCashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ultraPanel1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "ctrlCashForm";
            this.Size = new System.Drawing.Size(1092, 457);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.CashForm_Load);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.menuStripGetLots.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlYesterdayDayEnd)).EndInit();
            this.pnlYesterdayDayEnd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdYesterdayEnd)).EndInit();
            this.contextMenuStripItems.ResumeLayout(false);
            this.pnltabDayEnd.ResumeLayout(false);
            this.pnltabDayEnd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabCntlDayEndData)).EndInit();
            this.tabCntlDayEndData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpnGrpBxAllTransactions)).EndInit();
            this.ultraExpnGrpBxAllTransactions.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdGetLots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxDayEndParams)).EndInit();
            this.ugbxDayEndParams.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip menuStripGetLots;
        private System.Windows.Forms.ToolStripMenuItem addRowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private Infragistics.Win.Misc.UltraButton btnRunBatch;
        private Infragistics.Win.Misc.UltraLabel lblDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDate;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnGet;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdYesterdayEnd;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdGetLots;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpnGrpBxAllTransactions;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private Prana.CashManagement.Controls.ctrlDataGrid ctrlDayEndDataGrid1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripItems;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraGroupBox pnlYesterdayDayEnd;
        private System.Windows.Forms.Panel pnltabDayEnd;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCntlDayEndData;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private System.Windows.Forms.Label lblTodayDayEnd;
        private Infragistics.Win.Misc.UltraGroupBox ugbxDayEndParams;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private Controls.ctrlMasterFundAndAccountsDropdown ctrlMasterFundAndAccountsDropdown1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
    }
}