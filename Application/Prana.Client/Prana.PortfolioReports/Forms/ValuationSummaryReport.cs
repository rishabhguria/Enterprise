using Prana.BusinessObjects;
using Prana.Interfaces;
using System;
using System.Windows.Forms;

namespace Prana.PortfolioReports.Forms
{
    public partial class ValuationSummaryReport : Form, IPositionManagementReports
    {
        public ValuationSummaryReport()
        {
            InitializeComponent();
            ctrlValuationSummaryReport.SetupControl(_loginUser);
        }

        public event EventHandler FormClosedHandler;
        private CompanyUser _loginUser;

        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
                ctrlValuationSummaryReport.LoginUser = _loginUser;
            }
        }

        string _serverReportURL = string.Empty;
        public string ServerReportURL
        {
            set
            {
                _serverReportURL = value;
            }
        }

        string _serverReportName = string.Empty;
        public string ServerReportName
        {
            set
            {
                _serverReportName = value;
            }
        }

        public Form Reference()
        {

            return this;
        }

        //private CompanyUser _companyUser = new CompanyUser();
        //public ValuationSummaryReport(CompanyUser companyUser)
        //{
        //    InitializeComponent();
        //    _companyUser = companyUser;
        //}

        private void ValuationSummaryReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (FormClosedHandler != null)
            {
                FormClosedHandler(this, EventArgs.Empty);
            }
        }

        private void ctrlValuationSummaryReport_Load(object sender, EventArgs e)
        {
            //ctrlValuationSummaryReport.GenerateReports();
            //ctrlValuationSummaryReport.TempMethod();
        }
    }
}