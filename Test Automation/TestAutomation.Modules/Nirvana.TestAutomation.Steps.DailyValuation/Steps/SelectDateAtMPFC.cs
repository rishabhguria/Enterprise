using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.BussinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using TestAutomationFX.Core;
using System.IO;
using TestAutomationFX.UI;
using System.Xml;
using Nirvana.TestAutomation.UIAutomation;
using System.Configuration;
using System.Threading;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    public class SelectDateAtMPFC : DailyCashUIMap, IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["MarkPriceAndForexConversion"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                OpenDailyValuation();
                try
                {
                    bool isactionCompleted = uiAutomationHelper.SelectDateAtMPFC(testData.Tables[0], ApplicationArguments.mapdictionary);
                    Thread.Sleep(3000);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    try
                    {
                        if (ERROR.IsVisible)
                        {
                            ButtonOK.Click(MouseButtons.Left);
                        }
                        else if (CONFIRMATION.IsVisible)
                        {
                            ButtonYes.Click(MouseButtons.Left);
                        }
                        else if (WARNING.IsVisible)
                        {
                            ButtonYes1.Click(MouseButtons.Left);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    _result.IsPassed = false;
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                    if (rethrow)
                        throw;
                }

            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }
       
        protected void OpenDailyValuation()
        {
            try
            {
                int maxRetry = 3;
                string moduleWindow = ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue;
                string openDailyValShortcut = ConfigurationManager.AppSettings["OPEN_DAILY_VAL"];

                while (maxRetry > 0)
                {
                    if (UIAutomationHelper.DetectAndSwitchWindow(moduleWindow))
                    {
                        return;
                    }

                    Keyboard.SendKeys(openDailyValShortcut);
                    maxRetry--;
                    Thread.Sleep(500);
                }

                throw new InvalidOperationException("Failed to detect and switch to the module window after maximum retries.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OpenDailyValuation: "+ex.Message);
            }
        }

    }
}
