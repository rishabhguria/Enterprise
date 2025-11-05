using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Linq;
using System.Text;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.ShortLocate
{
    public class ApplyFilterOnShortLocate : ShortLocateUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenShortLocateUI();
                UltraPanel1ClientArea3.Click(MouseButtons.Left);
                String columnName = string.Empty;
                String list = string.Empty;
                List<string> filterList = new List<string>();
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    for (int i = 0; i < testData.Tables[sheetIndexToName[0]].Rows.Count; i++)
                    {
                        columnName = testData.Tables[sheetIndexToName[0]].Rows[i][TestDataConstants.COL_NAME].ToString();
                        list = testData.Tables[sheetIndexToName[0]].Rows[i][TestDataConstants.COL_FILTERLIST].ToString();
                        filterList = list.Split(',').ToList();
                        this.GrdShortLocate1.InvokeMethod("AddFilter", columnName, filterList);
                       // Wait(5000);
                    }
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
                KeyboardUtilities.MinimizeWindow(ref ShortLocate_UltraFormManager_Dock_Area_Top1);
            }
            return _result;
        }
    }
}
