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
    public class GetForwardPoints : ForwardPointsUIMap,ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet no.</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenForwardPointsTab();
                DataTable inputData = testData.Tables[sheetIndexToName[0]];
                String date = string.Empty;
                String selectDate = inputData.Rows[0][TestDataConstants.COL_SELECT_DATE].ToString();
                String copyDate = inputData.Rows[0][TestDataConstants.COL_COPY_DATE].ToString();

                if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_SELECT_DATE].ToString()))
                {
                    date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(inputData.Rows[0][TestDataConstants.COL_SELECT_DATE].ToString()));
                    DtDateMonth.Click(MouseButtons.Left);
                    DtDateMonth.Properties["Text"] = date;                    
                }

                if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_COPY_DATE].ToString()))
                {
                    date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(inputData.Rows[0][TestDataConstants.COL_COPY_DATE].ToString()));
                    DtCopyFromDate.Click(MouseButtons.Left);
                    DtCopyFromDate.Properties["Text"] = date;                    
                }
                BtnFetchData.Click(MouseButtons.Left);

                if (ERROR.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                }
                else if (CONFIRMATION.IsVisible)
                {
                    ButtonYes.Click(MouseButtons.Left);
                }
                else if (WARNING1.IsVisible)
                {
                    ButtonNo.Click(MouseButtons.Left);
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
