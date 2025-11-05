using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    public class SetUIPreferencesTT : TTPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        /// 

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenUIPrefrencesTT();
                if (testData != null)
                {
                    SetPreferences(testData.Tables[0].Rows[0], testData.Tables[0]);
                }
                Wait(1000);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetUIPreferencesTT");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                KeyboardUtilities.CloseWindow(ref PreferencesMain_UltraFormManager_Dock_Area_Top);
                Wait(2000);
            }

            return _result;
        }
        /// <summary>
        /// Opens the UI prefrences tt.
        /// </summary>
        private void OpenUIPrefrencesTT()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open Preferences under Tools (CTRL + ALT + F)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PREF"]);
                Wait(5000);
                // Tools.Click(MouseButtons.Left);
                //Preferences.Click(MouseButtons.Left);
                Trading.Click(MouseButtons.Left);
                UIPreferences.Click(MouseButtons.Left);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Sets the preferences.
        /// </summary>
        /// <param name="dr">The dr.</param>
        private void SetPreferences(DataRow dr, DataTable dt)
        {
            try
            {
                if (dt.Columns.Contains(TestDataConstants.Broker_Enable))
                {
                    if (!string.IsNullOrEmpty(dr[TestDataConstants.Broker_Enable].ToString()))
                    {
                        if (UcmbBroker.IsEnabled.ToString().ToUpper() != dr[TestDataConstants.Broker_Enable].ToString().ToUpper())
                        {
                            throw new Exception("Custodian broker checked status is " + UcmbBroker.IsChecked + " but excel is showing " + dr[TestDataConstants.Broker_Enable].ToString());
                        }
                    }
                }
                if (!UcmbBroker.Text.Equals(dr[TestDataConstants.COL_BROKER].ToString(), StringComparison.InvariantCultureIgnoreCase)) // change cmbobox only if value needs to be changed
                {
                    UcmbBroker.Click(MouseButtons.Left);
                    //ClearText(UcmbBroker);
                    DataUtilities.clearTextData();
                    Keyboard.SendKeys(dr[TestDataConstants.COL_BROKER].ToString());
                }
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                if (!UcmbOrderType.Text.Equals(dr[TestDataConstants.COL_ORDER_TYPE].ToString(), StringComparison.InvariantCultureIgnoreCase)) // change cmbobox only if value needs to be changed
                {
                    UcmbOrderType.Click(MouseButtons.Left);
                    //ClearText(UcmbOrderType);
                    DataUtilities.clearTextData();
                    Keyboard.SendKeys(dr[TestDataConstants.COL_ORDER_TYPE].ToString());
                }

                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                if (!UcmbTIF.Text.Equals(dr[TestDataConstants.COL_TIF].ToString(), StringComparison.InvariantCultureIgnoreCase)) // change cmbobox only if value needs to be changed
                {
                    UcmbTIF.Click(MouseButtons.Left);
                    //ClearText(UcmbTIF);
                    DataUtilities.clearTextData(20);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_TIF].ToString());
                }
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                UcmbAccount.Click(MouseButtons.Left);
                if (!UcmbAccount.Text.Equals(dr[TestDataConstants.COL_ACCOUNT].ToString(), StringComparison.InvariantCultureIgnoreCase)) // change cmbobox only if value needs to be changed
                {
                    //ClearText(UcmbAccount);
                    DataUtilities.clearTextData();
                    Keyboard.SendKeys(dr[TestDataConstants.COL_ACCOUNT].ToString());
                }

                UcmbTradingAcc.Click(MouseButtons.Left);
                if (!UcmbTradingAcc.Text.Equals(dr[TestDataConstants.COL_TRADER].ToString(), StringComparison.InvariantCultureIgnoreCase)) // change cmbobox only if value needs to be changed
                {
                    //ClearText(UcmbTradingAcc);   
                    DataUtilities.clearTextData();
                    Keyboard.SendKeys(dr[TestDataConstants.COL_TRADER].ToString());
                }

                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                if (dr[TestDataConstants.Col_Default_Quantity].ToString() == "Quantity")
                {
                    QuantityRadiobtn.Click(MouseButtons.Left);
                }
                else
                {
                    AmountRadiobtn.Click(MouseButtons.Left);
                }



                if (dt.Columns.Contains("Quantity") || dt.Columns.Contains("Increase On Quantity"))
                {
                    if (dr[TestDataConstants.COL_QUANTITY].ToString() != String.Empty && UNumQuantity.IsEnabled)
                    {
                        UNumQuantity.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_QUANTITY].ToString();
                    }
                    if (dr[TestDataConstants.COL_INCQUANTITY].ToString() != String.Empty && UNumIncQty.IsEnabled)
                    {
                        UNumIncQty.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_INCQUANTITY].ToString();
                    }
                    if (dr[TestDataConstants.COL_NUMINCSTOP].ToString() != String.Empty && UNumIncStop.IsEnabled)
                    {
                        UNumIncStop.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_NUMINCSTOP].ToString();
                    }
                    if (dr[TestDataConstants.COL_NUMINCLIMIT].ToString() != String.Empty && UNumIncLimit.IsEnabled)
                    {
                        UNumIncLimit.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_NUMINCLIMIT].ToString();
                    }

                    if (dt.Columns.Contains("Apply Round Lots"))
                    {
                        if (dr[TestDataConstants.COL_APPLYROUNDLOT].ToString() == "Yes")
                        {
                            RbtnRoundLotYes.Click(MouseButtons.Left);
                        }
                        else
                        {
                            RBtnRoundLotNo.Click(MouseButtons.Left);
                        }
                    }
                }

                BtnSave.Click(MouseButtons.Left);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Clears the text.
        /// </summary>
        /// <param name="cmb">The CMB.</param>
        private void ClearText(UIWindow cmb)
        {
            try
            {
                while (cmb.Text.Length > 0)
                {
                    cmb.Click(MouseButtons.Left);
                    Keyboard.SendKeys("[END][SHIFT+HOME][BACKSPACE]");

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
