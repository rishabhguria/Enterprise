using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    class SetGeneralPreference : TTGeneralPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Not Able to Set General Preferences.!\n (" + ex.Message + " )</exception>
        
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenGeneralPreferences();
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputDetailsInGeneralPreferences(dr, testData);
                        Wait(2000);
                        BtnSave.DoubleClick(MouseButtons.Left);
                    }
                }
                else
                {
                    _result.AddResult(false, "Input data is not provided");
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetGeneralPreference");
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

        /// <summary>
        /// Inputs the details in general preferences.
        /// </summary>
        /// <param name="dr">The dr.</param>
        private void InputDetailsInGeneralPreferences(DataRow dr, DataSet testData)
        {
            try
            {
                if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_Button_Status))
                {
                    if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_Button_Status].ToString()))
                    {
                        if (ChkUseCustodianAsExecutingBroker.IsChecked.ToString().ToUpper() != dr[TestDataConstants.COL_Button_Status].ToString().ToUpper())
                        {
                            throw new Exception("Custodian broker checked status is " + ChkUseCustodianAsExecutingBroker.IsChecked + " but excel is showing " + dr[TestDataConstants.COL_Button_Status].ToString());
                        }
                    }
               
                    if (!String.IsNullOrEmpty(dr[TestDataConstants.Cusdtodian_Broker].ToString())) {
                        if (ChkUseCustodianAsExecutingBroker.IsChecked.ToString().ToUpper() != dr[TestDataConstants.Cusdtodian_Broker].ToString().ToUpper())
                        {
                            ChkUseCustodianAsExecutingBroker.Click(MouseButtons.Left);
                        }
                    }
                      
                }
                if (dr[TestDataConstants.COL_DEFAULT_SYMBOLOGY].ToString() != String.Empty)
                {
                    CmbSymbology.Click(MouseButtons.Left);
                    string temp = dr[TestDataConstants.COL_DEFAULT_SYMBOLOGY].ToString();
                    Keyboard.SendKeys(temp[0].ToString());
                    CmbSymbology.Click(MouseButtons.Left);
                }
                if (dr[TestDataConstants.COL_DEFAULT_BROKER_NOTES].ToString() != String.Empty)
                {
                    TxtDefaultBrokerNotes.Click(MouseButtons.Left);
                    TxtDefaultBrokerNotes.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_DEFAULT_BROKER_NOTES].ToString();                    
                   
                }
                if (dr[TestDataConstants.COL_DEFAULT_NOTES].ToString() != String.Empty)
                {
                    TxtDeafultNotes.Click(MouseButtons.Left);
                    TxtDeafultNotes.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_DEFAULT_NOTES].ToString();
                    
                }
                
                if (dr[TestDataConstants.COL_DEFAULT_OPTION_TYPE].ToString() != String.Empty)
                {
                    CmbDefaultOptionType.Click(MouseButtons.Left);
                    string temp = dr[TestDataConstants.COL_DEFAULT_OPTION_TYPE].ToString();
                    Keyboard.SendKeys(temp[0].ToString());
                    CmbDefaultOptionType.Click(MouseButtons.Left);
                    TxtDefaultBrokerNotes.Click(MouseButtons.Left);
                    TxtDefaultBrokerNotes.Click(MouseButtons.Left);
                }
                if ((dr[TestDataConstants.COL_SHOW_OPTION_DETAILS].ToString().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase) && (!ChkShowOptionDetails.IsChecked)) || (dr[TestDataConstants.COL_SHOW_OPTION_DETAILS].ToString().Equals("FALSE", StringComparison.InvariantCultureIgnoreCase) && (ChkShowOptionDetails.IsChecked)))
                {
                    ChkShowOptionDetails.Click(MouseButtons.Left);
                }
                if ((dr[TestDataConstants.COL_KEEP_TRADING_TICKET_OPEN].ToString().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase) && (!ChkKeepTTOpen.IsChecked)) || (dr[TestDataConstants.COL_KEEP_TRADING_TICKET_OPEN].ToString().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase) && (ChkKeepTTOpen.IsChecked)))
                {
                    ChkKeepTTOpen.Click(MouseButtons.Left);
                }
                if (testData.Tables[0].Columns.Contains(TestDataConstants.CLEARCOLVALUE))
                {
                    if (!string.IsNullOrEmpty(dr[TestDataConstants.CLEARCOLVALUE].ToString()))
                    {
                        String[] ColValue = dr[TestDataConstants.CLEARCOLVALUE].ToString().Split(',');
                        foreach (String value in ColValue)
                        {
                            if (value.ToUpper() == "DEFAULTNOTES" || value.ToUpper() == "DEFAULT NOTES")
                            {
                                TxtDeafultNotes.Click(MouseButtons.Left);
                                DataUtilities.clearTextData(20, true);
                            }
                            if (value.ToUpper() == "DEFAULTBROKERNOTES" || value.ToUpper() == "DEFAULT BROKER NOTES")
                            {
                                TxtDefaultBrokerNotes.Click(MouseButtons.Left);
                                DataUtilities.clearTextData(20, true);
                            }
                        }
                    }
                }
                
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
