using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace Nirvana.TestAutomation.TestExecutor
{

    public class ColumnValueReplaceApproach : IFixingApproach
    {

        public void Execute(
            DataSet originalTestCaseDataSet,
            DataSet createdDataSet,
            Dictionary<string, List<string>> stepxDetails,
            Dictionary<string, List<string>> stepxUpdatedColumns,
            string testCaseID
        )
        {
            try
            {
                Console.WriteLine("ColumnValueReplaceApproach execution Started.");


                var table_ColumnValueReplace = ApplicationArguments.ITestCaseFixingTables[TestDataConstants.Sheet_ColumnValueReplace];
                if (table_ColumnValueReplace == null || table_ColumnValueReplace.Rows.Count == 0)
                {
                    throw new Exception("ColumnValueReplace not loaded successfully.");
                }

                var table_StepWiseMainColumns = ApplicationArguments.ITestCaseFixingTables[TestDataConstants.Sheet_StepWiseMainColumns];
                if (table_StepWiseMainColumns == null || table_StepWiseMainColumns.Rows.Count == 0)
                {
                    throw new Exception("Sheet_StepWiseMainColumns not loaded successfully.");
                }
                DataSet tempDS = new DataSet();

                var table_Sheet_StepWiseMainColumns = ApplicationArguments.ITestCaseFixingTables[TestDataConstants.Sheet_StepWiseMainColumns];
                if (table_Sheet_StepWiseMainColumns == null || table_Sheet_StepWiseMainColumns.Rows.Count == 0)
                {
                    throw new Exception("Sheet_StepWiseMainColumns not loaded successfully.");
                }

                CoreModeHelper.PopulateStepMainColumns(table_Sheet_StepWiseMainColumns);

                //for loop on originalTestCaseDataSet which is in argument
                //accesss each table with console wirteline each
                //for loop on table_ColumnValueReplace 
                // try to access Testcase ID	StepName	Input Sheet 1	Target Column	Condition	CurrentValue	ReplaceValue
                //column values..
                // 
                //loop closed

                //replace createdDataSet with tempDS
                foreach (DataTable originalTable in originalTestCaseDataSet.Tables)
                {


                    Console.WriteLine("Processing table: " + originalTable.TableName);

                    // Clone schema & data
                    DataTable tempTable = originalTable.Copy();

                    if (!createdDataSet.Tables.Contains(originalTable.TableName))
                    {
                        tempDS.Tables.Add(tempTable.Copy());
                        continue;
                    }

                    DataRow[] rules = table_ColumnValueReplace.Select(
                       "[Testcase ID] = '" + testCaseID + "'"
                   );

                    bool rowMatchInAnyOrderNeeded = true;

                    foreach (DataRow rule in rules)
                    {
                        string stepName = rule["StepName"].ToString().Trim();

                        List<string> inputSheet1List = rule["Input Sheet 1"]
                            .ToString()
                            .Trim()
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .ToList();

                        foreach (string name in inputSheet1List)
                        {
                            if (string.Equals(name, originalTable.TableName, StringComparison.OrdinalIgnoreCase))
                            {
                                rowMatchInAnyOrderNeeded = false;
                                break;
                            }
                        }
                        if (stepName.IndexOf(originalTable.TableName, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            rowMatchInAnyOrderNeeded = false;
                        }
                    }


                    if (rowMatchInAnyOrderNeeded && DataUtilities.RowsMatchInAnyOrder(
                                tempTable,
                                createdDataSet.Tables[tempTable.TableName]))
                    {
                        tempDS.Tables.Add(tempTable.Copy());
                        continue;
                    }



                    List<string> TargettedColumnToBeIgnored = new List<string>();
                    foreach (DataRow rule in rules)
                    {
                        string targetColumn = "";

                        if (rule["Target Column"] != null)
                        {
                            targetColumn = rule["Target Column"].ToString().Trim();
                        }

                        if (!string.IsNullOrEmpty(targetColumn))
                        {
                            TargettedColumnToBeIgnored.Add(targetColumn);
                        }
                    }
                    foreach (DataRow rule in rules)
                    {
                        string stepName = rule["StepName"].ToString().Trim();
                        List<string> inputSheet1List = rule["Input Sheet 1"]
                                                      .ToString()
                                                      .Trim()
                                                      .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(s => s.Trim())
                                                      .ToList();

                        string targetColumn = rule["Target Column"].ToString().Trim();
                        string condition = rule["Condition"].ToString().Trim();
                        string currentValue = rule["CurrentValue"].ToString().Trim();
                        string replaceValue = rule["ReplaceValue"].ToString().Trim();

                        if (string.IsNullOrEmpty(stepName) &&
            inputSheet1List.Count == 0 &&
            string.IsNullOrEmpty(targetColumn))
                        {
                            Console.WriteLine("Skipping rule because StepName, Input Sheet 1, and Target Column are all empty.");

                            if (!tempDS.Tables.Contains(tempTable.TableName))
                                tempDS.Tables.Add(tempTable.Copy());

                            continue;
                        }
                        if (!createdDataSet.Tables.Contains(tempTable.TableName))
                        {
                            throw new Exception("Table '" + tempTable.TableName + "' not found in createdDataSet.");
                        }



                        if (!string.IsNullOrEmpty(stepName) && tempTable.TableName.Contains(stepName))
                        {
                            UpdateTempTable(
                                tempTable.TableName,
                                inputSheet1List,
                                targetColumn,
                                condition,
                                currentValue,
                                replaceValue,
                                originalTestCaseDataSet,
                                createdDataSet,
                                stepxDetails,
                                stepxUpdatedColumns,
                                testCaseID,
                                tempTable,
                                TargettedColumnToBeIgnored
                            );
                        }
                        else if (inputSheet1List != null && inputSheet1List.Count > 0 && inputSheet1List.Contains(tempTable.TableName))
                        {
                            UpdateTempTable(
                                tempTable.TableName,
                                inputSheet1List,
                                targetColumn,
                                condition,
                                currentValue,
                                replaceValue,
                                originalTestCaseDataSet,
                                createdDataSet,
                                stepxDetails,
                                stepxUpdatedColumns,
                                testCaseID,
                                tempTable,
                                TargettedColumnToBeIgnored
                            );
                        }
                    }

                    // Add modified table to temp dataset
                    tempDS.Tables.Add(tempTable.Copy());
                }

                createdDataSet.Tables.Clear();

                foreach (DataTable table in tempDS.Tables)
                {
                    createdDataSet.Tables.Add(table.Copy());
                }


                Console.WriteLine("ColumnValueReplaceApproach execution completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ColumnValueReplaceApproach: " + ex.Message);
                throw new Exception("Error in ColumnValueReplaceApproach: " + ex.Message);
            }
        }

        private void UpdateTempTable(
      string stepName,
      List<string> inputSheet1List,
      string targetColumn,
      string condition,
      string currentValue,
      string replaceValue,
      DataSet originalTestCaseDataSet,
      DataSet createdDataSet,
      Dictionary<string, List<string>> stepxDetails,
      Dictionary<string, List<string>> stepxUpdatedColumns,
      string testCaseID,
      DataTable tempTable,
            List<string> TargettedColumnToBeIgnored)
        {
            string sheetName = stepName;
            try
            {
                Console.WriteLine("[INFO] Starting update for sheet: " + sheetName + ", TestCaseID: " + testCaseID);


                DataTable UItable = createdDataSet.Tables[sheetName];//uitable

                List<string> mainColumns = new List<string>();

                if (CoreModeHelper.StepMainColumns.ContainsKey(stepName))
                {
                    mainColumns = CoreModeHelper.StepMainColumns[stepName];
                }
                if (ApplicationArguments.TestCaseFixingRowDictionary == null)
                {
                    ApplicationArguments.TestCaseFixingRowDictionary = new Dictionary<string, TestCaseFixingRow>();
                }

                foreach (DataRow excelRow in tempTable.Rows)
                {
                    TestCaseFixingRow testcasefixingrow = new TestCaseFixingRow();
                    testcasefixingrow.TestCaseID = testCaseID;
                    testcasefixingrow.Step = sheetName;
                    testcasefixingrow.Approach = ApplicationArguments.ActiveApproach;
                    List<string> expressions = new List<string>();

                    foreach (DataColumn col in tempTable.Columns)
                    {
                        string colName = col.ColumnName;
                        string colValue = excelRow[col] != DBNull.Value ? excelRow[col].ToString() : string.Empty;
                        expressions.Add(colName + "=" + colValue);
                    }
                    testcasefixingrow.ExcelRow = string.Join(", ", expressions);



                    DataRow[] foundRows = null;

                    // 1️ Try matching using mainColumns first
                    for (int colIndex = 0; colIndex < mainColumns.Count; colIndex++)
                    {
                        string rawFilterExpression = "";
                        string formattedFilterExpression = "";

                        for (int i = 0; i <= colIndex; i++)
                        {
                            string colName = mainColumns[i];

                            if (!UItable.Columns.Contains(colName) || !tempTable.Columns.Contains(colName))
                            {
                                continue;
                            }

                            object valueObj = excelRow[colName];
                            string colValue = valueObj != null ? valueObj.ToString().Trim() : "";

                            if (string.IsNullOrEmpty(colValue))
                            {
                                Console.WriteLine("[SKIP] Column has empty or null value: " + colName);
                                continue;
                            }

                            decimal decimalTest;
                            bool isDecimal = decimal.TryParse(colValue, out decimalTest);

                            if (isDecimal && decimalTest.ToString().Contains(".") && decimalTest.ToString().Split('.')[1].Length > 2)
                            {
                                Console.WriteLine("[SKIP] Column has high-precision decimal (ignored): " + colName + " = " + colValue);
                                continue;
                            }

                            // RAW value (no formatting)
                            string safeValueRaw = colValue.Replace("'", "''");
                            if (rawFilterExpression.Length > 0) rawFilterExpression += " AND ";
                            rawFilterExpression += "[" + colName + "] = '" + safeValueRaw + "'";

                            // FORMATTED value (0.00 for decimals)
                            string safeValueFormatted = safeValueRaw;
                            if (isDecimal)
                            {
                                safeValueFormatted = decimalTest.ToString("0.00").Replace("'", "''");
                            }

                            if (formattedFilterExpression.Length > 0) formattedFilterExpression += " AND ";
                            formattedFilterExpression += "[" + colName + "] = '" + safeValueFormatted + "'";
                        }

                        foundRows = null;

                        // Try RAW filter first
                        if (!string.IsNullOrEmpty(rawFilterExpression))
                        {
                            foundRows = UItable.Select(rawFilterExpression);
                            if (foundRows.Length == 1)
                            {
                                Console.WriteLine("[MATCH] Found using main columns RAW filter: " + rawFilterExpression);
                                ApplyReplacement(foundRows[0], excelRow, targetColumn, condition, currentValue, replaceValue, testcasefixingrow);
                                break;
                            }
                        }

                        // Try FORMATTED filter if RAW failed
                        if ((foundRows == null || foundRows.Length != 1) && !string.IsNullOrEmpty(formattedFilterExpression))
                        {
                            foundRows = UItable.Select(formattedFilterExpression);
                            if (foundRows.Length == 1)
                            {
                                Console.WriteLine("[MATCH] Found using main columns FORMATTED filter: " + formattedFilterExpression);
                                ApplyReplacement(foundRows[0], excelRow, targetColumn, condition, currentValue, replaceValue, testcasefixingrow);
                                break;
                            }
                        }
                    }
                    //for (int colIndex = 0; colIndex < mainColumns.Count; colIndex++)
                    //{
                    //    string filterExpression = "";
                    //    for (int i = 0; i <= colIndex; i++)
                    //    {
                    //        string colName = mainColumns[i];
                    //        if (!UItable.Columns.Contains(colName) || !tempTable.Columns.Contains(colName))
                    //            continue;

                    //        string colValue = excelRow[colName] != null ? excelRow[colName].ToString().Replace("'", "''") : "";
                    //        if (filterExpression.Length > 0) filterExpression += " AND ";
                    //        filterExpression += "[" + colName + "] = '" + colValue + "'";
                    //    }

                    //    foundRows = UItable.Select(filterExpression);

                    //    if (foundRows.Length == 1)
                    //    {
                    //        Console.WriteLine("[MATCH] Found using main columns filter: " + filterExpression);
                    //        ApplyReplacement(foundRows[0], excelRow, targetColumn, condition, currentValue, replaceValue, testcasefixingrow);
                    //        break;
                    //    }
                    //}

                    // 2️ Fallback — try using ALL columns in excelRow
                    if (foundRows == null || foundRows.Length != 1)
                    {
                        List<string> allColumns = new List<string>();
                        foreach (DataColumn col in tempTable.Columns)
                        {
                            if (UItable.Columns.Contains(col.ColumnName))
                            {
                                allColumns.Add(col.ColumnName);
                            }
                        }

                        for (int colIndex = 0; colIndex < allColumns.Count; colIndex++)
                        {
                            string rawFilterExpression = "";
                            string formattedFilterExpression = "";

                            for (int i = 0; i <= colIndex; i++)
                            {
                                string colName = allColumns[i];
                                if (TargettedColumnToBeIgnored.Contains(colName))
                                {
                                    Console.WriteLine("[SKIP] Column ignored (TargettedColumnToBeIgnored): " + colName);
                                    continue;
                                }
                                object valueObj = excelRow[colName];
                                string colValue = valueObj != null ? valueObj.ToString().Trim() : "";

                                if (string.IsNullOrEmpty(colValue))
                                {
                                    Console.WriteLine("[SKIP] Column has empty or null value: " + colName);
                                    continue;
                                }

                                decimal decimalTest;
                                bool isDecimal = decimal.TryParse(colValue, out decimalTest);//currently ignoring decimal values

                                if (isDecimal && decimalTest.ToString().Contains(".") && decimalTest.ToString().Split('.')[1].Length > 2)
                                {
                                    Console.WriteLine("[SKIP] Column has high-precision decimal (ignored): " + colName + " = " + colValue);
                                    continue;
                                }
                                string safeValueRaw = colValue.Replace("'", "''");

                                if (rawFilterExpression.Length > 0) rawFilterExpression += " AND ";
                                rawFilterExpression += "[" + colName + "] = '" + safeValueRaw + "'";

                                string safeValueFormatted = safeValueRaw;
                                if (isDecimal)
                                {
                                    safeValueFormatted = decimalTest.ToString("0.00").Replace("'", "''");
                                }

                                if (formattedFilterExpression.Length > 0) formattedFilterExpression += " AND ";
                                formattedFilterExpression += "[" + colName + "] = '" + safeValueFormatted + "'";

                            }

                            foundRows = null;
                            if (!string.IsNullOrEmpty(rawFilterExpression))
                            {
                                foundRows = UItable.Select(rawFilterExpression);

                                if (foundRows.Length == 1)
                                {
                                    Console.WriteLine("[MATCH] Found using raw filter: " + rawFilterExpression);
                                    ApplyReplacement(foundRows[0], excelRow, targetColumn, condition, currentValue, replaceValue, testcasefixingrow);
                                    break;
                                }
                            }

                            // Try formatted expression if raw failed or didn't return exactly one
                            if ((foundRows == null || foundRows.Length != 1) && !string.IsNullOrEmpty(formattedFilterExpression))
                            {
                                foundRows = UItable.Select(formattedFilterExpression);

                                if (foundRows.Length == 1)
                                {
                                    Console.WriteLine("[MATCH] Found using formatted filter: " + formattedFilterExpression);
                                    ApplyReplacement(foundRows[0], excelRow, targetColumn, condition, currentValue, replaceValue, testcasefixingrow);
                                    break;
                                }
                            }

                            //if (!string.IsNullOrEmpty(filterExpression))
                            //{
                            //    foundRows = UItable.Select(filterExpression);

                            //    if (foundRows.Length == 1)
                            //    {
                            //        Console.WriteLine("[MATCH] Found using fallback all-column filter: " + filterExpression);
                            //        // TODO: Replacement logic here
                            //        ApplyReplacement(foundRows[0], excelRow, targetColumn, condition, currentValue, replaceValue, testcasefixingrow);
                            //        break;
                            //    }
                            //}
                        }
                    }

                    // 3️ Still not found → throw error
                    if (foundRows == null || foundRows.Length != 1)
                    {
                        throw new Exception("No unique matching row found for step '" + stepName + "' and TestCaseID '" + testCaseID + "' . TestCaseFixingMode Failed");
                    }

                    if (ApplicationArguments.TestCaseFixingRowDictionary.ContainsKey(testcasefixingrow.TestCaseID))
                    {
                        ApplicationArguments.TestCaseFixingRowDictionary[testcasefixingrow.TestCaseID] = testcasefixingrow;
                    }
                    else
                    {
                        ApplicationArguments.TestCaseFixingRowDictionary.Add(testcasefixingrow.TestCaseID, testcasefixingrow);
                    }

                    try
                    {
                        if (testcasefixingrow.UpdatedColumns != null && testcasefixingrow.UpdatedColumns.Count > 0 &&
     testcasefixingrow.OlderValues != null && testcasefixingrow.OlderValues.Count > 0 &&
     testcasefixingrow.NewerValues != null && testcasefixingrow.NewerValues.Count > 0 &&
     testcasefixingrow.Details != null && testcasefixingrow.Details.Count > 0)
                        {
                            CoreModeHelper.GenerateFixingReport(testCaseID, testcasefixingrow);
                            testcasefixingrow.Reset();
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }


                Console.WriteLine("[INFO] Successfully updated sheet: " + sheetName);

            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] An error occurred while updating '" + sheetName + "': " + ex.Message);
                Console.WriteLine("[STACKTRACE] " + ex.StackTrace);
                throw new Exception("No unique matching row found for step '" + stepName + "' and TestCaseID '" + testCaseID + "'");
            }
            finally
            {
                Console.WriteLine("[INFO] Finished processing sheet: " + sheetName);
            }
        }


        /// <summary>
        /// Applies replacement logic based on condition, currentValue, and replaceValue.
        /// </summary>
        private void ApplyReplacement(DataRow uiRow, DataRow excelRow, string targetColumn, string condition, string currentValue, string replaceValue, TestCaseFixingRow testcasefixingrow)
        {
            try
            {
                if (string.IsNullOrEmpty(targetColumn))
                {
                    // Complete replacement from excelRow → uiRow
                    foreach (DataColumn col in excelRow.Table.Columns)
                    {
                        string excelValue = excelRow[col.ColumnName] != null ? excelRow[col.ColumnName].ToString() : string.Empty;

                        if (!string.IsNullOrEmpty(excelValue))
                        {
                            if (uiRow.Table.Columns.Contains(col.ColumnName))
                            {
                                string uiValue = uiRow[col.ColumnName] != null ? uiRow[col.ColumnName].ToString() : string.Empty;
                                if (!string.Equals(excelValue, uiValue, StringComparison.OrdinalIgnoreCase))
                                {
                                    testcasefixingrow.UpdatedColumns.Add(col.ColumnName);
                                    testcasefixingrow.OlderValues.Add(excelValue);
                                    testcasefixingrow.NewerValues.Add(uiValue);
                                    testcasefixingrow.Details.Add("[UPDATE] Column: " + col.ColumnName +
                                           " | Excel(before): " + excelValue +
                                           " | UI: " + uiValue +
                                           " → Updated Excel to match UI.");

                                    Console.WriteLine("[UPDATE] Column: " + col.ColumnName +
                                           " | Excel(before): " + excelValue +
                                           " | UI: " + uiValue +
                                           " → Updated Excel to match UI.");


                                    excelRow[col.ColumnName] = uiValue;
                                }
                            }
                        }
                    }
                    return;
                }

                if (!uiRow.Table.Columns.Contains(targetColumn))
                {
                    Console.WriteLine("[WARN] Target column '" + targetColumn + "' does not exist. Skipping.");
                    return;
                }

                string existingValue = excelRow[targetColumn] != null ? excelRow[targetColumn].ToString() : "";

                // Case 1: All three provided
                if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(currentValue) && !string.IsNullOrEmpty(replaceValue))
                {
                    if (condition.ToLower() == "contains")
                    {
                        if (existingValue.Contains(currentValue))
                        {
                            testcasefixingrow.UpdatedColumns.Add(targetColumn);
                            testcasefixingrow.OlderValues.Add(existingValue);
                            testcasefixingrow.NewerValues.Add(replaceValue);
                            testcasefixingrow.Details.Add("[UPDATE-CONTAINS] Column: " + targetColumn +
                                " | Excel(before): " + existingValue +
                                " | ReplaceValue: " + replaceValue);


                            Console.WriteLine("[UPDATE-CONTAINS] Column: " + targetColumn +
                                " | Excel(before): " + existingValue +
                                " | ReplaceValue: " + replaceValue);

                            excelRow[targetColumn] = replaceValue;
                        }
                    }
                    else if (condition.ToLower() == "equals")
                    {
                        if (existingValue == currentValue)
                        {
                            testcasefixingrow.UpdatedColumns.Add(targetColumn);
                            testcasefixingrow.OlderValues.Add(existingValue);
                            testcasefixingrow.NewerValues.Add(replaceValue);
                            testcasefixingrow.Details.Add("[UPDATE-EQUALS] Column: " + targetColumn +
                                              " | Excel(before): " + existingValue +
                                              " | ReplaceValue: " + replaceValue);

                            Console.WriteLine("[UPDATE-EQUALS] Column: " + targetColumn +
                                              " | Excel(before): " + existingValue +
                                              " | ReplaceValue: " + replaceValue);

                            excelRow[targetColumn] = replaceValue;
                        }
                    }
                }
                // Case 2: Only replaceValue provided
                else if (!string.IsNullOrEmpty(replaceValue) && string.IsNullOrEmpty(currentValue))
                {
                    Console.WriteLine("[UPDATE-DIRECT] Column: " + targetColumn +
                         " | Excel(before): " + existingValue +
                         " | ReplaceValue: " + replaceValue);

                    testcasefixingrow.UpdatedColumns.Add(targetColumn);
                    testcasefixingrow.OlderValues.Add(existingValue);
                    testcasefixingrow.NewerValues.Add(replaceValue);
                    testcasefixingrow.Details.Add("[UPDATE-DIRECT] Column: " + targetColumn +
                         " | Excel(before): " + existingValue +
                         " | ReplaceValue: " + replaceValue);


                    excelRow[targetColumn] = replaceValue;
                }
                // Case 3: All empty — copy from excelRow
                else if (string.IsNullOrEmpty(condition) && string.IsNullOrEmpty(currentValue) && string.IsNullOrEmpty(replaceValue))
                {
                    string uiValue = uiRow[targetColumn] != null ? uiRow[targetColumn].ToString() : "";
                    string excelValue = excelRow[targetColumn] != null ? excelRow[targetColumn].ToString() : "";

                    // Only copy if values are different--new requirement
                    if (!string.Equals(uiValue, excelValue, StringComparison.Ordinal))
                    {
                        Console.WriteLine("[COPY] Column: " + targetColumn +
                                          " | Excel(before): " + excelValue +
                                          " | UI: " + uiValue);

                        excelRow[targetColumn] = uiValue;

                        testcasefixingrow.UpdatedColumns.Add(targetColumn);
                        testcasefixingrow.OlderValues.Add(excelValue);
                        testcasefixingrow.NewerValues.Add(uiValue);
                        testcasefixingrow.Details.Add("[COPY] Column: " + targetColumn +
                                          " | Excel(before): " + excelValue +
                                          " | UI: " + uiValue);
                    }
                    else
                    {
                        Console.WriteLine("[SKIP COPY] Column: " + targetColumn +
                                          " | Excel and UI values are the same: " + excelValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Replacement failed for column '" + targetColumn + "': " + ex.Message);
            }
        }
    }
}
