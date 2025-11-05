using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Collections.Generic;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    public partial class SaveLayoutPM : PortfolioManagementUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {


                OpenConsolidationView();

                Main.Click(MouseButtons.Left);
                MouseController.RightClick();
                if (!PopupMenuDropDown.IsVisible)
                {
                    Main.Click(MouseButtons.Right);
                }

                Wait(1000);
                DataUtilities.pickFromMenuItem(PopupMenuDropDown, "Save Layout");
                Wait(1000);
                if (!string.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TYPE].ToString()))
                {
                    if (testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TYPE].ToString().ToUpper().Equals("ALL"))
                    {
                        All.Click(MouseButtons.Left);
                    }
                    else if (testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TYPE].ToString().ToUpper().Equals("CURRENT"))
                    {
                        Current.Click(MouseButtons.Left);
                    }
                    else if (testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TYPE].ToString().ToUpper().Equals("AS DEFAULT"))
                    {
                        AsDefault.Click(MouseButtons.Left);
                    }
                }
                else
                {
                    AsDefault.Click(MouseButtons.Left);
                }

            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally {
                KeyboardUtilities.CloseWindow(ref PM_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }
    }
}
