using Prana.ClientCommon;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.PM.Client.UI.Controls
{
    partial class ctrlExpiryandSettlementNew
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> /
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
                if (_pricingServicesProxy != null)
                {
                    _pricingServicesProxy.Dispose();
                }
                if (headerCheckBoxUnExpired != null)
                {
                    headerCheckBoxUnExpired.Dispose();
                }
                if (headerCheckBoxUnWind != null)
                {
                    headerCheckBoxUnWind.Dispose();
                }
                if (myerror != null)
                {
                    myerror.Dispose();
                }
                if (_allocationServices != null)
                {
                    _allocationServices.Dispose();
                }
                if (_closingServices != null)
                {
                    _closingServices.Dispose();
                }
                if (ctrlCreateAndImportPosition1 != null)
                {
                    ctrlCreateAndImportPosition1.Dispose();
                    ctrlCreateAndImportPosition1 = null;
                }
                if (_grdBandNetPositions != null)
                {
                    _grdBandNetPositions.Dispose();
                }
                if (_bgExpireAndSettle != null)
                {
                    _bgExpireAndSettle.Dispose();
                }
                if (_bgUnwind != null)
                {
                    _bgUnwind.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctrlExpiryandSettlementNew));
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
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.grpUnexpired = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.grdAccountUnexpired = new PranaUltraGrid();
            this.menuUnexpired = new System.Windows.Forms.ContextMenu();
            this.menuCashSettlement = new System.Windows.Forms.MenuItem();
            this.menuCashSettlementAtCost = new System.Windows.Forms.MenuItem();
            this.menuCashSettlementAtZeroPrice = new System.Windows.Forms.MenuItem();
            this.menuCashSettlementAtClosingDateSpotPx = new System.Windows.Forms.MenuItem();
            //this.menuDeliverFXAtCost = new System.Windows.Forms.MenuItem();
            //this.menuDeliverFXAtCostandPNLAtClosingDateSpotPx = new System.Windows.Forms.MenuItem();
            this.menuPhysicalSettlement = new System.Windows.Forms.MenuItem();
            this.menuExpire = new System.Windows.Forms.MenuItem();
            this.menuExercise = new System.Windows.Forms.MenuItem();
            this.menuExerciseAssign = new System.Windows.Forms.MenuItem();
            this.saveLayoutMenuItem = new System.Windows.Forms.MenuItem();
            this.clearFiltersForUnexpiredMenuItem = new System.Windows.Forms.MenuItem();
            this.menuSwapRolloverandExpire = new System.Windows.Forms.MenuItem();
            this.menuSwapExpire = new System.Windows.Forms.MenuItem();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this.grpCreatePosition = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel4 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ctrlCreateAndImportPosition1 = new Prana.PM.Client.UI.Controls.CtrlCreateAndImportPosition();
            this.grdCashandExpire = new PranaUltraGrid();
            this.ctrlSwapClosing1 = new Prana.ClientCommon.CtrlSwapClosing();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnExpireSettle = new Infragistics.Win.Misc.UltraButton();
            this.btnAutoExercise = new Infragistics.Win.Misc.UltraButton();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.grpExpired = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel3 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.grdAccountExpired = new PranaUltraGrid();
            this.menuExpired = new System.Windows.Forms.ContextMenu();
            this.menuUnwind = new System.Windows.Forms.MenuItem();
            this.clearFiltersForExpiredGridMenuItem = new System.Windows.Forms.MenuItem();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.chkCopyOpeningTradeAttributes = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.grpUnexpired)).BeginInit();
            this.grpUnexpired.SuspendLayout();
            this.ultraExpandableGroupBoxPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountUnexpired)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCreatePosition)).BeginInit();
            this.grpCreatePosition.SuspendLayout();
            this.ultraExpandableGroupBoxPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCashandExpire)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpExpired)).BeginInit();
            this.grpExpired.SuspendLayout();
            this.ultraExpandableGroupBoxPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountExpired)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCopyOpeningTradeAttributes)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // grpUnexpired
            // 
            this.grpUnexpired.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.grpUnexpired.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpUnexpired.ExpandedSize = new System.Drawing.Size(940, 259);
            this.grpUnexpired.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.DownArrow1;
            this.grpUnexpired.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.UpArrow1;
            this.grpUnexpired.Location = new System.Drawing.Point(0, 25);
            this.grpUnexpired.Name = "grpUnexpired";
            this.grpUnexpired.Size = new System.Drawing.Size(940, 259);
            this.grpUnexpired.TabIndex = 3;
            this.grpUnexpired.Text = "UnExpired/Unsettled";
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.grdAccountUnexpired);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.splitter4);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.grpCreatePosition);
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(934, 237);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // grdAccountUnexpired
            // 
            this.grdAccountUnexpired.ContextMenu = this.menuUnexpired;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdAccountUnexpired.DisplayLayout.Appearance = appearance1;
            this.grdAccountUnexpired.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdAccountUnexpired.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccountUnexpired.DisplayLayout.GroupByBox.Hidden = true;
            this.grdAccountUnexpired.DisplayLayout.MaxBandDepth = 6;
            this.grdAccountUnexpired.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAccountUnexpired.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.LightSlateGray;
            appearance2.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance2.BorderColor = System.Drawing.Color.DimGray;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.grdAccountUnexpired.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.grdAccountUnexpired.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdAccountUnexpired.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccountUnexpired.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccountUnexpired.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccountUnexpired.DisplayLayout.Override.CellPadding = 0;
            this.grdAccountUnexpired.DisplayLayout.Override.CellSpacing = 0;
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8F;
            appearance3.TextHAlignAsString = "Center";
            this.grdAccountUnexpired.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdAccountUnexpired.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdAccountUnexpired.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.grdAccountUnexpired.DisplayLayout.Override.RowAlternateAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.Black;
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Right";
            appearance5.TextVAlignAsString = "Middle";
            this.grdAccountUnexpired.DisplayLayout.Override.RowAppearance = appearance5;
            this.grdAccountUnexpired.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccountUnexpired.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            appearance6.FontData.BoldAsString = "True";
            this.grdAccountUnexpired.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.grdAccountUnexpired.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccountUnexpired.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccountUnexpired.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdAccountUnexpired.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            this.grdAccountUnexpired.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdAccountUnexpired.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdAccountUnexpired.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance7.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdAccountUnexpired.DisplayLayout.Override.TemplateAddRowAppearance = appearance7;
            this.grdAccountUnexpired.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAccountUnexpired.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAccountUnexpired.DisplayLayout.UseFixedHeaders = true;
            this.grdAccountUnexpired.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAccountUnexpired.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAccountUnexpired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.grdAccountUnexpired.Location = new System.Drawing.Point(0, 0);
            this.grdAccountUnexpired.Name = "grdAccountUnexpired";
            this.grdAccountUnexpired.Size = new System.Drawing.Size(934, 213);
            this.grdAccountUnexpired.SyncWithCurrencyManager = false;
            this.grdAccountUnexpired.TabIndex = 4;
            this.grdAccountUnexpired.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate;
            this.grdAccountUnexpired.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccountUnexpired.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdAccountUnexpired_AfterCellUpdate);
            this.grdAccountUnexpired.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdAccountUnexpired_InitializeLayout);
            this.grdAccountUnexpired.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdAccountUnexpired_InitializeRow);
            this.grdAccountUnexpired.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdAccountUnexpired_CellChange);
            this.grdAccountUnexpired.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.grdAccountUnexpired_ClickCell);
            this.grdAccountUnexpired.FilterRow += new Infragistics.Win.UltraWinGrid.FilterRowEventHandler(this.grdAccountUnexpired_FilterRow);
            this.grdAccountUnexpired.AfterRowFilterChanged += new Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventHandler(this.grdAccountUnexpired_AfterRowFilterChanged);
            this.grdAccountUnexpired.BeforeRowFilterDropDown += new Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventHandler(this.grd_BeforeRowFilterDropDown);
            this.grdAccountUnexpired.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdAccountUnexpired_BeforeColumnChooserDisplayed);
            this.grdAccountExpired.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdAccountExpired_BeforeCustomRowFilterDialog);
            this.grdAccountUnexpired.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdAccountUnexpired_BeforeCustomRowFilterDialog);
            this.grdCashandExpire.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdCashandExpire_BeforeCustomRowFilterDialog);
            // 
            // saveLayoutMenuItem
            // 
            this.saveLayoutMenuItem.Name = "saveLayoutMenuItem";
            this.saveLayoutMenuItem.Text = "Save Layout";
            this.saveLayoutMenuItem.Click += new System.EventHandler(this.saveLayoutMenuItem_Click);
            // 
            //clearFilter
            // 
            this.clearFiltersForUnexpiredMenuItem.Index = 7;
            this.clearFiltersForUnexpiredMenuItem.Name = "clearFiltersMenuItem";
            this.clearFiltersForUnexpiredMenuItem.Text = "Clear Filters";
            this.clearFiltersForUnexpiredMenuItem.Click += new System.EventHandler(this.ClearAccountUnexpiredGridFiltersStripMenuItem_Click);
            // 
            
            // 
            // menuUnexpired
            // 
            this.menuUnexpired.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.clearFiltersForUnexpiredMenuItem,
            this.saveLayoutMenuItem,
            this.menuCashSettlement,
            this.menuPhysicalSettlement,
            this.menuExpire,
            this.menuExercise,
            this.menuExerciseAssign,
            this.menuSwapRolloverandExpire,
            this.menuSwapExpire});
            this.menuUnexpired.Popup += new System.EventHandler(this.menuUnexpired_Popup);
            // 
            // menuCashSettlement
            // 
            this.menuCashSettlement.Index = 0;
            this.menuCashSettlement.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuCashSettlementAtCost,
            this.menuCashSettlementAtZeroPrice,
            this.menuCashSettlementAtClosingDateSpotPx});
            //this.menuDeliverFXAtCost,
            //this.menuDeliverFXAtCostandPNLAtClosingDateSpotPx});
            this.menuCashSettlement.Text = "Cash Settlement";
            // 
            // menuCashSettlementAtCost
            // 
            this.menuCashSettlementAtCost.Index = 0;
            this.menuCashSettlementAtCost.Text = "Cash Settlement At Cost";
            this.menuCashSettlementAtCost.Click += new System.EventHandler(this.menuCashSettlementAtCost_Click);
            // 
            // menuCashSettlementAtZeroPrice
            // 
            this.menuCashSettlementAtZeroPrice.Index = 1;
            this.menuCashSettlementAtZeroPrice.Text = "Cash Settlement At Zero Price";
            this.menuCashSettlementAtZeroPrice.Click += new System.EventHandler(this.menuCashSettlementAtZeroPrice_Click);
            // 
            // menuCashSettlementAtClosingDateSpotPx
            // 
            this.menuCashSettlementAtClosingDateSpotPx.Index = 2;
            this.menuCashSettlementAtClosingDateSpotPx.Text = "Cash Settlement At Closing Date Spot Px";
            this.menuCashSettlementAtClosingDateSpotPx.Click += new System.EventHandler(this.menuCashSettlementAtClosingDateSpotPx_Click);
            // 
            // menuDeliverFXAtCost
            // 
            //this.menuDeliverFXAtCost.Index = 3;
            //this.menuDeliverFXAtCost.Text = "Deliver FX At Cost";
            //this.menuDeliverFXAtCost.Click += new System.EventHandler(this.menuDeliverFXAtCost_Click);
            // 
            // menuDeliverFXAtCostandPNLAtClosingDateSpotPx
            // 
            //this.menuDeliverFXAtCostandPNLAtClosingDateSpotPx.Index = 4;
            //this.menuDeliverFXAtCostandPNLAtClosingDateSpotPx.Text = "Deliver FX At Cost, PNL At Closing Date Spot Px";
            //this.menuDeliverFXAtCostandPNLAtClosingDateSpotPx.Click += new System.EventHandler(this.menuDeliverFXAtCostandPNLAtClosingDateSpotPx_Click);
            // 
            // menuPhysicalSettlement
            // 
            this.menuPhysicalSettlement.Index = 1;
            this.menuPhysicalSettlement.Text = "Physical Settlement";
            this.menuPhysicalSettlement.Click += new System.EventHandler(this.menuPhysicalSettlement_Click);
            // 
            // menuExpire
            // 
            this.menuExpire.Index = 2;
            this.menuExpire.Text = "Expire";
            this.menuExpire.Click += new System.EventHandler(this.menuExpire_Click);
            // 
            // menuExercise
            // 
            this.menuExercise.Index = 3;
            this.menuExercise.Text = "Exercise/Assignment";
            this.menuExercise.Click += new System.EventHandler(this.menuExercise_Click);
            // 
            // menuExerciseAssign
            // 
            this.menuExerciseAssign.Index = 4;
            this.menuExerciseAssign.Text = "Exercise/Assignment at Zero";
            this.menuExerciseAssign.Click += new System.EventHandler(this.menuExerciseAssign_Click);
            // 
            // menuSwapRolloverandExpire
            // 
            this.menuSwapRolloverandExpire.Index = 5;
            this.menuSwapRolloverandExpire.Text = "Swap Expire and Rollover";
            this.menuSwapRolloverandExpire.Click += new System.EventHandler(this.menuSwapRolloverandExpire_Click);
            // 
            // menuSwapExpire
            // 
            this.menuSwapExpire.Index = 6;
            this.menuSwapExpire.Text = "Swap Expire";
            this.menuSwapExpire.Click += new System.EventHandler(this.menuSwapExpire_Click);
            // 
            // splitter4
            // 
            this.splitter4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter4.Location = new System.Drawing.Point(0, 213);
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size(934, 3);
            this.inboxControlStyler1.SetStyleSettings(this.splitter4, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitter4.TabIndex = 2;
            this.splitter4.TabStop = false;
            // 
            // grpCreatePosition
            // 
            this.grpCreatePosition.Controls.Add(this.ultraExpandableGroupBoxPanel4);
            this.grpCreatePosition.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpCreatePosition.Expanded = false;
            this.grpCreatePosition.ExpandedSize = new System.Drawing.Size(934, 50);
            this.grpCreatePosition.Location = new System.Drawing.Point(0, 216);
            this.grpCreatePosition.Name = "grpCreatePosition";
            this.grpCreatePosition.Size = new System.Drawing.Size(934, 21);
            this.grpCreatePosition.TabIndex = 1;
            this.grpCreatePosition.Text = "Create Transaction";
            // 
            // ultraExpandableGroupBoxPanel4
            // 
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.ctrlCreateAndImportPosition1);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.grdCashandExpire);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.ctrlSwapClosing1);
            this.ultraExpandableGroupBoxPanel4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraExpandableGroupBoxPanel4.Name = "ultraExpandableGroupBoxPanel4";
            this.ultraExpandableGroupBoxPanel4.Size = new System.Drawing.Size(928, 106);
            this.ultraExpandableGroupBoxPanel4.TabIndex = 0;
            this.ultraExpandableGroupBoxPanel4.Visible = false;
            ultraExpandableGroupBoxPanel4.AutoScroll = true;
            // 
            // ctrlCreateAndImportPosition1
            // 
            this.ctrlCreateAndImportPosition1.AddButtonClicked = false;
            this.ctrlCreateAndImportPosition1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCreateAndImportPosition1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ctrlCreateAndImportPosition1.IsCloseTradePopup = false;
            this.ctrlCreateAndImportPosition1.Location = new System.Drawing.Point(0, 0);
            this.ctrlCreateAndImportPosition1.Name = "ctrlCreateAndImportPosition1";
            this.ctrlCreateAndImportPosition1.NetPositions = ((Prana.BusinessObjects.PositionManagement.NetPositionList)(resources.GetObject("ctrlCreateAndImportPosition1.NetPositions")));
            this.ctrlCreateAndImportPosition1.OTCPositionList = ((Prana.PM.BLL.OTCPositionList)(resources.GetObject("ctrlCreateAndImportPosition1.OTCPositionList")));
            this.ctrlCreateAndImportPosition1.Size = new System.Drawing.Size(928, 106);
            this.ctrlCreateAndImportPosition1.TabIndex = 0;
            //
            //grdCashandExpire
            //
            appearance8.BackColor = System.Drawing.Color.Black;
            appearance8.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdCashandExpire.DisplayLayout.Appearance = appearance8;
            this.grdCashandExpire.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdCashandExpire.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance9.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdCashandExpire.DisplayLayout.GroupByBox.Appearance = appearance9;
            appearance10.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdCashandExpire.DisplayLayout.GroupByBox.BandLabelAppearance = appearance10;
            this.grdCashandExpire.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance11.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance11.BackColor2 = System.Drawing.SystemColors.Control;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance11.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdCashandExpire.DisplayLayout.GroupByBox.PromptAppearance = appearance11;
            this.grdCashandExpire.DisplayLayout.MaxBandDepth = 6;
            this.grdCashandExpire.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCashandExpire.DisplayLayout.MaxRowScrollRegions = 1;
            appearance12.BackColor = System.Drawing.Color.Gold;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.grdCashandExpire.DisplayLayout.Override.ActiveRowAppearance = appearance12;
            this.grdCashandExpire.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdCashandExpire.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashandExpire.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashandExpire.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdCashandExpire.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            this.grdCashandExpire.DisplayLayout.Override.CardAreaAppearance = appearance13;
            appearance14.BorderColor = System.Drawing.Color.Silver;
            appearance14.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdCashandExpire.DisplayLayout.Override.CellAppearance = appearance14;
            this.grdCashandExpire.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdCashandExpire.DisplayLayout.Override.CellPadding = 0;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance15.TextHAlignAsString = "Left";
            this.grdCashandExpire.DisplayLayout.Override.HeaderAppearance = appearance15;
            this.grdCashandExpire.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCashandExpire.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance16.BorderColor = System.Drawing.Color.Transparent;
            appearance16.ForeColor = System.Drawing.Color.Gold;
            this.grdCashandExpire.DisplayLayout.Override.RowAlternateAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.Black;
            appearance17.BorderColor = System.Drawing.Color.Transparent;
            appearance17.ForeColor = System.Drawing.Color.Gold;
            this.grdCashandExpire.DisplayLayout.Override.RowAppearance = appearance17;
            this.grdCashandExpire.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdCashandExpire.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashandExpire.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.ExtendedAutoDrag;
            this.grdCashandExpire.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.ExtendedAutoDrag;
            this.grdCashandExpire.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.ExtendedAutoDrag;
            appearance18.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdCashandExpire.DisplayLayout.Override.TemplateAddRowAppearance = appearance18;
            this.grdCashandExpire.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCashandExpire.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCashandExpire.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCashandExpire.Location = new System.Drawing.Point(0, 0);
            this.grdCashandExpire.Name = "grdCashandExpire";
            this.grdCashandExpire.Size = new System.Drawing.Size(928, 106);
            this.grdCashandExpire.SyncWithCurrencyManager = false;
            this.grdCashandExpire.TabIndex = 6;
            this.grdCashandExpire.Text = "grdCashandExpire";
            this.grdCashandExpire.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCashandExpire_AfterCellUpdate);
            this.grdCashandExpire.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCashandExpire_InitializeLayout);
            this.grdCashandExpire.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.grdCashandExpire_ClickCell);
            this.grdCashandExpire.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdCashandExpire_BeforeColumnChooserDisplayed);
            
            // 
            // ctrlSwapClosing1
            // 
            this.ctrlSwapClosing1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlSwapClosing1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ctrlSwapClosing1.Location = new System.Drawing.Point(0, 0);
            this.ctrlSwapClosing1.Name = "ctrlSwapClosing1";
            this.ctrlSwapClosing1.Size = new System.Drawing.Size(928, 106);
            this.ctrlSwapClosing1.TabIndex = 0;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 284);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(940, 3);
            this.inboxControlStyler1.SetStyleSettings(this.splitter2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitter2.TabIndex = 4;
            this.splitter2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnExpireSettle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 287);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(940, 26);
            this.inboxControlStyler1.SetStyleSettings(this.panel1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnAutoExercise);
            this.panel2.Controls.Add(this.chkCopyOpeningTradeAttributes);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(940, 26);
            this.inboxControlStyler1.SetStyleSettings(this.panel2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.panel2.TabIndex = 6;
            // 
            // btnCancel
            // 
            //this.btnCancel.Image = global::Prana.PM.Client.UI.Properties.Resources.btn_cancel;
            this.btnCancel.Location = new System.Drawing.Point(540, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 26);
            this.inboxControlStyler1.SetStyleSettings(this.btnCancel, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            //this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnExpireSettle
            // 
            //this.btnExpireSettle.Image = global::Prana.PM.Client.UI.Properties.Resources.btn_save;
            this.btnExpireSettle.Location = new System.Drawing.Point(433, 1);
            this.btnExpireSettle.Name = "btnExpireSettle";
            this.btnExpireSettle.Size = new System.Drawing.Size(83, 26);
            this.inboxControlStyler1.SetStyleSettings(this.btnExpireSettle, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnExpireSettle.TabIndex = 1;
            this.btnExpireSettle.Text = "Save";
            //this.btnExpireSettle.UseVisualStyleBackColor = true;
            this.btnExpireSettle.Click += new System.EventHandler(this.btnExpireSettle_Click);
            // 
            // btnAutoExercise
            // 
            this.btnAutoExercise.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;
            this.btnAutoExercise.Location = new System.Drawing.Point(5, 5);
            this.btnAutoExercise.Name = "btnAutoExercise";
            this.btnAutoExercise.Size = new System.Drawing.Size(100, 21);
            this.btnAutoExercise.TabIndex = 3;
            this.btnAutoExercise.Text = "Auto Exercise";
            this.btnAutoExercise.Click += new System.EventHandler(this.btnAutoExercise_Click);
            // 
            // chkCopyOpeningTradeAttributes
            // 
            this.chkCopyOpeningTradeAttributes.Location = new System.Drawing.Point(120, 5);
            this.chkCopyOpeningTradeAttributes.Name = "chkCopyOpeningTradeAttributes";
            this.chkCopyOpeningTradeAttributes.Size = new System.Drawing.Size(290, 17);
            this.chkCopyOpeningTradeAttributes.TabIndex = 117;
            this.chkCopyOpeningTradeAttributes.Text = "Copy Opening Trade Attributes to Closing Trades";
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter3.Location = new System.Drawing.Point(0, 313);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(940, 3);
            this.inboxControlStyler1.SetStyleSettings(this.splitter3, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitter3.TabIndex = 6;
            this.splitter3.TabStop = false;
            // 
            // grpExpired
            // 
            this.grpExpired.Controls.Add(this.ultraExpandableGroupBoxPanel3);
            this.grpExpired.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpExpired.ExpandedSize = new System.Drawing.Size(940, 347);
            this.grpExpired.ExpansionIndicatorCollapsed = global::Prana.PM.Client.UI.Properties.Resources.DownArrow1;
            this.grpExpired.ExpansionIndicatorExpanded = global::Prana.PM.Client.UI.Properties.Resources.UpArrow1;
            this.grpExpired.Location = new System.Drawing.Point(0, 316);
            this.grpExpired.Name = "grpExpired";
            this.grpExpired.Size = new System.Drawing.Size(940, 347);
            this.grpExpired.TabIndex = 8;
            this.grpExpired.Text = "Expired/Settled";
            this.grpExpired.ExpandedStateChanged += new System.EventHandler(this.grpExpired_ExpandedStateChanged);
            // 
            // ultraExpandableGroupBoxPanel3
            // 
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.grdAccountExpired);
            this.ultraExpandableGroupBoxPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel3.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanel3.Name = "ultraExpandableGroupBoxPanel3";
            this.ultraExpandableGroupBoxPanel3.Size = new System.Drawing.Size(934, 325);
            this.ultraExpandableGroupBoxPanel3.TabIndex = 0;
            // 
            // grdAccountExpired
            // 
            this.grdAccountExpired.ContextMenu = this.menuExpired;
            appearance19.BackColor = System.Drawing.Color.Black;
            this.grdAccountExpired.DisplayLayout.Appearance = appearance19;
            this.grdAccountExpired.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdAccountExpired.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccountExpired.DisplayLayout.GroupByBox.Hidden = true;
            this.grdAccountExpired.DisplayLayout.MaxBandDepth = 6;
            this.grdAccountExpired.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAccountExpired.DisplayLayout.MaxRowScrollRegions = 1;
            appearance20.BackColor = System.Drawing.Color.LightSlateGray;
            appearance20.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance20.BorderColor = System.Drawing.Color.DimGray;
            appearance20.FontData.BoldAsString = "True";
            appearance20.ForeColor = System.Drawing.Color.White;
            this.grdAccountExpired.DisplayLayout.Override.ActiveRowAppearance = appearance20;
            this.grdAccountExpired.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdAccountExpired.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccountExpired.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccountExpired.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccountExpired.DisplayLayout.Override.CellPadding = 0;
            this.grdAccountExpired.DisplayLayout.Override.CellSpacing = 0;
            appearance21.FontData.Name = "Tahoma";
            appearance21.FontData.SizeInPoints = 8F;
            appearance21.TextHAlignAsString = "Center";
            this.grdAccountExpired.DisplayLayout.Override.HeaderAppearance = appearance21;
            this.grdAccountExpired.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdAccountExpired.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance22.ForeColor = System.Drawing.Color.White;
            appearance22.TextHAlignAsString = "Right";
            appearance22.TextVAlignAsString = "Middle";
            this.grdAccountExpired.DisplayLayout.Override.RowAlternateAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.Black;
            appearance23.ForeColor = System.Drawing.Color.White;
            appearance23.TextHAlignAsString = "Right";
            appearance23.TextVAlignAsString = "Middle";
            this.grdAccountExpired.DisplayLayout.Override.RowAppearance = appearance23;
            this.grdAccountExpired.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccountExpired.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance24.BackColor = System.Drawing.Color.Transparent;
            appearance24.BorderColor = System.Drawing.Color.Transparent;
            appearance24.FontData.BoldAsString = "True";
            this.grdAccountExpired.DisplayLayout.Override.SelectedRowAppearance = appearance24;
            this.grdAccountExpired.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccountExpired.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccountExpired.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdAccountExpired.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccountExpired.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdAccountExpired.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            appearance25.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdAccountExpired.DisplayLayout.Override.TemplateAddRowAppearance = appearance25;
            this.grdAccountExpired.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAccountExpired.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAccountExpired.DisplayLayout.UseFixedHeaders = true;
            this.grdAccountExpired.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAccountExpired.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAccountExpired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.grdAccountExpired.Location = new System.Drawing.Point(0, 0);
            this.grdAccountExpired.Name = "grdAccountExpired";
            this.grdAccountExpired.Size = new System.Drawing.Size(934, 325);
            this.grdAccountExpired.SyncWithCurrencyManager = false;
            this.grdAccountExpired.TabIndex = 5;
            this.grdAccountExpired.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdAccountExpired_InitializeLayout);
            this.grdAccountExpired.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdAccountExpired_BeforeColumnChooserDisplayed);
            this.grdAccountExpired.BeforeRowFilterDropDown += grd_BeforeRowFilterDropDown;
            this.grdAccountExpired.AfterRowFilterChanged += grdAccountExpired_AfterRowFilterChanged;
            // 
            // menuExpired
            // 
            this.menuExpired.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuUnwind,
            this.clearFiltersForExpiredGridMenuItem
            });
            this.menuExpired.Popup += new System.EventHandler(this.menuExpired_Popup);
            // 
            // menuUnwind     
            // 
            this.menuUnwind.Index = 0;
            this.menuUnwind.Text = "Unwind";
            this.menuUnwind.Click += new System.EventHandler(this.menuUnwind_Click);
            // 
            // menuUnwind     
            // 
            this.clearFiltersForExpiredGridMenuItem.Index = 1;
            this.clearFiltersForExpiredGridMenuItem.Text = "Clear Filters";
            this.clearFiltersForExpiredGridMenuItem.Click += new System.EventHandler(this.ClearAccountExpiredGridFiltersStripMenuItem_Click);
            // 
            // ctrlExpiryandSettlementNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpExpired);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.grpUnexpired);
            this.Controls.Add(this.panel2);
             this.Name = "ctrlExpiryandSettlementNew";
            this.Size = new System.Drawing.Size(940, 663);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.ctrlExpiryandSettlementNew_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpUnexpired)).EndInit();
            this.grpUnexpired.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountUnexpired)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCreatePosition)).EndInit();
            this.grpCreatePosition.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCashandExpire)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpExpired)).EndInit();
            this.grpExpired.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccountExpired)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCopyOpeningTradeAttributes)).EndInit();
            this.ResumeLayout(false);

        }


      
    


       

        

       

     

    
      
     

        

     

    

      
       
            
       
      
       
       
        #endregion

        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpUnexpired;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnExpireSettle;
        private System.Windows.Forms.Splitter splitter3;
        private Infragistics.Win.Misc.UltraButton btnAutoExercise;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpExpired;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel3;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpCreatePosition;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel4;
        private System.Windows.Forms.Splitter splitter4;
        private PranaUltraGrid grdAccountUnexpired;
        private PranaUltraGrid grdAccountExpired;
        private PranaUltraGrid grdCashandExpire;
        private System.Windows.Forms.ContextMenu menuUnexpired;
        private System.Windows.Forms.MenuItem menuPhysicalSettlement;
        private System.Windows.Forms.MenuItem menuExpire;
        private System.Windows.Forms.MenuItem menuExercise;
        private System.Windows.Forms.MenuItem menuExerciseAssign ;
        private System.Windows.Forms.MenuItem menuSwapRolloverandExpire;
        private System.Windows.Forms.MenuItem menuSwapExpire;
        private System.Windows.Forms.ContextMenu menuExpired;      
        private System.Windows.Forms.MenuItem menuUnwind;
        private System.Windows.Forms.MenuItem clearFiltersForExpiredGridMenuItem;
        private System.Windows.Forms.MenuItem menuCashSettlementAtCost;
        private System.Windows.Forms.MenuItem menuCashSettlementAtZeroPrice;
        private System.Windows.Forms.MenuItem menuCashSettlement;
        private System.Windows.Forms.MenuItem menuCashSettlementAtClosingDateSpotPx;
        private System.Windows.Forms.MenuItem saveLayoutMenuItem;
        private System.Windows.Forms.MenuItem clearFiltersForUnexpiredMenuItem;
        //private System.Windows.Forms.MenuItem menuDeliverFXAtCost;
        //private System.Windows.Forms.MenuItem menuDeliverFXAtCostandPNLAtClosingDateSpotPx;
        private Prana.PM.Client.UI.Controls.CtrlCreateAndImportPosition ctrlCreateAndImportPosition1;
        private Prana.ClientCommon.CtrlSwapClosing ctrlSwapClosing1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkCopyOpeningTradeAttributes;
    }
}
