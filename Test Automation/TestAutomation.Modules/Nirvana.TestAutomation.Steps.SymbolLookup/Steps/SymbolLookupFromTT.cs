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


namespace Nirvana.TestAutomation.Steps.SymbolLookup
{
    public class SymbolLookupFromTT : SymbolLookupUIMap, ITestStep
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
                List<String> errors = VerifySymbolData(testData, sheetIndexToName);
                GrdData.Click(MouseButtons.Left);
                if (errors.Count > 0)
                {
                    _result.ErrorMessage=String.Join("\n\r", errors);
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
                CloseSymbolLookup();
            }
            return _result;
        }

        /// <summary>
        /// Verifies the symbol data.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        private List<string> VerifySymbolData(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataTable superset = CSVHelper.CSVAsDataTable(this.GrdData.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
               
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                List<String> columns = new List<String>();
                columns.Add(TestDataConstants.COL_TICKER);
                List<String> errors = new List<String>();
                errors = Recon.RunRecon(superset, subset, columns, 0.01);
                return errors;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}