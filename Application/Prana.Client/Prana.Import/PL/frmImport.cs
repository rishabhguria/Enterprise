using Prana.BusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace Prana.Import.PL
{
    public partial class frmImport : Form, IPluggableTools, ILaunchForm
    {
        BackgroundWorker bgRunBatch;

        public frmImport()
        {
            AuditManager.BLL.AuditHandler.GetInstance().SetUIonPermission(false, int.MinValue);
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control when it is loaded
        /// </summary>
        /// <param name="thirdPartyID">ID of the third party</param>
        public void InitializeControl(int thirdPartyID)
        {
            try
            {
                cntrlBatchSetup1.InitializeControl(thirdPartyID);
                ctrlImportDashboard1.SetUp(_securityMaster);
                ImportCacheManager.CreateRunBatchDictionary(Application.StartupPath);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void frmImport_Load(object sender, EventArgs e)
        {
            try
            {
                SetButtonsColor();
                //modified by amit on 15.04.2015
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-6866
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_IMPORT_DASHBOARD);
                ImportManager.Instance.SetupLaunchForm(LaunchForm);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
                PositionMaster.AccountsList = CachedDataManager.GetInstance.GetUserAccounts();
                PositionMaster.TotalAccounts = CachedDataManager.GetInstance.GetAllAccountsCount();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnRunBatch.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnRunBatch.ForeColor = System.Drawing.Color.White;
                btnRunBatch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRunBatch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRunBatch.UseAppStyling = false;
                btnRunBatch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnRunBatch_Click(object sender, EventArgs e)
        {
            try
            {
                bgRunBatch = new BackgroundWorker();
                bgRunBatch.DoWork += bgRunBatch_DoWork;
                bgRunBatch.RunWorkerCompleted += bgRunBatch_RunWorkerCompleted;
                bgRunBatch.WorkerSupportsCancellation = true;

                object[] args = new object[1];
                List<string> formatNames = cntrlBatchSetup1.getFormatName();
                args[0] = formatNames;
                bgRunBatch.RunWorkerAsync(args);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region background worker methods

        void bgRunBatch_DoWork(object sender, DoWorkEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            try
            {
                //ImportCacheManager.CreateRunBatchDictionary(Application.StartupPath);
                //check that background worker is not busy
                object[] args = e.Argument as object[];
                List<string> formatNames = args[0] as List<string>;

                foreach (string formatName in formatNames)
                {
                    ImportManager.SetSchedulerRunningValue(false);
                    // batch are to be checked if started by client/scheduler
                    string msg = ImportExecutionHelper.ExecuteImportBatch(formatName).ToString();
                    if (!string.IsNullOrEmpty(msg))
                    {
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1502
                        //Show Proper Message to user if there are no file to upload.
                        message.Append(msg).Append(Environment.NewLine);
                    }
                }


            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            e.Result = message;
        }

        void bgRunBatch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                StringBuilder message = e.Result as StringBuilder;
                if (!string.IsNullOrWhiteSpace(message.ToString()))
                {
                    MessageBox.Show(message.ToString(), "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        #endregion

        #region IPluggableTools Members

        public void SetUP()
        {
            //For Third party id -1 all the scheduled batches will be fetched
            try
            {
                InitializeControl(-1);
                ImportManager.Instance.Initialize(_securityMaster);
                ClientPricingManager.GetInstance.SecurityMasterServices = _securityMaster;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        ISecurityMasterServices _securityMaster = null;

        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
                SecurityMasterManager.Instance.SecurityMaster = _securityMaster;
            }
        }

        public IPostTradeServices PostTradeServices
        {
            set {; }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        #endregion

        #region ILaunchForm Members

        public event EventHandler LaunchForm;

        #endregion



        private void ImportData_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {
                //stop form closing if user cancels the operation
                if (!ctrlImportDashboard1.StopDashBoradDataRefreshing())
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ImportData_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, EventArgs.Empty);
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
