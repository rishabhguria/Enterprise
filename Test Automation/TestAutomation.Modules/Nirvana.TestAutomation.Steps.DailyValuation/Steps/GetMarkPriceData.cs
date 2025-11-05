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
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
   public class GetMarkPriceData : MarkPriceUIMap, ITestStep
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
                //Wait(500);
                OpenMarkPriceTab();

                DataTable inputData = testData.Tables[sheetIndexToName[0]];               
                String selectDate = inputData.Rows[0][TestDataConstants.COL_SELECT_DATE].ToString();
                String copyDate = inputData.Rows[0][TestDataConstants.COL_COPY_DATE].ToString();

                if (selectDate.Length > 0 && copyDate.Length > 0)
                {
                    DailyMarkPrice(inputData, selectDate, copyDate);
                }
                else
                {
                    MonthlyMarkPrice(inputData);
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
       /// Daily mark price 
       /// </summary>
       /// <param name="inputData">The test data.</param>
       /// <param name="selectDate">Select date.</param>
       /// <param name="copyDate">Copy data from.</param>
        private void  DailyMarkPrice(DataTable inputData, String selectDate, String copyDate)
        {
            try
            {
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
               
            }
            catch (Exception)
            {                
                throw;
            }
            
        }

       /// <summary>
       /// Monthly mark price.
       /// </summary>
       /// <param name="inputData">The test data.</param>
       /// <param name="selectDate">c</param>
        private void MonthlyMarkPrice(DataTable inputData)
        {
            try
            {               
                String date = string.Empty;
                if (!string.IsNullOrWhiteSpace(inputData.Rows[0][TestDataConstants.COL_SELECT_DATE].ToString()))
                {
                    date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(inputData.Rows[0][TestDataConstants.COL_SELECT_DATE].ToString()));
                    DtDateMonth.Click(MouseButtons.Left);
                    DtDateMonth.Properties["Text"] = date;                    
                }

                OptMonthly.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
      
    }
}
