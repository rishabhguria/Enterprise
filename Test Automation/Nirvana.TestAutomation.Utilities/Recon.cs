using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.Core;
using System.Data.SqlClient;
using System.IO;
using Nirvana.TestAutomation.Interfaces.Enums;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Configuration;


namespace Nirvana.TestAutomation.Utilities
{
    public static class Recon
    {

        /// <summary>
        /// Public method to run recon and handle errors.
        /// </summary>
        /// 
        public static List<String> RunRecon(DataTable uiData, DataTable excelData, List<String> columns, double tolerance = 0.01, bool toleranceFlag = false, bool dateTimeFlag = false, ReconType reconType = ReconType.ExactMatch, int roundingDigit = 0, MidpointRounding midpointRounding = MidpointRounding.ToEven, bool isTaxLot = false)
        {
            try
            {
                List<String> errors = new List<String>();
                errors = InternalRunRecon(uiData, excelData, columns, tolerance, toleranceFlag, dateTimeFlag, reconType, roundingDigit, midpointRounding, isTaxLot);

                if ( ConfigurationManager.AppSettings["-TestCaseFixingMode"].ToLower().Equals("true"))
                {

                    try
                    {
                        if (ApplicationArguments.GlobalTestCaseDataSet.Tables.Contains(ApplicationArguments.ActiveInputStep))
                        {
                            int index = ApplicationArguments.GlobalTestCaseDataSet.Tables.IndexOf(ApplicationArguments.ActiveInputStep);
                            ApplicationArguments.GlobalTestCaseDataSet.Tables.RemoveAt(index);
                            DataTable copiedTable = uiData.Copy();
                            copiedTable.TableName = ApplicationArguments.ActiveInputStep;
                            DataSet tempDataSet = new DataSet();
                            for (int i = 0; i < ApplicationArguments.GlobalTestCaseDataSet.Tables.Count + 1; i++)
                            {
                                if (i == index)
                                {
                                    tempDataSet.Tables.Add(copiedTable);
                                }

                                if (i < ApplicationArguments.GlobalTestCaseDataSet.Tables.Count)
                                {
                                    tempDataSet.Tables.Add(ApplicationArguments.GlobalTestCaseDataSet.Tables[i].Copy());
                                }
                            }

                            ApplicationArguments.GlobalTestCaseDataSet.Tables.Clear();

                            foreach (DataTable tbl in tempDataSet.Tables)
                            {
                                ApplicationArguments.GlobalTestCaseDataSet.Tables.Add(tbl.Copy()); 
                            }
                        }
                        else
                        {
                            DataTable copiedTable = uiData.Copy();
                            copiedTable.TableName = ApplicationArguments.ActiveInputStep;
                            ApplicationArguments.GlobalTestCaseDataSet.Tables.Add(copiedTable);
                        }

                    }
                    catch (Exception ex)
                    {
                        string errorMsg = "[TestCaseFixingMode] Failed to update GlobalTestCaseDataSet for step: "
                       + ApplicationArguments.ActiveInputStep + ". Exception: " + ex.Message;
                        ApplicationArguments.GlobalErrorList.Add(errorMsg);
                        Console.WriteLine(errorMsg);
                        Console.WriteLine(ex.StackTrace);
                    }

                    if (errors.Count > 0)
                    {
                        string header = "StepName: " + ApplicationArguments.ActiveStep + ", InputStep: " + ApplicationArguments.ActiveInputStep;

                        ApplicationArguments.GlobalErrorList.Add(header);
                        for (int i = 0; i < errors.Count; i++)
                        {
                            ApplicationArguments.GlobalErrorList.Add("");
                            ApplicationArguments.GlobalErrorList.Add(errors[i]);
                        }
                        errors = new List<String>();
                    }// we need to have complete execution of cases in TestCaseFixingMode ,Therefore instead of throwing error saving it for future log

                }

                return errors;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Runs the recon.
        /// </summary>
        /// <param name="uiData">The uiData.</param>
        /// <param name="excelData">The excelData.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="toleranceFlag">if set to <c>true</c> [tolerance flag].</param>
        /// <param name="dateTimeFlag">if set to <c>true</c> [date time flag].</param>
        /// <returns></returns>
        /// /// <summary>
        /// Core recon logic implementation moved internally for reuse/testability.
        /// </summary>
        public static List<String> InternalRunRecon(DataTable uiData, DataTable excelData, List<String> columns, double tolerance = 0.01, bool toleranceFlag = false, bool dateTimeFlag = false, ReconType reconType = ReconType.ExactMatch, int roundingDigit = 0, MidpointRounding midpointRounding = MidpointRounding.ToEven, bool isTaxLot = false)
        {
            try
            { 
                List<String> errors = new List<String>();
                List<String> unmatchedCols = new List<String>();
                double numberColumnValue;
                DateTime dateColumnValue;
                int rowsExcel;
                int rowsUI;
                ///////////////////////////////////////////////////////////////////////////////
                // verification for negative testing
                if (excelData.Columns.Contains(TestDataConstants.TESTINGTYPE))
                {
                    // check first whether excel data have any negatve testing case'
                    int Anynegativetesting = 0;
                    bool negativeOnlyFound = false;

                    for (int ind = excelData.Rows.Count - 1; ind >= 0; ind--)
                    {
                        if (string.IsNullOrEmpty(excelData.Rows[ind]["Testing Type"].ToString()))
                        {
                            continue;
                        }
                        string testingType = excelData.Rows[ind]["Testing Type"].ToString().ToUpper().Trim();

                        if (testingType.Contains("NEGATIVE"))
                        {
                            Anynegativetesting++;
                            if (string.Equals(testingType, "NEGATIVEONLY", StringComparison.OrdinalIgnoreCase))
                            {
                                negativeOnlyFound = true;
                            }
                        }

                    }


                    if (Anynegativetesting > 0)
                    {
                        // not valid for exact duplicate trades as there is new step for it
                        DataTable Verifydata = new DataTable();
                        Verifydata = excelData.Copy();

                        // removing those rows having negative testing from excelData table
                        for (int ind = excelData.Rows.Count - 1; ind >= 0; ind--)
                        {
                            if (string.IsNullOrEmpty(excelData.Rows[ind]["Testing Type"].ToString()))
                            {
                                continue;
                            }
                            string testingType = excelData.Rows[ind]["Testing Type"].ToString().ToUpper().Trim();

                            if (testingType.Contains("NEGATIVE"))
                            {
                                excelData.Rows[ind].Delete();
                                excelData.AcceptChanges();
                            }

                        }

                        // removing columns having no requirement in further process of reconn
                        try
                        {
                            excelData.Columns.Remove("Testing Type");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        try
                        {
                            excelData.Columns.Remove("Testing Column");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        try
                        {
                            excelData.Columns.Remove("Testing Col Value");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        try
                        {
                            excelData.Columns.Remove("DuplicateCountOnUI");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        try
                        {
                            excelData.Columns.Remove("DuplicateRow");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        
                        Console.WriteLine(" Negative Testing Columns Removed");


                        //taking count of number of rows whaving negative testing & removing those rows having non - negative testing from copy table
                        for (int ind = Verifydata.Rows.Count - 1; ind >= 0; ind--)
                        {
                            if (Verifydata.Rows[ind]["Testing Type"].ToString() == String.Empty)
                            {
                                Verifydata.Rows[ind].Delete();
                                Verifydata.AcceptChanges();
                            }
                        }
                        int Negativetestcount = Verifydata.Rows.Count;  // count of negative testing rows


                        // maintaing the rows which are needed only for verification other than negative testing
                        for (int i = 0; i < Negativetestcount; i++)
                        {

                            DataRow dr = Verifydata.Rows[i];

                            //check if same row present on ui or not
                            DataRow[] foundRow;
                            try
                            {
                                String TestingColumn = dr[TestDataConstants.TESTINGCOLUMN].ToString();
                                String TestingVALUE = dr[TestDataConstants.TESTINGCOLUMNVAL].ToString();
                                String SYMBOLVALUE = dr[TestDataConstants.COL_SYMBOL].ToString();
                                foundRow = uiData.Select(String.Format(@"[" + TestDataConstants.COL_SYMBOL + "]='{0}' AND [" + TestingColumn + "] ='{1}'", SYMBOLVALUE, TestingVALUE));



                                string lengthofrows = foundRow.Length.ToString();

                                if (foundRow.Length > 0)
                                {
                                    if (dr[TestDataConstants.DUPLICATEROW].ToString() != "Yes" || dr[TestDataConstants.DUPLICATECOUNTONUI].ToString() != lengthofrows)
                                    {
                                        errors.Add("Negative Testing failed , Row found");
                                        return errors;

                                    }

                                }
                            }

                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }

                        }
                        Console.WriteLine(" Negative Testing Succeeded");

                        if (negativeOnlyFound)
                        {
                            return errors;
                        }

                    }



                    else
                    {
                        Console.WriteLine("Non- Negative Testing ");
                        try
                        {
                            excelData.Columns.Remove("Testing Type");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        try
                        {
                            excelData.Columns.Remove("Testing Column");
                        
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        try
                        {
                        excelData.Columns.Remove("Testing Col Value");
                        }
                        
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        Console.WriteLine(" Negative Testing Columns Removed");
                    }

                }

                rowsExcel = excelData.Rows.Count;
                rowsUI = uiData.Rows.Count;
                Boolean existed = excelData.Columns.Contains("isRowCountCheck");

                //to verify empty ui and excel values for old cases
                if (rowsUI == 0 && rowsExcel == 0 && excelData.Columns.Contains("isRowCountCheck") == false)
                {
                    return errors;
                }

                // DataRow dtrow = excelData.Rows[0];

                //   DataRow dtrowc = uiData.Rows[0];


                if (rowsExcel != 0)
                {
                    DataRow dtrow = excelData.Rows[0];
                    if (dtrow.Table.Columns.Contains(TestDataConstants.ROW_COUNT_CHECK))
                    {
                        if (rowsUI == 0)
                        {
                            rowsExcel = 0;

                        }


                        if ((rowsUI == 0 && rowsExcel == 0 && dtrow[TestDataConstants.ROW_COUNT_CHECK].ToString() != String.Empty) && (dtrow[TestDataConstants.ROW_COUNT_CHECK].ToString().Equals("True")) || (dtrow[TestDataConstants.ROW_COUNT_CHECK].ToString().Equals("TRUE")) && dtrow[TestDataConstants.COL_ORDER_TYPE].ToString() == String.Empty && dtrow[TestDataConstants.COL_SYMBOLOGY].ToString() == String.Empty)
                        {



                            return errors;

                        }
                    }
                }
                /////////////////////////////CheckPMLiveFeed

                if (rowsExcel != 0)
                {
                    DataRow dtrow = excelData.Rows[0];
                    if (dtrow.Table.Columns.Contains("Pricing Source") == true)
                    {
                        if (rowsUI != 0 && rowsExcel != 0 && dtrow[TestDataConstants.COL_PRICING_SOURCE].ToString() == "Live Feed")
                        {


                            // var CheckLiveFeedValues = new List<Tuple<string,int>>();
                            IDictionary<string, int> CheckLiveFeedValues = new Dictionary<string, int>();

                            foreach (DataRow dtrow2 in uiData.Rows)
                            {

                                CheckLiveFeedValues.Add("PX ASK", dtrow2[TestDataConstants.COL_PX_ASK].ToString() == "0" ? 0 : 1);
                                CheckLiveFeedValues.Add("PX MID", dtrow2[TestDataConstants.COL_PX_MID].ToString() == "0" ? 0 : 1);
                                CheckLiveFeedValues.Add("PX BID", dtrow2[TestDataConstants.COL_PX_BID].ToString() == "0" ? 0 : 1);
                                CheckLiveFeedValues.Add("PX LAST", dtrow2[TestDataConstants.COL_PX_LAST].ToString() == "0" ? 0 : 1);
                                CheckLiveFeedValues.Add("PX SELECTED FEED LOCAL", dtrow2[TestDataConstants.COL_PX_SELECTED_FEED_LOCAL].ToString() == "0" ? 0 : 1);
                                CheckLiveFeedValues.Add("PX SELECTED FEED BASE", dtrow2[TestDataConstants.COL_PX_SELECTED_FEED_BASE].ToString() == "0" ? 0 : 1);
                                CheckLiveFeedValues.Add("PRICING SOURCE", dtrow2[TestDataConstants.COL_PRICING_SOURCE].ToString() == "Live Feed" ? 1 : 0);

                                foreach (var item in CheckLiveFeedValues)
                                {
                                    if (item.Value == 0)
                                    {
                                        errors.Add(item.Key + " Contains 0 Value");
                                    }
                                }

                                //  if (( dtrow[TestDataConstants.COL_Px_Bid].ToString() ) && (dtrow[TestDataConstants.ROW_COUNT_CHECK].ToString().Equals("True")) || (dtrow[TestDataConstants.ROW_COUNT_CHECK].ToString().Equals("TRUE")) && dtrow[TestDataConstants.COL_ORDER_TYPE].ToString() == String.Empty && dtrow[TestDataConstants.COL_SYMBOLOGY].ToString() == String.Empty)



                                return errors;

                            }
                        }
                    }
                }



                if (rowsExcel != 0)
                {
                    DataRow dataRow1 = excelData.Rows[0];
                    if (dataRow1.Table.Columns.Contains(TestDataConstants.ROW_COUNT_CHECK))
                    {
                        if ((dataRow1[TestDataConstants.ROW_COUNT_CHECK].ToString() != String.Empty) && ((dataRow1[TestDataConstants.ROW_COUNT_CHECK].ToString().Equals("True")) || (dataRow1[TestDataConstants.ROW_COUNT_CHECK].ToString().Equals("TRUE"))))
                        {
                            if (!(rowsExcel.Equals(rowsUI)))
                            {
                                errors.Add("Number of rows Mismatched. UI Row = " + rowsUI + " Excel Row = " + rowsExcel);
                                return errors;
                            }
                        }
                    }
                }
                Dictionary<string, int> colToIndexMapping = ColToIndex(uiData);

                if (uiData == null && excelData != null)
                {
                    errors.Add("The grid is empty while the excelsheet is not.! ");
                    CommonMethods.SaveScreenshotAndPreferences(excelData.TableName);
                    return errors;
                }
                //check whether SMDB contains same value as shown on UI
                if (excelData.Columns.Contains(TestDataConstants.COL_VERIFY_ON_SM) && excelData.Columns.Contains(TestDataConstants.COLLIST_TO_VERIFY_ON_SM))
                {
                    try
                    {
                        List<string> columnsToVerifyOnSM = new List<string>();
                        Dictionary<string, bool> symbolBloombergSymbolMapping = new Dictionary<string, bool>();
                        DataTable fullExcelData = DataUtilities.CopyDataTable(excelData, new List<string>());
                        //DataTable fullExcelData = excelData.Clone();
                        string columnsToBeRemoved = TestDataConstants.COLLIST_TO_VERIFY_ON_SM + "," + TestDataConstants.COL_VERIFY_ON_SM;
                        excelData = DataUtilities.RemoveColumnsAndRows(columnsToBeRemoved, excelData);

                        for (int i = excelData.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow excelRow = excelData.Rows[i];

                            string verifyOnSMValue = fullExcelData.Rows[i][TestDataConstants.COL_VERIFY_ON_SM].ToString();
                            if (verifyOnSMValue.Equals("YES", StringComparison.OrdinalIgnoreCase))
                            {
                                string columnsToVerify = fullExcelData.Rows[i][TestDataConstants.COLLIST_TO_VERIFY_ON_SM].ToString();
                                columnsToVerifyOnSM.AddRange(columnsToVerify.Split(','));


                                if (!string.IsNullOrEmpty(columnsToVerify))
                                {
                                    DataRow[] DataRows = DataUtilities.GetMatchingDataRows(uiData, excelRow);
                                    if (DataRows.Count() > 0)
                                    {

                                        string symbol = excelRow[TestDataConstants.COL_SYMBOL].ToString();
                                        string bloombergSymbol = excelRow[TestDataConstants.COL_BLOOMBERG_SYMBOL].ToString();
                                        if (string.IsNullOrEmpty(bloombergSymbol))
                                        {
                                            errors.Add("Please enter Bloomberg Symbol column value to continue verification");
                                            return errors;

                                        }
                                        if (!symbolBloombergSymbolMapping.ContainsKey(symbol))
                                        {
                                            string query = "SELECT " + columnsToVerify + " FROM T_SMSymbolLookUpTable WHERE TickerSymbol = " + "'" + symbol + "'";


                                            DataTable data = SqlUtilities.GetDataFromQuery(query);

                                            foreach (DataRow dataDr in data.Rows)
                                            {
                                                foreach (DataColumn columnNames in data.Columns)
                                                {

                                                    string columnWithSpaces = DataUtilities.AddSpaceBetweenUppercase(columnNames.ToString());
                                                    if (excelRow.Table.Columns.Contains(columnNames.ToString()))
                                                    {
                                                        if (excelRow[columnNames.ToString()].Equals(dataDr[columnNames.ToString()]))
                                                        {
                                                            Console.WriteLine(excelRow[columnNames.ToString()] + " == " + dataDr[columnNames.ToString()]);

                                                        }

                                                    }
                                                    else if (excelRow.Table.Columns.Contains(columnWithSpaces))
                                                    {
                                                        if (excelRow[columnWithSpaces].Equals(dataDr[columnNames.ToString()]))
                                                        {
                                                            Console.WriteLine("columnWithSpaces =>" + excelRow[columnWithSpaces] + " == " + "SMDATA=>" + dataDr[columnNames.ToString()]);

                                                        }
                                                    }
                                                    else
                                                        throw new Exception("SM Verification failed on Recon=> either check mismatch on data or provide column Names matching on SM DB(withoutspace)");
                                                }
                                            }






                                            Console.WriteLine("BBG column value in in sync with SM value ");
                                        }
                                        else if (symbolBloombergSymbolMapping.ContainsKey(symbol))
                                        {
                                            if (symbolBloombergSymbolMapping[symbol].Equals(true))
                                            {

                                                Console.WriteLine("BBG column value in in sync with SM value ");
                                            }
                                        }


                                        //dr[TestDataConstants.COL_SYMBOL] = getTickerSymbol;

                                    }
                                    else
                                    {
                                        errors.Add("No MATCHING dataRow found  on UI dataRow->" + excelRow);
                                        return errors;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("There is no columns to Verify on SM");
                                }

                            }

                        }
                        // excelData.Columns.Remove(TestDataConstants.COL_VERIFY_ON_SM);
                        // excelData.Columns.Remove(TestDataConstants.COLLIST_TO_VERIFY_ON_SM);


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (rowsExcel != 0)
                {
                    if (columns != null && columns.Count > 0)
                    {

                        //To create key if no columns selected as key in order to be used for expression
                        DataRow dataRow = excelData.Rows[0];
                        foreach (DataColumn dc in excelData.Columns)
                        {
                            string colName = dc.ColumnName.Trim();
                            string colValue = dataRow[colName].ToString().Trim();
                            if (!(string.IsNullOrWhiteSpace(colValue) || double.TryParse(colValue, out numberColumnValue) || DateTime.TryParse(colValue, out dateColumnValue)))
                            {
                                if (colName == "Symbol" && !columns.Contains(colName))
                                {
                                    columns.Add(colName);
                                }
                                else if (colName == "Account Name" && !columns.Contains(colName))
                                {
                                    columns.Add(colName);
                                }
                            }
                        }
                    }
                }
                if (reconType == ReconType.RoundingMatch)
                {
                    if (excelData.Rows.Count > 0)
                    {
                        foreach (DataColumn col in excelData.Columns)
                        {
                            foreach (DataRow row in excelData.Rows)
                            {
                                decimal decValue = decimal.MinValue;
                                if (row[col.ColumnName].ToString() != "" && decimal.TryParse(row[col.ColumnName].ToString(), out decValue))
                                    row[col.ColumnName] = Math.Round(decValue, roundingDigit, midpointRounding).ToString("F" + roundingDigit.ToString());
                            }
                        }
                    }

                    if (uiData.Rows.Count > 0)
                    {
                        foreach (DataColumn col in uiData.Columns)
                        {
                            foreach (DataRow row in uiData.Rows)
                            {
                                decimal decValue = decimal.MinValue;
                                if (row[col.ColumnName].ToString() != "" && decimal.TryParse(row[col.ColumnName].ToString(), out decValue))
                                    row[col.ColumnName] = Math.Round(decValue, roundingDigit, midpointRounding).ToString("F" + roundingDigit.ToString());
                            }
                        }
                    }
                }

                //Dictionary to contain expression and corresponding matched rows from UI data table
                Dictionary<String, DataRow[]> supersetExpressionWise = new Dictionary<String, DataRow[]>();
                string expression = string.Empty;
                foreach (DataRow dr in uiData.Rows)
                {
                    expression = string.Empty;
                    //Generates expression for UI data rows
                    if (!isTaxLot)
                    {
                        foreach (string colName in columns)
                        {
                            string colValue = dr[colName].ToString().Trim();
                            if (columns.Contains(colName))
                            {
                                if (colValue.Equals(ExcelStructureConstants.BLANK_CONST))
                                    colValue = string.Empty;
                                expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                                expression = expression + "[" + colName + "] = '" + colValue + "' ";
                            }
                        }
                    }
                    //Finds the matching rows from the table and adds it to the dictionary
                    if (!supersetExpressionWise.ContainsKey(expression))
                    {
                        //DataRow[] matchedRows = uiData.Select(expression).ToArray();
                        DataRow[] matchedRows = uiData.Select(expression);
                        supersetExpressionWise.Add(expression, matchedRows);
                    }
                }


                //Verifies the excel sheet data to be present on UI
                foreach (DataRow dr in excelData.Rows)
                {
                    //frame the expression for each row
                    string expression1 = string.Empty;
                    string targetSymbol = string.Empty;
                    string targetAccount = string.Empty;
                    if (!isTaxLot)
                    {
                        foreach (string colName in columns)
                        {
                            string colValue = dr[colName].ToString().Trim();
                            //Take columns taken in dictionary expression only
                            if (colValue == "Current User" || colValue == "Actual User" || colValue.ToLower().Contains("internal"))
                            {
                                continue;
                            }
                            if (colValue.Equals(ExcelStructureConstants.BLANK_CONST))
                                colValue = string.Empty;
                            if (colName == "Symbol")
                            {
                                targetSymbol = colValue;
                            }
                            else if (colName == "Account Name")
                            {
                                targetAccount = colValue;
                            }

                            expression1 = string.IsNullOrWhiteSpace(expression1) ? expression1 : expression1 + " AND ";
                            expression1 = expression1 + "[" + colName + "] = '" + colValue + "' ";
                        }
                    }
                    if (!supersetExpressionWise.ContainsKey(expression1))
                    {
                        string expression3 = string.Empty;

                        //List<DataRow> matchingRows = new List<DataRow>();
                        DataRow[] matchingRows = new DataRow[supersetExpressionWise.Count];
                        int count = 0;
                        foreach (var kvp in supersetExpressionWise)
                        {
                            string condition = string.Empty;
                            string key = kvp.Key;
                            DataRow[] row = kvp.Value;
                            if (!String.IsNullOrEmpty(targetSymbol))
                            {
                                condition = "[" + "Symbol" + "] = '" + targetSymbol + "' ";
                            }
                            else
                            {
                                condition = "[" + "Account Name" + "] = '" + targetAccount + "' ";
                            }
                            if (key.Contains(condition))
                            {
                                matchingRows[count] = row[0];
                                count++;
                            }

                        }

                        // Assuming matchingRows is the array with some null values
                        List<DataRow> nonNullRowsList = new List<DataRow>();

                        foreach (var row in matchingRows)
                        {
                            if (row != null)
                            {
                                nonNullRowsList.Add(row);
                            }
                        }

                        DataRow[] nonNullRows = nonNullRowsList.ToArray();
                        //DataRow[] matchingDictionary = supersetExpressionWise[expression];
                        expression3 = GenerateErrorFile(dr, nonNullRows, columns, excelData.Columns, ApplicationArguments.TestCaseToBeRun, colToIndexMapping, "Mismatch");
                        if (expression3.ToString() == String.Empty)
                        {
                            errors.Add("Could not find entry : " + expression1);
                        }
                        else
                            errors.Add("Could not find entry : " + expression3);
                    }
                    else
                    {
                        DataRow[] matchingDictionary = supersetExpressionWise[expression1];
                        HashSet<string> dateColumns = new HashSet<string>();
                        expression = string.Empty;
                        expression1 = string.Empty;
                        String dateExpression = string.Empty;
                        foreach (DataColumn dc in excelData.Columns)
                        {
                            if (!columns.Contains(dc.ColumnName))
                            {
                                string colValue = dr[dc].ToString().Trim();
                                double T = tolerance;
                                if (dc.ColumnName == "Current User" || dc.ColumnName == "Actual User")
                                {
                                    continue;
                                }
                                if (colValue == String.Empty)
                                {
                                    continue;
                                }
                                if (colValue.Contains("$"))
                                {
                                    colValue = FormatCurrency(colValue);
                                    expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                                    expression = expression + "[" + dc + "] = '" + colValue + "' ";
                                }
                                else if (colValue == ExcelStructureConstants.BLANK_CONST)
                                {
                                    colValue = string.Empty;
                                    expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                                    expression = expression + "[" + dc + "] = '" + colValue + "' ";
                                }
                                else if (double.TryParse(colValue, out numberColumnValue))
                                {

                                    if (toleranceFlag)
                                    {

                                        expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                                        /*double value = numberColumnValue - T;
                                        expression = expression + "[" + dc + "] >= " + value;
                                        expression1 = string.IsNullOrWhiteSpace(expression1) ? expression1 : expression1 + " AND ";
                                        double value1 = numberColumnValue + T;
                                        expression1 = expression1 + "[" + dc + "] <= " + value1;*/


                                        if (numberColumnValue <= 0.1 && T == 0.1 && numberColumnValue >= 0)
                                        {

                                            double value = numberColumnValue - T;
                                            expression = expression + "[" + dc + "] >= " + value.ToString("0.00");
                                            expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                                            //double value1 = numberColumnValue + T;
                                            expression = expression + "[" + dc + "] <= '" + (numberColumnValue + T).ToString("0.00") + "' ";
                                        }

                                        else if (numberColumnValue < 0)
                                        {
                                            int maxExponent = 8; // Adjust the maximum exponent as needed
                                            bool conditionMet = false;
                                            for (int exponent = 1; exponent <= maxExponent && !conditionMet; exponent++)
                                            {
                                                int value = (int)Math.Pow(10, exponent);
                                                value = 0 - value;
                                                double result = numberColumnValue - T;

                                                if (numberColumnValue == value)
                                                {
                                                    expression = expression + "[" + dc + "] >= " + result.ToString("0.00");
                                                    expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                                                    expression = expression + "[" + dc + "] <= '" + (numberColumnValue + T).ToString("0.00") + "' ";
                                                    conditionMet = true;
                                                    break;
                                                }
                                            }
                                            if (!conditionMet)
                                            {
                                                expression = expression + "[" + dc + "] <= '" + (numberColumnValue - T).ToString("0.00") + "' ";
                                                expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                                                expression = expression + "[" + dc + "] >= '" + (numberColumnValue + T).ToString("0.00") + "' ";
                                            }
                                        }
                                        else
                                        {
                                            int maxExponent = 8; // Adjust the maximum exponent as needed
                                            bool conditionMet = false;


                                            for (int exponent = 1; exponent <= maxExponent && !conditionMet; exponent++)
                                            {
                                                int value = (int)Math.Pow(10, exponent);
                                                //double result = numberColumnValue - T;

                                                if (numberColumnValue == value)
                                                {
                                                    expression = expression + "[" + dc + "] >= '" + (numberColumnValue).ToString("0.00") + "' ";
                                                    conditionMet = true;
                                                    break;
                                                }
                                            }
                                            if (!conditionMet)
                                            {
                                                expression = expression + "[" + dc + "] >= '" + (numberColumnValue - T).ToString("0.00") + "' ";
                                            }
                                            expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                                            expression = expression + "[" + dc + "] <= '" + (numberColumnValue + T).ToString("0.00") + "' ";
                                        }

                                    }
                                    else
                                    {
                                        expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                                        expression = expression + "[" + dc + "] = '" + colValue + "' ";
                                    }
                                }
                                else if (DateTime.TryParse(dr[dc].ToString().Trim(), out dateColumnValue))
                                {

                                    if (dateTimeFlag)
                                    {
                                        expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                                        expression = expression + "[" + dc + "] = '" + dateColumnValue + "' ";

                                    }
                                    else
                                    {
                                        dateExpression = string.IsNullOrWhiteSpace(dateExpression) ? dateExpression : dateExpression + " AND ";
                                        dateColumns.Add(dc.Caption);
                                        dateExpression = dateExpression + "[" + dc + "] = '" + dateColumnValue.Date.ToString("MM/dd/yyyy") + "' ";
                                    }
                                }
                                else
                                {
                                    expression = string.IsNullOrWhiteSpace(expression) ? expression : expression + " AND ";
                                    expression = expression + "[" + dc + "] = '" + colValue + "' ";
                                }
                            }
                        }

                        //Finds the corresponding row exists or not in the dictionary
                        DataTable subsetTable = new DataTable();

                        subsetTable = matchingDictionary.CopyToDataTable();

                        //RemovingExtraColumnAddedBeforeMatching
                        if (expression.Contains("isRowCountCheck"))
                        {
                            int index = expression.IndexOf("isRowCountCheck");
                            index = index - 5;
                            expression = expression.Remove(index);
                        }
                        /* DataRow[] matchedRows = new DataRow[supersetExpressionWise.Count]; 
                        if (toleranceFlag)
                        {
                          DataRow[] matchedRows1 = subsetTable.Select(expression);
                          DataRow[] matchedRows2 = subsetTable.Select(expression1);
                          matchedRows = matchedRows1.Intersect(matchedRows2, DataRowComparer.Default).ToArray();
                        }
                        else
                        {
                            matchedRows = subsetTable.Select(expression);
                        }*/
                        DataRow[] matchedRows = subsetTable.Select(expression);
                        /*if (matchedRows.Length <= 0 && tolerance == 1)
                        {
                            matchedRows = DataUtilities.GetMatchingDataRows(subsetTable, dr);
                        }*/
                        if (matchedRows.Length <= 0)
                        {
                            //errors.Add("Values did not match for " + expression);
                            string expression2 = string.Empty;
                            expression2 = GenerateErrorFile(dr, matchingDictionary, columns, excelData.Columns, ApplicationArguments.TestCaseToBeRun, colToIndexMapping, "Mismatch");
                            errors.Add("Values did not match for " + expression2);
                            continue;
                        }

                        //Convert each value of each column in dateColumns to Date in specified Format by parsing each value to datetime Object

                        //Checks if dateTime Flag is true and dateColumns is not empty
                        if (dateColumns.Count > 0)
                        {
                            subsetTable = matchedRows.CopyToDataTable();
                            DateTime columnDateTimeValue = new DateTime();
                            foreach (DataRow row in subsetTable.Rows)
                            {
                                foreach (string columnName in dateColumns)
                                {
                                    DateTime.TryParse(row[columnName].ToString(), out columnDateTimeValue);
                                    row[columnName] = columnDateTimeValue.Date.ToString("MM/dd/yyyy");
                                }
                            }

                            if (subsetTable.Select(dateExpression).ToList().Count <= 0)
                            {
                                errors.Add("Values did not match for " + expression + dateExpression);
                            }
                        }
                    }
                }
                if (errors != null && errors.Count > 0)
                    CommonMethods.SaveScreenshotAndPreferences(uiData.TableName);
                return errors;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }

        private static string FormatCurrency(string value)
        {
            value = value.Trim();
            string numericPart = value.Replace("$", "").Trim();
            decimal number;
            if (decimal.TryParse(numericPart, out number))
            {
                return string.Format("$ {0:0.00}", number);
            }
            return value;
        }

        public static string GenerateErrorFile(DataRow dr, DataRow[] uiData, List<string> keyCols, DataColumnCollection cols, String testCase, Dictionary<string, int> colToIndex, String errorType)
        {
            int colCount;
            string expression1 = string.Empty;

            if (File.Exists("E:\\DistributedAutomation\\ReconErrors\\" + testCase + ".xlsx"))
            {
                File.Delete("E:\\DistributedAutomation\\ReconErrors\\" + testCase + ".xlsx");
                //GenerateErrorFile(dr, uiData, keyCols, cols, testCase, colToIndex, errorType);
            }

            if (!Directory.Exists("E:\\DistributedAutomation\\ReconErrors"))
            {
                Directory.CreateDirectory("E:\\DistributedAutomation\\ReconErrors");
            }

            if (!File.Exists("E:\\DistributedAutomation\\ReconErrors\\" + testCase + ".xlsx"))
            {
                colCount = 2;
                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo("E:\\DistributedAutomation\\ReconErrors\\" + testCase + ".xlsx")))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells[2, 1].Value = "ErrorType";
                    /*foreach (string col in keyCols)
                    {
                        worksheet.Cells[2, colCount].Value = col;
                        colCount++;
                    }*/
                    foreach (DataColumn dc in cols)
                    {
                        /*if (keyCols.Contains(dc.ToString()))
                            continue;*/
                        worksheet.Cells[1, colCount].Value = "Excel";
                        worksheet.Cells[1, colCount + 1].Value = "UI";
                        worksheet.Cells[2, colCount].Value = dc.ToString();
                        worksheet.Cells[2, colCount, 2, colCount + 1].Merge = true;
                        worksheet.Cells[2, colCount, 2, colCount + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colCount += 2;
                    }

                    xlPackage.Save();
                }
            }


            int rowCount = 0;


            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo("E:\\DistributedAutomation\\ReconErrors\\" + testCase + ".xlsx")))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sheet1"];


                foreach (DataRow uidr in uiData)
                {
                    rowCount = worksheet.Dimension.End.Row;
                    rowCount++;
                    colCount = 1;

                    worksheet.Cells[rowCount, colCount].Value = errorType;
                    colCount++;
                    /*foreach (string col in keyCols)
                    {
                        worksheet.Cells[rowCount, colCount].Value = dr[col].ToString();
                        colCount++;
                    }*/

                    foreach (DataColumn dc in cols)
                    {

                        /*if (keyCols.Contains(dc.ToString()))
                            continue;*/
                        //EnteringExcelData
                        worksheet.Cells[rowCount, colCount].Value = dr[dc].ToString();
                        colCount++;
                        //EnteringUIData
                        if (colToIndex.ContainsKey(dc.ToString()))
                        {
                            worksheet.Cells[rowCount, colCount].Value = uidr[colToIndex[dc.ToString()]].ToString();

                            //CreatingExpressionOfMismatchValues
                            if (!(dr[dc].ToString().Equals(uidr[colToIndex[dc.ToString()]].ToString())) && !(dr[dc].ToString().Equals("")) && !(uidr[colToIndex[dc.ToString()]].ToString().Equals("")))
                            {
                                string Excel = dr[dc].ToString();
                                string UI = uidr[colToIndex[dc.ToString()]].ToString();
                                string colName = dc.ToString();
                                expression1 = string.IsNullOrWhiteSpace(expression1) ? expression1 : expression1 + " , \n";
                                expression1 = expression1 + "[" + colName + "] = UI{" + UI + "}+Excel{" + Excel + "}";
                            }

                        }
                        colCount++;


                    }

                }
                xlPackage.Save();
            }
            return expression1;
        }

        public static Dictionary<string, int> ColToIndex(DataTable table)
        {
            Dictionary<string, int> colToIndexMapping = new Dictionary<string, int>();
            int index = 0;
            foreach (DataColumn dc in table.Columns)
            {
                colToIndexMapping.Add(dc.ToString(), index);
                index++;
            }
            return colToIndexMapping;
        }
    }
}
