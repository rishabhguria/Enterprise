using Prana.BusinessObjects;
//using Prana.PM.BLL;

using Prana.BusinessObjects.PositionManagement;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.PortfolioReports.Forms
{
    public partial class RealizedPL : Form, Prana.Interfaces.IPositionManagementReports
    {
        public event EventHandler FormClosedHandler;
        private CompanyUser _loginUser;
        ClosingHistory frmClosingHistory = null;
        public RealizedPL()
        {
            InitializeComponent();
        }

        public Prana.BusinessObjects.CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
                ctrlRealizedPL.SetUpControl();
                cntrlDailyPositions.SetUpControl();
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


        /// <summary>
        /// Handles the Disposed event of the frmAddSymbol form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void frmClosingHistory_Disposed(object sender, EventArgs e)
        {
            frmClosingHistory = null;
        }

        private void btnClosingHistory_Click(object sender, EventArgs e)
        {
            if (frmClosingHistory == null)
            {
                frmClosingHistory = new ClosingHistory();
                frmClosingHistory.Owner = this;
                frmClosingHistory.ShowInTaskbar = false;
            }

            NetPositionList positionList = new NetPositionList();
            if (tabCtrlPMReports.Tabs["tbpRealizedPL"].Selected == true)
            {
                positionList = ctrlRealizedPL.RealizedPLPositions;
                if (positionList.Count > 0)
                {
                    frmClosingHistory.SetUpForm(positionList);

                    frmClosingHistory.Show();
                    frmClosingHistory.Activate();
                    frmClosingHistory.Disposed += new EventHandler(frmClosingHistory_Disposed);
                }
                else
                {
                    InformationMessageBox.Display("Please select some positions to view history");
                }
            }
            else
            {
                positionList = cntrlDailyPositions.DailyPositions;
                if (positionList.Count > 0)
                {
                    frmClosingHistory.SetUpForm(positionList);

                    frmClosingHistory.Show();
                    frmClosingHistory.Activate();
                    frmClosingHistory.Disposed += new EventHandler(frmClosingHistory_Disposed);
                }
                else
                {
                    InformationMessageBox.Display("Please select some positions to view history");
                }
            }

        }

        private void RealizedPL_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (FormClosedHandler != null)
            {
                FormClosedHandler(this, EventArgs.Empty);
            }
        }
    }
}