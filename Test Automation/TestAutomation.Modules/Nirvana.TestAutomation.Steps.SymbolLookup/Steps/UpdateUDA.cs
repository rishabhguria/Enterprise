using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using System.Windows.Forms;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.SymbolLookup
{
    public class UpdateUDA : SymbolLookupUIMap, ITestStep
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
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    OpenSymbolLookup();
                    UpdateUDAValue(testData, sheetIndexToName);
                    Wait(1000);
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
                Wait(5000);
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
        /// Updates the uda value.
        /// </summary>
        /// <param name="testData">The test data.</param>
        private void UpdateUDAValue(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                var msaaObj = GrdData.MsaaObject;
                DataTable dtsymbolLookup = CSVHelper.CSVAsDataTable(this.GrdData.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                msaaObj.CachedChildren[0].CachedChildren[1].Click(MouseButtons.Right);
                Edit.Click(MouseButtons.Left);
                Wait(2000);
                if (dr[TestDataConstants.COL_BLOOMBERG_SYMBOL].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgpcBloombergSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_BLOOMBERG_SYMBOL].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                BtnNextTab.Click(MouseButtons.Left);

                if (dr.Table.Columns.Contains(TestDataConstants.ALLOWCAMELCASE))
                {
                    if (dr[TestDataConstants.ALLOWCAMELCASE].ToString().ToUpper().Equals("YES"))
                    {
                        CbActivSymbolCamelCase.Click(MouseButtons.Left);
                    }
                }
                if (dr.Table.Columns.Contains(TestDataConstants.ACTIVSYMBOL))
                {
                    if (dr[TestDataConstants.ACTIVSYMBOL].ToString() != string.Empty)
                    {
                        UgpcActivSymbol.Click(MouseButtons.Left);
                        Keyboard.SendKeys(dr[TestDataConstants.ACTIVSYMBOL].ToString());
                    }
                }

                BtnNextTab.Click(MouseButtons.Left);
                if (dr[TestDataConstants.UDA_Security_Type].ToString() != string.Empty)
                {
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    UgcpUDASecurity.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.UDA_Security_Type].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.UDA_Country].ToString() != string.Empty)
                {
                    UgpcUDACountry.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.UDA_Country].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.UDA_Asset_Class].ToString() != string.Empty)
                {
                    UgpcAssetID.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.UDA_Asset_Class].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.UDA_Sector].ToString() != string.Empty)
                {
                    UgpcUDASector.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.UDA_Sector].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.UDA_Sub_Sector].ToString() != string.Empty)
                {
                    UgpcUDASubSector.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.UDA_Sub_Sector].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }

                //Update UDA info from underlying symbol
                if (dr.Table.Columns.Contains(TestDataConstants.USERDEFAULTUDA))
                {
                    if (dr[TestDataConstants.USERDEFAULTUDA].ToString().ToUpper() == "YES")
                    {
                        GcpcUseDefaultUDA.Click(MouseButtons.Left);
                    }
                }

                BtnNextTab.Click(MouseButtons.Left);
                BtnSave.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}