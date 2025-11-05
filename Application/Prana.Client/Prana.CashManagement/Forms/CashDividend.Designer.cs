namespace Prana.CashManagement
{
	partial class CashDividends
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
            this.menuStripCashDividends = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuAddRow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteRow = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGet = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtBoxSymbol = new System.Windows.Forms.TextBox();
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
            this.btnExport = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDateType = new Infragistics.Win.Misc.UltraLabel();
            this.dtDateType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblFund = new Infragistics.Win.Misc.UltraLabel();
            this.MultiSelectDropDown1 = new Prana.Utilities.UIUtilities.MultiSelectDropDown();
            ((System.ComponentModel.ISupportInitialize)(this.grdCashDividends)).BeginInit();
            this.menuStripCashDividends.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFrom)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtDateType)).BeginInit();
            this.SuspendLayout();
            // 
            // grdCashDividends
            // 
            this.grdCashDividends.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCashDividends.ContextMenuStrip = this.menuStripCashDividends;
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
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8F;
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
            this.grdCashDividends.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdCashDividends.Location = new System.Drawing.Point(0, 72);
            this.grdCashDividends.Name = "grdCashDividends";
            this.grdCashDividends.Size = new System.Drawing.Size(956, 291);
            this.grdCashDividends.TabIndex = 17;
            this.grdCashDividends.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashDividends.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCashDividends_AfterCellUpdate);
            this.grdCashDividends.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdCashDividends_InitializeRow);
            this.grdCashDividends.AfterRowActivate += new System.EventHandler(this.grdCashDividends_AfterRowActivate);
            this.grdCashDividends.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCashDividends_CellChange);
            // 
            // menuStripCashDividends
            // 
            this.menuStripCashDividends.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddRow,
            this.mnuDeleteRow});
            this.menuStripCashDividends.Name = "menuStripCashValue";
            this.menuStripCashDividends.Size = new System.Drawing.Size(108, 48);
            // 
            // mnuAddRow
            // 
            this.mnuAddRow.Name = "mnuAddRow";
            this.mnuAddRow.Size = new System.Drawing.Size(107, 22);
            this.mnuAddRow.Text = "Add";
            this.mnuAddRow.Click += new System.EventHandler(this.mnuAddRow_Click);
            // 
            // mnuDeleteRow
            // 
            this.mnuDeleteRow.Name = "mnuDeleteRow";
            this.mnuDeleteRow.Size = new System.Drawing.Size(107, 22);
            this.mnuDeleteRow.Text = "Delete";
            this.mnuDeleteRow.Click += new System.EventHandler(this.mnuDeleteRow_Click);
            // 
            // btnGet
            // 
            this.btnGet.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGet.Location = new System.Drawing.Point(726, 7);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(60, 23);
            this.btnGet.TabIndex = 14;
            this.btnGet.Text = "Get";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(792, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtBoxSymbol
            // 
            this.txtBoxSymbol.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBoxSymbol.Location = new System.Drawing.Point(61, 42);
            this.txtBoxSymbol.Name = "txtBoxSymbol";
            this.txtBoxSymbol.Size = new System.Drawing.Size(134, 20);
            this.txtBoxSymbol.TabIndex = 1;
            // 
            // lblTo
            // 
            appearance8.TextHAlignAsString = "Left";
            appearance8.TextVAlignAsString = "Middle";
            this.lblTo.Appearance = appearance8;
            this.lblTo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.Location = new System.Drawing.Point(504, 13);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(16, 15);
            this.lblTo.TabIndex = 12;
            this.lblTo.Text = "to";
            // 
            // lblFrom
            // 
            appearance9.TextHAlignAsString = "Left";
            appearance9.TextVAlignAsString = "Middle";
            this.lblFrom.Appearance = appearance9;
            this.lblFrom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(367, 13);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(37, 15);
            this.lblFrom.TabIndex = 10;
            this.lblFrom.Text = "From";
            // 
            // dtTo
            // 
            this.dtTo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtTo.Location = new System.Drawing.Point(525, 9);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(86, 22);
            this.dtTo.TabIndex = 13;
            // 
            // dtFrom
            // 
            this.dtFrom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFrom.Location = new System.Drawing.Point(406, 9);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(86, 22);
            this.dtFrom.TabIndex = 11;
            // 
            // lblSymbol
            // 
            appearance10.TextHAlignAsString = "Left";
            appearance10.TextVAlignAsString = "Middle";
            this.lblSymbol.Appearance = appearance10;
            this.lblSymbol.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSymbol.Location = new System.Drawing.Point(13, 45);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(41, 15);
            this.lblSymbol.TabIndex = 0;
            this.lblSymbol.Text = "Symbol";
            // 
            // lblMatchOn
            // 
            this.lblMatchOn.Appearance = appearance10;
            this.lblMatchOn.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatchOn.Location = new System.Drawing.Point(209, 44);
            this.lblMatchOn.Name = "lblMatchOn";
            this.lblMatchOn.Size = new System.Drawing.Size(52, 15);
            this.lblMatchOn.TabIndex = 2;
            this.lblMatchOn.Text = "Match On";
            // 
            // cbMatchContains
            // 
            this.cbMatchContains.AutoSize = true;
            this.cbMatchContains.Location = new System.Drawing.Point(271, 42);
            this.cbMatchContains.Name = "cbMatchContains";
            this.cbMatchContains.Size = new System.Drawing.Size(66, 17);
            this.cbMatchContains.TabIndex = 3;
            this.cbMatchContains.TabStop = true;
            this.cbMatchContains.Text = "Contains";
            this.cbMatchContains.UseVisualStyleBackColor = true;
            // 
            // cbMatchExact
            // 
            this.cbMatchExact.AutoSize = true;
            this.cbMatchExact.Checked = true;
            this.cbMatchExact.Location = new System.Drawing.Point(341, 42);
            this.cbMatchExact.Name = "cbMatchExact";
            this.cbMatchExact.Size = new System.Drawing.Size(52, 17);
            this.cbMatchExact.TabIndex = 4;
            this.cbMatchExact.TabStop = true;
            this.cbMatchExact.Text = "Exact";
            this.cbMatchExact.UseVisualStyleBackColor = true;
            // 
            // cbMatchStartsWith
            // 
            this.cbMatchStartsWith.AutoSize = true;
            this.cbMatchStartsWith.Location = new System.Drawing.Point(397, 42);
            this.cbMatchStartsWith.Name = "cbMatchStartsWith";
            this.cbMatchStartsWith.Size = new System.Drawing.Size(77, 17);
            this.cbMatchStartsWith.TabIndex = 5;
            this.cbMatchStartsWith.TabStop = true;
            this.cbMatchStartsWith.Text = "Starts With";
            this.cbMatchStartsWith.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(858, 7);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(60, 23);
            this.btnExport.TabIndex = 16;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 366);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(956, 22);
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
            this.lblDateType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateType.Location = new System.Drawing.Point(209, 13);
            this.lblDateType.Name = "lblDateType";
            this.lblDateType.Size = new System.Drawing.Size(58, 15);
            this.lblDateType.TabIndex = 8;
            this.lblDateType.Text = "Date Type";
            // 
            // dtDateType
            // 
            this.dtDateType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.dtDateType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.dtDateType.Location = new System.Drawing.Point(272, 9);
            this.dtDateType.Name = "dtDateType";
            this.dtDateType.Size = new System.Drawing.Size(86, 22);
            this.dtDateType.TabIndex = 9;
            this.dtDateType.Text = "ExDate";
            // 
            // lblFund
            // 
            appearance12.TextHAlignAsString = "Left";
            appearance12.TextVAlignAsString = "Middle";
            this.lblFund.Appearance = appearance12;
            this.lblFund.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFund.Location = new System.Drawing.Point(13, 13);
            this.lblFund.Name = "lblFund";
            this.lblFund.Size = new System.Drawing.Size(32, 15);
            this.lblFund.TabIndex = 6;
            this.lblFund.Text = "Fund";
            // 
            // MultiSelectDropDown1
            // 
            this.MultiSelectDropDown1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.MultiSelectDropDown1.Location = new System.Drawing.Point(50, 9);
            this.MultiSelectDropDown1.Name = "MultiSelectDropDown1";
            this.MultiSelectDropDown1.Size = new System.Drawing.Size(145, 21);
            this.MultiSelectDropDown1.TabIndex = 7;
            // 
            // CashDividends
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 388);
            this.Controls.Add(this.lblFund);
            this.Controls.Add(this.MultiSelectDropDown1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblDateType);
            this.Controls.Add(this.dtDateType);
            this.Controls.Add(this.lblSymbol);
            this.Controls.Add(this.lblMatchOn);
            this.Controls.Add(this.cbMatchStartsWith);
            this.Controls.Add(this.cbMatchExact);
            this.Controls.Add(this.cbMatchContains);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.txtBoxSymbol);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.grdCashDividends);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnGet);
            this.Name = "CashDividends";
            this.Text = "Cash Dividends";
            ((System.ComponentModel.ISupportInitialize)(this.grdCashDividends)).EndInit();
            this.menuStripCashDividends.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFrom)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtDateType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdCashDividends;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtBoxSymbol;
        private Infragistics.Win.Misc.UltraLabel lblTo;
        private Infragistics.Win.Misc.UltraLabel lblFrom;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtTo;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFrom;
        private Infragistics.Win.Misc.UltraLabel lblSymbol;
        private Infragistics.Win.Misc.UltraLabel lblMatchOn;
        private System.Windows.Forms.RadioButton cbMatchContains;
        private System.Windows.Forms.RadioButton cbMatchExact;
        private System.Windows.Forms.RadioButton cbMatchStartsWith;
        private System.Windows.Forms.ContextMenuStrip menuStripCashDividends;
        private System.Windows.Forms.ToolStripMenuItem mnuAddRow;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteRow;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.Misc.UltraLabel lblDateType;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor dtDateType;
        private Infragistics.Win.Misc.UltraLabel lblFund;
        private Prana.Utilities.UIUtilities.MultiSelectDropDown MultiSelectDropDown1;
	}
}