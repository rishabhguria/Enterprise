using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class VerifySymbolLookUp : SymbolLookupUIMap, ITestStep
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
                OpenSymbolLookup();
                StringBuilder symbolErrors = new StringBuilder(string.Empty);
                GrdData.Click(MouseButtons.Left);
                if (testData != null)
                {
                    foreach (DataRow dtRow in testData.Tables[sheetIndexToName[0]].Rows)
                    {
                       //string GridCSVData = GrdData.Properties[TestDataConstants.COL_DESCRIPTION].ToString();
                       //GridCSVData = GridCSVData.Replace(TestDataConstants.COL_SYSTEMSTRING, TestDataConstants.COL_SYSTEMOBJECT);
                        DataTable dtsymbolLookup = CSVHelper.CSVAsDataTable(this.GrdData.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                        List<string> errors = VerifyData(dtsymbolLookup, testData.Tables[sheetIndexToName[0]]);
                        if (errors.Count > 0)
                        {
                            _result.ErrorMessage = String.Join("\n\r", errors);
                        }
                    }
                }


                if (!string.IsNullOrEmpty(symbolErrors.ToString()))
                {
                    _result.ErrorMessage = String.Join("\n\r", symbolErrors);
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
                //Wait(5000);
                if (SymbolLookUp1.IsValid)
                {
                    if (Alert.IsVisible)
                    {
                        ButtonNo.Click(MouseButtons.Left);
                    }
                }
            }

            return _result;
        }

        
        /// <summary>
        /// Verifies the data.
        /// </summary>
        /// <param name="dtSymbolLookup">The dt symbol lookup.</param>
        /// <param name="testData">The test data.</param>
        /// <param name="testID">The test identifier.</param>
        public List<string> VerifyData(DataTable dtSymbolLookup, DataTable testData)
        {
            string errorMessage = string.Empty;
            try
            {
                List<String> columns = new List<String>();
                columns.Add(TestDataConstants.COL_TICKER);
                List<String> errors = new List<String>();
                errors = Recon.RunRecon(dtSymbolLookup, testData, columns, 0.01);
                return errors;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                return null;
            }
        }
    }
}
