using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
namespace Prana.Tools
{
    public class NewUtilities
    {
        public static string GiveSideAfterIgnoringOpenCloseSide(string side)
        {
            switch (side)
            {
                case FIXConstants.SIDE_Buy_Open:
                case FIXConstants.SIDE_Buy_Closed:
                case FIXConstants.SIDE_Buy_Cover:
                case FIXConstants.SIDE_BuyMinus:
                    side = FIXConstants.SIDE_Buy;
                    break;

                case FIXConstants.SIDE_Sell:
                case FIXConstants.SIDE_Sell_Closed:
                case FIXConstants.SIDE_Sell_Open:
                case FIXConstants.SIDE_SellPlus:
                case FIXConstants.SIDE_SellShort:
                    side = FIXConstants.SIDE_Sell;
                    break;

            }
            return side;
        }
        //internal static void GetSMData(Dictionary<int, List<string>> dictUniqueSymbols, DataTable dt, int hashCode)
        //{
        //    try
        //    {
        //        if (dictUniqueSymbols.Count > 0)
        //        {
        //            SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

        //            foreach (KeyValuePair<int, List<string>> kvp in dictUniqueSymbols)
        //            {
        //                ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

        //                List<string> symbolList = dictUniqueSymbols[kvp.Key];

        //                if (symbolList.Count > 0)
        //                {
        //                    foreach (string symbol in symbolList)
        //                    {
        //                        secMasterRequestObj.AddData(symbol, symbology);

        //                        //  secMasterRequestObj.AddNewRow();
        //                    }
        //                }
        //            }

        //           secMasterRequestObj.HashCode = hashCode;
        //           List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

        //            if (secMasterCollection != null && secMasterCollection.Count > 0)
        //            {
        //                foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
        //                {
        //                    string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

        //                    int requestedSymbologyID = secMasterObj.RequestedSymbology;
        //                    string requestedSymbology = "Symbol";
        //                    switch (requestedSymbologyID)
        //                    {
        //                        case 0:
        //                            requestedSymbology = "Symbol";
        //                            break;
        //                        case 1:
        //                            requestedSymbology = "RIC";
        //                            break;
        //                        case 2:
        //                            requestedSymbology = "ISIN";
        //                            break;
        //                        case 3:
        //                            requestedSymbology = "SEDOL";
        //                            break;
        //                        case 4:
        //                            requestedSymbology = "CUSIP";
        //                            break;
        //                        case 5:
        //                            requestedSymbology = "Bloomberg";
        //                            break;
        //                        case 6:
        //                            requestedSymbology = "OSIOptionSymbol";
        //                            break;
        //                        case 7:
        //                            requestedSymbology = "IDCOOptionSymbol";
        //                            break;
        //                        case 8:
        //                            requestedSymbology = "OPRAOptionSymbol";
        //                            break;

        //                        default:
        //                            break;
        //                    }

        //                    if (dictUniqueSymbols.ContainsKey(requestedSymbologyID))
        //                    {
        //                        List<string> symbolList = dictUniqueSymbols[requestedSymbologyID];
        //                        if (symbolList.Contains(secMasterObj.RequestedSymbol))
        //                        {
        //                            DataRow[] rows = dt.Select(requestedSymbology + " = '" + secMasterObj.RequestedSymbol + "'");
        //                            foreach (DataRow dataRow in rows)
        //                            {
        //                                dataRow["Symbol"] = pranaSymbol;
        //                                // dataRow["CompanyName"] = secMasterObj.LongName;
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="dt"></param>
        internal static void AddPrimaryKey(DataTable dt)
        {
            try
            {
                if (!dt.Columns.Contains("RowID"))
                {
                    dt.Columns.Add("RowID");
                    int rowID = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        row["RowID"] = rowID;
                        rowID++;
                    }
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["RowID"] };
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static DataTable CreateDataTableFromDictionary(Dictionary<int, string> dict, string id, string name)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(id);
                dt.Columns.Add(name);
                dt.Rows.Add(new object[] { int.MinValue.ToString(), ApplicationConstants.C_COMBO_SELECT });
                foreach (KeyValuePair<int, string> item in dict)
                {
                    dt.Rows.Add(new object[] { item.Key, item.Value });
                }
                return dt;
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
            return null;
        }
        internal static DataTable GetSelectedRows(UltraGrid grid, List<string> names)
        {
            DataTable tempData = new DataTable();

            try
            {
                UltraGridRow[] rows = grid.Rows.GetFilteredInNonGroupByRows();

                tempData.TableName = "SecMasterTable";
                if (names == null)
                {
                    names = new List<string>();
                    foreach (UltraGridColumn column in grid.DisplayLayout.Bands[0].Columns)
                    {
                        names.Add(column.Header.Caption);
                    }
                }
                foreach (string column in names)
                {
                    tempData.Columns.Add(column);
                }
                foreach (UltraGridRow row in rows)
                {
                    object[] tempRow = new object[names.Count];
                    int i = 0;
                    foreach (string column in names)
                    {
                        tempRow[i] = row.Cells[column].Value;
                        i++;
                    }
                    tempData.Rows.Add(tempRow);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return tempData;
        }
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="lstMsgOrig"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static List<PranaMessage> RemoveDuplicateRows(List<PranaMessage> lstMsgOrig, List<string> tags)
        {
            try
            {
                List<PranaMessage> duplicateMsgs = new List<PranaMessage>();
                if (lstMsgOrig != null && tags != null)
                {
                    Dictionary<string, PranaMessage> _dict = new Dictionary<string, PranaMessage>();
                    foreach (PranaMessage pranaMessage in lstMsgOrig)
                    {
                        string keyValue = string.Empty;
                        foreach (string tag in tags)
                        {
                            keyValue += pranaMessage.FIXMessage.InternalInformation[tag].Value;
                        }

                        if (!_dict.ContainsKey(keyValue))
                        {
                            _dict.Add(keyValue, pranaMessage);
                        }
                        else
                        {
                            duplicateMsgs.Add(pranaMessage);
                        }
                    }

                    lstMsgOrig.Clear();

                    foreach (KeyValuePair<string, PranaMessage> item in _dict)
                    {
                        lstMsgOrig.Add(item.Value);
                    }
                    return duplicateMsgs;
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
            return null;
        }
        //public static void CopyColumns(DataTable dtCopyFrom, DataTable dtCopyTo)
        //{
        //    if (dtCopyFrom != null && dtCopyTo != null)
        //    {
        //        DataColumn[] columnNameList = new DataColumn[dtCopyFrom.Columns.Count];
        //        dtCopyFrom.Columns.CopyTo(columnNameList, 0);
        //        foreach (DataColumn column in columnNameList)
        //        {
        //            if (!dtCopyTo.Columns.Contains(column.ColumnName))
        //                dtCopyTo.Columns.Add(column.ColumnName);
        //        }
        //    }
        //}
        static ISecurityMasterServices _securityMaster = null;
        public static ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
            }
            get
            {
                return _securityMaster;
            }
        }
    }
}
