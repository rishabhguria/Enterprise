namespace Prana.PortfolioReports.Controls
{
    partial class CtrlMTDDailyPNL
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.pMGetMTDDailyPNLBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetMTDDailyPNL = new Prana.PortfolioReports.DataSetMTDDailyPNL();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.dtMonth = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.reportViewerMTDDailyPNL = new Microsoft.Reporting.WinForms.ReportViewer();
            this.btnGenerateReports = new System.Windows.Forms.Button();
            this.pMGetMTDDailyPNLTableAdapter = new Prana.PortfolioReports.DataSetMTDDailyPNLTableAdapters.PMGetMTDDailyPNLTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.pMGetMTDDailyPNLBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetMTDDailyPNL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // pMGetMTDDailyPNLBindingSource
            // 
            this.pMGetMTDDailyPNLBindingSource.DataMember = "PMGetMTDDailyPNL";
            this.pMGetMTDDailyPNLBindingSource.DataSource = this.dataSetMTDDailyPNL;
            // 
            // dataSetMTDDailyPNL
            // 
            this.dataSetMTDDailyPNL.DataSetName = "DataSetMTDDailyPNL";
            this.dataSetMTDDailyPNL.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ultraLabel1.Location = new System.Drawing.Point(219, 2);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(78, 16);
            this.ultraLabel1.TabIndex = 61;
            this.ultraLabel1.Text = "Select Month";
            // 
            // dtMonth
            // 
            this.dtMonth.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtMonth.Location = new System.Drawing.Point(303, 0);
            this.dtMonth.Name = "dtMonth";
            this.dtMonth.Size = new System.Drawing.Size(86, 21);
            this.dtMonth.TabIndex = 60;
            this.dtMonth.Value = null;
            // 
            // reportViewerMTDDailyPNL
            // 
            this.reportViewerMTDDailyPNL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource1.Name = "PranaClientDataSet_P_GetHolidays";
            reportDataSource1.Value = this.pMGetMTDDailyPNLBindingSource;
            reportDataSource2.Name = "PranaClientDataSet_PMGetMonthlySummaryValues";
            reportDataSource2.Value = this.pMGetMTDDailyPNLBindingSource;
            reportDataSource3.Name = "DataSetMonthlySummary_PMGetMonthlySummaryValues";
            reportDataSource3.Value = this.pMGetMTDDailyPNLBindingSource;
            reportDataSource4.Name = "DataSetMTDDailyPNL_PMGetMTDDailyPNL";
            reportDataSource4.Value = this.pMGetMTDDailyPNLBindingSource;
            this.reportViewerMTDDailyPNL.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewerMTDDailyPNL.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewerMTDDailyPNL.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewerMTDDailyPNL.LocalReport.DataSources.Add(reportDataSource4);
            this.reportViewerMTDDailyPNL.LocalReport.DisplayName = "Daily Sheet";
            this.reportViewerMTDDailyPNL.LocalReport.EnableExternalImages = true;
            this.reportViewerMTDDailyPNL.LocalReport.EnableHyperlinks = true;
            this.reportViewerMTDDailyPNL.LocalReport.ReportEmbeddedResource = "Prana.PortfolioReports.DailySheet.rdlc";
            this.reportViewerMTDDailyPNL.LocalReport.ReportPath = "Reports\\DailySheet.rdlc";
            this.reportViewerMTDDailyPNL.Location = new System.Drawing.Point(3, 27);
            this.reportViewerMTDDailyPNL.Name = "reportViewerMTDDailyPNL";
            this.reportViewerMTDDailyPNL.ServerReport.ReportServerUrl = new System.Uri("http://VS20052K3E/reportserver", System.UriKind.Absolute);
            this.reportViewerMTDDailyPNL.Size = new System.Drawing.Size(614, 558);
            this.reportViewerMTDDailyPNL.TabIndex = 62;
            // 
            // btnGenerateReports
            // 
            this.btnGenerateReports.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnGenerateReports.Location = new System.Drawing.Point(242, 591);
            this.btnGenerateReports.Name = "btnGenerateReports";
            this.btnGenerateReports.Size = new System.Drawing.Size(103, 23);
            this.btnGenerateReports.TabIndex = 63;
            this.btnGenerateReports.Text = "Generate Reports";
            this.btnGenerateReports.UseVisualStyleBackColor = true;
            this.btnGenerateReports.Click += new System.EventHandler(this.btnGenerateReports_Click);
            // 
            // pMGetMTDDailyPNLTableAdapter
            // 
            this.pMGetMTDDailyPNLTableAdapter.ClearBeforeFill = true;
            // 
            // CtrlMTDDailyPNL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnGenerateReports);
            this.Controls.Add(this.reportViewerMTDDailyPNL);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.dtMonth);
            this.Name = "CtrlMTDDailyPNL";
            this.Size = new System.Drawing.Size(621, 618);
            ((System.ComponentModel.ISupportInitialize)(this.pMGetMTDDailyPNLBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetMTDDailyPNL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtMonth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtMonth;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewerMTDDailyPNL;
        private System.Windows.Forms.Button btnGenerateReports;
        private DataSetMTDDailyPNL dataSetMTDDailyPNL;
        private System.Windows.Forms.BindingSource pMGetMTDDailyPNLBindingSource;
        private Prana.PortfolioReports.DataSetMTDDailyPNLTableAdapters.PMGetMTDDailyPNLTableAdapter pMGetMTDDailyPNLTableAdapter;
    }
}
