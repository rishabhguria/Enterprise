using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Prana.PositionServices
{
    class PranaPositionDataManager
    {
        #region Global Variables
        static ConcurrentDictionary<string, int> _dictCurrencies = new ConcurrentDictionary<string, int>();
        // static DataSet dtOpenPositions = new DataSet();        
        private const string _ConstBloombergExCode = "BloombergSymbolWithExchangeCode";
        #endregion
        //Creating Taxlots From Dataset
        public static List<TaxLot> GetOpenPositions(DateTime date, bool isTodaysTransactions, string commaSapratedAccountIds = "")
        {
            try
            {
                DataSet ds = GetOpenPositionsFromDB(date, isTodaysTransactions, null, commaSapratedAccountIds, false);
                return GetTaxlotsFromDataset(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            //As the GetTaxlotsFromDataset function is returning empty list of taxlots if the dataset is null.
            return new List<TaxLot>();
        }

        public static List<TaxLot> GetOpenPositions(DateTime date, string connectionString)
        {
            try
            {
                DataSet ds = GetOpenPositionsFromDB(date, connectionString);
                return GetTaxlotsFromDataset(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            //As the GetTaxlotsFromDataset function is returning empty list of taxlots if the dataset is null.
            return new List<TaxLot>();
        }

        public static List<TaxLot> GetOpenPositionsOrTransactions(string auecDatesString, bool isTodaysTransactions, string CommaSeparatedAccountIds, string CommaSeparatedAssetIDs, string commaSeparatedSymbols, string customConditions, int dateType)
        {
            try
            {
                DataSet ds = GetOpenPositionsFromDB(auecDatesString, isTodaysTransactions, CommaSeparatedAccountIds, CommaSeparatedAssetIDs, commaSeparatedSymbols, customConditions, dateType);
                return GetTaxlotsFromDataset(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            //As the GetTaxlotsFromDataset function is returning empty list of taxlots if the dataset is null.
            return new List<TaxLot>();
        }

        /// <summary>
        /// Get Open positions from database for a symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="auecDatesString"></param>
        /// <param name="CommaSeparatedAccountIds"></param>
        /// <returns></returns>
        public static List<TaxLot> GetOpenPositionsOrTransactionsForASymbol(string symbol, string auecDatesString, string CommaSeparatedAccountIds, string orderSideTagValue)
        {
            try
            {
                DataSet ds = GetOpenPositionsFromDBForASymbol(symbol, auecDatesString, CommaSeparatedAccountIds, orderSideTagValue);
                return GetTaxlotsFromDataset(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            //As the GetTaxlotsFromDataset function is returning empty list of taxlots if the dataset is null.
            return new List<TaxLot>();
        }

        public static List<TaxLot> GetOpenUnallocatedTradesForDateString(string auecDatesString)
        {
            try
            {
                DataSet ds = GetOpenUnallocatedTradesFromDB(auecDatesString);
                return GetTaxlotsFromDataset(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            //As the GetTaxlotsFromDataset function is returning empty list of taxlots if the dataset is null.
            return new List<TaxLot>();
        }

        public static List<TaxLot> GetPostDatedTransactions(string auecDatesString, int dateType)
        {
            try
            {
                DataSet ds = GetPostDatedTransactionsFromDB(auecDatesString, dateType);
                return GetTaxlotsFromDataset(ds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            //As the GetTaxlotsFromDataset function is returning empty list of taxlots if the dataset is null.
            return new List<TaxLot>();
        }

        public static List<TaxLot> GetTaxlotsFromDataset(DataSet ds)
        {
            List<TaxLot> taxlotList = new List<TaxLot>();
            try
            {
                if (ds != null)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        TaxLot taxLot = new TaxLot();
                        FillBasicDetails(row, taxLot);
                        FillTaxLotSpecificDetails(row, taxLot);
                        taxlotList.Add(taxLot);
                    }
                    return taxlotList;
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
            return taxlotList;
        }

        /// <summary>
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-7193
        /// </summary>
        private static void FillCurrencyDictionary()
        {
            try
            {
                foreach (KeyValuePair<int, string> kvp in CachedDataManager.GetInstance.GetAllCurrencies())
                {
                    _dictCurrencies.TryAdd(kvp.Value, kvp.Key);
                }
                _dictCurrencies.TryAdd("None", 0);
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

        public static List<TaxLot> GetTaxlotsFromDataTable(DataTable dt)
        {
            List<TaxLot> taxlotList = new List<TaxLot>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        TaxLot taxLot = new TaxLot();
                        FillBasicDetails(row, taxLot);
                        FillTaxLotSpecificDetails(row, taxLot);
                        taxlotList.Add(taxLot);
                    }
                    return taxlotList;
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
            return taxlotList;
        }

        private static void FillBasicDetails(DataRow row, PranaBasicMessage pranaPos)
        {
            try
            {
                FillCurrencyDictionary();
                pranaPos.SettlementDate = Convert.ToDateTime(row["SettlementDate"].ToString());
                pranaPos.AUECID = Convert.ToInt32(row["AUECID"].ToString());
                pranaPos.Symbol = row["Symbol"].ToString();

                pranaPos.OrderSideTagValue = row["SideID"].ToString().Trim();
                pranaPos.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(pranaPos.OrderSideTagValue);
                //OpenQuantity
                //TaxLotOpenQty
                if (row.Table.Columns.Contains("Quantity"))
                {
                    pranaPos.CumQty = Math.Abs(double.Parse(row["Quantity"].ToString()));
                    pranaPos.Quantity = Math.Abs(double.Parse(row["Quantity"].ToString()));
                }
                else
                {
                    pranaPos.CumQty = Math.Abs(double.Parse(row["OpenQuantity"].ToString()));
                    pranaPos.Quantity = Math.Abs(double.Parse(row["OpenQuantity"].ToString()));
                }
                //quantity is always side adjusted
                pranaPos.Quantity = pranaPos.Quantity * Calculations.GetSideMultilpier(pranaPos.OrderSideTagValue);
                pranaPos.ContractMultiplier = double.Parse(row["Multiplier"].ToString());
                pranaPos.AssetID = int.Parse(row["AssetID"].ToString());
                pranaPos.AssetName = CachedDataManager.GetInstance.GetAssetText(pranaPos.AssetID);
                if (row.Table.Columns.Contains("AveragePrice"))
                {
                    pranaPos.AvgPrice = double.Parse(row["AveragePrice"].ToString());
                }
                else
                {
                    pranaPos.AvgPrice = double.Parse(row["AvgPX"].ToString());
                }
                pranaPos.CompanyName = row["CompanyName"].ToString();
                pranaPos.Description = row["Description"].ToString();
                pranaPos.InternalComments = (row.Table.Columns.Contains("InternalComments") && (row["InternalComments"] != DBNull.Value)) ? row["InternalComments"].ToString() : string.Empty;
                if (row["UnderlyingSymbol"].Equals(System.DBNull.Value) || row["UnderlyingSymbol"].Equals(string.Empty))
                {
                    pranaPos.UnderlyingSymbol = pranaPos.Symbol;
                }
                else
                {
                    pranaPos.UnderlyingSymbol = row["UnderLyingSymbol"].ToString();
                }
                if (row.Table.Columns.Contains("IDCOSymbol") && row["IDCOSymbol"] != null)
                    pranaPos.IDCOSymbol = row["IDCOSymbol"].ToString();
                else if (row["IDCO"] != DBNull.Value)
                    pranaPos.IDCOSymbol = row["IDCO"].ToString();
                if (row.Table.Columns.Contains("ISINSymbol") && row["ISINSymbol"] != null)
                    pranaPos.ISINSymbol = row["ISINSymbol"].ToString();
                else if (row.Table.Columns.Contains("ISIN") && row["ISIN"] != null)
                    pranaPos.ISINSymbol = row["ISIN"].ToString();
                if (row.Table.Columns.Contains("BloombergSymbol") && row["BloombergSymbol"] != DBNull.Value)
                    pranaPos.BloombergSymbol = row["BloombergSymbol"].ToString();
                else if (row.Table.Columns.Contains("Bloomberg") && row["Bloomberg"] != DBNull.Value)
                    pranaPos.BloombergSymbol = row["Bloomberg"].ToString();
                if (row.Table.Columns.Contains("CUSIPSymbol") && row["CUSIPSymbol"] != DBNull.Value)
                    pranaPos.CusipSymbol = row["CUSIPSymbol"].ToString();
                else if (row.Table.Columns.Contains("CUSIP") && row["CUSIP"] != DBNull.Value)
                    pranaPos.CusipSymbol = row["CUSIP"].ToString();
                if (row.Table.Columns.Contains("SEDOLSymbol") && row["SEDOLSymbol"] != DBNull.Value)
                    pranaPos.SEDOLSymbol = row["SEDOLSymbol"].ToString();
                else if (row.Table.Columns.Contains("SEDOL") && row["SEDOL"] != DBNull.Value)
                    pranaPos.SEDOLSymbol = row["SEDOL"].ToString();
                if (row.Table.Columns.Contains("OSISymbol") && row["OSISymbol"] != DBNull.Value)
                    pranaPos.OSISymbol = row["OSISymbol"].ToString();
                else if (row.Table.Columns.Contains("OSI") && row["OSI"] != DBNull.Value)
                    pranaPos.OSISymbol = row["OSI"].ToString();
                if (row.Table.Columns.Contains("FactSetSymbol") && row["FactSetSymbol"] != DBNull.Value)
                    pranaPos.FactSetSymbol = row["FactSetSymbol"].ToString();
                else if (row.Table.Columns.Contains("FactSet") && row["FactSet"] != DBNull.Value)
                    pranaPos.FactSetSymbol = row["FactSet"].ToString();
                if (row.Table.Columns.Contains("ActivSymbol") && row["ActivSymbol"] != DBNull.Value)
                    pranaPos.ActivSymbol = row["ActivSymbol"].ToString();
                else if (row.Table.Columns.Contains("Activ") && row["Activ"] != DBNull.Value)
                    pranaPos.ActivSymbol = row["Activ"].ToString();
                pranaPos.ExpirationDate = Convert.ToDateTime(row["ExpirationDate"].ToString());
                pranaPos.CurrencyID = int.Parse(row["CurrencyID"].ToString());
                pranaPos.ExchangeID = int.Parse(row["ExchangeID"].ToString());
                pranaPos.ExchangeName = CachedDataManager.GetInstance.GetExchangeText(int.Parse(row["ExchangeID"].ToString()));
                pranaPos.UnderlyingID = int.Parse(row["UnderlyingID"].ToString());
                pranaPos.UnderlyingName = CachedDataManager.GetInstance.GetUnderLyingText(int.Parse(row["UnderlyingID"].ToString()));
                pranaPos.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(pranaPos.CurrencyID);
                pranaPos.CompanyUserName = CachedDataManager.GetInstance.GetUserText(int.Parse(row["UserID"].ToString()));
                pranaPos.CounterPartyID = int.Parse(row["CounterPartyID"].ToString());
                pranaPos.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(int.Parse(row["CounterPartyID"].ToString()));
                pranaPos.AUECLocalDate = DateTime.Parse(row["AUECLocalDate"].ToString());
                if (pranaPos.AssetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.FixedIncome || pranaPos.AssetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.ConvertibleBond)
                {
                    if (row.Table.Columns.Contains("Coupon"))
                    {
                        pranaPos.CouponRate = (row["Coupon"] != DBNull.Value) ? Convert.ToDouble(row["Coupon"]) : pranaPos.CouponRate;
                        pranaPos.FirstCouponDate = (row["FirstCouponDate"] != DBNull.Value) ? DateTime.Parse(row["FirstCouponDate"].ToString()) : pranaPos.FirstCouponDate;
                        pranaPos.IssueDate = (row["IssueDate"] != DBNull.Value) ? DateTime.Parse(row["IssueDate"].ToString()) : pranaPos.IssueDate;
                        pranaPos.AccrualBasis = (row["AccrualBasisID"] != DBNull.Value) ? (AccrualBasis)(int.Parse(row["AccrualBasisID"].ToString())) : pranaPos.AccrualBasis;
                        pranaPos.BondType = (row["BondTypeID"] != DBNull.Value) ? (SecurityType)(int.Parse(row["BondTypeID"].ToString())) : pranaPos.BondType;
                        pranaPos.Freq = (row["CouponFrequencyID"] != DBNull.Value) ? (CouponFrequency)(int.Parse(row["CouponFrequencyID"].ToString())) : pranaPos.Freq;
                        pranaPos.IsZero = (row["IsZero"] != DBNull.Value) ? Convert.ToBoolean(row["IsZero"]) : pranaPos.IsZero;
                        pranaPos.MaturityDate = (row["MaturityDate"] != DBNull.Value) ? DateTime.Parse(row["MaturityDate"].ToString()) : pranaPos.MaturityDate;
                    }
                }
                pranaPos.LeadCurrencyID = (row["LeadCurrencyID"] != DBNull.Value) ? int.Parse(row["LeadCurrencyID"].ToString()) : 0;
                pranaPos.VsCurrencyID = (row["VsCurrencyID"] != DBNull.Value) ? int.Parse(row["VsCurrencyID"].ToString()) : 0;
                if (row.Table.Columns.Contains("IsNDF"))
                {
                    pranaPos.IsNDF = (row["IsNDF"] != DBNull.Value) ? Convert.ToBoolean(row["IsNDF"]) : pranaPos.IsZero;
                }
                if (row.Table.Columns.Contains("FixingDate"))
                {
                    pranaPos.FixingDate = (row["FixingDate"] != DBNull.Value) ? DateTime.Parse(row["FixingDate"].ToString()) : pranaPos.FixingDate;
                }
                if (row.Table.Columns.Contains("UnderlyingDelta") && row["UnderlyingDelta"] != DBNull.Value)
                {
                    pranaPos.UnderlyingDelta = Convert.ToDouble(row["UnderlyingDelta"].ToString());
                }
                else
                {
                    pranaPos.UnderlyingDelta = 1.0;
                }

                if (row.Table.Columns.Contains("TradeAttribute1") && row["TradeAttribute1"] != DBNull.Value)
                    pranaPos.TradeAttribute1 = row["TradeAttribute1"].ToString();

                if (row.Table.Columns.Contains("TradeAttribute2") && row["TradeAttribute2"] != DBNull.Value)
                    pranaPos.TradeAttribute2 = row["TradeAttribute2"].ToString();

                if (row.Table.Columns.Contains("TradeAttribute3") && row["TradeAttribute3"] != DBNull.Value)
                    pranaPos.TradeAttribute3 = row["TradeAttribute3"].ToString();

                if (row.Table.Columns.Contains("TradeAttribute4") && row["TradeAttribute4"] != DBNull.Value)
                    pranaPos.TradeAttribute4 = row["TradeAttribute4"].ToString();

                if (row.Table.Columns.Contains("TradeAttribute5") && row["TradeAttribute5"] != DBNull.Value)
                    pranaPos.TradeAttribute5 = row["TradeAttribute5"].ToString();

                if (row.Table.Columns.Contains("TradeAttribute6") && row["TradeAttribute6"] != DBNull.Value)
                    pranaPos.TradeAttribute6 = row["TradeAttribute6"].ToString();

                if (row.Table.Columns.Contains("ProxySymbol") && row["ProxySymbol"] != DBNull.Value)
                    pranaPos.ProxySymbol = row["ProxySymbol"].ToString();

                if (row.Table.Columns.Contains("TransactionType") && row["TransactionType"] != DBNull.Value)
                    pranaPos.TransactionType = row["TransactionType"].ToString();
                if (row.Table.Columns.Contains("ReutersSymbol") && row["ReutersSymbol"] != DBNull.Value)
                    pranaPos.ReutersSymbol = row["ReutersSymbol"].ToString();

                if (row.Table.Columns.Contains("SettlCurrency") && row["SettlCurrency"] != DBNull.Value)
                {
                    //This is done so that if the value we get is currency if then we can get id or if it is symbol then we can get id from the currency dictionary.
                    //This should be fixed as there should be all columns in sp giving same parameter for the column Name
                    int settlementCurrency;
                    if (int.TryParse(row["SettlCurrency"].ToString(), out settlementCurrency))
                        pranaPos.SettlementCurrencyID = settlementCurrency;
                    else if (_dictCurrencies.ContainsKey(row["SettlCurrency"].ToString()))
                    {
                        pranaPos.SettlementCurrencyID = _dictCurrencies[row["SettlCurrency"].ToString()];
                    }
                }

                if (row.Table.Columns.Contains("TransactionSource") && row["TransactionSource"] != DBNull.Value)
                    pranaPos.TransactionSource = (TransactionSource)row["TransactionSource"];
                if (row.Table.Columns.Contains("VenueID") && row["VenueID"] != DBNull.Value)
                    pranaPos.VenueID = Convert.ToInt32(row["VenueID"].ToString());
                //pranaPos.Venue = CommonDataCache.CachedDataManager.GetInstance.GetVenueText(pranaPos.VenueID);
                if (row.Table.Columns.Contains("OrderTypeTagValue") && row["OrderTypeTagValue"] != DBNull.Value)
                    pranaPos.OrderTypeTagValue = row["OrderTypeTagValue"].ToString();
                //pranaPos.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeTextBasedOnID(pranaPos.OrderTypeTagValue);
                if (row.Table.Columns.Contains(_ConstBloombergExCode) && row[_ConstBloombergExCode] != DBNull.Value)
                {
                    pranaPos.BloombergSymbolWithExchangeCode = row[_ConstBloombergExCode].ToString();
                }
                if (row.Table.Columns.Contains("AdditionalTradeAttributes") && row["AdditionalTradeAttributes"] != DBNull.Value)
                    pranaPos.SetTradeAttribute(row["AdditionalTradeAttributes"].ToString());
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }
        private static void FillTaxLotSpecificDetails(DataRow row, TaxLot pranaPos)
        {
            try
            {
                FillCurrencyDictionary();
                pranaPos.SettlementDate = Convert.ToDateTime(row["SettlementDate"].ToString());
                pranaPos.TaxLotID = row["TaxLotID"].ToString();
                //OpenQuantity
                //TaxLotOpenQty
                if (row.Table.Columns.Contains("Quantity"))
                {
                    pranaPos.TaxLotQty = Math.Abs(double.Parse(row["Quantity"].ToString()));
                }
                else
                {
                    pranaPos.TaxLotQty = Math.Abs(double.Parse(row["OpenQuantity"].ToString()));
                }
                pranaPos.AUECModifiedDate = DateTime.Parse(row["AUECLocalDate"].ToString());
                if (row.Table.Columns.Contains("OriginalPurchaseDate"))
                {
                    pranaPos.OriginalPurchaseDate = DateTime.Parse(row["OriginalPurchaseDate"].ToString());
                    pranaPos.ProcessDate = DateTime.Parse(row["ProcessDate"].ToString());
                }
                if (row.Table.Columns.Contains("MasterFund") && row["MasterFund"] != DBNull.Value)
                {
                    pranaPos.MasterFund = Convert.ToString(row["MasterFund"].ToString());
                }
                pranaPos.Level1ID = row["FundID"] != DBNull.Value ? Convert.ToInt32(row["FundID"].ToString()) : 0;
                pranaPos.Level1Name = CachedDataManager.GetInstance.GetAccountText(pranaPos.Level1ID);
                pranaPos.Level2ID = Convert.ToInt32(Convert.ToString(row["Level2ID"]));
                pranaPos.Level2Name = CachedDataManager.GetInstance.GetStrategyText(pranaPos.Level2ID);
                pranaPos.ContractMultiplier = row["Multiplier"] != DBNull.Value ? Convert.ToDouble(row["Multiplier"]) : 1;
                pranaPos.AssetCategoryValue = (AssetCategory)(pranaPos.AssetID);
                pranaPos.GroupID = Convert.ToString(row["GroupID"]);

                if (row.Table.Columns.Contains("PositionTag"))
                {
                    pranaPos.PositionTag = (PositionTag)Convert.ToInt32(row["PositionTag"].ToString());
                }
                if (pranaPos.Quantity > 0)
                {
                    pranaPos.PositionType = PositionType.Long.ToString();
                }
                else
                {
                    pranaPos.PositionType = PositionType.Short.ToString();
                }

                pranaPos.StrikePrice = row["StrikePrice"] != DBNull.Value ? Convert.ToDouble(row["StrikePrice"].ToString()) : 0d;
                pranaPos.OrderSide = CommonDataCache.TagDatabaseManager.GetInstance.GetOrderSideText(pranaPos.OrderSideTagValue);
                pranaPos.SideMultiplier = Calculations.GetSideMultilpier(pranaPos.OrderSideTagValue);

                //TotalCommissionandFees
                //OpenTotalCommissionandFees
                pranaPos.OpenTotalCommissionandFees = Convert.ToDouble(Convert.ToString(row["TotalCommissionandFees"]));
                pranaPos.Delta = float.Parse(row["Delta"].ToString());

                pranaPos.FXRate = row["FXRate"] != DBNull.Value ? Convert.ToDouble(row["FXRate"]) : 0.0;
                pranaPos.FXConversionMethodOperator = row["FXConversionMethodOperator"] != DBNull.Value ? Convert.ToString(row["FXConversionMethodOperator"]) : "M";

                string putOrCall = row["PutOrCall"] != DBNull.Value ? row["PutOrCall"].ToString() : string.Empty;
                //pranaPos.PutOrCalls = (String.IsNullOrEmpty(putOrCall)) ? ' ' : char.Parse(putOrCall.Substring(0, 1));
                pranaPos.ExchangeID = int.Parse(row["ExchangeID"].ToString());
                pranaPos.ExchangeName = CachedDataManager.GetInstance.GetExchangeText(int.Parse(row["ExchangeID"].ToString()));
                if (putOrCall == OptionType.CALL.ToString().ToUpper())
                {
                    pranaPos.PutOrCall = (int)FIXConstants.Underlying_Call;
                }
                else if (putOrCall == OptionType.PUT.ToString().ToUpper())
                {
                    pranaPos.PutOrCall = (int)FIXConstants.Underlying_Put;
                }
                bool isSwap = row["IsSwapped"] != DBNull.Value ? Convert.ToBoolean(row["IsSwapped"].ToString()) : false;
                if (isSwap)
                {
                    Prana.BusinessObjects.SwapParameters sw = new Prana.BusinessObjects.SwapParameters();
                    pranaPos.SwapParameters = sw;
                    sw.NotionalValue = row["NotionalValue"] != DBNull.Value ? Convert.ToDouble(row["NotionalValue"].ToString()) : 0d;
                    sw.BenchMarkRate = row["BenchMarkRate"] != DBNull.Value ? Convert.ToDouble(row["BenchMarkRate"].ToString()) : 0d;
                    sw.Differential = row["Differential"] != DBNull.Value ? Convert.ToDouble(row["Differential"].ToString()) : 0d;
                    sw.OrigCostBasis = row["OrigCostBasis"] != DBNull.Value ? Convert.ToDouble(row["OrigCostBasis"].ToString()) : 0d;
                    sw.DayCount = row["DayCount"] != DBNull.Value ? Convert.ToInt32(row["DayCount"].ToString()) : 0;
                    sw.SwapDescription = row["SwapDescription"] != DBNull.Value ? row["SwapDescription"].ToString() : string.Empty;
                    sw.FirstResetDate = row["FirstResetDate"] != DBNull.Value ? Convert.ToDateTime(row["FirstResetDate"]) : DateTimeConstants.MinValue;
                    sw.OrigTransDate = row["OrigTransDate"] != DBNull.Value ? Convert.ToDateTime(row["OrigTransDate"]) : DateTimeConstants.MinValue;
                }
                pranaPos.ISSwap = isSwap;
                if (row.Table.Columns.Contains("IsNDF"))
                {
                    pranaPos.IsNDF = (row["IsNDF"] != DBNull.Value) ? Convert.ToBoolean(row["IsNDF"]) : pranaPos.IsZero;
                }
                if (row.Table.Columns.Contains("FixingDate"))
                {
                    pranaPos.FixingDate = (row["FixingDate"] != DBNull.Value) ? DateTime.Parse(row["FixingDate"].ToString()) : pranaPos.FixingDate;
                }

                #region Merged UDA data from data row - omshiv, Nov 2013, UDA Merged to Sec Master

                if (row.Table.Columns.Contains("AssetName") && row["AssetName"] != DBNull.Value)
                    pranaPos.UDAAsset = row["AssetName"].ToString();

                if (row.Table.Columns.Contains("SecurityTypeName") && row["SecurityTypeName"] != DBNull.Value)
                    pranaPos.SecurityTypeName = row["SecurityTypeName"].ToString();

                if (row.Table.Columns.Contains("SectorName") && row["SectorName"] != DBNull.Value)
                    pranaPos.SectorName = row["SectorName"].ToString();

                if (row.Table.Columns.Contains("SubSectorName") && row["SubSectorName"] != DBNull.Value)
                    pranaPos.SubSectorName = row["SubSectorName"].ToString();

                if (row.Table.Columns.Contains("CountryName") && row["CountryName"] != DBNull.Value)
                    pranaPos.CountryName = row["CountryName"].ToString();

                #endregion

                if (row.Table.Columns.Contains("LotId") && row["LotId"] != DBNull.Value)
                    pranaPos.LotId = row["LotId"].ToString();

                if (row.Table.Columns.Contains("ExternalTransId") && row["ExternalTransId"] != DBNull.Value)
                    pranaPos.ExternalTransId = row["ExternalTransId"].ToString();

                if (row.Table.Columns.Contains("TradeAttribute1") && row["TradeAttribute1"] != DBNull.Value)
                    pranaPos.TradeAttribute1 = row["TradeAttribute1"].ToString();

                if (row.Table.Columns.Contains("TradeAttribute2") && row["TradeAttribute2"] != DBNull.Value)
                    pranaPos.TradeAttribute2 = row["TradeAttribute2"].ToString();

                if (row.Table.Columns.Contains("TradeAttribute3") && row["TradeAttribute3"] != DBNull.Value)
                    pranaPos.TradeAttribute3 = row["TradeAttribute3"].ToString();

                if (row.Table.Columns.Contains("TradeAttribute4") && row["TradeAttribute4"] != DBNull.Value)
                    pranaPos.TradeAttribute4 = row["TradeAttribute4"].ToString();

                if (row.Table.Columns.Contains("TradeAttribute5") && row["TradeAttribute5"] != DBNull.Value)
                    pranaPos.TradeAttribute5 = row["TradeAttribute5"].ToString();

                if (row.Table.Columns.Contains("TradeAttribute6") && row["TradeAttribute6"] != DBNull.Value)
                    pranaPos.TradeAttribute6 = row["TradeAttribute6"].ToString();

                if (row.Table.Columns.Contains("ProxySymbol") && row["ProxySymbol"] != DBNull.Value)
                    pranaPos.ProxySymbol = row["ProxySymbol"].ToString();
                //fill commission details to the taxlot so that during generating exceptions from journal they remain as it is
                //Narendra Kumar Jangir 
                if (row.Table.Columns.Contains("StampDuty") && row["StampDuty"] != DBNull.Value)
                    pranaPos.StampDuty = Convert.ToDouble(row["StampDuty"].ToString());
                if (row.Table.Columns.Contains("ClearingFee") && row["ClearingFee"] != DBNull.Value)
                    pranaPos.ClearingFee = Convert.ToDouble(row["ClearingFee"].ToString());
                if (row.Table.Columns.Contains("MiscFees") && row["MiscFees"] != DBNull.Value)
                    pranaPos.MiscFees = Convert.ToDouble(row["MiscFees"].ToString());
                if (row.Table.Columns.Contains("TaxOnCommissions") && row["TaxOnCommissions"] != DBNull.Value)
                    pranaPos.TaxOnCommissions = Convert.ToDouble(row["TaxOnCommissions"].ToString());
                if (row.Table.Columns.Contains("TransactionLevy") && row["TransactionLevy"] != DBNull.Value)
                    pranaPos.TransactionLevy = Convert.ToDouble(row["TransactionLevy"].ToString());
                if (row.Table.Columns.Contains("OtherBrokerFees") && row["OtherBrokerFees"] != DBNull.Value)
                    pranaPos.OtherBrokerFees = Convert.ToDouble(row["OtherBrokerFees"].ToString());
                if (row.Table.Columns.Contains("ClearingBrokerFee") && row["ClearingBrokerFee"] != DBNull.Value)
                    pranaPos.ClearingBrokerFee = Convert.ToDouble(row["ClearingBrokerFee"].ToString());
                if (row.Table.Columns.Contains("Commission") && row["Commission"] != DBNull.Value)
                    pranaPos.Commission = Convert.ToDouble(row["Commission"].ToString());
                if (row.Table.Columns.Contains("SoftCommission") && row["SoftCommission"] != DBNull.Value)
                    pranaPos.SoftCommission = Convert.ToDouble(row["SoftCommission"].ToString());
                if (row.Table.Columns.Contains("SecFee") && row["SecFee"] != DBNull.Value)
                    pranaPos.SecFee = Convert.ToDouble(row["SecFee"].ToString());
                if (row.Table.Columns.Contains("OccFee") && row["OccFee"] != DBNull.Value)
                    pranaPos.OccFee = Convert.ToDouble(row["OccFee"].ToString());
                if (row.Table.Columns.Contains("OrfFee") && row["OrfFee"] != DBNull.Value)
                    pranaPos.OrfFee = Convert.ToDouble(row["OrfFee"].ToString());
                if (row.Table.Columns.Contains("OptionPremiumAdjustment") && row["OptionPremiumAdjustment"] != DBNull.Value)
                    pranaPos.OptionPremiumAdjustment = Convert.ToDouble(row["OptionPremiumAdjustment"].ToString());
                if (row.Table.Columns.Contains("SettlCurrency") && row["SettlCurrency"] != DBNull.Value)
                {
                    int settlementCurrency;
                    if (int.TryParse(row["SettlCurrency"].ToString(), out settlementCurrency))
                        pranaPos.SettlementCurrencyID = settlementCurrency;
                    else if (_dictCurrencies.ContainsKey(row["SettlCurrency"].ToString()))
                    {
                        pranaPos.SettlementCurrencyID = _dictCurrencies[row["SettlCurrency"].ToString()];
                    }
                }

                #region fields for post recon amendment UI
                if (row.Table.Columns.Contains("ExecutedQty"))
                {
                    pranaPos.ExecutedQty = double.Parse(row["ExecutedQty"].ToString());
                }
                if (row.Table.Columns.Contains("MarkPrice"))
                {
                    pranaPos.MarkPrice = double.Parse(row["MarkPrice"].ToString());
                }
                #endregion

                if (row.Table.Columns.Contains("TransactionType") && row["TransactionType"] != DBNull.Value)
                {
                    pranaPos.TransactionType = row["TransactionType"].ToString();
                }
                if (row.Table.Columns.Contains("ClosingTaxlotId") && row["ClosingTaxlotId"] != DBNull.Value)
                {
                    pranaPos.ClosingWithTaxlotID = row["ClosingTaxlotId"].ToString();
                }
                if (row.Table.Columns.Contains("IsCurrencyFuture") && row["IsCurrencyFuture"] != DBNull.Value)
                {
                    pranaPos.IsCurrencyFuture = Convert.ToBoolean(row["IsCurrencyFuture"]);
                }

                if (row.Table.Columns.Contains("ClosingTradeDate") && row["ClosingTradeDate"] != DBNull.Value)
                {
                    pranaPos.ClosingTradeDate = Convert.ToDateTime(row["ClosingTradeDate"].ToString());
                }
                if (row.Table.Columns.Contains("ClosingSettlementDate") && row["ClosingSettlementDate"] != DBNull.Value)
                {
                    pranaPos.ClosingSettlementDate = Convert.ToDateTime(row["ClosingSettlementDate"].ToString());
                }
                if (row.Table.Columns.Contains("ClosingStatus") && row["ClosingStatus"] != DBNull.Value)
                {
                    pranaPos.ClosingStatus = (ClosingStatus)Convert.ToInt32(row["ClosingStatus"]);
                }
                if (row.Table.Columns.Contains("ClosedQuantity") && row["ClosedQuantity"] != DBNull.Value)
                {
                    pranaPos.ClosedQuantity = Math.Abs(double.Parse(row["ClosedQuantity"].ToString())); ;
                }
                if (row.Table.Columns.Contains("AccruedInterest") && row["AccruedInterest"] != DBNull.Value)
                {
                    pranaPos.AccruedInterest = double.Parse(row["AccruedInterest"].ToString()); ;
                }

                #region Dynamic-UDA
                if (row.Table.Columns.Contains("Analyst") && row["Analyst"] != DBNull.Value)
                    pranaPos.Analyst = row["Analyst"].ToString();

                if (row.Table.Columns.Contains("CountryOfRisk") && row["CountryOfRisk"] != DBNull.Value)
                    pranaPos.CountryOfRisk = row["CountryOfRisk"].ToString();

                if (row.Table.Columns.Contains("CustomUDA1") && row["CustomUDA1"] != DBNull.Value)
                    pranaPos.CustomUDA1 = row["CustomUDA1"].ToString();

                if (row.Table.Columns.Contains("CustomUDA2") && row["CustomUDA2"] != DBNull.Value)
                    pranaPos.CustomUDA2 = row["CustomUDA2"].ToString();

                if (row.Table.Columns.Contains("CustomUDA3") && row["CustomUDA3"] != DBNull.Value)
                    pranaPos.CustomUDA3 = row["CustomUDA3"].ToString();

                if (row.Table.Columns.Contains("CustomUDA4") && row["CustomUDA4"] != DBNull.Value)
                    pranaPos.CustomUDA4 = row["CustomUDA4"].ToString();

                if (row.Table.Columns.Contains("CustomUDA5") && row["CustomUDA5"] != DBNull.Value)
                    pranaPos.CustomUDA5 = row["CustomUDA5"].ToString();

                if (row.Table.Columns.Contains("CustomUDA6") && row["CustomUDA6"] != DBNull.Value)
                    pranaPos.CustomUDA6 = row["CustomUDA6"].ToString();

                if (row.Table.Columns.Contains("CustomUDA7") && row["CustomUDA7"] != DBNull.Value)
                    pranaPos.CustomUDA7 = row["CustomUDA7"].ToString();

                if (row.Table.Columns.Contains("Issuer") && row["Issuer"] != DBNull.Value)
                    pranaPos.Issuer = row["Issuer"].ToString();

                if (row.Table.Columns.Contains("LiquidTag") && row["LiquidTag"] != DBNull.Value)
                    pranaPos.LiquidTag = row["LiquidTag"].ToString();

                if (row.Table.Columns.Contains("MarketCap") && row["MarketCap"] != DBNull.Value)
                    pranaPos.MarketCap = row["MarketCap"].ToString();

                if (row.Table.Columns.Contains("Region") && row["Region"] != DBNull.Value)
                    pranaPos.Region = row["Region"].ToString();

                if (row.Table.Columns.Contains("RiskCurrency") && row["RiskCurrency"] != DBNull.Value)
                    pranaPos.RiskCurrency = row["RiskCurrency"].ToString();

                if (row.Table.Columns.Contains("UCITSEligibleTag") && row["UCITSEligibleTag"] != DBNull.Value)
                    pranaPos.UcitsEligibleTag = row["UCITSEligibleTag"].ToString();

                if (row.Table.Columns.Contains("CustomUDA8") && row["CustomUDA8"] != DBNull.Value)
                    pranaPos.CustomUDA8 = row["CustomUDA8"].ToString();

                if (row.Table.Columns.Contains("CustomUDA9") && row["CustomUDA9"] != DBNull.Value)
                    pranaPos.CustomUDA9 = row["CustomUDA9"].ToString();

                if (row.Table.Columns.Contains("CustomUDA10") && row["CustomUDA10"] != DBNull.Value)
                    pranaPos.CustomUDA10 = row["CustomUDA10"].ToString();

                if (row.Table.Columns.Contains("CustomUDA11") && row["CustomUDA11"] != DBNull.Value)
                    pranaPos.CustomUDA11 = row["CustomUDA11"].ToString();

                if (row.Table.Columns.Contains("CustomUDA12") && row["CustomUDA12"] != DBNull.Value)
                    pranaPos.CustomUDA12 = row["CustomUDA12"].ToString();

                if (row.Table.Columns.Contains(_ConstBloombergExCode) && row[_ConstBloombergExCode] != DBNull.Value)
                { 
                    pranaPos.BloombergSymbolWithExchangeCode = row[_ConstBloombergExCode].ToString();
                }

                if (row.Table.Columns.Contains("AdditionalTradeAttributes") && row["AdditionalTradeAttributes"] != DBNull.Value)
                    pranaPos.SetTradeAttribute(row["AdditionalTradeAttributes"].ToString());
                #endregion
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private static AssetCategory AssetCategory(int p)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //Retrive open position of all the taxlots from PM_Taxlot table
        public static DataSet GetOpenPositionsFromDB(DateTime GivenDate, bool IsTodaysTransactions, string CommaSapratedAssetIDs, string CommaSapratedAccountIds, bool isSameDateInAllAUEC)
        {
            DataSet dsOpenPositions = new DataSet();
            try
            {
                string sqlCommand;
                if (IsTodaysTransactions)
                {
                    sqlCommand = "P_GetTransactions";
                }
                else
                {
                    sqlCommand = "PMGetFundOpenPositionsForDateBase_New";
                }

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = sqlCommand;
                queryData.CommandTimeout = 900;
                queryData.DictionaryDatabaseParameter.Add("@AssetIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AssetIds",
                    ParameterType = DbType.String,
                    ParameterValue = CommaSapratedAssetIDs
                });
                queryData.DictionaryDatabaseParameter.Add("@FundIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundIds",
                    ParameterType = DbType.String,
                    ParameterValue = CommaSapratedAccountIds
                });

                string ToAllAUECDatesString = string.Empty;

                if (isSameDateInAllAUEC)
                    ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(GivenDate);
                else
                    ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(GivenDate);

                queryData.DictionaryDatabaseParameter.Add("@ToAllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ToAllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = ToAllAUECDatesString
                });

                if (IsTodaysTransactions)
                {
                    queryData.DictionaryDatabaseParameter.Add("@FromAllAUECDatesString", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@FromAllAUECDatesString",
                        ParameterType = DbType.String,
                        ParameterValue = ToAllAUECDatesString
                    });
                    queryData.DictionaryDatabaseParameter.Add("@ReconDateType", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@ReconDateType",
                        ParameterType = DbType.Int32,
                        ParameterValue = (int)ReconDateType.TradeDate
                    });
                }
                else
                {
                    queryData.DictionaryDatabaseParameter.Add("@Symbols", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@Symbols",
                        ParameterType = DbType.String,
                        ParameterValue = string.Empty
                    });
                    queryData.DictionaryDatabaseParameter.Add("@CustomConditions", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@CustomConditions",
                        ParameterType = DbType.String,
                        ParameterValue = string.Empty
                    });
                }
                dsOpenPositions = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return dsOpenPositions;
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

        //narendra kumar jangir
        //method used in recon only 
        //remove istodaytransaction and pass sp name
        public static DataSet FetchDataForGivenSpName(ReconParameters reconParameters, string commaSeparatedAssetIDs, string commaSeparatedAccountIDs)
        {
            DataSet dsOpenPositions = new DataSet();
            try
            {
                //Commented To Implement SnapShotConcept
                //string sqlCommand = "PMGetFundOpenPositionsForDateBase_New";

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = reconParameters.SpName;

                //timeout issue in PNL Recon
                //http://jira.nirvanasolutions.com:8080/browse/LEUCADIA-14
                queryData.CommandTimeout = 900;
                string FromAllAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateInUseAUECStr(reconParameters.DTFromDate);
                string ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateInUseAUECStr(reconParameters.DTToDate);
                //transction sp is used in other places also, so that we have to pass auec datestring in the transaction sp else we pass fromdate and todate 

                if (reconParameters.ReconType.Equals(ReconType.PNL.ToString()))
                {
                    queryData.DictionaryDatabaseParameter.Add("@StartDate", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@StartDate",
                        ParameterType = DbType.DateTime,
                        ParameterValue = reconParameters.DTFromDate
                    });
                    queryData.DictionaryDatabaseParameter.Add("@EndDate", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@EndDate",
                        ParameterType = DbType.DateTime,
                        ParameterValue = reconParameters.DTToDate
                    });
                }
                else
                {
                    queryData.DictionaryDatabaseParameter.Add("@FromAllAUECDatesString", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@FromAllAUECDatesString",
                        ParameterType = DbType.String,
                        ParameterValue = FromAllAUECDatesString
                    });
                    queryData.DictionaryDatabaseParameter.Add("@ToAllAUECDatesString", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@ToAllAUECDatesString",
                        ParameterType = DbType.String,
                        ParameterValue = ToAllAUECDatesString
                    });
                }

                queryData.DictionaryDatabaseParameter.Add("@AssetIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AssetIds",
                    ParameterType = DbType.String,
                    ParameterValue = commaSeparatedAssetIDs
                });
                queryData.DictionaryDatabaseParameter.Add("@FundIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundIds",
                    ParameterType = DbType.String,
                    ParameterValue = commaSeparatedAccountIDs
                });

                //Modified by omshiv, get data based on pranProcessDate or AUEClocalDate
                if (reconParameters.ReconType.Equals(ReconType.Transaction.ToString()))
                {
                    queryData.DictionaryDatabaseParameter.Add("@ReconDateType", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@ReconDateType",
                        ParameterType = DbType.Int32,
                        ParameterValue = (int)reconParameters.ReconDateType
                    });
                }
                dsOpenPositions = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return dsOpenPositions;
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

        //Retrive open position of all the taxlots from PM_Taxlot table with connection string define in the report db
        private static DataSet GetOpenPositionsFromDB(DateTime GivenDate, string connectionString)
        {
            DataSet dsOpenPositions = new DataSet();
            try
            {
                SqlConnection myConn;
                myConn = new SqlConnection(connectionString);
                myConn.Open();
                SqlCommand cmd = new SqlCommand("PMGetFundOpenPositionsForDateBase_New", myConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 900;
                string ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(GivenDate);

                cmd.Parameters.AddWithValue("@AssetIds", string.Empty);
                cmd.Parameters.AddWithValue("@FundIds", string.Empty);
                cmd.Parameters.AddWithValue("@Symbols", string.Empty);
                cmd.Parameters.AddWithValue("@CustomConditions", string.Empty);
                cmd.Parameters.AddWithValue("@ToAllAUECDatesString", ToAllAUECDatesString);
                // create the data adapter 
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                // fill the DataSet using our DataAdapter 
                dataAdapter.Fill(dsOpenPositions);
                return dsOpenPositions;
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

        private static DataSet GetOpenPositionsFromDB(string auecDatesString, bool isTodaysTransactions, string CommaSeparatedAccountIds, string CommaSeparatedAssetIDs, string commaSeparatedSymbols, string customConditions, int dateType)
        {
            DataSet dsOpenPositions = new DataSet();
            try
            {
                string sqlCommand;
                if (isTodaysTransactions)
                {
                    sqlCommand = "P_GetTransactions";
                }
                else
                {
                    sqlCommand = "PMGetFundOpenPositionsForDateBase_new";
                }

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = sqlCommand;
                queryData.CommandTimeout = 900;

                queryData.DictionaryDatabaseParameter.Add("@AssetIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AssetIds",
                    ParameterType = DbType.String,
                    ParameterValue = CommaSeparatedAssetIDs
                });
                queryData.DictionaryDatabaseParameter.Add("@FundIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundIds",
                    ParameterType = DbType.String,
                    ParameterValue = CommaSeparatedAccountIds
                });
                queryData.DictionaryDatabaseParameter.Add("@ToAllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ToAllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = auecDatesString
                });

                if (isTodaysTransactions)
                {
                    queryData.DictionaryDatabaseParameter.Add("@FromAllAUECDatesString", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@FromAllAUECDatesString",
                        ParameterType = DbType.String,
                        ParameterValue = auecDatesString
                    });
                    queryData.DictionaryDatabaseParameter.Add("@ReconDateType", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@ReconDateType",
                        ParameterType = DbType.Int32,
                        ParameterValue = dateType
                    });
                }
                else
                {
                    queryData.DictionaryDatabaseParameter.Add("@ReconDateType", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@ReconDateType",
                        ParameterType = DbType.Int32,
                        ParameterValue = dateType
                    });
                    queryData.DictionaryDatabaseParameter.Add("@Symbols", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@Symbols",
                        ParameterType = DbType.String,
                        ParameterValue = commaSeparatedSymbols
                    });
                    queryData.DictionaryDatabaseParameter.Add("@CustomConditions", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@CustomConditions",
                        ParameterType = DbType.String,
                        ParameterValue = customConditions
                    });
                }
                // return taxlotList;
                dsOpenPositions = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return dsOpenPositions;
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

        /// <summary>
        /// Get Open positions for a symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="auecDatesString"></param>
        /// <param name="CommaSeparatedAccountIds"></param>
        /// <returns></returns>
        private static DataSet GetOpenPositionsFromDBForASymbol(string symbol, string auecDatesString, string CommaSeparatedAccountIds, string orderSideTagValue)
        {
            DataSet dsOpenPositions = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetFundOpenPositionsForASymbol";
                queryData.CommandTimeout = 200;
                queryData.DictionaryDatabaseParameter.Add("@ToAllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ToAllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = auecDatesString
                });
                queryData.DictionaryDatabaseParameter.Add("@FundIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundIds",
                    ParameterType = DbType.String,
                    ParameterValue = CommaSeparatedAccountIds
                });
                queryData.DictionaryDatabaseParameter.Add("@Symbol", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Symbol",
                    ParameterType = DbType.String,
                    ParameterValue = symbol
                });
                queryData.DictionaryDatabaseParameter.Add("@SideSideTagValue", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@SideSideTagValue",
                    ParameterType = DbType.String,
                    ParameterValue = orderSideTagValue
                });

                dsOpenPositions = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return dsOpenPositions;
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

        private static DataSet GetPostDatedTransactionsFromDB(string auecDatesString, int dateType)
        {
            DataSet dsOpenPositions = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetPostDatedTransactions";
                queryData.CommandTimeout = 200;
                queryData.DictionaryDatabaseParameter.Add("@ToAllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ToAllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = auecDatesString
                });
                queryData.DictionaryDatabaseParameter.Add("@ReconDateType", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ReconDateType",
                    ParameterType = DbType.Int32,
                    ParameterValue = dateType
                });

                dsOpenPositions = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return dsOpenPositions;
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

        private static DataSet GetOpenUnallocatedTradesFromDB(string auecDatesString)
        {
            DataSet dsOpenUnallocatedTrades = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetOpenUnallocatedTrades";
                queryData.CommandTimeout = 200;
                queryData.DictionaryDatabaseParameter.Add("@ToAllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ToAllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = auecDatesString
                });

                dsOpenUnallocatedTrades = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return dsOpenUnallocatedTrades;
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

        public static int SaveSnapShotData()
        {
            int NoOfRowsEffected = 0;
            try
            {
                //Coomented To Implement SnapShotConcept
                //string sqlCommand = "PMGetFundOpenPositionsForDateBase_New";

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SavePositions";
                queryData.CommandTimeout = 200;
                NoOfRowsEffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
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
            return NoOfRowsEffected;
        }

        public static DateTime GetSnapShotDate()
        {
            DateTime snapShotDate = new DateTime();
            try
            {
                QueryData queryData = new QueryData();
                queryData.Query = "SELECT SpecifiedDate From T_PositionDate";
                snapShotDate = (DateTime)DatabaseManager.DatabaseManager.ExecuteScalar(queryData);
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
            return snapShotDate;
        }

        public static int SaveSnapShotDate(DateTime givenDate)
        {
            int NoOfRowsEffected = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.Query = "UPDATE T_PositionDate SET SpecifiedDate='" + givenDate + "'";
                NoOfRowsEffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
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
            return NoOfRowsEffected;
        }
        internal static List<TaxLot> GetTransactions(DateTime FromDate, DateTime ToDate)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Modified by: bharat raturi, 1 aug 2014
        /// send the accountIDs to pick the data after the cash mgmt start date
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="accountIDs"></param>
        /// <returns></returns>
        internal static DataSet GetCashJournalExceptionalTransactions(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            DataSet dsJournalExceptions = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCashJournalExceptionalTransactions";
                queryData.CommandTimeout = 200;
                queryData.DictionaryDatabaseParameter.Add("@startDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@startDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = fromDate
                });
                queryData.DictionaryDatabaseParameter.Add("@endDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@endDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = toDate
                });
                queryData.DictionaryDatabaseParameter.Add("@fundIDs", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@fundIDs",
                    ParameterType = DbType.String,
                    ParameterValue = accountIDs
                });

                dsJournalExceptions = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return dsJournalExceptions;
        }

        internal static Dictionary<string, List<TaxLot>> GetOpenPositionsAndTransactions(DateTime StartDate, DateTime EndDate, string CommaSepratedAccountIds, string CommaSepratedAssetIDs, string CommaSeparatedCustomConditions)
        {
            DataSet dsAllPositions = new DataSet();
            Dictionary<string, List<TaxLot>> DicDateWisePositions = new Dictionary<string, List<TaxLot>>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetOpenPositionsAndTransactions";
                queryData.CommandTimeout = 900;
                queryData.DictionaryDatabaseParameter.Add("@AssetIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AssetIds",
                    ParameterType = DbType.String,
                    ParameterValue = CommaSepratedAssetIDs
                });
                queryData.DictionaryDatabaseParameter.Add("@FundIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundIds",
                    ParameterType = DbType.String,
                    ParameterValue = CommaSepratedAccountIds
                });
                queryData.DictionaryDatabaseParameter.Add("@StartDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@StartDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = StartDate
                });
                queryData.DictionaryDatabaseParameter.Add("@EndDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@EndDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = EndDate
                });
                queryData.DictionaryDatabaseParameter.Add("@ReconDateType", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ReconDateType",
                    ParameterType = DbType.Int32,
                    ParameterValue = (int)ReconDateType.TradeDate
                });
                queryData.DictionaryDatabaseParameter.Add("@Symbols", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Symbols",
                    ParameterType = DbType.String,
                    ParameterValue = string.Empty
                });
                queryData.DictionaryDatabaseParameter.Add("@CustomConditions", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CustomConditions",
                    ParameterType = DbType.String,
                    ParameterValue = CommaSeparatedCustomConditions
                });

                dsAllPositions = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                foreach (DataTable dt in dsAllPositions.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DateTime runDate = Convert.ToDateTime(row["RunDate"].ToString());
                        TaxLot taxLot = new TaxLot();
                        FillBasicDetails(row, taxLot);
                        FillTaxLotSpecificDetails(row, taxLot);

                        if (taxLot.PositionTag != PositionTag.LongWithdrawal && taxLot.PositionTag != PositionTag.ShortAddition
                        && taxLot.PositionTag != PositionTag.ShortWithdrawal && taxLot.PositionTag != PositionTag.LongAddition)
                        {
                            if (!DicDateWisePositions.ContainsKey(runDate.ToShortDateString()))
                            {
                                List<TaxLot> taxlotList = new List<TaxLot>();
                                taxlotList.Add(taxLot);
                                DicDateWisePositions.Add(runDate.ToShortDateString(), taxlotList);
                            }
                            else
                            {
                                DicDateWisePositions[runDate.ToShortDateString()].Add(taxLot);
                            }
                        }
                    }
                }
                return DicDateWisePositions;
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

        /// <summary>
        /// This method returns list of taxlot for which we have to update cash activity and journals for the day end fx rate
        /// </summary>
        /// <param name="ToDate"></param>
        /// <param name="CommaSepratedAccountIds"></param>
        /// <returns></returns>
        internal static List<TaxLot> GetTransactionsToUpdateSettlementFxRate(string ToDate, string CommaSepratedAccountIds)
        {
            DataSet dsAllPositions = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetTransactionsToUpdatedSettlementFields";
                queryData.CommandTimeout = 900;
                queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ToDate",
                    ParameterType = DbType.Date,
                    ParameterValue = ToDate
                });
                queryData.DictionaryDatabaseParameter.Add("@FundIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundIds",
                    ParameterType = DbType.String,
                    ParameterValue = CommaSepratedAccountIds
                });

                dsAllPositions = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                List<TaxLot> taxlotList = new List<TaxLot>();
                foreach (DataTable dt in dsAllPositions.Tables)
                    foreach (DataRow row in dt.Rows)
                    {
                        TaxLot taxLot = new TaxLot();
                        FillBasicDetails(row, taxLot);
                        FillTaxLotSpecificDetails(row, taxLot);
                        taxLot.TaxLotState = ApplicationConstants.TaxLotState.Updated;
                        taxlotList.Add(taxLot);
                    }
                return taxlotList;
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

        internal static int HideOrderFromBlotter(string parentClOrderIds)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_HideOrderFromBlotter";
                queryData.DictionaryDatabaseParameter.Add("@parentClOrderID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@parentClOrderID",
                    ParameterType = DbType.String,
                    ParameterValue = parentClOrderIds
                });
                queryData.DictionaryDatabaseParameter.Add("@rowAffected", new DatabaseParameter()
                {
                    IsOutParameter = true,
                    ParameterName = "@rowAffected",
                    ParameterType = DbType.Int32,
                    ParameterValue = 0,
                    OutParameterSize = sizeof(Int32)
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                //Exception Error Number and Error Message Handling
                int dbErrorNumber = 0;
                string dbErrorMessage = string.Empty;
                dbErrorNumber = Convert.ToInt32(queryData.DictionaryDatabaseParameter["@ErrorNumber"].ParameterValue);
                dbErrorMessage = Convert.ToString(queryData.DictionaryDatabaseParameter["@ErrorMessage"].ParameterValue);

                if (!int.Equals(dbErrorNumber, 0))
                    throw new Exception(dbErrorMessage);
                else
                    return Convert.ToInt32(queryData.DictionaryDatabaseParameter["@rowAffected"].ParameterValue);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return -1;
        }

        /// <summary>
        /// Hides the sub orders from blotter.
        /// </summary>
        /// <param name="subOrderClOrderIDs">The sub order cl order i ds.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        internal static int HideSubOrdersFromBlotter(string subOrderClOrderIDs)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_HideSubOrdersFromBlotter";
                queryData.DictionaryDatabaseParameter.Add("@SubOrderClOrderIDs", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@SubOrderClOrderIDs",
                    ParameterType = DbType.String,
                    ParameterValue = subOrderClOrderIDs
                });
                queryData.DictionaryDatabaseParameter.Add("@rowAffected", new DatabaseParameter()
                {
                    IsOutParameter = true,
                    ParameterName = "@rowAffected",
                    ParameterType = DbType.Int32,
                    ParameterValue = 0,
                    OutParameterSize = sizeof(Int32)
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                //Exception Error Number and Error Message Handling
                int dbErrorNumber = 0;
                string dbErrorMessage = string.Empty;
                dbErrorNumber = Convert.ToInt32(queryData.DictionaryDatabaseParameter["@ErrorNumber"].ParameterValue);
                dbErrorMessage = Convert.ToString(queryData.DictionaryDatabaseParameter["@ErrorMessage"].ParameterValue);

                if (!int.Equals(dbErrorNumber, 0))
                    throw new Exception(dbErrorMessage);
                else
                    return Convert.ToInt32(queryData.DictionaryDatabaseParameter["@rowAffected"].ParameterValue);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return -1;
        }
    }
}
