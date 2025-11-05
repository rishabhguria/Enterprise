using Prana.BusinessObjects;
using Prana.Interfaces;
using System;
using System.Windows.Forms;

namespace Prana.PortfolioReports.Forms
{
    public partial class MTDDailyPNL : Form, IPositionManagementReports
    {

        public event EventHandler FormClosedHandler;
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
            ctrlMTDDailyPNL1.SetupControl(_loginUser);
            return this;
        }

        public MTDDailyPNL()
        {
            InitializeComponent();
        }

        private void MonthlyReports_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (FormClosedHandler != null)
            {
                FormClosedHandler(this, EventArgs.Empty);
            }
        }
    }
}