using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
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
    public class GetDataSymbolLookup : SymbolLookupUIMap, ITestStep
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
                if (testData != null)
                {
                    foreach (DataRow dtRow in testData.Tables[sheetIndexToName[0]].Rows)
                    {
                       TxtbxInput.DoubleClick(MouseButtons.Left);

                     
                     Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);

           
//////////////////////////////////////
                      while (TxtbxInput.Text.Length > 0)
                        {
                       
                            TxtbxInput.DoubleClick(MouseButtons.Left);
                            
                            Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                        }

//////////////////////////////
                        Keyboard.SendKeys(dtRow[TestDataConstants.COL_SYMBOL].ToString());

                        CmbbxSearchType.Click(MouseButtons.Left);
                        Keyboard.SendKeys(dtRow[TestDataConstants.COL_SEARCH_TYPE].ToString());
                        BtnGetData.DoubleClick(MouseButtons.Left);
                        DataUtilities.waitForGridDataToGetVisible(GrdData, 10, 0);
                        //Wait(4000);
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
                MinimzieSybolLookup();
                //Wait(3000);
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

      }
}
