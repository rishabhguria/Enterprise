using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.UIAutomation;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestAutomationFX.Core;
using UIAutomationClient;
using System.Xml.Linq;
using System.Xml;


namespace Nirvana.TestAutomation.UIAutomation
{
    public class UIAutomationRunData : IUIAutomationTestStep
    {
        public static Dictionary<string, List<FocusGridData>> GridFocusDataDictionary = null;
        public static Dictionary<string, List<FocusGridData>> GridEditFocusDataDictionary = null;
        public static String ActiveGrid = string.Empty;
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                if (testData.Tables[0] == null || testData.Tables[0].Rows.Count <= 0)
                {
                    throw new Exception(ApplicationArguments.ActiveStep +" failed as DataSet is empty.");
                }
                DataTable uiData = null;
                DataTable ExcelData = testData.Tables[0];
                UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                IUIAutomationElement targetElement = null;
                GridDataProvider gridDataProvider = new GridDataProvider();
                GridDataProcessor processor = new GridDataProcessor();
              
            
                if (ApplicationArguments.IUIAutomationDataTables == null || !ApplicationArguments.IUIAutomationDataTables.ContainsKey(ApplicationArguments.ActiveStep))
                {
                    throw new Exception("UIAutomation Data Table not found for active step.");
                }
                var table = ApplicationArguments.IUIAutomationDataTables[ApplicationArguments.ActiveStep];
                if (table == null || table.Rows.Count == 0)
                {
                    throw new Exception("Data Table is empty for the active step.");
                }
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string stepName = table.Rows[i]["Steps"].ToString().Trim();
                    if (string.IsNullOrEmpty(stepName))
                    {
                        continue;
                    }
                    try
                    {
                        switch (stepName)
                        {
                            
                            case "CreateDictionaryFromDataTable":
                                if (ApplicationArguments.IUIAutomationMappingTables.ContainsKey(table.Rows[i]["WindowAutomationID"].ToString()))
                                {
                                    ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[table.Rows[i]["WindowAutomationID"].ToString()]);
                                }
                                else
                                {
                                    Console.WriteLine("Mapping table for "+table.Rows[i]["WindowAutomationID"].ToString() +" not found.");
                                }
                                break;

                            case "CheckOptionalNirvanaDialogueBoxHandlerPopup":

                                try
                                {
                                    ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[table.Rows[i]["WindowAutomationID"].ToString()]);
                                    string PromptYesOrNo = table.Rows[i]["DataName"].ToString();
                                    string allowHardCodedDefaultPrompt = table.Rows[i]["Arg1"].ToString();
                                    uiAutomationHelper.CheckOptionalNirvanaDialogueBoxHandlerPopup(PromptYesOrNo, allowHardCodedDefaultPrompt);
                                }
                                catch { }
                                break;


                            case "CheckRequiredColumnsOnTestData":
                                try
                                {
                                    string requiredColsRaw = table.Rows[i]["DataName"].ToString();
                                    if (!string.IsNullOrEmpty(requiredColsRaw))
                                    {
                                        string[] requiredColumns = requiredColsRaw.Split(',');
                                       
                                        foreach (string col in requiredColumns)
                                        {
                                            string colTrimmed = col.Trim();
                                            if (!ExcelData.Columns.Contains(colTrimmed))
                                            {
                                                return _res;  
                                            }
                                        }
                                    }

                                }
                                catch { }
                                break;
                           

                            case "RemoveColumnsFromDataSheet":
                                ExcelData = DataUtilities.RemoveColumnsAndRows(table.Rows[i]["Action"].ToString(), ExcelData);
                                break;

                            case "SetActiveGrid":
                                object gridIdValue = table.Rows[i]["GridID"];

                                if (gridIdValue != DBNull.Value)
                                {
                                    string gridId = gridIdValue.ToString().Trim();

                                    if (!string.IsNullOrEmpty(gridId))
                                    {
                                        ActiveGrid = gridId;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Warning: GridID is empty at row " + i);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Warning: GridID is null (DBNull) at row " + i);
                                }
                                break;
                            
                            case "RightClickAndSelectMenuonActiveGrid":

                                if (!string.IsNullOrEmpty(ActiveGrid))
                                {
                                    bool success = gridDataProvider.RightClickAndSelectMenuonActiveGrid(ActiveGrid, ExcelData);
                                    if (!success)
                                    {
                                        string errorMessage = ApplicationArguments.TestCaseToBeRun + " failed at step - " + stepName + " at RightClickAndSelectMenuonActiveGrid operation ";
                                        throw new Exception(errorMessage);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Warning: GridID is empty at row " + i);
                                }

                                break;
                            
                            case "RemoveColumnsFromDataSetForAction":
                                string tempAction =table.Rows[i]["Action"].ToString();
                                DataTable tableRef = testData.Tables[0];
                                DataUtilities.RemoveColumnsAndRows(ref tempAction, ref tableRef);
                                testData.Tables.RemoveAt(0);
                                testData.Tables.Add(tableRef);
                                break;
                            
                            case "RemoveColumnsFromDataSheetUI":
                                uiData = DataUtilities.RemoveColumnsAndRows(table.Rows[i]["Action"].ToString(), uiData);
                                break;

                            case "RunDateChange":

                                string actionRunDateChange = "";
                                if (table.Rows[i]["Action"] != null && !string.IsNullOrWhiteSpace(table.Rows[i]["Action"].ToString()))
                                {
                                    actionRunDateChange = table.Rows[i]["Action"].ToString();
                                }
                                else
                                {
                                    break;
                                }

                                if (string.Equals(actionRunDateChange, "UIData", StringComparison.OrdinalIgnoreCase))
                                {
                                    uiData = DateDataAdjuster.AdjustDateData(uiData);
                                }
                                else if (string.Equals(actionRunDateChange, "ExcelData", StringComparison.OrdinalIgnoreCase))
                                {
                                    ExcelData = DateDataAdjuster.AdjustDateData(ExcelData);
                                }
                                break;
                            
                            case "VerifyWithRecon":
                                try
                                {
                                    if (ExcelData == null)
                                    {
                                        ExcelData = testData.Tables[0];
                                    }
                                    List<String> columns = new List<string>();
                                    if (uiData == null)
                                    {
                                        throw new Exception("UI Data is null");
                                    }
                                    uiData = DataUtilities.RemoveCommas(uiData);
                                    uiData = DataUtilities.RemovePercent(uiData);
                                    uiData = DataUtilities.RemoveTrailingZeroes(uiData);
                                    ExcelData = DataUtilities.RemoveCommas(ExcelData);
                                    ExcelData = DataUtilities.RemovePercent(ExcelData);
                                    ExcelData = DataUtilities.RemoveTrailingZeroes(ExcelData);
                                    List<String> errors = Recon.RunRecon(uiData, ExcelData, columns, 0.01);
                                    if (errors.Count > 0)
                                    {
                                        if (errors.Count > 0)
                                        {
                                            foreach (string error in errors)
                                            {
                                                Console.WriteLine(error);
                                            }
                                            string errorMessage = ApplicationArguments.TestCaseToBeRun + " failed at step - " + stepName + " Errors:\n";
                                            foreach (string error in errors)
                                            {
                                                Console.WriteLine(error);
                                                errorMessage += error + "\n";
                                            }
                                            throw new Exception(errorMessage);
                                        }

                                    }
                                }
                                catch 
                                {
                                        throw;
                                }

                                break;
                            case "GetUIGridData":
                                try
                                {
                                    try
                                    {
                                        IUIAutomation automation = new CUIAutomation();
                                        IUIAutomationElement rootElement = automation.GetRootElement();
                                        ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[table.Rows[i]["WindowAutomationID"].ToString()]);

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
                                                string moduleID = table.Rows[i]["WindowAutomationID"].ToString();
                                                string gridID = table.Rows[i]["GridID"].ToString();
                                                string DataExtractionScriptName = table.Rows[i]["Arg1"].ToString();
                                                string tableName = table.Rows[i]["Arg2"].ToString();
                                                string framework = !string.Equals(table.Rows[i]["Arg3"].ToString(), "WPF", StringComparison.OrdinalIgnoreCase) ? "WinForm" : "WPF";
                                                if (GridFocusDataDictionary == null)
                                                {
                                                    GridFocusDataDictionary = processor.ProcessFocusedGridData(ApplicationArguments.IUIAutomationMappingTables["GetUIGridData"]);
                                                }
                                                DataSet dS = uiAutomationHelper.GetTargetedGridData(GridFocusDataDictionary,moduleID, gridID, DataExtractionScriptName, framework);
                                                if (dS != null && dS.Tables.Count > 0)
                                                {
                                                    if (string.IsNullOrEmpty(tableName))
                                                    {
                                                        uiData = dS.Tables[0];
                                                    }
                                                    else if (dS.Tables.Contains(tableName))
                                                    {
                                                        uiData = dS.Tables[tableName];
                                                    }
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
                                    Console.WriteLine("Exception caught in outer catch block for GetWPFGridData: " + ex.Message);
                                    Console.WriteLine("Stack Trace: " + ex.StackTrace);
                                    throw;
                                }

                                break;

                            case "EditGridData":
                                try
                                {
                                    try
                                    {
                                        IUIAutomation automation = new CUIAutomation();
                                        IUIAutomationElement rootElement = automation.GetRootElement();
                                        ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[table.Rows[i]["WindowAutomationID"].ToString()]);

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
                                                string moduleID = table.Rows[i]["WindowAutomationID"].ToString();
                                                string gridID = table.Rows[i]["GridID"].ToString();
                                                string DataExtractionScriptName = table.Rows[i]["Arg1"].ToString();
                                                string tableName = table.Rows[i]["Arg2"].ToString();
                                                string framework = !string.Equals(table.Rows[i]["Arg3"].ToString(), "WPF", StringComparison.OrdinalIgnoreCase) ? "WinForm" : "WPF";

                                                if (GridEditFocusDataDictionary == null)
                                                {
                                                    GridEditFocusDataDictionary = processor.ProcessFocusedGridData(ApplicationArguments.IUIAutomationMappingTables["GetUIEditGridData"]);
                                                }

                                                bool successEditGridDataState = uiAutomationHelper.EditGridData(GridEditFocusDataDictionary, moduleID, gridID, DataExtractionScriptName, framework, ExcelData);

                                                if (!successEditGridDataState)
                                                {
                                                   throw new Exception("EditGridData operation Failed");
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
                                    Console.WriteLine("Exception caught in outer catch block for GetWPFGridData: " + ex.Message);
                                    Console.WriteLine("Stack Trace: " + ex.StackTrace);
                                    throw;
                                }
                                break;
                            case "SelectWinFormGridData":
                                try
                                {
                                    try
                                    {
                                        IUIAutomation automation = new CUIAutomation();
                                        IUIAutomationElement rootElement = automation.GetRootElement();
                                        ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[table.Rows[i]["WindowAutomationID"].ToString()]);

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
                                                string moduleID = table.Rows[i]["WindowAutomationID"].ToString();
                                                string gridID = table.Rows[i]["GridID"].ToString();
                                                
                                                if (GridFocusDataDictionary == null)
                                                {
                                                    GridFocusDataDictionary = processor.ProcessFocusedGridData(ApplicationArguments.IUIAutomationMappingTables["GetUIGridData"]);
                                                }

                                                if (ExcelData != null || uiData != null)
                                                {
                                                    //assuming grid data already extracted
                                                    bool selectionSuccess = GridDataProvider.SelectWinFormGridData(ExcelData, uiData, moduleID, gridID, automation, windowElement);

                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("Error retrieving SelectWinFormGridData: " + ex.Message);
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
                                    Console.WriteLine("Exception caught in outer catch block for GetWPFGridData: " + ex.Message);
                                    Console.WriteLine("Stack Trace: " + ex.StackTrace);
                                    throw;
                                }

                                break;
                            case "GetWPFGridData":
                                try
                                {
                                    try
                                    {
                                        IUIAutomation automation = new CUIAutomation();
                                        IUIAutomationElement rootElement = automation.GetRootElement();
                                        ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[table.Rows[i]["WindowAutomationID"].ToString()]);

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
                                                string moduleID = table.Rows[i]["WindowAutomationID"].ToString() ;
                                                string gridID = table.Rows[i]["GridID"].ToString();
                                                string action = "OLD";
                                                if (!string.IsNullOrEmpty(table.Rows[i]["Action"].ToString()))
                                                {
                                                    uiData = gridDataProvider.GetWPFGridDataNew(moduleID, gridID,false);
                                                }
                                                else
                                                uiData = gridDataProvider.GetWPFGridData(moduleID, gridID);
                                                
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("Error retrieving WPF Grid Data: " + ex.Message);
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
                                    Console.WriteLine("Exception caught in outer catch block for GetWPFGridData: " + ex.Message);
                                    Console.WriteLine("Stack Trace: " + ex.StackTrace);
                                    throw;
                                }

                                break;
                            case "GetWinformGridData":
                                try
                                {
                                    try
                                    {
                                        IUIAutomation automation = new CUIAutomation();
                                        IUIAutomationElement rootElement = automation.GetRootElement();
                                        ApplicationArguments.mapdictionary = DataUtilities.CreateDictionaryFromDataTable(ApplicationArguments.IUIAutomationMappingTables[table.Rows[i]["WindowAutomationID"].ToString()]);

                                        IUIAutomationElement windowElement = null;
                                        int retryCount = 2;
                                        bool success = false;

                                        while (retryCount > 0 && !success)
                                        {
                                            try
                                            {
                                                windowElement = UIAutomationHelper.FindElementByUniquePropertyType(automation, rootElement, ApplicationArguments.mapdictionary["ModuleWindow"].UniquePropertyType, ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
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
                                            }
                                        }

                                        if (windowElement != null)
                                        {
                                            try
                                            {
                                                string moduleID = table.Rows[i]["WindowAutomationID"].ToString();
                                                string gridID = table.Rows[i]["GridID"].ToString();
                                                DataSet dataSet = GridDataProvider.GetWinformGridData(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                                                uiData = dataSet.Tables[0];

                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("Error retrieving Winform Grid Data: " + ex.Message);
                                                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error in GetWinformGridData: " + ex.Message);
                                        Console.WriteLine("Stack Trace: " + ex.StackTrace);
                                        throw;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Exception caught in outer catch block for GetWinformGridData: " + ex.Message);
                                    Console.WriteLine("Stack Trace: " + ex.StackTrace);
                                    throw;
                                }

                                break;
                            case "DetectAndSwitchWindow":
                                if (ApplicationArguments.mapdictionary != null)
                                {
                                    UIAutomationHelper.DetectAndSwitchWindow(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                                }
                                else
                                {
                                    Console.WriteLine("ModuleWindow not available in mapdictionary.");
                                }
                                break;

                            case "CommonAction":
                                if (ApplicationArguments.mapdictionary != null)
                                {
                                    try
                                    {
                                        uiAutomationHelper.CommonAction(testData.Tables[0], table.Rows[i]["ShortCutString"].ToString(), ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                                    }
                                    catch { }
                                }
                                else
                                {
                                    Console.WriteLine("Requirements not completed for CommonAction");
                                }
                                break;
                            case "SaveSimulatorData":
                                bool delete = true;
                                for (int k = testData.Tables[0].Rows.Count - 1; k >= 0; k--)
                                {
                                    if (!string.IsNullOrEmpty(testData.Tables[0].Rows[k]["btnSend"].ToString()))
                                    {
                                        delete = true;
                                    }
                                    if (!string.IsNullOrEmpty(testData.Tables[0].Rows[k]["btnCreateOrder"].ToString()) || !string.IsNullOrEmpty(testData.Tables[0].Rows[k]["btnDoneAway"].ToString()) || !string.IsNullOrEmpty(testData.Tables[0].Rows[k]["btnReplace"].ToString()))
                                    {
                                        delete = false;
                                    }
                                    if (!delete)
                                        testData.Tables[0].Rows[k].Delete();
                                    
                                }
                                testData.Tables[0].AcceptChanges();
                                List<DataRow> rowsToDelete = new List<DataRow>();
                                int lastSendIndex = -1;

                                for (int k = 0; k < testData.Tables[0].Rows.Count; k++)
                                {
                                    DataRow current = testData.Tables[0].Rows[k];
                                    if (!string.IsNullOrWhiteSpace(current["btnSend"].ToString()))
                                    {
                                        // Merge from lastSendIndex+1 to i-1 into current row
                                        for (int j = k - 1; j > lastSendIndex; j--)
                                        {
                                            DataRow above = testData.Tables[0].Rows[j];

                                            foreach (DataColumn col in testData.Tables[0].Columns)
                                            {
                                                string colName = col.ColumnName;

                                                // Skip btnSend, we don't want to overwrite it
                                                if (colName == "btnSend")
                                                    continue;

                                                string currentValue = current[colName].ToString();
                                                string aboveValue = above[colName].ToString();

                                                if (!string.IsNullOrWhiteSpace(aboveValue))
                                                {
                                                    if (string.IsNullOrWhiteSpace(currentValue))
                                                    {
                                                        current[colName] = aboveValue;
                                                    }
                                                    else if (!currentValue.Contains(aboveValue))
                                                    {
                                                        // Append values (space-separated)
                                                        current[colName] = currentValue + " " + aboveValue;
                                                    }
                                                }
                                            }

                                            // Mark for deletion
                                            if (!rowsToDelete.Contains(above))
                                            {
                                                rowsToDelete.Add(above);
                                            }
                                        }

                                        lastSendIndex = k;
                                    }
                                }

                                // Delete merged rows
                                foreach (DataRow row in rowsToDelete)
                                {
                                    testData.Tables[0].Rows.Remove(row);
                                }

                                testData.Tables[0].AcceptChanges();
                                if (testData.Tables[0].Rows.Count > 0)
                                {
                                    SaveData(testData);
                                }
                                break;
                            case "PerformActionWithRetry":
                                if (ApplicationArguments.mapdictionary != null)
                                {
                                    try
                                    {
                                        string shortCutString = table.Rows[i]["ShortCutString"].ToString();
                                        string moduleWindow = table.Rows[i]["WindowAutomationID"].ToString();
                                        Keyboard.SendKeys(ConfigurationManager.AppSettings[shortCutString]);
                                        UIAutomationHelper.PerformActionWithRetry(moduleWindow, shortCutString);
                                        Keyboard.SendKeys(ConfigurationManager.AppSettings[shortCutString]);
                                    }
                                    catch { }
                                }
                                else
                                {
                                    Console.WriteLine("Requirements not completed for CommonAction");
                                }
                                break;
                            case "CheckUIAutomationCommonActionResult":
                                if (!string.IsNullOrEmpty(ApplicationArguments.UIAutomationCommonActionResult))
                                {
                                    string error = ApplicationArguments.UIAutomationCommonActionResult;
                                    ApplicationArguments.UIAutomationCommonActionResult = "";
                                    throw new Exception(error);
                                }
                                break;
                            case "MaximizeWindow":
                                uiAutomationHelper.Maximize(ApplicationArguments.mapdictionary["ModuleWindow"].AutomationUniqueValue);
                                break;
                            case "FindAndClickElement":
                                if (ApplicationArguments.mapdictionary != null &&
                                table.Rows[i]["DataName"] != DBNull.Value)
                                {
                                    string dataName = table.Rows[i]["DataName"].ToString();
                                    if (ApplicationArguments.mapdictionary.ContainsKey(dataName))
                                    {
                                        var element = ApplicationArguments.mapdictionary[dataName];
                                        string action = table.Rows[i]["Action"] != DBNull.Value ? table.Rows[i]["Action"].ToString() : string.Empty;
                                        string automationValue = element.AutomationUniqueValue;
                                        bool usecursorclick = false;
                                        if (table.Columns.Contains("Arg1"))
                                        {
                                            var arg1Value = table.Rows[i]["Arg1"].ToString();
                                            if (!string.IsNullOrEmpty(arg1Value) && string.Equals(arg1Value, "true", StringComparison.OrdinalIgnoreCase))
                                            {
                                                usecursorclick = true;
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(action) && action.Equals("Right", StringComparison.OrdinalIgnoreCase))
                                            uiAutomationHelper.FindAndClickElement(automationValue, "Right");
                                        else
                                            uiAutomationHelper.FindAndClickElement(automationValue,"Left",usecursorclick);
                                    }
                                }

                                break;

                            case "FindAndClickElementPartiallyLeft":
                                if (ApplicationArguments.mapdictionary != null &&
                                table.Rows[i]["DataName"] != DBNull.Value)
                                {
                                    string dataName = table.Rows[i]["DataName"].ToString();
                                    if (ApplicationArguments.mapdictionary.ContainsKey(dataName))
                                    {
                                        var element = ApplicationArguments.mapdictionary[dataName];
                                        string action = table.Rows[i]["Action"] != DBNull.Value ? table.Rows[i]["Action"].ToString() : string.Empty;
                                        string automationValue = element.AutomationUniqueValue;
                                        
                                        if (!string.IsNullOrEmpty(action) && action.Equals("Right", StringComparison.OrdinalIgnoreCase))
                                            uiAutomationHelper.FindAndClickElementPartiallyLeft(automationValue, "Right");
                                        else
                                            uiAutomationHelper.FindAndClickElementPartiallyLeft(automationValue, "Left");
                                    }
                                }

                                break;
                            case "OpenSmGrid":
                                uiAutomationHelper.OpenSmGrid(targetElement);
                                break;
                            case "CreateIUIElement":
                                targetElement = uiAutomationHelper.CreateUIElement(table.Rows[i]["DataName"].ToString());
                                break;
                            case "FindAndDoubleClickElement":
                                if (ApplicationArguments.mapdictionary != null &&
                                table.Rows[i]["DataName"] != DBNull.Value)
                                {
                                    string dataName = table.Rows[i]["DataName"].ToString();
                                    if (ApplicationArguments.mapdictionary.ContainsKey(dataName))
                                    {
                                        var element = ApplicationArguments.mapdictionary[dataName];
                                        string action = table.Rows[i]["Action"] != DBNull.Value ? table.Rows[i]["Action"].ToString() : string.Empty;
                                        string automationValue = element.AutomationUniqueValue;

                                        uiAutomationHelper.FindAndDoubleClickElement(targetElement, automationValue);
                                    }
                                }
                                break;
                            case "WaitForElement":
                                {
                                    string dataName = table.Rows[i]["DataName"].ToString();

                                    if (ApplicationArguments.mapdictionary != null && ApplicationArguments.mapdictionary.ContainsKey(dataName))
                                    {
                                        var elementInfo = ApplicationArguments.mapdictionary[dataName];
                                        string automationValue = elementInfo.AutomationUniqueValue;

                                        int timeoutInSeconds = 30;
                                        if (table.Columns.Contains("Time(In Seconds)") && table.Rows[i]["Time(In Seconds)"] != DBNull.Value)
                                        {
                                            int parsedTimeout;
                                            if (int.TryParse(table.Rows[i]["Time(In Seconds)"].ToString(), out parsedTimeout))
                                            {
                                                timeoutInSeconds = parsedTimeout;
                                            }
                                        }

                                        Console.WriteLine("Waiting for element '" + dataName + "' with AutomationId '" + automationValue + "' for up to " + timeoutInSeconds + " seconds.");

                                        bool isFound = uiAutomationHelper.WaitForElement(automationValue, timeoutInSeconds);

                                        if (!isFound)
                                        {
                                            throw new Exception("Element '" + dataName + "' with AutomationId '" + automationValue + "' not found after waiting " + timeoutInSeconds + " seconds.");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("DataName '" + dataName + "' not found in mapdictionary for WaitForElement.");
                                    }
                                }
                                break;
                            
                            default:
                                {
                                    try
                                    {
                                        TestResult stepResult = new TestResult();

                                        var secStepWrapper = TestStepFactory.GetMethodDynamically(
                                            ApplicationArguments.ApplicationStartUpPath,
                                            ApplicationArguments.NirvanaUIAutomationModuleName,
                                            stepName);

                                        if (secStepWrapper == null)
                                        {
                                            Console.WriteLine("secStepWrapper is null for step: " + stepName);
                                            throw new Exception("DataStep Failed at " + stepName );
                                        }
                                        if (secStepWrapper.CanRunUIAutomationTest)
                                        {
                                            try
                                            {
                                                ParameterInfo[] methodParams = secStepWrapper.RunUIAutomationTestMethod.GetParameters();
                                                List<object> methodArgs = new List<object>();
                                                for (int paramIndex = 0; paramIndex < methodParams.Length; paramIndex++)
                                                {
                                                    string argColumnName = "Arg" + (paramIndex + 1);
                                                    string argTypeColumnName = "Arg" + (paramIndex + 1) + "Type";
                                                    if (table.Columns.Contains(argColumnName))
                                                    {
                                                        var value = table.Rows[i][argColumnName];
                                                        var type = table.Rows[i][argColumnName + "Type"];
                                                        if (string.Equals(value.ToString(), "uiData", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            methodArgs.Add(uiData);
                                                        }
                                                        else if (string.Equals(value.ToString(), "excelData", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            methodArgs.Add(ExcelData);
                                                        }
                                                        else if (string.Equals(type.ToString(), "Object", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            methodArgs.Add(value != DBNull.Value ? value : string.Empty);
                                                        }
                                                        else if (string.Equals(type.ToString(), "int", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            int intValue = 0;
                                                            if (int.TryParse(value.ToString(), out intValue))
                                                            {
                                                                methodArgs.Add(intValue);
                                                            }
                                                            else
                                                            {
                                                                methodArgs.Add(0);
                                                            }

                                                        }
                                                        else if (string.Equals(type.ToString(), "bool", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            bool boolValue = false;
                                                            if (bool.TryParse(value.ToString(), out boolValue))
                                                            {
                                                                methodArgs.Add(boolValue);
                                                            }
                                                            else
                                                            {
                                                                methodArgs.Add(false);
                                                            }
                                                        }
                                                        else
                                                            methodArgs.Add(value != DBNull.Value ? value.ToString() : string.Empty);
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Not enough Args present in Data at " + stepName);
                                                       
                                                    }
                                                }
                                                var returnType = secStepWrapper.RunUIAutomationTestMethod.ReturnType;

                                                if (returnType == typeof(void))
                                                {
                                                    try
                                                    {
                                                        // Invoke the method without expecting a return value
                                                        secStepWrapper.RunUIAutomationTestMethod.Invoke(
                                                            secStepWrapper.Instance,
                                                            methodArgs.ToArray()
                                                        );
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        throw new Exception("DataStep Failed at " + stepName + ", Reason: " + ex.Message, ex);
                                                    }
                                                }
                                                else
                                                {
                                                    stepResult = (TestResult)secStepWrapper.RunUIAutomationTestMethod.Invoke(
                                                        secStepWrapper.Instance,
                                                        methodArgs.ToArray()
                                                    );

                                                    if (!stepResult.IsPassed)
                                                    {
                                                        throw new Exception("DataStep Failed at " + stepName + ", Reason: " + stepResult.ErrorMessage);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                            
                                        }
                                       


                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Exception thrown at TestStepFactory: GetDataModeStepWrapper - " + ex.Message);
                                        throw;
                                    }
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        string combinedMessage = "Exception Message: " + ex.Message + Environment.NewLine +
                                                 "Stack Trace: " + ex.StackTrace;

                        Console.WriteLine(combinedMessage);

                        throw new Exception(combinedMessage, ex);
                    }

                }
            }

            catch (Exception ex)
            {
                _res.IsPassed = false;

                string errorDetails = "Exception Message: " + ex.Message + Environment.NewLine +
                                       "Stack Trace: " + ex.StackTrace + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    errorDetails += "Inner Exception: " + ex.InnerException.Message + Environment.NewLine +
                                    "Inner Stack Trace: " + ex.InnerException.StackTrace + Environment.NewLine;
                }
                Console.WriteLine(errorDetails);
                _res.ErrorMessage =errorDetails;
                throw;

            }

            return _res;
        }
        
        public void IUIAutomationFileLoading()
        {
            try
            {
                if (ApplicationArguments.IUIAutomationDataTables == null)
                {
                    ApplicationArguments.IUIAutomationDataTables = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["IUIAutomationDataFile"]);
                }
                if (ApplicationArguments.IUIAutomationMappingTables == null)
                {
                    ApplicationArguments.IUIAutomationMappingTables = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["IUIAutomationMappingFile"]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void SaveData(DataSet LiveData)
        {
            try
            {
                LiveData.Tables[0].TableName = "CustomOrderTT";
                if (!Directory.Exists("SimulatorData"))
                    Directory.CreateDirectory("SimulatorData");
                if (File.Exists(@"SimulatorData/LiveTrades.xml"))
                {
                    try
                    {
                        XmlDocument xml1 = new XmlDocument();
                        XmlDocument xml2 = new XmlDocument();
                        xml1.Load(@"SimulatorData/LiveTrades.xml");

                        if (xml1.SelectSingleNode("NewDataSet/CreateNewSubLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateReplaceLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/SendOrderUsingMTT") != null || xml1.SelectSingleNode("NewDataSet/CustomOrderTT") != null)
                        {
                            xml2.LoadXml(LiveData.GetXml());
                            foreach (XmlNode list in xml2.SelectSingleNode("NewDataSet").ChildNodes)
                                xml1.SelectSingleNode("NewDataSet").AppendChild(xml1.ImportNode(list, true));
                            xml1.Save(@"SimulatorData/LiveTrades.xml");
                        }
                        else
                        {
                            LiveData.WriteXml(@"SimulatorData/LiveTrades.xml");
                        }
                    }
                    catch { LiveData.WriteXml(@"SimulatorData/LiveTrades.xml"); }
                }
                else
                    LiveData.WriteXml(@"SimulatorData/LiveTrades.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ValidateAndGetActiveStepDataTable(out DataTable table)
        {
            table = null;
            try
            {
                try
                { IUIAutomationFileLoading(); }
                catch { }
                if (ApplicationArguments.IUIAutomationDataTables == null)
                    return false;

                if (!ApplicationArguments.IUIAutomationDataTables.ContainsKey(ApplicationArguments.ActiveStep))
                    return false;

                table = ApplicationArguments.IUIAutomationDataTables[ApplicationArguments.ActiveStep];

                if (table == null || table.Rows.Count == 0)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                string errorDetails = "Exception in ValidateAndGetActiveStepDataTable: " + ex.Message + Environment.NewLine +
                                       "Stack Trace: " + ex.StackTrace + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    errorDetails += "Inner Exception: " + ex.InnerException.Message + Environment.NewLine +
                                    "Inner Stack Trace: " + ex.InnerException.StackTrace + Environment.NewLine;
                }
                Console.WriteLine(errorDetails);
                return false;
            }
        }

    }
}
