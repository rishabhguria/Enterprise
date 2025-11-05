namespace Prana.PortfolioReports
{
    partial class ClosingHistory
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
                if (saveFileDialog1 != null)
                {
                    saveFileDialog1.Dispose();
                }
                if (_gridBandHistoryPositions != null)
                {
                    _gridBandHistoryPositions.Dispose();
                }
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
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
            this.btnExportToExcel = new Infragistics.Win.Misc.UltraButton();
            this.btnGenerateReport = new Infragistics.Win.Misc.UltraButton();
            this.dtStartDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.grdHistoryPositions = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.lblStartDate = new Infragistics.Win.Misc.UltraLabel();
            this.cmbReportType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblReportType = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.lblEndDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtEndDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistoryPositions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReportType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEndDate)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExportToExcel
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnExportToExcel.Appearance = appearance1;
            this.btnExportToExcel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnExportToExcel.Location = new System.Drawing.Point(666, 2);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(49, 23);
            this.btnExportToExcel.TabIndex = 40;
            this.btnExportToExcel.Text = "Export";
            this.btnExportToExcel.Visible = false;
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Location = new System.Drawing.Point(589, 1);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(71, 23);
            this.btnGenerateReport.TabIndex = 38;
            this.btnGenerateReport.Text = "Generate";
            this.btnGenerateReport.Visible = false;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // dtStartDate
            // 
            this.dtStartDate.DateTime = new System.DateTime(2007, 5, 30, 0, 0, 0, 0);
            this.dtStartDate.Location = new System.Drawing.Point(78, 3);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(86, 21);
            this.dtStartDate.TabIndex = 37;
            this.dtStartDate.Value = new System.DateTime(2007, 5, 30, 0, 0, 0, 0);
            this.dtStartDate.Visible = false;
            // 
            // grdHistoryPositions
            // 
            this.grdHistoryPositions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.Black;
            this.grdHistoryPositions.DisplayLayout.Appearance = appearance2;
            this.grdHistoryPositions.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand1.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.Bottom;
            appearance3.TextVAlign = Infragistics.Win.VAlign.Bottom;
            ultraGridBand1.Override.SummaryFooterAppearance = appearance3;
            appearance4.TextVAlign = Infragistics.Win.VAlign.Bottom;
            ultraGridBand1.Override.SummaryFooterCaptionAppearance = appearance4;
            ultraGridBand1.Override.SummaryFooterSpacingAfter = 0;
            appearance5.BackColor = System.Drawing.Color.Black;
            appearance5.BackColor2 = System.Drawing.Color.Black;
            appearance5.TextVAlign = Infragistics.Win.VAlign.Top;
            ultraGridBand1.Override.SummaryValueAppearance = appearance5;
            ultraGridBand1.SummaryFooterCaption = "";
            this.grdHistoryPositions.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdHistoryPositions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistoryPositions.DisplayLayout.InterBandSpacing = 1;
            appearance6.BackColor = System.Drawing.Color.Gold;
            appearance6.BorderColor = System.Drawing.Color.Black;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.grdHistoryPositions.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdHistoryPositions.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdHistoryPositions.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdHistoryPositions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdHistoryPositions.DisplayLayout.Override.CellPadding = 0;
            this.grdHistoryPositions.DisplayLayout.Override.DefaultColWidth = 50;
            this.grdHistoryPositions.DisplayLayout.Override.GroupByRowInitialExpansionState = Infragistics.Win.UltraWinGrid.GroupByRowInitialExpansionState.Expanded;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdHistoryPositions.DisplayLayout.Override.HeaderAppearance = appearance7;
            this.grdHistoryPositions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdHistoryPositions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance8.BackColor = System.Drawing.Color.Black;
            appearance8.ForeColor = System.Drawing.Color.Orange;
            this.grdHistoryPositions.DisplayLayout.Override.RowAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdHistoryPositions.DisplayLayout.Override.RowSelectorAppearance = appearance9;
            this.grdHistoryPositions.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.grdHistoryPositions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistoryPositions.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdHistoryPositions.DisplayLayout.Override.SpecialRowSeparator = ((Infragistics.Win.UltraWinGrid.SpecialRowSeparator)((Infragistics.Win.UltraWinGrid.SpecialRowSeparator.TemplateAddRow | Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FixedRows)));
            appearance10.BackColor = System.Drawing.SystemColors.Info;
            this.grdHistoryPositions.DisplayLayout.Override.SpecialRowSeparatorAppearance = appearance10;
            this.grdHistoryPositions.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.grdHistoryPositions.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            scrollBarLook1.Appearance = appearance11;
            this.grdHistoryPositions.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdHistoryPositions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdHistoryPositions.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdHistoryPositions.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grdHistoryPositions.Location = new System.Drawing.Point(11, 31);
            this.grdHistoryPositions.Name = "grdHistoryPositions";
            this.grdHistoryPositions.Size = new System.Drawing.Size(797, 338);
            this.grdHistoryPositions.TabIndex = 36;
            this.grdHistoryPositions.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdHistoryPositions_InitializeRow);
            this.grdHistoryPositions.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdHistoryPositions_InitializeLayout);
            // 
            // lblStartDate
            // 
            this.lblStartDate.Location = new System.Drawing.Point(21, 6);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(60, 16);
            this.lblStartDate.TabIndex = 41;
            this.lblStartDate.Text = "Start Date";
            this.lblStartDate.Visible = false;
            // 
            // cmbReportType
            // 
            this.cmbReportType.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbReportType.DisplayLayout.Appearance = appearance12;
            this.cmbReportType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbReportType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbReportType.DisplayLayout.GroupByBox.Appearance = appearance13;
            appearance14.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbReportType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance14;
            this.cmbReportType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance15.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance15.BackColor2 = System.Drawing.SystemColors.Control;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbReportType.DisplayLayout.GroupByBox.PromptAppearance = appearance15;
            this.cmbReportType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbReportType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance16.BackColor = System.Drawing.SystemColors.Window;
            appearance16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbReportType.DisplayLayout.Override.ActiveCellAppearance = appearance16;
            appearance17.BackColor = System.Drawing.SystemColors.Highlight;
            appearance17.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbReportType.DisplayLayout.Override.ActiveRowAppearance = appearance17;
            this.cmbReportType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbReportType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance18.BackColor = System.Drawing.SystemColors.Window;
            this.cmbReportType.DisplayLayout.Override.CardAreaAppearance = appearance18;
            appearance19.BorderColor = System.Drawing.Color.Silver;
            appearance19.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbReportType.DisplayLayout.Override.CellAppearance = appearance19;
            this.cmbReportType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbReportType.DisplayLayout.Override.CellPadding = 0;
            appearance20.BackColor = System.Drawing.SystemColors.Control;
            appearance20.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance20.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance20.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbReportType.DisplayLayout.Override.GroupByRowAppearance = appearance20;
            appearance21.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbReportType.DisplayLayout.Override.HeaderAppearance = appearance21;
            this.cmbReportType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbReportType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance22.BackColor = System.Drawing.SystemColors.Window;
            appearance22.BorderColor = System.Drawing.Color.Silver;
            this.cmbReportType.DisplayLayout.Override.RowAppearance = appearance22;
            this.cmbReportType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance23.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbReportType.DisplayLayout.Override.TemplateAddRowAppearance = appearance23;
            this.cmbReportType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbReportType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbReportType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbReportType.DisplayMember = "";
            this.cmbReportType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbReportType.Location = new System.Drawing.Point(412, 2);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new System.Drawing.Size(100, 21);
            this.cmbReportType.TabIndex = 45;
            this.cmbReportType.ValueMember = "";
            this.cmbReportType.Visible = false;
            this.cmbReportType.ValueChanged += new System.EventHandler(this.cmbReportType_ValueChanged);
            // 
            // lblReportType
            // 
            this.lblReportType.Location = new System.Drawing.Point(343, 7);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(70, 16);
            this.lblReportType.TabIndex = 44;
            this.lblReportType.Text = "Report Type";
            this.lblReportType.Visible = false;
            // 
            // lblEndDate
            // 
            this.lblEndDate.Location = new System.Drawing.Point(172, 6);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(60, 16);
            this.lblEndDate.TabIndex = 43;
            this.lblEndDate.Text = "End Date";
            this.lblEndDate.Visible = false;
            // 
            // dtEndDate
            // 
            this.dtEndDate.DateTime = new System.DateTime(2007, 5, 30, 0, 0, 0, 0);
            this.dtEndDate.Location = new System.Drawing.Point(234, 3);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(86, 21);
            this.dtEndDate.TabIndex = 42;
            this.dtEndDate.Value = new System.DateTime(2007, 5, 30, 0, 0, 0, 0);
            this.dtEndDate.Visible = false;
            // 
            // btnClose
            // 
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnClose.Appearance = appearance24;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(342, 375);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(71, 23);
            this.btnClose.TabIndex = 46;
            this.btnClose.Text = "Close";
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ClosingHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 410);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExportToExcel);
            this.Controls.Add(this.btnGenerateReport);
            this.Controls.Add(this.dtStartDate);
            this.Controls.Add(this.grdHistoryPositions);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.cmbReportType);
            this.Controls.Add(this.lblReportType);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.dtEndDate);
            this.MinimumSize = new System.Drawing.Size(826, 444);
            this.Name = "ClosingHistory";
            this.Text = "Closing History";
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistoryPositions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReportType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEndDate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnExportToExcel;
        private Infragistics.Win.Misc.UltraButton btnGenerateReport;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtStartDate;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdHistoryPositions;
        private Infragistics.Win.Misc.UltraLabel lblStartDate;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbReportType;
        private Infragistics.Win.Misc.UltraLabel lblReportType;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private Infragistics.Win.Misc.UltraLabel lblEndDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtEndDate;
        private Infragistics.Win.Misc.UltraButton btnClose;
    }
}