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

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    public partial class ExportSymbolFromRAList : TTRestristedAllowedUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRestrictedAllowedTab();
                List<String> errors = CheckExportedSymbols(testData, sheetIndexToName);
                if (errors.Count > 0)
                {
                   _result.ErrorMessage = String.Join("\n\r", errors);
                }
                if (SecuritiesListExport.IsVisible)
                {
                    ButtonOK1.Click(MouseButtons.Left);
                }
                BtnSave.Click(MouseButtons.Left);
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
                KeyboardUtilities.CloseWindow(ref PreferencesMain_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }


        private List<string> CheckExportedSymbols(DataSet testData, Dictionary<int, String> sheetIndexToName)
        {
            try
            {
                DataTable dtExportData = ExportSymbol();
                List<String> columns = new List<String>();
                DataTable excelData = testData.Tables[sheetIndexToName[0]];
                List<String> errors = Utilities.Recon.RunRecon(dtExportData, excelData, columns);
                return errors;
            }
            catch (Exception)
            {
                throw;
            }

        }



    }
}
