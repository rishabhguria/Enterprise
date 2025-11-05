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

namespace Nirvana.TestAutomation.Steps.Import
{
    class ApplyFilterOnImport : ImportUIMap, ITestStep
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
                GrdImportData.Click(MouseButtons.Left);
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
                        this.GrdImportData.InvokeMethod("AddFilter", columnName, filterList);
                        //Wait(5000);
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
            return _result;
        }
    }
}
