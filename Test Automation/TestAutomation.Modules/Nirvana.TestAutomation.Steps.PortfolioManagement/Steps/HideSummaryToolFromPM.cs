using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;


namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    class HideSummaryToolFromPM : PortfolioManagementUIMap, ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData">test data.</param>
        /// <param name="sheetIndexToName">sheet name.</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenConsolidationView();
                Wait(5000);
                Main.Click(MouseButtons.Left);
                Main.InvokeMethod("RemoveGrouping", null);
                Main.Click(MouseButtons.Left);
                Wait(1000);
                Main.Click(MouseButtons.Right);
                if (!PopupMenuDropDown.IsVisible)
                {
                    Main.Click(MouseButtons.Right);
                }
                HideSummaryTool.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                KeyboardUtilities.MinimizeWindow(ref PM_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }
    }
}
