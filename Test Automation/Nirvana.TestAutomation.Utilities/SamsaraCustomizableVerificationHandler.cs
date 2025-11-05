using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using System.Text.RegularExpressions;

namespace Nirvana.TestAutomation.Utilities
{
    public class SamsaraCustomizableVerificationHandler
    {
        public static DataTable ExcelData = null;
        public class Column_Mapping
        {
            public string KeyName { get; set; }
            public string ValueName { get; set; }

            public Column_Mapping()
            {
                KeyName = "";
                ValueName = "";
            }
        }
        public static void LinkExcelData(ref DataTable dt)
        {

            ExcelData = dt;
        }
        public static void CustomizableVerificationHandler(ref string stepName, DataTable dt, ref DataTable UIData)
        {
            try
            {
                Dictionary<string, Dictionary<string, Column_Mapping>> ColumnMappingDictionary = new Dictionary<string, Dictionary<string, Column_Mapping>>();
                try
                {
                    string localStepName = stepName;

                    var resultRows = from row in dt.AsEnumerable()
                                     where row.Field<string>("StepName") == localStepName
                                     select row;

                    int rowCount = resultRows.Count();


                    foreach (DataRow dr in resultRows)
                    {
                        if (!ColumnMappingDictionary.ContainsKey(dr["Action"].ToString()))
                        {
                            ColumnMappingDictionary[dr["Action"].ToString()] = new Dictionary<string, Column_Mapping>();
                            ColumnMappingDictionary[dr["Action"].ToString()][dr["Comparator"].ToString()] = new Column_Mapping();
                            ColumnMappingDictionary[dr["Action"].ToString()][dr["Comparator"].ToString()].KeyName = dr["KeyName"].ToString();
                            ColumnMappingDictionary[dr["Action"].ToString()][dr["Comparator"].ToString()].ValueName = dr["ValueName"].ToString();

                        }


                        else if (!ColumnMappingDictionary[dr["Action"].ToString()].ContainsKey(dr["Comparator"].ToString()))
                        {
                            ColumnMappingDictionary[dr["Action"].ToString()][dr["Comparator"].ToString()] = new Column_Mapping();
                            ColumnMappingDictionary[dr["Action"].ToString()][dr["Comparator"].ToString()].KeyName = dr["KeyName"].ToString();
                            ColumnMappingDictionary[dr["Action"].ToString()][dr["Comparator"].ToString()].ValueName = dr["ValueName"].ToString();
                        }
                        else
                        {
                            Console.WriteLine("No two operations should be done in single Column");
                        }

                    }

                    if (ColumnMappingDictionary.ContainsKey("ReplaceColumnValue"))
                    {
                        foreach (var val in ColumnMappingDictionary["ReplaceColumnValue"])
                        {
                            if (UIData.Columns.Contains(val.Key))
                            {
                                UIData.Columns[val.Key].ColumnName = ColumnMappingDictionary["ReplaceColumnValue"][val.Key].ValueName;
                            }
                            else
                            {
                                Console.WriteLine("Column with name " + val + " not found.");
                            }
                        }
                    }

                    if (ColumnMappingDictionary.ContainsKey("ReplaceRowValue"))
                    {

                        foreach (DataRow drow in UIData.Rows)
                        {
                            try
                            {
                                foreach (var val in ColumnMappingDictionary["ReplaceRowValue"])
                                {

                                    if (string.Equals(drow[val.Key].ToString(), ColumnMappingDictionary["ReplaceRowValue"][val.Key].KeyName, StringComparison.OrdinalIgnoreCase))
                                    {
                                        drow[val.Key] = ColumnMappingDictionary["ReplaceRowValue"][val.Key].ValueName;


                                    }
                                    else
                                    {
                                        Console.WriteLine("Column with name " + val.Key + " not found in DataRow.");
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }

                        }


                    }

                    if (ColumnMappingDictionary.ContainsKey("RemoveColumnsAndRows"))
                    {
                        try
                        {
                            foreach (var val in ColumnMappingDictionary["RemoveColumnsAndRows"])
                            {
                                string columnList = val.Key.ToString();
                                DataUtilities.RemoveColumnsAndRows(ref columnList, ref ExcelData);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }



                    }
                    if (ColumnMappingDictionary.ContainsKey("RemoveColumnsAndRowsUI"))
                    {
                        try
                        {
                            foreach (var val in ColumnMappingDictionary["RemoveColumnsAndRowsUI"])
                            {
                                string columnList = val.Key.ToString();
                                DataUtilities.RemoveColumnsAndRows(ref columnList, ref UIData);
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }

        public static bool CustomizableDataManipulator(string stepName, DataTable excelData, DataTable UIData, DataTable DataManipulator)
        {
            bool anyDataChanged = false;
            try
            {
                foreach (DataRow row in DataManipulator.Rows)
                {
                    string step = row["Steps"].ToString().Trim();
                    string action = row["Action"].ToString().Trim();
                    List<string> columns = row["Columns"].ToString().Split(',').ToList();

                    if (step.Equals(stepName, StringComparison.OrdinalIgnoreCase))
                    {
                        anyDataChanged = true;
                        string[] actionParts = action.Split('-');
                        string mainAction = actionParts[0].Trim();
                        string target = actionParts.Length > 1 ? actionParts[1].Trim() : "";
                        switch (mainAction)
                        {
                            case "Normalize":
                                if (target == "UI")
                                    Normalize(UIData, columns);
                                else
                                    Normalize(excelData, columns);
                                break;
                            default:
                                Console.WriteLine("Unsupported action: " + mainAction);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return anyDataChanged;
        }
        private static void Normalize(DataTable table, List<string> columns)
        {
            try
            {
                if (table == null || columns == null || columns.Count == 0)
                    return;

                foreach (string column in columns)
                {
                    if (!table.Columns.Contains(column))
                        continue;

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string originalValue = table.Rows[i][column].ToString();
                        Console.WriteLine(originalValue);
                        if (!string.IsNullOrEmpty(originalValue))
                        {
                            string updatedValue = NormalizeDecimalFormat(originalValue);
                            Console.WriteLine(updatedValue);
                            table.Rows[i][column] = updatedValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        private static string NormalizeDecimalFormat(string input)
        {
            string originalString = input;
            try
            {
                input = Regex.Replace(input, @"(\d+)\.00\.(\d+)", "$1.$2");
                input = Regex.Replace(input, @"(\d+)\.00([-.\d])", "$1$2");
                if (Regex.IsMatch(input, @"^\d+\.\d+$"))
                {
                    return input;
                }

                return input;
            }
            catch { return originalString; }
        }
       
    }
}
