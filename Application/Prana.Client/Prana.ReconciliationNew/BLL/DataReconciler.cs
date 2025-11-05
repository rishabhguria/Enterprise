using Prana.BusinessObjects;
//using Infragistics.Documents.Excel;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Prana.ReconciliationNew
{
    public class DataReconciler
    {
        static Dictionary<string, RuleParameters> rulefieldCollection = new Dictionary<string, RuleParameters>();
        private static void RemoveBreaks(DataTable dtExceptions)
        {
            try
            {
                DataRow row;
                for (int rowno = 0; rowno < dtExceptions.Rows.Count; rowno++)
                {
                    row = dtExceptions.Rows[rowno];
                    if (dtExceptions.Columns.Contains(ReconConstants.MismatchType))
                    {
                        if (row[ReconConstants.MismatchType].Equals(ReconConstants.CAPTION_MissingData))
                        {
                            dtExceptions.Rows[rowno].Delete();
                            rowno = 0;
                        }
                        dtExceptions.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static DataTable Reconcile(DataTable dtAppDataToMatch, DataTable dtPBDataToMatch, out DataTable dtAppDataMatched, out DataTable dtPBDataMatched, MatchingRule rule, bool includeMatchedInExceptions, bool includeToleranceMatchedInExceptions, List<ColumnInfo> listExceptionReportColumns, bool isClearExpCache, ReconType reconType, ref string errMsg, bool isReconHasgroupping, HashSet<string> hSetCommonColumn)
        {

            DataTable dtExceptions = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            dtAppDataMatched = new DataTable();
            dtPBDataMatched = new DataTable();
            try
            {
                if (dtAppDataMatched != null && dtAppDataToMatch != null && dtPBDataMatched != null && dtPBDataToMatch != null && rule != null && listExceptionReportColumns != null)
                {

                    SetDataTableSchema(rule, listExceptionReportColumns, isClearExpCache, reconType, dtExceptions, isReconHasgroupping);
                    //Narendra Kumar Jangir 2012 Oct 30
                    //validate matching rules for both Nirvana and PB data
                    //TODO: Here validation should be done using xsd
                    StringBuilder ReconErrorMessage = rule.ValidateMatchingRules(dtAppDataToMatch, dtPBDataToMatch);
                    //if ReconErrorMessage is empty than move to reconcile process else show the proper error message
                    if (string.IsNullOrEmpty(ReconErrorMessage.ToString()))
                    {
                        //if (isClearExpCache)
                        //{
                        //    _dtExceptions.Rows.Clear();
                        //}


                        List<string> lstCommonColumns = GetListCommonColumns(listExceptionReportColumns);
                        AddColumnsInDataTables(dtAppDataToMatch, rule, lstCommonColumns);
                        AddColumnsInDataTables(dtPBDataToMatch, rule, lstCommonColumns);

                        #region Fast Compare
                        //Add exactly matched data to the exception data table
                        FastCompare(dtAppDataToMatch, dtPBDataToMatch, ref dt3, ref dt4, rule, includeMatchedInExceptions, listExceptionReportColumns, dtExceptions);
                        #endregion

                        CompareRowsAndAddInDataTable(dtAppDataToMatch, dtPBDataToMatch, rule, hSetCommonColumn, includeMatchedInExceptions, includeToleranceMatchedInExceptions, listExceptionReportColumns, dtExceptions, dt3, dt4, lstCommonColumns);
                    }
                    else
                    {
                        //Message box should be shown in UI control instead of background classes
                        errMsg = ReconErrorMessage.ToString();
                    }
                    BackupNirvanaValues(dtExceptions);
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
            dtAppDataMatched = dt3;
            dtPBDataMatched = dt4;
            if (dtAppDataToMatch != null)
            {
                dtAppDataToMatch.AcceptChanges();
            }
            if (dtPBDataMatched != null)
            {
                dtPBDataMatched.AcceptChanges();
            }
            if (dtPBDataToMatch != null)
            {
                dtPBDataToMatch.AcceptChanges();
            }
            if (dtAppDataMatched != null)
            {
                dtAppDataMatched.AcceptChanges();
            }
            //dtAppMisMatchedData = dtAppDataToMatch;
            //dtPBMisMatchedData = dtPBDataToMatch;
            return dtExceptions;
        }

        /// <summary>
        /// This method will compare the rows for Multiple Mismatch or match by tolerance
        /// </summary>
        /// <param name="dtAppDataToMatch"></param>
        /// <param name="dtPBDataToMatch"></param>
        /// <param name="rule"></param>
        /// <param name="hSetCommonColumn"></param>
        /// <param name="includeMatchedInExceptions"></param>
        /// <param name="includeTolMatchedInExcpetions"></param>
        /// <param name="listExceptionReportColumns"></param>
        /// <param name="dtExceptions"></param>
        /// <param name="dt3"></param>
        /// <param name="dt4"></param>
        /// <param name="lstCommonColumns"></param>
       private static void CompareRowsAndAddInDataTable(DataTable dtAppDataToMatch, DataTable dtPBDataToMatch, MatchingRule rule, HashSet<string> hSetCommonColumn, bool includeMatchedInExceptions, bool includeTolMatchedInExcpetions, List<ColumnInfo> listExceptionReportColumns, DataTable dtExceptions, DataTable dt3, DataTable dt4, List<string> lstCommonColumns)
        {
            try
            {
                List<string> columns = rule.ComparisonFields;
                //Add data matched with the defined rules to the exception data table
                Dictionary<string, List<DataRow>> dt1Dict = GetDictNew(dtAppDataToMatch, columns, hSetCommonColumn);
                Dictionary<string, List<DataRow>> dt2Dict = GetDictNew(dtPBDataToMatch, columns, hSetCommonColumn);
                //  bool isRulePassed = false;
                foreach (KeyValuePair<string, List<DataRow>> dt1Value in dt1Dict)
                {
                    //for now the comparisonkey is kept as symbol...
                    string comparisonKey = dt1Value.Key;

                    bool isKeyMissing = false;

                    if (!string.IsNullOrEmpty(comparisonKey) && comparisonKey[comparisonKey.Length - 1] == ':' || comparisonKey.Contains("::::"))
                    {
                        isKeyMissing = true;

                    }

                    if (dt2Dict.ContainsKey(comparisonKey) && !isKeyMissing)//check wether key present in PB data or not
                    {
                        List<DataRow> rowsPB = new List<DataRow>();
                        rowsPB.AddRange(dt2Dict[comparisonKey]);
                        Dictionary<string, RuleParameters> inculdedRulefieldCollection = rule.GetInculdedRuleFieldCollection();

                        //Generate the list of numeric parameters
                        List<string> numericParameterNameLst = new List<string>();
                        foreach (string column in columns)
                        {

                            if (!hSetCommonColumn.Contains(column) && dtPBDataToMatch.Columns.Contains(column) && inculdedRulefieldCollection[column].Type.ToString() == "Numeric")
                            {
                                numericParameterNameLst.Add(column);
                            }
                        }
                        if (numericParameterNameLst.Count > 0)
                        {
                            #region New logic
                            string numericParameterName = numericParameterNameLst[0];
                            Dictionary<double, List<DataRow>> parameterValuesDict = CreateNumericParameterDictionary(numericParameterName, rowsPB);

                            foreach (DataRow row1 in dt1Value.Value)
                            {
                                bool isRow1Matched = false;
                                double parameterValue = 0;
                                if (!row1.Table.Columns.Contains(numericParameterName))
                                {
                                    continue;
                                }
                                double row1ParamValue = double.Parse(row1[numericParameterName].ToString());
                                parameterValue = SetNumericRulesParameterValue(inculdedRulefieldCollection, numericParameterName, parameterValue, row1ParamValue);
                                List<double> lstParam = new List<double>();

                                //Check for parameter value matched with PB data value
                                foreach (KeyValuePair<double, List<DataRow>> kvp in parameterValuesDict)
                                {
                                    if (kvp.Key >= row1ParamValue - parameterValue && kvp.Key <= row1ParamValue + parameterValue)
                                    {
                                        lstParam.Add(kvp.Key);
                                    }
                                }
                                bool isIterationRequired = true;
                                if (lstParam.Count == 0)
                                {
                                    lstParam.Add(parameterValuesDict.Keys.Max());
                                    isIterationRequired = false;
                                }
                                foreach (double paramValue in lstParam)//Compare the data with each matched value
                                {
                                    double row1ParamVal = paramValue;
                                    DataRow row2;
                                    if (!parameterValuesDict.ContainsKey(row1ParamVal))
                                    {
                                        continue;
                                    }

                                    List<DataRow> paramMatchedDataRow = new List<DataRow>();
                                    paramMatchedDataRow = parameterValuesDict[row1ParamVal];
                                    bool isMatched = true;
                                    //Check for the next numeric parameter and filter the comparison rows.
                                    if (numericParameterNameLst.Count > 1 && paramMatchedDataRow.Count > 1 && isIterationRequired)
                                    {
                                        for (int i = 1; i < numericParameterNameLst.Count; i++)
                                        {
                                            paramMatchedDataRow = GetNextParameterMatchedDataRows(inculdedRulefieldCollection, row1, numericParameterNameLst[i], paramMatchedDataRow, ref isIterationRequired, ref isMatched);
                                        }
                                    }

                                    if (!isMatched)
                                    {
                                        paramMatchedDataRow = new List<DataRow>();
                                        paramMatchedDataRow.Add(parameterValuesDict[row1ParamVal][parameterValuesDict[row1ParamVal].Count - 1]);
                                    }

                                    //Comapare the data, add the match with tolerance or mismatch details.
                                    for (int i = 0; i < paramMatchedDataRow.Count; ++i)
                                    {
                                        row2 = paramMatchedDataRow[i];
                                        List<Result> lstResult = IsRulePassed(rule, row1, row2);
                                        Tuple<Result, DataRow, DataRow> tpl = GetFinalResult(rule, row1, row2, lstResult);
                                        Result finalResult = tpl.Item1;
                                        UpdateRows(row1, row2, tpl);

                                        if (finalResult.IsPassed)
                                        {
                                            AddMismatchValuesInRow(row1, row2, finalResult, false);
                                            // AddToRowMappingDictionary(row1, row2);
                                            //here finalresult.mismatchtype.contains("Integral matching") also needs to be checked, then only the condition will be true.

                                            if ((includeTolMatchedInExcpetions && (finalResult.MisMatchType != null && finalResult.MisMatchType.Contains("Matched by"))) || ((includeMatchedInExceptions && (finalResult.MisMatchType != null && finalResult.MisMatchType == ReconConstants.MismatchType_ExactlyMatched))))
                                            {
                                                AddToExceptionsDatatable(row1, row2, listExceptionReportColumns, rule.NumericFields, rule.ComparisonFields, dtExceptions);
                                            }
                                            dt3.Rows.Add(row1.ItemArray);
                                            dt4.Rows.Add(row2.ItemArray);
                                            dtAppDataToMatch.Rows.Remove(row1);
                                            dtPBDataToMatch.Rows.Remove(row2);
                                            parameterValuesDict[row1ParamVal].Remove(row2);
                                            isRow1Matched = true;

                                            break;
                                        }

                                        if (!isIterationRequired)
                                        {
                                            break;
                                        }

                                    }
                                    RemoveDeletedRows(rowsPB);
                                    if (isRow1Matched)
                                    {
                                        break;
                                    }
                                }
                            }
                            RemoveDeletedRows(dt1Value.Value);
                            FindAndAddMismatchRowsInDataTable(rule, listExceptionReportColumns, dtExceptions, dt1Value.Value, rowsPB);
                            FillReconStatusForMissingData(rule, listExceptionReportColumns, dtExceptions, lstCommonColumns, dt1Value.Value, ReconStatus.PBDataMissing, ReconConstants.MismatchReason_DataMissingPB);
                            FillReconStatusForMissingData(rule, listExceptionReportColumns, dtExceptions, lstCommonColumns, rowsPB, ReconStatus.NirvanaDataMissing, ReconConstants.MismatchReason_DataMissingNirvana);
                            dt2Dict.Remove(comparisonKey);
                            #endregion

                        }
                        else
                        {
                            #region old logic
                            foreach (DataRow row1 in dt1Value.Value)
                            {
                                foreach (DataRow row2 in rowsPB)
                                {
                                    List<Result> lstResult = IsRulePassed(rule, row1, row2);
                                    Tuple<Result, DataRow, DataRow> tpl = GetFinalResult(rule, row1, row2, lstResult);
                                    Result finalResult = tpl.Item1;
                                    UpdateRows(row1, row2, tpl);
                                    if (finalResult.IsPassed)
                                    {
                                        AddMismatchValuesInRow(row1, row2, finalResult, false);
                                        // AddToRowMappingDictionary(row1, row2);
                                        //here finalresult.mismatchtype.contains("Integral matching") also needs to be checked, then only the condition will be true.
                                        if ((includeTolMatchedInExcpetions && (finalResult.MisMatchType != null && finalResult.MisMatchType.Contains("Matched by"))) || ((includeMatchedInExceptions && (finalResult.MisMatchType != null && finalResult.MisMatchType == ReconConstants.MismatchType_ExactlyMatched))))
                                        {
                                            AddToExceptionsDatatable(row1, row2, listExceptionReportColumns, rule.NumericFields, rule.ComparisonFields, dtExceptions);
                                        }
                                        dt3.Rows.Add(row1.ItemArray);
                                        dt4.Rows.Add(row2.ItemArray);
                                        dtAppDataToMatch.Rows.Remove(row1);
                                        dtPBDataToMatch.Rows.Remove(row2);
                                        break;
                                    }
                                }
                                RemoveDeletedRows(rowsPB);
                            }
                            RemoveDeletedRows(dt1Value.Value);
                            FindAndAddMismatchRowsInDataTable(rule, listExceptionReportColumns, dtExceptions, dt1Value.Value, rowsPB);

                            FillReconStatusForMissingData(rule, listExceptionReportColumns, dtExceptions, lstCommonColumns, dt1Value.Value, ReconStatus.PBDataMissing, ReconConstants.MismatchReason_DataMissingPB);
                            FillReconStatusForMissingData(rule, listExceptionReportColumns, dtExceptions, lstCommonColumns, rowsPB, ReconStatus.NirvanaDataMissing, ReconConstants.MismatchReason_DataMissingNirvana);
                            dt2Dict.Remove(comparisonKey);
                            #endregion
                        }
                    }
                    else
                    {
                        AddNirvanaRowsInDataTable(rule, listExceptionReportColumns, dtExceptions, lstCommonColumns, dt1Value.Value);
                    }
                }
                //the remaining rows in primeBroker data has no corresponding row in nirvana data...
                AddPBRowsInDataTable(rule, listExceptionReportColumns, dtExceptions, lstCommonColumns, dt2Dict);
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
        /// Create the dictionary for next numeric parameter and match for next Paramter value. 
        /// </summary>
        /// <param name="inculdedRulefieldCollection"></param>
        /// <param name="row1"></param>
        /// <param name="numericParameterName"></param>
        /// <param name="rowsPB"></param>
        /// <param name="isIterationRequired"></param>
        /// <returns></returns>
        private static List<DataRow> GetNextParameterMatchedDataRows(Dictionary<string, RuleParameters> inculdedRulefieldCollection, DataRow row1, string numericParameterName, List<DataRow> rowsPB, ref bool isIterationRequired, ref bool isMatched)
        {
            List<DataRow> paramMatchedDataRow2 = new List<DataRow>();
            try
            {
                if (!isIterationRequired)
                    return rowsPB;
                Dictionary<double, List<DataRow>> parameterValuesDict = CreateNumericParameterDictionary(numericParameterName, rowsPB);

                double parameterValue = 0;
                if (!row1.Table.Columns.Contains(numericParameterName))
                {
                    return rowsPB;
                }
                double row1ParamValue = double.Parse(row1[numericParameterName].ToString());
                parameterValue = SetNumericRulesParameterValue(inculdedRulefieldCollection, numericParameterName, parameterValue, row1ParamValue);
                List<double> lstParam = new List<double>();
                foreach (KeyValuePair<double, List<DataRow>> kvp in parameterValuesDict)
                {
                    if (kvp.Key >= row1ParamValue - parameterValue && kvp.Key <= row1ParamValue + parameterValue)
                    {
                        lstParam.Add(kvp.Key);
                    }
                }
                if (lstParam.Count == 0)
                {
                    //  lstParam.Add(parameterValuesDict.Keys.Max());
                    isIterationRequired = false;
                    isMatched = false;
                    return parameterValuesDict[parameterValuesDict.Keys.Max()];
                }
                foreach (double value in lstParam)
                {
                    paramMatchedDataRow2.AddRange(parameterValuesDict[value]);
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
            return paramMatchedDataRow2;
        }

        /// <summary>
        /// Set the param value based Match with tolerance [Absolute Tolerance, Percentage Tolerance etc] 
        /// </summary>
        /// <param name="inculdedRulefieldCollection"></param>
        /// <param name="numericParameterName"></param>
        /// <param name="parameterValue"></param>
        /// <param name="row1ParamValue"></param>
        /// <returns></returns>
        private static double SetNumericRulesParameterValue(Dictionary<string, RuleParameters> inculdedRulefieldCollection, string numericParameterName, double parameterValue, double row1ParamValue)
        {
            try
            {
                if (inculdedRulefieldCollection.ContainsKey(numericParameterName) && inculdedRulefieldCollection[numericParameterName].IsAbsoluteDifference && inculdedRulefieldCollection[numericParameterName].IsPercentMatch)
                {
                    double absDiff = double.Parse(inculdedRulefieldCollection[numericParameterName].AbsDiff.ToString());
                    double perTol = inculdedRulefieldCollection[numericParameterName].ErrorTolerance;
                    double tolValue = (perTol * row1ParamValue) / 100;
                    if (absDiff > tolValue)
                    {
                        parameterValue = absDiff;
                    }
                    else
                    {
                        parameterValue = tolValue;
                    }
                }
                else if (inculdedRulefieldCollection.ContainsKey(numericParameterName) && inculdedRulefieldCollection[numericParameterName].IsAbsoluteDifference)
                {
                    parameterValue = double.Parse(inculdedRulefieldCollection[numericParameterName].AbsDiff.ToString());
                }
                else if (inculdedRulefieldCollection.ContainsKey(numericParameterName) && inculdedRulefieldCollection[numericParameterName].IsPercentMatch)
                {
                    double perTol = inculdedRulefieldCollection[numericParameterName].ErrorTolerance;
                    parameterValue = (perTol * row1ParamValue) / 100;
                }
                else
                {
                    parameterValue = 0;
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
            return parameterValue;
        }

        /// <summary>
        /// Create the Parameter Dictionary [Create the dictionary of the distinct numeric value and list of data rows.
        /// For example, 10,20,30 are distinct numeric value and there are 15 rows with 10 value and 25 rows with 20 value 
        /// and 10 rows with 30 value. then create a dictionary like 10 --> 15 rows, 20 --> 25 rows , 30 --> 10 rows. ]
        /// </summary>
        /// <param name="numericParameterName"></param>
        /// <param name="rowsPB"></param>
        /// <returns></returns>
        private static Dictionary<double, List<DataRow>> CreateNumericParameterDictionary(string numericParameterName, List<DataRow> rowsPB)
        {
            Dictionary<double, List<DataRow>> parameterValuesDict = new Dictionary<double, List<DataRow>>();
            try
            {
                for (int rowIndex = 0; rowIndex < rowsPB.Count; ++rowIndex)
                {
                    double paramValue = double.Parse(rowsPB[rowIndex][numericParameterName].ToString());
                    if (parameterValuesDict.ContainsKey(paramValue))
                    {
                        parameterValuesDict[paramValue].Add(rowsPB[rowIndex]);
                    }
                    else
                    {
                        List<DataRow> lst = new List<DataRow>();
                        lst.Add(rowsPB[rowIndex]);
                        parameterValuesDict.Add(paramValue, lst);
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
            return parameterValuesDict;
        }

        private static void UpdateRows(DataRow row1, DataRow row2, Tuple<Result, DataRow, DataRow> tpl)
        {
            row1 = tpl.Item2;
            row2 = tpl.Item3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rule"></param>
        /// <param name="lstCommonColumns"></param>
        private static void AddColumnsInDataTables(DataTable dt, MatchingRule rule, List<string> lstCommonColumns)
        {
            try
            {
                #region add new MisMatch status columns
                AddMisMatchColumnsInDataTable(dt);
                #endregion

                #region add new recon status columns
                AddReconColumnsInDataTable(dt, rule, lstCommonColumns);
                #endregion
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
        /// 
        /// </summary>
        /// <param name="listExceptionReportColumns"></param>
        /// <returns></returns>
        private static List<string> GetListCommonColumns(List<ColumnInfo> listExceptionReportColumns)
        {
            //This list will prevent to add recon status columns
            List<string> lstCommonColumns = new List<string>();
            try
            {
                #region add common columns to list
                foreach (ColumnInfo column in listExceptionReportColumns)
                {
                    if (column.GroupType.Equals(ColumnGroupType.Common))
                    {
                        lstCommonColumns.Add(column.ColumnName);
                    }
                }
                #endregion
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
            return lstCommonColumns;
        }

        /// <summary>
        /// it means that there is no corresponding row for this symbol in primeBroker datatable..
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="listExceptionReportColumns"></param>
        /// <param name="dtExceptions"></param>
        /// <param name="lstCommonColumns"></param>
        /// <param name="rowsApp"></param>
        private static void AddNirvanaRowsInDataTable(MatchingRule rule, List<ColumnInfo> listExceptionReportColumns, DataTable dtExceptions, List<string> lstCommonColumns, List<DataRow> rowsApp)
        {
            try
            {
                foreach (DataRow dr in rowsApp)
                {
                    dr[ReconConstants.MismatchReason] = ReconConstants.MismatchReason_DataMissingPB;
                    dr[ReconConstants.MismatchType] = ReconConstants.CAPTION_MissingData;
                    foreach (string field in rule.ComparisonFields)
                    {
                        //No need to store original value,recon status, tolerance type, tolerance value for the common columns
                        if (!lstCommonColumns.Contains(field))
                        {
                            FillMissingStatusValue(dr, field, ReconStatus.PBDataMissing);
                        }
                    }
                    // the corresponding primeBroker row is null..                               
                    AddToExceptionsDatatable(dr, null, listExceptionReportColumns, rule.NumericFields, rule.ComparisonFields, dtExceptions);
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
        /// the remaining rows in primeBroker data has no corresponding row in nirvana data...
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="listExceptionReportColumns"></param>
        /// <param name="dtExceptions"></param>
        /// <param name="lstCommonColumns"></param>
        /// <param name="dt2Dict"></param>
        private static void AddPBRowsInDataTable(MatchingRule rule, List<ColumnInfo> listExceptionReportColumns, DataTable dtExceptions, List<string> lstCommonColumns, Dictionary<string, List<DataRow>> dt2Dict)
        {
            try
            {
                if (dt2Dict.Values.Count > 0)
                {
                    foreach (KeyValuePair<string, List<DataRow>> kp in dt2Dict)
                    {
                        List<DataRow> listRowsSymbolNotFound = kp.Value;

                        foreach (DataRow dr in listRowsSymbolNotFound)
                        {
                            dr[ReconConstants.MismatchReason] = ReconConstants.MismatchReason_DataMissingNirvana;
                            dr[ReconConstants.MismatchType] = ReconConstants.CAPTION_MissingData;

                            foreach (string field in rule.ComparisonFields)
                            {
                                //No need to store original value,recon status, tolerance type, tolerance value for the common columns
                                if (!lstCommonColumns.Contains(field))
                                {
                                    FillMissingStatusValue(dr, field, ReconStatus.NirvanaDataMissing);
                                }
                            }
                            // the corresponding nirvana row is null...
                            //AddToRowMappingDictionary(dt3.NewRow(), dr2);
                            AddToExceptionsDatatable(null, dr, listExceptionReportColumns, rule.NumericFields, rule.ComparisonFields, dtExceptions);
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
        /// Used to find whether row2 is perfect mismatch row for row1
        /// </summary>
        /// <param name="dr1"></param>
        /// <param name="ExactmismatchReconFields"></param>
        /// <param name="dr2"></param>
        /// <returns></returns>

        public static Boolean isPerfectMismatchRow(DataRow dr1, Dictionary<String, ComparisionType> ExactmismatchReconFields, DataRow dr2)
        {
            try
            {
                bool isMatch = true;
                foreach (KeyValuePair<String, ComparisionType> k in ExactmismatchReconFields)
                {
                    var var1 = dr1[k.Key].ToString().Trim().ToUpper();
                    var var2 = dr2[k.Key].ToString().Trim().ToUpper();
                    isMatch = ExactMatch(var1, var2);
                    if (!isMatch)
                        break;
                }
                return isMatch;
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
            return false;
        }
        /// <summary>
        /// if there are any unmatched rows left then we need to find the reason for mismatch...
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="listExceptionReportColumns"></param>
        /// <param name="dtExceptions"></param>
        /// <param name="rowsApp"></param>
        /// <param name="rowsPB"></param>
        private static void FindAndAddMismatchRowsInDataTable(MatchingRule rule, List<ColumnInfo> listExceptionReportColumns, DataTable dtExceptions, List<DataRow> rowsApp, List<DataRow> rowsPB)
        {
            try
            {
                if (rowsApp.Count > 0 && rowsPB.Count > 0)
                {
                    Dictionary<String, ComparisionType> ExactmismatchReconFields = rule.ExactmismatchReconFields;
                    foreach (DataRow row1 in rowsApp)
                    {

                        foreach (DataRow row2 in rowsPB)
                        {
                            // we will match pbrow2match with  row1 of Nirvanadata
                            DataRow pbrow2match = row2;

                            //check If exact mismatch required ? Otherwise the regular flow will work.
                            if (ExactmismatchReconFields.Count > 0)
                            {
                                // Get the correct pb row to match with nirvana data.
                                pbrow2match = rowsPB.Find(delegate (DataRow dr)
                                   {
                                       return isPerfectMismatchRow(dr, ExactmismatchReconFields, row1);

                                   });

                                //thi extra check needed if we didn't get the appropriate pb row for the nirvana data then we prefer current pbrow but checking here if this current row can match with any other nirvana data.
                                if (pbrow2match == null)
                                {
                                    DataRow approwmatchingwithpb = rowsApp.Find(delegate (DataRow dr)
                                    {
                                        return isPerfectMismatchRow(dr, ExactmismatchReconFields, row2);
                                    });
                                    if (approwmatchingwithpb != null)
                                    {
                                        continue;
                                    }

                                    pbrow2match = row2;
                                }
                            }

                            List<Result> lstResult = IsRulePartiallyPassed(rule, row1, pbrow2match, listExceptionReportColumns);
                            Result finalResult = GetFinalResultWithMismatchReasons(rule, row1, pbrow2match, lstResult);
                            if (!string.IsNullOrWhiteSpace(finalResult.Text))
                            {
                                //Check if Last character is a Comma. If it is so. Remove that from final result.
                                if (finalResult.Text[(finalResult.Text.Length) - 1] == ',')
                                {
                                    finalResult.Text = finalResult.Text.Substring(0, (finalResult.Text.Length) - 1);
                                }
                                AddMismatchValuesInRow(row1, pbrow2match, finalResult, true);
                                //AddToRowMappingDictionary(row1, row2);
                                AddToExceptionsDatatable(row1, pbrow2match, listExceptionReportColumns, rule.NumericFields, rule.ComparisonFields, dtExceptions);
                                break;
                            }
                        }
                        RemoveMatchedRows(rowsPB);
                    }
                    RemoveMatchedRows(rowsApp);
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
        /// 
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        /// <param name="lstResult"></param>
        /// <returns></returns>
        private static Result GetFinalResultWithMismatchReasons(MatchingRule rule, DataRow row1, DataRow row2, List<Result> lstResult)
        {
            Result finalResult = new Result();
            try
            {
                foreach (Result result in lstResult)
                {
                    if (!result.IsPassed)
                    {
                        if (result.ColumnName != ReconConstants.COLUMN_Symbol)
                        {
                            if (string.IsNullOrEmpty(finalResult.MisMatchType))
                            {
                                if (result.ColumnName == ReconConstants.COLUMN_AccountName)
                                {
                                    finalResult.MisMatchType = ReconConstants.MismatchType_AllocationMismatch;
                                }
                                else
                                {
                                    finalResult.MisMatchType = result.ColumnName + ' ' + "Mismatch";
                                }
                            }
                            else
                            {
                                finalResult.MisMatchType = ReconConstants.MismatchType_MultipleMismatches;
                            }
                            finalResult.Text += result.ColumnName + ' ' + ReconConstants.MismatchType_MismatchWithPB;
                            if (!string.IsNullOrWhiteSpace(finalResult.Text))
                            {
                                finalResult.Text += ',';
                            }
                        }
                    }
                    AddToleranceValuesInRow(rule, row1, result);
                    AddToleranceValuesInRow(rule, row2, result);
                    //if (finalResult.Text != string.Empty)
                    //{
                    //    finalResult.Text += ReconConstants.MismatchType_MismatchwithPB;
                    //}
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
            return finalResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        /// <param name="lstResult"></param>
        /// <returns></returns>
        private static Tuple<Result, DataRow, DataRow> GetFinalResult(MatchingRule rule, DataRow row1, DataRow row2, List<Result> lstResult)
        {
            Result finalResult = new Result();
            try
            {
                finalResult.IsPassed = true;
                string multipleToleranceMismatchType = string.Empty;
                bool isMultipleToleranceAppiled = false;
                foreach (Result result in lstResult)
                {
                    if (result.IsPassed == true && !string.IsNullOrWhiteSpace(result.Text))
                    {
                        if (string.IsNullOrWhiteSpace(finalResult.Text))
                        {
                            finalResult.Text = result.Text;
                        }
                        else if (string.IsNullOrEmpty(finalResult.Text))
                        {
                            finalResult.Text += result.Text;
                        }
                        else if (!finalResult.Text.Contains(result.Text))
                        {
                            //Remove exactly matched tag if there is mismatch or tolerance match for the row
                            if (finalResult.Text.Contains(ReconConstants.MismatchType_ExactlyMatched))
                            {
                                finalResult.Text = finalResult.Text.Replace(ReconConstants.MismatchType_ExactlyMatched, string.Empty);
                                finalResult.Text += result.Text;
                                multipleToleranceMismatchType += "Matched by " + result.ToleranceType.ToString();
                            }
                            else if (!result.Text.Equals(ReconConstants.MismatchType_ExactlyMatched, StringComparison.InvariantCultureIgnoreCase))
                            {
                                finalResult.Text += ", " + result.Text;
                                if (!multipleToleranceMismatchType.Contains(result.ToleranceType.ToString()))
                                {
                                    multipleToleranceMismatchType += ", " + result.ToleranceType.ToString();
                                    isMultipleToleranceAppiled = true;
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(finalResult.MisMatchType))
                        {
                            finalResult.MisMatchType = result.MisMatchType;
                        }
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-900
                        //Recon: matched within tolerance showing "exactly matched" in general information
                        else if (finalResult.MisMatchType.Equals(ReconConstants.MismatchType_ExactlyMatched, StringComparison.InvariantCultureIgnoreCase) || !result.MisMatchType.Equals(ReconConstants.MismatchType_ExactlyMatched, StringComparison.InvariantCultureIgnoreCase))
                        {
                            finalResult.MisMatchType = result.MisMatchType;
                        }
                        if (isMultipleToleranceAppiled)
                        {
                            finalResult.MisMatchType = multipleToleranceMismatchType + " tolerance.";
                        }
                    }
                    if (finalResult.IsPassed && !result.IsPassed)
                    {
                        finalResult.IsPassed = result.IsPassed;
                    }

                    row1 = AddToleranceValuesInRow(rule, row1, result);
                    row2 = AddToleranceValuesInRow(rule, row2, result);

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
            return new Tuple<Result, DataRow, DataRow>(finalResult, row1, row2);
        }

        /// <summary>
        /// removing all matched rows with row state detached or deleted
        /// </summary>
        /// <param name="rows"></param>
        private static void RemoveDeletedRows(List<DataRow> rows)
        {
            try
            {
                rows.RemoveAll(delegate (DataRow dr)
                {
                    if (dr.RowState == DataRowState.Detached || dr.RowState == DataRowState.Deleted)
                    {
                        return true;
                    }
                    return false;
                });
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
        /// removing all matched rows..k
        /// </summary>
        /// <param name="rows"></param>
        private static void RemoveMatchedRows(List<DataRow> rows)
        {
            try
            {
                rows.RemoveAll(delegate (DataRow dr)
                {
                    if (!dr[ReconConstants.MismatchReason].Equals(System.DBNull.Value))
                    {
                        return true;

                    }
                    return false;
                });
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

        private static void FillReconStatusForMissingData(MatchingRule rule, List<ColumnInfo> listExceptionReportColumns, DataTable dtExceptions, List<string> lstCommonColumns, List<DataRow> rows, ReconStatus status, string mismatchReason)
        {
            try
            {
                foreach (DataRow dr in rows)
                {
                    dr[ReconConstants.MismatchReason] = mismatchReason;
                    dr[ReconConstants.MismatchType] = ReconConstants.CAPTION_MissingData;
                    foreach (string field in rule.ComparisonFields)
                    {
                        //No need to store original value,recon status, tolerance type, tolerance value for the common columns
                        if (!lstCommonColumns.Contains(field))
                        {
                            FillMissingStatusValue(dr, field, status);
                        }
                    }
                    if (mismatchReason == ReconConstants.MismatchReason_DataMissingNirvana)
                    {
                        // the corresponding row is null..                               
                        AddToExceptionsDatatable(null, dr, listExceptionReportColumns, rule.NumericFields, rule.ComparisonFields, dtExceptions);
                    }
                    else if (mismatchReason == ReconConstants.MismatchReason_DataMissingPB)
                    {
                        // the corresponding row is null..                               
                        AddToExceptionsDatatable(dr, null, listExceptionReportColumns, rule.NumericFields, rule.ComparisonFields, dtExceptions);
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

        private static void FillMissingStatusValue(DataRow dr, string field, ReconStatus status)
        {
            try
            {
                if (dr.Table.Columns.Contains(ReconConstants.CONST_ReconStatus + field))
                    dr[ReconConstants.CONST_ReconStatus + field] = status.ToString();
                if (dr.Table.Columns.Contains(ReconConstants.CONST_ToleranceType + field))
                    dr[ReconConstants.CONST_ToleranceType + field] = ToleranceType.None.ToString();
                if (dr.Table.Columns.Contains(ReconConstants.CONST_ToleranceValue + field))
                    dr[ReconConstants.CONST_ToleranceValue + field] = string.Empty;
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

        private static void AddMismatchValuesInRow(DataRow row1, DataRow row2, Result finalResult, bool isEmptyStatusToBeUpdated)
        {
            try
            {
                row1[ReconConstants.Matched] = row2[ReconConstants.PrimaryKey];
                row2[ReconConstants.Matched] = row1[ReconConstants.PrimaryKey];
                if (!string.IsNullOrWhiteSpace(finalResult.Text) || isEmptyStatusToBeUpdated)
                {
                    row1[ReconConstants.MismatchReason] = finalResult.Text;
                    row2[ReconConstants.MismatchReason] = finalResult.Text;
                }
                row1[ReconConstants.MismatchType] = finalResult.MisMatchType;
                row2[ReconConstants.MismatchType] = finalResult.MisMatchType;
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

        private static DataRow AddToleranceValuesInRow(MatchingRule rule, DataRow row, Result result)
        {
            try
            {
                if (row.Table.Columns.Contains(ReconConstants.CONST_ReconStatus + result.ColumnName))
                {
                    row[ReconConstants.CONST_ReconStatus + result.ColumnName] = result.ReconStaus.ToString();
                }
                if (rule.NumericFields.Contains(result.ColumnName))
                {
                    if (row.Table.Columns.Contains(ReconConstants.CONST_ToleranceType + result.ColumnName))
                    {
                        row[ReconConstants.CONST_ToleranceType + result.ColumnName] = result.ToleranceType.ToString();
                    }
                    if (row.Table.Columns.Contains(ReconConstants.CONST_ToleranceValue + result.ColumnName))
                    {
                        row[ReconConstants.CONST_ToleranceValue + result.ColumnName] = result.ToleranceValue;
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
            return row;
        }

        private static void AddReconColumnsInDataTable(DataTable dt, MatchingRule rule, List<string> lstCommonColumns)
        {
            try
            {
                foreach (string field in rule.ComparisonFields)
                {
                    //No need to store original value,recon status, tolerance type, tolerance value for the common columns
                    if (!lstCommonColumns.Contains(field))
                    {
                        //checks if column exist or not
                        if (!dt.Columns.Contains(ReconConstants.CONST_ReconStatus + field))
                        {
                            dt.Columns.Add(ReconConstants.CONST_ReconStatus + field);
                        }
                        if (rule.NumericFields.Contains(field))
                        {
                            if (!dt.Columns.Contains(ReconConstants.CONST_ToleranceType + field))
                            {
                                dt.Columns.Add(ReconConstants.CONST_ToleranceType + field);
                            }
                            if (!dt.Columns.Contains(ReconConstants.CONST_ToleranceValue + field))
                            {
                                dt.Columns.Add(ReconConstants.CONST_ToleranceValue + field);
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

        private static void AddMisMatchColumnsInDataTable(DataTable dt)
        {
            try
            {
                if (!dt.Columns.Contains(ReconConstants.Matched))
                {
                    dt.Columns.Add(ReconConstants.Matched);
                }
                if (!dt.Columns.Contains(ReconConstants.MismatchType))
                {
                    dt.Columns.Add(ReconConstants.MismatchType);
                    dt.Columns[ReconConstants.MismatchType].Caption = ReconConstants.CAPTION_MismatchType;
                }
                if (!dt.Columns.Contains(ReconConstants.MismatchReason))
                {
                    dt.Columns.Add(ReconConstants.MismatchReason);
                    dt.Columns[ReconConstants.MismatchReason].Caption = ReconConstants.CAPTION_MismatchReason;
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

        private static void SetDataTableSchema(MatchingRule rule, List<ColumnInfo> listExceptionReportColumns, bool isClearExpCache, ReconType reconType, DataTable dtExceptions, bool isReconHasgroupping)
        {
            try
            {
                if (isClearExpCache)
                    GenerateExceptionsDataTableSchema(listExceptionReportColumns, rule, dtExceptions, reconType, isReconHasgroupping);
                else
                    RemoveBreaks(dtExceptions);       // in case when we do multi-recon,remove breaks because the breaks are processed this time again.( Surendra) 
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

        private static void BackupNirvanaValues(DataTable dtExceptions)
        {
            try
            {
                foreach (DataRow row in dtExceptions.Rows)
                {
                    foreach (DataColumn column in dtExceptions.Columns)
                    {
                        string colName = string.Empty;
                        if (column.ColumnName.Length > ReconConstants.CONST_Nirvana.Length)
                        {
                            colName = column.ColumnName.Substring(ReconConstants.CONST_Nirvana.Length);
                        }
                        if (column.ColumnName.Contains(ReconConstants.CONST_Nirvana) && dtExceptions.Columns.Contains(ReconConstants.CONST_OriginalValue + colName) && dtExceptions.Columns.Contains(ReconConstants.CONST_Nirvana + colName))
                        {
                            row[ReconConstants.CONST_OriginalValue + colName] = row[ReconConstants.CONST_Nirvana + colName];
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

        private static Dictionary<string, List<DataRow>> GetDictNew(DataTable dt, List<string> columns, HashSet<string> hSetCommonColumn)
        {

            Dictionary<string, List<DataRow>> dtDict = new Dictionary<string, List<DataRow>>(StringComparer.OrdinalIgnoreCase);
            try
            {
                StringBuilder sbKey = new StringBuilder();
                DataColumnCollection dataColumn = dt.Columns;
                HashSet<string> hsKeyColumn = new HashSet<string>();

                foreach (string column in columns)
                {

                    if (hSetCommonColumn.Contains(column) && dataColumn.Contains(column))
                        hsKeyColumn.Add(column);
                }

                foreach (DataRow row1 in dt.Rows)
                {
                    sbKey.Clear();
                    foreach (string columnName in hsKeyColumn)
                    {
                        sbKey.Append("::").Append(row1[columnName].ToString());
                        // key = key + "::" + row1[str].ToString();
                    }

                    string key = sbKey.ToString();

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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            // dt.PrimaryKey = new DataColumn[] { dt.Columns[ReconConstants.PrimaryKey] };

            return dtDict;
        }

        private static bool ExactMatch(string var1, string var2)
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        private static bool PartialMatch(string var1, string var2)
        {
            try
            {
                if ((!string.IsNullOrWhiteSpace(var1) && !string.IsNullOrWhiteSpace(var2)) &&
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
                if ((!string.IsNullOrWhiteSpace(var1) && !string.IsNullOrWhiteSpace(var2)) &&
                    (var1.Contains(var2) || var2.Contains(var1)))
                {
                    return true;
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
            return false;
        }

        private static Result NumericMatch(ref string var1, ref string var2, RuleParameters ruleParameters)
        {
            Result result = new Result();
            result.Text = String.Empty;
            result.MisMatchType = string.Empty;
            result.IsPassed = false;
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
                bool isApplyToleranceOnReconReport = false;
                bool.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_IsApplyToleranceOnReconReport), out isApplyToleranceOnReconReport);
                if (double.TryParse(temp1, out num1) && double.TryParse(temp2, out num2))
                {
                    num1 = double.Parse(temp1);
                    num2 = double.Parse(temp2);

                    if (num1.Equals(num2))
                    {
                        result.IsPassed = true;
                        //result.Text = ReconConstants.MismatchType_ExactlyMatched;
                        result.MisMatchType = ReconConstants.MismatchType_ExactlyMatched;
                        result.ReconStaus = ReconStatus.ExactlyMatched;
                        // return result;
                    }
                    else if (ruleParameters.IsRoundOff)
                    {
                        double int1 = System.Math.Round(num1, ruleParameters.RoundDigits);
                        double int2 = System.Math.Round(num2, ruleParameters.RoundDigits);
                        if (isApplyToleranceOnReconReport)
                        {
                            // http://jira.nirvanasolutions.com:8080/browse/CHMW-1407
                            var1 = int1.ToString();
                            var2 = int2.ToString();
                        }
                        if (int1.Equals(int2))
                        {
                            result.IsPassed = true;
                            result.Text = ruleParameters.FieldName + "(" + "Rounded to " + ruleParameters.RoundDigits + " significant Digits)";
                            result.MisMatchType = "Matched by roundoff tolerance";
                            result.ToleranceType = ToleranceType.RoundOff;
                            result.ToleranceValue = ruleParameters.RoundDigits;
                            result.ReconStaus = ReconStatus.MatchedWithInTolerance;
                            //return result;
                        }
                    }
                    else if (ruleParameters.IsIntegralMatch)
                    {
                        double int1 = System.Math.Floor(num1);
                        double int2 = System.Math.Floor(num2);
                        if (isApplyToleranceOnReconReport)
                        {
                            // http://jira.nirvanasolutions.com:8080/browse/CHMW-1407
                            var1 = int1.ToString();
                            var2 = int2.ToString();
                        }
                        if (int1.Equals(int2))
                        {
                            result.MisMatchType = "Matched by integral tolerance";
                            result.IsPassed = true;
                            result.Text = ruleParameters.FieldName + "(" + "Integral" + ")";
                            result.ToleranceType = ToleranceType.Integral;
                            result.ReconStaus = ReconStatus.MatchedWithInTolerance;
                            // return result;
                        }
                    }
                    else if (ruleParameters.IsPercentMatch)
                    {
                        double errorPercent = System.Math.Abs(((num1 - num2) / num2) * 100);
                        if (errorPercent <= ruleParameters.ErrorTolerance)
                        {
                            result.IsPassed = true;
                            result.Text = ruleParameters.FieldName + "(" + errorPercent.ToString("#.00") + " %" + ")";
                            result.MisMatchType = "Matched by tolerance";
                            result.ToleranceType = ToleranceType.Percentage;
                            result.ToleranceValue = Convert.ToInt32(errorPercent);
                            result.ReconStaus = ReconStatus.MatchedWithInTolerance;
                        }
                    }
                    else if (ruleParameters.IsAbsoluteDifference)
                    {
                        double AbsDiff = System.Math.Abs(num1 - num2);
                        if (AbsDiff <= ruleParameters.AbsDiff)
                        {
                            result.IsPassed = true;
                            result.Text = ruleParameters.FieldName + "(" + "Matched with " + AbsDiff + " Absolute Tolerance)";
                            result.MisMatchType = "Matched by absolute tolerance";
                            result.ToleranceType = ToleranceType.Absolute;
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-1406
                            result.ToleranceValue = AbsDiff;
                            result.ReconStaus = ReconStatus.MatchedWithInTolerance;
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
            return result;
        }

        private static void AddToExceptionsDatatable(DataRow appRow, DataRow Pbrow, List<ColumnInfo> listSelectedColumns, List<String> numericColumns, List<String> listComparisonColumns, DataTable dtExceptions)
        {
            //ReconUtilities.NumericColumn = numericColumns;
            try
            {
                DataRow dataRowExceptions = dtExceptions.NewRow();

                foreach (ColumnInfo column in listSelectedColumns)
                {
                    string columnKeyNewNirvana = string.Empty;
                    string columnKeyNewBroker = string.Empty;
                    string DiffColumnKey = string.Empty;
                    string CommonColumnKey = string.Empty;

                    switch (column.GroupType)
                    {
                        case ColumnGroupType.Nirvana:
                            columnKeyNewNirvana = ReconConstants.CONST_Nirvana + column.ColumnName;
                            break;
                        case ColumnGroupType.PrimeBroker:
                            columnKeyNewBroker = ReconConstants.CONST_Broker + column.ColumnName;
                            break;
                        case ColumnGroupType.Common:
                            CommonColumnKey = column.ColumnName;
                            break;
                        case ColumnGroupType.Both:
                            break;
                        case ColumnGroupType.Diff:
                            DiffColumnKey = ReconConstants.CONST_Diff + column.ColumnName;
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
                        if (appRow == null && dataRowExceptions.Table.Columns.Contains(columnKeyNewNirvana) && numericColumns.Contains(column.ColumnName))    // surendra
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
                        if (Pbrow == null && dataRowExceptions.Table.Columns.Contains(columnKeyNewBroker) && numericColumns.Contains(column.ColumnName))    // surendra
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
                        //TDOD: Round to 4 decimal is hardcoded, need to make it generic
                        // http://jira.nirvanasolutions.com:8080/browse/CHMW-2337
                        //int RoundToDecimal = 4;

                        if (dataRowExceptions.Table.Columns.Contains(DiffColumnKey))
                        {
                            if (appRow != null && appRow.Table.Columns.Contains(column.ColumnName))
                            {
                                if (Pbrow != null && Pbrow.Table.Columns.Contains(column.ColumnName))
                                {
                                    //  if (Convert.ToDouble(Pbrow[column.ColumnName]) == 0.0)   // surendra   for if pb data present, but Zero. 
                                    //    dataRowExceptions[DiffColumnKey] = 0;                  //surendra         then diffcolumn=0
                                    //else
                                    decimal value1 = 0;
                                    decimal value2 = 0;
                                    if (decimal.TryParse(appRow[column.ColumnName].ToString(), out value1) && decimal.TryParse(Pbrow[column.ColumnName].ToString(), out value2))
                                    {
                                        //dataRowExceptions[DiffColumnKey] = Math.Round((value1 - value2), RoundToDecimal);
                                        //decimal v = value1 - value2;
                                        dataRowExceptions[DiffColumnKey] = (value1 - value2);
                                    }
                                }
                                else if ((appRow.Table.Columns.Contains(column.ColumnName)))
                                {
                                    decimal value1 = 0;
                                    if (decimal.TryParse(appRow[column.ColumnName].ToString(), out value1))
                                    {
                                        //dataRowExceptions[DiffColumnKey] = Math.Round(value1, RoundToDecimal);    //surendra , for if pb data not present, diffcolumn=appdata   
                                        dataRowExceptions[DiffColumnKey] = value1;
                                        //dataRowExceptions[DiffColumnKey] = 0;  
                                    }// surendra  ,  for if pb data not present, diffcolumn=0
                                }

                            }
                            else                                                                                   // surendra, if nirvana data not there
                            {
                                if (Pbrow != null && Pbrow.Table.Columns.Contains(column.ColumnName))
                                {
                                    decimal value1 = 0;
                                    if (decimal.TryParse(Pbrow[column.ColumnName].ToString(), out value1))
                                    {
                                        //dataRowExceptions[DiffColumnKey] = -1 * Math.Round(value1, RoundToDecimal);     // surendra ,  then  diff=0-pbdata 
                                        dataRowExceptions[DiffColumnKey] = -1 * value1;
                                    }
                                }
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

                foreach (string column in listComparisonColumns)
                {

                    string columnReconStatus = ReconConstants.CONST_ReconStatus + column;
                    string columnToleranceType = ReconConstants.CONST_ToleranceType + column;
                    string columnToleranceValue = ReconConstants.CONST_ToleranceValue + column;

                    if (appRow != null && dataRowExceptions.Table.Columns.Contains(columnReconStatus))
                    {
                        if (appRow.Table.Columns.Contains(columnReconStatus))
                            dataRowExceptions[columnReconStatus] = appRow[columnReconStatus];
                        if (numericColumns.Contains(column) && appRow.Table.Columns.Contains(columnToleranceType) && appRow.Table.Columns.Contains(columnToleranceType))
                        {
                            dataRowExceptions[columnToleranceType] = appRow[columnToleranceType];
                            dataRowExceptions[columnToleranceValue] = appRow[columnToleranceValue];
                        }
                    }
                    if (Pbrow != null && dataRowExceptions.Table.Columns.Contains(columnReconStatus))
                    {
                        if (Pbrow.Table.Columns.Contains(columnReconStatus))
                            dataRowExceptions[columnReconStatus] = Pbrow[columnReconStatus];
                        if (numericColumns.Contains(column) && Pbrow.Table.Columns.Contains(columnToleranceType) && Pbrow.Table.Columns.Contains(columnToleranceType))
                        {
                            dataRowExceptions[columnToleranceType] = Pbrow[columnToleranceType];
                            dataRowExceptions[columnToleranceValue] = Pbrow[columnToleranceValue];
                        }
                    }
                }

                #region fill taxlotid from app data to exception data table
                if (appRow != null && dataRowExceptions.Table.Columns.Contains(ReconConstants.COLUMN_TaxLotID))
                {
                    if (appRow.Table.Columns.Contains(ReconConstants.COLUMN_TaxLotID))
                        dataRowExceptions[ReconConstants.COLUMN_TaxLotID] = appRow[ReconConstants.COLUMN_TaxLotID];
                }
                #endregion
                //setting balnk values in any columns which are null..
                foreach (DataColumn column in dataRowExceptions.Table.Columns)
                {
                    if (dataRowExceptions.IsNull(column))
                        dataRowExceptions[column] = String.Empty;
                    //{
                    //    //modified by amit 09.04.2015
                    //    //http://jira.nirvanasolutions.com:8080/browse/PRANA-7135
                    //    if (column.DataType == typeof(double))
                    //        dataRowExceptions[column] = 0.0;
                    //    else

                    //}
                }


                dtExceptions.Rows.Add(dataRowExceptions);
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

        #region Commented Section
        /// <summary>
        /// Fast compare method matches data using all the comparison fields as a key, no numeric match here, so all the matched data is exactly matched
        /// </summary>
        /// <param name="dtAppDataToMatch"></param>
        /// <param name="dtPBDataToMatch"></param>
        /// <param name="dtAppDataMatched"></param>
        /// <param name="dtPBDataMatched"></param>
        /// <param name="rule"></param>
        /// <param name="includeMatchedInExceptions"></param>

        /// <param name="listExceptionReportColumns"></param>
        private static void FastCompare(DataTable dtAppDataToMatch, DataTable dtPBDataToMatch, ref DataTable dtAppDataMatched, ref DataTable dtPBDataMatched, MatchingRule rule, bool includeMatchedInExceptions, List<ColumnInfo> listExceptionReportColumns, DataTable dtExceptions)
        {
            try
            {
                List<string> columns = rule.ComparisonFields;
                Dictionary<string, List<DataRow>> dtAppDict = GetDictForFastCompare(dtAppDataToMatch, columns);
                Dictionary<string, List<DataRow>> dtPBDict = GetDictForFastCompare(dtPBDataToMatch, columns);

                dtAppDataMatched = dtAppDataToMatch.Clone();
                dtPBDataMatched = dtPBDataToMatch.Clone();

                //List<string> matchedKeys = new List<string>();
                foreach (KeyValuePair<string, List<DataRow>> dtAppValue in dtAppDict)
                {

                    string comparisonKey = dtAppValue.Key;
                    List<DataRow> rowsApp = new List<DataRow>();
                    rowsApp.AddRange(dtAppValue.Value);

                    if (dtPBDict.ContainsKey(dtAppValue.Key))
                    {
                        List<DataRow> rowsPB = new List<DataRow>();
                        rowsPB.AddRange(dtPBDict[comparisonKey]);
                        //matchedKeys.Add(dt1Value.Key);
                        foreach (DataRow rowApp in rowsApp)
                        {
                            foreach (DataRow rowPB in rowsPB)
                            {
                                //Fill mismatch type data in nirvana and PB table, these tables are used to show data to user on lower recon grids
                                if (rowApp.Table.Columns.Contains(ReconConstants.MismatchType))
                                    rowApp[ReconConstants.MismatchType] = ReconConstants.MismatchType_ExactlyMatched;
                                if (rowPB.Table.Columns.Contains(ReconConstants.MismatchType))
                                    rowPB[ReconConstants.MismatchType] = ReconConstants.MismatchType_ExactlyMatched;
                                foreach (string comparisonField in rule.ComparisonFields)
                                {
                                    if (rowApp.Table.Columns.Contains(ReconConstants.CONST_ReconStatus + comparisonField))
                                        rowApp[ReconConstants.CONST_ReconStatus + comparisonField] = ReconStatus.ExactlyMatched;
                                    if (rowPB.Table.Columns.Contains(ReconConstants.CONST_ReconStatus + comparisonField))
                                        rowPB[ReconConstants.CONST_ReconStatus + comparisonField] = ReconStatus.ExactlyMatched;

                                    if (rule.NumericFields.Contains(comparisonField))
                                    {
                                        if (rowApp.Table.Columns.Contains(ReconConstants.CONST_ToleranceType + comparisonField))
                                            rowApp[ReconConstants.CONST_ToleranceType + comparisonField] = ToleranceType.None;
                                        if (rowPB.Table.Columns.Contains(ReconConstants.CONST_ToleranceType + comparisonField))
                                            rowPB[ReconConstants.CONST_ToleranceType + comparisonField] = ToleranceType.None;

                                        if (rowApp.Table.Columns.Contains(ReconConstants.CONST_ToleranceValue + comparisonField))
                                            rowApp[ReconConstants.CONST_ToleranceValue + comparisonField] = string.Empty;
                                        if (rowPB.Table.Columns.Contains(ReconConstants.CONST_ToleranceValue + comparisonField))
                                            rowPB[ReconConstants.CONST_ToleranceValue + comparisonField] = string.Empty;
                                    }
                                }
                                if (includeMatchedInExceptions)
                                {
                                    AddToExceptionsDatatable(rowApp, rowPB, listExceptionReportColumns, rule.NumericFields, rule.ComparisonFields, dtExceptions);
                                }
                                dtAppDataMatched.Rows.Add(rowApp.ItemArray);
                                dtPBDataMatched.Rows.Add(rowPB.ItemArray);
                                dtAppDataToMatch.Rows.Remove(rowApp);
                                dtPBDataToMatch.Rows.Remove(rowPB);
                                break;
                            }

                            // removing all matched rows..k
                            rowsPB.RemoveAll(delegate (DataRow dr)
                            {
                                if (dr.RowState == DataRowState.Detached || dr.RowState == DataRowState.Deleted)
                                {
                                    return true;

                                }
                                return false;
                            });
                        }
                        dtPBDict.Remove(dtAppValue.Key);
                    }

                }

                dtAppDataToMatch.AcceptChanges();
                dtPBDataToMatch.AcceptChanges();
                dtAppDataMatched.AcceptChanges();
                dtPBDataMatched.AcceptChanges();
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dtDict;
        }

        public static DataTable GenerateExceptionsReport(DataTable dtExceptions, string exceptionFileName, AutomationEnum.FileFormat ExpReportFormat, List<ColumnInfo> selectedColumnList, List<ColumnInfo> sortByColumnList, List<ColumnInfo> groupByColumnList, bool isReconReportToBeGenerated)
        {

            try
            {
                if (dtExceptions != null && selectedColumnList != null && sortByColumnList != null && groupByColumnList != null)
                {
                    if (dtExceptions.Rows.Count > 0 && isReconReportToBeGenerated == true)
                    {
                        dtExceptions.AcceptChanges();
                        ReconUtilities.GenerateExceptionsReport(dtExceptions, exceptionFileName, ExpReportFormat, selectedColumnList, sortByColumnList, groupByColumnList);
                    }

                    //Modified By: Surendra Bisht
                    //Modification date: Dec 31, 2013
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-3036
                    else if (isReconReportToBeGenerated == true)
                    {
                        DataRow row = dtExceptions.NewRow();

                        //    for (int i = 0; i < dtExceptions.Columns.Count; i++)
                        //        row[i] = string.Empty;
                        if (dtExceptions.Columns.Contains(ReconConstants.CAPTION_MismatchType))
                            row[dtExceptions.Columns[ReconConstants.CAPTION_MismatchType].Ordinal] = "App data in Sync";

                        dtExceptions.Rows.Add(row);
                        dtExceptions.AcceptChanges();
                        ReconUtilities.GenerateExceptionsReport(dtExceptions, exceptionFileName, ExpReportFormat, selectedColumnList, sortByColumnList, groupByColumnList);
                    }
                    return dtExceptions;
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
            finally
            {
                //TODO: Following part is temporarily committed to bind recon output with ultragrid
                //ClearExceptionsDataTable();
            }
            return null;
        }

        private static void ClearExceptionsDataTable(DataTable dtExceptions)
        {
            try
            {
                dtExceptions.Rows.Clear();
                dtExceptions.Columns.Clear();
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

        // send selected columns list and numeric columns list...

        private static void GenerateExceptionsDataTableSchema(List<ColumnInfo> listSelectedColumn, MatchingRule matchingRules, DataTable dtExceptions, ReconType reconType, bool isReconHasgroupping)
        {
            try
            {

                ClearExceptionsDataTable(dtExceptions);
                //condition removed of isSchemaCreated because schema need to refreshed for each template
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-2032
                //_dtExceptions = ReconUtilities.GetExceptionsDataTableSchema(listSelectedColumn);                
                List<string> lstCommonColumns = new List<string>();

                #region add TaxLot id to the exception report DataTable for CH mode only
                //CHMW-2783
                //if (CachedDataManager.GetPranaReleaseType() == PranaReleaseViewType.CHMiddleWare)
                {
                    //CHMW-2309	position recon showing exactly matched even though closing not run
                    if (reconType == ReconType.Transaction || reconType == ReconType.Position || reconType == ReconType.TaxLot)
                    {
                        if (!isReconHasgroupping)
                        {
                            dtExceptions.Columns.Add(ReconConstants.COLUMN_TaxLotID, typeof(string));
                        }
                    }
                }
                #endregion

                foreach (ColumnInfo column in listSelectedColumn)
                {
                    //if (!comparisonColumns.Contains(column.ColumnName))
                    //{
                    //modified by amit 09.04.2015
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-7135
                    //List<string> listOfNumericFields = ReconUtilities.GetListOfNumericFields(matchingRules);
                    switch (column.GroupType)
                    {

                        case ColumnGroupType.Nirvana:

                            string NirvanaColumnKey = ReconConstants.CONST_Nirvana + column.ColumnName;

                            string NirvanaCaption = ReconConstants.CONST_Nirvana + " " + column.ColumnName;

                            DataColumn NirvanaColumn = new DataColumn(NirvanaColumnKey);
                            NirvanaColumn.Caption = NirvanaCaption;
                            //if (listOfNumericFields.Contains(NirvanaColumnKey))
                            //{
                            //    dtExceptions.Columns.Add(NirvanaColumnKey, typeof(double));
                            //    //Following column is added to keep the original value of that field, as user may copy or change the Nirvana Value
                            //    dtExceptions.Columns.Add(ReconConstants.CONST_OriginalValue + column.ColumnName, typeof(double));
                            //}
                            //else
                            //{
                            dtExceptions.Columns.Add(NirvanaColumn);
                            //Following column is added to keep the original value of that field, as user may copy or change the Nirvana Value
                            dtExceptions.Columns.Add(ReconConstants.CONST_OriginalValue + column.ColumnName);
                            //}
                            break;
                        case ColumnGroupType.PrimeBroker:

                            string BrokerColumnKey = ReconConstants.CONST_Broker + column.ColumnName;

                            string BrokerCaption = ReconConstants.CONST_Broker + " " + column.ColumnName;

                            DataColumn BrokerColumn = new DataColumn(BrokerColumnKey);
                            BrokerColumn.Caption = BrokerCaption;
                            //if (listOfNumericFields.Contains(BrokerColumnKey))
                            //{
                            //    dtExceptions.Columns.Add(BrokerColumnKey, typeof(double));
                            //}
                            //else
                            //{
                            dtExceptions.Columns.Add(BrokerColumn);
                            //}
                            break;
                        case ColumnGroupType.Common:
                            string CommonColumnKey = column.ColumnName;
                            DataColumn CommonColumn = new DataColumn(CommonColumnKey);

                            dtExceptions.Columns.Add(CommonColumn);
                            break;
                        case ColumnGroupType.Both:

                            break;
                        case ColumnGroupType.Diff:
                            string DiffColumnKey = ReconConstants.CONST_Diff + column.ColumnName;
                            string DiffCaption = ReconConstants.CONST_Diff + '_' + column.ColumnName;
                            DataColumn diffColumn = new DataColumn(DiffColumnKey);
                            diffColumn.Caption = DiffCaption;
                            //if (listOfNumericFields.Contains(DiffColumnKey))
                            //{
                            //    dtExceptions.Columns.Add(DiffColumnKey, typeof(double));
                            //}
                            //else
                            //{
                            dtExceptions.Columns.Add(diffColumn);
                            //}
                            break;
                        default:
                            break;
                    }

                }
                //if (!dtExceptions.Columns.Contains(ReconConstants.MismatchType))
                //{
                //    dtExceptions.Columns.Add(ReconConstants.MismatchType);
                //}
                //if (!dtExceptions.Columns.Contains(ReconConstants.Matched))
                //{
                //    dtExceptions.Columns.Add(ReconConstants.Matched);
                //}

                if (dtExceptions.Columns.Contains(ReconConstants.COLUMN_MasterFund))
                {
                    dtExceptions.Columns[ReconConstants.COLUMN_MasterFund].Caption = ReconConstants.CAPTION_MasterFund;
                }
                if (dtExceptions.Columns.Contains(ReconConstants.COLUMN_AccountName))
                {
                    dtExceptions.Columns[ReconConstants.COLUMN_AccountName].Caption = ReconConstants.CAPTION_AccountName;
                }

                #region add common columns to list
                foreach (ColumnInfo column in listSelectedColumn)
                {
                    if (column.GroupType.Equals(ColumnGroupType.Common))
                    {
                        lstCommonColumns.Add(column.ColumnName);
                    }
                }
                #endregion

                foreach (string field in matchingRules.ComparisonFields)
                {
                    //No need to store original value,recon status, tolerance type, tolerance value for the common columns
                    if (!lstCommonColumns.Contains(field))
                    {
                        //checks if column exist
                        if (!dtExceptions.Columns.Contains(ReconConstants.CONST_ReconStatus + field))
                        {
                            dtExceptions.Columns.Add(ReconConstants.CONST_ReconStatus + field);
                        }

                        if (matchingRules.NumericFields.Contains(field))
                        {
                            if (!dtExceptions.Columns.Contains(ReconConstants.CONST_ToleranceType + field))
                            {
                                dtExceptions.Columns.Add(ReconConstants.CONST_ToleranceType + field);
                            }
                            if (!dtExceptions.Columns.Contains(ReconConstants.CONST_ToleranceValue + field))
                            {
                                dtExceptions.Columns.Add(ReconConstants.CONST_ToleranceValue + field);
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

        #endregion

        private static List<Result> IsRulePassed(MatchingRule rule, DataRow row1, DataRow row2)
        {
            List<Result> lstAllResult = new List<Result>();
            try
            {
                // The item with tolerance should be only one and last in the rule list
                // TODO: ENHANCEMENT REQUIRED !!!
                Dictionary<string, RuleParameters> inculdedRulefieldCollection = rule.GetInculdedRuleFieldCollection();
                foreach (KeyValuePair<string, RuleParameters> ruleItem in inculdedRulefieldCollection)
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
                    result.IsPassed = true;
                    result.Text = String.Empty;
                    result.MisMatchType = String.Empty;
                    result.ColumnName = ruleItem.Key;
                    switch (ruleItem.Value.Type)
                    {
                        case ComparisionType.Exact:
                            result.IsPassed = ExactMatch(var1, var2);
                            if (result.IsPassed)
                            {
                                result.ReconStaus = ReconStatus.ExactlyMatched;
                                result.Text = ReconConstants.MismatchType_ExactlyMatched;
                                result.MisMatchType = ReconConstants.MismatchType_ExactlyMatched; ;
                            }
                            else
                            {
                                result.ReconStaus = ReconStatus.MisMatch;
                                result.Text = ReconConstants.MismatchType_MismatchWithPB;
                                result.MisMatchType = ReconConstants.MismatchType_MismatchWithPB;
                            }
                            lstAllResult.Add(result);
                            break;

                        case ComparisionType.Partial:
                            result.IsPassed = PartialMatch(var1, var2);
                            lstAllResult.Add(result);
                            break;

                        case ComparisionType.Numeric:
                            result = NumericMatch(ref var1, ref var2, ruleItem.Value);
                            result.ColumnName = ruleItem.Key;
                            row1[ruleItem.Key] = var1;
                            row2[ruleItem.Key] = var2;
                            if (!result.IsPassed)
                            {
                                result.ReconStaus = ReconStatus.MisMatch;
                            }
                            lstAllResult.Add(result);
                            //if (result.IsPassed == true && result.Text != String.Empty)
                            //{
                            //    allConditionMatched.ToleranceType = result.ToleranceType;
                            //    allConditionMatched.ToleranceValue = result.ToleranceValue;
                            //    if (allConditionMatched.Text == String.Empty)
                            //    {
                            //        allConditionMatched.Text = result.Text;
                            //    }
                            //    else
                            //    {
                            //        allConditionMatched.Text += ", " + result.Text;
                            //    }

                            //    allConditionMatched.MisMatchType = result.MisMatchType;
                            //}
                            break;
                    }

                    //if (!result.IsPassed)
                    //{
                    //    allConditionMatched.IsPassed = false;
                    //    allConditionMatched.Text = String.Empty;
                    //    allConditionMatched.MisMatchType = string.Empty;
                    //    break;
                    //}
                }
                //if (allConditionMatched.IsPassed && allConditionMatched.MisMatchType == String.Empty)
                //{
                //    allConditionMatched.MisMatchType = ReconConstants.MismatchType_ExactlyMatched;
                //}
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return lstAllResult;
        }

        private static List<Result> IsRulePartiallyPassed(MatchingRule rule, DataRow row1, DataRow row2, List<ColumnInfo> listExceptionReportColumns)
        {
            List<Result> lstAllUnmatchedRules = new List<Result>();
            try
            {
                Dictionary<string, RuleParameters> rulefieldCollection = rule.GetInculdedRuleFieldCollection();
                foreach (KeyValuePair<string, RuleParameters> ruleItem in rulefieldCollection)
                {
                    string var1 = string.Empty;
                    string var2 = string.Empty;
                    if (row1.Table.Columns.Contains(ruleItem.Key))
                        var1 = row1[ruleItem.Key].ToString().Trim().ToUpper();
                    if (row2.Table.Columns.Contains(ruleItem.Key))
                        var2 = row2[ruleItem.Key].ToString().Trim().ToUpper();
                    DateTime dateValue = new DateTime();
                    double doubleValue;
                    if ((DateTime.TryParse(var1, out dateValue)) && (!double.TryParse(var1, out doubleValue)))
                    {
                        var1 = dateValue.Date.ToShortDateString();
                    }

                    if ((DateTime.TryParse(var2, out dateValue)) && (!double.TryParse(var2, out doubleValue)))
                    {
                        var2 = dateValue.Date.ToShortDateString();
                    }

                    Result result = new Result();
                    result.IsPassed = false;
                    result.Text = String.Empty;
                    result.ColumnName = ruleItem.Key;
                    switch (ruleItem.Value.Type)
                    {
                        case ComparisionType.Exact:
                            result.IsPassed = ExactMatch(var1, var2);
                            if (result.IsPassed)
                            {
                                result.ReconStaus = ReconStatus.ExactlyMatched;
                            }
                            else
                            {
                                result.ReconStaus = ReconStatus.MisMatch;
                            }

                            break;

                        case ComparisionType.Partial:
                            result.IsPassed = PartialMatch(var1, var2);
                            break;

                        case ComparisionType.Numeric:
                            result = NumericMatch(ref var1, ref var2, ruleItem.Value);
                            result.ColumnName = ruleItem.Key;
                            if (result.IsPassed)
                            {
                                //result.ReconStaus = ReconStatus.ExactlyMatched;
                            }
                            else
                            {
                                result.ReconStaus = ReconStatus.MisMatch;
                            }
                            break;
                    }

                    lstAllUnmatchedRules.Add(result);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return lstAllUnmatchedRules;
        }

        public static DataTable GetExceptionDataSet()
        {
            return null;
            // return _dtExceptions;
        }

    }
}