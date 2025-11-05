using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.MultiTradingTicket
{
    class VerifyMTT : MultiTradingTicketUIMap, ITestStep 
    {
        /// <summary>
        /// Run test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet name</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                List<String> errors = VerifyData(testData.Tables[0]);
                if (errors.Count > 0)
                    _res.ErrorMessage = String.Join("\n\r", errors);

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyMTT");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
               
            }
            return _res;
        }

        /// <summary>
        /// This method compares the data of the MTT grid with the excel sheet
        /// </summary>
        /// <param name="dTable"></param>
        /// <returns></returns>
        private List<String> VerifyData(DataTable dTable)
        {
            List<string> errors = new List<string>();
            try
            {
                DataTable dtBlotter = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.GrdTrades.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                dtBlotter = DataUtilities.RemoveCommas(dtBlotter);
                List<String> columns = new List<string>();
                errors = Recon.RunRecon(dtBlotter, dTable, columns, 0.01);
               
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errors;
        }
           
        

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
