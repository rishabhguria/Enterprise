using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.UIAutomation;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class AddCustomCashFlow: IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_REBALANCER"]);

                Dictionary<string, AutomationUniqueControlType> tempMapDic = null;
                tempMapDic = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["CustomCashFlowWindow"]);
                ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["RebalancerWindow"]);
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                UIAutomationHelper.DetectAndSwitchWindow(tempMapDic["PranaMain"].AutomationUniqueValue);
                try
                {
                    int maxRetries = 3;
                    int attempt = 0;
                    bool isWindowSwitched = false;

                    UIAutomationHelper.DetectAndSwitchWindow(tempMapDic["RebalancerModuleWindow"].AutomationUniqueValue, "6");
                    uiAutomationHelper.FindAndClickElement(ApplicationArguments.mapdictionary["btnCustomCashFlow"].AutomationUniqueValue);

                    while (attempt < maxRetries && !isWindowSwitched)
                    {
                        isWindowSwitched = UIAutomationHelper.DetectAndSwitchWindow(tempMapDic["ModuleWindow"].AutomationUniqueValue, "6");

                        if (!isWindowSwitched)
                        {
                            UIAutomationHelper.DetectAndSwitchWindow(tempMapDic["RebalancerModuleWindow"].AutomationUniqueValue, "6");
                            uiAutomationHelper.FindAndClickElement(ApplicationArguments.mapdictionary["btnCustomCashFlow"].AutomationUniqueValue);
                        }

                        attempt++;
                    }

                }
                catch (Exception ex)
                {
                    string errorMessage = "Exception Message: " + ex.Message + "\nStack Trace: " + ex.StackTrace;
                    Console.WriteLine(errorMessage);
                    throw new Exception(errorMessage, ex);
                }
                try
                {
                    ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables["CustomCashFlowWindow"]);
                    DataSet ds = new DataSet();
                    
                    
                    DataTable dt = GridDataProvider.CustomCashFlowNAVGrid(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue, ApplicationArguments.mapdictionary["GridName"].AutomationUniqueValue);
                    if (dt == null)
                    {
                        throw new Exception("Unable to extract grid data in CustomCashFlowWindow");
                    }
                    GridDataProvider gridData = new GridDataProvider();
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        try
                        {
                            string accountValue = dr["Account"].ToString();

                            var matchingRow = dt.AsEnumerable()
                            .FirstOrDefault(row => row["Account"].ToString() == accountValue);

                            if (matchingRow != null)
                            {
                                int index = dt.Rows.IndexOf(matchingRow);
                                
                                string valuetoFill = "0";
                                try
                                {
                                    valuetoFill = dr["Cash Flow"].ToString();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error parsing Cash Flow value: " + ex.Message);
                                }

                                gridData.CustomCashFlowNAVGridEdit(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue, ApplicationArguments.mapdictionary["GridName"].AutomationUniqueValue, index, valuetoFill);
                            }

                            
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing row: " + ex.Message);
                        }
                    }

                     dt = GridDataProvider.CustomCashFlowNAVGrid(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue, ApplicationArguments.mapdictionary["GridName"].AutomationUniqueValue);
                    
                    if (dt == null)
                    {
                        throw new Exception("Unable to extract grid data in CustomCashFlowWindow");
                    }
                   
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

