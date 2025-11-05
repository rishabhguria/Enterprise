namespace Prana.PortfolioReports.Controls
{
    partial class CntrlRealizedPL
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
            if(saveFileDialog1 != null)
            {
                saveFileDialog1.Dispose();
            }
            if(_gridBandRealizedPL != null)
            {
                _gridBandRealizedPL.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ToBeIncluded", 0);
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            this.cmbReportType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblReportType = new Infragistics.Win.Misc.UltraLabel();
            this.lblEndDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtEndDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblStartDate = new Infragistics.Win.Misc.UltraLabel();
            this.btnExportToExcel = new Infragistics.Win.Misc.UltraButton();
            this.btnGetDetailedReport = new Infragistics.Win.Misc.UltraButton();
            this.btnGenerateReport = new Infragistics.Win.Misc.UltraButton();
            this.dtStartDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.grdRealizedPL = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cmbReportType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRealizedPL)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbReportType
            // 
            this.cmbReportType.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbReportType.DisplayLayout.Appearance = appearance1;
            this.cmbReportType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbReportType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbReportType.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbReportType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbReportType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbReportType.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbReportType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbReportType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbReportType.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbReportType.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbReportType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbReportType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbReportType.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbReportType.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbReportType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbReportType.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbReportType.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbReportType.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbReportType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbReportType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbReportType.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbReportType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbReportType.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbReportType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbReportType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbReportType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbReportType.DisplayMember = "";
            this.cmbReportType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbReportType.Location = new System.Drawing.Point(404, 2);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new System.Drawing.Size(100, 21);
            this.cmbReportType.TabIndex = 25;
            this.cmbReportType.ValueMember = "";
            this.cmbReportType.ValueChanged += new System.EventHandler(this.cmbReportType_ValueChanged);
            // 
            // lblReportType
            // 
            this.lblReportType.Location = new System.Drawing.Point(335, 7);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(70, 16);
            this.lblReportType.TabIndex = 24;
            this.lblReportType.Text = "Report Type";
            // 
            // lblEndDate
            // 
            this.lblEndDate.Location = new System.Drawing.Point(169, 6);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(60, 16);
            this.lblEndDate.TabIndex = 23;
            this.lblEndDate.Text = "End Date";
            // 
            // dtEndDate
            // 
            this.dtEndDate.DateTime = new System.DateTime(2007, 5, 30, 0, 0, 0, 0);
            this.dtEndDate.Location = new System.Drawing.Point(226, 3);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(86, 21);
            this.dtEndDate.TabIndex = 22;
            this.dtEndDate.Value = new System.DateTime(2007, 5, 30, 0, 0, 0, 0);
            // 
            // lblStartDate
            // 
            this.lblStartDate.Location = new System.Drawing.Point(13, 6);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(60, 16);
            this.lblStartDate.TabIndex = 21;
            this.lblStartDate.Text = "Start Date";
            // 
            // btnExportToExcel
            // 
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnExportToExcel.Appearance = appearance13;
            this.btnExportToExcel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnExportToExcel.Location = new System.Drawing.Point(658, 1);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(49, 23);
            this.btnExportToExcel.TabIndex = 20;
            this.btnExportToExcel.Text = "Export";
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // btnGetDetailedReport
            // 
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnGetDetailedReport.Appearance = appearance14;
            this.btnGetDetailedReport.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnGetDetailedReport.Location = new System.Drawing.Point(722, 0);
            this.btnGetDetailedReport.Name = "btnGetDetailedReport";
            this.btnGetDetailedReport.Size = new System.Drawing.Size(71, 23);
            this.btnGetDetailedReport.TabIndex = 19;
            this.btnGetDetailedReport.Text = "Get Details";
            this.btnGetDetailedReport.Visible = false;
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Location = new System.Drawing.Point(581, 1);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(71, 23);
            this.btnGenerateReport.TabIndex = 18;
            this.btnGenerateReport.Text = "Generate";
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // dtStartDate
            // 
            this.dtStartDate.Location = new System.Drawing.Point(70, 3);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(86, 21);
            this.dtStartDate.TabIndex = 17;
            // 
            // grdRealizedPL
            // 
            this.grdRealizedPL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance15.BackColor = System.Drawing.Color.Black;
            this.grdRealizedPL.DisplayLayout.Appearance = appearance15;
            this.grdRealizedPL.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            ultraGridColumn1.DataType = typeof(bool);
            ultraGridColumn1.DefaultCellValue = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            ultraGridColumn1.Width = 795;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1});
            this.grdRealizedPL.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdRealizedPL.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdRealizedPL.DisplayLayout.InterBandSpacing = 1;
            appearance16.BackColor = System.Drawing.Color.Gold;
            appearance16.BorderColor = System.Drawing.Color.Black;
            appearance16.ForeColor = System.Drawing.Color.Black;
            this.grdRealizedPL.DisplayLayout.Override.ActiveRowAppearance = appearance16;
            this.grdRealizedPL.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdRealizedPL.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdRealizedPL.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdRealizedPL.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdRealizedPL.DisplayLayout.Override.CellPadding = 0;
            this.grdRealizedPL.DisplayLayout.Override.DefaultColWidth = 50;
            this.grdRealizedPL.DisplayLayout.Override.GroupByRowInitialExpansionState = Infragistics.Win.UltraWinGrid.GroupByRowInitialExpansionState.Expanded;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdRealizedPL.DisplayLayout.Override.HeaderAppearance = appearance17;
            this.grdRealizedPL.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdRealizedPL.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance18.BackColor = System.Drawing.Color.Black;
            appearance18.ForeColor = System.Drawing.Color.Orange;
            this.grdRealizedPL.DisplayLayout.Override.RowAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdRealizedPL.DisplayLayout.Override.RowSelectorAppearance = appearance19;
            this.grdRealizedPL.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.grdRealizedPL.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdRealizedPL.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdRealizedPL.DisplayLayout.Override.SpecialRowSeparator = ((Infragistics.Win.UltraWinGrid.SpecialRowSeparator)(((Infragistics.Win.UltraWinGrid.SpecialRowSeparator.TemplateAddRow | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FixedRows)
                        | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.SummaryRow)));
            appearance20.BackColor = System.Drawing.SystemColors.Info;
            this.grdRealizedPL.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance20;
            this.grdRealizedPL.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.grdRealizedPL.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            scrollBarLook1.Appearance = appearance21;
            this.grdRealizedPL.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdRealizedPL.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdRealizedPL.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdRealizedPL.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdRealizedPL.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grdRealizedPL.Location = new System.Drawing.Point(3, 31);
            this.grdRealizedPL.Name = "grdRealizedPL";
            this.grdRealizedPL.Size = new System.Drawing.Size(797, 344);
            this.grdRealizedPL.TabIndex = 16;
            this.grdRealizedPL.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdRealizedPL_InitializeLayout);
            // 
            // CntrlRealizedPL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbReportType);
            this.Controls.Add(this.lblReportType);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.dtEndDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.btnExportToExcel);
            this.Controls.Add(this.btnGetDetailedReport);
            this.Controls.Add(this.btnGenerateReport);
            this.Controls.Add(this.dtStartDate);
            this.Controls.Add(this.grdRealizedPL);
            this.Name = "CntrlRealizedPL";
            this.Size = new System.Drawing.Size(804, 378);
            ((System.ComponentModel.ISupportInitialize)(this.cmbReportType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdRealizedPL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraCombo cmbReportType;
        private Infragistics.Win.Misc.UltraLabel lblReportType;
        private Infragistics.Win.Misc.UltraLabel lblEndDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtEndDate;
        private Infragistics.Win.Misc.UltraLabel lblStartDate;
        private Infragistics.Win.Misc.UltraButton btnExportToExcel;
        private Infragistics.Win.Misc.UltraButton btnGetDetailedReport;
        private Infragistics.Win.Misc.UltraButton btnGenerateReport;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtStartDate;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdRealizedPL;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
    }
}
