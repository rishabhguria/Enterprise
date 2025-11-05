using Infragistics.Win.SupportDialogs.FilterUIProvider;
using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.Blotter
{
    partial class WorkingSubBlotterGrid
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
            UnwireEvents();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (subOrders != null)
            {
                subOrders.Close();
                subOrders = null;
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
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedBottom, new System.Guid("c1d7899b-6644-4fc3-9d36-3d2915669164"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("d4925b1d-21d6-460b-94bf-e2f54aff30f4"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("c1d7899b-6644-4fc3-9d36-3d2915669164"), -1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.dgBlotter = new PranaUltraGrid();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.menuAuditTrail = new System.Windows.Forms.MenuItem();
            this.menuTrade = new System.Windows.Forms.MenuItem();
            this.menuRepeatTrade = new System.Windows.Forms.MenuItem();
            this.menuCancel = new System.Windows.Forms.MenuItem();
            this.menuEditOrReplaceOrder = new System.Windows.Forms.MenuItem();
            this.mnuRemoveOrder = new System.Windows.Forms.MenuItem();
            this.menuTransferTrade = new System.Windows.Forms.MenuItem();
            this.menuOtherUsers = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuShowDetails = new System.Windows.Forms.MenuItem();
            this.menuAddFills = new System.Windows.Forms.MenuItem();
            this.menuDivider1 = new System.Windows.Forms.MenuItem();
            this.menuSaveLayout = new System.Windows.Forms.MenuItem();
            this.menuViewAllocation = new System.Windows.Forms.MenuItem();
            this.menuViewAllocationStagedOrder = new System.Windows.Forms.MenuItem();
            this.menuAllocate = new System.Windows.Forms.MenuItem();
            this.menuFilters = new System.Windows.Forms.MenuItem();
            this.menuDivider2 = new System.Windows.Forms.MenuItem();
            this.menuRemoveTab = new System.Windows.Forms.MenuItem();
            this.menuRenameTab = new System.Windows.Forms.MenuItem();
            this.menuRemoveFilter = new System.Windows.Forms.MenuItem();
            this.ultraDockManager1 = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._WorkingSubBlotterGridUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._WorkingSubBlotterGridUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._WorkingSubBlotterGridUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._WorkingSubBlotterGridUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._WorkingSubBlotterGridAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.btnExpansion = new Infragistics.Win.Misc.UltraButton();
            this.btnCollapse = new Infragistics.Win.Misc.UltraButton();
            this.lblCollapseALL = new System.Windows.Forms.Label();
            this.lblExpansion = new System.Windows.Forms.Label();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.ultraToolbarsManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolTipManager = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.menuItemGoToAllocation = new System.Windows.Forms.MenuItem();
            this.menuItemRemoveManualExecution = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgBlotter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).BeginInit();
            this.windowDockingArea1.SuspendLayout();
            this.dockableWindow1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager)).BeginInit();
            this.SuspendLayout();
            // 
            // dgBlotter
            // 
            this.dgBlotter.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.dgBlotter.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.dgBlotter.DisplayLayout.GroupByBox.Hidden = true;
            this.dgBlotter.DisplayLayout.MaxColScrollRegions = 1;
            this.dgBlotter.DisplayLayout.MaxRowScrollRegions = 1;
            this.dgBlotter.DisplayLayout.NewColumnLoadStyle = Infragistics.Win.UltraWinGrid.NewColumnLoadStyle.Hide;
            this.dgBlotter.DisplayLayout.NewBandLoadStyle = Infragistics.Win.UltraWinGrid.NewBandLoadStyle.Hide;
            this.dgBlotter.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.dgBlotter.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.dgBlotter.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.dgBlotter.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.dgBlotter.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.dgBlotter.DisplayLayout.Override.CellPadding = 0;
            //this.dgBlotter.DisplayLayout.Override.DefaultColWidth = 50;
            this.dgBlotter.DisplayLayout.Override.FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Like;
            this.dgBlotter.DisplayLayout.Override.FilterOperatorLocation = Infragistics.Win.UltraWinGrid.FilterOperatorLocation.Hidden;
            this.dgBlotter.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value]";
            this.dgBlotter.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.dgBlotter.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.dgBlotter.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.dgBlotter.DisplayLayout.Override.MaxSelectedRows = 1;
            this.dgBlotter.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.dgBlotter.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.dgBlotter.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.dgBlotter.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.dgBlotter.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.dgBlotter.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.dgBlotter.DisplayLayout.Override.SpecialRowSeparator = ((Infragistics.Win.UltraWinGrid.SpecialRowSeparator)(((Infragistics.Win.UltraWinGrid.SpecialRowSeparator.TemplateAddRow | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FixedRows) 
            | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.SummaryRow)));
            this.dgBlotter.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.dgBlotter.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.dgBlotter.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.dgBlotter.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.dgBlotter.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.dgBlotter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgBlotter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.dgBlotter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dgBlotter.Location = new System.Drawing.Point(0, 0);
            this.dgBlotter.Name = "dgBlotter";
            this.dgBlotter.Size = new System.Drawing.Size(755, 316);
            this.dgBlotter.TabIndex = 0;
            this.dgBlotter.Text = "ultraGrid1";
            this.dgBlotter.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dgBlotter.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.dgBlotter_InitializeLayout);
            this.dgBlotter.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.dgBlotter_InitializeRow);
            this.dgBlotter.AfterRowExpanded += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.dgBlotter_AfterRowExpanded);
            this.dgBlotter.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.dgBlotter_DoubleClickRow);
            this.dgBlotter.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.dgBlotter_AfterSortChange);
            this.dgBlotter.AfterColPosChanged += new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.dgBlotter_AfterColPosChanged);
            this.dgBlotter.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.dgBlotter_BeforeCustomRowFilterDialog);
            this.dgBlotter.InitializeRowsCollection += new Infragistics.Win.UltraWinGrid.InitializeRowsCollectionEventHandler(this.dgBlotter_InitializeRowsCollection);
            this.dgBlotter.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.dgBlotter_BeforeColumnChooserDisplayed);
            this.dgBlotter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgBlotter_MouseDown);
            this.dgBlotter.BeforeRowFilterDropDown += dgBlotter_BeforeRowFilterDropDown;
            this.dgBlotter.AfterRowFilterChanged += dgBlotter_AfterRowFilterChanged;
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuAuditTrail,
            this.menuRepeatTrade,
            this.menuTrade,
            this.menuCancel,
            this.menuEditOrReplaceOrder,
            this.mnuRemoveOrder,
            this.menuTransferTrade,
            this.menuShowDetails,
            this.menuAddFills,
            this.menuDivider1,
            this.menuSaveLayout,
            this.menuViewAllocation,
            this.menuViewAllocationStagedOrder,
            this.menuAllocate,
            this.menuItemGoToAllocation,
            this.menuDivider2,
            this.menuRenameTab,
            this.menuRemoveTab,
            this.menuRemoveFilter,
            this.menuItemRemoveManualExecution});
            this.contextMenu.Popup += new System.EventHandler(this.contextMenu_Popup);
            // 
            // menuAuditTrail
            // 
            this.menuAuditTrail.Index = 0;
            this.menuAuditTrail.Text = "&Audit Trail";
            this.menuAuditTrail.Click += new System.EventHandler(this.menuAuditTrail_Click);
            // 
            // menuRepeatTrade
            // 
            this.menuRepeatTrade.Index = 5;
            this.menuRepeatTrade.Text = "&Reload Order";
            this.menuRepeatTrade.Click += new System.EventHandler(this.menuRepeatTrade_Click);
            // 
            // menuTrade
            // 
            this.menuTrade.Index = 1;
            this.menuTrade.Text = "&Trade";
            this.menuTrade.Click += new System.EventHandler(this.menuTrade_Click);
            // 
            // menuCancel
            // 
            this.menuCancel.Index = 2;
            this.menuCancel.Text = "&Cancel";
            this.menuCancel.Click += new System.EventHandler(this.menuCancel_Click);
            // 
            // menuEditOrReplaceOrder
            // 
            this.menuEditOrReplaceOrder.Index = 3;
            this.menuEditOrReplaceOrder.Text = "&Edit Order(s)";
            this.menuEditOrReplaceOrder.Click += new System.EventHandler(this.menuEditOrReplaceOrder_Clicked);
            // mnuRemoveOrder
            // 
            this.mnuRemoveOrder.Index = 4;
            this.mnuRemoveOrder.Text = "Remove Order";
            this.mnuRemoveOrder.Click += new System.EventHandler(this.menuRemoveOrderClicked);
            // 
            // menuTransferTrade
            // 
            this.menuTransferTrade.Index = 6;
            this.menuTransferTrade.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuOtherUsers,
            this.menuItem3});
            this.menuTransferTrade.Text = "Transfer to &User";
            this.menuTransferTrade.Select += menuTransferTrade_Select;
            // 
            // menuOtherUsers
            // 
            this.menuOtherUsers.Index = 0;
            this.menuOtherUsers.Text = "User1";
            this.menuOtherUsers.Click += new System.EventHandler(this.menuOtherUsers_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "User2";
            this.menuItem3.Click += new System.EventHandler(this.menuOtherUsers_Click);
            // 
            // menuShowDetails
            // 
            this.menuShowDetails.Index = 7;
            this.menuShowDetails.Text = "Show Details";
            this.menuShowDetails.Visible = false;
            this.menuShowDetails.Click += new System.EventHandler(this.menuShowDetails_Click);
            // 
            // menuAddFills
            // 
            this.menuAddFills.Index = 8;
            this.menuAddFills.Text = "Add/Modify &Fills";
            this.menuAddFills.Click += new System.EventHandler(this.menuAddFills_Click);
            // 
            // menuItem4
            // 
            this.menuDivider1.Index = 9;
            this.menuDivider1.Text = "-";
            // 
            // menuSaveLayout
            // 
            this.menuSaveLayout.Index = 10;
            this.menuSaveLayout.Text = "Save Layout";
            this.menuSaveLayout.Click += new System.EventHandler(this.menuSaveLayout_Click);
            // 
            // menuViewAllocation
            // 
            this.menuViewAllocation.Index = 11;
            this.menuViewAllocation.Text = "PTT Allocation Details";
            this.menuViewAllocation.Click += new System.EventHandler(this.menuViewAllocation_Click);
            // 
            // menuViewAllocationStagedOrder
            // 
            this.menuViewAllocationStagedOrder.Index = 12;
            this.menuViewAllocationStagedOrder.Text = "View Allocation";
            this.menuViewAllocationStagedOrder.Click += new System.EventHandler(this.menuViewAllocationStagedOrder_Click);
            // 
            // menuViewAllocationDetails
            // 
            this.menuAllocate.Index = 13;
            this.menuAllocate.Text = "Allocate";
            this.menuAllocate.Click += new System.EventHandler(this.menuAllocate_Click);
            // 
            // menuFilters
            // 
            this.menuFilters.Checked = true;
            this.menuFilters.Index = -1;
            this.menuFilters.Text = "F&ilters";
            this.menuFilters.Click += new System.EventHandler(this.menuFilters_Click);
            // 
            // menuDivider
            // 
            this.menuDivider2.Index = 14;
            this.menuDivider2.Text = "-";
            // 
            // menuRenameTab
            // 
            this.menuRenameTab.Index = 15;
            this.menuRenameTab.Text = "Rename Tab";
            this.menuRenameTab.Click += new System.EventHandler(this.menuRenameTab_Click);
            // 
            // menuRemoveTab
            // 
            this.menuRemoveTab.Index = 16;
            this.menuRemoveTab.Text = "Remove Tab";
            this.menuRemoveTab.Click += new System.EventHandler(this.menuRemoveTab_Click);
            // 
            // menuRemoveFilter
            // 
            this.menuRemoveFilter.Index = 17;
            this.menuRemoveFilter.Text = "Remove Filter";
            this.menuRemoveFilter.Click += new System.EventHandler(this.menuRemoveFilter_Click);
           
             // 
            // ultraDockManager1
            // 
            this.ultraDockManager1.AnimationSpeed = Infragistics.Win.UltraWinDock.AnimationSpeed.StandardSpeedPlus3;
            this.ultraDockManager1.DefaultPaneSettings.AllowClose = Infragistics.Win.DefaultableBoolean.False;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ultraDockManager1.DefaultPaneSettings.Appearance = appearance1;
            this.ultraDockManager1.DefaultPaneSettings.DoubleClickAction = Infragistics.Win.UltraWinDock.PaneDoubleClickAction.ToggleDockedState;
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(49, 316, 603, 86);
            dockableControlPane1.Size = new System.Drawing.Size(100, 100);
            dockableControlPane1.Text = "Row Filter";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1});
            dockAreaPane1.Size = new System.Drawing.Size(755, 84);
            this.ultraDockManager1.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1});
            this.ultraDockManager1.HostControl = this;
            this.ultraDockManager1.ShowDisabledButtons = false;
            this.ultraDockManager1.UseDefaultContextMenus = false;
            this.ultraDockManager1.AfterPaneButtonClick += new Infragistics.Win.UltraWinDock.PaneButtonEventHandler(this.ultraDockManager1_AfterPaneButtonClick);
            // 
            // _WorkingSubBlotterGridUnpinnedTabAreaLeft
            // 
            this._WorkingSubBlotterGridUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._WorkingSubBlotterGridUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._WorkingSubBlotterGridUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._WorkingSubBlotterGridUnpinnedTabAreaLeft.Name = "_WorkingSubBlotterGridUnpinnedTabAreaLeft";
            this._WorkingSubBlotterGridUnpinnedTabAreaLeft.Owner = this.ultraDockManager1;
            this._WorkingSubBlotterGridUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 405);
            this._WorkingSubBlotterGridUnpinnedTabAreaLeft.TabIndex = 7;
            // 
            // _WorkingSubBlotterGridUnpinnedTabAreaRight
            // 
            this._WorkingSubBlotterGridUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._WorkingSubBlotterGridUnpinnedTabAreaRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._WorkingSubBlotterGridUnpinnedTabAreaRight.Location = new System.Drawing.Point(755, 0);
            this._WorkingSubBlotterGridUnpinnedTabAreaRight.Name = "_WorkingSubBlotterGridUnpinnedTabAreaRight";
            this._WorkingSubBlotterGridUnpinnedTabAreaRight.Owner = this.ultraDockManager1;
            this._WorkingSubBlotterGridUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 405);
            this._WorkingSubBlotterGridUnpinnedTabAreaRight.TabIndex = 8;
            // 
            // _WorkingSubBlotterGridUnpinnedTabAreaTop
            // 
            this._WorkingSubBlotterGridUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._WorkingSubBlotterGridUnpinnedTabAreaTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._WorkingSubBlotterGridUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._WorkingSubBlotterGridUnpinnedTabAreaTop.Name = "_WorkingSubBlotterGridUnpinnedTabAreaTop";
            this._WorkingSubBlotterGridUnpinnedTabAreaTop.Owner = this.ultraDockManager1;
            this._WorkingSubBlotterGridUnpinnedTabAreaTop.Size = new System.Drawing.Size(755, 0);
            this._WorkingSubBlotterGridUnpinnedTabAreaTop.TabIndex = 9;
            // 
            // _WorkingSubBlotterGridUnpinnedTabAreaBottom
            // 
            this._WorkingSubBlotterGridUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._WorkingSubBlotterGridUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._WorkingSubBlotterGridUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 405);
            this._WorkingSubBlotterGridUnpinnedTabAreaBottom.Name = "_WorkingSubBlotterGridUnpinnedTabAreaBottom";
            this._WorkingSubBlotterGridUnpinnedTabAreaBottom.Owner = this.ultraDockManager1;
            this._WorkingSubBlotterGridUnpinnedTabAreaBottom.Size = new System.Drawing.Size(755, 0);
            this._WorkingSubBlotterGridUnpinnedTabAreaBottom.TabIndex = 10;
            // 
            // _WorkingSubBlotterGridAutoHideControl
            // 
            this._WorkingSubBlotterGridAutoHideControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._WorkingSubBlotterGridAutoHideControl.Location = new System.Drawing.Point(0, 0);
            this._WorkingSubBlotterGridAutoHideControl.Name = "_WorkingSubBlotterGridAutoHideControl";
            this._WorkingSubBlotterGridAutoHideControl.Owner = this.ultraDockManager1;
            this._WorkingSubBlotterGridAutoHideControl.Size = new System.Drawing.Size(0, 405);
            this._WorkingSubBlotterGridAutoHideControl.TabIndex = 11;
            // 
            // btnExpansion
            // 
            this.btnExpansion.AcceptsFocus = false;
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.btnExpansion.Appearance = appearance2;
            this.btnExpansion.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnExpansion.ButtonStyle = Infragistics.Win.UIElementButtonStyle.ButtonSoftExtended;
            this.btnExpansion.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnExpansion.Location = new System.Drawing.Point(3, 0);
            this.btnExpansion.Name = "btnExpansion";
            this.btnExpansion.Size = new System.Drawing.Size(23, 15);
            this.btnExpansion.TabIndex = 13;
            this.btnExpansion.Text = "+";
            this.btnExpansion.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnExpansion.Click += new System.EventHandler(this.btnExpansion_Click);
            // 
            // btnCollapse
            // 
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.btnCollapse.Appearance = appearance3;
            this.btnCollapse.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnCollapse.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCollapse.Location = new System.Drawing.Point(125, 1);
            this.btnCollapse.Name = "btnCollapse";
            this.btnCollapse.Size = new System.Drawing.Size(14, 14);
            this.btnCollapse.TabIndex = 15;
            this.btnCollapse.Text = "-";
            this.btnCollapse.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnCollapse.Visible = false;
            this.btnCollapse.Click += new System.EventHandler(this.btnCollapse_Click);
            // 
            // lblCollapseALL
            // 
            this.lblCollapseALL.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblCollapseALL.Location = new System.Drawing.Point(138, 1);
            this.lblCollapseALL.Name = "lblCollapseALL";
            this.lblCollapseALL.Size = new System.Drawing.Size(78, 12);
            this.lblCollapseALL.TabIndex = 16;
            this.lblCollapseALL.Text = "Collapse All";
            this.lblCollapseALL.Visible = false;
            // 
            // lblExpansion
            // 
            this.lblExpansion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblExpansion.Location = new System.Drawing.Point(45, 1);
            this.lblExpansion.Name = "lblExpansion";
            this.lblExpansion.Size = new System.Drawing.Size(74, 12);
            this.lblExpansion.TabIndex = 14;
            this.lblExpansion.Text = "Expand All Orders";
            this.lblExpansion.Visible = false;
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Controls.Add(this.dockableWindow1);
            this.windowDockingArea1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.windowDockingArea1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowDockingArea1.Location = new System.Drawing.Point(0, 316);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.ultraDockManager1;
            this.windowDockingArea1.Size = new System.Drawing.Size(755, 89);
            this.windowDockingArea1.TabIndex = 12;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Location = new System.Drawing.Point(0, 5);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.ultraDockManager1;
            this.dockableWindow1.Size = new System.Drawing.Size(755, 84);
            this.dockableWindow1.TabIndex = 21;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager.DesignerFlags = 1;
            this.ultraToolbarsManager.DockWithinContainer = this;
            // 
            // _WorkingSubBlotterGrid_Toolbars_Dock_Area_Left
            // 
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Left.Name = "_WorkingSubBlotterGrid_Toolbars_Dock_Area_Left";
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 405);
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager;
            // 
            // _WorkingSubBlotterGrid_Toolbars_Dock_Area_Right
            // 
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(755, 0);
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Right.Name = "_WorkingSubBlotterGrid_Toolbars_Dock_Area_Right";
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 405);
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager;
            // 
            // _WorkingSubBlotterGrid_Toolbars_Dock_Area_Top
            // 
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Top.Name = "_WorkingSubBlotterGrid_Toolbars_Dock_Area_Top";
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(755, 0);
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager;
            // 
            // _WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom
            // 
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 405);
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom.Name = "_WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom";
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(755, 0);
            this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager;
            // 
            // ultraToolTipManager
            // 
            this.ultraToolTipManager.ContainingControl = this;
            // 
            // menuItemGoToAllocation
            // 
            this.menuItemGoToAllocation.Index = 12;
            this.menuItemGoToAllocation.Text = "Go to Allocation";
            this.menuItemGoToAllocation.Click += new System.EventHandler(this.menuItemGoToAllocation_Click);
            // 
            // menuItemRemoveManualExecution
            // 
            this.menuItemRemoveManualExecution.Index = 17;
            this.menuItemRemoveManualExecution.Text = "Remove Execution";
            this.menuItemRemoveManualExecution.Click += new System.EventHandler(this.menuItemRemoveManualExecution_Click);
            // 
            // WorkingSubBlotterGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._WorkingSubBlotterGridAutoHideControl);
            this.Controls.Add(this.btnExpansion);
            this.Controls.Add(this.btnCollapse);
            this.Controls.Add(this.lblCollapseALL);
            this.Controls.Add(this.lblExpansion);
            this.Controls.Add(this.dgBlotter);
            this.Controls.Add(this.windowDockingArea1);
            this.Controls.Add(this._WorkingSubBlotterGridUnpinnedTabAreaTop);
            this.Controls.Add(this._WorkingSubBlotterGridUnpinnedTabAreaBottom);
            this.Controls.Add(this._WorkingSubBlotterGridUnpinnedTabAreaLeft);
            this.Controls.Add(this._WorkingSubBlotterGridUnpinnedTabAreaRight);
            this.Controls.Add(this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._WorkingSubBlotterGrid_Toolbars_Dock_Area_Top);
            this.Name = "WorkingSubBlotterGrid";
            this.Size = new System.Drawing.Size(755, 405);
            this.Load += new System.EventHandler(this.WorkingSubBlotterGrid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgBlotter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).EndInit();
            this.windowDockingArea1.ResumeLayout(false);
            this.dockableWindow1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        public PranaUltraGrid dgBlotter;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem menuAuditTrail;
        public System.Windows.Forms.MenuItem menuTrade;
        public System.Windows.Forms.MenuItem menuRepeatTrade;
        private System.Windows.Forms.MenuItem menuCancel;
        private System.Windows.Forms.MenuItem menuEditOrReplaceOrder;
        private System.Windows.Forms.MenuItem menuTransferTrade;
        private System.Windows.Forms.MenuItem menuOtherUsers;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuAddFills;
        private System.Windows.Forms.MenuItem menuDivider1;
        private System.Windows.Forms.MenuItem menuSaveLayout;
        private System.Windows.Forms.MenuItem menuFilters;
        private System.Windows.Forms.MenuItem menuViewAllocation;
        private System.Windows.Forms.MenuItem menuViewAllocationStagedOrder;
        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager1;
        private Infragistics.Win.UltraWinDock.AutoHideControl _WorkingSubBlotterGridAutoHideControl;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _WorkingSubBlotterGridUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _WorkingSubBlotterGridUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _WorkingSubBlotterGridUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _WorkingSubBlotterGridUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
        protected Infragistics.Win.Misc.UltraButton btnExpansion;
        protected Infragistics.Win.Misc.UltraButton btnCollapse;
        protected System.Windows.Forms.Label lblCollapseALL;
        protected System.Windows.Forms.Label lblExpansion;
        protected System.Windows.Forms.MenuItem menuShowDetails;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _WorkingSubBlotterGrid_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _WorkingSubBlotterGrid_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _WorkingSubBlotterGrid_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _WorkingSubBlotterGrid_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager;
        private System.Windows.Forms.MenuItem menuAllocate;
        private System.Windows.Forms.MenuItem menuItemGoToAllocation;
        private System.Windows.Forms.MenuItem menuItemRemoveManualExecution ;
        private System.Windows.Forms.MenuItem mnuRemoveOrder;
        private System.Windows.Forms.MenuItem menuDivider2;
        private System.Windows.Forms.MenuItem menuRemoveTab;
        private System.Windows.Forms.MenuItem menuRenameTab;
        private System.Windows.Forms.MenuItem menuRemoveFilter;
    }
}