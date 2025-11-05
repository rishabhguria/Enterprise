using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class VerifyAlgoDetails : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                //Wait(3000);
                List<String> errors = InputEnter(testData.Tables[0]);
                KeyboardUtilities.CloseWindow(ref BlotterMain_UltraFormManager_Dock_Area_Top);
                if (errors.Count > 0)
                    _res.ErrorMessage = String.Join("\n\r", errors);
                Keyboard.SendKeys("[ALT+SPACE]N");
                
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyAlgoDetails");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
        private List<String> InputEnter(DataTable dTable)
        {
            try
            {
                //DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.OrderInformation.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataTable dtBlotter = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.OrderInformation.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                dtBlotter = DataUtilities.RemoveCommas(dtBlotter);
                List<String> columns = new List<string>();
                /*columns = (from DataColumn x in dtBlotter.Columns
                select x.ColumnName).ToList();*/
                columns.Add("StrategyName");
                List<String> errors = Recon.RunRecon(dtBlotter, dTable, columns, 0.01);
                return errors;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
