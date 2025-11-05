using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.UIAutomation;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class ActionOnTBSListSummary : RebalancerUIMap, IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                try
                {
                    UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                     ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["TradeBuySellListSummaryView"]);
                     bool isactionCompleted = uiAutomationHelper.TestScriptActiononButton(testData.Tables[0], ApplicationArguments.mapdictionary);

                     if (!isactionCompleted)
                     {
                         Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
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
                    CloseTradeList();
                    Wait(4000);
                    KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
                }
                return _result;
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
    }
}
