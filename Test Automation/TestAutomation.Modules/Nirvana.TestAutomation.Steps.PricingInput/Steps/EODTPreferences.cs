using System;
using System.ComponentModel;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Configuration;
namespace Nirvana.TestAutomation.Steps.PricingInput
{
    [UITestFixture]
    public partial class EODTPreferences : PricingInputUIMap,ITestStep
    {

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 60);
                }
                //Shortcut to open pricing input module(CTRL + ALT + P)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PRICING_INP"]);
                ExtentionMethods.WaitForVisible(ref OptionModelInputs, 15);
                //Wait(5000);
                //Tools.Click(MouseButtons.Left);
                //PricingInputsMenuItem.Click(MouseButtons.Left);
                OptionModelInputs.WaitForVisible();
                Preferences1.Click(MouseButtons.Left);
                UseClosingMarkstosimulateEODTMinus1snapshot.Click(MouseButtons.Left);
                OpenInsertValues(testData);
                BtnSavePreferences.Click(MouseButtons.Left);
               // Wait(2000);

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
            //DataTable dt = collection[0];
            try
            {
                DataRow dt = testData.Tables[0].Rows[0];
                if (dt[TestDataConstants.COL_USE_DEFAULT_DELTA].ToString() == "True" && !CheckBoxUseDefaultDelta.IsChecked)
                {
                    CheckBoxUseDefaultDelta.Check();
                }
                else if (dt[TestDataConstants.COL_USE_DEFAULT_DELTA].ToString() == "False" && CheckBoxUseDefaultDelta.IsChecked)
                {
                    CheckBoxUseDefaultDelta.Uncheck();

                }
                //Wait(1000);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
