namespace Prana.PortfolioReports.Controls
{
    partial class CtrlDailySheets
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
            this.pMGetMonthlySummaryValuesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetMonthlySummary = new Prana.PortfolioReports.DataSetMonthlySummary();
            this.reportViewerMonthlySummary = new Microsoft.Reporting.WinForms.ReportViewer();
            this.btnGenerateReports = new System.Windows.Forms.Button();
            this.dtMonth = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.pMGetMonthlySummaryValuesTableAdapter = new Prana.PortfolioReports.DataSetMonthlySummaryTableAdapters.PMGetMonthlySummaryValuesTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.pMGetMonthlySummaryValuesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetMonthlySummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // pMGetMonthlySummaryValuesBindingSource
            // 
            this.pMGetMonthlySummaryValuesBindingSource.DataMember = "PMGetMonthlySummaryValues";
            this.pMGetMonthlySummaryValuesBindingSource.DataSource = this.dataSetMonthlySummary;
            // 
            // dataSetMonthlySummary
            // 
            this.dataSetMonthlySummary.DataSetName = "DataSetMonthlySummary";
            this.dataSetMonthlySummary.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewerMonthlySummary
            // 
            this.reportViewerMonthlySummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource1.Name = "DataSetMTDDailyPNL_PMGetMTDDailyPNL";
            reportDataSource1.Value = this.pMGetMonthlySummaryValuesBindingSource;
            reportDataSource2.Name = "DataSetMonthlySummary_PMGetMonthlySummaryValues";
            reportDataSource2.Value = this.pMGetMonthlySummaryValuesBindingSource;
            this.reportViewerMonthlySummary.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewerMonthlySummary.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewerMonthlySummary.LocalReport.DisplayName = "Monthly Summary";
            this.reportViewerMonthlySummary.LocalReport.EnableExternalImages = true;
            this.reportViewerMonthlySummary.LocalReport.EnableHyperlinks = true;
            this.reportViewerMonthlySummary.LocalReport.ReportEmbeddedResource = "Prana.PortfolioReports.MTDDailyPNL.rdlc";
            this.reportViewerMonthlySummary.LocalReport.ReportPath = "Reports\\MTDDailyPNL.rdlc";
            this.reportViewerMonthlySummary.Location = new System.Drawing.Point(3, 30);
            this.reportViewerMonthlySummary.Name = "reportViewerMonthlySummary";
            this.reportViewerMonthlySummary.ServerReport.ReportServerUrl = new System.Uri("http://VS20052K3E/reportserver", System.UriKind.Absolute);
            this.reportViewerMonthlySummary.Size = new System.Drawing.Size(588, 528);
            this.reportViewerMonthlySummary.TabIndex = 0;
            // 
            // btnGenerateReports
            // 
            this.btnGenerateReports.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnGenerateReports.Location = new System.Drawing.Point(244, 564);
            this.btnGenerateReports.Name = "btnGenerateReports";
            this.btnGenerateReports.Size = new System.Drawing.Size(103, 23);
            this.btnGenerateReports.TabIndex = 57;
            this.btnGenerateReports.Text = "Generate Reports";
            this.btnGenerateReports.UseVisualStyleBackColor = true;
            this.btnGenerateReports.Click += new System.EventHandler(this.btnGenerateReports_Click);
            // 
            // dtMonth
            // 
            this.dtMonth.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtMonth.Location = new System.Drawing.Point(202, 3);
            this.dtMonth.Name = "dtMonth";
            this.dtMonth.Size = new System.Drawing.Size(86, 21);
            this.dtMonth.TabIndex = 58;
            this.dtMonth.Value = null;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ultraLabel1.Location = new System.Drawing.Point(118, 8);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(78, 16);
            this.ultraLabel1.TabIndex = 59;
            this.ultraLabel1.Text = "Select Month";
            // 
            // pMGetMonthlySummaryValuesTableAdapter
            // 
            this.pMGetMonthlySummaryValuesTableAdapter.ClearBeforeFill = true;
            // 
            // CtrlDailySheets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.dtMonth);
            this.Controls.Add(this.btnGenerateReports);
            this.Controls.Add(this.reportViewerMonthlySummary);
            this.Name = "CtrlDailySheets";
            this.Size = new System.Drawing.Size(594, 592);
            ((System.ComponentModel.ISupportInitialize)(this.pMGetMonthlySummaryValuesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetMonthlySummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtMonth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewerMonthlySummary;
        private System.Windows.Forms.Button btnGenerateReports;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtMonth;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private DataSetMonthlySummary dataSetMonthlySummary;
        private System.Windows.Forms.BindingSource pMGetMonthlySummaryValuesBindingSource;
        private Prana.PortfolioReports.DataSetMonthlySummaryTableAdapters.PMGetMonthlySummaryValuesTableAdapter pMGetMonthlySummaryValuesTableAdapter;
        //private DataSetMonthlySummary dataSetMonthlySummary;
        //private System.Windows.Forms.BindingSource pMGetMonthlySummaryValuesBindingSource;
        //private Prana.PortfolioReports.DataSetMonthlySummaryTableAdapters.PMGetMonthlySummaryValuesTableAdapter pMGetMonthlySummaryValuesTableAdapter;

    }
}
