
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Reconciliation;
using Prana.BusinessObjects;
using Prana.Interfaces;
using Infragistics.Documents.Excel;
using System.Windows.Forms;
using Prana.BusinessObjects.AppConstants;

namespace Prana.Reconciliation
{
    public class ComparingLogic
    {

        static bool _isSchemaCreated = false;

        private static DataTable _dtExceptions = new DataTable();

        static Dictionary<string, RuleParameters> rulefieldCollection = new Dictionary<string, RuleParameters>();

        #region commented
        //private static Dictionary<DataRow, DataRow> _dictRowMapping = new Dictionary<DataRow, DataRow>();

        //public void CompareOld(DataTable dt1, DataTable dt2, DataTable dt3, DataTable dt4, MatchingRule rule)
        //{

        //    try
        //    {
        //        if (!dt1.Columns.Contains("Matched"))
        //        {
        //            dt1.Columns.Add("Matched");
        //        }
        //        if (!dt2.Columns.Contains("Matched"))
        //        {
        //            dt2.Columns.Add("Matched");
        //        }

        //        if (!dt1.Columns.Contains("Mismatch"))
        //        {
        //            dt1.Columns.Add("Mismatch");
        //        }
        //        if (!dt2.Columns.Contains("Mismatch"))
        //        {
        //            dt2.Columns.Add("Mismatch");
        //        }

        //        //NewUtilities.CopyColumns(dt1, dt3);
        //        //NewUtilities.CopyColumns(dt2, dt4);

        //        DataTableUtilities.CopyColumns(dt1, dt3);
        //        DataTableUtilities.CopyColumns(dt2, dt4);
        //        //dt3 = dt1.Clone();
        //        //dt4 = dt2.Clone();

        //        List<object> deletedRowsIDsFromdt1 = new List<object>();
        //        List<object> deletedRowsIDsFromdt2 = new List<object>();

        //        foreach (DataRow row1 in dt1.Rows)
        //        {
        //            foreach (DataRow row2 in dt2.Rows)
        //            {
        //                if (row2["Matched"].ToString() != rule.Name)
        //                {
        //                    if (deletedRowsIDsFromdt2.Contains(row2["RowID"]))
        //                    {
        //                        continue;
        //                    }
        //                    Result result = rule.IsRulePassed(row1, row2);
        //                    if (result.IsPassed)
        //                    {
        //                        row1["Matched"] = row2["RowID"];
        //                        row2["Matched"] = row1["RowID"];
        //                        if (result.Text != String.Empty)
        //                        {
        //                            row1["Mismatch"] = result.Text;
        //                            row2["Mismatch"] = result.Text;
        //                        }
        //                        deletedRowsIDsFromdt1.Add(row1["RowID"]);
        //                        deletedRowsIDsFromdt2.Add(row2["RowID"]);
        //                        //dt3.ImportRow(row1);
        //                        //dt4.ImportRow(row2);
        //                        dt3.Rows.Add(row1.ItemArray);
        //                        dt4.Rows.Add(row2.ItemArray);
        //                        break;
        //                    }


        //                }
        //            }
        //        }
        //        foreach (object rowID in deletedRowsIDsFromdt1)
        //        {
        //            DataRow row = dt1.Rows.Find(rowID);
        //            if (row != null)
        //                dt1.Rows.Remove(row);
        //        }
        //        foreach (object rowID in deletedRowsIDsFromdt2)
        //        {
        //            DataRow row = dt2.Rows.Find(rowID);
        //            if (row != null)
        //                dt2.Rows.Remove(row);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}

        //public void PartialCompareOld(DataTable dt1, DataTable dt2, MatchingRule rule)
        //{
        //    try
        //    {
        //        // Text Contains the reson of mismatch
        //        if (!dt1.Columns.Contains("Text"))
        //        {
        //            dt1.Columns.Add("Text");
        //        }
        //        if (!dt2.Columns.Contains("Text"))
        //        {
        //            dt2.Columns.Add("Text");
        //        }

        //        List<string> symbolFoundRows = new List<string>();
        //        if (dt1.Columns.Contains("Symbol") && dt2.Columns.Contains("Symbol"))
        //        {

        //            foreach (DataRow row1 in dt1.Rows)
        //            {
        //                bool isSymbolFound = false;
        //                foreach (DataRow row2 in dt2.Rows)
        //                {

        //                    if (row1["Symbol"].ToString() == row2["Symbol"].ToString())
        //                    {
        //                        isSymbolFound = true;
        //                        if (!symbolFoundRows.Contains(row2["RowID"].ToString()))
        //                        {
        //                            symbolFoundRows.Add(row2["RowID"].ToString());
        //                        }
        //                    }
        //                    if (row2["Text"].ToString() == String.Empty)
        //                    {
        //                        string result = rule.IsRulePartiallyPassed(row1, row2);
        //                        if (result != String.Empty)
        //                        {
        //                            row1["Text"] = result.ToString() + " Mismatched";
        //                            row2["Text"] = result.ToString() + " Mismatched";
        //                            row1["Matched"] = row2["RowID"];
        //                            row2["Matched"] = row1["RowID"];
        //                            break;
        //                        }
        //                    }
        //                }
        //                if (row1["Text"].ToString() == String.Empty && !isSymbolFound)
        //                {
        //                    row1["Text"] = "Symbol not found";
        //                }
        //            }
        //            foreach (DataRow row2 in dt2.Rows)
        //            {
        //                if (row2["Text"].ToString() == String.Empty && !symbolFoundRows.Contains(row2["RowID"].ToString()))
        //                {
        //                    row2["Text"] = "Symbol not found";
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        #endregion
        public static void Reconcile(DataTable dtAppDataToMatch, DataTable dtPBDataToMatch, out DataTable dtAppDataMatched, out DataTable dtPBdataMatched, MatchingRule rule, bool includeMatchedInExceptions, bool includeTolMatchedInExcpetions, List<ColumnInfo> listExceptionReportColumns, bool isClearExpCache)
        {
            // DataTable dtExceptions = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            dtAppDataMatched = new DataTable();
            dtPBdataMatched = new DataTable();
            try
            {
                //Narendra Kumar Jangir 2012 Oct 30
                //validate matching rules for both Nirvana and PB data
                StringBuilder ReconErrorMessage = rule.ValidateMatchingRules(dtAppDataToMatch, dtPBDataToMatch);
                //if ReconErrorMessage is empty than move to reconcile process else show the proper error message
                if (string.IsNullOrEmpty(ReconErrorMessage.ToString()))
                {
                    //if (isClearExpCache)
                    //{
                    //    _dtExceptions.Rows.Clear();
                    //}
                    List<string> lisdisplayColums = new List<string>();

                    if (!dtAppDataToMatch.Columns.Contains(ReconConstants.Matched))
                    {
                        dtAppDataToMatch.Columns.Add(ReconConstants.Matched);
                    }
                    if (!dtPBDataToMatch.Columns.Contains(ReconConstants.Matched))
                    {
                        dtPBDataToMatch.Columns.Add(ReconConstants.Matched);
                    }
                    if (!dtAppDataToMatch.Columns.Contains(ReconConstants.MismatchType))
                    {
                        dtAppDataToMatch.Columns.Add(ReconConstants.MismatchType);
                        dtAppDataToMatch.Columns[ReconConstants.MismatchType].Caption = ReconConstants.CAPTION_MismatchType; ;
                    }
                    if (!dtPBDataToMatch.Columns.Contains(ReconConstants.MismatchType))
                    {
                        dtPBDataToMatch.Columns.Add(ReconConstants.MismatchType);
                        dtPBDataToMatch.Columns[ReconConstants.MismatchType].Caption = ReconConstants.CAPTION_MismatchType; ;
                    }
                    if (!dtAppDataToMatch.Columns.Contains(ReconConstants.MismatchReason))
                    {
                        dtAppDataToMatch.Columns.Add(ReconConstants.MismatchReason);
                        dtAppDataToMatch.Columns[ReconConstants.MismatchReason].Caption = ReconConstants.CAPTION_MismatchReason; ;

                    }
                    if (!dtPBDataToMatch.Columns.Contains(ReconConstants.MismatchReason))
                    {
                        dtPBDataToMatch.Columns.Add(ReconConstants.MismatchReason);
                        dtPBDataToMatch.Columns[ReconConstants.MismatchReason].Caption = ReconConstants.CAPTION_MismatchReason; ;
                    }



                    //foreach (DataColumn column in dtAppDataToMatch.Columns)
                    //{
                    //    lisdisplayColums.Add(column.ColumnName);
                    //}
                    //dtExceptions = GetExceptionsDataTableSchema(rule.ComparisonFields, lisdisplayColums);


                    //dt3 = dtAppDataToMatch.Clone();
                    //dt4 = dtPBDataToMatch.Clone();
                    FastCompare(dtAppDataToMatch, dtPBDataToMatch, ref dt3, ref dt4, rule, includeMatchedInExceptions, includeTolMatchedInExcpetions, listExceptionReportColumns);
                    List<string> columns = rule.ComparisonFields;
                    Dictionary<string, List<DataRow>> dt1Dict = GetDictNew(dtAppDataToMatch, columns);
                    Dictionary<string, List<DataRow>> dt2Dict = GetDictNew(dtPBDataToMatch, columns);


                    bool isRulePassed = false;
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
                                    Result result = IsRulePassed(row1, row2);
                                    if (result.IsPassed)
                                    {
                                        row1[ReconConstants.Matched] = row2[ReconConstants.PrimaryKey];
                                        row2[ReconConstants.Matched] = row1[ReconConstants.PrimaryKey];
                                        if (result.Text != String.Empty)
                                        {
                                            row1[ReconConstants.MismatchReason] = result.Text;
                                            row2[ReconConstants.MismatchReason] = result.Text;
                                        }
                                        row1[ReconConstants.MismatchType] = result.MisMatchType; ;
                                        row2[ReconConstants.MismatchType] = result.MisMatchType;

                                        // AddToRowMappingDictionary(row1, row2);
                                        if ((includeTolMatchedInExcpetions && (result.MisMatchType.Contains("Matched Within"))) || ((includeMatchedInExceptions && (result.MisMatchType == "Exactly Matched"))))
                                        {
                                            AddToExceptionsDatatable(row1, row2, listExceptionReportColumns, rule.NumericFields);
                                        }
                                        dt3.Rows.Add(row1.ItemArray);
                                        dt4.Rows.Add(row2.ItemArray);
                                        dtAppDataToMatch.Rows.Remove(row1);
                                        dtPBDataToMatch.Rows.Remove(row2);

                                        break;
                                    }
                                }

                                // removing all matched rows..k
                                rowsPB.RemoveAll(delegate(DataRow dr)
                                     {
                                         if (dr.RowState == DataRowState.Detached || dr.RowState == DataRowState.Deleted)
                                         {
                                             return true;

                                         }
                                         return false;
                                     });
                            }

                            rowsApp.RemoveAll(delegate(DataRow dr)
                                  {
                                      if (dr.RowState == DataRowState.Detached || dr.RowState == DataRowState.Deleted)
                                      {
                                          return true;
                                      }
                                      return false;
                                  });


                            // if there are any unmatched rows left then we need to find the reason for mismatch...
                            if (rowsApp.Count > 0 && rowsPB.Count > 0)
                            {
                                foreach (DataRow row1 in rowsApp)
                                {
                                    foreach (DataRow row2 in rowsPB)
                                    {
                                        Result result = IsRulePartiallyPassed(row1, row2);

                                        if (result.Text != string.Empty)
                                        {
                                            row1[ReconConstants.Matched] = row2[ReconConstants.PrimaryKey];
                                            row2[ReconConstants.Matched] = row1[ReconConstants.PrimaryKey];
                                            row1[ReconConstants.MismatchReason] = result.Text;
                                            row2[ReconConstants.MismatchReason] = result.Text;
                                            row1[ReconConstants.MismatchType] = result.MisMatchType;
                                            row2[ReconConstants.MismatchType] = result.MisMatchType;

                                            //AddToRowMappingDictionary(row1, row2);

                                            AddToExceptionsDatatable(row1, row2, listExceptionReportColumns, rule.NumericFields);
                                            break;
                                        }
                                    }
                                    // removing all matched rows..k
                                    rowsPB.RemoveAll(delegate(DataRow dr)
                                         {
                                             if (!dr[ReconConstants.MismatchReason].Equals(System.DBNull.Value))
                                             {
                                                 return true;

                                             }
                                             return false;
                                         });

                                }


                                rowsApp.RemoveAll(delegate(DataRow dr)
                                   {
                                       if (!dr[ReconConstants.MismatchReason].Equals(System.DBNull.Value))
                                       {
                                           return true;

                                       }
                                       return false;
                                   });

                            }
                            foreach (DataRow dr in rowsApp)
                            {
                                dr[ReconConstants.MismatchReason] = "Data Missing in PB File";
                                dr[ReconConstants.MismatchType] = ReconConstants.CAPTION_MissingData;
                                AddToExceptionsDatatable(dr, null, listExceptionReportColumns, rule.NumericFields);
                            }

                            foreach (DataRow dr in rowsPB)
                            {
                                dr[ReconConstants.MismatchReason] = "Data Missing in Nirvana";
                                dr[ReconConstants.MismatchType] = ReconConstants.CAPTION_MissingData;
                                AddToExceptionsDatatable(null, dr, listExceptionReportColumns, rule.NumericFields);
                            }


                            dt2Dict.Remove(comparisonKey);
                        }
                        // it means that there is no corresponding row for this symbol in primeBroker datatable..
                        else
                        {
                            foreach (DataRow dr in rowsApp)
                            {
                                dr[ReconConstants.MismatchReason] = "Data Missing in PB File";
                                dr[ReconConstants.MismatchType] = ReconConstants.CAPTION_MissingData;


                                // the corresponding primeBroker row is null..
                                //AddToRowMappingDictionary(dr, dt4.NewRow());
                                AddToExceptionsDatatable(dr, null, listExceptionReportColumns, rule.NumericFields);
                            }
                        }
                    }
                    // the remaining rows in primeBroker data has no corresponding row in nirvana data...
                    if (dt2Dict.Values.Count > 0)
                    {
                        foreach (KeyValuePair<string, List<DataRow>> kp in dt2Dict)
                        {
                            List<DataRow> listRowsSymbolNotFound = kp.Value;

                            foreach (DataRow dr2 in listRowsSymbolNotFound)
                            {
                                dr2[ReconConstants.MismatchReason] = "Data Missing in Nirvana";

                                dr2[ReconConstants.MismatchType] = ReconConstants.CAPTION_MissingData;

                                // the corresponding nirvana row is null...
                                //AddToRowMappingDictionary(dt3.NewRow(), dr2);
                                AddToExceptionsDatatable(null, dr2, listExceptionReportColumns, rule.NumericFields);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(ReconErrorMessage.ToString(), "Matching Rule Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            dtAppDataMatched = dt3;
            dtPBdataMatched = dt4;
            dtAppDataToMatch.AcceptChanges();
            dtPBdataMatched.AcceptChanges();
            dtPBDataToMatch.AcceptChanges();
            dtAppDataMatched.AcceptChanges();
            //dtAppMisMatchedData = dtAppDataToMatch;
            //dtPBMisMatchedData = dtPBDataToMatch;
            //  return _dtExceptions;
        }

        private static Dictionary<string, List<DataRow>> GetDictNew(DataTable dt, List<string> columns)
        {
            //the rowID will act as primaryKey for the datatable passed..
            //if (!dt.Columns.Contains(ReconConstants.PrimaryKey))
            //{
            //    dt.Columns.Add(ReconConstants.PrimaryKey);
            //}
            //int rowID = 0;
            Dictionary<string, List<DataRow>> dtDict = new Dictionary<string, List<DataRow>>(StringComparer.OrdinalIgnoreCase);
            try
            {

                foreach (DataRow row1 in dt.Rows)
                {
                    //row1[ReconConstants.PrimaryKey] = rowID;
                    //rowID++;

                    string key = string.Empty;
                    //foreach (string column in columns)
                    //{
                    //if (column.Equals("Symbol"))
                    //{
                    int index = dt.Columns.IndexOf("Symbol");
                    if (index != -1)
                    {
                        key = key + "::" + row1.ItemArray[index].ToString();
                        // break;
                    }

                    foreach (string column in columns)
                    {
                        if (column.Equals("Side") || column.Equals("FundName"))
                        {
                            index = dt.Columns.IndexOf(column);

                            if (index != -1)
                            {
                                key = key + "::" + row1.ItemArray[index].ToString();
                            }
                        }
                    }
                    // }
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
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            // dt.PrimaryKey = new DataColumn[] { dt.Columns[ReconConstants.PrimaryKey] };

            return dtDict;
        }

        public static bool ExactMatch(string var1, string var2)
        {
            try
            {
                if (!string.IsNullOrEmpty(var1) || !string.IsNullOrEmpty(var2))
                {
                    if (var1.Equals(var2, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        public static bool PartialMatch(string var1, string var2)
        {
            try
            {
                if ((var1 != string.Empty && var2 != string.Empty) &&
                    (var1.Contains(var2) || var2.Contains(var1)))
                {
                    return true;
                }
                int newvar1Length = var1.IndexOf(' ');
                int newvar2Length = var2.IndexOf(' ');
                if (newvar1Length != -1)
                    var1 = var1.Substring(0, newvar1Length);
                if (newvar2Length != -1)
                    var2 = var2.Substring(0, newvar2Length);
                if ((var1 != string.Empty && var2 != string.Empty) &&
                    (var1.Contains(var2) || var2.Contains(var1)))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        public static Result NumericMatch(ref string var1, ref string var2, RuleParameters ruleParams)
        {
            Result result = new Result();
            result.Text = String.Empty;
            result.MisMatchType = string.Empty;

            try
            {
                string temp1 = var1.Replace(",", "");
                string temp2 = var2.Replace(",", "");
                double num1 = 0;
                double num2 = 0;
                //http://jira.nirvanasolutions.com:8080/browse/WINTON-37
                //if values are very small(i.e. having 10^-6) regular expression failed to identify number

                //if (Prana.Utilities.StringUtilities.RegularExpressionValidation.IsNumber(temp1)
                //    && Prana.Utilities.StringUtilities.RegularExpressionValidation.IsNumber(temp2))

                if (double.TryParse(temp1, out num1) && double.TryParse(temp2, out num2))
                {
                    num1 = double.Parse(temp1);
                    num2 = double.Parse(temp2);

                    if (num1.Equals(num2))
                    {
                        result.IsPassed = true;
                        result.Text = String.Empty;
                        // return result;
                    }
                    else if (ruleParams.IsRoundOff)
                    {
                        double int1 = System.Math.Round(num1, ruleParams.RoundDigits);
                        double int2 = System.Math.Round(num2, ruleParams.RoundDigits);
                        var1 = int1.ToString();
                        var2 = int2.ToString();
                        if (int1.Equals(int2))
                        {
                            result.IsPassed = true;
                            result.Text = ruleParams.FieldName + "(" + "Rounded to " + ruleParams.RoundDigits + " significant Digits)";
                            result.MisMatchType = "Matched By Rounding Digits";
                            //return result;
                        }
                    }
                    else if (ruleParams.IsIntegralMatch)
                    {
                        double int1 = System.Math.Floor(num1);
                        double int2 = System.Math.Floor(num2);
                        var1 = int1.ToString();
                        var2 = int2.ToString();
                        if (int1.Equals(int2))
                        {
                            result.IsPassed = true;
                            result.Text = ruleParams.FieldName + "(" + "Integral" + ")";
                            // return result;
                        }
                    }
                    else if (ruleParams.IsPercentMatch)
                    {
                        double errorPercent = System.Math.Abs(((num1 - num2) / num2) * 100);
                        if (errorPercent <= ruleParams.ErrorTolerance)
                        {
                            result.IsPassed = true;
                            result.Text = ruleParams.FieldName + "(" + errorPercent.ToString("#.00") + " %" + ")";
                            result.MisMatchType = "Matched Within Tolerance";
                        }
                    }
                    else if (ruleParams.IsAbsoluteDifference)
                    {
                        double AbsDiff = System.Math.Abs(num1 - num2);
                        if (AbsDiff <= ruleParams.AbsDiff)
                        {
                            result.IsPassed = true;
                            result.Text = ruleParams.FieldName + "(" + "Matched with " + ruleParams.AbsDiff + " Absolute Tolerance)";
                            result.MisMatchType = "Matched Within Absolute Tolerance";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        private static void AddToExceptionsDatatable(DataRow appRow, DataRow Pbrow, List<ColumnInfo> listSelectedColumns, List<String> numericColumns)
        {
            ReconUtilities.NumericColumn = numericColumns;
            try
            {
                DataRow dataRowExceptions = _dtExceptions.NewRow();

                foreach (ColumnInfo column in listSelectedColumns)
                {
                    string columnKeyNewNirvana = string.Empty;
                    string columnKeyNewBroker = string.Empty;
                    string DiffColumnKey = string.Empty;
                    string CommonColumnKey = string.Empty;

                    switch (column.GroupType)
                    {
                        case ColumnGroupType.Nirvana:
                            columnKeyNewNirvana = "Nirvana" + column.ColumnName;
                            break;
                        case ColumnGroupType.PrimeBroker:
                            columnKeyNewBroker = "Broker" + column.ColumnName;
                            break;
                        case ColumnGroupType.Common:
                            CommonColumnKey = column.ColumnName;
                            break;
                        case ColumnGroupType.Both:
                            break;
                        case ColumnGroupType.Diff:
                            DiffColumnKey = "Diff" + column.ColumnName;
                            break;
                        default:
                            break;
                    }

                    //Modified By: Surendra Bisht
                    //Modification date: Dec 31, 2013
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-2276

                    if (!string.IsNullOrEmpty(columnKeyNewNirvana))
                    {
                        if (appRow != null && dataRowExceptions.Table.Columns.Contains(columnKeyNewNirvana))
                        {
                            if (appRow.Table.Columns.Contains(column.ColumnName))
                                dataRowExceptions[columnKeyNewNirvana] = appRow[column.ColumnName];
                            else if (numericColumns.Contains(column.ColumnName))
                                dataRowExceptions[columnKeyNewNirvana] = 0;
                        }
                        if (appRow == null && numericColumns.Contains(column.ColumnName))    // surendra
                            dataRowExceptions[columnKeyNewNirvana] = 0;                      // surendra


                    }
                    if (!string.IsNullOrEmpty(columnKeyNewBroker))
                    {
                        if (Pbrow != null && dataRowExceptions.Table.Columns.Contains(columnKeyNewBroker))
                        {
                            if (Pbrow.Table.Columns.Contains(column.ColumnName))
                                dataRowExceptions[columnKeyNewBroker] = Pbrow[column.ColumnName];
                            else if (numericColumns.Contains(column.ColumnName))
                                dataRowExceptions[columnKeyNewBroker] = 0;
                        }
                        if (Pbrow == null && numericColumns.Contains(column.ColumnName))    // surendra
                            dataRowExceptions[columnKeyNewBroker] = 0;                      // surendra
                    }

                    if (!string.IsNullOrEmpty(DiffColumnKey))
                    {
                        // surendra change full inside this method 

                        /* if (appRow != null && Pbrow != null)
                         {
                             if ((dataRowExceptions.Table.Columns.Contains(DiffColumnKey)) && (appRow.Table.Columns.Contains(column.ColumnName)) && (Pbrow.Table.Columns.Contains(column.ColumnName)))
                             {
                                 dataRowExceptions[DiffColumnKey] = Convert.ToDouble(appRow[column.ColumnName]) - Convert.ToDouble(Pbrow[column.ColumnName]);
                             }

                         }*/
                        double number;
                        int RoundToDecimal = 4;  // diff column to round to decimal values

                        if (dataRowExceptions.Table.Columns.Contains(DiffColumnKey))
                        {
                            if (appRow != null && appRow.Table.Columns.Contains(column.ColumnName))
                            {
                                if (Pbrow != null && Pbrow.Table.Columns.Contains(column.ColumnName))
                                {
                                    //  if (Convert.ToDouble(Pbrow[column.ColumnName]) == 0.0)   // surendra   for if pb data present, but Zero. 
                                    //    dataRowExceptions[DiffColumnKey] = 0;                  //surendra         then diffcolumn=0
                                    //else
                                    dataRowExceptions[DiffColumnKey] = Math.Round((Double.TryParse(appRow[column.ColumnName].ToString(), out number) ? number : 0) - (Double.TryParse(Pbrow[column.ColumnName].ToString(), out number) ? number : 0), RoundToDecimal);

                                }
                                else if ((appRow.Table.Columns.Contains(column.ColumnName)))
                                {

                                    dataRowExceptions[DiffColumnKey] = Math.Round(Double.TryParse(appRow[column.ColumnName].ToString(), out number) ? number : 0, RoundToDecimal);    //surendra , for if pb data not present, diffcolumn=appdata   
                                    //dataRowExceptions[DiffColumnKey] = 0;                                            // surendra  ,  for if pb data not present, diffcolumn=0
                                }

                            }
                            else                                                                                   // surendra, if nirvana data not there
                            {
                                
                                if (Pbrow.Table.Columns.Contains(column.ColumnName))
                                    dataRowExceptions[DiffColumnKey] = -1 * Math.Round(Double.TryParse(Pbrow[column.ColumnName].ToString(), out number) ? number : 0, RoundToDecimal);     // surendra ,  then  diff=0-pbdata 

                                else
                                    dataRowExceptions[DiffColumnKey] = 0;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(CommonColumnKey))
                    {
                        if (appRow != null)
                        {
                            string columnName = column.ColumnName;
                            //check applied that column exist in exception datatable schema
                            if ((dataRowExceptions.Table.Columns.Contains(CommonColumnKey)) && (appRow.Table.Columns.Contains(columnName)))
                            {
                                dataRowExceptions[CommonColumnKey] = appRow[columnName];
                            }

                        }
                        else if (Pbrow != null)
                        {

                            string columnName = column.ColumnName;
                            //check applied that column exist in exception datatable schema
                            if ((dataRowExceptions.Table.Columns.Contains(CommonColumnKey)) && (Pbrow.Table.Columns.Contains(columnName)))
                            {
                                dataRowExceptions[CommonColumnKey] = Pbrow[columnName];
                            }

                        }
                    }
                }
                //setting balnk values in any columns which are null..
                foreach (DataColumn column in dataRowExceptions.Table.Columns)
                {
                    if (dataRowExceptions.IsNull(column))
                    {
                        dataRowExceptions[column] = string.Empty;
                    }
                }


                _dtExceptions.Rows.Add(dataRowExceptions);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }



        //private static void AddToRowMappingDictionary(DataRow row1, DataRow row2)
        //{
        //    if (row1 != null && row2 != null)
        //    {
        //        if (!_dictRowMapping.ContainsKey(row1))
        //        {
        //            _dictRowMapping.Add(row1, row2);
        //        }

        //    }
        //    //else if (row1 == null)
        //    //{
        //    //    row1 = new DataRow;
        //    //    _dictRowMapping.Add(row1, row2);
        //    //}
        //    //else if (row2 != null)
        //    //{
        //    //    row2 = new DataRow();
        //    //    _dictRowMapping.Add(row1, row2);
        //    //}
        //}






        #region Commented Section

        private static void FastCompare(DataTable dt1, DataTable dt2, ref  DataTable dt3, ref  DataTable dt4, MatchingRule rule, bool includeMatchedInExceptions, bool includeTolMatchedInExcpetions, List<ColumnInfo> listExceptionReportColumns)
        {
            try
            {
                //if (!dt1.Columns.Contains("Matched"))
                //{
                //    dt1.Columns.Add("Matched");
                //}
                //if (!dt2.Columns.Contains("Matched"))
                //{
                //    dt2.Columns.Add("Matched");
                //}

                //NewUtilities.CopyColumns(dt1, dt3);
                //NewUtilities.CopyColumns(dt2, dt4);

                //DataTableUtilities.CopyColumns(dt1, dt3);
                //DataTableUtilities.CopyColumns(dt2, dt4);

                List<string> columns = rule.ComparisonFields;
                Dictionary<string, List<DataRow>> dt1Dict = GetDictForFastCompare(dt1, columns);
                Dictionary<string, List<DataRow>> dt2Dict = GetDictForFastCompare(dt2, columns);



                dt3 = dt1.Clone();
                dt4 = dt2.Clone();

                //List<string> matchedKeys = new List<string>();
                foreach (KeyValuePair<string, List<DataRow>> dt1Value in dt1Dict)
                {

                    string comparisonKey = dt1Value.Key;
                    List<DataRow> rowsApp = new List<DataRow>();
                    rowsApp.AddRange(dt1Value.Value);

                    if (dt2Dict.ContainsKey(dt1Value.Key))
                    {
                        List<DataRow> rowsPB = new List<DataRow>();
                        rowsPB.AddRange(dt2Dict[comparisonKey]);
                        //matchedKeys.Add(dt1Value.Key);
                        foreach (DataRow row1 in rowsApp)
                        {
                            foreach (DataRow row2 in rowsPB)
                            {
                                row1[ReconConstants.MismatchType] = "Exactly Matched";
                                row2[ReconConstants.MismatchType] = "Exactly Matched";
                                if (includeMatchedInExceptions)
                                {
                                    AddToExceptionsDatatable(row1, row2, listExceptionReportColumns, rule.NumericFields);
                                }
                                dt3.Rows.Add(row1.ItemArray);
                                dt4.Rows.Add(row2.ItemArray);
                                dt1.Rows.Remove(row1);
                                dt2.Rows.Remove(row2);
                                break;
                            }

                            // removing all matched rows..k
                            rowsPB.RemoveAll(delegate(DataRow dr)
                                 {
                                     if (dr.RowState == DataRowState.Detached || dr.RowState == DataRowState.Deleted)
                                     {
                                         return true;

                                     }
                                     return false;
                                 });
                        }


                        //rowsApp.RemoveAll(delegate(DataRow dr)
                        //         {
                        //             if (dr.RowState == DataRowState.Detached || dr.RowState == DataRowState.Deleted)
                        //             {
                        //                 return true;
                        //             }
                        //             return false;
                        //         });

                        //foreach (DataRow row in dt1Value.Value)
                        //{
                        //    row[ReconConstants.MismatchType] = "ExactlyMatched";

                        //    dt3.Rows.Add(row.ItemArray);
                        //    dt1.Rows.Remove(row);

                        // }
                        //foreach (DataRow row in dt2Dict[dt1Value.Key])
                        //{
                        //    row[ReconConstants.MismatchType] = "ExactlyMatched";
                        //    dt4.Rows.Add(row.ItemArray);
                        //    dt2.Rows.Remove(row);

                        //}

                        //for (int i = 0; i < length; i++)
                        //{

                        //}


                        dt2Dict.Remove(dt1Value.Key);
                    }

                }

                dt1.AcceptChanges();
                dt2.AcceptChanges();
                dt3.AcceptChanges();
                dt4.AcceptChanges();

                //foreach (string key in matchedKeys)
                //{
                //    dt1Dict.Remove(key);
                //}

                //DataTable dt5 = new DataTable();
                //DataTable dt6 = new DataTable();
                ////NewUtilities.CopyColumns(dt1, dt5);
                ////NewUtilities.CopyColumns(dt2, dt6);

                //DataTableUtilities.CopyColumns(dt1, dt5);
                //DataTableUtilities.CopyColumns(dt2, dt6);


                //GetTableBack(dt5, dt1Dict);
                //GetTableBack(dt6, dt2Dict);
                //dt1.Rows.Clear();
                //dt2.Rows.Clear();
                //foreach (DataRow row1 in dt5.Rows)
                //{
                //    dt1.Rows.Add(row1.ItemArray);
                //}
                //foreach (DataRow row1 in dt6.Rows)
                //{
                //    dt2.Rows.Add(row1.ItemArray);
                //}

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }


        private static Dictionary<string, List<DataRow>> GetDictForFastCompare(DataTable dt, List<string> columns)
        {
            Dictionary<string, List<DataRow>> dtDict = new Dictionary<string, List<DataRow>>();
            try
            {
                //the rowID will act as primaryKey for the datatable passed..
                if (!dt.Columns.Contains(ReconConstants.PrimaryKey))
                {
                    dt.Columns.Add(ReconConstants.PrimaryKey);
                }
                int rowID = 0;

                foreach (DataRow row1 in dt.Rows)
                {

                    row1[ReconConstants.PrimaryKey] = rowID;
                    rowID++;
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

                dt.PrimaryKey = new DataColumn[] { dt.Columns[ReconConstants.PrimaryKey] };
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dtDict;
        }



        public static void GenerateExceptionsReport(string exceptionFileName, AutomationEnum.FileFormat ExpReportFormat, List<ColumnInfo> selectedColumnList, List<ColumnInfo> sortByColumnList, List<ColumnInfo> groupByColumnList, bool isReconReportToBeGenerated)
        {
            //Workbook exceptionReport = new Workbook();

            try
            {
                //generate schema for the exception report
                //DataTable dtExceptions = ReconUtilities.GetExceptionsDataTableSchema(selectedColumnList, sortByColumnList, groupByColumnList);
                //foreach (KeyValuePair<DataRow, DataRow> kp in _dictRowMapping)
                //{
                //    DataRow row1 = kp.Key;
                //    DataRow row2 = kp.Value;
                //    AddToExceptionsDatatable(row1, row2, dtExceptions, comparisonFields);
                //}

                if (_dtExceptions.Rows.Count > 0 && isReconReportToBeGenerated == true)
                {
                    _dtExceptions.AcceptChanges();
                    ReconUtilities.GenerateExceptionsReport(_dtExceptions, exceptionFileName, ExpReportFormat, selectedColumnList, sortByColumnList, groupByColumnList);
                }

                //Modified By: Surendra Bisht
                //Modification date: Dec 31, 2013
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3036
                else if (isReconReportToBeGenerated == true)
                {
                    DataRow row = _dtExceptions.NewRow();

                    for (int i = 0; i < _dtExceptions.Columns.Count; i++)
                        row[i] = string.Empty;
                    if (_dtExceptions.Columns.Contains("MismatchType"))
                        row[_dtExceptions.Columns["MismatchType"].Ordinal] = "App data in Sync";

                    _dtExceptions.Rows.Add(row);
                    _dtExceptions.AcceptChanges();
                    ReconUtilities.GenerateExceptionsReport(_dtExceptions, exceptionFileName, ExpReportFormat, selectedColumnList, sortByColumnList, groupByColumnList);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                ClearExceptionsDataTable();
            }


            // return exceptionReport;

        }

        public static void ClearExceptionsDataTable()
        {
            try
            {
                _dtExceptions.Rows.Clear();
                _dtExceptions.Columns.Clear();
                _isSchemaCreated = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }



        // send selected columns list and numeric columns list...

        public static void GenerateExceptionsDataTableSchema(List<ColumnInfo> listSelectedColumn)
        {
            try
            {
                ClearExceptionsDataTable();
                //condition removed of isSchemaCreated because schema need to refreshed for each template
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-2032
                _dtExceptions = ReconUtilities.GetExceptionsDataTableSchema(listSelectedColumn);
                _isSchemaCreated = true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        //private  void GetTableBack(DataTable dt, Dictionary<string, List<DataRow>> dict)
        //{
        //    foreach (KeyValuePair<string,List< DataRow>> dictItem in dict)
        //    {
        //        if (dictItem.Value.Count > 1)
        //        { 

        //        }
        //        foreach (DataRow row in dictItem.Value)
        //        {
        //            dt.Rows.Add(row.ItemArray);
        //        }
        //    }
        //}
        #endregion

        public static Result IsRulePassed(DataRow row1, DataRow row2)
        {
            Result allCondiditionMatched = new Result();
            try
            {
                allCondiditionMatched.IsPassed = true;
                allCondiditionMatched.Text = String.Empty;
                allCondiditionMatched.MisMatchType = String.Empty;

                // The item with tolerance should be only one and last in the rule list
                // TODO: ENHANCEMENT REQUIRED !!!
                rulefieldCollection = MatchingRule.GetInstance().GetRuleFieldCollection();
                foreach (KeyValuePair<string, RuleParameters> ruleItem in rulefieldCollection)
                {
                    if (ruleItem.Value.IsIncluded)
                    {
                        string var1 = row1[ruleItem.Key].ToString().Trim().ToUpper();
                        string var2 = row2[ruleItem.Key].ToString().Trim().ToUpper();
                        DateTime dateValue;
                        double doubleValue;
                        //Narendra Kumar Jangir 2013 Mar 07
                        //DateTime.TryParse would be true for date
                        //double.TryParse is used to hadle doubles(like 1.18) which can be parsed in date
                        //http://jira.nirvanasolutions.com:8080/browse/LAZARD-6
                        if ((DateTime.TryParse(var1, out dateValue)) && (!double.TryParse(var1, out doubleValue)))
                        {
                            var1 = dateValue.Date.ToShortDateString();
                        }

                        if ((DateTime.TryParse(var2, out dateValue)) && (!double.TryParse(var2, out doubleValue)))
                        {
                            var2 = dateValue.Date.ToShortDateString();
                        }
                        Result result = new Result();
                        result.Text = string.Empty;
                        switch (ruleItem.Value.Type)
                        {
                            case ComparisionType.Exact:
                                result.IsPassed = ComparingLogic.ExactMatch(var1, var2);
                                break;
                            case ComparisionType.Partial:
                                result.IsPassed = ComparingLogic.PartialMatch(var1, var2);
                                break;
                            case ComparisionType.Numeric:
                                result = ComparingLogic.NumericMatch(ref var1, ref var2, ruleItem.Value);
                                row1[ruleItem.Key] = var1;
                                row2[ruleItem.Key] = var2;
                                if (result.IsPassed == true && result.Text != String.Empty)
                                {
                                    if (allCondiditionMatched.Text == String.Empty)
                                    {
                                        allCondiditionMatched.Text = result.Text;
                                    }
                                    else
                                    {
                                        allCondiditionMatched.Text += ", " + result.Text;
                                    }

                                    allCondiditionMatched.MisMatchType = result.MisMatchType;
                                }
                                break;
                        }

                        if (!result.IsPassed)
                        {
                            allCondiditionMatched.IsPassed = false;
                            allCondiditionMatched.Text = String.Empty;
                            allCondiditionMatched.MisMatchType = string.Empty;
                            break;
                        }


                    }
                }
                if (allCondiditionMatched.IsPassed && allCondiditionMatched.MisMatchType == String.Empty)
                {
                    allCondiditionMatched.MisMatchType = "Exactly Matched";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return allCondiditionMatched;
        }

        public static Result IsRulePartiallyPassed(DataRow row1, DataRow row2)
        {

            string text = String.Empty;
            //  List<string> unmatchedRules = new List<string>();
            Result allUnmatchedRules = new Result();
            allUnmatchedRules.MisMatchType = string.Empty;
            allUnmatchedRules.Text = string.Empty;
            rulefieldCollection = MatchingRule.GetInstance().GetRuleFieldCollection();
            foreach (KeyValuePair<string, RuleParameters> ruleItem in rulefieldCollection)
            {
                if (ruleItem.Value.IsIncluded)
                {
                    string var1 = string.Empty;
                    string var2 = string.Empty;
                    if (row1.Table.Columns.Contains(ruleItem.Key))
                        var1 = row1[ruleItem.Key].ToString().Trim().ToUpper();
                    if (row2.Table.Columns.Contains(ruleItem.Key))
                        var2 = row2[ruleItem.Key].ToString().Trim().ToUpper();

                    Result result = new Result();
                    result.IsPassed = false;
                    result.Text = String.Empty;

                    switch (ruleItem.Value.Type)
                    {
                        case ComparisionType.Exact:
                            result.IsPassed = ComparingLogic.ExactMatch(var1, var2);
                            break;
                        case ComparisionType.Partial:
                            result.IsPassed = ComparingLogic.PartialMatch(var1, var2);
                            break;
                        case ComparisionType.Numeric:
                            result = ComparingLogic.NumericMatch(ref var1, ref var2, ruleItem.Value);
                            break;
                    }
                    if (!result.IsPassed)
                    {
                        if (ruleItem.Key != "Symbol")
                        {

                            if (allUnmatchedRules.MisMatchType == string.Empty)
                            {
                                if (ruleItem.Key == "FundName")
                                {
                                    allUnmatchedRules.MisMatchType = "Allocation Mismatch";
                                }
                                else
                                {
                                    allUnmatchedRules.MisMatchType = ruleItem.Key + ' ' + "Mismatch";
                                }
                            }
                            else
                            {
                                allUnmatchedRules.MisMatchType = "Multiple Mismatches";
                            }
                            if (allUnmatchedRules.Text != string.Empty)
                            {
                                allUnmatchedRules.Text += ',';
                            }
                            allUnmatchedRules.Text += ruleItem.Key + ' ';
                        }
                    }
                }
            }

            if (allUnmatchedRules.Text != string.Empty)
            {
                allUnmatchedRules.Text += "Mismatch with PB";
            }
            return allUnmatchedRules;

        }


    }
}