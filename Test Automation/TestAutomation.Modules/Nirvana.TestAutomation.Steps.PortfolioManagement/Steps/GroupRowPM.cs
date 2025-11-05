using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    public partial class GroupRowPM : PortfolioManagementUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Failed to Group the row on PM. Reason : \n(" + ex.Message + ")</exception>
        
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                
                    OpenConsolidationView();
               
                String colList = string.Empty;
                List<string> columnList = new List<string>();
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    colList = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_NAME].ToString();
                }
                foreach (string colName in colList.Split(','))
                {
                    columnList.Add(colName);
                }
                this.Main.InvokeMethod("AddGrouping", columnList);
                //Wait(5000);
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
