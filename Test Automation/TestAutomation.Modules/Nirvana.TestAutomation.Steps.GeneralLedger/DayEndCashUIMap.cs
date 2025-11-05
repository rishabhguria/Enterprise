using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    [UITestFixture]
    public partial class DayEndCashUIMap : UIMap
    {
        public DayEndCashUIMap()
        {
            InitializeComponent();
        }

        public DayEndCashUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Opens the day end cash
        /// </summary>
        protected void OpenDayEndCash()
        {
            try
            {
                //Shortcut to open general ledger module(CTRL + SHIFT + G)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_GL"]);
                ExtentionMethods.WaitForVisible(ref FrmCashManagementMain, 15);
                //GeneralLedger1.Click(MouseButtons.Left);
                DayEndCash.Click(MouseButtons.Left);
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
        /// get key columns for comparing rows in Day End Cash
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeyColumnsForDayEndCash()
        {
            List<string> columnList = new List<string>();
            try
            {
                columnList.Add(TestDataConstants.COL_ACCOUNT);
                columnList.Add(TestDataConstants.COL_DATE);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return columnList;
        }
    }
}
