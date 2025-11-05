using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using TestAutomationFX.Core.UIAutomationSupport;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    class CheckPMLiveFeed : PortfolioManagementUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenConsolidationView();
                //Wait(9000);
                List<String> errors = CheckPMLiveFeeds(testData, sheetIndexToName);
                Main.Click(MouseButtons.Left);
                if (errors.Count > 0)
                {
                    _result.IsPassed = false;
                    _result.ErrorMessage = String.Join(", ", errors.ToArray());
                }

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
                PMclose();
            }
            return _result;
        }

        /// <summary>
        /// Checks the pm.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        private List<string> CheckPMLiveFeeds(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                Main.WaitForRespondingOrExited();
                Main.Click(MouseButtons.Left);
                AutomationElementWrapper wrapper = new AutomationElementWrapper(Main.MsaaObject.Children[1].WindowHandle);
                WaitOnItems(wrapper);
                Wait(4000);
                DataTable superset = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.Main.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                List<String> columns = new List<String>();
                List<String> errors = new List<String>();
                columns.Add("Symbol");
                columns.Add("Order Side");

                /*these columns can be added for more verification
                columns.Add("Px Ask");
                columns.Add("Px Mid");
                columns.Add("Px Bid");
                columns.Add("Px Last");
                columns.Add("Px Selected Feed (Local)");
                columns.Add("Px Selected Feed (Base)");
                columns.Add("Pricing Source");*/

                errors = Recon.RunRecon(superset, subset, columns, 0.0000000001);
                return errors;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void WaitOnItems(AutomationElementWrapper wrapper)
        {
            try
            {
                Stopwatch tmr = new Stopwatch();
                tmr.Start();

                while (wrapper.Children.Count <= 1)
                {
                    if (tmr.ElapsedMilliseconds >= 30000)
                    {
                        break;
                    }
                }
                tmr.Stop();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
