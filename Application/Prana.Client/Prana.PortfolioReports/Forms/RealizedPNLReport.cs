using Prana.BusinessObjects;
using Prana.Interfaces;
using System;
using System.Windows.Forms;


namespace Prana.PortfolioReports.Forms
{
    public partial class RealizedPNLReport : Form, IPositionManagementReports
    {
        public RealizedPNLReport()
        {
            InitializeComponent();
            ctrlRealizedPNLReport.SetupControl();
        }

        public event EventHandler FormClosedHandler;
        private CompanyUser _loginUser;

        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
                ctrlRealizedPNLReport.LoginUser = _loginUser;
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

        private void RealizedPNLReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (FormClosedHandler != null)
            {
                FormClosedHandler(this, EventArgs.Empty);
            }
        }

        private void ctrlRealizedPNLReport_Load(object sender, EventArgs e)
        {
            ctrlRealizedPNLReport.GenerateReports();
        }


    }
}