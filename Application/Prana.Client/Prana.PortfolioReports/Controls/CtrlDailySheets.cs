using Microsoft.Reporting.WinForms;
using Prana.BusinessObjects;
using System;
using System.Windows.Forms;

namespace Prana.PortfolioReports.Controls
{
    public partial class CtrlDailySheets : UserControl
    {
        public CtrlDailySheets()
        {
            InitializeComponent();
        }

        private CompanyUser _loginUser;
        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
                //ctrlRealizedPL.SetUpControl();
                //cntrlDailyPositions.SetUpControl();
            }
        }

        public void SetupControl(CompanyUser loginUser)
        {
            _loginUser = loginUser;
            if (_loginUser == null)
            {
                throw new Exception("User could not be found while initializing Daily Reports");
            }
            dtMonth.Value = DateTime.Now;
            //dtMonth.Value = DateTime.Now.Date.AddMonths(-1);
            dtMonth.MaxDate = DateTime.Now;
            dtMonth.MaskInput = "{LOC}mm/yyyy";
        }

        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            dataSetMonthlySummary.EnforceConstraints = false;
            //pMGetMonthlySummaryValuesTableAdapter.Connection.ConnectionString = "Data Source=VS20052K3E;Initial Catalog=QADBPRODj;Persist Security Info=True;User ID=sa;Password=Prana2@@6";// .Connection.Database = "QADBPRODj";
            //string connectionString = ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString.ToString();

            //string month = "Aug";
            DateTime date = (DateTime)dtMonth.Value;
            string errMessage = " ";
            int? errNumber = 0;
            this.pMGetMonthlySummaryValuesTableAdapter.Fill(this.dataSetMonthlySummary.PMGetMonthlySummaryValues, _loginUser.CompanyID, _loginUser.CompanyUserID, date, ref errMessage, ref errNumber);
            this.reportViewerMonthlySummary.RefreshReport();

            LocalReport localReport = reportViewerMonthlySummary.LocalReport;
            DataSetMonthlySummary dataSetPranaClient = new DataSetMonthlySummary();
            ReportDataSource repDataSource = new ReportDataSource();
            repDataSource.Name = "DataSource";
            repDataSource.Value = dataSetPranaClient.PMGetMonthlySummaryValues;

            localReport.DataSources.Add(repDataSource);

            this.reportViewerMonthlySummary.RefreshReport();
        }

    }
}
