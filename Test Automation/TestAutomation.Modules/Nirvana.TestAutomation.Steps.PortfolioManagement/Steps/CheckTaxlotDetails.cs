using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using TestAutomationFX.Core.UIAutomationSupport;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    class CheckTaxlotDetails : PortfolioManagementUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                List<String> errors = CheckTaxLot(testData);
                if (errors.Count > 0)
                    _result.ErrorMessage = String.Join("\n\r", errors);


            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CheckTaxlotDetails");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                KeyboardUtilities.CloseWindow(ref PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top);
                Wait(1000);
                PMclose();
            }
            return _result;
        }

        private List<String> CheckTaxLot(DataSet dtable)
        {
            try
            {
                UltraGrid1.WaitForRespondingOrExited();
                KeyboardUtilities.MaximizeWindow(ref PMTaxLotsDisplayForm_UltraFormManager_Dock_Area_Top);
                UltraGrid1.Click(MouseButtons.Left);
                Wait(2000);
                DataTable superset = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(UltraGrid1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                List<String> columns = new List<string>();
                List<String> errors = Recon.RunRecon(superset, dtable.Tables[0], columns, 0.01);
                return errors;
            }
            catch (Exception)
            {
                throw;
            }


        }
    }
}
