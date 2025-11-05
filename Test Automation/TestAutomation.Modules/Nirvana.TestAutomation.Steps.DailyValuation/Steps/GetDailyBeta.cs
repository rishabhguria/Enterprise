using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.Steps.DailyValuation 
{
    public class GetDailyBeta: DailyBetaUIMap, ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet no.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenDailyBetaTab();

                DataTable inputData = testData.Tables[sheetIndexToName[0]];
                String date = string.Empty;               

                if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_SELECT_DATE].ToString()))
                {
                    date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(inputData.Rows[0][TestDataConstants.COL_SELECT_DATE].ToString()));
                    DtDateMonth.Click(MouseButtons.Left);
                    DtDateMonth.Properties["Text"] = date;
                }

                if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_COPY_FROM].ToString()))
                {
                    date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(inputData.Rows[0][TestDataConstants.COL_COPY_FROM].ToString()));
                    DtCopyFromDate.Click(MouseButtons.Left);
                    DtCopyFromDate.Properties["Text"] = date;
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
