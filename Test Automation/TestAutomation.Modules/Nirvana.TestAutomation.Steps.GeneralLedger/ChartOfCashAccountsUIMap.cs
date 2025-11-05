using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    [UITestFixture]
    public partial class ChartOfCashAccountsUIMap : UIMap
    {
        public ChartOfCashAccountsUIMap()
        {
            InitializeComponent();
        }

        public ChartOfCashAccountsUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Open account details tab under Chart of Cash Accounts under the cash journal
        /// </summary>
        protected void OpenAccountDetails()
        {
            try
            {
                //Shortcut to open general ledger module(CTRL + SHIFT + G)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_GL"]);
                ExtentionMethods.WaitForVisible(ref FrmCashManagementMain, 15);
                //GeneralLedger1.Click(MouseButtons.Left);
                ChartofCashAccounts.Click(MouseButtons.Left);
                AccountDetails.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Minimize General Ledger 
        /// </summary>
        public void MinimizeGeneralLedger()
        {
            try
            {
                KeyboardUtilities.MinimizeWindow(ref FrmCashManagementMain_UltraFormManager_Dock_Area_Top);
                //Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

        }

        /// <summary>
        /// To open Account Balances Tab in Chart Of Cash Accounts in General Ledger
        /// </summary>
        public void OpenAccountBalancesForRevaluation()
        {
            try
            {
                GeneralLedger1.Click(MouseButtons.Left);
                ChartofCashAccounts.Click(MouseButtons.Left);
                AccountBalances.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

    }
}
