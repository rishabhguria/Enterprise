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
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
  public  class SetBrokerWisePreferencesTT : BrokerWisePreferencesTTUIMap , ITestStep
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
                OpenBrokerWisePrefrencesTT();
                SetPreferences(testData);
                Wait(2000);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetBrokerWisePreferencesTT");
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
        /// Sets the preferences.
        /// </summary>
        /// <param name="testData">The test data.</param>
        private void SetPreferences(DataSet testData)
        {
            try
            {
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputPreferences(dr);
                        Wait(1000);
                    }
                    BtnSave.Click(MouseButtons.Left);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        /// <summary>
        /// Opens the broker wise prefrences tt.
        /// </summary>
        private void OpenBrokerWisePrefrencesTT()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PREF"]);
                ExtentionMethods.WaitForVisible(ref PreferencesMain, 10);
                if (PreferencesMain.IsVisible)
                {
                    Trading.Click(MouseButtons.Left);
                    BrokerWisePreferences.Click(MouseButtons.Left);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Inputs the preferences.
        /// </summary>
        /// <param name="dr">The dr.</param>
        private void InputPreferences(DataRow dr)
        {
            try
            {
                var gridMssaObject = GrdBrokerWiseSettings.MsaaObject;
                //Dictionary that stores mapping of column names with it's index
                Dictionary<string, int> colToIndexMappingDictionary = new Dictionary<string, int>();
                for (int i = 1; i < gridMssaObject.CachedChildren[0].ChildCount; i++)
                {
                    colToIndexMappingDictionary.Add(gridMssaObject.CachedChildren[0].CachedChildren[i].CachedChildren[0].Value, i);
                }
                if (dr["Venue"].ToString() != String.Empty)
                {
                    gridMssaObject.CachedChildren[0].CachedChildren[colToIndexMappingDictionary[dr["Broker"].ToString()]].CachedChildren[1].Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr["Venue"].ToString());
                }
                if (dr["Algo Type"].ToString() != String.Empty)
                {
                    gridMssaObject.CachedChildren[0].CachedChildren[colToIndexMappingDictionary[dr["Broker"].ToString()]].CachedChildren[2].Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr["Algo Type"].ToString());
                }
                if (dr["Commission Basis"].ToString() != String.Empty)
                {
                    gridMssaObject.CachedChildren[0].CachedChildren[colToIndexMappingDictionary[dr["Broker"].ToString()]].CachedChildren[3].Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr["Commission Basis"].ToString());
                }
                if (dr["Soft Commission Basis"].ToString() != String.Empty)
                {
                    gridMssaObject.CachedChildren[0].CachedChildren[colToIndexMappingDictionary[dr["Broker"].ToString()]].CachedChildren[4].Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr["Soft Commission Basis"].ToString());
                }
                if (dr["Execution Instruction"].ToString() != String.Empty)
                {
                    gridMssaObject.CachedChildren[0].CachedChildren[colToIndexMappingDictionary[dr["Broker"].ToString()]].CachedChildren[5].Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr["Execution Instruction"].ToString());
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
