namespace Prana.CashManagement
{
	partial class CashTransactions
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.grdCashDividends = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuCashDividends = new System.Windows.Forms.ContextMenu();
            this.menuAdd = new System.Windows.Forms.MenuItem();
            this.menuDeleteRow = new System.Windows.Forms.MenuItem();
            this.btnGet = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.txtBoxSymbol = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblTo = new Infragistics.Win.Misc.UltraLabel();
            this.lblFrom = new Infragistics.Win.Misc.UltraLabel();
            this.dtTo = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtFrom = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.lblMatchOn = new Infragistics.Win.Misc.UltraLabel();
            this.cbMatchContains = new System.Windows.Forms.RadioButton();
            this.cbMatchExact = new System.Windows.Forms.RadioButton();
            this.cbMatchStartsWith = new System.Windows.Forms.RadioButton();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.btnExport = new Infragistics.Win.Misc.UltraButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDateType = new Infragistics.Win.Misc.UltraLabel();
            this.dtDateType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblAccount = new Infragistics.Win.Misc.UltraLabel();
            this.MultiSelectDropDown1 = new Prana.Utilities.UIUtilities.MultiSelectDropDown();
            this.CashTransactions_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            //this.ctrlImageListButtons1 = new Prana.Utilities.UIUtilities.CtrlImageListButtons(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._CashTransactions_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CashTransactions_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CashTransactions_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CashTransactions_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.grdCashDividends)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBoxSymbol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFrom)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtDateType)).BeginInit();
            this.CashTransactions_Fill_Panel.ClientArea.SuspendLayout();
            this.CashTransactions_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // grdCashDividends
            // 
            this.grdCashDividends.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCashDividends.ContextMenu = this.contextMenuCashDividends;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdCashDividends.DisplayLayout.Appearance = appearance1;
            this.grdCashDividends.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdCashDividends.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdCashDividends.DisplayLayout.GroupByBox.Hidden = true;
            this.grdCashDividends.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCashDividends.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.LightSlateGray;
            appearance2.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance2.BorderColor = System.Drawing.Color.DimGray;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.grdCashDividends.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.grdCashDividends.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdCashDividends.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCashDividends.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.grdCashDividends.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashDividends.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdCashDividends.DisplayLayout.Override.CellPadding = 0;
            this.grdCashDividends.DisplayLayout.Override.CellSpacing = 0;
            appearance3.FontData.Name = "Segoe UI";
            appearance3.FontData.SizeInPoints = 9F;
            appearance3.TextHAlignAsString = "Center";
            this.grdCashDividends.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdCashDividends.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdCashDividends.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.grdCashDividends.DisplayLayout.Override.RowAlternateAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.Black;
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Right";
            appearance5.TextVAlignAsString = "Middle";
            this.grdCashDividends.DisplayLayout.Override.RowAppearance = appearance5;
            this.grdCashDividends.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashDividends.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            appearance6.FontData.BoldAsString = "True";
            this.grdCashDividends.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.grdCashDividends.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCashDividends.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCashDividends.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCashDividends.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCashDividends.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdCashDividends.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdCashDividends.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance7.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdCashDividends.DisplayLayout.Override.TemplateAddRowAppearance = appearance7;
            this.grdCashDividends.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCashDividends.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCashDividends.DisplayLayout.UseFixedHeaders = true;
            this.grdCashDividends.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCashDividends.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grdCashDividends.Location = new System.Drawing.Point(0, 89);
            this.grdCashDividends.Margin = new System.Windows.Forms.Padding(4);
            this.grdCashDividends.Name = "grdCashDividends";
            this.grdCashDividends.Size = new System.Drawing.Size(1047, 306);
            this.grdCashDividends.TabIndex = 17;
            this.grdCashDividends.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashDividends.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCashDividends_AfterCellUpdate);
            this.grdCashDividends.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdCashDividends_InitializeRow);
            this.grdCashDividends.AfterRowActivate += new System.EventHandler(this.grdCashDividends_AfterRowActivate);
            this.grdCashDividends.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCashDividends_CellChange);
            this.grdCashDividends.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdCashDividends_BeforeCustomRowFilterDialog);
            this.grdCashDividends.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdCashDividends_BeforeColumnChooserDisplayed);
            this.grdCashDividends.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdCashDividends_MouseDown);
            this.grdCashDividends.CellDataError += grdCashDividends_CellDataError;
            // 
            // contextMenuCashDividends
            // 
            this.contextMenuCashDividends.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuAdd,
            this.menuDeleteRow});
            this.contextMenuCashDividends.Popup += new System.EventHandler(this.contextMenuCashDividends_Popup);
            // 
            // menuAdd
            // 
            this.menuAdd.Index = 0;
            this.menuAdd.Text = "Add";
            this.menuAdd.Click += new System.EventHandler(this.menuAdd_Click);
            // 
            // menuDeleteRow
            // 
            this.menuDeleteRow.Index = 1;
            this.menuDeleteRow.Text = "Delete";
            this.menuDeleteRow.Click += new System.EventHandler(this.menuDeleteRow_Click);
            // 
            // btnGet
            // 
            this.btnGet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGet.Location = new System.Drawing.Point(797, 14);
            this.btnGet.Margin = new System.Windows.Forms.Padding(4);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(80, 23);
            this.btnGet.TabIndex = 14;
            this.btnGet.Text = "Get";
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(885, 14);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtBoxSymbol
            // 
            this.txtBoxSymbol.AutoSize = false;
            this.txtBoxSymbol.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBoxSymbol.Location = new System.Drawing.Point(81, 53);
            this.txtBoxSymbol.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxSymbol.Name = "txtBoxSymbol";
            this.txtBoxSymbol.Size = new System.Drawing.Size(177, 23);
            this.txtBoxSymbol.TabIndex = 1;
            // 
            // lblTo
            // 
            appearance8.TextHAlignAsString = "Left";
            appearance8.TextVAlignAsString = "Middle";
            this.lblTo.Appearance = appearance8;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.Location = new System.Drawing.Point(630, 16);
            this.lblTo.Margin = new System.Windows.Forms.Padding(4);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(21, 18);
            this.lblTo.TabIndex = 12;
            this.lblTo.Text = "to";
            // 
            // lblFrom
            // 
            appearance9.TextHAlignAsString = "Left";
            appearance9.TextVAlignAsString = "Middle";
            this.lblFrom.Appearance = appearance9;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(456, 16);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(4);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 18);
            this.lblFrom.TabIndex = 10;
            this.lblFrom.Text = "From";
            // 
            // dtTo
            // 
            this.dtTo.AutoSize = false;
            this.dtTo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtTo.Location = new System.Drawing.Point(658, 14);
            this.dtTo.Margin = new System.Windows.Forms.Padding(4);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(115, 23);
            this.dtTo.TabIndex = 13;
            // 
            // dtFrom
            // 
            this.dtFrom.AutoSize = false;
            this.dtFrom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFrom.Location = new System.Drawing.Point(508, 14);
            this.dtFrom.Margin = new System.Windows.Forms.Padding(4);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(115, 23);
            this.dtFrom.TabIndex = 11;
            // 
            // lblSymbol
            // 
            appearance10.TextHAlignAsString = "Left";
            appearance10.TextVAlignAsString = "Middle";
            this.lblSymbol.Appearance = appearance10;
            this.lblSymbol.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSymbol.Location = new System.Drawing.Point(17, 55);
            this.lblSymbol.Margin = new System.Windows.Forms.Padding(4);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(51, 18);
            this.lblSymbol.TabIndex = 0;
            this.lblSymbol.Text = "Symbol";
            // 
            // lblMatchOn
            // 
            this.lblMatchOn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatchOn.Location = new System.Drawing.Point(279, 55);
            this.lblMatchOn.Margin = new System.Windows.Forms.Padding(4);
            this.lblMatchOn.Name = "lblMatchOn";
            this.lblMatchOn.Size = new System.Drawing.Size(63, 18);
            this.lblMatchOn.TabIndex = 2;
            this.lblMatchOn.Text = "Match On";
            // 
            // cbMatchContains
            // 
            this.cbMatchContains.AutoSize = true;
            this.cbMatchContains.Location = new System.Drawing.Point(348, 54);
            this.cbMatchContains.Margin = new System.Windows.Forms.Padding(4);
            this.cbMatchContains.Name = "cbMatchContains";
            this.cbMatchContains.Size = new System.Drawing.Size(72, 19);
            this.inboxControlStyler1.SetStyleSettings(this.cbMatchContains, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.cbMatchContains.TabIndex = 3;
            this.cbMatchContains.TabStop = true;
            this.cbMatchContains.Text = "Contains";
            this.cbMatchContains.UseVisualStyleBackColor = true;
            // 
            // cbMatchExact
            // 
            this.cbMatchExact.AutoSize = true;
            this.cbMatchExact.Checked = true;
            this.cbMatchExact.Location = new System.Drawing.Point(434, 54);
            this.cbMatchExact.Margin = new System.Windows.Forms.Padding(4);
            this.cbMatchExact.Name = "cbMatchExact";
            this.cbMatchExact.Size = new System.Drawing.Size(52, 19);
            this.inboxControlStyler1.SetStyleSettings(this.cbMatchExact, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.cbMatchExact.TabIndex = 4;
            this.cbMatchExact.TabStop = true;
            this.cbMatchExact.Text = "Exact";
            this.cbMatchExact.UseVisualStyleBackColor = true;
            // 
            // cbMatchStartsWith
            // 
            this.cbMatchStartsWith.AutoSize = true;
            this.cbMatchStartsWith.Location = new System.Drawing.Point(500, 54);
            this.cbMatchStartsWith.Margin = new System.Windows.Forms.Padding(4);
            this.cbMatchStartsWith.Name = "cbMatchStartsWith";
            this.cbMatchStartsWith.Size = new System.Drawing.Size(82, 19);
            this.inboxControlStyler1.SetStyleSettings(this.cbMatchStartsWith, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.cbMatchStartsWith.TabIndex = 5;
            this.cbMatchStartsWith.TabStop = true;
            this.cbMatchStartsWith.Text = "Starts With";
            this.cbMatchStartsWith.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(973, 14);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 23);
            this.btnExport.TabIndex = 16;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(4, 452);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1047, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // lblDateType
            // 
            appearance11.TextHAlignAsString = "Left";
            appearance11.TextVAlignAsString = "Middle";
            this.lblDateType.Appearance = appearance11;
            this.lblDateType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateType.Location = new System.Drawing.Point(264, 16);
            this.lblDateType.Margin = new System.Windows.Forms.Padding(4);
            this.lblDateType.Name = "lblDateType";
            this.lblDateType.Size = new System.Drawing.Size(61, 18);
            this.lblDateType.TabIndex = 8;
            this.lblDateType.Text = "Date Type";
            // 
            // dtDateType
            // 
            this.dtDateType.AutoSize = false;
            this.dtDateType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.dtDateType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            valueListItem1.DataValue = "ExDate";
            valueListItem1.DisplayText = "ExDate";
            valueListItem2.DataValue = "PayoutDate";
            valueListItem2.DisplayText = "PayoutDate";
            valueListItem3.DataValue = "RecordDate";
            valueListItem3.DisplayText = "RecordDate";
            valueListItem4.DataValue = "DeclarationDate";
            valueListItem4.DisplayText = "DeclarationDate";
            this.dtDateType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3,
            valueListItem4});
            this.dtDateType.Location = new System.Drawing.Point(330, 14);
            this.dtDateType.Margin = new System.Windows.Forms.Padding(4);
            this.dtDateType.Name = "dtDateType";
            this.dtDateType.Size = new System.Drawing.Size(115, 23);
            this.dtDateType.TabIndex = 9;
            this.dtDateType.Text = "ExDate";
            // 
            // lblAccount
            // 
            appearance12.TextHAlignAsString = "Left";
            appearance12.TextVAlignAsString = "Middle";
            this.lblAccount.Appearance = appearance12;
            this.lblAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccount.Location = new System.Drawing.Point(17, 16);
            this.lblAccount.Margin = new System.Windows.Forms.Padding(4);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(38, 18);
            this.lblAccount.TabIndex = 6;
            this.lblAccount.Text = "Account";
            // 
            // MultiSelectDropDown1
            // 
            this.MultiSelectDropDown1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.MultiSelectDropDown1.Location = new System.Drawing.Point(60, 14);
            this.MultiSelectDropDown1.Margin = new System.Windows.Forms.Padding(4);
            this.MultiSelectDropDown1.Name = "MultiSelectDropDown1";
            this.MultiSelectDropDown1.Size = new System.Drawing.Size(193, 23);
            this.MultiSelectDropDown1.TabIndex = 7;
            this.MultiSelectDropDown1.TitleText = "";
            // 
            // CashTransactions_Fill_Panel
            // 
            // 
            // CashTransactions_Fill_Panel.ClientArea
            // 
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.btnSave);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.lblAccount);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.MultiSelectDropDown1);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.lblDateType);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.dtDateType);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.lblSymbol);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.lblMatchOn);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.cbMatchStartsWith);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.cbMatchExact);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.cbMatchContains);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.lblTo);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.txtBoxSymbol);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.lblFrom);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.grdCashDividends);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.dtTo);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.dtFrom);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.btnExport);
            this.CashTransactions_Fill_Panel.ClientArea.Controls.Add(this.btnGet);
            this.CashTransactions_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.CashTransactions_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CashTransactions_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.CashTransactions_Fill_Panel.Margin = new System.Windows.Forms.Padding(4);
            this.CashTransactions_Fill_Panel.Name = "CashTransactions_Fill_Panel";
            this.CashTransactions_Fill_Panel.Size = new System.Drawing.Size(1047, 425);
            this.CashTransactions_Fill_Panel.TabIndex = 19;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _CashTransactions_UltraFormManager_Dock_Area_Left
            // 
            this._CashTransactions_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CashTransactions_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CashTransactions_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._CashTransactions_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CashTransactions_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._CashTransactions_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._CashTransactions_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._CashTransactions_UltraFormManager_Dock_Area_Left.Name = "_CashTransactions_UltraFormManager_Dock_Area_Left";
            this._CashTransactions_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 447);
            // 
            // _CashTransactions_UltraFormManager_Dock_Area_Right
            // 
            this._CashTransactions_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CashTransactions_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CashTransactions_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._CashTransactions_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CashTransactions_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._CashTransactions_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._CashTransactions_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1051, 27);
            this._CashTransactions_UltraFormManager_Dock_Area_Right.Name = "_CashTransactions_UltraFormManager_Dock_Area_Right";
            this._CashTransactions_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 447);
            // 
            // _CashTransactions_UltraFormManager_Dock_Area_Top
            // 
            this._CashTransactions_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CashTransactions_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CashTransactions_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._CashTransactions_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CashTransactions_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._CashTransactions_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._CashTransactions_UltraFormManager_Dock_Area_Top.Name = "_CashTransactions_UltraFormManager_Dock_Area_Top";
            this._CashTransactions_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1055, 27);
            // 
            // _CashTransactions_UltraFormManager_Dock_Area_Bottom
            // 
            this._CashTransactions_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CashTransactions_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CashTransactions_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._CashTransactions_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CashTransactions_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._CashTransactions_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._CashTransactions_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 474);
            this._CashTransactions_UltraFormManager_Dock_Area_Bottom.Name = "_CashTransactions_UltraFormManager_Dock_Area_Bottom";
            this._CashTransactions_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1055, 4);
            // 
            // CashTransactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1055, 478);
            this.Controls.Add(this.CashTransactions_Fill_Panel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._CashTransactions_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._CashTransactions_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._CashTransactions_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._CashTransactions_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CashTransactions";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Cash Transactions";
            this.Activated += new System.EventHandler(this.CashTransactions_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CashTransactions_FormClosing);
            this.Load += new System.EventHandler(this.CashTransactions_Load);
            this.Resize += new System.EventHandler(this.CashTransactions_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.grdCashDividends)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBoxSymbol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFrom)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtDateType)).EndInit();
            this.CashTransactions_Fill_Panel.ClientArea.ResumeLayout(false);
            this.CashTransactions_Fill_Panel.ClientArea.PerformLayout();
            this.CashTransactions_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdCashDividends;
        private Infragistics.Win.Misc.UltraButton btnGet;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtBoxSymbol;
        private Infragistics.Win.Misc.UltraLabel lblTo;
        private Infragistics.Win.Misc.UltraLabel lblFrom;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtTo;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFrom;
        private Infragistics.Win.Misc.UltraLabel lblSymbol;
        private Infragistics.Win.Misc.UltraLabel lblMatchOn;
        private System.Windows.Forms.RadioButton cbMatchContains;
        private System.Windows.Forms.RadioButton cbMatchExact;
        private System.Windows.Forms.RadioButton cbMatchStartsWith;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private Infragistics.Win.Misc.UltraButton btnExport;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.Misc.UltraLabel lblDateType;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor dtDateType;
        private Infragistics.Win.Misc.UltraLabel lblAccount;
        private Prana.Utilities.UIUtilities.MultiSelectDropDown MultiSelectDropDown1;
        private System.Windows.Forms.ContextMenu contextMenuCashDividends;
        private System.Windows.Forms.MenuItem menuAdd;
        private System.Windows.Forms.MenuItem menuDeleteRow;
        private Infragistics.Win.Misc.UltraPanel CashTransactions_Fill_Panel;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CashTransactions_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CashTransactions_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CashTransactions_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CashTransactions_UltraFormManager_Dock_Area_Bottom;
	}
}