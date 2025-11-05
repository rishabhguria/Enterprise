using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.Core;
using System.IO;
using Nirvana.TestAutomation.Interfaces.Enums;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Configuration;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.UIAutomation;
using UtilitiesTCFM = Nirvana.TestAutomation.Utilities.TestCaseFixingMode;

namespace Nirvana.TestAutomation.TestExecutor
{
    class TestCaseFixingMode
    {
        //create a method of TestCaseFixingMode(filepath,newdataset,testdataset,TestCaseID)
        private static int dataStartingRow = 5;

        public static void FixTestCase(string fileBasePath, DataSet newDataSet, DataSet testDataSet, string testCaseID)
        {
            try
            {
                try
                {
                    if (newDataSet == null || testDataSet == null || string.IsNullOrEmpty(testCaseID))
                    {
                        Console.WriteLine("Invalid input for FixTestCase.");
                        return;
                    }
                    if (ApplicationArguments.TestCaseFixingDic == null)
                    {
                        ApplicationArguments.TestCaseFixingDic =
                            new Dictionary<string, UtilitiesTCFM>();
                    }

                    if (!ApplicationArguments.TestCaseFixingDic.ContainsKey(testCaseID))
                    {
                        ApplicationArguments.TestCaseFixingDic[testCaseID] = new UtilitiesTCFM();
                    }

                    Console.WriteLine("FixTestCase Process Started for TestCase: " + testCaseID);

                    string newApproach = "DefaultApproach";
                    ApplicationArguments.ActiveApproach = "DefaultApproach"; 
                    if (ConfigurationManager.AppSettings["UseCustomApproach"].ToLower() == "true")
                    {
                        newApproach = "CustomApproach";
                        ApplicationArguments.ActiveApproach = "CustomApproach"; 
                        ApplicationArguments.TestCaseFixingDic[testCaseID].Approach = newApproach;

                        fileBasePath = fileBasePath.Replace("DefaultApproach", newApproach);
                        try
                        {
                            IFixingApproach approach = ModeManager.GetApproach();
                            if (approach != null)
                            {
                                approach.Execute(
                                    testDataSet,
                                    newDataSet,
                                    ApplicationArguments.TestCaseFixingDic[testCaseID].StepxDetails,
                                    ApplicationArguments.TestCaseFixingDic[testCaseID].StepxUpdatedColumns,
                                    testCaseID
                                );
                            }
                            else
                            {
                                throw new Exception("Error in ModeManager.GetApproach is null");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error in ModeManager.GetApproach: " + ex.Message);
                            throw;
                        }

                    }
                    else
                    {
                        //need to update code for concrete class
                        ApplicationArguments.TestCaseFixingDic[testCaseID].Approach = newApproach;
                        SyncCreatedDataSetWithOriginal(
                                                            testDataSet,                     // originalTestCaseDataSet
                                                            newDataSet,               
                                                            ApplicationArguments.TestCaseFixingDic[testCaseID].StepxDetails,
                                                            ApplicationArguments.TestCaseFixingDic[testCaseID].StepxUpdatedColumns,
                                                            testCaseID
                                                        );
                    }

                    if (!Directory.Exists(fileBasePath))
                    {
                        Directory.CreateDirectory(fileBasePath);
                    }
                    TestCaseCreation testcaseCreation = new TestCaseCreation();
                   
                    testcaseCreation.GenerateXlsxTestCaseSheetSyncDateTimeManager(newDataSet, fileBasePath.ToString(), testCaseID, dataStartingRow, true);

                    Console.WriteLine("Test case fixed and saved successfully.");

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while fixing test case: " + ex.Message);
                    throw;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fixing test case: " + ex.Message);
                throw;
            }
        }


        public static void SyncCreatedDataSetWithOriginal(
    DataSet originalTestCaseDataSet,
    DataSet createdDataSet,
    Dictionary<string, List<string>> stepxDetails,
    Dictionary<string, List<string>> stepxUpdatedColumns,
            string testCaseID
            )
        {
            try
            {
                foreach (DataTable originalTable in originalTestCaseDataSet.Tables)
                {
                    TestCaseFixingRow testcasefixingrow = new TestCaseFixingRow();

                    string tableName = originalTable.TableName;

                    if (!createdDataSet.Tables.Contains(tableName))
                        continue;

                    DataTable createdTable = createdDataSet.Tables[tableName];
                    if (DataUtilities.RowsMatchInAnyOrder(originalTable, createdTable))
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Mismatch found between original and created table: " + tableName);
                        testcasefixingrow.Details.Add("Mismatch found between original and created table: " + tableName);

                        if (!stepxDetails.ContainsKey(tableName))
                        {
                            stepxDetails[tableName] = new List<string>();
                        }

                        if (!stepxUpdatedColumns.ContainsKey(tableName))
                        {
                            stepxUpdatedColumns[tableName] = new List<string>();
                        }

                        stepxDetails[tableName].Add("Mismatch found between original and created table: " + tableName); 
                    }

                    foreach (DataColumn col in originalTable.Columns)
                    {
                        if (!createdTable.Columns.Contains(col.ColumnName))
                        {
                            createdTable.Columns.Add(col.ColumnName, typeof(string));
                            Console.WriteLine("Added column '" + col.ColumnName + "' to table '" + tableName + "'");
                              testcasefixingrow.Details.Add("Added column '" + col.ColumnName + "' to table '" + tableName + "'");
                            if (!stepxDetails.ContainsKey(tableName))
                            {
                                stepxDetails[tableName] = new List<string>();
                            }

                            if (!stepxUpdatedColumns.ContainsKey(tableName))
                            {
                                stepxUpdatedColumns[tableName] = new List<string>();
                            }

                            stepxDetails[tableName].Add("Added column '" + col.ColumnName + "' to table '" + tableName + "'"); // NEW
                            stepxUpdatedColumns[tableName].Add(col.ColumnName); 
                        }
                    }

                    HashSet<string> nonEmptyColumns = new HashSet<string>();
                    for (int i = 0; i < originalTable.Rows.Count; i++)
                    {
                        DataRow originalRow = originalTable.Rows[i];
                        for (int j = 0; j < originalTable.Columns.Count; j++)
                        {
                            DataColumn col = originalTable.Columns[j];
                            object valObj = originalRow[col.ColumnName];
                            string val = valObj != null ? valObj.ToString() : "";
                            if (!string.IsNullOrEmpty(val))
                            {
                                if (!nonEmptyColumns.Contains(col.ColumnName))
                                {
                                    nonEmptyColumns.Add(col.ColumnName);
                                }
                            }
                        }
                    }

                    Console.WriteLine("Non-empty columns in original table '" + tableName + "':");
                    foreach (string colName in nonEmptyColumns)
                    {
                        Console.WriteLine("- " + colName);
                    }

                    for (int i = 0; i < createdTable.Rows.Count; i++)
                    {
                        DataRow createdRow = createdTable.Rows[i];
                        string expr = DataUtilities.RowExpressionIfNotExists(createdRow, originalTable);
                        if (expr != string.Empty)
                        {
                            if (!stepxDetails.ContainsKey(tableName))
                                stepxDetails[tableName] = new List<string>();

                            stepxDetails[tableName].Add(expr);
                        }
                        for (int j = 0; j < createdTable.Columns.Count; j++)
                        {
                            DataColumn col = createdTable.Columns[j];

                            if (!nonEmptyColumns.Contains(col.ColumnName))
                            {
                                object value = createdRow[col.ColumnName];
                                string valueStr = value != null ? value.ToString() : "";
                                Console.WriteLine("Prev column '" + col.ColumnName + "' value : " + valueStr);

                                createdRow[col.ColumnName] = "";

                                Console.WriteLine("Cleared column '" + col.ColumnName + "' in created row.");

                                testcasefixingrow.UpdatedColumns.Add(col.ColumnName);
                                testcasefixingrow.Details.Add("Cleared column '" + col.ColumnName + "' in created row.");
                            }
                        }
                    }

                    Console.WriteLine("Completed syncing table: " + tableName);
                   
                    if (testcasefixingrow.UpdatedColumns != null && testcasefixingrow.UpdatedColumns.Count > 0 &&
     testcasefixingrow.OlderValues != null && testcasefixingrow.OlderValues.Count > 0 &&
     testcasefixingrow.NewerValues != null && testcasefixingrow.NewerValues.Count > 0 &&
     testcasefixingrow.Details != null && testcasefixingrow.Details.Count > 0)
                    {
                        CoreModeHelper.GenerateFixingReport(testCaseID, testcasefixingrow);
                        testcasefixingrow.Reset();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

       
    }
}
