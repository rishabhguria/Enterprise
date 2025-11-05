using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public class AllocationSchemeImportHelper
    {
        static AllocationSchemeImportHelper()
        {
            CreateAllocationServicesProxy();
        }

        static ProxyBase<IAllocationManager> _allocationServices = null;
        /// <summary>
        /// create allocation scheme services proxy to save data 
        /// </summary>
        private static void CreateAllocationServicesProxy()
        {
            _allocationServices = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");

        }

        static Dictionary<int, Dictionary<string, List<DataRow>>> _allocationSchemeSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<DataRow>>>();
        /// <summary>
        /// this collection used to validate symbols from Security Master
        /// it contaions symbologywise data
        /// </summary>
        public static Dictionary<int, Dictionary<string, List<DataRow>>> AllocationSchemeSymbologyWiseDict
        {
            get { return _allocationSchemeSymbologyWiseDict; }
            set { _allocationSchemeSymbologyWiseDict = value; }
        }

        public static DataSet UpdateAllocationSchemeValueCollection(DataSet ds, List<string> currencyListForAlloScheme, bool isInternalData)
        {
            DataSet dsAllocationScheme = null;
            try
            {
                _allocationSchemeSymbologyWiseDict.Clear();
                // this is for Normal Trade Allocation
                Dictionary<string, double> AllocSchemeKeywiseTotalQtyDictForAllocationScheme = new Dictionary<string, double>();

                // this is for Symbol and Side key Allocation
                Dictionary<string, double> symbolSidewithTotalQtyDictForAllocationScheme = new Dictionary<string, double>();

                // this is for Swap Non AUD Trade Allocation
                //Dictionary<string, double> PBSymbolSideWithTotalQtyDictForAllocationScheme = new Dictionary<string, double>();

                Dictionary<string, DataRow> duplicateAccountSymbolSideDict = new Dictionary<string, DataRow>();

                DataTable dtAllocationScheme = ds.Tables[0];

                //allocation scheme based on TickerSymbol, SEDOL, Bloomberg or any other symbology type
                string AllocationBasedOnSymbolType = string.Empty;
                bool blnAllocationBasedOn = dtAllocationScheme.Columns.Contains("AllocationBasedOn");
                if (blnAllocationBasedOn)
                {
                    AllocationBasedOnSymbolType = dtAllocationScheme.Rows[0]["AllocationBasedOn"].ToString();
                }

                AllocationSchemeKey AllocSchemeKey = AllocationSchemeKey.Symbol;
                //allocation scheme key can be Symbol or SymbolSide or PBSymbolSide
                string AllocSchemeKeyName = string.Empty;
                //check whether alloc scheme key exists or not
                bool blnAllocationSchemeKey = dtAllocationScheme.Columns.Contains("AllocationSchemeKey");
                if (blnAllocationSchemeKey)
                {
                    AllocSchemeKeyName = dtAllocationScheme.Rows[0]["AllocationSchemeKey"].ToString();
                    // cast allocation scheme key into ASchemeKey enum as we get it from XSLT mapped data table
                    AllocSchemeKey = (AllocationSchemeKey)Enum.Parse(typeof(AllocationSchemeKey), AllocSchemeKeyName);
                }

                //these are the required columns for allocation scheme
                if (!dtAllocationScheme.Columns.Contains("Validated"))
                {
                    DataColumn dcValidated = new DataColumn("Validated");
                    dcValidated.DataType = typeof(string);
                    dcValidated.DefaultValue = "NotValidated";
                    dtAllocationScheme.Columns.Add(dcValidated);
                }

                if (!dtAllocationScheme.Columns.Contains("Symbol"))
                {
                    DataColumn dcTicker = new DataColumn("Symbol");
                    dcTicker.DataType = typeof(string);
                    dcTicker.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcTicker);
                }
                if (!dtAllocationScheme.Columns.Contains("RIC"))
                {
                    DataColumn dcRIC = new DataColumn("RIC");
                    dcRIC.DataType = typeof(string);
                    dcRIC.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcRIC);
                }
                if (!dtAllocationScheme.Columns.Contains("ISIN"))
                {
                    DataColumn dcISIN = new DataColumn("ISIN");
                    dcISIN.DataType = typeof(string);
                    dcISIN.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcISIN);
                }
                if (!dtAllocationScheme.Columns.Contains("SEDOL"))
                {
                    DataColumn dcSEDOL = new DataColumn("SEDOL");
                    dcSEDOL.DataType = typeof(string);
                    dcSEDOL.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcSEDOL);
                }
                if (!dtAllocationScheme.Columns.Contains("CUSIP"))
                {
                    DataColumn dcCUSIP = new DataColumn("CUSIP");
                    dcCUSIP.DataType = typeof(string);
                    dcCUSIP.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcCUSIP);
                }
                if (!dtAllocationScheme.Columns.Contains("Bloomberg"))
                {
                    DataColumn dcBloomberg = new DataColumn("Bloomberg");
                    dcBloomberg.DataType = typeof(string);
                    dcBloomberg.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcBloomberg);
                }

                if (!dtAllocationScheme.Columns.Contains("OSIOptionSymbol"))
                {
                    DataColumn dcOSIOptionSymbol = new DataColumn("OSIOptionSymbol");
                    dcOSIOptionSymbol.DataType = typeof(string);
                    dcOSIOptionSymbol.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcOSIOptionSymbol);
                }

                if (!dtAllocationScheme.Columns.Contains("IDCOOptionSymbol"))
                {
                    DataColumn dcIDCOOptionSymbol = new DataColumn("IDCOOptionSymbol");
                    dcIDCOOptionSymbol.DataType = typeof(string);
                    dcIDCOOptionSymbol.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcIDCOOptionSymbol);
                }

                if (!dtAllocationScheme.Columns.Contains("LongName"))
                {
                    DataColumn dcLongName = new DataColumn("LongName");
                    dcLongName.DataType = typeof(string);
                    dcLongName.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcLongName);
                }

                if (!dtAllocationScheme.Columns.Contains("FundID"))
                {
                    DataColumn dcAccountID = new DataColumn("FundID");
                    dcAccountID.DataType = typeof(Int32);
                    dcAccountID.DefaultValue = 0;
                    dtAllocationScheme.Columns.Add(dcAccountID);
                }
                if (!dtAllocationScheme.Columns.Contains("RoundLot"))
                {
                    DataColumn dcRoundLot = new DataColumn("RoundLot", typeof(Int32));
                    dcRoundLot.DefaultValue = 0;
                    dtAllocationScheme.Columns.Add(dcRoundLot);
                }

                if (!dtAllocationScheme.Columns.Contains("Percentage"))
                {
                    DataColumn dcPercentage = new DataColumn("Percentage");
                    dcPercentage.DataType = typeof(double);
                    dcPercentage.DefaultValue = 0;
                    dtAllocationScheme.Columns.Add(dcPercentage);
                }
                if (!dtAllocationScheme.Columns.Contains("OrderSideTagValue"))
                {
                    DataColumn dcOrderSideTagValue = new DataColumn("OrderSideTagValue");
                    dcOrderSideTagValue.DataType = typeof(string);
                    dcOrderSideTagValue.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcOrderSideTagValue);
                }
                if (!dtAllocationScheme.Columns.Contains("Side"))
                {
                    DataColumn dcSide = new DataColumn("Side");
                    dcSide.DataType = typeof(string);
                    dcSide.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcSide);
                }

                if (!dtAllocationScheme.Columns.Contains("IsDuplicate"))
                {
                    DataColumn dcIsDuplicate = new DataColumn("IsDuplicate");
                    dcIsDuplicate.DataType = typeof(bool);
                    dcIsDuplicate.DefaultValue = false;
                    dtAllocationScheme.Columns.Add(dcIsDuplicate);
                }

                if (!dtAllocationScheme.Columns.Contains("TotalQty"))
                {
                    DataColumn dcTotalQty = new DataColumn("TotalQty", typeof(double));
                    dcTotalQty.DefaultValue = 0;
                    dtAllocationScheme.Columns.Add(dcTotalQty);
                }
                // this column is used to differentiate whether the trade is swap or not
                if (!dtAllocationScheme.Columns.Contains("TradeType"))
                {
                    DataColumn dcTradeType = new DataColumn("TradeType", typeof(string));
                    dcTradeType.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcTradeType);
                }

                if (!dtAllocationScheme.Columns.Contains("Currency"))
                {
                    DataColumn dcCurrency = new DataColumn("Currency", typeof(string));
                    dcCurrency.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcCurrency);
                }



                if (!dtAllocationScheme.Columns.Contains("PB"))
                {
                    DataColumn dcPB = new DataColumn("PB", typeof(string));
                    dcPB.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcPB);
                }

                if (!dtAllocationScheme.Columns.Contains("IsSymbolValidatedFromSM"))
                {
                    DataColumn dcIsSymbolValidatedFromSM = new DataColumn("IsSymbolValidatedFromSM", typeof(string));
                    //if data fetched from database, then there is no need to validate from Security Master
                    if (isInternalData)
                    {
                        dcIsSymbolValidatedFromSM.DefaultValue = "Validated";
                    }
                    else
                    {
                        dcIsSymbolValidatedFromSM.DefaultValue = "NotValidated";
                    }
                    dtAllocationScheme.Columns.Add(dcIsSymbolValidatedFromSM);
                }

                if (!dtAllocationScheme.Columns.Contains("AllocationSchemeKey"))
                {
                    DataColumn dcAllocationSchemeKey = new DataColumn("AllocationSchemeKey", typeof(string));
                    dcAllocationSchemeKey.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcAllocationSchemeKey);
                }
                // To set rowIndex for merging in case of existing import xml
                if (!dtAllocationScheme.Columns.Contains("RowIndex"))
                {
                    DataColumn dcRowIndex = new DataColumn("RowIndex", typeof(Int32));
                    dcRowIndex.DefaultValue = 0;
                    dtAllocationScheme.Columns.Add(dcRowIndex);
                }
                if (!dtAllocationScheme.Columns.Contains("ImportStatus"))
                {
                    DataColumn dcImpStatus = new DataColumn("ImportStatus", typeof(string));
                    dcImpStatus.DefaultValue = ImportStatus.None.ToString();
                    dtAllocationScheme.Columns.Add(dcImpStatus);
                }
                if (!dtAllocationScheme.Columns.Contains("ValidationError"))
                {
                    DataColumn dcValidationError = new DataColumn("ValidationError");
                    dcValidationError.DataType = typeof(string);
                    dcValidationError.DefaultValue = string.Empty;
                    dtAllocationScheme.Columns.Add(dcValidationError);
                }
                ds.AcceptChanges();
                dsAllocationScheme = ds.Clone();
                // Purpose : In case of ImportIntoApplication tableName is ImportData instead of ""PositionMaster"
                string tableName = ds.Tables[0].TableName;
                // fill values on the basis of IDs and collect data for Security Master request
                //
                foreach (DataRow row in dtAllocationScheme.Rows)
                {
                    //DataRow dr = dsAllocationScheme.Tables["PositionMaster"].NewRow();
                    DataRow dr = dsAllocationScheme.Tables[tableName].NewRow();

                    row["RowIndex"] = dtAllocationScheme.Rows.IndexOf(row);
                    row["ImportStatus"] = ImportStatus.NotImported.ToString();

                    if (!String.IsNullOrEmpty(row["OrderSideTagValue"].ToString().Trim()))
                    {
                        row["Side"] = TagDatabaseManager.GetInstance.GetOrderSideText(row["OrderSideTagValue"].ToString().Trim());
                    }

                    if (!String.IsNullOrEmpty(row["FundName"].ToString().Trim()))
                    {
                        row["FundID"] = CachedDataManager.GetInstance.GetAccountID(row["FundName"].ToString().Trim());
                    }

                    dr.ItemArray = row.ItemArray;
                    dsAllocationScheme.Tables[tableName].Rows.Add(dr);


                    #region Security Master request

                    if (!String.IsNullOrEmpty(row["Symbol"].ToString()))
                    {
                        if (_allocationSchemeSymbologyWiseDict.ContainsKey(0))
                        {
                            Dictionary<string, List<DataRow>> positionMasterSameSymbologyDict = _allocationSchemeSymbologyWiseDict[0];
                            if (positionMasterSameSymbologyDict.ContainsKey(row["Symbol"].ToString()))
                            {
                                List<DataRow> positionMasterSymbolWiseList = positionMasterSameSymbologyDict[row["Symbol"].ToString()];
                                positionMasterSymbolWiseList.Add(dr);
                                positionMasterSameSymbologyDict[row["Symbol"].ToString()] = positionMasterSymbolWiseList;
                                _allocationSchemeSymbologyWiseDict[0] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<DataRow> positionMasterlist = new List<DataRow>();
                                positionMasterlist.Add(dr);
                                _allocationSchemeSymbologyWiseDict[0].Add(row["Symbol"].ToString(), positionMasterlist);
                            }
                        }
                        else
                        {
                            List<DataRow> positionMasterlist = new List<DataRow>();
                            positionMasterlist.Add(dr);
                            Dictionary<string, List<DataRow>> positionMasterSameSymbolDict = new Dictionary<string, List<DataRow>>();
                            positionMasterSameSymbolDict.Add(row["Symbol"].ToString(), positionMasterlist);
                            _allocationSchemeSymbologyWiseDict.Add(0, positionMasterSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(row["RIC"].ToString()))
                    {
                        if (_allocationSchemeSymbologyWiseDict.ContainsKey(1))
                        {
                            Dictionary<string, List<DataRow>> positionMasterSameSymbologyDict = _allocationSchemeSymbologyWiseDict[1];
                            if (positionMasterSameSymbologyDict.ContainsKey(row["RIC"].ToString()))
                            {
                                List<DataRow> positionMasterSymbolWiseList = positionMasterSameSymbologyDict[row["RIC"].ToString()];
                                positionMasterSymbolWiseList.Add(dr);
                                positionMasterSameSymbologyDict[row["RIC"].ToString()] = positionMasterSymbolWiseList;
                                _allocationSchemeSymbologyWiseDict[1] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<DataRow> positionMasterlist = new List<DataRow>();
                                positionMasterlist.Add(dr);
                                _allocationSchemeSymbologyWiseDict[1].Add(row["RIC"].ToString(), positionMasterlist);
                            }
                        }
                        else
                        {
                            List<DataRow> positionMasterlist = new List<DataRow>();
                            positionMasterlist.Add(dr);
                            Dictionary<string, List<DataRow>> positionMasterSameSymbolDict = new Dictionary<string, List<DataRow>>();
                            positionMasterSameSymbolDict.Add(row["RIC"].ToString(), positionMasterlist);
                            _allocationSchemeSymbologyWiseDict.Add(1, positionMasterSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(row["ISIN"].ToString()))
                    {
                        if (_allocationSchemeSymbologyWiseDict.ContainsKey(2))
                        {
                            Dictionary<string, List<DataRow>> positionMasterSameSymbologyDict = _allocationSchemeSymbologyWiseDict[2];
                            if (positionMasterSameSymbologyDict.ContainsKey(row["ISIN"].ToString()))
                            {
                                List<DataRow> positionMasterSymbolWiseList = positionMasterSameSymbologyDict[row["RIC"].ToString()];
                                positionMasterSymbolWiseList.Add(dr);
                                positionMasterSameSymbologyDict[row["ISIN"].ToString()] = positionMasterSymbolWiseList;
                                _allocationSchemeSymbologyWiseDict[2] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<DataRow> positionMasterlist = new List<DataRow>();
                                positionMasterlist.Add(dr);
                                _allocationSchemeSymbologyWiseDict[2].Add(row["ISIN"].ToString(), positionMasterlist);
                            }
                        }
                        else
                        {
                            List<DataRow> positionMasterlist = new List<DataRow>();
                            positionMasterlist.Add(dr);
                            Dictionary<string, List<DataRow>> positionMasterSameSymbolDict = new Dictionary<string, List<DataRow>>();
                            positionMasterSameSymbolDict.Add(row["ISIN"].ToString(), positionMasterlist);
                            _allocationSchemeSymbologyWiseDict.Add(2, positionMasterSameSymbolDict);
                        }

                    }
                    else if (!String.IsNullOrEmpty(row["SEDOL"].ToString()))
                    {
                        if (_allocationSchemeSymbologyWiseDict.ContainsKey(3))
                        {
                            Dictionary<string, List<DataRow>> positionMasterSameSymbologyDict = _allocationSchemeSymbologyWiseDict[3];
                            if (positionMasterSameSymbologyDict.ContainsKey(row["SEDOL"].ToString()))
                            {
                                List<DataRow> positionMasterSymbolWiseList = positionMasterSameSymbologyDict[row["SEDOL"].ToString()];
                                positionMasterSymbolWiseList.Add(dr);
                                positionMasterSameSymbologyDict[row["SEDOL"].ToString()] = positionMasterSymbolWiseList;
                                _allocationSchemeSymbologyWiseDict[3] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<DataRow> positionMasterlist = new List<DataRow>();
                                positionMasterlist.Add(dr);
                                _allocationSchemeSymbologyWiseDict[3].Add(row["SEDOL"].ToString(), positionMasterlist);
                            }
                        }
                        else
                        {
                            List<DataRow> positionMasterlist = new List<DataRow>();
                            positionMasterlist.Add(dr);
                            Dictionary<string, List<DataRow>> positionMasterSameSymbolDict = new Dictionary<string, List<DataRow>>();
                            positionMasterSameSymbolDict.Add(row["SEDOL"].ToString(), positionMasterlist);
                            _allocationSchemeSymbologyWiseDict.Add(3, positionMasterSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(row["CUSIP"].ToString()))
                    {
                        if (_allocationSchemeSymbologyWiseDict.ContainsKey(4))
                        {
                            Dictionary<string, List<DataRow>> positionMasterSameSymbologyDict = _allocationSchemeSymbologyWiseDict[4];
                            if (positionMasterSameSymbologyDict.ContainsKey(row["CUSIP"].ToString()))
                            {
                                List<DataRow> positionMasterSymbolWiseList = positionMasterSameSymbologyDict[row["CUSIP"].ToString()];
                                positionMasterSymbolWiseList.Add(dr);
                                positionMasterSameSymbologyDict[row["CUSIP"].ToString()] = positionMasterSymbolWiseList;
                                _allocationSchemeSymbologyWiseDict[4] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<DataRow> positionMasterlist = new List<DataRow>();
                                positionMasterlist.Add(dr);
                                _allocationSchemeSymbologyWiseDict[4].Add(row["CUSIP"].ToString(), positionMasterlist);
                            }
                        }
                        else
                        {
                            List<DataRow> positionMasterlist = new List<DataRow>();
                            positionMasterlist.Add(dr);
                            Dictionary<string, List<DataRow>> positionMasterSameSymbolDict = new Dictionary<string, List<DataRow>>();
                            positionMasterSameSymbolDict.Add(row["CUSIP"].ToString(), positionMasterlist);
                            _allocationSchemeSymbologyWiseDict.Add(4, positionMasterSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(row["Bloomberg"].ToString()))
                    {
                        if (_allocationSchemeSymbologyWiseDict.ContainsKey(5))
                        {
                            Dictionary<string, List<DataRow>> positionMasterSameSymbologyDict = _allocationSchemeSymbologyWiseDict[5];
                            if (positionMasterSameSymbologyDict.ContainsKey(row["Bloomberg"].ToString()))
                            {
                                List<DataRow> positionMasterSymbolWiseList = positionMasterSameSymbologyDict[row["Bloomberg"].ToString()];
                                positionMasterSymbolWiseList.Add(dr);
                                positionMasterSameSymbologyDict[row["Bloomberg"].ToString()] = positionMasterSymbolWiseList;
                                _allocationSchemeSymbologyWiseDict[5] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<DataRow> positionMasterlist = new List<DataRow>();
                                positionMasterlist.Add(dr);
                                _allocationSchemeSymbologyWiseDict[5].Add(row["Bloomberg"].ToString(), positionMasterlist);
                            }
                        }
                        else
                        {
                            List<DataRow> positionMasterlist = new List<DataRow>();
                            positionMasterlist.Add(dr);
                            Dictionary<string, List<DataRow>> positionMasterSameSymbolDict = new Dictionary<string, List<DataRow>>();
                            positionMasterSameSymbolDict.Add(row["Bloomberg"].ToString(), positionMasterlist);
                            _allocationSchemeSymbologyWiseDict.Add(5, positionMasterSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(row["OSIOptionSymbol"].ToString()))
                    {
                        if (_allocationSchemeSymbologyWiseDict.ContainsKey(6))
                        {
                            Dictionary<string, List<DataRow>> positionMasterSameSymbologyDict = _allocationSchemeSymbologyWiseDict[6];
                            if (positionMasterSameSymbologyDict.ContainsKey(row["OSIOptionSymbol"].ToString()))
                            {
                                List<DataRow> positionMasterSymbolWiseList = positionMasterSameSymbologyDict[row["OSIOptionSymbol"].ToString()];
                                positionMasterSymbolWiseList.Add(dr);
                                positionMasterSameSymbologyDict[row["OSIOptionSymbol"].ToString()] = positionMasterSymbolWiseList;
                                _allocationSchemeSymbologyWiseDict[6] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<DataRow> positionMasterlist = new List<DataRow>();
                                positionMasterlist.Add(dr);
                                _allocationSchemeSymbologyWiseDict[6].Add(row["OSIOptionSymbol"].ToString(), positionMasterlist);
                            }
                        }
                        else
                        {
                            List<DataRow> positionMasterlist = new List<DataRow>();
                            positionMasterlist.Add(dr);
                            Dictionary<string, List<DataRow>> positionMasterSameSymbolDict = new Dictionary<string, List<DataRow>>();
                            positionMasterSameSymbolDict.Add(row["OSIOptionSymbol"].ToString(), positionMasterlist);
                            _allocationSchemeSymbologyWiseDict.Add(6, positionMasterSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(row["IDCOOptionSymbol"].ToString()))
                    {
                        if (_allocationSchemeSymbologyWiseDict.ContainsKey(7))
                        {
                            Dictionary<string, List<DataRow>> positionMasterSameSymbologyDict = _allocationSchemeSymbologyWiseDict[7];
                            if (positionMasterSameSymbologyDict.ContainsKey(row["IDCOOptionSymbol"].ToString()))
                            {
                                List<DataRow> positionMasterSymbolWiseList = positionMasterSameSymbologyDict[row["IDCOOptionSymbol"].ToString()];
                                positionMasterSymbolWiseList.Add(dr);
                                positionMasterSameSymbologyDict[row["IDCOOptionSymbol"].ToString()] = positionMasterSymbolWiseList;
                                _allocationSchemeSymbologyWiseDict[7] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<DataRow> positionMasterlist = new List<DataRow>();
                                positionMasterlist.Add(dr);
                                _allocationSchemeSymbologyWiseDict[7].Add(row["IDCOOptionSymbol"].ToString(), positionMasterlist);
                            }
                        }
                        else
                        {
                            List<DataRow> positionMasterlist = new List<DataRow>();
                            positionMasterlist.Add(dr);
                            Dictionary<string, List<DataRow>> positionMasterSameSymbolDict = new Dictionary<string, List<DataRow>>();
                            positionMasterSameSymbolDict.Add(row["IDCOOptionSymbol"].ToString(), positionMasterlist);
                            _allocationSchemeSymbologyWiseDict.Add(7, positionMasterSameSymbolDict);
                        }
                    }

                    #endregion Security Master request

                    string tradeType = dr["TradeType"].ToString();
                    string currency = dr["Currency"].ToString();

                    string duplicateRecordKey = string.Empty;
                    // here we sum up of qty on the basis of key and assign in total qty field
                    switch (AllocSchemeKey)
                    {
                        case AllocationSchemeKey.Symbol:
                            // key -- Symbol
                            duplicateRecordKey = dr["FundID"].ToString() + dr[AllocationBasedOnSymbolType].ToString();
                            string totalQtyCalculationKey_Symbol = dr[AllocationBasedOnSymbolType].ToString();

                            if (!duplicateAccountSymbolSideDict.ContainsKey(duplicateRecordKey))
                            {
                                duplicateAccountSymbolSideDict.Add(duplicateRecordKey, dr);

                                if (!AllocSchemeKeywiseTotalQtyDictForAllocationScheme.ContainsKey(totalQtyCalculationKey_Symbol))
                                {
                                    AllocSchemeKeywiseTotalQtyDictForAllocationScheme.Add(totalQtyCalculationKey_Symbol, Convert.ToDouble(row["Quantity"].ToString()));
                                }
                                else
                                {
                                    double qty = AllocSchemeKeywiseTotalQtyDictForAllocationScheme[totalQtyCalculationKey_Symbol];

                                    AllocSchemeKeywiseTotalQtyDictForAllocationScheme[totalQtyCalculationKey_Symbol] = (qty + Convert.ToDouble(row["Quantity"].ToString()));
                                }
                            }
                            else
                            {
                                dr["IsDuplicate"] = true;
                                DataRow duplicateRow = duplicateAccountSymbolSideDict[duplicateRecordKey];
                                duplicateRow["IsDuplicate"] = true;
                                double qty = AllocSchemeKeywiseTotalQtyDictForAllocationScheme[totalQtyCalculationKey_Symbol];
                                AllocSchemeKeywiseTotalQtyDictForAllocationScheme[totalQtyCalculationKey_Symbol] = (qty - Convert.ToDouble(duplicateRow["Quantity"].ToString()));
                            }
                            break;
                        case AllocationSchemeKey.SymbolSide:
                            // key -- Symbol + Side
                            duplicateRecordKey = dr["FundID"].ToString() + dr[AllocationBasedOnSymbolType].ToString() + dr["OrderSideTagValue"].ToString();
                            string totalQtyCalculationKey_SymbolSide = dr[AllocationBasedOnSymbolType].ToString() + Prana.BusinessObjects.Seperators.SEPERATOR_5 + dr["OrderSideTagValue"].ToString();

                            if (!duplicateAccountSymbolSideDict.ContainsKey(duplicateRecordKey))
                            {
                                duplicateAccountSymbolSideDict.Add(duplicateRecordKey, dr);

                                if (!AllocSchemeKeywiseTotalQtyDictForAllocationScheme.ContainsKey(totalQtyCalculationKey_SymbolSide))
                                {
                                    AllocSchemeKeywiseTotalQtyDictForAllocationScheme.Add(totalQtyCalculationKey_SymbolSide, Convert.ToDouble(row["Quantity"].ToString()));
                                }
                                else
                                {
                                    double qty = AllocSchemeKeywiseTotalQtyDictForAllocationScheme[totalQtyCalculationKey_SymbolSide];

                                    AllocSchemeKeywiseTotalQtyDictForAllocationScheme[totalQtyCalculationKey_SymbolSide] = (qty + Convert.ToDouble(row["Quantity"].ToString()));
                                }
                            }
                            else
                            {
                                dr["IsDuplicate"] = true;
                                DataRow duplicateRow = duplicateAccountSymbolSideDict[duplicateRecordKey];
                                duplicateRow["IsDuplicate"] = true;
                                double qty = AllocSchemeKeywiseTotalQtyDictForAllocationScheme[totalQtyCalculationKey_SymbolSide];
                                AllocSchemeKeywiseTotalQtyDictForAllocationScheme[totalQtyCalculationKey_SymbolSide] = (qty - Convert.ToDouble(duplicateRow["Quantity"].ToString()));
                            }
                            break;
                        case AllocationSchemeKey.PBSymbolSide:
                            // key -- PB + Symbol + Side
                            if (currencyListForAlloScheme != null && currencyListForAlloScheme.Count > 0 && !currencyListForAlloScheme.Contains(currency) && tradeType.ToLower().Equals("swap"))
                            {
                                string totalQtyCalculationKey_PBSymbolSide = dr["PB"].ToString() + Prana.BusinessObjects.Seperators.SEPERATOR_5 + dr[AllocationBasedOnSymbolType].ToString() + Prana.BusinessObjects.Seperators.SEPERATOR_5 + dr["OrderSideTagValue"].ToString();

                                if (!AllocSchemeKeywiseTotalQtyDictForAllocationScheme.ContainsKey(totalQtyCalculationKey_PBSymbolSide))
                                {
                                    AllocSchemeKeywiseTotalQtyDictForAllocationScheme.Add(totalQtyCalculationKey_PBSymbolSide, Convert.ToDouble(row["Quantity"].ToString()));
                                }
                                else
                                {
                                    double qty = AllocSchemeKeywiseTotalQtyDictForAllocationScheme[totalQtyCalculationKey_PBSymbolSide];

                                    AllocSchemeKeywiseTotalQtyDictForAllocationScheme[totalQtyCalculationKey_PBSymbolSide] = (qty + Convert.ToDouble(row["Quantity"].ToString()));
                                }
                            }
                            else
                            {
                                //  key -- Symbol + Side
                                string totalQtyCalculationKey_SymSide = dr[AllocationBasedOnSymbolType].ToString() + Prana.BusinessObjects.Seperators.SEPERATOR_5 + dr["OrderSideTagValue"].ToString();

                                duplicateRecordKey = dr["FundID"].ToString() + dr[AllocationBasedOnSymbolType].ToString() + dr["OrderSideTagValue"].ToString();

                                if (!duplicateAccountSymbolSideDict.ContainsKey(duplicateRecordKey))
                                {
                                    duplicateAccountSymbolSideDict.Add(duplicateRecordKey, dr);

                                    if (!symbolSidewithTotalQtyDictForAllocationScheme.ContainsKey(totalQtyCalculationKey_SymSide))
                                    {
                                        symbolSidewithTotalQtyDictForAllocationScheme.Add(totalQtyCalculationKey_SymSide, Convert.ToDouble(row["Quantity"].ToString()));
                                    }
                                    else
                                    {
                                        double qty = symbolSidewithTotalQtyDictForAllocationScheme[totalQtyCalculationKey_SymSide];

                                        symbolSidewithTotalQtyDictForAllocationScheme[totalQtyCalculationKey_SymSide] = (qty + Convert.ToDouble(row["Quantity"].ToString()));
                                    }
                                }
                                else
                                {
                                    dr["IsDuplicate"] = true;
                                    DataRow duplicateRow = duplicateAccountSymbolSideDict[duplicateRecordKey];
                                    duplicateRow["IsDuplicate"] = true;
                                    double qty = symbolSidewithTotalQtyDictForAllocationScheme[totalQtyCalculationKey_SymSide];
                                    symbolSidewithTotalQtyDictForAllocationScheme[totalQtyCalculationKey_SymSide] = (qty - Convert.ToDouble(duplicateRow["Quantity"].ToString()));
                                }
                            }
                            break;
                        default:
                            break;
                    }


                }

                // Sensato has special case, two keys i.e. one is Symbol + Side for normal trades and one is PB +Symbol + Side for NON AUD swaps
                //TODO: we have to generalize it, 
                // calculate prorata percentage based on Symbol and Side
                if (symbolSidewithTotalQtyDictForAllocationScheme.Count > 0)
                    CalculateAllocationScheme(dsAllocationScheme, symbolSidewithTotalQtyDictForAllocationScheme, AllocationBasedOnSymbolType, AllocationSchemeKey.SymbolSide);

                // calculate prorata percentage based on Symbol or Symbol Side or PB Symbol and Side
                if (AllocSchemeKeywiseTotalQtyDictForAllocationScheme.Count > 0)
                    CalculateAllocationScheme(dsAllocationScheme, AllocSchemeKeywiseTotalQtyDictForAllocationScheme, AllocationBasedOnSymbolType, AllocSchemeKey);
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
            return dsAllocationScheme;
        }
        /// <summary>
        /// Allocation Scheme Prorata percentage calculation
        /// </summary>
        /// <param name="dsAScheme"></param>
        /// <param name="symbolwithTotalQtyDictForAllocationScheme"></param>
        /// <param name="AllocationBasedOnSymbolType"></param>
        /// <param name="allocationSchemeKey"></param>
        private static void CalculateAllocationScheme(DataSet dsAScheme, Dictionary<string, double> symbolwithTotalQtyDictForAllocationScheme, string AllocationBasedOnSymbolType, AllocationSchemeKey allocationSchemeKey)
        {
            try
            {
                switch (allocationSchemeKey)
                {
                    case AllocationSchemeKey.Symbol:
                        foreach (KeyValuePair<string, double> kvp in symbolwithTotalQtyDictForAllocationScheme)
                        {
                            string[] symbolSide = kvp.Key.Split(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                            if (symbolSide.Length > 0)
                            {
                                if (symbolSide[0].Contains("\'"))
                                    symbolSide[0] = symbolSide[0].Replace("\'", "\'\'");
                                DataRow[] rows = dsAScheme.Tables[0].Select(AllocationBasedOnSymbolType + " = '" + symbolSide[0] + "' And IsDuplicate = " + false);
                                //Added to remove row containing total quantity equals to 0, PRANA-11260
                                if (kvp.Value == 0)
                                {
                                    foreach (DataRow row in rows)
                                    {
                                        dsAScheme.Tables[0].Rows.Remove(row);
                                    }
                                    continue;
                                }
                                foreach (DataRow row in rows)
                                {
                                    row["Percentage"] = (Convert.ToDouble(row["Quantity"].ToString()) * 100) / kvp.Value;

                                    row["TotalQty"] = kvp.Value;
                                }
                            }
                        }
                        break;
                    case AllocationSchemeKey.SymbolSide:
                        foreach (KeyValuePair<string, double> kvp in symbolwithTotalQtyDictForAllocationScheme)
                        {
                            string[] symbolSide = kvp.Key.Split(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                            if (symbolSide.Length > 0)
                            {
                                DataRow[] rows = dsAScheme.Tables[0].Select(AllocationBasedOnSymbolType + " = '" + symbolSide[0] + "' And OrderSideTagValue = '" + symbolSide[1] + "' And IsDuplicate = " + false);
                                foreach (DataRow row in rows)
                                {
                                    row["Percentage"] = (Convert.ToDouble(row["Quantity"].ToString()) * 100) / kvp.Value;

                                    row["TotalQty"] = kvp.Value;
                                }
                            }
                        }
                        break;
                    case AllocationSchemeKey.PBSymbolSide:
                        foreach (KeyValuePair<string, double> kvp in symbolwithTotalQtyDictForAllocationScheme)
                        {
                            string[] pbsymbolSide = kvp.Key.Split(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                            if (pbsymbolSide.Length > 0)
                            {
                                DataRow[] rows = dsAScheme.Tables[0].Select("PB = '" + pbsymbolSide[0] + "' And " + AllocationBasedOnSymbolType + " = '" + pbsymbolSide[1] + "' And OrderSideTagValue = '" + pbsymbolSide[2] + "' And IsDuplicate = " + false);
                                foreach (DataRow row in rows)
                                {
                                    row["Percentage"] = (Convert.ToDouble(row["Quantity"].ToString()) * 100) / kvp.Value;

                                    row["TotalQty"] = kvp.Value;
                                }
                            }
                        }
                        break;
                    default:
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
        /// remove extra columns 
        /// </summary>
        /// <param name="dsAllocationScheme"></param>
        /// <returns></returns>
        public static DataSet GetUpdatedDataSet(DataSet dsAllocationScheme)
        {
            try
            {
                //// insert values into the DB  
                DataTable dtUpdatedAllocationScheme = new DataTable();
                // Purpose : In case of ImportIntoApplication tableName is ImportData instead of ""PositionMaster"
                //dtUpdatedAllocationScheme.TableName = "PositionMaster";
                dtUpdatedAllocationScheme.TableName = dsAllocationScheme.Tables[0].TableName;
                dtUpdatedAllocationScheme = dsAllocationScheme.Tables[0].Copy();

                List<string> deletedColumns = AllocationSchemeDeletedColumns();

                foreach (string colName in deletedColumns)
                {
                    if (dtUpdatedAllocationScheme.Columns.Contains(colName))
                    {
                        dtUpdatedAllocationScheme.Columns.Remove(colName);
                    }
                }

                dsAllocationScheme.Tables.Remove(dsAllocationScheme.Tables[0].TableName);
                dsAllocationScheme.Tables.Add(dtUpdatedAllocationScheme);
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

            return dsAllocationScheme;
        }
        /// <summary>
        /// some extra columns we need to validate from Security Master but need not to save in the data base, so we remove these while saving in db
        /// </summary>
        /// <returns></returns>
        private static List<string> AllocationSchemeDeletedColumns()
        {
            List<string> deletedColumns = new List<string>();
            try
            {
                deletedColumns.Add("SMMappingReq");
                deletedColumns.Add("Validated");
                deletedColumns.Add("IsSymbolValidatedFromSM");
                deletedColumns.Add("IsDuplicate");
                //deletedColumns.Add("RowIndex");
                deletedColumns.Add("ImportStatus");
                deletedColumns.Add("ValidationError");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return deletedColumns;
        }

        /// <summary>
        /// this method is called from Allocation Module
        /// its purpose is to Save Allocation scheme in the database
        /// it get open positions on the basis of date it passed and calculate Prorata % and save in the data base in XML format
        /// </summary>
        /// <returns></returns>
        public static int SaveAllocationSchemeFromApp(string allocationSchemeName, DateTime date, string schemeBasis, bool isMFRatioSchemEnabled)
        {
            int allocationSchemeID = 0;
            try
            {
                // the date is use here to get today's positions
                //DateTime date = DateTime.Today;
                // this form is used to provide date to get open positions

                // allocation scheme entered by user

                //As allocation Scheme Name is being provided by user now no need to fetch from app.config

                //get today's positions from database
                DataTable dtOpenPositions = new DataTable();
                dtOpenPositions = GetPositions(date, isMFRatioSchemEnabled, schemeBasis);

                //give table name because name is used to transform it into required XML
                dtOpenPositions.TableName = "PositionMaster";

                if (dtOpenPositions != null && dtOpenPositions.Rows.Count > 0)
                {
                    //generate XML on startup path, it helps in debugging
                    if (!Directory.Exists(Application.StartupPath + @"\xmls\Transformation\Temp"))
                        Directory.CreateDirectory(Application.StartupPath + @"\xmls\Transformation\Temp");
                    string serializedPMXml = Application.StartupPath + @"\xmls\Transformation\Temp\serializedXMLforPM.xml";
                    dtOpenPositions.WriteXml(serializedPMXml);
                    // get a new mapped xml
                    string mappedxml = Application.StartupPath + @"\xmls\Transformation\Temp\ConvertedXMLforPM.xml";
                    // get the XSLT name only
                    string strXSLTName = ConfigurationManager.AppSettings["GetPositionofADateXSLTName"];
                    // set the XSLT path as StartUp Path
                    string dirPath = ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.PMImportXSLT.ToString();
                    string strXSLTStartUpPath = Application.StartupPath + "\\" + dirPath + "\\" + strXSLTName;
                    //transform into the required XML
                    //                                                  serializedXML , MappedserXML, XSLTPath                             
                    string mappedfilePath = XMLUtilities.GetTransformed(serializedPMXml, mappedxml, strXSLTStartUpPath);

                    if (!mappedfilePath.Equals(""))
                    {
                        DataSet ds = new DataSet();
                        //convert required XML into dataset
                        ds.ReadXml(mappedfilePath);

                        // added table count check to avaoid exception "Table[0] doesn't exist" when there is no table in dataset
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7713
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            //convert the data into allocation scheme format i.e. XML as allocation scheme saves in db
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                row["AllocationSchemeKey"] = schemeBasis;
                            }
                            ds = UpdateAllocationSchemeValueCollection(ds, null, true);

                            // save allcation scheme data as we have open positions in our applications
                            AllocationFixedPreference fixedPref = new AllocationFixedPreference(int.MinValue, allocationSchemeName, ds.GetXml(), date, true, FixedPreferenceCreationSource.ProrataUI);
                            allocationSchemeID = _allocationServices.InnerChannel.SaveAllocationScheme(fixedPref);
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
            return allocationSchemeID;
        }

        /// <summary>
        /// Gets the positions based on isMFRatioSchemEnabled and schemeKey.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="isMFRatioSchemEnabled">if set to <c>true</c> [is mf ratio schem enabled].</param>
        /// <param name="schemeKey">The scheme key.</param>
        /// <returns></returns>
        private static DataTable GetPositions(DateTime date, bool isMFRatioSchemEnabled, string schemeKey)
        {
            DataTable dtPositions = null;
            try
            {
                String allocationSPName = string.Empty;
                if (isMFRatioSchemEnabled)
                {
                    switch (schemeKey)
                    {
                        case "Symbol":
                            allocationSPName = "GetTodayPositionsWithMFRatio";
                            break;
                        case "SymbolSide":
                            allocationSPName = "GetTodayPositionsWithMFRatioSymbolSide";
                            break;
                    }
                }
                else
                {
                    switch (schemeKey)
                    {
                        case "Symbol":
                            allocationSPName = "GetTodayPositions";
                            break;
                        case "SymbolSide":
                            allocationSPName = "GetTodayPositionsBySymbolSide";
                            break;
                    }
                }
                dtPositions = MarkPositionManager.GetPositions(date, allocationSPName);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtPositions;
        }



        /// <summary>
        /// Get open positions of a date
        /// Modified by- omshiv, 15, Jan 2014
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataTable GetPositions(DateTime date)
        {
            DataTable dtPositions = null;
            try
            {
                //DateTime date = DateTime.Today;


                //modified by omshiv, Jan 2014, add sp name to fetch postions in config as jira issue-- 
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3171

                String allocationSPName = ConfigurationHelper.Instance.GetAppSettingValueByKey("AllocationScheme_ProrataSPName");
                dtPositions = MarkPositionManager.GetPositions(date, allocationSPName);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtPositions;
        }


    }
}
