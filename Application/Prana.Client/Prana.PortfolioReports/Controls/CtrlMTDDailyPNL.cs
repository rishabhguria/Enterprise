using Microsoft.Reporting.WinForms;
using Prana.BusinessObjects;
using System;
using System.Windows.Forms;

namespace Prana.PortfolioReports.Controls
{
    public partial class CtrlMTDDailyPNL : UserControl
    {
        public void SetupControl(CompanyUser loginUser)
        {
            _loginuser = loginUser;
            dtMonth.Value = DateTime.Now;
            //dtMonth.Value = DateTime.Now.Date.AddMonths(-1);
            dtMonth.MaxDate = DateTime.Now;
            dtMonth.MaskInput = "{LOC}mm/yyyy";
        }

        private CompanyUser _loginuser;
        public CompanyUser loginuser
        {
            get { return _loginuser; }
            set
            {
                _loginuser = value;

            }
        }

        public CtrlMTDDailyPNL()
        {
            InitializeComponent();
        }

        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            //string month = "Aug";
            DateTime date = (DateTime)dtMonth.Value;
            string errMessage = " ";
            int? errNumber = 0;
            this.pMGetMTDDailyPNLTableAdapter.Fill(this.dataSetMTDDailyPNL.PMGetMTDDailyPNL, _loginuser.CompanyID, _loginuser.CompanyUserID, date, ref errMessage, ref errNumber);
            this.reportViewerMTDDailyPNL.RefreshReport();

            LocalReport localReport = reportViewerMTDDailyPNL.LocalReport;
            DataSetMTDDailyPNL dataSetPranaClient = new DataSetMTDDailyPNL();
            ReportDataSource repDataSource = new ReportDataSource();
            repDataSource.Name = "DataSource";
            repDataSource.Value = dataSetPranaClient.PMGetMTDDailyPNL;

            localReport.DataSources.Add(repDataSource);

            this.reportViewerMTDDailyPNL.RefreshReport();
        }
    }
}
