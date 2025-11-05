using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.PricingInput
{
    [UITestFixture]
    public partial class LiveData : PricingInputUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open pricing input module(CTRL + ALT + P)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PRICING_INP"]);
                //Wait(5000);
                ExtentionMethods.WaitForVisible(ref OptionModelInputs, 15);
                //Tools.Click(MouseButtons.Left);
                //PricingInputsMenuItem.Click(MouseButtons.Left);
                OptionModelInputs.WaitForVisible();
                Preferences1.Click(MouseButtons.Left);
                UltraPanel11.Click(MouseButtons.Left);
                OpenInsertValues(testData);
                BtnSavePreferences.Click(MouseButtons.Left);
                //Wait(2000);
                
                //DataRow dataRow = testData.Tables[sheetIndexToName[0]].Rows;
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
                KeyboardUtilities.CloseWindow(ref OptionModelInputs_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        private void OpenInsertValues(DataSet testData)
        {
            //DataTableCollection collection = testData.Tables;
            //DataTable dt=collection[0];
            try
            {
                DataRow dt = testData.Tables[0].Rows[0];
                if (!LiveData.IsChecked)
                {
                    LiveData.Click(MouseButtons.Left);
                }
                Wait(2000);
                Label4.Click(MouseButtons.Left);
                if (dt[TestDataConstants.COL_OPTIONPRICE].ToString() != "")
                {
                    CmbOptPrice.Click(MouseButtons.Left);
                    CmbOptPrice.Properties["Text"] = dt[TestDataConstants.COL_OPTIONPRICE].ToString();
                }
                Label2.Click(MouseButtons.Left);
                if (dt[TestDataConstants.COL_OPTION_OVERRIDE_SELECTED_FEED_PRICE].ToString() != "")
                {
                    CmbBxOverrideSelectedPxOptions.Click(MouseButtons.Left);
                    CmbBxOverrideSelectedPxOptions.Properties["Text"] = dt[TestDataConstants.COL_OPTION_OVERRIDE_SELECTED_FEED_PRICE].ToString();
                }
                Label6.Click(MouseButtons.Left);
                if (dt[TestDataConstants.COL_ASSETPRICE].ToString() != "")
                {
                    CmbStockPrice.Click(MouseButtons.Left);
                    CmbStockPrice.Properties["Text"] = dt[TestDataConstants.COL_ASSETPRICE].ToString();
                }
                Label3.Click(MouseButtons.Left);
                if (dt[TestDataConstants.COL_ASSET_OVERRIDE_SELECTED_FEED_PRICE].ToString() != "")
                {
                    CmbBxOverrideSelectedPxOthers.Click(MouseButtons.Left);
                    CmbBxOverrideSelectedPxOthers.Properties["Text"] = dt[TestDataConstants.COL_ASSET_OVERRIDE_SELECTED_FEED_PRICE].ToString();
                }
                if (dt[TestDataConstants.COL_USE_DEFAULT_DELTA].ToString() == "True" && !CheckBoxUseDefaultDelta.IsChecked)
                {
                    CheckBoxUseDefaultDelta.Check();
                }
                else if (dt[TestDataConstants.COL_USE_DEFAULT_DELTA].ToString() == "False" && CheckBoxUseDefaultDelta.IsChecked)
                {
                    CheckBoxUseDefaultDelta.Uncheck();

                }
               // Wait(1000);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
