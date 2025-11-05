using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using Prana.Tools.BLL;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
namespace Prana.Reconciliation
{
    public class GroupingLogic
    {
        //public static void  Group(DataTable dt, MatchingRule rule)
        //{
        //    if (!dt.Columns.Contains("Matched"))
        //    {
        //        dt.Columns.Add("Matched");
        //    }
        //    List<DataRow> deletdRows = new List<DataRow>();
        //    List<string> columnstoMatch = new List<string>();
        //    int outerLoop = 0;
        //    for(outerLoop=0;outerLoop <dt.Rows.Count;outerLoop++)
        //    {
        //        DataRow row1 = dt.Rows[outerLoop];
        //        int innerLoop = 0;
        //        for (innerLoop = outerLoop + 1; innerLoop < dt.Rows.Count; innerLoop++)
        //        {
        //            DataRow row2 = dt.Rows[innerLoop];

        //            if (row2["Matched"].ToString() != rule.Name)
        //            {
        //                Result result = rule.IsRulePassed(row1, row2);
        //                if (result.IsPassed)
        //                {
        //                    MergeRows(row1, row2);
        //                    deletdRows.Add(row2);
        //                    row2["Matched"] = rule.Name;
        //                    //row1["Matched"] = rule.Name;
        //                    //break;
        //                }
        //            }
        //        }

        //    }
        //    foreach (DataRow row in deletdRows)
        //    {
        //        dt.Rows.Remove(row);
        //    }
        //}

        public static void Group(DataTable dt, GroupingCriteria groupingCrteria, ReconType reconType, SerializableDictionary<string, string> DictGroupingSummary)
        {
            //if (!dt.Columns.Contains("Matched"))
            //{
            //    dt.Columns.Add("Matched");
            //}
            try
            {

                if (groupingCrteria.IsGroupByFund || groupingCrteria.IsGroupBySide || groupingCrteria.IsGroupBySymbol || groupingCrteria.IsGroupByBroker || groupingCrteria.IsGroupbyMasterFund)
                {
                    Dictionary<string, DataRow> dictGroupedRows = new Dictionary<string, DataRow>();
                    List<DataRow> deletdRows = new List<DataRow>();
                    StringBuilder GroupingErrorMessage = new StringBuilder();
                    //List<string> columnstoMatch = new List<string>();
                    // int outerLoop = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = dt.Rows[i];
                        //Narendra Kumar Jangir 2102 Oct 30
                        //GroupingErrorMessage contains proper error message for Grouping
                        string compareKey = GetCompareKey(row, groupingCrteria, reconType, out GroupingErrorMessage);
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
                                MergePNLRows(targetRow, row, reconType, DictGroupingSummary);
                                deletdRows.Add(row);
                            }
                            else
                            {
                                MergeRows(targetRow, row, reconType);
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
                AdjustDataBasedonReconType(dt, groupingCrteria, reconType);
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


        private static void AdjustDataBasedonReconType(DataTable dt, GroupingCriteria groupingCrteria, ReconType reconType)
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
                        qty = double.Parse(row["Quantity"].ToString());
                    }
                    //setting the side based on new grouped Qty..
                    if (reconType.Equals(ReconType.Position))
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
                    else if (reconType.Equals(ReconType.Transaction))
                    {
                        if (groupingCrteria.IsGroupByFund || groupingCrteria.IsGroupBySide || groupingCrteria.IsGroupBySymbol || groupingCrteria.IsGroupbyMasterFund || groupingCrteria.IsGroupByBroker)
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
                        // http://jira.nirvanasolutions.com:8080/browse/lazard-3
                        // Fixed Income Handling in Recon UI
                        // Currently in Nirvana calculation is coming by this formula (Quantity*Avg. Price or Mark Price). But as you all know it should be dividing by 100. 
                        if (row.Table.Columns.Contains("Asset"))
                        {
                            if (row["Asset"].Equals(AssetCategory.FixedIncome.ToString()) || row["Asset"].Equals(AssetCategory.ConvertibleBond.ToString()))
                            {
                                if (row.Table.Columns.Contains("NetNotionalValue"))
                                {
                                    row["NetNotionalValue"] = ((double.Parse(row["NetNotionalValue"].ToString())) / 100);
                                }
                                if (row.Table.Columns.Contains("NetNotionalValueBase"))
                                {
                                    row["NetNotionalValueBase"] = ((double.Parse(row["NetNotionalValueBase"].ToString())) / 100);
                                }
                                if (row.Table.Columns.Contains("GrossNotionalValue"))
                                {
                                    row["GrossNotionalValue"] = ((double.Parse(row["GrossNotionalValue"].ToString())) / 100);
                                }
                                if (row.Table.Columns.Contains("GrossNotionalValueBase"))
                                {
                                    row["GrossNotionalValueBase"] = ((double.Parse(row["GrossNotionalValueBase"].ToString())) / 100);
                                }
                            }
                        }
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static string GetCompareKey(DataRow row, GroupingCriteria criteria, ReconType reconType, out StringBuilder GroupingErrorMessage)
        {
            StringBuilder compareKey = new StringBuilder();
            GroupingErrorMessage = new StringBuilder();
            //narendra kumar jangir 2012 Oct 29
            //check applied for each column existance while making compare key for grouping
            try
            {

                if (criteria.IsGroupBySymbol)
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
                if (criteria.IsGroupBySide)
                {
                    if (row.Table.Columns.Contains("SideID"))
                    {
                        string sideID = row["SideID"].ToString();

                        if (reconType.Equals(ReconType.Position))
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
                        if(row.Table.Columns.Contains("Side"))
                        compareKey.Append(row["Side"]);
                        else
                            GroupingErrorMessage.Append("Column Side not found in table " + row.Table.TableName.ToString() + "\n");
                    }
                }
                if (criteria.IsGroupByFund)
                {
                    if (row.Table.Columns.Contains("FundName"))
                    compareKey.Append(row["FundName"]);
                    else
                        GroupingErrorMessage.Append("Column FundName not found in table " + row.Table.TableName.ToString() + "\n");
                }
                if (criteria.IsGroupbyMasterFund)
                {
                    if (row.Table.Columns.Contains("MasterFund"))
                    compareKey.Append(row["MasterFund"]);
                    else
                        GroupingErrorMessage.Append("Column MasterFund not found in table " + row.Table.TableName.ToString() + "\n");
                }
                if (criteria.IsGroupByBroker)
                {
                    if (row.Table.Columns.Contains("CounterParty"))
                    compareKey.Append(row["CounterParty"]);
                    else
                        GroupingErrorMessage.Append("Column CounterParty not found in table " + row.Table.TableName.ToString() + "\n");
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
            return compareKey.ToString();
        }
        public static void MergePNLRows(DataRow row1, DataRow row2, ReconType reconType, SerializableDictionary<string, string> DictGroupingSummary)
        {
            try
            {
            //BeginningQuantity is hardcoded for the weighted avg.
            const string BeginningQuantity="BeginningQuantity";
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
                if (DictGroupingSummary.ContainsKey(column.ColumnName) && row1[column.ColumnName] != System.DBNull.Value && row2[column.ColumnName] != System.DBNull.Value)
                {
                    //check that values are double or int
                    if (double.TryParse(row1[column.ColumnName].ToString(), out val1) && double.TryParse(row2[column.ColumnName].ToString(), out val2))
                    {
                        //typecast string to double
                        double.TryParse(row1[column.ColumnName].ToString(), out val1);
                        double.TryParse(row2[column.ColumnName].ToString(), out val2);
                    }
                    switch ((Summary)(Enum.Parse(typeof(Summary), DictGroupingSummary[column.ColumnName])))
                    {
                        case Summary.None:
                            //check that row1 value and row2 value are equal or not and also check for data type string
                            if (!(row1[column.ColumnName].ToString().Equals(row2[column.ColumnName].ToString())) && column.DataType.Equals(typeof(string)))
                                targetRow[column.ColumnName]= "Multiple";
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
                            if (indexBegQty!=-1)
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        #region MergeRowsBySummary commented can be used in future to make grouping generic
        /*
        public static void MergeRowsBySummary(DataRow row1, DataRow row2, ReconType reconType, SerializableDictionary<string, string> DictGroupingSummary)
        {
            try
            {
                //BeginningQuantity is hardcoded for the weighted avg.
                const string Quantity = "Quantity";
                int indexQty = row1.Table.Columns.IndexOf(Quantity);
                double Qty1 = 0;
                double Qty2 = 0;
                double val1 = 0;
                double val2 = 0;
                DataRow targetRow = row1.Table.NewRow();
                //target row is the current row and row1 is the grouped row.
                targetRow.ItemArray = row1.ItemArray;
                foreach (DataColumn column in row1.Table.Columns)
                {
                    if (DictGroupingSummary.ContainsKey(column.ColumnName))
                    {
                        //if summary is none than get the values, it means that values are double or int.
                        if ((Summary)(Enum.Parse(typeof(Summary), DictGroupingSummary[column.ColumnName])) != Summary.None)
                        {
                            //typecast string to double
                            double.TryParse(row1[column.ColumnName].ToString(), out val1);
                            double.TryParse(row2[column.ColumnName].ToString(), out val2);
                        }
                        switch ((Summary)(Enum.Parse(typeof(Summary), DictGroupingSummary[column.ColumnName])))
                        {
                            case Summary.None:
                                if (!(row1[column.ColumnName].ToString().Equals(row2[column.ColumnName].ToString())) && column.ColumnName != "TradeDate")
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
                                if (indexQty != -1)
                                {
                                    //weighted avg on the basis of BeginningQuantity
                                    double.TryParse(row1[Quantity].ToString(), out Qty1);
                                    double.TryParse(row2[Quantity].ToString(), out Qty2);
                                    targetRow[column.ColumnName] = Convert.ToString((val1 * Qty1 + val2 * Qty2) / (Qty1 + Qty2));
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        */
        #endregion
        public static void MergeRows(DataRow row1, DataRow row2, ReconType reconType)
        {
            try
            {
                int indexQty = row1.Table.Columns.IndexOf("Quantity");
                int indexAvgPx = row1.Table.Columns.IndexOf("AvgPX");
                int indexMV = row1.Table.Columns.IndexOf("MarketValue");
                int indexMVBase = row1.Table.Columns.IndexOf("MarketValueBase");
                int indexNotionalValue = row1.Table.Columns.IndexOf("GrossNotionalValueBase");
                int indexNotionalValueBase = row1.Table.Columns.IndexOf("GrossNotionalValue");
                int indexNetNotionalValue = row1.Table.Columns.IndexOf("NetNotionalValueBase");
                int indexNetNotionalValueBase = row1.Table.Columns.IndexOf("NetNotionalValue");
                int indexTotalCommissionFees = row1.Table.Columns.IndexOf("TotalCommissionandFees");
                int indexTotalCommissionFeesBase = row1.Table.Columns.IndexOf("TotalCommissionandFeesBase");
                int indexContractMultiplier = row1.Table.Columns.IndexOf("Multiplier");
                int indexSideMultiplier = row1.Table.Columns.IndexOf("SideMultiplier");
                int indexConversionOperator = row1.Table.Columns.IndexOf("FXConversionMethodOperator");
                int indexfxRateTradeDate = row1.Table.Columns.IndexOf("FXRate");

                // grouping is done assuming that quantity values are side multiplier adjusted otherwise the logic will fail..   
                double qty1 = 0;
                double qty2 = 0;


                double totQty = 0;
                double avgPrice = 0;
                string conversionOperator = Operator.M.ToString();
                double contractMultiplier = 1;
                int sideMultiplier1 = 1;
                int sideMultiplier2 = 1;
                double fxrate = 0;
                double totalCommission = 0;
                double totalCommissionBase = 0;
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
                if (indexContractMultiplier != -1)
                {
                    double.TryParse(row1["Multiplier"].ToString(), out contractMultiplier);
                }
                if (indexConversionOperator != -1)
                {
                    conversionOperator = row1["FXConversionMethodOperator"].ToString();
                }
                if (indexfxRateTradeDate != -1)
                {
                    double.TryParse(row1["FXRate"].ToString(), out fxrate);
                }
                if (indexQty != -1)
                {
                    //double.TryParse(row1["Quantity"].ToString(), out qty1);
                    //double.TryParse(row2["Quantity"].ToString(), out qty2);

                    qty1 = qty1 * sideMultiplier1;
                    qty2 = qty2 * sideMultiplier2;
                    totQty = (qty1 + qty2);
                    row1["Quantity"] = totQty.ToString();
                }

                if (indexNotionalValue != -1)
                {
                    double grossNotionalValue1 = 0;
                    double grossNotionalValue2 = 0;

                    double.TryParse(row1["GrossNotionalValueBase"].ToString(), out grossNotionalValue1);
                    double.TryParse(row2["GrossNotionalValueBase"].ToString(), out grossNotionalValue2);

                    row1["GrossNotionalValueBase"] = grossNotionalValue1 * sideMultiplier1;
                    row2["GrossNotionalValueBase"] = grossNotionalValue2 * sideMultiplier2;

                }
                if (indexNotionalValueBase != -1)
                {

                    double grossNotionalValueBase1 = 0;
                    double grossNotionalValueBase2 = 0;

                    double.TryParse(row1["GrossNotionalValue"].ToString(), out grossNotionalValueBase1);
                    double.TryParse(row2["GrossNotionalValue"].ToString(), out grossNotionalValueBase2);

                    row1["GrossNotionalValue"] = grossNotionalValueBase1 * sideMultiplier1;
                    row2["GrossNotionalValue"] = grossNotionalValueBase2 * sideMultiplier2;
                }


                if (indexNetNotionalValue != -1)
                {
                    double NetNotionalValue1 = 0;
                    double NetNotionalValue2 = 0;

                    double.TryParse(row1["NetNotionalValueBase"].ToString(), out NetNotionalValue1);
                    double.TryParse(row2["NetNotionalValueBase"].ToString(), out NetNotionalValue2);

                    row1["NetNotionalValueBase"] = NetNotionalValue1 * sideMultiplier1;
                    row2["NetNotionalValueBase"] = NetNotionalValue2 * sideMultiplier2;

                }
                if (indexNetNotionalValueBase != -1)
                {
                    double NetNotionalValueBase1 = 0;
                    double NetNotionalValueBase2 = 0;

                    double.TryParse(row1["NetNotionalValue"].ToString(), out NetNotionalValueBase1);
                    double.TryParse(row2["NetNotionalValue"].ToString(), out NetNotionalValueBase2);

                    row1["NetNotionalValue"] = NetNotionalValueBase1 * sideMultiplier1;
                    row2["NetNotionalValue"] = NetNotionalValueBase2 * sideMultiplier2;

                }

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
                if (indexAvgPx != -1)
                {
                    double avgPrice1 = 0;
                    double avgPrice2 = 0;
                    double.TryParse(row1["AvgPX"].ToString(), out avgPrice1);
                    double.TryParse(row2["AvgPX"].ToString(), out avgPrice2);

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
                    row1["AvgPX"] = avgPrice.ToString();
                }
                foreach (DataColumn column in row1.Table.Columns)
                {
                    //These check are done because there sum calculation cannot be done
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-3125
                    // while transaction recon there is an error "Value was either too large or too small for an Int32.Couldn't store <-4294967296> in UserID Column. Expected type is Int32."
                    if (column.ColumnName.ToUpper() != "SIDEID" && column.ColumnName.ToUpper() != "QUANTITY" && column.ColumnName.ToUpper() != "AVGPX" && column.ColumnName.ToUpper() != "MULTIPLIER" && column.ColumnName.ToUpper() != "FXRATE" && column.ColumnName.ToUpper() != "MARKPRICE" && column.ColumnName.ToUpper() != "SEDOL" && column.ColumnName.ToUpper() != "CUSIP" && column.ColumnName.ToUpper() != "COUNTERPARTYID" && column.ColumnName.ToUpper() != "FUNDNAME" && column.ColumnName.ToUpper() != "SYMBOL" && column.ColumnName.ToUpper() != "USERID")
                    {
                        CalculateSum(row1, row2, column.ColumnName);
                    }
                }
                #region commented
                //if (indexTotalCommissionFees != -1)
                //{
                //    double.TryParse(row1["TotalCommissionandFees"].ToString(), out totalCommission);
                //}
                //if (indexTotalCommissionFeesBase != -1)
                //{
                //    double.TryParse(row1["TotalCommissionandFeesBase"].ToString(), out totalCommissionBase);
                //}


                //if (totQty > 0)
                //{
                //    if (indexNetNotionalValue != -1)
                //    {
                //        row1["NetNotionalValue"] = (avgPrice * totQty * contractMultiplier) + totalCommission;

                //    }
                //    if (indexNotionalValueBase != -1)
                //    {
                //        if (conversionOperator.Equals(Operator.M.ToString()))
                //        {
                //            row1["NetNotionalValueBase"] = ((avgPrice * totQty * contractMultiplier) + totalCommission) * fxrate;
                //        }
                //        else if (conversionOperator.Equals(Operator.D.ToString()) && fxrate > 0)
                //        {
                //            row1["NetNotionalValueBase"] = ((avgPrice * totQty * contractMultiplier) + totalCommission) / fxrate;
                //        }
                //    }
                //}
                //else
                //{
                //    if (indexNetNotionalValue != -1)
                //    {
                //        row1["NetNotionalValue"] = (avgPrice * totQty * contractMultiplier) - totalCommission;

                //    }
                //    if (indexNotionalValueBase != -1)
                //    {
                //        if (conversionOperator.Equals(Operator.M.ToString()))
                //        {
                //            row1["NetNotionalValueBase"] = ((avgPrice * totQty * contractMultiplier) - totalCommission) * fxrate;
                //        }
                //        else if (conversionOperator.Equals(Operator.D.ToString()) && fxrate > 0)
                //        {
                //            row1["NetNotionalValueBase"] = ((avgPrice * totQty * contractMultiplier) - totalCommission) / fxrate;
                //        }

                //    }

                //}
                #endregion
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return sideMul;
                }






        private static void CalculateSum(DataRow targetRow, DataRow row2, string fieldName)
        {
            try
            {
           DateTime parseResult = DateTime.MinValue;
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        //private static void CalculateGroupedValue(object value1, object value2, GroupingMethod method)
        //{

        //    double val1 = 0;
        //    double val2 = 0;

        //    double.TryParse(value1.ToString(), out val1);
        //    double.TryParse(value2.ToString(), out val2);




        //}
    }
}
