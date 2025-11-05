namespace Prana.Tools
{
    partial class CashForm
    {
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
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            this.menuStripGetLots = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.pnlGetData = new System.Windows.Forms.Panel();
            this.btnRunBatch = new System.Windows.Forms.Button();
            this.lblDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnGetFx = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlYesterdayDayEnd = new System.Windows.Forms.Panel();
            this.lblYesterdayDate = new System.Windows.Forms.Label();
            this.lblYesterdayDayEnd = new System.Windows.Forms.Label();
            this.grdYesterdayEnd = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pnltabDayEnd = new System.Windows.Forms.Panel();
            this.tabCntlDayEndData = new System.Windows.Forms.TabControl();
            this.lblTodayDayEnd = new System.Windows.Forms.Label();
            this.grdGetLots = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.menuStripGetLots.SuspendLayout();
            this.pnlGetData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlYesterdayDayEnd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdYesterdayEnd)).BeginInit();
            this.pnltabDayEnd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdGetLots)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripGetLots
            // 
            this.menuStripGetLots.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRowToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.menuStripGetLots.Name = "menuStripCashValue";
            this.menuStripGetLots.Size = new System.Drawing.Size(117, 48);
            // 
            // addRowToolStripMenuItem
            // 
            this.addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
            this.addRowToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.addRowToolStripMenuItem.Text = "Add";
            this.addRowToolStripMenuItem.Click += new System.EventHandler(this.addRowToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // pnlGetData
            // 
            this.pnlGetData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGetData.Controls.Add(this.btnRunBatch);
            this.pnlGetData.Controls.Add(this.lblDate);
            this.pnlGetData.Controls.Add(this.dtDate);
            this.pnlGetData.Controls.Add(this.btnSave);
            this.pnlGetData.Controls.Add(this.btnGetFx);
            this.pnlGetData.Location = new System.Drawing.Point(2, 3);
            this.pnlGetData.Name = "pnlGetData";
            this.pnlGetData.Size = new System.Drawing.Size(713, 35);
            this.pnlGetData.TabIndex = 102;
            // 
            // btnRunBatch
            // 
            this.btnRunBatch.Location = new System.Drawing.Point(216, 4);
            this.btnRunBatch.Name = "btnRunBatch";
            this.btnRunBatch.Size = new System.Drawing.Size(60, 23);
            this.btnRunBatch.TabIndex = 106;
            this.btnRunBatch.Text = "Run ";
            this.btnRunBatch.UseVisualStyleBackColor = true;
            this.btnRunBatch.Click += new System.EventHandler(this.btnRunBatch_Click);
            // 
            // lblDate
            // 
            this.lblDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(3, 8);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(37, 15);
            this.lblDate.TabIndex = 105;
            this.lblDate.Text = "Date";
            // 
            // dtDate
            // 
            this.dtDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtDate.Location = new System.Drawing.Point(42, 4);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(86, 22);
            this.dtDate.TabIndex = 104;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(282, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 23);
            this.btnSave.TabIndex = 101;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGetFx
            // 
            this.btnGetFx.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetFx.Location = new System.Drawing.Point(151, 4);
            this.btnGetFx.Name = "btnGetFx";
            this.btnGetFx.Size = new System.Drawing.Size(60, 23);
            this.btnGetFx.TabIndex = 103;
            this.btnGetFx.Text = "Get Fx";
            this.btnGetFx.UseVisualStyleBackColor = true;
            this.btnGetFx.Click += new System.EventHandler(this.btnGetFx_Click);
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
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.grdGetLots);
            this.splitContainer2.Size = new System.Drawing.Size(713, 425);
            this.splitContainer2.SplitterDistance = 212;
            this.splitContainer2.TabIndex = 110;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlYesterdayDayEnd);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnltabDayEnd);
            this.splitContainer1.Size = new System.Drawing.Size(713, 212);
            this.splitContainer1.SplitterDistance = 369;
            this.splitContainer1.TabIndex = 108;
            // 
            // pnlYesterdayDayEnd
            // 
            this.pnlYesterdayDayEnd.Controls.Add(this.lblYesterdayDate);
            this.pnlYesterdayDayEnd.Controls.Add(this.lblYesterdayDayEnd);
            this.pnlYesterdayDayEnd.Controls.Add(this.grdYesterdayEnd);
            this.pnlYesterdayDayEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlYesterdayDayEnd.Location = new System.Drawing.Point(0, 0);
            this.pnlYesterdayDayEnd.Name = "pnlYesterdayDayEnd";
            this.pnlYesterdayDayEnd.Size = new System.Drawing.Size(369, 212);
            this.pnlYesterdayDayEnd.TabIndex = 108;
            // 
            // lblYesterdayDate
            // 
            this.lblYesterdayDate.AutoSize = true;
            this.lblYesterdayDate.Location = new System.Drawing.Point(106, 5);
            this.lblYesterdayDate.Name = "lblYesterdayDate";
            this.lblYesterdayDate.Size = new System.Drawing.Size(30, 13);
            this.lblYesterdayDate.TabIndex = 104;
            this.lblYesterdayDate.Text = "Date";
            // 
            // lblYesterdayDayEnd
            // 
            this.lblYesterdayDayEnd.AutoSize = true;
            this.lblYesterdayDayEnd.Location = new System.Drawing.Point(3, 5);
            this.lblYesterdayDayEnd.Name = "lblYesterdayDayEnd";
            this.lblYesterdayDayEnd.Size = new System.Drawing.Size(98, 13);
            this.lblYesterdayDayEnd.TabIndex = 103;
            this.lblYesterdayDayEnd.Text = "Yesterday Day End";
            // 
            // grdYesterdayEnd
            // 
            this.grdYesterdayEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdYesterdayEnd.ContextMenuStrip = this.menuStripGetLots;
            appearance17.BackColor = System.Drawing.Color.Black;
            this.grdYesterdayEnd.DisplayLayout.Appearance = appearance17;
            this.grdYesterdayEnd.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdYesterdayEnd.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdYesterdayEnd.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdYesterdayEnd.DisplayLayout.EmptyRowSettings.ShowEmptyRows = true;
            this.grdYesterdayEnd.DisplayLayout.EmptyRowSettings.Style = Infragistics.Win.UltraWinGrid.EmptyRowStyle.HideRowSelector;
            this.grdYesterdayEnd.DisplayLayout.MaxColScrollRegions = 1;
            this.grdYesterdayEnd.DisplayLayout.MaxRowScrollRegions = 1;
            appearance18.BackColor = System.Drawing.Color.LightSlateGray;
            appearance18.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance18.BorderColor = System.Drawing.Color.DimGray;
            appearance18.FontData.BoldAsString = "True";
            appearance18.ForeColor = System.Drawing.Color.White;
            this.grdYesterdayEnd.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdYesterdayEnd.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdYesterdayEnd.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdYesterdayEnd.DisplayLayout.Override.CellPadding = 0;
            this.grdYesterdayEnd.DisplayLayout.Override.CellSpacing = 0;
            this.grdYesterdayEnd.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance19.BorderColor = System.Drawing.Color.Transparent;
            appearance19.ForeColor = System.Drawing.Color.White;
            this.grdYesterdayEnd.DisplayLayout.Override.GroupByRowAppearance = appearance19;
            appearance20.FontData.Name = "Tahoma";
            appearance20.FontData.SizeInPoints = 8F;
            appearance20.TextHAlignAsString = "Center";
            this.grdYesterdayEnd.DisplayLayout.Override.HeaderAppearance = appearance20;
            this.grdYesterdayEnd.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdYesterdayEnd.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance21.ForeColor = System.Drawing.Color.White;
            appearance21.TextHAlignAsString = "Right";
            appearance21.TextVAlignAsString = "Middle";
            this.grdYesterdayEnd.DisplayLayout.Override.RowAlternateAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.Black;
            appearance22.ForeColor = System.Drawing.Color.White;
            appearance22.TextHAlignAsString = "Right";
            appearance22.TextVAlignAsString = "Middle";
            this.grdYesterdayEnd.DisplayLayout.Override.RowAppearance = appearance22;
            this.grdYesterdayEnd.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdYesterdayEnd.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdYesterdayEnd.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance23.BackColor = System.Drawing.Color.Transparent;
            appearance23.BorderColor = System.Drawing.Color.Transparent;
            appearance23.FontData.BoldAsString = "True";
            this.grdYesterdayEnd.DisplayLayout.Override.SelectedRowAppearance = appearance23;
            this.grdYesterdayEnd.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdYesterdayEnd.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdYesterdayEnd.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdYesterdayEnd.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdYesterdayEnd.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdYesterdayEnd.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdYesterdayEnd.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            this.grdYesterdayEnd.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdYesterdayEnd.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.grdYesterdayEnd.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdYesterdayEnd.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdYesterdayEnd.DisplayLayout.UseFixedHeaders = true;
            this.grdYesterdayEnd.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdYesterdayEnd.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdYesterdayEnd.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdYesterdayEnd.Location = new System.Drawing.Point(0, 22);
            this.grdYesterdayEnd.Name = "grdYesterdayEnd";
            this.grdYesterdayEnd.Size = new System.Drawing.Size(369, 190);
            this.grdYesterdayEnd.TabIndex = 102;
            this.grdYesterdayEnd.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // pnltabDayEnd
            // 
            this.pnltabDayEnd.Controls.Add(this.tabCntlDayEndData);
            this.pnltabDayEnd.Controls.Add(this.lblTodayDayEnd);
            this.pnltabDayEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnltabDayEnd.Location = new System.Drawing.Point(0, 0);
            this.pnltabDayEnd.Name = "pnltabDayEnd";
            this.pnltabDayEnd.Size = new System.Drawing.Size(340, 212);
            this.pnltabDayEnd.TabIndex = 109;
            // 
            // tabCntlDayEndData
            // 
            this.tabCntlDayEndData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCntlDayEndData.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabCntlDayEndData.Location = new System.Drawing.Point(0, 0);
            this.tabCntlDayEndData.Name = "tabCntlDayEndData";
            this.tabCntlDayEndData.SelectedIndex = 0;
            this.tabCntlDayEndData.Size = new System.Drawing.Size(340, 212);
            this.tabCntlDayEndData.TabIndex = 104;
            // 
            // lblTodayDayEnd
            // 
            this.lblTodayDayEnd.AutoSize = true;
            this.lblTodayDayEnd.Location = new System.Drawing.Point(3, 5);
            this.lblTodayDayEnd.Name = "lblTodayDayEnd";
            this.lblTodayDayEnd.Size = new System.Drawing.Size(81, 13);
            this.lblTodayDayEnd.TabIndex = 103;
            this.lblTodayDayEnd.Text = "Today Day End";
            // 
            // grdGetLots
            // 
            this.grdGetLots.ContextMenuStrip = this.menuStripGetLots;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdGetLots.DisplayLayout.Appearance = appearance1;
            this.grdGetLots.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdGetLots.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdGetLots.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.EmptyRowSettings.ShowEmptyRows = true;
            this.grdGetLots.DisplayLayout.EmptyRowSettings.Style = Infragistics.Win.UltraWinGrid.EmptyRowStyle.HideRowSelector;
            this.grdGetLots.DisplayLayout.MaxColScrollRegions = 1;
            this.grdGetLots.DisplayLayout.MaxRowScrollRegions = 1;
            appearance9.BackColor = System.Drawing.Color.LightSlateGray;
            appearance9.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance9.BorderColor = System.Drawing.Color.DimGray;
            appearance9.FontData.BoldAsString = "True";
            appearance9.ForeColor = System.Drawing.Color.White;
            this.grdGetLots.DisplayLayout.Override.ActiveRowAppearance = appearance9;
            this.grdGetLots.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes;
            this.grdGetLots.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdGetLots.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdGetLots.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdGetLots.DisplayLayout.Override.CellPadding = 0;
            this.grdGetLots.DisplayLayout.Override.CellSpacing = 0;
            this.grdGetLots.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance10.BorderColor = System.Drawing.Color.Transparent;
            appearance10.ForeColor = System.Drawing.Color.White;
            this.grdGetLots.DisplayLayout.Override.GroupByRowAppearance = appearance10;
            appearance11.FontData.Name = "Tahoma";
            appearance11.FontData.SizeInPoints = 8F;
            appearance11.TextHAlignAsString = "Center";
            this.grdGetLots.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.grdGetLots.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdGetLots.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance12.ForeColor = System.Drawing.Color.White;
            appearance12.TextHAlignAsString = "Right";
            appearance12.TextVAlignAsString = "Middle";
            this.grdGetLots.DisplayLayout.Override.RowAlternateAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.Black;
            appearance13.ForeColor = System.Drawing.Color.White;
            appearance13.TextHAlignAsString = "Right";
            appearance13.TextVAlignAsString = "Middle";
            this.grdGetLots.DisplayLayout.Override.RowAppearance = appearance13;
            this.grdGetLots.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdGetLots.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdGetLots.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            appearance14.BorderColor = System.Drawing.Color.Transparent;
            appearance14.FontData.BoldAsString = "True";
            this.grdGetLots.DisplayLayout.Override.SelectedRowAppearance = appearance14;
            this.grdGetLots.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdGetLots.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdGetLots.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdGetLots.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdGetLots.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdGetLots.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdGetLots.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            this.grdGetLots.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance15.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdGetLots.DisplayLayout.Override.TemplateAddRowAppearance = appearance15;
            this.grdGetLots.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdGetLots.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdGetLots.DisplayLayout.UseFixedHeaders = true;
            this.grdGetLots.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdGetLots.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdGetLots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdGetLots.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdGetLots.Location = new System.Drawing.Point(0, 0);
            this.grdGetLots.Name = "grdGetLots";
            this.grdGetLots.Size = new System.Drawing.Size(713, 209);
            this.grdGetLots.TabIndex = 107;
            this.grdGetLots.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // CashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 467);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.pnlGetData);
            this.Name = "CashForm";
            this.Text = "CashForm";
            this.Load += new System.EventHandler(this.CashForm_Load);
            this.menuStripGetLots.ResumeLayout(false);
            this.pnlGetData.ResumeLayout(false);
            this.pnlGetData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.pnlYesterdayDayEnd.ResumeLayout(false);
            this.pnlYesterdayDayEnd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdYesterdayEnd)).EndInit();
            this.pnltabDayEnd.ResumeLayout(false);
            this.pnltabDayEnd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdGetLots)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip menuStripGetLots;
        private System.Windows.Forms.ToolStripMenuItem addRowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private System.Windows.Forms.Panel pnlGetData;
        private System.Windows.Forms.Button btnRunBatch;
        private Infragistics.Win.Misc.UltraLabel lblDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtDate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnGetFx;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnlYesterdayDayEnd;
        private System.Windows.Forms.Label lblYesterdayDate;
        private System.Windows.Forms.Label lblYesterdayDayEnd;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdYesterdayEnd;
        private System.Windows.Forms.Panel pnltabDayEnd;
        private System.Windows.Forms.TabControl tabCntlDayEndData;
        private System.Windows.Forms.Label lblTodayDayEnd;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdGetLots;
    }
}