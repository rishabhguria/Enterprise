using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.UIAutomation;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;

namespace Nirvana.TestAutomation.Steps.Unknown
{
    class VerifyOverBuySellTR : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["TradingRulesViolatedPopUp"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();

                UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

                if (testData.Tables[0].Columns.Contains("ButtonNo"))
                {
                    try
                    {
                        uiAutomationHelper.CommonAction(testData.Tables[0], "", ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                    }
                    catch (Exception ex)
                    {
                        _res.IsPassed = false;
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                        if (rethrow)
                            throw;
                    }
                }
                else
                {
                    DataSet ds = uiAutomationHelper.ExtractWinformGridData();
                    List<string> errors = new List<string>();
                    List<string> columns = new List<string>();
                    try
                    {
                        DataUtilities.RemoveEmptyRows(ds);
                        DataTable uiData = ds.Tables[ApplicationArguments.mapdictionary["ultraGridOverBuyOverSellRuleViolated"].AutomationUniqueValue];
                        errors = Recon.RunRecon(uiData, testData.Tables[0], columns, 0.01);
                        if (errors.Count > 0)
                            _res.ErrorMessage = String.Join("\n\r", errors);
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                        if (rethrow)
                            throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {

            }
            return _res;
        }
    }
}
