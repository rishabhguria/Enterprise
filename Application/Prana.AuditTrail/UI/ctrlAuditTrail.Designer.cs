namespace Prana.AuditTrail
{
    partial class ctrlAuditTrail
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
            _tbSymbol.UnwireEvents();
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
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo5 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Comma Separated Sides", Infragistics.Win.ToolTipImage.None, "", Infragistics.Win.DefaultableBoolean.True);
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo3 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Comma Separated AccountIDs", Infragistics.Win.ToolTipImage.None, "", Infragistics.Win.DefaultableBoolean.True);
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Symbol", Infragistics.Win.ToolTipImage.None, "", Infragistics.Win.DefaultableBoolean.True);
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo2 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Comma Separated GroupIDs", Infragistics.Win.ToolTipImage.None, "", Infragistics.Win.DefaultableBoolean.True);
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo4 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Reset Filters", Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
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
            this._uccFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this._uccToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this._tbSide = new Prana.Utilities.UI.UIUtilities.AutoCompleteTextBox();
            this._lbSide = new Infragistics.Win.Misc.UltraLabel();
            this._tbAccount = new Prana.Utilities.UI.UIUtilities.AutoCompleteTextBox();
            this._tbSymbol = new Prana.Utilities.UI.UIUtilities.AutoCompleteTextBox();
            this._lbTo = new Infragistics.Win.Misc.UltraLabel();
            this._lbAccount = new Infragistics.Win.Misc.UltraLabel();
            this._lbSource = new Infragistics.Win.Misc.UltraLabel();
            this._multiSelectDropDown1 = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this._lbSymbol = new Infragistics.Win.Misc.UltraLabel();
            this._lbFrom = new Infragistics.Win.Misc.UltraLabel();
            this._lbGroupId = new Infragistics.Win.Misc.UltraLabel();
            this._tbGroupID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this._btGetData = new Infragistics.Win.Misc.UltraButton();
            this._btClearFilters = new Infragistics.Win.Misc.UltraButton();
            this._btExport = new Infragistics.Win.Misc.UltraButton();
            this._btScreenShot = new Infragistics.Win.Misc.UltraButton();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this._grdTradeAuditContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._grdTradeAuditMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this._grdTradeAuditDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._grdAuditSaveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._grdTradeAudit = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._grdGroupsTaxlots = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._grdGroupsTaxlotsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._grdTaxlotGroupCopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.ugbxGeaderAuditTrail = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._uccFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._uccToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._tbSide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._tbAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._tbSymbol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._tbGroupID)).BeginInit();
            this._grdTradeAuditContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grdTradeAudit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._grdGroupsTaxlots)).BeginInit();
            this._grdGroupsTaxlotsContextMenuStrip.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel3.ClientArea.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxGeaderAuditTrail)).BeginInit();
            this.ugbxGeaderAuditTrail.SuspendLayout();
            this.SuspendLayout();
            // 
            // _uccFromDate
            // 
            appearance1.FontData.SizeInPoints = 9F;
            this._uccFromDate.Appearance = appearance1;
            this._uccFromDate.DateTime = new System.DateTime(2014, 11, 24, 0, 0, 0, 0);
            this._uccFromDate.Location = new System.Drawing.Point(55, 8);
            this._uccFromDate.Name = "_uccFromDate";
            this._uccFromDate.Size = new System.Drawing.Size(109, 23);
            this._uccFromDate.TabIndex = 0;
            this._uccFromDate.Value = new System.DateTime(2014, 11, 24, 0, 0, 0, 0);
            this._uccFromDate.ValueChanged += new System.EventHandler(this.filters_ValueChanged);
            // 
            // _uccToDate
            // 
            appearance2.FontData.SizeInPoints = 9F;
            this._uccToDate.Appearance = appearance2;
            this._uccToDate.DateTime = new System.DateTime(2014, 11, 24, 0, 0, 0, 0);
            this._uccToDate.Location = new System.Drawing.Point(204, 8);
            this._uccToDate.Name = "_uccToDate";
            this._uccToDate.Size = new System.Drawing.Size(114, 23);
            this._uccToDate.TabIndex = 1;
            this._uccToDate.Value = new System.DateTime(2014, 11, 24, 0, 0, 0, 0);
            this._uccToDate.ValueChanged += new System.EventHandler(this.filters_ValueChanged);
            // 
            // _tbSide
            // 
            appearance3.FontData.SizeInPoints = 9F;
            this._tbSide.Appearance = appearance3;
            this._tbSide.AutoSize = false;
            this._tbSide.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tbSide.Location = new System.Drawing.Point(55, 35);
            this._tbSide.Name = "_tbSide";
            this._tbSide.Size = new System.Drawing.Size(263, 23);
            this._tbSide.TabIndex = 4;
            ultraToolTipInfo5.Enabled = Infragistics.Win.DefaultableBoolean.True;
            ultraToolTipInfo5.ToolTipImage = Infragistics.Win.ToolTipImage.None;
            ultraToolTipInfo5.ToolTipText = "Comma Separated Sides";
            this.ultraToolTipManager1.SetUltraToolTip(this._tbSide, ultraToolTipInfo5);
            this._tbSide.Values = null;
            this._tbSide.ValueChanged += new System.EventHandler(this.filters_ValueChanged);
            // 
            // _lbSide
            // 
            appearance20.FontData.SizeInPoints = 9F;
            appearance20.TextHAlignAsString = "Left";
            appearance20.TextVAlignAsString = "Middle";
            this._lbSide.Appearance = appearance20;
            this._lbSide.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbSide.Location = new System.Drawing.Point(10, 35);
            this._lbSide.Name = "_lbSide";
            this._lbSide.Size = new System.Drawing.Size(28, 23);
            this._lbSide.TabIndex = 7;
            this._lbSide.Text = "Side";
            // 
            // _tbAccount
            // 
            appearance24.FontData.SizeInPoints = 9F;
            this._tbAccount.Appearance = appearance24;
            this._tbAccount.AutoSize = false;
            this._tbAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tbAccount.Location = new System.Drawing.Point(536, 7);
            this._tbAccount.Name = "_tbAccount";
            this._tbAccount.Size = new System.Drawing.Size(182, 23);
            this._tbAccount.TabIndex = 3;
            ultraToolTipInfo3.Enabled = Infragistics.Win.DefaultableBoolean.True;
            ultraToolTipInfo3.ToolTipImage = Infragistics.Win.ToolTipImage.None;
            ultraToolTipInfo3.ToolTipText = "Comma Separated AccountIDs";
            this.ultraToolTipManager1.SetUltraToolTip(this._tbAccount, ultraToolTipInfo3);
            this._tbAccount.Values = null;
            this._tbAccount.ValueChanged += new System.EventHandler(this.filters_ValueChanged);
            // 
            // _tbSymbol
            // 
            appearance21.FontData.SizeInPoints = 9F;
            this._tbSymbol.Appearance = appearance21;
            this._tbSymbol.AutoSize = false;
            this._tbSymbol.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tbSymbol.Location = new System.Drawing.Point(395, 7);
            this._tbSymbol.Name = "_tbSymbol";
            this._tbSymbol.Size = new System.Drawing.Size(81, 23);
            this._tbSymbol.TabIndex = 2;
            ultraToolTipInfo1.Enabled = Infragistics.Win.DefaultableBoolean.True;
            ultraToolTipInfo1.ToolTipImage = Infragistics.Win.ToolTipImage.None;
            ultraToolTipInfo1.ToolTipText = "Symbol";
            this.ultraToolTipManager1.SetUltraToolTip(this._tbSymbol, ultraToolTipInfo1);
            this._tbSymbol.Values = null;
            this._tbSymbol.ValueChanged += new System.EventHandler(this.filters_ValueChanged);
            this._tbSymbol.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._tbSymbol_KeyPress);
            // 
            // _lbTo
            // 
            appearance17.FontData.SizeInPoints = 9F;
            appearance17.TextHAlignAsString = "Left";
            appearance17.TextVAlignAsString = "Middle";
            this._lbTo.Appearance = appearance17;
            this._lbTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lbTo.Location = new System.Drawing.Point(170, 7);
            this._lbTo.Name = "_lbTo";
            this._lbTo.Size = new System.Drawing.Size(28, 23);
            this._lbTo.TabIndex = 5;
            this._lbTo.Text = "To";
            // 
            // _lbAccount
            // 
            appearance15.FontData.SizeInPoints = 9F;
            appearance15.TextHAlignAsString = "Left";
            appearance15.TextVAlignAsString = "Middle";
            this._lbAccount.Appearance = appearance15;
            this._lbAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbAccount.Location = new System.Drawing.Point(482, 7);
            this._lbAccount.Name = "_lbAccount";
            this._lbAccount.Size = new System.Drawing.Size(49, 23);
            this._lbAccount.TabIndex = 4;
            this._lbAccount.Text = "Account";

            // 
            // _lbSource
            // 
            this._lbSource.Appearance = appearance15;
            this._lbSource.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbSource.Location = new System.Drawing.Point(730, 7);
            this._lbSource.Name = "_lbSource";
            this._lbSource.Size = new System.Drawing.Size(49, 23);
            this._lbSource.TabIndex = 5;
            this._lbSource.Text = "Source";

            // 
            // MultiSelectDropDown1
            // 
            this._multiSelectDropDown1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._multiSelectDropDown1.Location = new System.Drawing.Point(790, 7);
            this._multiSelectDropDown1.Name = "MultiSelectDropDown1";
            this._multiSelectDropDown1.Size = new System.Drawing.Size(150, 21);
            this._multiSelectDropDown1.TabIndex = 0;
            this._multiSelectDropDown1.TitleText = "Source";

            // 
            // _lbSymbol
            // 
            appearance16.FontData.SizeInPoints = 9F;
            appearance16.TextHAlignAsString = "Left";
            appearance16.TextVAlignAsString = "Middle";
            this._lbSymbol.Appearance = appearance16;
            this._lbSymbol.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbSymbol.Location = new System.Drawing.Point(333, 7);
            this._lbSymbol.Name = "_lbSymbol";
            this._lbSymbol.Size = new System.Drawing.Size(45, 23);
            this._lbSymbol.TabIndex = 4;
            this._lbSymbol.Text = "Symbol";
            // 
            // _lbFrom
            // 
            appearance19.FontData.SizeInPoints = 9F;
            appearance19.TextHAlignAsString = "Left";
            appearance19.TextVAlignAsString = "Middle";
            this._lbFrom.Appearance = appearance19;
            this._lbFrom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbFrom.Location = new System.Drawing.Point(10, 7);
            this._lbFrom.Name = "_lbFrom";
            this._lbFrom.Size = new System.Drawing.Size(38, 23);
            this._lbFrom.TabIndex = 4;
            this._lbFrom.Text = "From";
            // 
            // _lbGroupId
            // 
            appearance23.FontData.SizeInPoints = 9F;
            appearance23.TextHAlignAsString = "Left";
            appearance23.TextVAlignAsString = "Middle";
            this._lbGroupId.Appearance = appearance23;
            this._lbGroupId.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbGroupId.Location = new System.Drawing.Point(333, 35);
            this._lbGroupId.Name = "_lbGroupId";
            this._lbGroupId.Size = new System.Drawing.Size(100, 23);
            this._lbGroupId.TabIndex = 4;
            this._lbGroupId.Text = "GroupID/OrderID";
            // 
            // _tbGroupID
            // 
            appearance22.FontData.SizeInPoints = 9F;
            this._tbGroupID.Appearance = appearance22;
            this._tbGroupID.AutoSize = false;
            this._tbGroupID.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tbGroupID.Location = new System.Drawing.Point(455, 35);
            this._tbGroupID.Name = "_tbGroupID";
            this._tbGroupID.Size = new System.Drawing.Size(325, 23);
            this._tbGroupID.TabIndex = 5;
            ultraToolTipInfo2.Enabled = Infragistics.Win.DefaultableBoolean.True;
            ultraToolTipInfo2.ToolTipImage = Infragistics.Win.ToolTipImage.None;
            ultraToolTipInfo2.ToolTipText = "Comma Separated GroupIDs";
            this.ultraToolTipManager1.SetUltraToolTip(this._tbGroupID, ultraToolTipInfo2);
            this._tbGroupID.ValueChanged += new System.EventHandler(this._tbGroupID_ValueChanged);
            // 
            // _btGetData
            // 
            appearance18.FontData.SizeInPoints = 9F;
            this._btGetData.Appearance = appearance18;
            this._btGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btGetData.Location = new System.Drawing.Point(790, 37);
            this._btGetData.Name = "_btGetData";
            this._btGetData.Size = new System.Drawing.Size(97, 23);
            this._btGetData.TabIndex = 6;
            this._btGetData.Text = "Get Data";
            this._btGetData.Click += new System.EventHandler(this._btGetData_Click);
            // 
            // _btClearFilters
            // 
            appearance27.FontData.SizeInPoints = 9F;
            appearance27.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this._btClearFilters.Appearance = appearance27;
            this._btClearFilters.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btClearFilters.Location = new System.Drawing.Point(251, 3);
            this._btClearFilters.Name = "_btClearFilters";
            this._btClearFilters.Size = new System.Drawing.Size(115, 23);
            this._btClearFilters.TabIndex = 10;
            this._btClearFilters.Text = "Clear Filters";
            ultraToolTipInfo4.ToolTipText = "Reset Filters";
            this.ultraToolTipManager1.SetUltraToolTip(this._btClearFilters, ultraToolTipInfo4);
            this._btClearFilters.Click += new System.EventHandler(this._btClearFilters_Click);
            // 
            // _btExport
            // 
            appearance25.FontData.SizeInPoints = 9F;
            this._btExport.Appearance = appearance25;
            this._btExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btExport.Location = new System.Drawing.Point(3, 3);
            this._btExport.Name = "_btExport";
            this._btExport.Size = new System.Drawing.Size(130, 23);
            this._btExport.TabIndex = 5;
            this._btExport.Text = "Export To Excel";
            this._btExport.Click += new System.EventHandler(this._btExport_Click);
            // 
            // _btScreenShot
            // 
            appearance26.FontData.SizeInPoints = 9F;
            this._btScreenShot.Appearance = appearance26;
            this._btScreenShot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btScreenShot.Location = new System.Drawing.Point(139, 3);
            this._btScreenShot.Name = "_btScreenShot";
            this._btScreenShot.Size = new System.Drawing.Size(106, 23);
            this._btScreenShot.TabIndex = 5;
            this._btScreenShot.Text = "Screenshot";
            this._btScreenShot.Click += new System.EventHandler(this._btScreenShot_Click);
            // 
            // _grdTradeAuditContextMenu
            // 
            this._grdTradeAuditContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._grdTradeAuditMenuItemCopy,
            this._grdTradeAuditDetailsToolStripMenuItem,
            this._grdAuditSaveLayoutToolStripMenuItem});
            this._grdTradeAuditContextMenu.Name = "_grdContextMenu";
            this._grdTradeAuditContextMenu.Size = new System.Drawing.Size(138, 70);
            // 
            // _grdTradeAuditMenuItemCopy
            // 
            this._grdTradeAuditMenuItemCopy.Name = "_grdTradeAuditMenuItemCopy";
            this._grdTradeAuditMenuItemCopy.Size = new System.Drawing.Size(137, 22);
            this._grdTradeAuditMenuItemCopy.Text = "Copy";
            this._grdTradeAuditMenuItemCopy.Click += new System.EventHandler(this._grdMenuItemCopy_Click);
            // 
            // _grdTradeAuditDetailsToolStripMenuItem
            // 
            this._grdTradeAuditDetailsToolStripMenuItem.Name = "_grdTradeAuditDetailsToolStripMenuItem";
            this._grdTradeAuditDetailsToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this._grdTradeAuditDetailsToolStripMenuItem.Text = "Details";
            this._grdTradeAuditDetailsToolStripMenuItem.Click += new System.EventHandler(this.detailsToolStripMenuItem_Click);
            // 
            // _grdAuditSaveLayoutToolStripMenuItem
            // 
            this._grdAuditSaveLayoutToolStripMenuItem.Name = "_grdAuditSaveLayoutToolStripMenuItem";
            this._grdAuditSaveLayoutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this._grdAuditSaveLayoutToolStripMenuItem.Text = "Save Layout";
            this._grdAuditSaveLayoutToolStripMenuItem.Click += new System.EventHandler(this._grdAuditSaveLayoutToolStripMenuItem_Click);
            // 
            // _grdTradeAudit
            // 
            appearance4.BackColor = System.Drawing.Color.Black;
            this._grdTradeAudit.DisplayLayout.Appearance = appearance4;
            this._grdTradeAudit.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this._grdTradeAudit.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this._grdTradeAudit.DisplayLayout.MaxBandDepth = 4;
            this._grdTradeAudit.DisplayLayout.MaxColScrollRegions = 1;
            this._grdTradeAudit.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            appearance5.ForeColor = System.Drawing.Color.Black;
            this._grdTradeAudit.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            this._grdTradeAudit.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._grdTradeAudit.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None;
            this._grdTradeAudit.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this._grdTradeAudit.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            appearance6.BackColor = System.Drawing.Color.Blue;
            appearance6.ForeColor = System.Drawing.Color.White;
            this._grdTradeAudit.DisplayLayout.Override.DataErrorRowAppearance = appearance6;
            this._grdTradeAudit.DisplayLayout.Override.FilterClearButtonLocation = Infragistics.Win.UltraWinGrid.FilterClearButtonLocation.RowAndCell;
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.ForeColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._grdTradeAudit.DisplayLayout.Override.GroupByRowAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance8.BackColor2 = System.Drawing.Color.DarkGray;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Middle";
            this._grdTradeAudit.DisplayLayout.Override.HeaderAppearance = appearance8;
            this._grdTradeAudit.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._grdTradeAudit.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance9.ForeColor = System.Drawing.Color.White;
            appearance9.TextHAlignAsString = "Center";
            appearance9.TextVAlignAsString = "Middle";
            this._grdTradeAudit.DisplayLayout.Override.RowAlternateAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.Black;
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.TextHAlignAsString = "Center";
            appearance10.TextVAlignAsString = "Middle";
            this._grdTradeAudit.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.AntiqueWhite;
            appearance11.ForeColor = System.Drawing.Color.Black;
            this._grdTradeAudit.DisplayLayout.Override.SelectedRowAppearance = appearance11;
            this._grdTradeAudit.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.SingleAutoDrag;
            this._grdTradeAudit.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            this._grdTradeAudit.DisplayLayout.PriorityScrolling = true;
            this._grdTradeAudit.DisplayLayout.RowConnectorColor = System.Drawing.Color.Transparent;
            this._grdTradeAudit.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._grdTradeAudit.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._grdTradeAudit.DisplayLayout.UseFixedHeaders = true;
            this._grdTradeAudit.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._grdTradeAudit.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grdTradeAudit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._grdTradeAudit.Location = new System.Drawing.Point(0, 0);
            this._grdTradeAudit.Name = "_grdTradeAudit";
            this._grdTradeAudit.Size = new System.Drawing.Size(839, 240);
            this._grdTradeAudit.TabIndex = 3;
            this._grdTradeAudit.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this._grdTradeAudit.AfterExitEditMode += new System.EventHandler(this._grdTradeAudit_AfterExitEditMode);
            this._grdTradeAudit.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this._grdTradeAudit_BeforeEnterEditMode);
            this._grdTradeAudit.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this._grdTradeAudit_BeforeCustomRowFilterDialog);
            this._grdTradeAudit.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this._grdTradeAudit_BeforeColumnChooserDisplayed);
            this._grdTradeAudit.MouseDown += new System.Windows.Forms.MouseEventHandler(this._grdTradeAudit_MouseDown);
            // 
            // _grdGroupsTaxlots
            // 
            appearance12.BackColor = System.Drawing.Color.Black;
            this._grdGroupsTaxlots.DisplayLayout.Appearance = appearance12;
            this._grdGroupsTaxlots.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this._grdGroupsTaxlots.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this._grdGroupsTaxlots.DisplayLayout.MaxBandDepth = 4;
            this._grdGroupsTaxlots.DisplayLayout.MaxColScrollRegions = 1;
            this._grdGroupsTaxlots.DisplayLayout.MaxRowScrollRegions = 1;
            this._grdGroupsTaxlots.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._grdGroupsTaxlots.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None;
            this._grdGroupsTaxlots.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this._grdGroupsTaxlots.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this._grdGroupsTaxlots.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this._grdGroupsTaxlots.DisplayLayout.Override.FilterClearButtonLocation = Infragistics.Win.UltraWinGrid.FilterClearButtonLocation.RowAndCell;
            appearance13.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance13.BackColor2 = System.Drawing.Color.DarkGray;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.TextHAlignAsString = "Center";
            appearance13.TextVAlignAsString = "Middle";
            this._grdGroupsTaxlots.DisplayLayout.Override.HeaderAppearance = appearance13;
            this._grdGroupsTaxlots.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._grdGroupsTaxlots.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance14.BackColor = System.Drawing.Color.Black;
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.TextHAlignAsString = "Center";
            appearance14.TextVAlignAsString = "Middle";
            this._grdGroupsTaxlots.DisplayLayout.Override.RowAppearance = appearance14;
            this._grdGroupsTaxlots.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.SingleAutoDrag;
            this._grdGroupsTaxlots.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            this._grdGroupsTaxlots.DisplayLayout.PriorityScrolling = true;
            this._grdGroupsTaxlots.DisplayLayout.RowConnectorColor = System.Drawing.Color.Transparent;
            this._grdGroupsTaxlots.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._grdGroupsTaxlots.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._grdGroupsTaxlots.DisplayLayout.UseFixedHeaders = true;
            this._grdGroupsTaxlots.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this._grdGroupsTaxlots.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grdGroupsTaxlots.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._grdGroupsTaxlots.Location = new System.Drawing.Point(0, 0);
            this._grdGroupsTaxlots.Name = "_grdGroupsTaxlots";
            this._grdGroupsTaxlots.Size = new System.Drawing.Size(839, 174);
            this._grdGroupsTaxlots.TabIndex = 4;
            this._grdGroupsTaxlots.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this._grdGroupsTaxlots.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this._grdGroupsTaxlots_InitializeRow);
            this._grdGroupsTaxlots.AfterExitEditMode += new System.EventHandler(this._grdGroupsTaxlots_AfterExitEditMode);
            this._grdGroupsTaxlots.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this._grdGroupsTaxlots_BeforeEnterEditMode);
            this._grdGroupsTaxlots.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this._grdGroupsTaxlots_BeforeCustomRowFilterDialog);
            this._grdGroupsTaxlots.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this._grdGroupsTaxlots_BeforeColumnChooserDisplayed);
            // 
            // _grdGroupsTaxlotsContextMenuStrip
            // 
            this._grdGroupsTaxlotsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._grdTaxlotGroupCopyMenuItem});
            this._grdGroupsTaxlotsContextMenuStrip.Name = "_grdGroupsTaxlotsContextMenuStrip";
            this._grdGroupsTaxlotsContextMenuStrip.Size = new System.Drawing.Size(103, 26);
            // 
            // _grdTaxlotGroupCopyMenuItem
            // 
            this._grdTaxlotGroupCopyMenuItem.Name = "_grdTaxlotGroupCopyMenuItem";
            this._grdTaxlotGroupCopyMenuItem.Size = new System.Drawing.Size(102, 22);
            this._grdTaxlotGroupCopyMenuItem.Text = "Copy";
            this._grdTaxlotGroupCopyMenuItem.Click += new System.EventHandler(this._grdTaxlotGroupCopyMenuItem_Click);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this._btExport);
            this.ultraPanel1.ClientArea.Controls.Add(this._btScreenShot);
            this.ultraPanel1.ClientArea.Controls.Add(this._btClearFilters);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(839, 28);
            this.ultraPanel1.TabIndex = 14;
            // 
            // ultraToolTipManager1
            // 
            this.ultraToolTipManager1.ContainingControl = this;
            this.ultraToolTipManager1.DisplayStyle = Infragistics.Win.ToolTipDisplayStyle.WindowsVista;
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this._grdTradeAudit);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 98);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(839, 240);
            this.ultraPanel2.TabIndex = 15;
            // 
            // ultraPanel3
            // 
            // 
            // ultraPanel3.ClientArea
            // 
            this.ultraPanel3.ClientArea.Controls.Add(this._grdGroupsTaxlots);
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 348);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(839, 174);
            this.ultraPanel3.TabIndex = 17;
            // 
            // ugbxGeaderAuditTrail
            // 
            this.ugbxGeaderAuditTrail.Controls.Add(this._uccFromDate);
            this.ugbxGeaderAuditTrail.Controls.Add(this._tbSide);
            this.ugbxGeaderAuditTrail.Controls.Add(this._lbAccount);
            this.ugbxGeaderAuditTrail.Controls.Add(this._lbSource);
            this.ugbxGeaderAuditTrail.Controls.Add(this._multiSelectDropDown1);
            
            this.ugbxGeaderAuditTrail.Controls.Add(this._uccToDate);
            this.ugbxGeaderAuditTrail.Controls.Add(this._lbSymbol);
            this.ugbxGeaderAuditTrail.Controls.Add(this._lbTo);
            this.ugbxGeaderAuditTrail.Controls.Add(this._btGetData);
            this.ugbxGeaderAuditTrail.Controls.Add(this._lbFrom);
            this.ugbxGeaderAuditTrail.Controls.Add(this._lbSide);
            this.ugbxGeaderAuditTrail.Controls.Add(this._tbSymbol);
            this.ugbxGeaderAuditTrail.Controls.Add(this._tbGroupID);
            this.ugbxGeaderAuditTrail.Controls.Add(this._lbGroupId);
            this.ugbxGeaderAuditTrail.Controls.Add(this._tbAccount);
            this.ugbxGeaderAuditTrail.Dock = System.Windows.Forms.DockStyle.Top;
            this.ugbxGeaderAuditTrail.Location = new System.Drawing.Point(0, 28);
            this.ugbxGeaderAuditTrail.Name = "ugbxGeaderAuditTrail";
            this.ugbxGeaderAuditTrail.Size = new System.Drawing.Size(839, 70);
            this.ugbxGeaderAuditTrail.TabIndex = 16;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 338);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 174;
            this.ultraSplitter1.Size = new System.Drawing.Size(839, 10);
            this.ultraSplitter1.TabIndex = 18;
            // 
            // ctrlAuditTrail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.ultraPanel3);
            this.Controls.Add(this.ugbxGeaderAuditTrail);
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlAuditTrail";
            this.Size = new System.Drawing.Size(839, 522);
            this.Load += new System.EventHandler(this.ctrlAuditTrail_Load);
            ((System.ComponentModel.ISupportInitialize)(this._uccFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._uccToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._tbSide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._tbAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._tbSymbol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._tbGroupID)).EndInit();
            this._grdTradeAuditContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grdTradeAudit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._grdGroupsTaxlots)).EndInit();
            this._grdGroupsTaxlotsContextMenuStrip.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel3.ClientArea.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxGeaderAuditTrail)).EndInit();
            this.ugbxGeaderAuditTrail.ResumeLayout(false);
            this.ugbxGeaderAuditTrail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel _lbFrom;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor _uccFromDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor _uccToDate;
        public Infragistics.Win.Misc.UltraButton _btGetData;
        private Infragistics.Win.Misc.UltraLabel _lbTo;
        private Infragistics.Win.Misc.UltraLabel _lbAccount;
        private Infragistics.Win.Misc.UltraLabel _lbSource;
        private Prana.Utilities.UI.UIUtilities.MultiSelectDropDown _multiSelectDropDown1;
        private Infragistics.Win.Misc.UltraLabel _lbSymbol;
        private Prana.Utilities.UI.UIUtilities.AutoCompleteTextBox _tbSymbol;
        private Prana.Utilities.UI.UIUtilities.AutoCompleteTextBox _tbAccount;
        private Infragistics.Win.Misc.UltraLabel _lbSide;
        private Prana.Utilities.UI.UIUtilities.AutoCompleteTextBox _tbSide;
        private Infragistics.Win.Misc.UltraLabel _lbGroupId;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private Infragistics.Win.Misc.UltraButton _btExport;
        private Infragistics.Win.Misc.UltraButton _btScreenShot;
        private Infragistics.Win.Misc.UltraButton _btClearFilters;
        private System.Windows.Forms.ContextMenuStrip _grdTradeAuditContextMenu;
        private System.Windows.Forms.ToolStripMenuItem _grdTradeAuditMenuItemCopy;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor _tbGroupID;
        private Infragistics.Win.UltraWinGrid.UltraGrid _grdTradeAudit;
        private Infragistics.Win.UltraWinGrid.UltraGrid _grdGroupsTaxlots;
        private System.Windows.Forms.ToolStripMenuItem _grdTradeAuditDetailsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip _grdGroupsTaxlotsContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _grdTaxlotGroupCopyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _grdAuditSaveLayoutToolStripMenuItem;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private Infragistics.Win.Misc.UltraGroupBox ugbxGeaderAuditTrail;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
    }
}