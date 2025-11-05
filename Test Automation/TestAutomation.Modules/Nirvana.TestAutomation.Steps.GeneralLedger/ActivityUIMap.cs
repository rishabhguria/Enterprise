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
    public partial class ActivityUIMap : UIMap
    {
        public ActivityUIMap()
        {
            InitializeComponent();
        }

        public ActivityUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        
        /// <summary>
        /// Opening Activity tab 
        /// </summary>
        public void OpenActivityTab()
        {
            try
            {
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open general ledger module(CTRL + SHIFT + G)

                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_GL"]);
                ExtentionMethods.WaitForVisible(ref FrmCashManagementMain, 15);
                //Wait(5000);

                //GeneralLedger1.Click(MouseButtons.Left);
                Activity.Click(MouseButtons.Left);
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
               // Wait(100);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// get key columns for comparing rows in cash journal
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeyColumnsForActivity()
        {
            List<string> columnList = new List<string>();
            try
            {
                columnList.Add(TestDataConstants.COL_ACCOUNT);
                columnList.Add(TestDataConstants.COL_SYMBOL);
                columnList.Add(TestDataConstants.COL_CURRENCY);
                columnList.Add(TestDataConstants.COL_ACTIVITY_TYPE);
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
