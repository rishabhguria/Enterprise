using Prana.BusinessObjects;
using Prana.Interfaces;
using System;
using System.Windows.Forms;

namespace Prana.PortfolioReports.Forms
{
    public partial class TransactionSummaryReport : Form, IPositionManagementReports
    {
        public TransactionSummaryReport()
        {
            InitializeComponent();
            ctrlTransactionSummaryReport.SetupControl();
        }

        public event EventHandler FormClosedHandler;
        private CompanyUser _loginUser;

        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
                ctrlTransactionSummaryReport.LoginUser = _loginUser;
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

        private void TransactionSummaryReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (FormClosedHandler != null)
            {
                FormClosedHandler(this, EventArgs.Empty);
            }
        }

        private void ctrlTransactionSummaryReport_Load(object sender, EventArgs e)
        {
            //ctrlTransactionSummaryReport.GenerateReports();
        }

    }
}