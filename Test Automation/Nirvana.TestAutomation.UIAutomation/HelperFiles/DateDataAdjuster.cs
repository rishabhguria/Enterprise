using System;
using System.Data;
using System.Text.RegularExpressions;

namespace Nirvana.TestAutomation.UIAutomation
{

        public class DateDataAdjuster
        {
            public static DataTable AdjustDateData(DataTable originalTable)
            {
                try
                {
                    if (originalTable == null)
                        throw new ArgumentNullException("originalTable");

                    DataTable updatedTable = originalTable.Clone(); // Copy structure

                    for (int i = 0; i < originalTable.Rows.Count; i++)
                    {
                        DataRow newRow = updatedTable.NewRow();

                        for (int j = 0; j < originalTable.Columns.Count; j++)
                        {
                            object cellObj = originalTable.Rows[i][j];
                            string cellValue = (cellObj != null) ? cellObj.ToString() : "";

                            double tempDouble;
                            if (double.TryParse(cellValue, out tempDouble))
                            {
                                newRow[j] = cellValue;
                                continue;
                            }

                            DateTime cellDate;
                            if (DateTime.TryParse(cellValue, out cellDate))
                            {
                                string formula = GetExcelDateFormulaAsString(cellDate);
                                newRow[j] = formula;
                            }
                            else
                            {
                                try
                                {
                                    Match dateMatch = Regex.Match(cellValue, @"\d{1,2}/\d{1,2}/\d{4}");

                                    if (dateMatch.Success && DateTime.TryParse(dateMatch.Value, out cellDate))
                                    {
                                        string todayFormula = GetExcelDateFormulaAsString(cellDate);
                                        string updatedText = cellValue.Replace(dateMatch.Value, todayFormula);
                                        newRow[j] = updatedText;
                                    }
                                    else
                                    {
                                        newRow[j] = cellValue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error parsing cell: " + ex.Message);
                                    newRow[j] = cellValue;
                                }
                            }
                        }

                        updatedTable.Rows.Add(newRow);
                    }

                    return updatedTable;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in AdjustDateData: " + ex.Message);
                    return originalTable;
                }
            }

            public static string GetExcelDateFormulaAsString(DateTime cellDate)
            {
                try
                {
                    int offset = (cellDate.Date - DateTime.Today).Days;

                    if (offset == 0)
                    {
                        return "TODAY()";
                    }
                    else if (offset > 0)
                    {
                        return "TODAY()+" + offset.ToString();
                    }
                    else // offset < 0
                    {
                        return "TODAY()-" + Math.Abs(offset).ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in GetExcelDateFormulaAsString: " + ex.Message);
                }

                return null;
            }

        }
}
