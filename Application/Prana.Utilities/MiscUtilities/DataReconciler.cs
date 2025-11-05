using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.Utilities.MiscUtilities
{
    public class DATAReconciler
    {
        /// <summary>
        /// Matching rows will be removed from dt2 and will be added to dtOutputTable
        /// </summary>
        /// <param name="dt1">Table1 having columns to match</param>
        /// <param name="dt2">Table2 having coluns to match</param>
        /// <param name="lstColumnsToKey">List of colums which will be used to create a comparision key</param>
        /// <param name="dictColumnsToReconcile">This dictionary will be used to keep mapping of source-destination columns</param>
        /// <param name="ToleranceValue">Price tolerance value for price </param>
        /// <param name="comparisionType"></param>
        /// <returns>at the end of function if dt2.rows.count=0 then isRecociledSuccessfully will be true</returns>
        /// TODO: We can make a business object to keep all matching info and pass this matching object to reconcile data
        public static void RecocileData(DataTable dt1, DataTable dt2, List<string> lstColumnsToKey, Dictionary<string, string> dictColumnsToReconcile, double ToleranceValue, ComparisionType comparisionType)
        {
            try
            {
                //Columns to reconcile in table1
                List<string> lstColumnsToReconcile = dictColumnsToReconcile.Keys.ToList();
                //Columns to reconcile in table2
                List<string> lstMappedColumnsToReconcile = dictColumnsToReconcile.Where(x => lstColumnsToKey.Contains(x.Key)).Select(x => x.Value).ToList();

                Dictionary<string, List<DataRow>> dt1Dict = GetDictForFastCompare(dt1, lstColumnsToKey);
                Dictionary<string, List<DataRow>> dt2Dict = GetDictForFastCompare(dt2, lstMappedColumnsToReconcile);

                List<string> lstSymbolMatchColumns = new List<string>();
                lstSymbolMatchColumns.Add("Symbol");
                lstSymbolMatchColumns.Add("Bloomberg");
                lstSymbolMatchColumns.Add("SEDOL");
                lstSymbolMatchColumns.Add("ISIN");
                lstSymbolMatchColumns.Add("RIC");

                foreach (string column in lstColumnsToReconcile)
                {
                    if (!dt2.Columns.Contains(column + "(UploadedSymbol)"))
                    {
                        dt2.Columns.Add(column + "(UploadedSymbol)", typeof(string));
                    }
                    if (!dt2.Columns.Contains(column + "(FoundSymbol)"))
                    {
                        dt2.Columns.Add(column + "(FoundSymbol)", typeof(string));
                    }
                }

                //add mismmatch details column
                if (!dt2.Columns.Contains("Mismatch"))
                {
                    dt2.Columns.Add("Mismatch", typeof(string));
                }

                //add account column to show account list on symbol management UI
                if (!dt2.Columns.Contains("Funds"))
                {
                    dt2.Columns.Add("Funds", typeof(string));
                }

                //add account column to show account list on symbol management UI
                if (!dt2.Columns.Contains("ThirdParty"))
                {
                    dt2.Columns.Add("ThirdParty", typeof(string));
                }

                foreach (KeyValuePair<string, List<DataRow>> dt1Value in dt1Dict)
                {
                    //for now the comparisonkey is kept as symbol...
                    string comparisonKey = dt1Value.Key;

                    List<DataRow> rowsApp = new List<DataRow>();
                    rowsApp.AddRange(dt1Value.Value);

                    if (dt2Dict.ContainsKey(comparisonKey))
                    {
                        List<DataRow> rowsPB = new List<DataRow>();
                        rowsPB.AddRange(dt2Dict[comparisonKey]);
                        foreach (DataRow row1 in rowsApp)
                        {
                            foreach (DataRow row2 in rowsPB)
                            {
                                foreach (string matchingColumn in lstColumnsToReconcile)
                                {
                                    string var1 = row1[matchingColumn].ToString().Replace(",", "");
                                    string var2 = row2[dictColumnsToReconcile[matchingColumn]].ToString().Replace(",", "");
                                    switch (comparisionType)
                                    {
                                        case ComparisionType.Exact:
                                            row2[matchingColumn + "(UploadedSymbol)"] = var1;
                                            row2[matchingColumn + "(FoundSymbol)"] = var2;
                                            if (!row2["Funds"].ToString().Contains(row1["FundName"].ToString()))
                                                row2["Funds"] += row1["FundName"].ToString() + Seperators.SEPERATOR_8;
                                            //TODO: fill third party details here

                                            //ignore matching for columns which have blank value
                                            //as this may be possible that imported trades only have one symbol on which they want to request symbol.
                                            //In this case there will be no symbol mismatch
                                            if ((!string.IsNullOrEmpty(var1) && !string.IsNullOrEmpty(var2))) //&& (!var1.Equals(var2)))
                                            {
                                                if (lstSymbolMatchColumns.Contains(matchingColumn) && !row2["Mismatch"].ToString().Contains("Symbol"))
                                                    row2["Mismatch"] += "Symbol" + Seperators.SEPERATOR_8;
                                                if (matchingColumn == "Multiplier" && !row2["Mismatch"].ToString().Contains("Multiplier"))
                                                    row2["Mismatch"] += "Multiplier" + Seperators.SEPERATOR_8;
                                                if (matchingColumn == "CurrencyID" && !row2["Mismatch"].ToString().Contains("CurrencyID"))
                                                    row2["Mismatch"] += "CurrencyID" + Seperators.SEPERATOR_8;
                                            }
                                            break;
                                        case ComparisionType.Partial:
                                            break;
                                        case ComparisionType.Numeric:
                                            double num1 = 0;
                                            double num2 = 0;
                                            if (double.TryParse(var1, out num1) && double.TryParse(var2, out num2))
                                            {
                                                if (row1.Table.Columns.Contains("MarkPrice"))
                                                {
                                                    row1["MarkPrice"] = row2["MarkPrice"];
                                                }
                                                double tolerancePercent = System.Math.Abs(((num1 - num2) / num2) * 100);
                                                //For multiple matching columns, if a single column is unmatched then set the isPriceMatched=false
                                                if (tolerancePercent > ToleranceValue)
                                                {
                                                    if (row1.Table.Columns.Contains("ValidationStatus"))
                                                    {

                                                        row1["ValidationStatus"] = ApplicationConstants.ValidationStatus.NonValidated.ToString();
                                                    }
                                                    if (row1.Table.Columns.Contains("MismatchType"))
                                                    {
                                                        if (!string.IsNullOrEmpty(row1["MismatchType"].ToString()))
                                                        {
                                                            row1["MismatchType"] += ", ";
                                                        }
                                                        row1["MismatchType"] += "Price Mismatch";
                                                    }
                                                    if (row1.Table.Columns.Contains("MisMatchDetails"))
                                                    {
                                                        row1["MisMatchDetails"] += "~Price~" + num1 + "~";
                                                    }
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }



                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Create dictionary from datatable for data compare
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private static Dictionary<string, List<DataRow>> GetDictForFastCompare(DataTable dt, List<string> columns)
        {
            Dictionary<string, List<DataRow>> dtDict = new Dictionary<string, List<DataRow>>();
            try
            {
                //the rowID will act as primaryKey for the datatable passed..
                //if (!dt.Columns.Contains(PrimaryKey))
                //{
                //    dt.Columns.Add(PrimaryKey);
                //}
                //int rowID = 0;

                foreach (DataRow row1 in dt.Rows)
                {

                    //row1[PrimaryKey] = rowID;
                    //rowID++;
                    string key = string.Empty;
                    foreach (string column in columns)
                    {
                        int index = dt.Columns.IndexOf(column);
                        if (index != -1)
                        {
                            key = key + "::" + row1.ItemArray[index].ToString();
                        }
                    }
                    if (!dtDict.ContainsKey(key))
                    {
                        List<DataRow> rows = new List<DataRow>();
                        rows.Add(row1);
                        dtDict.Add(key, rows);
                    }
                    else
                    {
                        dtDict[key].Add(row1);

                    }
                }

                //dt.PrimaryKey = new DataColumn[] { dt.Columns[PrimaryKey] };
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dtDict;
        }
    }
}
