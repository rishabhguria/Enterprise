using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.UIAutomation;
using TestAutomationFX.Core;
using System.Configuration;
using UIAutomationClient;
using System.Threading;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class EditAttRenaming : IUIAutomationTestStep
    {
        public static Dictionary<string, List<FocusGridData>> GridFocusDataDictionary = null;

        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {

            TestResult _res = new TestResult();
            try
            {
                if (testData == null
                                 || testData.Tables[0].Rows.Count <= 0 
                                 || !testData.Tables[0].Columns.Contains("AttributeName")
                                 || !testData.Tables[0].Columns.Contains("KeepRecord")
                                 || !testData.Tables[0].Columns.Contains("DefaultValues"))
                {
                    return _res;
                }

                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_ALLOCATION"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["AllocationPreferenceWindow"]);
                try
                {
                    UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                    uiAutomationHelper.FindAndClickElementIfVisible(ApplicationArguments.mapdictionary["AttributeTabRenaming"].AutomationUniqueValue);

                    try
                    {
                        IUIAutomation automation = new CUIAutomation();
                        IUIAutomationElement rootElement = automation.GetRootElement();
                        ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["AllocationPreferenceWindow"]);

                        IUIAutomationElement windowElement = null;
                        int retryCount = 2;
                        bool success = false;

                        while (retryCount > 0 && !success)
                        {
                            try
                            {
                                windowElement = UIAutomationHelper.FindElementByUniquePropertyType(
                                    automation,
                                    rootElement,
                                    ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType,
                                    ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue
                                );
                                success = true;
                            }
                            catch (Exception ex)
                            {
                                retryCount--;
                                Console.WriteLine(string.Format("Error finding Window element (attempt {0}): {1}", 3 - retryCount, ex.Message));

                                if (retryCount == 0)
                                {
                                    Console.WriteLine("All retry attempts failed.");
                                    throw;
                                }

                                Thread.Sleep(2000);
                            }
                        }
                        if (windowElement != null)
                        {
                            try
                            {
                                DataTable ExcelData = testData.Tables[0];
                                GridDataProvider gridDataProvider = new GridDataProvider();
                                GridDataProcessor processor = new GridDataProcessor();

                                string moduleID = "AllocationPreferenceWindow";
                                string gridID = "AttributeRenaming";
                                string DataExtractionScriptName = "AttributeRenaming";
                                string tableName = "AttributeRenaming";
                                string framework =  "WPF";

                                if (GridFocusDataDictionary == null)
                                {
                                    GridFocusDataDictionary = processor.ProcessFocusedGridData(ApplicationArguments.IUIAutomationMappingTables["GetUIGridData"]);
                                }

                                bool successEditGridDataState = uiAutomationHelper.EditGridData(GridFocusDataDictionary, moduleID, gridID, DataExtractionScriptName, framework, ExcelData);

                                if (!successEditGridDataState)
                                {
                                    //throw;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error retrieving GetUIGridData: " + ex.Message);
                                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in GetWPFGridData: " + ex.Message);
                        Console.WriteLine("Stack Trace: " + ex.StackTrace);
                        throw;
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
