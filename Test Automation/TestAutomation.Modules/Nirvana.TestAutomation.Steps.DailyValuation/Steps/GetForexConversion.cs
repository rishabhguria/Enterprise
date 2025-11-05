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
    public class GetForexConversion : ForexConversionUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenForexConversion();

                DataTable inputData = testData.Tables[sheetIndexToName[0]];
                try
                {
                    DataUtilities.ReplaceTodayPlaceholders(inputData);
                }
                catch { }
                String selectDate = inputData.Rows[0][TestDataConstants.COL_SELECT_DATE].ToString();
                String copyDate = inputData.Rows[0][TestDataConstants.COL_COPY_DATE].ToString();

                if (selectDate.Length > 0 && copyDate.Length > 0)
                {
                    DailyForexConversion(inputData, selectDate, copyDate);
                }
                else
                {
                    MonthlyForexConversion(inputData);
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

        /// <summary>
        /// for Daily forex conversion.
        /// </summary>
        /// <param name="inputData">The test data.</param>
        /// <param name="selectDate">Select date.</param>
        /// <param name="copyDate">Copre date from</param>
        private void DailyForexConversion(DataTable inputData, String selectDate, String copyDate)
        {
            try
            {
                OptDaily.Click(MouseButtons.Left);
               // Wait(1000);
                String date = string.Empty;
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
                else if (WARNING.IsVisible)
                {
                    ButtonYes1.Click(MouseButtons.Left);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Monthly forex conversion.
        /// </summary>
        /// <param name="inputData">The test data.</param>
        private void MonthlyForexConversion(DataTable inputData)
        {
            try
            {
                OptMonthly.Click(MouseButtons.Left);
               // Wait(1000);
                String date = string.Empty;
                if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_SELECT_DATE].ToString()))
                {
                    date = String.Format(ExcelStructureConstants.MONTH_YEAR_DATE_FORMAT, DateTime.Parse(inputData.Rows[0][TestDataConstants.COL_SELECT_DATE].ToString()));
                    DtDateMonth.Click(MouseButtons.Left);
                    DtDateMonth.Properties["Text"] = date;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
      
    }
}
