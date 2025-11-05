namespace Prana.Blotter
{
    partial class OrderBlotterGrid
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
            if (disposing)
            {
                if(components != null)
                   components.Dispose();
                if(_allocationProxy != null)
                    _allocationProxy.Dispose(); 
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Select All", 0);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.SubOrderBlotterGrid = new Prana.Blotter.Controls.SubOrderBlotterGrid();
            this.ultraLabelSubOrders = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanelSubOrdersBlotter = new Infragistics.Win.Misc.UltraPanel();
            this.ultraSplitter = new Infragistics.Win.Misc.UltraSplitter();
            ((System.ComponentModel.ISupportInitialize)(this.dgBlotter)).BeginInit();
            this.ultraPanelSubOrdersBlotter.ClientArea.SuspendLayout();
            this.ultraPanelSubOrdersBlotter.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgBlotter
            // 
            ultraGridColumn1.DataType = typeof(bool);
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Width = 734;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1});
            this.dgBlotter.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.dgBlotter.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.dgBlotter.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.dgBlotter.DisplayLayout.GroupByBox.Hidden = true;
            this.dgBlotter.DisplayLayout.MaxBandDepth = 2;
            this.dgBlotter.DisplayLayout.MaxColScrollRegions = 1;
            this.dgBlotter.DisplayLayout.MaxRowScrollRegions = 1;
            appearance1.BackColor = System.Drawing.Color.Gold;
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.dgBlotter.DisplayLayout.Override.ActiveRowAppearance = appearance1;
            this.dgBlotter.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.dgBlotter.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.dgBlotter.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.dgBlotter.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.dgBlotter.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.dgBlotter.DisplayLayout.Override.DefaultRowHeight = 20;
            this.dgBlotter.DisplayLayout.Override.FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Like;
            this.dgBlotter.DisplayLayout.Override.FilterOperatorLocation = Infragistics.Win.UltraWinGrid.FilterOperatorLocation.Hidden;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.dgBlotter.DisplayLayout.Override.GroupByRowAppearance = appearance2;
            this.dgBlotter.DisplayLayout.Override.GroupByRowDescriptionMask = "[caption] : [value]";
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.dgBlotter.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.dgBlotter.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.dgBlotter.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.dgBlotter.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.dgBlotter.DisplayLayout.Override.MaxSelectedRows = 1;
            appearance4.BackColor = System.Drawing.Color.Black;
            appearance4.ForeColor = System.Drawing.Color.Lime;
            this.dgBlotter.DisplayLayout.Override.RowAlternateAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            appearance5.ForeColor = System.Drawing.Color.Lime;
            this.dgBlotter.DisplayLayout.Override.RowAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.dgBlotter.DisplayLayout.Override.RowSelectorAppearance = appearance6;
            this.dgBlotter.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.dgBlotter.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.dgBlotter.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance7.BackColor = System.Drawing.Color.Gold;
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.dgBlotter.DisplayLayout.Override.SelectedCellAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.dgBlotter.DisplayLayout.Override.SelectedRowAppearance = appearance8;
            this.dgBlotter.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.dgBlotter.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.dgBlotter.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.dgBlotter.DisplayLayout.Override.SpecialRowSeparator = ((Infragistics.Win.UltraWinGrid.SpecialRowSeparator)(((Infragistics.Win.UltraWinGrid.SpecialRowSeparator.TemplateAddRow | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FixedRows) 
            | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.SummaryRow)));
            appearance9.BackColor = System.Drawing.SystemColors.Info;
            this.dgBlotter.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance9;
            this.dgBlotter.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.dgBlotter.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            scrollBarLook1.Appearance = appearance10;
            this.dgBlotter.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.dgBlotter.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.dgBlotter.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.dgBlotter.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.dgBlotter.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.dgBlotter.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.dgBlotter.Size = new System.Drawing.Size(755, 205);
            this.dgBlotter.DragDrop += new System.Windows.Forms.DragEventHandler(this.OrderBlotterGrid_DragDrop);
            this.dgBlotter.DragOver += new System.Windows.Forms.DragEventHandler(this.OrderBlotterGrid_DragOver);
            this.dgBlotter.AfterRowActivate += this.dgBlotter_AfterRowActivate;
            this.dgBlotter.BeforeHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.BeforeHeaderCheckStateChangedEventHandler(this.dgBlotter_BeforeHeaderCheckStateChanged);
            this.dgBlotter.AfterHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventHandler(this.dgBlotter_AfterHeaderCheckStateChanged);
            this.dgBlotter.CellChange += this.dgBlotter_CellChange;

            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            // 
            // btnExpansion
            // 
            this.btnExpansion.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnExpansion.Location = new System.Drawing.Point(3, 1);
            // 
            // btnCollapse
            // 
            this.btnCollapse.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            // 
            // SubOrderBlotterGrid
            // 
            this.SubOrderBlotterGrid.BlotterType = Prana.Global.OrderFields.BlotterTypes.SubOrders;
            this.SubOrderBlotterGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubOrderBlotterGrid.Key = "SubOrders";
            this.SubOrderBlotterGrid.Location = new System.Drawing.Point(0, 17);
            this.SubOrderBlotterGrid.Name = "SubOrderBlotterGrid";
            this.SubOrderBlotterGrid.Size = new System.Drawing.Size(755, 178);
            this.SubOrderBlotterGrid.TabIndex = 23;
            this.SubOrderBlotterGrid.LaunchAddFills += this.SubOrderBlotterGrid_LaunchAddFills;
            this.SubOrderBlotterGrid.LaunchAuditTrail += this.SubOrderBlotterGrid_LaunchAuditTrail;
            this.SubOrderBlotterGrid.TradeClick += this.SubOrderBlotterGrid_TradeClick;
			this.SubOrderBlotterGrid.GoToAllocationClicked += this.SubOrderBlotterGrid_GoToAllocationClicked;
            this.SubOrderBlotterGrid.ReplaceOrEditOrderClicked += this.SubOrderBlotterGrid_ReplaceOrEditOrderClicked;
            this.SubOrderBlotterGrid.UpdateStatusBar += this.SubOrderBlotterGrid_UpdateStatusBar;
            this.SubOrderBlotterGrid.DisableRolloverButton += this.SubOrderBlotterGrid_DisableRolloverButton;
            // 
            // ultraLabelSubOrders
            // 
            appearance11.TextHAlignAsString = "Center";
            appearance11.TextVAlignAsString = "Middle";
            this.ultraLabelSubOrders.Appearance = appearance11;
            this.ultraLabelSubOrders.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraLabelSubOrders.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ultraLabelSubOrders.Location = new System.Drawing.Point(0, 0);
            this.ultraLabelSubOrders.Name = "ultraLabelSubOrders";
            this.ultraLabelSubOrders.Size = new System.Drawing.Size(755, 17);
            this.ultraLabelSubOrders.TabIndex = 24;
            this.ultraLabelSubOrders.Text = " Sub Orders";
            // 
            // ultraPanelSubOrdersBlotter
            // 
            // 
            // ultraPanelSubOrdersBlotter.ClientArea
            // 
            this.ultraPanelSubOrdersBlotter.ClientArea.Controls.Add(this.SubOrderBlotterGrid);
            this.ultraPanelSubOrdersBlotter.ClientArea.Controls.Add(this.ultraLabelSubOrders);
            this.ultraPanelSubOrdersBlotter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanelSubOrdersBlotter.Location = new System.Drawing.Point(0, 210);
            this.ultraPanelSubOrdersBlotter.Name = "ultraPanelSubOrdersBlotter";
            this.ultraPanelSubOrdersBlotter.Size = new System.Drawing.Size(755, 195);
            this.ultraPanelSubOrdersBlotter.TabIndex = 25;
            // 
            // ultraSplitter
            // 
            this.ultraSplitter.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter.CollapseUIType = Infragistics.Win.Misc.CollapseUIType.None;
            this.ultraSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter.Location = new System.Drawing.Point(0, 205);
            this.ultraSplitter.Name = "ultraSplitter";
            this.ultraSplitter.RestoreExtent = 0;
            this.ultraSplitter.Size = new System.Drawing.Size(755, 5);
            this.ultraSplitter.TabIndex = 22;
            // 
            // OrderBlotterGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.ultraSplitter);
            this.Controls.Add(this.ultraPanelSubOrdersBlotter);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "OrderBlotterGrid";
            this.Controls.SetChildIndex(this.ultraPanelSubOrdersBlotter, 0);
            this.Controls.SetChildIndex(this.lblExpansion, 0);
            this.Controls.SetChildIndex(this.lblCollapseALL, 0);
            this.Controls.SetChildIndex(this.btnCollapse, 0);
            this.Controls.SetChildIndex(this.btnExpansion, 0);
            this.Controls.SetChildIndex(this.ultraSplitter, 0);
            this.Controls.SetChildIndex(this.dgBlotter, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgBlotter)).EndInit();
            this.ultraPanelSubOrdersBlotter.ClientArea.ResumeLayout(false);
            this.ultraPanelSubOrdersBlotter.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        internal Controls.SubOrderBlotterGrid SubOrderBlotterGrid;
        private Infragistics.Win.Misc.UltraLabel ultraLabelSubOrders;
        private Infragistics.Win.Misc.UltraPanel ultraPanelSubOrdersBlotter;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
    }
}