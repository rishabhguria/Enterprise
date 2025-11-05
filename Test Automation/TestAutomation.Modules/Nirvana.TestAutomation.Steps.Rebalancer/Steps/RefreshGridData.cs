using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.IO;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Steps.Rebalancer;
using System.Diagnostics;
namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    public partial class RefreshGridData : RebalancerUIMap,ITestStep
    {
        /// <summary>
        /// Run Test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
                Wait(4000);
                RebalancerTabButton.Click(MouseButtons.Left);

                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    DataRow row = testData.Tables[sheetIndexToName[0]].Rows[0];
                    InputDataRebalancertoRefresh(row);
                }

                Refresh2.DoubleClick(MouseButtons.Left);

                Wait(2000);

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
                //Minimize Rebalancer
                KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
            }
            return _result;
        }

        /// <summary>
        /// Selecting the Refresh Group Item from the Drop Down
        /// </summary>
        /// <param name="dr">DataRow from the Excel Sheet</param>
        private void InputDataRebalancertoRefresh(DataRow dr)
        {
            try
            {
                string value = string.Empty;

                //Select Refresh Type
                if (!dr[TestDataConstants.COL_REFRESHSIDE].ToString().Equals(String.Empty))
                {
                    value = dr[TestDataConstants.COL_REFRESHSIDE].ToString();

                    ToggleButton3.Click(MouseButtons.Left);
                    Wait(2000);

                    List<string> listCmbRefreshGroups = new List<string>();
                    listCmbRefreshGroups.Add("Positions");
                    listCmbRefreshGroups.Add("Prices");
                    listCmbRefreshGroups.Add("Positions and Prices");

                    int indexofRefreshGroupItem = listCmbRefreshGroups.IndexOf(value);

                    for (int i = 0; i < indexofRefreshGroupItem; i++)
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);

                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                }
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
