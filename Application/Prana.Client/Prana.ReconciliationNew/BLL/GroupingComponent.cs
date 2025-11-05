using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Prana.ReconciliationNew
{
    public partial class GroupingComponent
    {


        /// <summary>
        /// This method is called whenever we apply grouping in the Recon.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="groupingCriteria"></param>
        /// <param name="reconType"></param>
        /// <param name="dictGroupingSummary"></param>
        public static void Group(DataTable dt, GroupingCriteria groupingCriteria, ReconType reconType, SerializableDictionary<string, string> dictGroupingSummary)
        {
            try
            {
                if (dt != null && groupingCriteria != null && dictGroupingSummary != null)
                {
                    if (groupingCriteria.DictGroupingCriteria.ContainsValue(true))
                    {
                        Dictionary<string, DataRow> dictGroupedRows = new Dictionary<string, DataRow>();
                        List<DataRow> deletdRows = new List<DataRow>();
                        StringBuilder GroupingErrorMessage = new StringBuilder();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dt.Rows[i];
                            //GroupingErrorMessage contains proper error message for Grouping
                            string compareKey = GetCompareKey(row, groupingCriteria, reconType, out GroupingErrorMessage);
                            //if GroupingErrorMessage is not empty tha show the error message and halt the group process
                            if (!string.IsNullOrEmpty(GroupingErrorMessage.ToString()))
                            {
                                MessageBox.Show(GroupingErrorMessage.ToString(), "Grouping Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                            if (!dictGroupedRows.ContainsKey(compareKey))
                            {
                                dictGroupedRows.Add(compareKey, row);
                            }
                            else
                            {
                                DataRow targetRow = dictGroupedRows[compareKey];
                                if (reconType.Equals(ReconType.PNL))
                                {
                                    MergePNLRows(targetRow, row, dictGroupingSummary);
                                    deletdRows.Add(row);
                                }
                                else
                                {
                                    MergeRows(targetRow, row, reconType, dictGroupingSummary);
                                    //MergeRowsBySummary(targetRow, row, reconType, DictGroupingSummary);
                                    deletdRows.Add(row);
                                }
                            }
                        }
                        foreach (DataRow row in deletdRows)
                        {
                            dt.Rows.Remove(row);
                        }
                    }
                    AdjustDataBasedonReconType(ref dt, groupingCriteria, reconType);
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
        /// <param name="dt"></param>
        /// <param name="groupingCrteria"></param>
        /// <param name="reconType"></param>
        private static void AdjustDataBasedonReconType(ref DataTable dt, GroupingCriteria groupingCrteria, ReconType reconType)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    double qty = 0;
                    //double MktValue = 0;
                    //double MktValueBase = 0;
                    if (row.Table.Columns.Contains("Quantity"))
                    {
                        double.TryParse(row["Quantity"].ToString(), out qty);
                    }
                    //setting the side based on new grouped Qty..
                    if (reconType.Equals(ReconType.Position) || reconType.Equals(ReconType.TaxLot))
                    {
                        UpdateRowForPositionRecon(row, qty);
                    }
                    else if (reconType.Equals(ReconType.Transaction))
                    {
                        UpdateRowForTransactioRecon(groupingCrteria, row, qty);
                    }
                    else if (reconType.Equals(ReconType.PNL))
                    {
                        //TODO:
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
        /// This method handles Transaction Recon
        /// </summary>
        /// <param name="groupingCrteria"></param>
        /// <param name="row"></param>
        /// <param name="qty"></param>
        private static void UpdateRowForTransactioRecon(GroupingCriteria groupingCrteria, DataRow row, double qty)
        {
            try
            {
                UpdateRowGrouppingAccordingGruppingCriteria(groupingCrteria, row, qty);
                UpdateRowGrouppingAccordingAsset(row);
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
        /// <param name="row"></param>
        private static void UpdateRowGrouppingAccordingAsset(DataRow row)
        {
            try
            {
                // http://jira.nirvanasolutions.com:8080/browse/lazard-3
                // Fixed Income Handling in Recon UI
                // Currently in Nirvana calculation is coming by this formula (Quantity*Avg. Price or Mark Price). But as you all know it should be dividing by 100. 
                if (row.Table.Columns.Contains("Asset"))
                {
                    if (row["Asset"].Equals(AssetCategory.FixedIncome.ToString()) || row["Asset"].Equals(AssetCategory.ConvertibleBond.ToString()))
                    {
                        double value = 0;
                        if (row.Table.Columns.Contains("NetNotionalValue"))
                        {
                            if ((double.TryParse(row["NetNotionalValue"].ToString(), out value)))
                                row["NetNotionalValue"] = value;
                        }
                        if (row.Table.Columns.Contains("NetNotionalValueBase"))
                        {
                            if ((double.TryParse(row["NetNotionalValueBase"].ToString(), out value)))
                                row["NetNotionalValueBase"] = value;
                        }
                        if (row.Table.Columns.Contains("GrossNotionalValue"))
                        {
                            if ((double.TryParse(row["GrossNotionalValue"].ToString(), out value)))
                                row["GrossNotionalValue"] = value;
                        }
                        if (row.Table.Columns.Contains("GrossNotionalValueBase"))
                        {
                            if ((double.TryParse(row["GrossNotionalValueBase"].ToString(), out value)))
                                row["GrossNotionalValueBase"] = value;
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
        /// 
        /// </summary>
        /// <param name="groupingCrteria"></param>
        /// <param name="row"></param>
        /// <param name="qty"></param>
        private static void UpdateRowGrouppingAccordingGruppingCriteria(GroupingCriteria groupingCrteria, DataRow row, double qty)
        {
            try
            {
                if (groupingCrteria.DictGroupingCriteria.ContainsValue(true))
                {
                    // for transaction Recon the quantity and other values are displayed without being side multiplier adjusted... 
                    int indexNotionalValue = row.Table.Columns.IndexOf("GrossNotionalValueBase");
                    int indexNotionalValueBase = row.Table.Columns.IndexOf("GrossNotionalValue");
                    int indexNetNotionalValue = row.Table.Columns.IndexOf("NetNotionalValueBase");
                    int indexNetNotionalValueBase = row.Table.Columns.IndexOf("NetNotionalValue");

                    if (qty < 0)
                    {
                        row["Quantity"] = -qty;

                        if (indexNetNotionalValue != -1)
                        {
                            double netnotionalValue = double.Parse(row["NetNotionalValueBase"].ToString());
                            row["NetNotionalValueBase"] = netnotionalValue * (-1);
                        }

                        if (indexNotionalValue != -1)
                        {
                            double grossNotionalValue = double.Parse(row["GrossNotionalValueBase"].ToString());
                            row["GrossNotionalValueBase"] = grossNotionalValue * (-1);
                        }

                        if (indexNotionalValueBase != -1)
                        {
                            double grossNotionalValueBase = double.Parse(row["GrossNotionalValue"].ToString());
                            row["GrossNotionalValue"] = grossNotionalValueBase * (-1);
                        }
                        if (indexNetNotionalValueBase != -1)
                        {
                            double netnotionalValueBase = double.Parse(row["NetNotionalValue"].ToString());
                            row["NetNotionalValue"] = netnotionalValueBase * (-1);
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
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="qty"></param>
        private static void UpdateRowForPositionRecon(DataRow row, double qty)
        {
            try
            {
                if (qty == 0)
                {
                    // row.Delete();
                }
                else if (qty > 0)
                {
                    if (row.Table.Columns.Contains("Side"))
                    {
                        row["Side"] = "Buy";
                    }
                }
                else
                {
                    if (row.Table.Columns.Contains("Side"))
                    {
                        row["Side"] = "Sell";
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
        /// This method is used for  Update CompareKey on the basics of grouping type.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="criteria"></param>
        /// <param name="reconType"></param>
        /// <param name="GroupingErrorMessage"></param>
        /// <returns></returns>
        private static string GetCompareKey(DataRow row, GroupingCriteria criteria, ReconType reconType, out StringBuilder GroupingErrorMessage)
        {
            StringBuilder compareKey = new StringBuilder();
            GroupingErrorMessage = new StringBuilder();

            //check applied for each column existance while making compare key for grouping
            try
            {
                foreach (KeyValuePair<string, bool> entry in criteria.DictGroupingCriteria)
                {
                    //the code will enter only if the entry.key is true

                    if (criteria.DictGroupingCriteria[entry.Key] == true)
                    {
                        if (entry.Key == "Account")
                        {
                            UpdateCompareKeyForAccountGroupping(row, GroupingErrorMessage, compareKey);
                        }
                        else if (entry.Key == "Symbol")
                        {
                            UpdateCompareKeyForSymbolGroupping(row, criteria, GroupingErrorMessage, compareKey);
                        }
                        else if (entry.Key == "Side")
                        {
                            UpdateCompareKeyForSideGroupping(row, reconType, GroupingErrorMessage, compareKey);
                        }
                        else if (entry.Key == "MasterFund")
                        {
                            UpdateCompareKeyForMasterFundGroupping(row, GroupingErrorMessage, compareKey);
                        }
                        else if (entry.Key == "Broker")
                        {
                            UpdateCompareKeyForBrokerGroupping(row, GroupingErrorMessage, compareKey);
                        }
                        else
                        {
                            UpdateCompareKeyForGroupping(row, GroupingErrorMessage, compareKey, entry.Key);
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
            return compareKey.ToString();
        }

        /// <summary>
        /// This method is updates the "CompareKey" for grouping
        /// </summary>
        /// <param name="row"></param>
        /// <param name="GroupingErrorMessage"></param>
        /// <param name="compareKey"></param>
        private static void UpdateCompareKeyForGroupping(DataRow row, StringBuilder GroupingErrorMessage, StringBuilder compareKey, string columnName)
        {
            try
            {

                if (row.Table.Columns.Contains(columnName))
                {
                    //Pranay Deep 19 Oct 2015
                    //DateTime.TryParse would be true for date
                    //double.TryParse is used to hadle doubles(like 1.18) which can be parsed in date
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-11681
                    string columnData = row[columnName].ToString().Trim().ToUpper();
                    DateTime dateValue;
                    double doubleValue;

                    if ((DateTime.TryParse(columnData, out dateValue)) && (!double.TryParse(columnData, out doubleValue)))
                    {
                        columnData = dateValue.Date.ToShortDateString();
                        compareKey.Append(columnData);
                    }

                    //Here the object compareKey will be appended for other grouping criteria except "TradeDate"
                    else
                    {
                        compareKey.Append(row[columnName]);
                    }
                }
                else
                    GroupingErrorMessage.Append("Column " + columnName + " not found in table " + row.Table.TableName.ToString() + "\n");

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
        /// <param name="row"></param>
        /// <param name="GroupingErrorMessage"></param>
        /// <param name="compareKey"></param>
        private static void UpdateCompareKeyForBrokerGroupping(DataRow row, StringBuilder GroupingErrorMessage, StringBuilder compareKey)
        {
            try
            {
                if (row.Table.Columns.Contains("CounterParty"))
                    compareKey.Append(row["CounterParty"]);
                else
                    GroupingErrorMessage.Append("Column CounterParty not found in table " + row.Table.TableName.ToString() + "\n");

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
        /// <param name="row"></param>
        /// <param name="GroupingErrorMessage"></param>
        /// <param name="compareKey"></param>
        private static void UpdateCompareKeyForMasterFundGroupping(DataRow row, StringBuilder GroupingErrorMessage, StringBuilder compareKey)
        {
            try
            {
                if (row.Table.Columns.Contains("MasterFund"))
                    compareKey.Append(row["MasterFund"]);
                else
                    GroupingErrorMessage.Append("Column MasterFund not found in table " + row.Table.TableName.ToString() + "\n");

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
        /// <param name="row"></param>
        /// <param name="GroupingErrorMessage"></param>
        /// <param name="compareKey"></param>
        private static void UpdateCompareKeyForAccountGroupping(DataRow row, StringBuilder GroupingErrorMessage, StringBuilder compareKey)
        {
            try
            {
                if (row.Table.Columns.Contains("AccountName"))
                    compareKey.Append(row["AccountName"]);
                else
                    GroupingErrorMessage.Append("Column AccountName not found in table " + row.Table.TableName.ToString() + "\n");

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
        /// <param name="row"></param>
        /// <param name="reconType"></param>
        /// <param name="GroupingErrorMessage"></param>
        /// <param name="compareKey"></param>
        private static void UpdateCompareKeyForSideGroupping(DataRow row, ReconType reconType, StringBuilder GroupingErrorMessage, StringBuilder compareKey)
        {
            try
            {
                if (row.Table.Columns.Contains("SideID"))
                {
                    string sideID = row["SideID"].ToString();
                    if (reconType.Equals(ReconType.Position) || reconType.Equals(ReconType.TaxLot))
                    {
                        if (sideID.Equals(FIXConstants.SIDE_Buy) || sideID.Equals(FIXConstants.SIDE_Buy_Closed) || sideID.Equals(FIXConstants.SIDE_Buy_Open))
                        {
                            compareKey.Append("Buy");
                        }
                        else
                        {
                            compareKey.Append("Sell");
                        }
                    }
                    else
                    {
                        compareKey.Append(row["SideID"]);
                    }
                }
                else
                {
                    if (row.Table.Columns.Contains("Side"))
                        compareKey.Append(row["Side"]);
                    else
                        GroupingErrorMessage.Append("Column Side not found in table " + row.Table.TableName.ToString() + "\n");
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
        /// <param name="row"></param>
        /// <param name="criteria"></param>
        /// <param name="GroupingErrorMessage"></param>
        /// <param name="compareKey"></param>
        private static void UpdateCompareKeyForSymbolGroupping(DataRow row, GroupingCriteria criteria, StringBuilder GroupingErrorMessage, StringBuilder compareKey)
        {
            try
            {
                int AssetID = int.MinValue;
                if (row.Table.Columns.Contains("AssetID"))
                {
                    AssetID = int.Parse(row["AssetID"].ToString());
                }
                else if ((row.Table.Columns.Contains("Asset")))
                {
                    AssetID = (int)Enum.Parse(typeof(AssetCategory), row["Asset"].ToString());
                }
                SymbologyCodesForRecon symbologyCode = SymbologyCodesForRecon.Ticker;
                if (criteria.DictGroupingSymbology.ContainsKey(AssetID))
                {
                    symbologyCode = criteria.DictGroupingSymbology[AssetID];
                }

                switch (symbologyCode)
                {
                    case SymbologyCodesForRecon.Ticker:
                        if (row.Table.Columns.Contains("Symbol"))
                            compareKey.Append(row["Symbol"]);
                        else
                            GroupingErrorMessage.Append("Column Symbol not found in table " + row.Table.TableName.ToString() + "\n");
                        break;
                    case SymbologyCodesForRecon.OSIOption:
                        if (row.Table.Columns.Contains("OSI"))
                            compareKey.Append(row["OSI"]);
                        else
                            GroupingErrorMessage.Append("Column OSI not found in table " + row.Table.TableName.ToString() + "\n");
                        break;
                    case SymbologyCodesForRecon.Bloomberg:
                        if (row.Table.Columns.Contains("Bloomberg"))
                            compareKey.Append(row["Bloomberg"]);
                        else
                            GroupingErrorMessage.Append("Column Bloomberg not found in table " + row.Table.TableName.ToString() + "\n");
                        break;
                    case SymbologyCodesForRecon.IDCOOption:
                        if (row.Table.Columns.Contains("IDCO"))
                            compareKey.Append(row["IDCO"]);
                        else
                            GroupingErrorMessage.Append("Column IDCO not found in table " + row.Table.TableName.ToString() + "\n");
                        break;
                    case SymbologyCodesForRecon.SEDOL:
                        if (row.Table.Columns.Contains("SEDOL"))
                            compareKey.Append(row["SEDOL"]);
                        else
                            GroupingErrorMessage.Append("Column SEDOL not found in table " + row.Table.TableName.ToString() + "\n");
                        break;

                    case SymbologyCodesForRecon.CUSIP:
                        if (row.Table.Columns.Contains("CUSIP"))
                            compareKey.Append(row["CUSIP"]);
                        else
                            GroupingErrorMessage.Append("Column CUSIP not found in table " + row.Table.TableName.ToString() + "\n");
                        break;
                    default:
                        if (row.Table.Columns.Contains("Symbol"))
                            compareKey.Append(row["Symbol"]);
                        else
                            GroupingErrorMessage.Append("Column Symbol not found in table " + row.Table.TableName.ToString() + "\n");
                        break;
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
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        /// <param name="reconType"></param>
        /// <param name="dictGroupingSummary"></param>
        private static void MergePNLRows(DataRow row1, DataRow row2, SerializableDictionary<string, string> dictGroupingSummary)
        {
            try
            {
                //BeginningQuantity is hardcoded for the weighted avg.
                const string BeginningQuantity = "BeginningQuantity";
                int indexBegQty = row1.Table.Columns.IndexOf(BeginningQuantity);
                double BegQty1 = 0;
                double BegQty2 = 0;
                double val1 = 0;
                double val2 = 0;
                DataRow targetRow = row1.Table.NewRow();
                //operation would be performed on the target row and at the end of operation for all the columns it would be assigned to the row1.
                targetRow.ItemArray = row1.ItemArray;
                foreach (DataColumn column in row1.Table.Columns)
                {
                    if (dictGroupingSummary.ContainsKey(column.ColumnName) && row1[column.ColumnName] != System.DBNull.Value && row2[column.ColumnName] != System.DBNull.Value)
                    {
                        //check that values are double or int
                        if (double.TryParse(row1[column.ColumnName].ToString(), out val1) && double.TryParse(row2[column.ColumnName].ToString(), out val2))
                        {
                            //typecast string to double
                            double.TryParse(row1[column.ColumnName].ToString(), out val1);
                            double.TryParse(row2[column.ColumnName].ToString(), out val2);
                        }
                        switch ((Summary)(Enum.Parse(typeof(Summary), dictGroupingSummary[column.ColumnName])))
                        {
                            case Summary.None:
                                //check that row1 value and row2 value are equal or not and also check for data type string
                                if (!(row1[column.ColumnName].ToString().Equals(row2[column.ColumnName].ToString())) && column.DataType.Equals(typeof(string)))
                                    targetRow[column.ColumnName] = "Multiple";
                                break;
                            case Summary.Sum:
                                targetRow[column.ColumnName] = Convert.ToString(val1 + val2);
                                break;
                            case Summary.Max:
                                targetRow[column.ColumnName] = Convert.ToString(Math.Max(val1, val2));
                                break;
                            case Summary.Min:
                                targetRow[column.ColumnName] = Convert.ToString(Math.Min(val1, val2));
                                break;
                            case Summary.WeightedAvg:
                                if (indexBegQty != -1)
                                {
                                    //weighted avg on the basis of BeginningQuantity
                                    double.TryParse(row1[BeginningQuantity].ToString(), out BegQty1);
                                    double.TryParse(row2[BeginningQuantity].ToString(), out BegQty2);
                                    targetRow[column.ColumnName] = Convert.ToString((val1 * BegQty1 + val2 * BegQty2) / (BegQty1 + BegQty2));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                //copy calculated target row to the row1.
                row1.ItemArray = targetRow.ItemArray;
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
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        /// <param name="reconType"></param>
        private static void MergeRows(DataRow row1, DataRow row2, ReconType reconType, SerializableDictionary<string, string> dictGroupingSummary)
        {
            try
            {
                int indexQty = row1.Table.Columns.IndexOf("Quantity");
                //int indexAvgPx = row1.Table.Columns.IndexOf("AvgPX");
                //int indexMV = row1.Table.Columns.IndexOf("MarketValue");
                //int indexMVBase = row1.Table.Columns.IndexOf("MarketValueBase");//int indexContractMultiplier = row1.Table.Columns.IndexOf("Multiplier");
                //int indexSideMultiplier = row1.Table.Columns.IndexOf("SideMultiplier");
                //int indexConversionOperator = row1.Table.Columns.IndexOf("FXConversionMethodOperator");
                //int indexfxRateTradeDate = row1.Table.Columns.IndexOf("FXRate");

                // grouping is done assuming that quantity values are side multiplier adjusted otherwise the logic will fail..   
                double qty1 = 0;
                double qty2 = 0;
                //string conversionOperator = Operator.M.ToString();
                //double contractMultiplier = 1;
                int sideMultiplier1 = 1;
                int sideMultiplier2 = 1;
                //double fxrate = 0;
                if (indexQty != -1)
                {
                    double.TryParse(row1["Quantity"].ToString(), out qty1);
                    double.TryParse(row2["Quantity"].ToString(), out qty2);
                }
                // since for transaction the Quantity is not sideMultiplier Adjusted we are calculating side Multiplier so as to asjust the quantity values with sideMultiplier...
                if (reconType == ReconType.Transaction)
                {
                    sideMultiplier1 = GetSideMultiplier(row1);
                    sideMultiplier2 = GetSideMultiplier(row2);
                    if (qty1 < 0 && sideMultiplier1 == -1)
                    {
                        sideMultiplier1 = 1;
                    }
                    if (qty2 < 0 && sideMultiplier2 == -1)
                    {
                        sideMultiplier2 = 1;
                    }
                }
                //if (indexContractMultiplier != -1)
                //{
                //    double.TryParse(row1["Multiplier"].ToString(), out contractMultiplier);
                //}
                //if (indexConversionOperator != -1)
                //{
                //    conversionOperator = row1["FXConversionMethodOperator"].ToString();
                //}
                //if (indexfxRateTradeDate != -1)
                //{
                //    double.TryParse(row1["FXRate"].ToString(), out fxrate);
                //}
                if (indexQty != -1)
                {
                    //double.TryParse(row1["Quantity"].ToString(), out qty1);
                    //double.TryParse(row2["Quantity"].ToString(), out qty2);

                    qty1 = qty1 * sideMultiplier1;
                    qty2 = qty2 * sideMultiplier2;
                    double totQty = (qty1 + qty2);
                    row1["Quantity"] = totQty.ToString();
                }

                UpdateRowsNotionalValue(row1, row2, "GrossNotionalValueBase", sideMultiplier1, sideMultiplier2);
                UpdateRowsNotionalValue(row1, row2, "GrossNotionalValue", sideMultiplier1, sideMultiplier2);
                UpdateRowsNotionalValue(row1, row2, "NetNotionalValueBase", sideMultiplier1, sideMultiplier2);
                UpdateRowsNotionalValue(row1, row2, "NetNotionalValue", sideMultiplier1, sideMultiplier2);

                if (Math.Sign(qty1) != Math.Sign(qty2))
                {
                    if (Math.Abs(qty1) < Math.Abs(qty2))
                    {
                        if (row1.Table.Columns.Contains("Side"))
                        {
                            row1["Side"] = row2["Side"];
                        }
                        if (row1.Table.Columns.Contains("SideID"))
                        {
                            row1["SideID"] = row2["SideID"];
                        }
                    }
                }
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7291
                //Here we write the column for Weighted Average.
                UpdateRows(row1, row2, qty1, qty2, "AvgPX");
                UpdateRows(row1, row2, qty1, qty2, "UnitCost");
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-10069
                UpdateRows(row1, row2, qty1, qty2, "SettlementCurrencyCostBasis");
                UpdateRows(row1, row2, qty1, qty2, "SettlementCurrencyMarkPrice");
                UpdateRows(row1, row2, qty1, qty2, "SettlementCurrencyAveragePrice");
                UpdateRows(row1, row2, qty1, qty2, "ForwardPoint");
                UpdateRows(row1, row2, qty1, qty2, "MarkedFXRate");

                UpdateRowsFxRate(row1, row2, qty1, qty2);
                foreach (DataColumn column in row1.Table.Columns)
                {
                    //These check are done because there sum calculation cannot be done
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-3125
                    // while transaction recon there is an error "Value was either too large or too small for an Int32.Couldn't store <-4294967296> in UserID Column. Expected type is Int32."

                    //modified by amit 01.04.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3179
                    //Here we exclude those columns whose Weighted Average has already been done.
                    if (column.ColumnName.ToUpper() != "SIDEID" && column.ColumnName.ToUpper() != "QUANTITY" && column.ColumnName.ToUpper() != "AVGPX"
                        && column.ColumnName.ToUpper() != "MULTIPLIER" && column.ColumnName.ToUpper() != "FXRATE" && column.ColumnName.ToUpper() != "MARKPRICE"
                        && column.ColumnName.ToUpper() != "SEDOL" && column.ColumnName.ToUpper() != "CUSIP" && column.ColumnName.ToUpper() != "COUNTERPARTYID"
                        && column.ColumnName.ToUpper() != "FUNDNAME" && column.ColumnName.ToUpper() != "SYMBOL" && column.ColumnName.ToUpper() != "USERID"
                        && column.ColumnName.ToUpper() != "SIDEMULTIPLIER" && column.ColumnName.ToUpper() != "UNITCOST"
                        && column.ColumnName.ToUpper() != "SETTLEMENTCURRENCYCOSTBASIS" && column.ColumnName.ToUpper() != "SETTLEMENTCURRENCYMARKPRICE"
                        && column.ColumnName.ToUpper() != "SETTLEMENTCURRENCYAVERAGEPRICE" && column.ColumnName.ToUpper() != "SYMBOL_PK")
                    {
                        if (dictGroupingSummary.ContainsKey(column.ColumnName) && row1[column.ColumnName] != System.DBNull.Value && row2[column.ColumnName] != System.DBNull.Value)
                        {
                            double val1 = 0;
                            double val2 = 0;
                            if (double.TryParse(row1[column.ColumnName].ToString(), out val1) && double.TryParse(row2[column.ColumnName].ToString(), out val2))
                            {
                                //typecast string to double
                                double.TryParse(row1[column.ColumnName].ToString(), out val1);
                                double.TryParse(row2[column.ColumnName].ToString(), out val2);

                                if (((Summary)(Enum.Parse(typeof(Summary), dictGroupingSummary[column.ColumnName]))).Equals(Summary.Sum))
                                {
                                    CalculateSum(row1, row2, column.ColumnName);
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
        /// Update Weighted FxRate if Grouping is Applied
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        /// <param name="qty1"></param>
        /// <param name="qty2"></param>
        private static void UpdateRowsFxRate(DataRow row1, DataRow row2, double qty1, double qty2)
        {
            try
            {
                if (row1.Table.Columns.IndexOf("FxRate") != -1 && row1.Table.Columns.Contains("FxRate") && row2.Table.Columns.Contains("FxRate"))
                {
                    double fxRate = 0;
                    double fxRate1 = 0;
                    double fxRate2 = 0;
                    double.TryParse(row1["FxRate"].ToString(), out fxRate1);
                    double.TryParse(row2["FxRate"].ToString(), out fxRate2);
                    fxRate = (Math.Abs(qty1) * fxRate1 + Math.Abs(qty2) * fxRate2) / (Math.Abs(qty1) + Math.Abs(qty2));
                    row1["FxRate"] = fxRate.ToString();
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
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        /// <param name="qty1"></param>
        /// <param name="qty2"></param>
        private static void UpdateRows(DataRow row1, DataRow row2, double qty1, double qty2, string columnName)
        {
            try
            {
                if (row1.Table.Columns.IndexOf(columnName) != -1)
                {
                    double avgPrice = 0;
                    double avgPrice1 = 0;
                    double avgPrice2 = 0;
                    double.TryParse(row1[columnName].ToString(), out avgPrice1);
                    double.TryParse(row2[columnName].ToString(), out avgPrice2);

                    if (Math.Sign(qty1) == Math.Sign(qty2))
                    {
                        avgPrice = (qty1 * avgPrice1 + qty2 * avgPrice2) / (qty1 + qty2);
                    }
                    else
                    {
                        if (Math.Abs(qty1) == Math.Abs(qty2))
                        {
                            avgPrice = 0;
                        }
                        else if (Math.Abs(qty1) < Math.Abs(qty2))
                        {
                            avgPrice = avgPrice2;

                            if (row1.Table.Columns.Contains("Side"))
                            {
                                row1["Side"] = row2["Side"];
                            }
                            if (row1.Table.Columns.Contains("SideID"))
                            {
                                row1["SideID"] = row2["SideID"];
                            }
                        }
                        else
                        {
                            avgPrice = avgPrice1;
                        }
                    }
                    row1[columnName] = avgPrice.ToString();
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
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        /// <param name="columnName"></param>
        /// <param name="multiplier1"></param>
        /// <param name="multiplie2"></param>
        private static void UpdateRowsNotionalValue(DataRow row1, DataRow row2, string columnName, int multiplier1, int multiplie2)
        {
            try
            {
                int index = row1.Table.Columns.IndexOf(columnName);
                if (index != -1)
                {
                    UpdateRowValue(row1, columnName, multiplier1);
                    UpdateRowValue(row2, columnName, multiplie2);
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
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="multiplier"></param>
        private static void UpdateRowValue(DataRow row, string columnName, int multiplier)
        {
            try
            {
                double value = 0;
                double.TryParse(row[columnName].ToString(), out value);
                row[columnName] = value * multiplier;
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
        /// <param name="row"></param>
        /// <returns></returns>
        private static int GetSideMultiplier(DataRow row)
        {
            int sideMul = 1;
            try
            {
                double qty = 0;
                int indexQty = row.Table.Columns.IndexOf("Quantity");
                if (indexQty != -1)
                {
                    double.TryParse(row["Quantity"].ToString(), out qty);
                }

                if (row.Table.Columns.Contains("SideID"))
                {
                    string sideID = row["SideID"].ToString();

                    if (sideID.Equals(FIXConstants.SIDE_Buy) || sideID.Equals(FIXConstants.SIDE_Buy_Closed) || sideID.Equals(FIXConstants.SIDE_Buy_Open))
                    {
                        sideMul = 1;
                    }
                    else
                    {
                        sideMul = -1;
                    }
                }
                else if (row.Table.Columns.Contains("Side"))
                {
                    string side = row["Side"].ToString();
                    if (side.Equals("Buy") || side.Equals("Buy to Close") || side.Equals("Buy to Open"))
                    {
                        sideMul = 1;
                    }
                    else
                    {
                        sideMul = -1;
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
            return sideMul;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetRow"></param>
        /// <param name="row2"></param>
        /// <param name="fieldName"></param>
        private static void CalculateSum(DataRow targetRow, DataRow row2, string fieldName)
        {
            try
            {
                //DateTime parseResult = DateTime.MinValue;
                int fieldIndex = targetRow.Table.Columns.IndexOf(fieldName);
                if (fieldIndex != -1)
                {

                    double result = 0;
                    double.TryParse(targetRow[fieldName].ToString(), out result);
                    if (double.TryParse(targetRow[fieldName].ToString(), out result))
                    {
                        double val1 = 0;
                        double val2 = 0;
                        double totValue = double.Parse(targetRow[fieldName].ToString());
                        double.TryParse(targetRow[fieldName].ToString(), out val1);
                        double.TryParse(row2[fieldName].ToString(), out val2);
                        totValue = (val1 + val2);
                        targetRow[fieldName] = totValue.ToString();
                    }
                    else if (targetRow[fieldName] != System.DBNull.Value)
                    {
                        DataColumn column = targetRow.Table.Columns[fieldName];
                        if (column.DataType.Equals(typeof(string)))
                        {
                            if (!targetRow[fieldName].ToString().Equals(row2[fieldName]))
                            {
                                targetRow[fieldName] = "Multiple";
                            }
                        }
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

        #region commented
        /// <summary>
        /// Input parameter: Datatable, Columns to group, summary of all the columns
        /// Output: Datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="?"></param>
        /// <param name="DictGroupingSummary">This dictionary will contain all columns and their grouping summary, e.g.: Symvol:None, Qty: Sum, FXRate:Max</param>
        /// <param name="GroupingType">Grouping type (Grouping like grid or group data)</param>
        /// <returns></returns>
        //public DataTable Group(DataTable dt,List<string> columnsToGroup ,Dictionary<string, string> DictGroupingSummary,Prana.NewRecon.Enums.GroupingType grpType)
        //{
        //    DataTable dtGrouped = new DataTable();
        //    StringBuilder GroupingErrorMessage = new StringBuilder();
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return dtGrouped;
        //}
        #endregion

    }
}
