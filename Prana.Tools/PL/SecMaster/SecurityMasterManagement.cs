using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class SecurityMasterManagement : Form, IPluggableTools
    {
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public SecurityMasterManagement()
        {
            try
            {
                InitializeComponent();
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    //TODO - We need to move symbol validation UI to here.
                    ultraTabControl1.Tabs["tabLookup"].Visible = false;
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

        #region IPluggableTools Members

        public void SetUP()
        {

        }

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;

                ctrlSMBatchSetup1.SecurityMaster = value;
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

        public ISecurityMasterServices _securityMaster { get; set; }

        private void SecurityMasterManagement_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }

                ultraTabControl1.Tabs["tabLookup"].Visible = false;
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

        private void SecurityMasterManagement_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {
                //stop form closing if user cancels the operation
                if (!ctrlSMBatchSetup1.SetUpWarningBeforeClose())
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

        void SecurityMasterManagement_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
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
