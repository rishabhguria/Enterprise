using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    [UITestFixture]
    public partial class JournalExceptionsUIMap : UIMap
    {
        public JournalExceptionsUIMap()
        {
            InitializeComponent();
        }

        public JournalExceptionsUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
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
        /// To open Journal Exceptions in General Ledger
        /// </summary>
        public void OpenJournalExceptions()
        {
            try
            {
                //Shortcut to open general ledger module(CTRL + SHIFT + G)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_GL"]);
                ExtentionMethods.WaitForVisible(ref FrmCashManagementMain, 15);
               // GeneralLedger.Click(MouseButtons.Left);
                JournalExceptions.Click(MouseButtons.Left);
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
