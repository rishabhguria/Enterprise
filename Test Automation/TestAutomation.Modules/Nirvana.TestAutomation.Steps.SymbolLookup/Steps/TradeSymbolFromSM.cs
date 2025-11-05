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
    public class TradeSymbolFromSM : SymbolLookupUIMap, ITestStep
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
                GrdData.Click(MouseButtons.Left);
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputEnter(dr);
                    }
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
               // Wait(5000);
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
        private void InputEnter(DataRow dr)
        {
            try
            {
                var msaaObj = GrdData.MsaaObject;
                DataTable dtsymbolLookup = CSVHelper.CSVAsDataTable(this.GrdData.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtsymbolLookup), dr);
                int index = dtsymbolLookup.Rows.IndexOf(dtRow);
                GrdData.InvokeMethod("ScrollToRow", index);
                msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                if (PopupMenuDropDown.IsVisible)
                {
                    TradeSymbol.Click(MouseButtons.Left);
                }
                else
                {
                    throw new Exception("Pop Up Menu Drop down Not Visible");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
