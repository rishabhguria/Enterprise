namespace Prana.PortfolioReports
{
    partial class CtrlTransactionSummaryReport
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
                if (_dataSetTransactionSummaryUpdated != null)
                {
                    _dataSetTransactionSummaryUpdated.Dispose();
                }
                if (_dtTransactionStatement != null)
                {
                    _dtTransactionStatement.Dispose();
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
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
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
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
            this.pMGetTransactionSummaryReportDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetTransactionSummary = new Prana.PortfolioReports.DataSetTransactionSummary();
            this.pMGetCompanyLogoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetCompanyLogo = new Prana.PortfolioReports.DataSetCompanyLogo();
            this.dtStartDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblStartDate = new Infragistics.Win.Misc.UltraLabel();
            this.btnGenerateReports = new System.Windows.Forms.Button();
            this.reportViewerTransactionSummary = new Microsoft.Reporting.WinForms.ReportViewer();
            this.dtEndDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblEndDate = new Infragistics.Win.Misc.UltraLabel();
            this.pMGetTransactionSummaryReportDataTableAdapter = new Prana.PortfolioReports.DataSetTransactionSummaryTableAdapters.PMGetTransactionSummaryReportDataTableAdapter();
            this.lblSelectAccounts = new System.Windows.Forms.Label();
            this.checkedListAccounts = new System.Windows.Forms.CheckedListBox();
            this.lblSelectDataSource = new System.Windows.Forms.Label();
            this.checkedListDataSource = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmb2ndGroup = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmb3rdGroup = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmb4thGroup = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.btnAggregagte = new System.Windows.Forms.Button();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.pMGetCompanyLogoTableAdapter = new Prana.PortfolioReports.DataSetCompanyLogoTableAdapters.PMGetCompanyLogoTableAdapter();
            this.cmb1stGroup = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pMGetTransactionSummaryReportDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTransactionSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMGetCompanyLogoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCompanyLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb2ndGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb3rdGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb4thGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb1stGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // pMGetTransactionSummaryReportDataBindingSource
            // 
            this.pMGetTransactionSummaryReportDataBindingSource.DataMember = "PMGetTransactionSummaryReportData";
            this.pMGetTransactionSummaryReportDataBindingSource.DataSource = this.dataSetTransactionSummary;
            // 
            // dataSetTransactionSummary
            // 
            this.dataSetTransactionSummary.DataSetName = "DataSetTransactionSummary";
            this.dataSetTransactionSummary.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pMGetCompanyLogoBindingSource
            // 
            this.pMGetCompanyLogoBindingSource.DataMember = "PMGetCompanyLogo";
            this.pMGetCompanyLogoBindingSource.DataSource = this.dataSetCompanyLogo;
            // 
            // dataSetCompanyLogo
            // 
            this.dataSetCompanyLogo.DataSetName = "DataSetCompanyLogo";
            this.dataSetCompanyLogo.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dtStartDate
            // 
            this.dtStartDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtStartDate.Location = new System.Drawing.Point(409, 2);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(86, 21);
            this.dtStartDate.TabIndex = 66;
            this.dtStartDate.Value = null;
            // 
            // lblStartDate
            // 
            this.lblStartDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblStartDate.Location = new System.Drawing.Point(508, 6);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(51, 17);
            this.lblStartDate.TabIndex = 67;
            this.lblStartDate.Text = "End Date";
            // 
            // btnGenerateReports
            // 
            this.btnGenerateReports.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnGenerateReports.Location = new System.Drawing.Point(667, 3);
            this.btnGenerateReports.Name = "btnGenerateReports";
            this.btnGenerateReports.Size = new System.Drawing.Size(103, 23);
            this.btnGenerateReports.TabIndex = 65;
            this.btnGenerateReports.Text = "Generate Reports";
            this.btnGenerateReports.UseVisualStyleBackColor = true;
            this.btnGenerateReports.Click += new System.EventHandler(this.btnGenerateReports_Click);
            // 
            // reportViewerTransactionSummary
            // 
            this.reportViewerTransactionSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource1.Name = "DataSetTransactionSummary_PMGetTransactionSummaryReportData";
            reportDataSource1.Value = this.pMGetTransactionSummaryReportDataBindingSource;
            reportDataSource2.Name = "DataSetCompanyLogo_PMGetCompanyLogo";
            reportDataSource2.Value = this.pMGetCompanyLogoBindingSource;
            this.reportViewerTransactionSummary.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewerTransactionSummary.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewerTransactionSummary.LocalReport.DisplayName = "Transaction Summary";
            this.reportViewerTransactionSummary.LocalReport.EnableExternalImages = true;
            this.reportViewerTransactionSummary.LocalReport.EnableHyperlinks = true;
            this.reportViewerTransactionSummary.LocalReport.ReportEmbeddedResource = "Prana.PortfolioReports.TransactionSummary.rdlc";
            this.reportViewerTransactionSummary.LocalReport.ReportPath = "Reports\\TransactionSummary.rdlc";
            this.reportViewerTransactionSummary.Location = new System.Drawing.Point(3, 57);
            this.reportViewerTransactionSummary.Name = "reportViewerTransactionSummary";
            this.reportViewerTransactionSummary.ServerReport.ReportServerUrl = new System.Uri("http://VS20052K3E/reportserver", System.UriKind.Absolute);
            this.reportViewerTransactionSummary.Size = new System.Drawing.Size(1112, 514);
            this.reportViewerTransactionSummary.TabIndex = 64;
            this.reportViewerTransactionSummary.ReportRefresh += new System.ComponentModel.CancelEventHandler(this.reportViewerTransactionSummary_ReportRefresh);
            this.reportViewerTransactionSummary.ReportError += new Microsoft.Reporting.WinForms.ReportErrorEventHandler(this.reportViewerTransactionSummary_ReportError);
            this.reportViewerTransactionSummary.ReportExport += new Microsoft.Reporting.WinForms.ExportEventHandler(this.reportViewerTransactionSummary_ReportExport);
            // 
            // dtEndDate
            // 
            this.dtEndDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtEndDate.Location = new System.Drawing.Point(565, 2);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(86, 21);
            this.dtEndDate.TabIndex = 68;
            this.dtEndDate.Value = null;
            // 
            // lblEndDate
            // 
            this.lblEndDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblEndDate.Location = new System.Drawing.Point(860, 33);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(64, 16);
            this.lblEndDate.TabIndex = 69;
            this.lblEndDate.Text = "End Date";
            this.lblEndDate.Visible = false;
            // 
            // pMGetTransactionSummaryReportDataTableAdapter
            // 
            this.pMGetTransactionSummaryReportDataTableAdapter.ClearBeforeFill = true;
            // 
            // lblSelectAccounts
            // 
            this.lblSelectAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSelectAccounts.AutoSize = true;
            this.lblSelectAccounts.Location = new System.Drawing.Point(855, 67);
            this.lblSelectAccounts.Name = "lblSelectAccounts";
            this.lblSelectAccounts.Size = new System.Drawing.Size(69, 13);
            this.lblSelectAccounts.TabIndex = 71;
            this.lblSelectAccounts.Text = "Select Accounts";
            this.lblSelectAccounts.Visible = false;
            // 
            // checkedListAccounts
            // 
            this.checkedListAccounts.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.checkedListAccounts.CheckOnClick = true;
            this.checkedListAccounts.FormattingEnabled = true;
            this.checkedListAccounts.Location = new System.Drawing.Point(804, 61);
            this.checkedListAccounts.Name = "checkedListAccounts";
            this.checkedListAccounts.Size = new System.Drawing.Size(120, 19);
            this.checkedListAccounts.TabIndex = 70;
            this.checkedListAccounts.Visible = false;
            // 
            // lblSelectDataSource
            // 
            this.lblSelectDataSource.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSelectDataSource.AutoSize = true;
            this.lblSelectDataSource.Location = new System.Drawing.Point(801, 105);
            this.lblSelectDataSource.Name = "lblSelectDataSource";
            this.lblSelectDataSource.Size = new System.Drawing.Size(100, 13);
            this.lblSelectDataSource.TabIndex = 73;
            this.lblSelectDataSource.Text = "Select Data Source";
            this.lblSelectDataSource.Visible = false;
            // 
            // checkedListDataSource
            // 
            this.checkedListDataSource.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.checkedListDataSource.CheckOnClick = true;
            this.checkedListDataSource.FormattingEnabled = true;
            this.checkedListDataSource.Location = new System.Drawing.Point(804, 83);
            this.checkedListDataSource.Name = "checkedListDataSource";
            this.checkedListDataSource.Size = new System.Drawing.Size(120, 19);
            this.checkedListDataSource.TabIndex = 72;
            this.checkedListDataSource.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(186, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 77;
            this.label1.Text = "Group By";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(566, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 78;
            this.label2.Text = "Then By";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(757, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 79;
            this.label3.Text = "Then By";
            // 
            // cmb2ndGroup
            // 
            this.cmb2ndGroup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmb2ndGroup.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmb2ndGroup.DisplayLayout.Appearance = appearance1;
            this.cmb2ndGroup.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmb2ndGroup.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmb2ndGroup.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmb2ndGroup.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmb2ndGroup.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmb2ndGroup.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmb2ndGroup.DisplayLayout.MaxColScrollRegions = 1;
            this.cmb2ndGroup.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmb2ndGroup.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmb2ndGroup.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmb2ndGroup.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmb2ndGroup.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmb2ndGroup.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmb2ndGroup.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmb2ndGroup.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmb2ndGroup.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmb2ndGroup.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmb2ndGroup.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmb2ndGroup.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmb2ndGroup.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmb2ndGroup.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmb2ndGroup.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmb2ndGroup.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmb2ndGroup.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmb2ndGroup.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmb2ndGroup.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmb2ndGroup.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmb2ndGroup.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmb2ndGroup.Location = new System.Drawing.Point(433, 29);
            this.cmb2ndGroup.Name = "cmb2ndGroup";
            this.cmb2ndGroup.Size = new System.Drawing.Size(121, 22);
            this.cmb2ndGroup.TabIndex = 80;
            this.cmb2ndGroup.ValueChanged += new System.EventHandler(this.cmb2ndGroup_ValueChanged);
            // 
            // cmb3rdGroup
            // 
            this.cmb3rdGroup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmb3rdGroup.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance37.BackColor = System.Drawing.SystemColors.Window;
            appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmb3rdGroup.DisplayLayout.Appearance = appearance37;
            this.cmb3rdGroup.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmb3rdGroup.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance38.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance38.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.BorderColor = System.Drawing.SystemColors.Window;
            this.cmb3rdGroup.DisplayLayout.GroupByBox.Appearance = appearance38;
            appearance39.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmb3rdGroup.DisplayLayout.GroupByBox.BandLabelAppearance = appearance39;
            this.cmb3rdGroup.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance40.BackColor2 = System.Drawing.SystemColors.Control;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance40.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmb3rdGroup.DisplayLayout.GroupByBox.PromptAppearance = appearance40;
            this.cmb3rdGroup.DisplayLayout.MaxColScrollRegions = 1;
            this.cmb3rdGroup.DisplayLayout.MaxRowScrollRegions = 1;
            appearance41.BackColor = System.Drawing.SystemColors.Window;
            appearance41.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmb3rdGroup.DisplayLayout.Override.ActiveCellAppearance = appearance41;
            appearance42.BackColor = System.Drawing.SystemColors.Highlight;
            appearance42.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmb3rdGroup.DisplayLayout.Override.ActiveRowAppearance = appearance42;
            this.cmb3rdGroup.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmb3rdGroup.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance43.BackColor = System.Drawing.SystemColors.Window;
            this.cmb3rdGroup.DisplayLayout.Override.CardAreaAppearance = appearance43;
            appearance44.BorderColor = System.Drawing.Color.Silver;
            appearance44.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmb3rdGroup.DisplayLayout.Override.CellAppearance = appearance44;
            this.cmb3rdGroup.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmb3rdGroup.DisplayLayout.Override.CellPadding = 0;
            appearance45.BackColor = System.Drawing.SystemColors.Control;
            appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance45.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance45.BorderColor = System.Drawing.SystemColors.Window;
            this.cmb3rdGroup.DisplayLayout.Override.GroupByRowAppearance = appearance45;
            appearance46.TextHAlignAsString = "Left";
            this.cmb3rdGroup.DisplayLayout.Override.HeaderAppearance = appearance46;
            this.cmb3rdGroup.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmb3rdGroup.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            appearance47.BorderColor = System.Drawing.Color.Silver;
            this.cmb3rdGroup.DisplayLayout.Override.RowAppearance = appearance47;
            this.cmb3rdGroup.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance48.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmb3rdGroup.DisplayLayout.Override.TemplateAddRowAppearance = appearance48;
            this.cmb3rdGroup.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmb3rdGroup.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmb3rdGroup.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmb3rdGroup.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmb3rdGroup.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmb3rdGroup.Location = new System.Drawing.Point(621, 29);
            this.cmb3rdGroup.Name = "cmb3rdGroup";
            this.cmb3rdGroup.Size = new System.Drawing.Size(121, 22);
            this.cmb3rdGroup.TabIndex = 81;
            this.cmb3rdGroup.ValueChanged += new System.EventHandler(this.cmb3rdGroup_ValueChanged);
            // 
            // cmb4thGroup
            // 
            this.cmb4thGroup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmb4thGroup.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmb4thGroup.DisplayLayout.Appearance = appearance25;
            this.cmb4thGroup.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmb4thGroup.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.cmb4thGroup.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmb4thGroup.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.cmb4thGroup.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmb4thGroup.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.cmb4thGroup.DisplayLayout.MaxColScrollRegions = 1;
            this.cmb4thGroup.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmb4thGroup.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmb4thGroup.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.cmb4thGroup.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmb4thGroup.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.cmb4thGroup.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmb4thGroup.DisplayLayout.Override.CellAppearance = appearance32;
            this.cmb4thGroup.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmb4thGroup.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.cmb4thGroup.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlignAsString = "Left";
            this.cmb4thGroup.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.cmb4thGroup.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmb4thGroup.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.cmb4thGroup.DisplayLayout.Override.RowAppearance = appearance35;
            this.cmb4thGroup.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmb4thGroup.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.cmb4thGroup.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmb4thGroup.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmb4thGroup.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmb4thGroup.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmb4thGroup.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmb4thGroup.Location = new System.Drawing.Point(812, 29);
            this.cmb4thGroup.Name = "cmb4thGroup";
            this.cmb4thGroup.Size = new System.Drawing.Size(121, 22);
            this.cmb4thGroup.TabIndex = 82;
            this.cmb4thGroup.ValueChanged += new System.EventHandler(this.cmb4thGroup_ValueChanged);
            // 
            // btnAggregagte
            // 
            this.btnAggregagte.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnAggregagte.Location = new System.Drawing.Point(812, 3);
            this.btnAggregagte.Name = "btnAggregagte";
            this.btnAggregagte.Size = new System.Drawing.Size(80, 23);
            this.btnAggregagte.TabIndex = 90;
            this.btnAggregagte.Text = "Hide Details";
            this.btnAggregagte.UseVisualStyleBackColor = true;
            this.btnAggregagte.Click += new System.EventHandler(this.btnAggregagte_Click);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ultraLabel1.Location = new System.Drawing.Point(348, 6);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(55, 15);
            this.ultraLabel1.TabIndex = 67;
            this.ultraLabel1.Text = "Start Date";
            // 
            // pMGetCompanyLogoTableAdapter
            // 
            this.pMGetCompanyLogoTableAdapter.ClearBeforeFill = true;
            // 
            // cmb1stGroup
            // 
            this.cmb1stGroup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmb1stGroup.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmb1stGroup.DisplayLayout.Appearance = appearance13;
            this.cmb1stGroup.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmb1stGroup.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmb1stGroup.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmb1stGroup.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmb1stGroup.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmb1stGroup.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmb1stGroup.DisplayLayout.MaxColScrollRegions = 1;
            this.cmb1stGroup.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmb1stGroup.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmb1stGroup.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmb1stGroup.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmb1stGroup.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmb1stGroup.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmb1stGroup.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmb1stGroup.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmb1stGroup.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmb1stGroup.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.cmb1stGroup.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmb1stGroup.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmb1stGroup.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmb1stGroup.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmb1stGroup.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmb1stGroup.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmb1stGroup.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmb1stGroup.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmb1stGroup.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmb1stGroup.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmb1stGroup.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmb1stGroup.Location = new System.Drawing.Point(242, 29);
            this.cmb1stGroup.Name = "cmb1stGroup";
            this.cmb1stGroup.Size = new System.Drawing.Size(121, 22);
            this.cmb1stGroup.TabIndex = 91;
            this.cmb1stGroup.ValueChanged += new System.EventHandler(this.cmb1stGroup_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(369, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 92;
            this.label4.Text = "Then By";
            // 
            // CtrlTransactionSummaryReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmb1stGroup);
            this.Controls.Add(this.btnAggregagte);
            this.Controls.Add(this.cmb4thGroup);
            this.Controls.Add(this.cmb3rdGroup);
            this.Controls.Add(this.cmb2ndGroup);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSelectDataSource);
            this.Controls.Add(this.checkedListDataSource);
            this.Controls.Add(this.lblSelectAccounts);
            this.Controls.Add(this.checkedListAccounts);
            this.Controls.Add(this.dtEndDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.dtStartDate);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.btnGenerateReports);
            this.Controls.Add(this.reportViewerTransactionSummary);
            this.Name = "CtrlTransactionSummaryReport";
            this.Size = new System.Drawing.Size(1118, 607);
            ((System.ComponentModel.ISupportInitialize)(this.pMGetTransactionSummaryReportDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTransactionSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMGetCompanyLogoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCompanyLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb2ndGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb3rdGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb4thGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb1stGroup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtStartDate;
        private Infragistics.Win.Misc.UltraLabel lblStartDate;
        private System.Windows.Forms.Button btnGenerateReports;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewerTransactionSummary;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtEndDate;
        private Infragistics.Win.Misc.UltraLabel lblEndDate;
        private DataSetTransactionSummary dataSetTransactionSummary;
        private System.Windows.Forms.BindingSource pMGetTransactionSummaryReportDataBindingSource;
        private Prana.PortfolioReports.DataSetTransactionSummaryTableAdapters.PMGetTransactionSummaryReportDataTableAdapter pMGetTransactionSummaryReportDataTableAdapter;
        private System.Windows.Forms.Label lblSelectAccounts;
        private System.Windows.Forms.CheckedListBox checkedListAccounts;
        private System.Windows.Forms.Label lblSelectDataSource;
        private System.Windows.Forms.CheckedListBox checkedListDataSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmb2ndGroup;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmb3rdGroup;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmb4thGroup;
        private System.Windows.Forms.Button btnAggregagte;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.BindingSource pMGetCompanyLogoBindingSource;
        private DataSetCompanyLogo dataSetCompanyLogo;
        private Prana.PortfolioReports.DataSetCompanyLogoTableAdapters.PMGetCompanyLogoTableAdapter pMGetCompanyLogoTableAdapter;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmb1stGroup;
        private System.Windows.Forms.Label label4;
    }
}
