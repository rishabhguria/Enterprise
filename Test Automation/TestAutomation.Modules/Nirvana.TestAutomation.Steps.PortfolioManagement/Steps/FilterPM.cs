using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    public partial class FilterPM : PortfolioManagementUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Failed to filter data on PM. Reason : \n(" + ex.Message + ")</exception>

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                    OpenConsolidationView();
                String columnName = string.Empty;
                String list = string.Empty;
                List<string> filterList = new List<string>();
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    
                    for (int i = 0; i < testData.Tables[sheetIndexToName[0]].Rows.Count; i++)
                    {
                        columnName = string.Empty;
                        list = string.Empty;
                        filterList.Clear();
                       
                        columnName = testData.Tables[sheetIndexToName[0]].Rows[i][TestDataConstants.COL_NAME].ToString();
                        list = testData.Tables[sheetIndexToName[0]].Rows[i][TestDataConstants.COL_FILTERLIST].ToString();


                        foreach (string colName in list.Split(','))
                        {
                            filterList.Add(colName);
                        }
                        this.Main.InvokeMethod("AddFilter", columnName, filterList);
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