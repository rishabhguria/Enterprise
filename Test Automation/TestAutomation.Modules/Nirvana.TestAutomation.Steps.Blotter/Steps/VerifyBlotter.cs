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
using TestAutomationFX.Core;


namespace Nirvana.TestAutomation.Steps.Blotter
{
    class VerifyBlotter : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]);
            UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();

            UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["PranaMain"].AutomationUniqueValue);
            Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_BLOTTER"]);
            UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);

            try
            {
                List<string> errors = new List<string>();
                List<string> columns = new List<string>();
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                try
                {
                    string StepName = "VerifyBlotter";
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref subset);
                }
                catch (Exception ex)
                {
                    string errorMessage = "Exception Message: " + ex.Message + "\nStack Trace: " + ex.StackTrace;
                    Console.WriteLine(errorMessage);
                    throw new Exception(errorMessage, ex);
                }


                DataSet ds = UIAutomationHelper.DetectBlotterGridAndExtractData();

                try
                {
                    DataUtilities.RemoveEmptyRows(ds);
                    DataTable[] tables = { ds.Tables[0], subset };
                    foreach (var table in tables)
                    {
                        DataUtilities.RemoveCommas(table);
                        DataUtilities.RemovePercent(table);
                        DataUtilities.RemoveTrailingZeroes(table);
                    }
                    errors = Recon.RunRecon(ds.Tables[0], subset, columns, 0.01);
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
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

            return _res;
        }
    }
}
