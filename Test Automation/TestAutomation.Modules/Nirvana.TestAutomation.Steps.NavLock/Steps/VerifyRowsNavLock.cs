using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Interfaces.Enums;
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

namespace Nirvana.TestAutomation.Steps.NavLock
{
    class VerifyRowsNavLock : NavLockUIMap, ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet name.</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenNavLock();
                DataTable dt = testData.Tables[sheetIndexToName[0]];
                DataTable dTable = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.PranaUltraGrid1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                List<String> errors = new List<String>();
                List<String> columns = new List<String>();
                columns.Add("Locked By");
                errors = Recon.RunRecon(dTable, dt, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
                if (errors.Count > 0)
                {
                    _result.ErrorMessage = String.Join("\n\r", errors);
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
                if (FrmNavLock.IsEnabled)
                {
                    KeyboardUtilities.CloseWindow(ref AboutPrana_UltraFormManager_Dock_Area_Top);
                }
            }
            return _result;
        }
    }
}
