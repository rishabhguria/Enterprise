using Microsoft.Reporting.WinForms;
using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PortfolioReports.Constants;
using System;
using System.Configuration;
using System.Net;
using System.Web;
using System.Windows.Forms;

namespace Prana.PortfolioReports.ServerReports
{
    public partial class ServerReportForm : Form, IPositionManagementReports
    {
        public ServerReportForm()
        {
            InitializeComponent();
        }

        #region IPositionManagementReports Members

        public event EventHandler FormClosedHandler;
        public Form Reference()
        {
            return this;
        }

        private CompanyUser _loginUser;
        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
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
        #endregion

        /// <summary>
        /// Network Credentials
        /// </summary>
        public ICredentials NetworkCredentials
        {
            get
            {
                try
                {
                    string UserName = ConfigurationManager.AppSettings[ReportConstants.REPORTS_USER_NAME];
                    if (!String.IsNullOrWhiteSpace(UserName))
                    {
                        string password = ConfigurationManager.AppSettings[ReportConstants.REPORTS_USER_PASSWORD];
                        return new NetworkCredential(UserName, password);
                    }
                    else
                    {
                        return new NetworkCredential(ReportConstants.REPORTS_DEFAULT_USER_NAME, ReportConstants.REPORTS_DEFAULT_USER_PASSWORD);
                    }
                }
                catch (Exception)
                {
                    return new NetworkCredential(ReportConstants.REPORTS_DEFAULT_USER_NAME, ReportConstants.REPORTS_DEFAULT_USER_PASSWORD);
                }
            }
        }

        /// <summary>
        /// Form Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerReportForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = _serverReportName;
                ServerReport serverReport = reportViewer.ServerReport;

                if (_serverReportName == "Allocation Ticket Report")
                {
                    string reportServerUrl = ConfigurationManager.AppSettings["ReportServerUrl"];

                    string reportPath = ConfigurationManager.AppSettings["ReportPath"];

                    serverReport.ReportServerUrl = new Uri(reportServerUrl);

                    serverReport.ReportPath = reportPath;
                }
                else
                {

                    Uri url = new Uri(HttpUtility.UrlDecode(_serverReportURL));
                    serverReport.ReportServerUrl = new Uri(url.Scheme + "://" + url.Host + "/" + url.Segments[1]);

                    string query = HttpUtility.UrlDecode(url.Query);
                    int stringTo = query.LastIndexOf("&");
                    serverReport.ReportPath = query.Substring(1, (stringTo - 1));
                }
                // Get a reference to the default credentials
                ICredentials credentials = NetworkCredentials;

                // Get a reference to the report server credentials
                ReportServerCredentials rsCredentials = serverReport.ReportServerCredentials;

                // Set the credentials for the server report
                rsCredentials.NetworkCredentials = credentials;

                this.reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Form Closed Event Handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValuationSummaryServerReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (FormClosedHandler != null)
                    FormClosedHandler(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}