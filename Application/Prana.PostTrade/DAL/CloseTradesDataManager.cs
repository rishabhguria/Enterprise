using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.XMLUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace Prana.PostTrade
{
    class CloseTradesDataManager
    {
        private static int _errorNumber = 0;
        private static string _errorMessage = string.Empty;
        private static int _heavySaveTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["HeavySaveTimeout"]);
        static IPranaPositionServices _positionServices = null;

        public static IPranaPositionServices positionServices
        {
            set { _positionServices = value; }
        }

        static CloseTradesDataManager()
        {
            CreatePublishingProxy();
        }

        #region GetAllocatedOrders

        //Module: Close Trade/PM
        /// <summary>
        /// Gets the Unclosed Data for the Close Trade form 
        /// </summary>
        /// <param name="closeTradeInterface">The close trade interface.</param>
        public static List<TaxLot> GetOpenPositions(string ToAllAUECDatesString, string CommaSeparatedAccountIds, string CommaSeparatedAssetIds, string commaSeparatedSymbols, string customConditions)
        {
            try
            {
                List<TaxLot> opentaxlots = new List<TaxLot>();
                opentaxlots = _positionServices.GetOpenPositionsOrTransactions(ToAllAUECDatesString, false, CommaSeparatedAccountIds, CommaSeparatedAssetIds, commaSeparatedSymbols, customConditions);
                return opentaxlots;
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
                return null;
            }
        }

        /// <summary>
        /// Gets the Unclosed Data for the Close Trade form for Symbol to close Symbol wise data 
        /// Module: Close Trade
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="ToAllAUECDatesString"></param>
        /// <param name="CommaSeparatedAccountIds"></param>
        /// <returns></returns>
        public static List<TaxLot> GetOpenPositionsForASymbol(string symbol, string ToAllAUECDatesString, string CommaSeparatedAccountIds, string orderSideTagValue)
        {
            try
            {
                List<TaxLot> opentaxlots = new List<TaxLot>();
                opentaxlots = _positionServices.GetOpenPositionsOrTransactionsForASymbol(symbol, ToAllAUECDatesString, CommaSeparatedAccountIds, orderSideTagValue);

                return opentaxlots;
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
                return null;
            }
        }

        public static Dictionary<string, double> GetSymbolAccountOpenQty(string ToAllAUECDatesString, string symbols, string accountIDs)
        {
            Dictionary<string, double> dictSymbolPos = new Dictionary<string, double>();
            try
            {
                DataTable dt = GetSymbolAccountOpenQtyBase(ToAllAUECDatesString, symbols, accountIDs);
                if (dt != null)
                {
                    int sideMul = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        sideMul = 1;
                        string symbolValue = Convert.ToString(row["Symbol"]);
                        double openQtyvalue = Convert.ToDouble(row["OpenQuantity"]);
                        int positiontagvalue = row["Positiontag"] != DBNull.Value ? Convert.ToInt32(row["Positiontag"].ToString()) : 0;
                        if (positiontagvalue.Equals((int)PositionTag.Short))
                        {
                            sideMul = -1;
                        }
                        if (!dictSymbolPos.ContainsKey(symbolValue))
                        {
                            dictSymbolPos.Add(symbolValue, (openQtyvalue * sideMul));
                        }
                        else
                        {
                            dictSymbolPos[symbolValue] += (openQtyvalue * sideMul);
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
            return dictSymbolPos;
        }

        public static Dictionary<PositionTag, double> GetSymbolAccountOpenQtyByPositionTag(string ToAllAUECDatesString, string symbols, string accountIDs)
        {
            Dictionary<PositionTag, double> dictPositionTagPos = new Dictionary<PositionTag, double>();
            DataTable dt = GetSymbolAccountOpenQtyBase(ToAllAUECDatesString, symbols, accountIDs);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string symbolValue = Convert.ToString(row["Symbol"]);
                    double openQtyvalue = Convert.ToDouble(row["OpenQuantity"]);
                    int positiontagvalue = row["Positiontag"] != DBNull.Value ? Convert.ToInt32(row["Positiontag"].ToString()) : 0;
                    if (positiontagvalue.Equals((int)PositionTag.Short))
                    {
                        if (dictPositionTagPos.ContainsKey(PositionTag.Short))
                        {
                            dictPositionTagPos[PositionTag.Short] += openQtyvalue;
                        }
                        else
                        {
                            dictPositionTagPos.Add(PositionTag.Short, openQtyvalue);
                        }
                    }
                    else
                    {
                        if (dictPositionTagPos.ContainsKey(PositionTag.Long))
                        {
                            dictPositionTagPos[PositionTag.Long] += openQtyvalue;
                        }
                        else
                        {
                            dictPositionTagPos.Add(PositionTag.Long, openQtyvalue);
                        }
                    }
                }
            }
            return dictPositionTagPos;
        }

        private static DataTable GetSymbolAccountOpenQtyBase(string ToAllAUECDatesString, string symbols, string accountIDs)
        {
            DataTable dt = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetFundOpenPositionQtyFromDatabase_Symbol";
            queryData.CommandTimeout = 200;
            DateTime FromDate = DateTimeConstants.MinValue;
            queryData.DictionaryDatabaseParameter.Add("@ToAllAUECDatesString", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ToAllAUECDatesString",
                ParameterType = DbType.String,
                ParameterValue = ToAllAUECDatesString
            });
            queryData.DictionaryDatabaseParameter.Add("@FundIds", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@FundIds",
                ParameterType = DbType.String,
                ParameterValue = accountIDs
            });
            queryData.DictionaryDatabaseParameter.Add("@Symbols", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@Symbols",
                ParameterType = DbType.String,
                ParameterValue = symbols
            });

            DataSet productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            if (productsDataSet != null && productsDataSet.Tables != null && productsDataSet.Tables.Count > 0)
            {
                dt = productsDataSet.Tables[0];
            }
            return dt;
        }

        internal static List<TaxLot> FillOpenPositions(DataSet ds)
        {
            List<TaxLot> openTaxlots = new List<TaxLot>();

            int TaxlotID = 0;
            int tradeDate = 1;
            int ProcessDate = 2;
            int OriginalPurchaseDate = 3;
            int sideID = 4;
            int symbol = 5;
            int OpenQty = 6;
            int costBasis = 7; //Same as  average price
            int AccountID = 8;
            int AssetID = 9;
            int UnderLyingID = 10;
            int ExchangeID = 11;
            int CurrencyID = 12;
            int AUECID = 13;
            int TotalCommissionandFees = 14;
            int Multplier = 15;
            int SettlementDate = 16;
            int ExpirationDate = 19;
            int Description = 20;
            int StrategyID = 21; // same as stategyid or Analyst ID
            int NotionalValue = 22;
            int BenchMarkRate = 23;
            int Differential = 24;
            int OrigCostBasis = 25;
            int DayCount = 26;
            int SwapDescription = 27;
            int FirstResetDate = 28;
            int OrigTransDate = 29;
            int IsSwapped = 30;
            int GroupID = 32;
            int Positiontag = 33;
            int FxRate = 34;
            int FxConversionMethodOperator = 35;
            int CompanyName = 36;
            int UnderlyingSymbol = 37;
            int PutOrCall = 39;
            int Taxlot_pk = 44;
            int ParentRow_Pk = 45;
            int StrikePrice = 46;
            int isNDF = 47;
            int FixingDate = 48;
            int LeadCurrencyID = 49;
            int VsCurrencyID = 50;
            int LotId = 51;
            int ExternalTransId = 52;
            int TradeAttribute1 = 53;
            int TradeAttribute2 = 54;
            int TradeAttribute3 = 55;
            int TradeAttribute4 = 56;
            int TradeAttribute5 = 57;
            int TradeAttribute6 = 58;
            int ExecutedQty = 59;
            int TransactionType = 60;
            int InternalComments = 61;
            int TradingAccountId = 62;
            int UserID = 63;
            int NirvanaProcessDate = 64;
            int SettlCurrency = 65;
            int Bloomberg = 66;
            int SEDOL = 67;
            int CUSIP = 68;
            int ISIN = 69;
            int SecurityTypeName = 70;
            int CountryName = 71;
            int Proxy = 72;
            int IDCO = 73;
            int OSI = 74;
            int SectorName = 75;
            int SubSectorName = 76;
            int ReutersSymbol = 77;
            int AssetName = 78;
            int AdditionalTradeAttributes = 79;
            try
            {
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        string taxlotIDValue = Convert.ToString(row[TaxlotID]);
                        DateTime tradeDateValue = Convert.ToDateTime(Convert.ToString(row[tradeDate]));
                        DateTime ProcessDateValue = Convert.ToDateTime(Convert.ToString(row[ProcessDate]));
                        DateTime OriginalPurchaseDateValue = Convert.ToDateTime(Convert.ToString(row[OriginalPurchaseDate]));
                        string sideIDValue = Convert.ToString(row[sideID]).Trim();
                        string symbolValue = Convert.ToString(row[symbol]);
                        double openQtyvalue = Convert.ToDouble(row[OpenQty]);
                        double averagePriceValue = Convert.ToDouble(Convert.ToString(row[costBasis]));
                        int accountIDValue = row[AccountID] != DBNull.Value ? Convert.ToInt32(row[AccountID].ToString()) : 0;
                        int assetIDValue = Convert.ToInt32(row[AssetID].ToString());
                        int underLyingIDValue = Convert.ToInt32(row[UnderLyingID].ToString());
                        int exchangeIDvalue = Convert.ToInt32(row[ExchangeID].ToString());
                        int currencyIDValue = Convert.ToInt32(row[CurrencyID].ToString());
                        int auecIDValue = Convert.ToInt32(Convert.ToString(row[AUECID]));
                        double TotalCommissionandFeesValue = row[TotalCommissionandFees] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[TotalCommissionandFees])) : 0d;
                        string description = Convert.ToString(row[Description]);
                        DateTime fixingDate = Convert.ToDateTime(Convert.ToString(row[FixingDate]));
                        bool iSNDF = Convert.ToBoolean(row[isNDF]);
                        string lotId = Convert.ToString(row[LotId]);
                        string externalTransId = Convert.ToString(row[ExternalTransId]);
                        string tradeAttribute1 = Convert.ToString(row[TradeAttribute1]);
                        string tradeAttribute2 = Convert.ToString(row[TradeAttribute2]);
                        string tradeAttribute3 = Convert.ToString(row[TradeAttribute3]);
                        string tradeAttribute4 = Convert.ToString(row[TradeAttribute4]);
                        string tradeAttribute5 = Convert.ToString(row[TradeAttribute5]);
                        string tradeAttribute6 = Convert.ToString(row[TradeAttribute6]);
                        double executedQty = Convert.ToDouble(row[ExecutedQty]);
                        double multiplierValue = row[Multplier] != DBNull.Value ? Convert.ToDouble(row[Multplier]) : 1;
                        DateTime settlementDateValue = row[SettlementDate] != DBNull.Value ? Convert.ToDateTime(row[SettlementDate]) : DateTimeConstants.MinValue;
                        DateTime expirationDateValue = row[ExpirationDate] != DBNull.Value ? Convert.ToDateTime(row[ExpirationDate]) : DateTimeConstants.MinValue;
                        int strategyIDvalue = row[StrategyID] != DBNull.Value ? Convert.ToInt32(Convert.ToString(row[StrategyID])) : 0;
                        double notionalValue = row[NotionalValue] != DBNull.Value ? Convert.ToDouble(row[NotionalValue].ToString()) : 0d;
                        double benchMarkRateValue = row[BenchMarkRate] != DBNull.Value ? Convert.ToDouble(row[BenchMarkRate].ToString()) : 0d;
                        double differentialValue = row[Differential] != DBNull.Value ? Convert.ToDouble(row[Differential].ToString()) : 0d;
                        double origCostBasisValue = row[OrigCostBasis] != DBNull.Value ? Convert.ToDouble(row[OrigCostBasis].ToString()) : 0d;
                        int dayCountValue = row[DayCount] != DBNull.Value ? Convert.ToInt32(row[DayCount].ToString()) : 0;
                        string swapDescriptionValue = row[SwapDescription] != DBNull.Value ? row[SwapDescription].ToString() : string.Empty;
                        DateTime firstResetDateValue = row[FirstResetDate] != DBNull.Value ? Convert.ToDateTime(row[FirstResetDate]) : DateTimeConstants.MinValue;
                        DateTime origTransDateValue = row[OrigTransDate] != DBNull.Value ? Convert.ToDateTime(row[OrigTransDate]) : DateTimeConstants.MinValue;
                        bool isSwappedValue = row[IsSwapped] != DBNull.Value ? Convert.ToBoolean(row[IsSwapped].ToString()) : false;
                        string groupIDvalue = Convert.ToString(row[GroupID]);
                        int positiontagvalue = row[Positiontag] != DBNull.Value ? Convert.ToInt32(row[Positiontag].ToString()) : 0;
                        double fxRate = row[FxRate] != DBNull.Value ? Convert.ToDouble(row[FxRate].ToString()) : 1.0;
                        string FxConversionMethod = row[FxConversionMethodOperator] != DBNull.Value ? row[FxConversionMethodOperator].ToString() : string.Empty;
                        string companyName = row[CompanyName] != DBNull.Value ? row[CompanyName].ToString() : string.Empty;
                        Int64 taxlot_pk = row[Taxlot_pk] != DBNull.Value ? Convert.ToInt64(row[Taxlot_pk].ToString()) : 0;
                        Int64 parentRow_pk = row[ParentRow_Pk] != DBNull.Value ? Convert.ToInt64(row[ParentRow_Pk].ToString()) : 0;
                        double strikePrice = row[StrikePrice] != DBNull.Value ? Convert.ToDouble(row[StrikePrice].ToString()) : 0d;
                        string putOrCall = row[PutOrCall] != DBNull.Value ? row[PutOrCall].ToString() : string.Empty;
                        int leadCurrency = row[LeadCurrencyID] != DBNull.Value ? Convert.ToInt32(row[LeadCurrencyID].ToString()) : 0;
                        int vsCurrency = row[VsCurrencyID] != DBNull.Value ? Convert.ToInt32(row[VsCurrencyID].ToString()) : 0;
                        string transactionType = row[TransactionType] != DBNull.Value ? row[TransactionType].ToString() : string.Empty;
                        string internalComments = row[InternalComments] != DBNull.Value ? row[InternalComments].ToString() : string.Empty;
                        int tradingAccountId = row[TradingAccountId] != DBNull.Value ? Convert.ToInt32(row[TradingAccountId].ToString()) : 0;
                        int userID = row[UserID] != DBNull.Value ? Convert.ToInt32(row[UserID].ToString()) : 0;
                        DateTime nirvanaProcessDate = row[NirvanaProcessDate] != DBNull.Value ? Convert.ToDateTime(row[NirvanaProcessDate]) : DateTimeConstants.MinValue;
                        int settlCurrency = row[SettlCurrency] != DBNull.Value ? Convert.ToInt32(row[SettlCurrency].ToString()) : 0;
                        string BloombergSymbol = row[Bloomberg] != DBNull.Value ? row[Bloomberg].ToString() : string.Empty;
                        string SEDOLSymbol = row[SEDOL] != DBNull.Value ? row[SEDOL].ToString() : string.Empty;
                        string CUSIPSymbol = row[CUSIP] != DBNull.Value ? row[CUSIP].ToString() : string.Empty;
                        string ISINSymbol = row[ISIN] != DBNull.Value ? row[ISIN].ToString() : string.Empty;
                        string securityTypeName = row[SecurityTypeName] != DBNull.Value ? row[SecurityTypeName].ToString() : string.Empty;
                        string countryName = row[CountryName] != DBNull.Value ? row[CountryName].ToString() : string.Empty;
                        string ProxySymbol = row[Proxy] != DBNull.Value ? row[Proxy].ToString() : string.Empty;
                        string IDCOSymbol = row[IDCO] != DBNull.Value ? row[IDCO].ToString() : string.Empty;
                        string OSISymbol = row[OSI] != DBNull.Value ? row[OSI].ToString() : string.Empty;
                        string sectorName = row[SectorName] != DBNull.Value ? row[SectorName].ToString() : string.Empty;
                        string subSectorName = row[SubSectorName] != DBNull.Value ? row[SubSectorName].ToString() : string.Empty;
                        string reutersSymbol = row[ReutersSymbol] != DBNull.Value ? row[ReutersSymbol].ToString() : string.Empty;
                        string assetName = row[AssetName] != DBNull.Value ? row[AssetName].ToString() : string.Empty;
                        string additionalTradeAttributes = row[AdditionalTradeAttributes] != DBNull.Value ? row[AdditionalTradeAttributes].ToString() : string.Empty;

                        TaxLot allocatedTrade = new TaxLot();
                        allocatedTrade.LeadCurrencyID = leadCurrency;
                        allocatedTrade.VsCurrencyID = vsCurrency;
                        allocatedTrade.TaxLotID = taxlotIDValue;
                        allocatedTrade.CurrencyID = currencyIDValue;
                        allocatedTrade.AUECID = auecIDValue;
                        allocatedTrade.AvgPrice = averagePriceValue;
                        allocatedTrade.Level1ID = accountIDValue;
                        allocatedTrade.Level1Name = CachedDataManager.GetInstance.GetAccountText(accountIDValue);
                        allocatedTrade.Level2ID = strategyIDvalue;
                        allocatedTrade.Level2Name = CachedDataManager.GetInstance.GetStrategyText(strategyIDvalue);
                        allocatedTrade.AssetCategoryValue = (AssetCategory)assetIDValue;
                        allocatedTrade.AssetID = assetIDValue;
                        allocatedTrade.AssetName = CachedDataManager.GetInstance.GetAssetText(assetIDValue).ToString();
                        allocatedTrade.UnderlyingID = underLyingIDValue;
                        allocatedTrade.UnderlyingName = CachedDataManager.GetInstance.GetUnderLyingText(underLyingIDValue);
                        allocatedTrade.AUECLocalDate = tradeDateValue;
                        allocatedTrade.ProcessDate = ProcessDateValue;
                        allocatedTrade.OriginalPurchaseDate = OriginalPurchaseDateValue;
                        allocatedTrade.ExpirationDate = expirationDateValue;
                        allocatedTrade.OpenTotalCommissionandFees = TotalCommissionandFeesValue;
                        allocatedTrade.TaxLotQty = openQtyvalue;
                        allocatedTrade.OrderSideTagValue = sideIDValue;
                        allocatedTrade.Symbol = symbolValue;
                        allocatedTrade.ContractMultiplier = multiplierValue;
                        allocatedTrade.GroupID = groupIDvalue;
                        allocatedTrade.PositionTag = (PositionTag)positiontagvalue;
                        allocatedTrade.CompanyName = companyName;
                        allocatedTrade.UnderlyingSymbol = row[UnderlyingSymbol].ToString();
                        //uncommented as added TaxlotPk and ParentRowPk fields in Taxlot.cs
                        allocatedTrade.TaxlotPk = taxlot_pk;
                        allocatedTrade.ParentRowPk = parentRow_pk;
                        allocatedTrade.StrikePrice = strikePrice;
                        allocatedTrade.ExchangeID = exchangeIDvalue;
                        allocatedTrade.FXRate = fxRate;
                        allocatedTrade.SideMultiplier = Calculations.GetSideMultilpier(allocatedTrade.OrderSideTagValue);
                        allocatedTrade.ExchangeName = CachedDataManager.GetInstance.GetExchangeText(exchangeIDvalue);
                        if (putOrCall == OptionType.CALL.ToString().ToUpper())
                        {
                            allocatedTrade.PutOrCall = (int)FIXConstants.Underlying_Call;
                        }
                        else if (putOrCall == OptionType.PUT.ToString().ToUpper())
                        {
                            allocatedTrade.PutOrCall = (int)FIXConstants.Underlying_Put;
                        }
                        allocatedTrade.OrderSide = CommonDataCache.TagDatabaseManager.GetInstance.GetOrderSideText(sideIDValue);
                        allocatedTrade.FXConversionMethodOperator = FxConversionMethod;
                        if (isSwappedValue)
                        {
                            Prana.BusinessObjects.SwapParameters sw = new Prana.BusinessObjects.SwapParameters();
                            allocatedTrade.SwapParameters = sw;
                            sw.NotionalValue = notionalValue;
                            sw.BenchMarkRate = benchMarkRateValue;
                            sw.Differential = differentialValue;
                            sw.OrigCostBasis = origCostBasisValue;
                            sw.DayCount = dayCountValue;
                            sw.SwapDescription = swapDescriptionValue;
                            sw.FirstResetDate = firstResetDateValue;
                            sw.OrigTransDate = origTransDateValue;
                        }
                        allocatedTrade.ISSwap = isSwappedValue;
                        allocatedTrade.Description = description;
                        allocatedTrade.InternalComments = internalComments;
                        allocatedTrade.IsNDF = iSNDF;
                        allocatedTrade.FixingDate = fixingDate;
                        allocatedTrade.LotId = lotId;
                        allocatedTrade.ExternalTransId = externalTransId;
                        allocatedTrade.TradeAttribute1 = tradeAttribute1;
                        allocatedTrade.TradeAttribute2 = tradeAttribute2;
                        allocatedTrade.TradeAttribute3 = tradeAttribute3;
                        allocatedTrade.TradeAttribute4 = tradeAttribute4;
                        allocatedTrade.TradeAttribute5 = tradeAttribute5;
                        allocatedTrade.TradeAttribute6 = tradeAttribute6;
                        allocatedTrade.ExecutedQty = executedQty;
                        allocatedTrade.TransactionType = transactionType;
                        allocatedTrade.CompanyUserID = userID;
                        allocatedTrade.TradingAccountID = tradingAccountId;
                        allocatedTrade.CompanyUserName = CachedDataManager.GetInstance.GetUserText(userID);
                        allocatedTrade.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(tradingAccountId);
                        allocatedTrade.NirvanaProcessDate = nirvanaProcessDate;
                        allocatedTrade.SettlementCurrencyID = settlCurrency;
                        allocatedTrade.BloombergSymbol = BloombergSymbol;
                        allocatedTrade.SEDOLSymbol = SEDOLSymbol;
                        allocatedTrade.CusipSymbol = CUSIPSymbol;
                        allocatedTrade.ISINSymbol = ISINSymbol;
                        allocatedTrade.SecurityTypeName = securityTypeName;
                        allocatedTrade.CountryName = countryName;
                        allocatedTrade.ProxySymbol = ProxySymbol;
                        allocatedTrade.IDCOSymbol = IDCOSymbol;
                        allocatedTrade.OSISymbol = OSISymbol;
                        allocatedTrade.SectorName = sectorName;
                        allocatedTrade.SubSectorName = subSectorName;
                        allocatedTrade.ReutersSymbol = reutersSymbol;
                        allocatedTrade.UDAAsset = assetName;
                        allocatedTrade.SetTradeAttribute(additionalTradeAttributes);
                        openTaxlots.Add(allocatedTrade);
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
            return openTaxlots;
        }

        //Module: Close Trade/PM
        public static Dictionary<string, TaxlotClosingInfo> GetTaxlotsLatestClosingDates(DateTime toCloseDate, DateTime fromCloseDate, DateTime toTradeDate, DateTime fromTradeDate, bool isUpdateOpenTaxlots)
        {
            Dictionary<string, TaxlotClosingInfo> closingInfoDict = new Dictionary<string, TaxlotClosingInfo>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllClosedData";

                //increase CommandTimeout as P_GetAllClosedData is taking more time to execute.
                //http://jira.nirvanasolutions.com:8080/browse/CH-61
                queryData.CommandTimeout = 600;

                queryData.DictionaryDatabaseParameter.Add("@closingtodate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@closingtodate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = toCloseDate
                });
                queryData.DictionaryDatabaseParameter.Add("@closingfromdate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@closingfromdate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = fromCloseDate
                });
                queryData.DictionaryDatabaseParameter.Add("@totradedate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@totradedate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = toTradeDate
                });
                queryData.DictionaryDatabaseParameter.Add("@fromtradedate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@fromtradedate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = fromTradeDate
                });
                queryData.DictionaryDatabaseParameter.Add("@isUpdateOpenTaxlots", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@isUpdateOpenTaxlots",
                    ParameterType = DbType.Boolean,
                    ParameterValue = isUpdateOpenTaxlots
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        try
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            if (row != null)
                            {
                                int taxlotIdIndex = 0;
                                int taxlotClosingIdIndex = 1;
                                int taxlotUnderlyingIdIndex = 2;
                                int taxlotOpenQtyIndex = 3;
                                int closingDateIndex = 4;
                                int closingAlgoIndex = 5;
                                int IsManualyExerciseAssignIndex = 6;

                                string taxlotId = string.Empty;
                                string taxlotClosingId = string.Empty;
                                string taxlotUnderlyingId = string.Empty;
                                double taxlotOpenQuantity = 0.0;
                                string ClosingDate = string.Empty;
                                bool? IsManualyExerciseAssign = null;

                                int ClosingAlgo = 0;
                                if (row[taxlotIdIndex] != System.DBNull.Value)
                                {
                                    taxlotId = (row[taxlotIdIndex]).ToString();
                                }
                                if (row[taxlotClosingIdIndex] != System.DBNull.Value)
                                {
                                    taxlotClosingId = (row[taxlotClosingIdIndex]).ToString();
                                }
                                if (row[taxlotUnderlyingIdIndex] != System.DBNull.Value)
                                {
                                    taxlotUnderlyingId = (row[taxlotUnderlyingIdIndex]).ToString();
                                }
                                if (row[taxlotOpenQtyIndex] != System.DBNull.Value)
                                {
                                    var taxlotOpenQty = (row[taxlotOpenQtyIndex]).ToString();
                                    taxlotOpenQuantity = Convert.ToDouble(taxlotOpenQty);
                                }
                                if (row[closingDateIndex] != System.DBNull.Value)
                                {
                                    ClosingDate = (row[closingDateIndex]).ToString(); ;
                                }
                                if (row[closingAlgoIndex] != System.DBNull.Value)
                                {
                                    int.TryParse((row[closingAlgoIndex]).ToString(), out ClosingAlgo);
                                }
                                if (row[IsManualyExerciseAssignIndex] != System.DBNull.Value)
                                {
                                    bool _isManualyExerciseAssign;
                                    bool.TryParse((row[IsManualyExerciseAssignIndex]).ToString(), out _isManualyExerciseAssign);
                                    IsManualyExerciseAssign = _isManualyExerciseAssign;
                                }
                                if (closingInfoDict.ContainsKey(taxlotId))
                                {
                                    TaxlotClosingInfo info = closingInfoDict[taxlotId];

                                    info.TaxLotID = taxlotId;
                                    info.OpenQuantity = taxlotOpenQuantity;
                                    info.ListClosingId.Add(taxlotClosingId);
                                    if (!string.IsNullOrEmpty(taxlotUnderlyingId))
                                    {
                                        if (!info.DictExercisedUnderlying.ContainsKey(taxlotUnderlyingId))
                                        {
                                            info.DictExercisedUnderlying.Add(taxlotUnderlyingId, taxlotClosingId);
                                        }
                                    }
                                    if (taxlotOpenQuantity > 0)
                                    {
                                        info.ClosingStatus = ClosingStatus.PartiallyClosed;
                                    }
                                    else
                                    {
                                        info.ClosingStatus = ClosingStatus.Closed;
                                    }
                                    //Modified By : Manvendra Jira : PRANA-10341
                                    if (info.ClosingAlgo != ClosingAlgo)
                                    {
                                        info.ClosingAlgo = (int)PostTradeEnums.CloseTradeAlogrithm.Multiple;
                                    }
                                    else
                                    {
                                        info.ClosingAlgo = ClosingAlgo;
                                    }

                                    if (IsManualyExerciseAssign != null)
                                        info.IsManualyExerciseAssign = IsManualyExerciseAssign;
                                }
                                else
                                {
                                    TaxlotClosingInfo infoNew = new TaxlotClosingInfo();
                                    infoNew.TaxLotID = taxlotId;
                                    infoNew.OpenQuantity = taxlotOpenQuantity;
                                    closingInfoDict.Add(taxlotId, infoNew);
                                    infoNew.AUECMaxModifiedDate = Convert.ToDateTime(ClosingDate);
                                    if (taxlotOpenQuantity > 0)
                                    {
                                        infoNew.ClosingStatus = ClosingStatus.PartiallyClosed;
                                    }
                                    else
                                    {
                                        infoNew.ClosingStatus = ClosingStatus.Closed;
                                    }
                                    infoNew.ListClosingId.Add(taxlotClosingId);
                                    if (!string.IsNullOrEmpty(taxlotUnderlyingId))
                                    {
                                        if (!infoNew.DictExercisedUnderlying.ContainsKey(taxlotUnderlyingId))
                                            infoNew.DictExercisedUnderlying.Add(taxlotUnderlyingId, taxlotClosingId);
                                    }
                                    infoNew.ClosingAlgo = ClosingAlgo;

                                    if (IsManualyExerciseAssign != null)
                                        infoNew.IsManualyExerciseAssign = IsManualyExerciseAssign;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Invoke our policy that is responsible for making sure no secure information
                            // gets out of our layer.
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
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
                return null;
            }
            return closingInfoDict;
        }

        /// <summary>
        /// Gets the closed and Expired Data for the Close Trade form 
        /// </summary>
        /// <param name="closeTradeInterface">The close trade interface.</param>
        public static List<Position> GetNetPositionsList(string ToAllAUECDatesString, string FromAllAUECDatesString, string CommaSeparatedAccountIds, string CommaSeparatedAssetIds, string commaSeparatedSymbols, string customConditions, PostTradeEnums.DateType closingDateType)
        {

            try
            {
                List<Position> NetPositions = new List<Position>();
                int closingDateTypeParam = (int)closingDateType;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetClosedPositionsForDateRange_New";
                queryData.CommandTimeout = 20000;
                queryData.DictionaryDatabaseParameter.Add("@ToAllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ToAllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = ToAllAUECDatesString
                });
                queryData.DictionaryDatabaseParameter.Add("@FromAllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FromAllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = FromAllAUECDatesString
                });
                queryData.DictionaryDatabaseParameter.Add("@FundIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundIds",
                    ParameterType = DbType.String,
                    ParameterValue = CommaSeparatedAccountIds
                });
                queryData.DictionaryDatabaseParameter.Add("@AssetIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AssetIds",
                    ParameterType = DbType.String,
                    ParameterValue = CommaSeparatedAssetIds
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
                queryData.DictionaryDatabaseParameter.Add("@ClosingDateType", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ClosingDateType",
                    ParameterType = DbType.Int16,
                    ParameterValue = closingDateTypeParam
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    int PositionaltaxLotID = 0;
                    int ClosingtaxLotID = 1;
                    int symbol = 2;
                    int PositionSideID = 3;
                    int ClosingSideID = 4;
                    int PositionTradeDate = 5;
                    int ClosingTradeDate = 6;
                    int OpenPrice = 7;
                    int ClosingPrice = 8;
                    int AccountID = 9;
                    int Level2ID = 10;
                    int AssetID = 11;
                    int UnderLyingID = 12;
                    int ExchangeID = 13;
                    int CurrencyID = 14;
                    int PositionalTaxlotCommission = 15;
                    int ClosingTaxlotCommission = 16;
                    int ClosingMode = 17;
                    int Multiplier = 18;
                    int OpeiningPositionTag = 19;
                    int ClosingPositionTag = 20;
                    int ClosedQty = 21;
                    int PositionalNotionalValue = 22;
                    int PositionalBenchMarkRate = 23;
                    int PositionalDifferential = 24;
                    int PositionalOrigCostBasis = 25;
                    int PositionalDayCount = 26;
                    int PositionalSwapDescription = 27;
                    int PositionalFirstResetDate = 28;
                    int PositionalOrigTransDate = 29;
                    int ClosingNotionalValue = 30;
                    int ClosingBenchMarkRate = 31;
                    int ClosingDifferential = 32;
                    int ClosingOrigCostBasis = 33;
                    int ClosingDayCount = 34;
                    int ClosingSwapDescription = 35;
                    int ClosingFirstResetDate = 36;
                    int ClosingOrigTransDate = 37;
                    int PositionalIsSwapped = 38;
                    int ClosingIsSwapped = 39;
                    int TaxLotClosingId = 40;
                    int PositionSide = 41;
                    int ClosingAlgoIndex = 42;
                    int LeadCurrencyId = 46;
                    int VsCurrencyId = 47;
                    int tradeAttribute1 = 48;
                    int tradeAttribute2 = 49;
                    int tradeAttribute3 = 50;
                    int tradeAttribute4 = 51;
                    int tradeAttribute5 = 52;
                    int tradeAttribute6 = 53;
                    int additionalTradeAttribute = 54;

                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            string positionaltaxLotIDValue = Convert.ToString(row[PositionaltaxLotID]);
                            string closingtaxLotIDValue = Convert.ToString(row[ClosingtaxLotID]);
                            string symbolValue = Convert.ToString(row[symbol]);
                            string positionsideIDValue = row[PositionSideID] != DBNull.Value ? Convert.ToString(row[PositionSideID]).Trim() : string.Empty;
                            string closingsideIDValue = row[ClosingSideID] != DBNull.Value ? Convert.ToString(row[ClosingSideID]).Trim() : string.Empty;
                            DateTime positiontradeDateValue = row[PositionTradeDate] != DBNull.Value ? Convert.ToDateTime(Convert.ToString(row[PositionTradeDate])) : DateTimeConstants.MinValue;
                            DateTime closingtradeDateValue = row[ClosingTradeDate] != DBNull.Value ? Convert.ToDateTime(Convert.ToString(row[ClosingTradeDate])) : DateTimeConstants.MinValue;
                            double openaveragePriceValue = row[OpenPrice] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[OpenPrice])) : 0d;
                            double closedaveragePriceValue = row[ClosingPrice] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[ClosingPrice])) : 0d;
                            int accountIDValue = Convert.ToInt32(row[AccountID].ToString());
                            int level2IDValue = Convert.ToInt32(row[Level2ID].ToString());
                            int assetIDValue = Convert.ToInt32(row[AssetID].ToString());
                            int underLyingIDValue = Convert.ToInt32(row[UnderLyingID].ToString());
                            int exchangeIDvalue = Convert.ToInt32(row[ExchangeID].ToString());
                            int currencyIDValue = Convert.ToInt32(row[CurrencyID].ToString());
                            double positionalCommissionandFeesValue = row[PositionalTaxlotCommission] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[PositionalTaxlotCommission])) : 0d;
                            double closingCommissionandFeesValue = row[ClosingTaxlotCommission] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[ClosingTaxlotCommission])) : 0d;
                            int intclosingModeValue = row[ClosingMode] != DBNull.Value ? Convert.ToInt32(Convert.ToString(row[ClosingMode])) : 0;
                            double multiplierValue = row[Multiplier] != DBNull.Value ? Convert.ToDouble(row[Multiplier]) : 1;
                            int intPositionTagvalue = row[OpeiningPositionTag] != DBNull.Value ? Convert.ToInt32(row[OpeiningPositionTag]) : 0;
                            double intclosingtaxlottagvalue = row[ClosingPositionTag] != DBNull.Value ? Convert.ToInt32(row[ClosingPositionTag]) : 1;
                            double closeQtyValue = Convert.ToDouble(row[ClosedQty].ToString());
                            double positionalnotionalValue = row[PositionalNotionalValue] != DBNull.Value ? Convert.ToDouble(row[PositionalNotionalValue].ToString()) : 0d;
                            double positionalbenchMarkRateValue = row[PositionalBenchMarkRate] != DBNull.Value ? Convert.ToDouble(row[PositionalBenchMarkRate].ToString()) : 0d;
                            double positionaldifferentialValue = row[PositionalDifferential] != DBNull.Value ? Convert.ToDouble(row[PositionalDifferential].ToString()) : 0d;
                            double positionalorigCostBasisValue = row[PositionalOrigCostBasis] != DBNull.Value ? Convert.ToDouble(row[PositionalOrigCostBasis].ToString()) : 0d;
                            int positionaldayCountValue = row[PositionalDayCount] != DBNull.Value ? Convert.ToInt32(row[PositionalDayCount].ToString()) : 0;
                            string positionalswapDescriptionValue = row[PositionalSwapDescription] != DBNull.Value ? row[PositionalSwapDescription].ToString() : string.Empty;
                            DateTime positionalfirstResetDateValue = row[PositionalFirstResetDate] != DBNull.Value ? Convert.ToDateTime(row[PositionalFirstResetDate]) : DateTimeConstants.MinValue;
                            DateTime positionalorigTransDateValue = row[PositionalOrigTransDate] != DBNull.Value ? Convert.ToDateTime(row[PositionalOrigTransDate]) : DateTimeConstants.MinValue;
                            bool positionalisSwappedValue = row[PositionalIsSwapped] != DBNull.Value ? Convert.ToBoolean(row[PositionalIsSwapped].ToString()) : false;
                            double closingnotionalValue = row[ClosingNotionalValue] != DBNull.Value ? Convert.ToDouble(row[ClosingNotionalValue].ToString()) : 0d;
                            double closingbenchMarkRateValue = row[ClosingBenchMarkRate] != DBNull.Value ? Convert.ToDouble(row[ClosingBenchMarkRate].ToString()) : 0d;
                            double closingdifferentialValue = row[ClosingDifferential] != DBNull.Value ? Convert.ToDouble(row[ClosingDifferential].ToString()) : 0d;
                            double closingorigCostBasisValue = row[ClosingOrigCostBasis] != DBNull.Value ? Convert.ToDouble(row[ClosingOrigCostBasis].ToString()) : 0d;
                            int closingdayCountValue = row[ClosingDayCount] != DBNull.Value ? Convert.ToInt32(row[ClosingDayCount].ToString()) : 0;
                            string closingswapDescriptionValue = row[ClosingSwapDescription] != DBNull.Value ? row[ClosingSwapDescription].ToString() : string.Empty;
                            DateTime closingfirstResetDateValue = row[ClosingFirstResetDate] != DBNull.Value ? Convert.ToDateTime(row[ClosingFirstResetDate]) : DateTimeConstants.MinValue;
                            DateTime closingorigTransDateValue = row[ClosingOrigTransDate] != DBNull.Value ? Convert.ToDateTime(row[ClosingOrigTransDate]) : DateTimeConstants.MinValue;
                            bool closingisSwappedValue = row[ClosingIsSwapped] != DBNull.Value ? Convert.ToBoolean(row[ClosingIsSwapped].ToString()) : false;
                            string taxLotClosingIdValue = row[TaxLotClosingId].ToString();
                            string positionSide = row[PositionSide] != DBNull.Value ? row[PositionSide].ToString() : string.Empty;
                            int closingAlgo = row[ClosingAlgoIndex] != DBNull.Value ? Convert.ToInt32(row[ClosingAlgoIndex]) : (int)PostTradeEnums.CloseTradeAlogrithm.NONE;
                            int leadCurrencyId = row[LeadCurrencyId] != DBNull.Value ? Convert.ToInt32(row[LeadCurrencyId].ToString()) : 0;
                            int vsCurrencyId = row[VsCurrencyId] != DBNull.Value ? Convert.ToInt32(row[VsCurrencyId].ToString()) : 0;
                            string tradeAttribute1Value = row[tradeAttribute1] != DBNull.Value ? Convert.ToString(row[tradeAttribute1].ToString()) : string.Empty;
                            string tradeAttribute2Value = row[tradeAttribute2] != DBNull.Value ? Convert.ToString(row[tradeAttribute2].ToString()) : string.Empty;
                            string tradeAttribute3Value = row[tradeAttribute3] != DBNull.Value ? Convert.ToString(row[tradeAttribute3].ToString()) : string.Empty;
                            string tradeAttribute4Value = row[tradeAttribute4] != DBNull.Value ? Convert.ToString(row[tradeAttribute4].ToString()) : string.Empty;
                            string tradeAttribute5Value = row[tradeAttribute5] != DBNull.Value ? Convert.ToString(row[tradeAttribute5].ToString()) : string.Empty;
                            string tradeAttribute6Value = row[tradeAttribute6] != DBNull.Value ? Convert.ToString(row[tradeAttribute6].ToString()) : string.Empty;
                            string additionalTradeAttributeValue = row[additionalTradeAttribute] != DBNull.Value ? Convert.ToString(row[additionalTradeAttribute].ToString()) : string.Empty;
                            Position position = new Position();

                            position.ID = positionaltaxLotIDValue;
                            position.ClosingID = closingtaxLotIDValue;
                            position.Symbol = symbolValue;
                            position.Side = TagDatabaseManager.GetInstance.GetOrderSideText(positionsideIDValue); ;
                            position.ClosingSide = TagDatabaseManager.GetInstance.GetOrderSideText(closingsideIDValue); ;
                            position.TradeDate = positiontradeDateValue;
                            position.ClosingTradeDate = closingtradeDateValue;
                            position.OpenAveragePrice = openaveragePriceValue;
                            position.ClosedAveragePrice = closedaveragePriceValue;
                            position.AccountID = accountIDValue;
                            position.AccountValue.ID = accountIDValue;
                            position.AccountValue.FullName = CachedDataManager.GetInstance.GetAccountText(accountIDValue);
                            position.StrategyID = level2IDValue;
                            position.Strategy = CachedDataManager.GetInstance.GetStrategyText(position.StrategyID);
                            position.AssetCategoryValue = (AssetCategory)assetIDValue;
                            position.UnderlyingName = CachedDataManager.GetInstance.GetUnderLyingText(underLyingIDValue);
                            position.Exchange = CachedDataManager.GetInstance.GetExchangeText(exchangeIDvalue);
                            position.CurrencyID = currencyIDValue;
                            position.Currency = CachedDataManager.GetInstance.GetCurrencyText(currencyIDValue);
                            position.PositionTotalCommissionandFees = positionalCommissionandFeesValue;
                            position.ClosingTotalCommissionandFees = closingCommissionandFeesValue;
                            position.Multiplier = multiplierValue;
                            position.IntClosingMode = intclosingModeValue;
                            position.PositionalTag = (PositionTag)intPositionTagvalue;
                            position.ClosingPositionTag = (PositionTag)intclosingtaxlottagvalue;
                            position.ClosedQty = closeQtyValue;
                            position.TaxLotClosingId = taxLotClosingIdValue;
                            position.ClosingTotalCommissionandFees = closingCommissionandFeesValue;
                            position.PositionSide = positionSide;
                            position.ClosingAlgo = closingAlgo;
                            position.LeadCurrencyID = leadCurrencyId;
                            position.VsCurrencyID = vsCurrencyId;
                            if (position.ClosingMode != Prana.BusinessObjects.AppConstants.ClosingMode.Offset)
                            {
                                position.IsExpired_Settled = true;
                            }
                            if (position.ClosingMode != Prana.BusinessObjects.AppConstants.ClosingMode.CorporateAction)
                            {
                                switch (positionsideIDValue)
                                {
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Buy:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Buy_Open:
                                        position.PositionalTag = Prana.BusinessObjects.AppConstants.PositionTag.Long;
                                        break;

                                    case Prana.BusinessObjects.FIXConstants.SIDE_Sell_Open:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_SellShort:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Sell:
                                        position.PositionalTag = Prana.BusinessObjects.AppConstants.PositionTag.Short;
                                        break;
                                }
                                switch (closingsideIDValue)
                                {
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Buy:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Buy_Open:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Buy_Closed:
                                        position.ClosingPositionTag = Prana.BusinessObjects.AppConstants.PositionTag.Long;
                                        break;

                                    case Prana.BusinessObjects.FIXConstants.SIDE_Sell_Open:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_SellShort:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Sell:
                                        position.ClosingPositionTag = Prana.BusinessObjects.AppConstants.PositionTag.Short;
                                        break;
                                }
                            }
                            if (positionalisSwappedValue)
                            {
                                Prana.BusinessObjects.SwapParameters positionswap = new Prana.BusinessObjects.SwapParameters();
                                position.PositionSwapParameters = positionswap;
                                positionswap.NotionalValue = positionalnotionalValue;
                                positionswap.BenchMarkRate = positionalbenchMarkRateValue;
                                positionswap.Differential = positionaldifferentialValue;
                                positionswap.OrigCostBasis = positionalorigCostBasisValue;
                                positionswap.DayCount = positionaldayCountValue;
                                positionswap.SwapDescription = positionalswapDescriptionValue;
                                positionswap.FirstResetDate = positionalfirstResetDateValue;
                                positionswap.OrigTransDate = positionalorigTransDateValue;
                            }
                            position.IsPositionSwapped = positionalisSwappedValue;
                            if (closingisSwappedValue)
                            {
                                Prana.BusinessObjects.SwapParameters closingswap = new Prana.BusinessObjects.SwapParameters();
                                position.PositionSwapParameters = closingswap;
                                closingswap.NotionalValue = closingnotionalValue;
                                closingswap.BenchMarkRate = closingbenchMarkRateValue;
                                closingswap.Differential = closingdifferentialValue;
                                closingswap.OrigCostBasis = closingorigCostBasisValue;
                                closingswap.DayCount = closingdayCountValue;
                                closingswap.SwapDescription = closingswapDescriptionValue;
                                closingswap.FirstResetDate = closingfirstResetDateValue;
                                closingswap.OrigTransDate = closingorigTransDateValue;
                            }
                            position.IsClosedSwapped = closingisSwappedValue;
                            Prana.BusinessLogic.Calculations.SetAveragePriceRealizedPNL(position);
                            position.TradeAttribute1 = tradeAttribute1Value;
                            position.TradeAttribute2 = tradeAttribute2Value;
                            position.TradeAttribute3 = tradeAttribute3Value;
                            position.TradeAttribute4 = tradeAttribute4Value;
                            position.TradeAttribute5 = tradeAttribute5Value;
                            position.TradeAttribute6 = tradeAttribute6Value;
                            position.SetTradeAttribute(additionalTradeAttributeValue);

                            NetPositions.Add(position);
                        }
                    }
                }
                return NetPositions;
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
                return null;
            }
        }

        public static List<Position> GetNetPositionsListForASymbol(string ToAllAUECDatesString, string FromAllAUECDatesString, string CommaSeparatedAccountIds, string Symbol, string groupID)
        {
            try
            {
                List<Position> NetPositions = new List<Position>();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetClosedPositionsForASymbol";
                queryData.CommandTimeout = 20000;
                queryData.DictionaryDatabaseParameter.Add("@ToAllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ToAllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = ToAllAUECDatesString
                });
                queryData.DictionaryDatabaseParameter.Add("@FromAllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FromAllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = FromAllAUECDatesString
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
                    ParameterValue = Symbol
                });
                queryData.DictionaryDatabaseParameter.Add("@GroupID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@GroupID",
                    ParameterType = DbType.String,
                    ParameterValue = groupID
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    int PositionaltaxLotID = 0;
                    int ClosingtaxLotID = 1;
                    int symbol = 2;
                    int PositionSideID = 3;
                    int ClosingSideID = 4;
                    int PositionTradeDate = 5;
                    int ClosingTradeDate = 6;
                    int OpenPrice = 7;
                    int ClosingPrice = 8;
                    int AccountID = 9;
                    int Level2ID = 10;
                    int AssetID = 11;
                    int UnderLyingID = 12;
                    int ExchangeID = 13;
                    int CurrencyID = 14;
                    int PositionalTaxlotCommission = 15;
                    int ClosingTaxlotCommission = 16;
                    int ClosingMode = 17;
                    int Multiplier = 18;
                    int OpeiningPositionTag = 19;
                    int ClosingPositionTag = 20;
                    int ClosedQty = 21;
                    int PositionalNotionalValue = 22;
                    int PositionalBenchMarkRate = 23;
                    int PositionalDifferential = 24;
                    int PositionalOrigCostBasis = 25;
                    int PositionalDayCount = 26;
                    int PositionalSwapDescription = 27;
                    int PositionalFirstResetDate = 28;
                    int PositionalOrigTransDate = 29;
                    int ClosingNotionalValue = 30;
                    int ClosingBenchMarkRate = 31;
                    int ClosingDifferential = 32;
                    int ClosingOrigCostBasis = 33;
                    int ClosingDayCount = 34;
                    int ClosingSwapDescription = 35;
                    int ClosingFirstResetDate = 36;
                    int ClosingOrigTransDate = 37;
                    int PositionalIsSwapped = 38;
                    int ClosingIsSwapped = 39;
                    int TaxLotClosingId = 40;
                    int PositionSide = 41;
                    int ClosingAlgoIndex = 42;
                    int tradeAttribute1 = 43;
                    int tradeAttribute2 = 44;
                    int tradeAttribute3 = 45;
                    int tradeAttribute4 = 46;
                    int tradeAttribute5 = 47;
                    int tradeAttribute6 = 48;
                    int additionalTradeAttribute = 49;

                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            string positionaltaxLotIDValue = Convert.ToString(row[PositionaltaxLotID]);
                            string closingtaxLotIDValue = Convert.ToString(row[ClosingtaxLotID]);
                            string symbolValue = Convert.ToString(row[symbol]);
                            string positionsideIDValue = row[PositionSideID] != DBNull.Value ? Convert.ToString(row[PositionSideID]).Trim() : string.Empty;
                            string closingsideIDValue = row[ClosingSideID] != DBNull.Value ? Convert.ToString(row[ClosingSideID]).Trim() : string.Empty;
                            DateTime positiontradeDateValue = row[PositionTradeDate] != DBNull.Value ? Convert.ToDateTime(Convert.ToString(row[PositionTradeDate])) : DateTimeConstants.MinValue;
                            DateTime closingtradeDateValue = row[ClosingTradeDate] != DBNull.Value ? Convert.ToDateTime(Convert.ToString(row[ClosingTradeDate])) : DateTimeConstants.MinValue;
                            double openaveragePriceValue = row[OpenPrice] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[OpenPrice])) : 0d;
                            double closedaveragePriceValue = row[ClosingPrice] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[ClosingPrice])) : 0d;
                            int accountIDValue = Convert.ToInt32(row[AccountID].ToString());
                            int level2IDValue = Convert.ToInt32(row[Level2ID].ToString());
                            int assetIDValue = Convert.ToInt32(row[AssetID].ToString());
                            int underLyingIDValue = Convert.ToInt32(row[UnderLyingID].ToString());
                            int exchangeIDvalue = Convert.ToInt32(row[ExchangeID].ToString());
                            int currencyIDValue = Convert.ToInt32(row[CurrencyID].ToString());
                            double positionalCommissionandFeesValue = row[PositionalTaxlotCommission] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[PositionalTaxlotCommission])) : 0d;
                            double closingCommissionandFeesValue = row[ClosingTaxlotCommission] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[ClosingTaxlotCommission])) : 0d;
                            int intclosingModeValue = row[ClosingMode] != DBNull.Value ? Convert.ToInt32(Convert.ToString(row[ClosingMode])) : 0;
                            double multiplierValue = row[Multiplier] != DBNull.Value ? Convert.ToDouble(row[Multiplier]) : 1;
                            int intPositionTagvalue = row[OpeiningPositionTag] != DBNull.Value ? Convert.ToInt32(row[OpeiningPositionTag]) : 0;
                            double intclosingtaxlottagvalue = row[ClosingPositionTag] != DBNull.Value ? Convert.ToInt32(row[ClosingPositionTag]) : 1;
                            double closeQtyValue = Convert.ToDouble(row[ClosedQty].ToString());
                            double positionalnotionalValue = row[PositionalNotionalValue] != DBNull.Value ? Convert.ToDouble(row[PositionalNotionalValue].ToString()) : 0d;
                            double positionalbenchMarkRateValue = row[PositionalBenchMarkRate] != DBNull.Value ? Convert.ToDouble(row[PositionalBenchMarkRate].ToString()) : 0d;
                            double positionaldifferentialValue = row[PositionalDifferential] != DBNull.Value ? Convert.ToDouble(row[PositionalDifferential].ToString()) : 0d;
                            double positionalorigCostBasisValue = row[PositionalOrigCostBasis] != DBNull.Value ? Convert.ToDouble(row[PositionalOrigCostBasis].ToString()) : 0d;
                            int positionaldayCountValue = row[PositionalDayCount] != DBNull.Value ? Convert.ToInt32(row[PositionalDayCount].ToString()) : 0;
                            string positionalswapDescriptionValue = row[PositionalSwapDescription] != DBNull.Value ? row[PositionalSwapDescription].ToString() : string.Empty;
                            DateTime positionalfirstResetDateValue = row[PositionalFirstResetDate] != DBNull.Value ? Convert.ToDateTime(row[PositionalFirstResetDate]) : DateTimeConstants.MinValue;
                            DateTime positionalorigTransDateValue = row[PositionalOrigTransDate] != DBNull.Value ? Convert.ToDateTime(row[PositionalOrigTransDate]) : DateTimeConstants.MinValue;
                            bool positionalisSwappedValue = row[PositionalIsSwapped] != DBNull.Value ? Convert.ToBoolean(row[PositionalIsSwapped].ToString()) : false;

                            double closingnotionalValue = row[ClosingNotionalValue] != DBNull.Value ? Convert.ToDouble(row[ClosingNotionalValue].ToString()) : 0d;
                            double closingbenchMarkRateValue = row[ClosingBenchMarkRate] != DBNull.Value ? Convert.ToDouble(row[ClosingBenchMarkRate].ToString()) : 0d;
                            double closingdifferentialValue = row[ClosingDifferential] != DBNull.Value ? Convert.ToDouble(row[ClosingDifferential].ToString()) : 0d;
                            double closingorigCostBasisValue = row[ClosingOrigCostBasis] != DBNull.Value ? Convert.ToDouble(row[ClosingOrigCostBasis].ToString()) : 0d;
                            int closingdayCountValue = row[ClosingDayCount] != DBNull.Value ? Convert.ToInt32(row[ClosingDayCount].ToString()) : 0;
                            string closingswapDescriptionValue = row[ClosingSwapDescription] != DBNull.Value ? row[ClosingSwapDescription].ToString() : string.Empty;
                            DateTime closingfirstResetDateValue = row[ClosingFirstResetDate] != DBNull.Value ? Convert.ToDateTime(row[ClosingFirstResetDate]) : DateTimeConstants.MinValue;
                            DateTime closingorigTransDateValue = row[ClosingOrigTransDate] != DBNull.Value ? Convert.ToDateTime(row[ClosingOrigTransDate]) : DateTimeConstants.MinValue;
                            bool closingisSwappedValue = row[ClosingIsSwapped] != DBNull.Value ? Convert.ToBoolean(row[ClosingIsSwapped].ToString()) : false;
                            string taxLotClosingIdValue = row[TaxLotClosingId].ToString();
                            string positionSide = row[PositionSide] != DBNull.Value ? row[PositionSide].ToString() : string.Empty;
                            int closingAlgo = row[ClosingAlgoIndex] != DBNull.Value ? Convert.ToInt32(row[ClosingAlgoIndex]) : (int)PostTradeEnums.CloseTradeAlogrithm.NONE;
                            string tradeAttribute1Value = row[tradeAttribute1] != DBNull.Value ? Convert.ToString(row[tradeAttribute1].ToString()) : string.Empty;
                            string tradeAttribute2Value = row[tradeAttribute2] != DBNull.Value ? Convert.ToString(row[tradeAttribute2].ToString()) : string.Empty;
                            string tradeAttribute3Value = row[tradeAttribute3] != DBNull.Value ? Convert.ToString(row[tradeAttribute3].ToString()) : string.Empty;
                            string tradeAttribute4Value = row[tradeAttribute4] != DBNull.Value ? Convert.ToString(row[tradeAttribute4].ToString()) : string.Empty;
                            string tradeAttribute5Value = row[tradeAttribute5] != DBNull.Value ? Convert.ToString(row[tradeAttribute5].ToString()) : string.Empty;
                            string tradeAttribute6Value = row[tradeAttribute6] != DBNull.Value ? Convert.ToString(row[tradeAttribute6].ToString()) : string.Empty;
                            string additionalTradeAttributeValue = row[additionalTradeAttribute] != DBNull.Value ? Convert.ToString(row[additionalTradeAttribute].ToString()) : string.Empty;

                            Position position = new Position();

                            position.ID = positionaltaxLotIDValue;
                            position.ClosingID = closingtaxLotIDValue;
                            position.Symbol = symbolValue;
                            position.Side = TagDatabaseManager.GetInstance.GetOrderSideText(positionsideIDValue); ;
                            position.ClosingSide = TagDatabaseManager.GetInstance.GetOrderSideText(closingsideIDValue); ;
                            position.TradeDate = positiontradeDateValue;
                            position.ClosingTradeDate = closingtradeDateValue;
                            position.OpenAveragePrice = openaveragePriceValue;
                            position.ClosedAveragePrice = closedaveragePriceValue;
                            position.AccountID = accountIDValue;
                            position.AccountValue.ID = accountIDValue;
                            position.AccountValue.FullName = CachedDataManager.GetInstance.GetAccountText(accountIDValue);
                            position.StrategyID = level2IDValue;
                            position.Strategy = CachedDataManager.GetInstance.GetStrategyText(position.StrategyID);
                            position.AssetCategoryValue = (AssetCategory)assetIDValue;
                            position.UnderlyingName = CachedDataManager.GetInstance.GetUnderLyingText(underLyingIDValue);
                            position.Exchange = CachedDataManager.GetInstance.GetExchangeText(exchangeIDvalue);
                            position.CurrencyID = currencyIDValue;
                            position.Currency = CachedDataManager.GetInstance.GetCurrencyText(currencyIDValue);
                            position.PositionTotalCommissionandFees = positionalCommissionandFeesValue;
                            position.ClosingTotalCommissionandFees = closingCommissionandFeesValue;
                            position.Multiplier = multiplierValue;
                            position.IntClosingMode = intclosingModeValue;
                            position.PositionalTag = (PositionTag)intPositionTagvalue;
                            position.ClosingPositionTag = (PositionTag)intclosingtaxlottagvalue;
                            position.ClosedQty = closeQtyValue;
                            position.TaxLotClosingId = taxLotClosingIdValue;
                            position.ClosingTotalCommissionandFees = closingCommissionandFeesValue;
                            position.PositionSide = positionSide;
                            position.ClosingAlgo = closingAlgo;
                            if (position.ClosingMode != Prana.BusinessObjects.AppConstants.ClosingMode.Offset)
                            {
                                position.IsExpired_Settled = true;
                            }
                            if (position.ClosingMode != Prana.BusinessObjects.AppConstants.ClosingMode.CorporateAction)
                            {
                                switch (positionsideIDValue)
                                {
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Buy:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Buy_Open:
                                        position.PositionalTag = Prana.BusinessObjects.AppConstants.PositionTag.Long;
                                        break;

                                    case Prana.BusinessObjects.FIXConstants.SIDE_Sell_Open:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_SellShort:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Sell:
                                        position.PositionalTag = Prana.BusinessObjects.AppConstants.PositionTag.Short;
                                        break;
                                }
                                switch (closingsideIDValue)
                                {
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Buy:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Buy_Open:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Buy_Closed:
                                        position.ClosingPositionTag = Prana.BusinessObjects.AppConstants.PositionTag.Long;
                                        break;

                                    case Prana.BusinessObjects.FIXConstants.SIDE_Sell_Open:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_SellShort:
                                    case Prana.BusinessObjects.FIXConstants.SIDE_Sell:
                                        position.ClosingPositionTag = Prana.BusinessObjects.AppConstants.PositionTag.Short;
                                        break;
                                }
                            }

                            if (positionalisSwappedValue)
                            {
                                Prana.BusinessObjects.SwapParameters positionswap = new Prana.BusinessObjects.SwapParameters();
                                position.PositionSwapParameters = positionswap;
                                positionswap.NotionalValue = positionalnotionalValue;
                                positionswap.BenchMarkRate = positionalbenchMarkRateValue;
                                positionswap.Differential = positionaldifferentialValue;
                                positionswap.OrigCostBasis = positionalorigCostBasisValue;
                                positionswap.DayCount = positionaldayCountValue;
                                positionswap.SwapDescription = positionalswapDescriptionValue;
                                positionswap.FirstResetDate = positionalfirstResetDateValue;
                                positionswap.OrigTransDate = positionalorigTransDateValue;
                            }
                            position.IsPositionSwapped = positionalisSwappedValue;
                            if (closingisSwappedValue)
                            {
                                Prana.BusinessObjects.SwapParameters closingswap = new Prana.BusinessObjects.SwapParameters();
                                position.PositionSwapParameters = closingswap;
                                closingswap.NotionalValue = closingnotionalValue;
                                closingswap.BenchMarkRate = closingbenchMarkRateValue;
                                closingswap.Differential = closingdifferentialValue;
                                closingswap.OrigCostBasis = closingorigCostBasisValue;
                                closingswap.DayCount = closingdayCountValue;
                                closingswap.SwapDescription = closingswapDescriptionValue;
                                closingswap.FirstResetDate = closingfirstResetDateValue;
                                closingswap.OrigTransDate = closingorigTransDateValue;
                            }
                            position.IsClosedSwapped = closingisSwappedValue;
                            position.TradeAttribute1 = tradeAttribute1Value;
                            position.TradeAttribute2 = tradeAttribute2Value;
                            position.TradeAttribute3 = tradeAttribute3Value;
                            position.TradeAttribute4 = tradeAttribute4Value;
                            position.TradeAttribute5 = tradeAttribute5Value;
                            position.TradeAttribute6 = tradeAttribute6Value;
                            position.SetTradeAttribute(additionalTradeAttributeValue);
                            Prana.BusinessLogic.Calculations.SetAveragePriceRealizedPNL(position);
                            NetPositions.Add(position);
                        }
                    }
                }
                return NetPositions;
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
                return null;
            }
        }

        public static List<Position> GetNetPositionsForTaxlotIds(string CommaSeparatedTaxlotIds)
        {
            try
            {
                List<Position> NetPositions = new List<Position>();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetClosedPositionsForTaxlotIds";
                queryData.CommandTimeout = 20000;
                queryData.DictionaryDatabaseParameter.Add("@TaxlotIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@TaxlotIds",
                    ParameterType = DbType.String,
                    ParameterValue = CommaSeparatedTaxlotIds
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    NetPositions = GetPositionsFromSqlDataReader(reader, ApplicationConstants.TaxLotState.Updated);
                }
                return NetPositions;
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
                return null;
            }
        }

        private static List<Position> GetPositionsFromSqlDataReader(IDataReader reader, ApplicationConstants.TaxLotState positionState)
        {
            List<Position> NetPositions = new List<Position>();
            try
            {
                int PositionaltaxLotID = 0;
                int ClosingtaxLotID = 1;
                int symbol = 2;
                int PositionSideID = 3;
                int ClosingSideID = 4;
                int PositionTradeDate = 5;
                int ClosingTradeDate = 6;
                int OpenPrice = 7;
                int ClosingPrice = 8;
                int AccountID = 9;
                int Level2ID = 10;
                int AssetID = 11;
                int UnderLyingID = 12;
                int ExchangeID = 13;
                int CurrencyID = 14;
                int PositionalTaxlotCommission = 15;
                int ClosingTaxlotCommission = 16;
                int ClosingMode = 17;
                int Multiplier = 18;
                int OpeiningPositionTag = 19;
                int ClosingPositionTag = 20;
                int ClosedQty = 21;
                int PositionalNotionalValue = 22;
                int PositionalBenchMarkRate = 23;
                int PositionalDifferential = 24;
                int PositionalOrigCostBasis = 25;
                int PositionalDayCount = 26;
                int PositionalSwapDescription = 27;
                int PositionalFirstResetDate = 28;
                int PositionalOrigTransDate = 29;
                int ClosingNotionalValue = 30;
                int ClosingBenchMarkRate = 31;
                int ClosingDifferential = 32;
                int ClosingOrigCostBasis = 33;
                int ClosingDayCount = 34;
                int ClosingSwapDescription = 35;
                int ClosingFirstResetDate = 36;
                int ClosingOrigTransDate = 37;
                int PositionalIsSwapped = 38;
                int ClosingIsSwapped = 39;
                int TaxLotClosingId = 40;
                int PositionSide = 41;
                int ClosingAlgoIndex = 42;
                int LeadCurrencyId = 43;
                int VsCurrencyId = 44;

                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    if (row != null)
                    {
                        string positionaltaxLotIDValue = Convert.ToString(row[PositionaltaxLotID]);
                        string closingtaxLotIDValue = Convert.ToString(row[ClosingtaxLotID]);
                        string symbolValue = Convert.ToString(row[symbol]);
                        string positionsideIDValue = row[PositionSideID] != DBNull.Value ? Convert.ToString(row[PositionSideID]).Trim() : string.Empty;
                        string closingsideIDValue = row[ClosingSideID] != DBNull.Value ? Convert.ToString(row[ClosingSideID]).Trim() : string.Empty;
                        DateTime positiontradeDateValue = row[PositionTradeDate] != DBNull.Value ? Convert.ToDateTime(Convert.ToString(row[PositionTradeDate])) : DateTimeConstants.MinValue;
                        DateTime closingtradeDateValue = row[ClosingTradeDate] != DBNull.Value ? Convert.ToDateTime(Convert.ToString(row[ClosingTradeDate])) : DateTimeConstants.MinValue;
                        double openaveragePriceValue = row[OpenPrice] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[OpenPrice])) : 0d;
                        double closedaveragePriceValue = row[ClosingPrice] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[ClosingPrice])) : 0d;
                        int accountIDValue = Convert.ToInt32(row[AccountID].ToString());
                        int level2IDValue = Convert.ToInt32(row[Level2ID].ToString());
                        int assetIDValue = Convert.ToInt32(row[AssetID].ToString());
                        int underLyingIDValue = Convert.ToInt32(row[UnderLyingID].ToString());
                        int exchangeIDvalue = Convert.ToInt32(row[ExchangeID].ToString());
                        int currencyIDValue = Convert.ToInt32(row[CurrencyID].ToString());
                        double positionalCommissionandFeesValue = row[PositionalTaxlotCommission] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[PositionalTaxlotCommission])) : 0d;
                        double closingCommissionandFeesValue = row[ClosingTaxlotCommission] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[ClosingTaxlotCommission])) : 0d;
                        int intclosingModeValue = row[ClosingMode] != DBNull.Value ? Convert.ToInt32(Convert.ToString(row[ClosingMode])) : 0;
                        double multiplierValue = row[Multiplier] != DBNull.Value ? Convert.ToDouble(row[Multiplier]) : 1;
                        int intPositionTagvalue = row[OpeiningPositionTag] != DBNull.Value ? Convert.ToInt32(row[OpeiningPositionTag]) : 0;
                        double intclosingtaxlottagvalue = row[ClosingPositionTag] != DBNull.Value ? Convert.ToInt32(row[ClosingPositionTag]) : 1;
                        double closeQtyValue = Convert.ToDouble(row[ClosedQty].ToString());
                        double positionalnotionalValue = row[PositionalNotionalValue] != DBNull.Value ? Convert.ToDouble(row[PositionalNotionalValue].ToString()) : 0d;
                        double positionalbenchMarkRateValue = row[PositionalBenchMarkRate] != DBNull.Value ? Convert.ToDouble(row[PositionalBenchMarkRate].ToString()) : 0d;
                        double positionaldifferentialValue = row[PositionalDifferential] != DBNull.Value ? Convert.ToDouble(row[PositionalDifferential].ToString()) : 0d;
                        double positionalorigCostBasisValue = row[PositionalOrigCostBasis] != DBNull.Value ? Convert.ToDouble(row[PositionalOrigCostBasis].ToString()) : 0d;
                        int positionaldayCountValue = row[PositionalDayCount] != DBNull.Value ? Convert.ToInt32(row[PositionalDayCount].ToString()) : 0;
                        string positionalswapDescriptionValue = row[PositionalSwapDescription] != DBNull.Value ? row[PositionalSwapDescription].ToString() : string.Empty;
                        DateTime positionalfirstResetDateValue = row[PositionalFirstResetDate] != DBNull.Value ? Convert.ToDateTime(row[PositionalFirstResetDate]) : DateTimeConstants.MinValue;
                        DateTime positionalorigTransDateValue = row[PositionalOrigTransDate] != DBNull.Value ? Convert.ToDateTime(row[PositionalOrigTransDate]) : DateTimeConstants.MinValue;
                        bool positionalisSwappedValue = row[PositionalIsSwapped] != DBNull.Value ? Convert.ToBoolean(row[PositionalIsSwapped].ToString()) : false;

                        double closingnotionalValue = row[ClosingNotionalValue] != DBNull.Value ? Convert.ToDouble(row[ClosingNotionalValue].ToString()) : 0d;
                        double closingbenchMarkRateValue = row[ClosingBenchMarkRate] != DBNull.Value ? Convert.ToDouble(row[ClosingBenchMarkRate].ToString()) : 0d;
                        double closingdifferentialValue = row[ClosingDifferential] != DBNull.Value ? Convert.ToDouble(row[ClosingDifferential].ToString()) : 0d;
                        double closingorigCostBasisValue = row[ClosingOrigCostBasis] != DBNull.Value ? Convert.ToDouble(row[ClosingOrigCostBasis].ToString()) : 0d;
                        int closingdayCountValue = row[ClosingDayCount] != DBNull.Value ? Convert.ToInt32(row[ClosingDayCount].ToString()) : 0;
                        string closingswapDescriptionValue = row[ClosingSwapDescription] != DBNull.Value ? row[ClosingSwapDescription].ToString() : string.Empty;
                        DateTime closingfirstResetDateValue = row[ClosingFirstResetDate] != DBNull.Value ? Convert.ToDateTime(row[ClosingFirstResetDate]) : DateTimeConstants.MinValue;
                        DateTime closingorigTransDateValue = row[ClosingOrigTransDate] != DBNull.Value ? Convert.ToDateTime(row[ClosingOrigTransDate]) : DateTimeConstants.MinValue;
                        bool closingisSwappedValue = row[ClosingIsSwapped] != DBNull.Value ? Convert.ToBoolean(row[ClosingIsSwapped].ToString()) : false;
                        string taxLotClosingIdValue = row[TaxLotClosingId].ToString();
                        string positionSide = row[PositionSide] != DBNull.Value ? row[PositionSide].ToString() : string.Empty;
                        int closingAlgo = row[ClosingAlgoIndex] != DBNull.Value ? Convert.ToInt32(row[ClosingAlgoIndex]) : (int)PostTradeEnums.CloseTradeAlogrithm.NONE;
                        int leadCurrencyId = row[LeadCurrencyId] != DBNull.Value ? Convert.ToInt32(row[LeadCurrencyId].ToString()) : 0;
                        int vsCurrencyId = row[VsCurrencyId] != DBNull.Value ? Convert.ToInt32(row[VsCurrencyId].ToString()) : 0;

                        Position position = new Position();

                        position.ID = positionaltaxLotIDValue;
                        position.ClosingID = closingtaxLotIDValue;
                        position.Symbol = symbolValue;
                        position.Side = TagDatabaseManager.GetInstance.GetOrderSideText(positionsideIDValue); ;
                        position.ClosingSide = TagDatabaseManager.GetInstance.GetOrderSideText(closingsideIDValue); ;
                        position.TradeDate = positiontradeDateValue;
                        position.ClosingTradeDate = closingtradeDateValue;
                        position.OpenAveragePrice = openaveragePriceValue;
                        position.ClosedAveragePrice = closedaveragePriceValue;
                        position.AccountID = accountIDValue;
                        position.AccountValue.ID = accountIDValue;
                        position.AccountValue.FullName = CachedDataManager.GetInstance.GetAccountText(accountIDValue);
                        position.StrategyID = level2IDValue;
                        position.Strategy = CachedDataManager.GetInstance.GetStrategyText(position.StrategyID);
                        position.AssetCategoryValue = (AssetCategory)assetIDValue;
                        position.UnderlyingName = CachedDataManager.GetInstance.GetUnderLyingText(underLyingIDValue);
                        position.Exchange = CachedDataManager.GetInstance.GetExchangeText(exchangeIDvalue);
                        position.CurrencyID = currencyIDValue;
                        position.Currency = CachedDataManager.GetInstance.GetCurrencyText(currencyIDValue);
                        position.PositionTotalCommissionandFees = positionalCommissionandFeesValue;
                        position.ClosingTotalCommissionandFees = closingCommissionandFeesValue;
                        position.Multiplier = multiplierValue;
                        position.IntClosingMode = intclosingModeValue;
                        position.PositionalTag = (PositionTag)intPositionTagvalue;
                        position.ClosingPositionTag = (PositionTag)intclosingtaxlottagvalue;
                        position.ClosedQty = closeQtyValue;
                        position.TaxLotClosingId = taxLotClosingIdValue;
                        position.ClosingTotalCommissionandFees = closingCommissionandFeesValue;
                        position.PositionSide = positionSide;
                        position.ClosingAlgo = closingAlgo;
                        position.LeadCurrencyID = leadCurrencyId;
                        position.VsCurrencyID = vsCurrencyId;
                        if (position.ClosingMode != Prana.BusinessObjects.AppConstants.ClosingMode.Offset)
                        {
                            position.IsExpired_Settled = true;
                        }
                        if (position.ClosingMode != Prana.BusinessObjects.AppConstants.ClosingMode.CorporateAction)
                        {
                            switch (positionsideIDValue)
                            {
                                case Prana.BusinessObjects.FIXConstants.SIDE_Buy:
                                case Prana.BusinessObjects.FIXConstants.SIDE_Buy_Open:
                                    position.PositionalTag = Prana.BusinessObjects.AppConstants.PositionTag.Long;
                                    break;

                                case Prana.BusinessObjects.FIXConstants.SIDE_Sell_Open:
                                case Prana.BusinessObjects.FIXConstants.SIDE_SellShort:
                                case Prana.BusinessObjects.FIXConstants.SIDE_Sell:
                                    position.PositionalTag = Prana.BusinessObjects.AppConstants.PositionTag.Short;
                                    break;
                            }
                            switch (closingsideIDValue)
                            {
                                case Prana.BusinessObjects.FIXConstants.SIDE_Buy:
                                case Prana.BusinessObjects.FIXConstants.SIDE_Buy_Open:
                                case Prana.BusinessObjects.FIXConstants.SIDE_Buy_Closed:
                                    position.ClosingPositionTag = Prana.BusinessObjects.AppConstants.PositionTag.Long;
                                    break;

                                case Prana.BusinessObjects.FIXConstants.SIDE_Sell_Open:
                                case Prana.BusinessObjects.FIXConstants.SIDE_SellShort:
                                case Prana.BusinessObjects.FIXConstants.SIDE_Sell:
                                    position.ClosingPositionTag = Prana.BusinessObjects.AppConstants.PositionTag.Short;
                                    break;
                            }
                        }
                        if (positionalisSwappedValue)
                        {
                            Prana.BusinessObjects.SwapParameters positionswap = new Prana.BusinessObjects.SwapParameters();
                            position.PositionSwapParameters = positionswap;
                            positionswap.NotionalValue = positionalnotionalValue;
                            positionswap.BenchMarkRate = positionalbenchMarkRateValue;
                            positionswap.Differential = positionaldifferentialValue;
                            positionswap.OrigCostBasis = positionalorigCostBasisValue;
                            positionswap.DayCount = positionaldayCountValue;
                            positionswap.SwapDescription = positionalswapDescriptionValue;
                            positionswap.FirstResetDate = positionalfirstResetDateValue;
                            positionswap.OrigTransDate = positionalorigTransDateValue;
                        }
                        position.IsPositionSwapped = positionalisSwappedValue;
                        if (closingisSwappedValue)
                        {
                            Prana.BusinessObjects.SwapParameters closingswap = new Prana.BusinessObjects.SwapParameters();
                            position.PositionSwapParameters = closingswap;
                            closingswap.NotionalValue = closingnotionalValue;
                            closingswap.BenchMarkRate = closingbenchMarkRateValue;
                            closingswap.Differential = closingdifferentialValue;
                            closingswap.OrigCostBasis = closingorigCostBasisValue;
                            closingswap.DayCount = closingdayCountValue;
                            closingswap.SwapDescription = closingswapDescriptionValue;
                            closingswap.FirstResetDate = closingfirstResetDateValue;
                            closingswap.OrigTransDate = closingorigTransDateValue;
                        }
                        position.IsClosedSwapped = closingisSwappedValue;
                        Prana.BusinessLogic.Calculations.SetAveragePriceRealizedPNL(position);
                        position.PositionState = positionState;
                        NetPositions.Add(position);
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
            return NetPositions;
        }

        private static List<Position> FillNetPositionList(DataSet ds)
        {
            List<Position> NetPositions = new List<Position>();

            int PositionaltaxLotID = 0;
            int ClosingtaxLotID = 1;
            int symbol = 2;
            int PositionSideID = 3;
            int ClosingSideID = 4;
            int PositionTradeDate = 5;
            int ClosingTradeDate = 6;
            int OpenPrice = 7;
            int ClosingPrice = 8;
            int AccountID = 9;
            int Level2ID = 10;
            int AssetID = 11;
            int UnderLyingID = 12;
            int ExchangeID = 13;
            int CurrencyID = 14;
            int PositionalTaxlotCommission = 15;
            int ClosingTaxlotCommission = 16;
            int ClosingMode = 17;
            int Multiplier = 18;
            int OpeiningPositionTag = 19;
            int ClosingPositionTag = 20;
            int ClosedQty = 21;
            int PositionalNotionalValue = 22;
            int PositionalBenchMarkRate = 23;
            int PositionalDifferential = 24;
            int PositionalOrigCostBasis = 25;
            int PositionalDayCount = 26;
            int PositionalSwapDescription = 27;
            int PositionalFirstResetDate = 28;
            int PositionalOrigTransDate = 29;
            int ClosingNotionalValue = 30;
            int ClosingBenchMarkRate = 31;
            int ClosingDifferential = 32;
            int ClosingOrigCostBasis = 33;
            int ClosingDayCount = 34;
            int ClosingSwapDescription = 35;
            int ClosingFirstResetDate = 36;
            int ClosingOrigTransDate = 37;
            int PositionalIsSwapped = 38;
            int ClosingIsSwapped = 39;
            int TaxLotClosingId = 40;
            int PositionSide = 41;
            int ClosingAlgoIndex = 42;

            try
            {
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        string positionaltaxLotIDValue = Convert.ToString(row[PositionaltaxLotID]);
                        string closingtaxLotIDValue = Convert.ToString(row[ClosingtaxLotID]);
                        string symbolValue = Convert.ToString(row[symbol]);
                        string positionsideIDValue = Convert.ToString(row[PositionSideID]).Trim();
                        string closingsideIDValue = Convert.ToString(row[ClosingSideID]).Trim();
                        DateTime positiontradeDateValue = Convert.ToDateTime(Convert.ToString(row[PositionTradeDate]));
                        DateTime closingtradeDateValue = Convert.ToDateTime(Convert.ToString(row[ClosingTradeDate]));
                        double openaveragePriceValue = Convert.ToDouble(Convert.ToString(row[OpenPrice]));
                        double closedaveragePriceValue = Convert.ToDouble(Convert.ToString(row[ClosingPrice]));
                        int accountIDValue = Convert.ToInt32(row[AccountID].ToString());
                        int level2IDValue = Convert.ToInt32(row[Level2ID].ToString());
                        int assetIDValue = Convert.ToInt32(row[AssetID].ToString());
                        int underLyingIDValue = Convert.ToInt32(row[UnderLyingID].ToString());
                        int exchangeIDvalue = Convert.ToInt32(row[ExchangeID].ToString());
                        int currencyIDValue = Convert.ToInt32(row[CurrencyID].ToString());
                        double positionalCommissionandFeesValue = row[PositionalTaxlotCommission] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[PositionalTaxlotCommission])) : 0d;
                        double closingCommissionandFeesValue = row[ClosingTaxlotCommission] != DBNull.Value ? Convert.ToDouble(Convert.ToString(row[ClosingTaxlotCommission])) : 0d;
                        int intclosingModeValue = row[ClosingMode] != DBNull.Value ? Convert.ToInt32(Convert.ToString(row[ClosingMode])) : 0;
                        double multiplierValue = row[Multiplier] != DBNull.Value ? Convert.ToDouble(row[Multiplier]) : 1;
                        int intPositionTagvalue = row[OpeiningPositionTag] != DBNull.Value ? Convert.ToInt32(row[OpeiningPositionTag]) : 0;
                        double intclosingtaxlottagvalue = row[ClosingPositionTag] != DBNull.Value ? Convert.ToInt32(row[ClosingPositionTag]) : 1;
                        double closeQtyValue = Convert.ToDouble(row[ClosedQty].ToString());
                        double positionalnotionalValue = row[PositionalNotionalValue] != DBNull.Value ? Convert.ToDouble(row[PositionalNotionalValue].ToString()) : 0d;
                        double positionalbenchMarkRateValue = row[PositionalBenchMarkRate] != DBNull.Value ? Convert.ToDouble(row[PositionalBenchMarkRate].ToString()) : 0d;
                        double positionaldifferentialValue = row[PositionalDifferential] != DBNull.Value ? Convert.ToDouble(row[PositionalDifferential].ToString()) : 0d;
                        double positionalorigCostBasisValue = row[PositionalOrigCostBasis] != DBNull.Value ? Convert.ToDouble(row[PositionalOrigCostBasis].ToString()) : 0d;
                        int positionaldayCountValue = row[PositionalDayCount] != DBNull.Value ? Convert.ToInt32(row[PositionalDayCount].ToString()) : 0;
                        string positionalswapDescriptionValue = row[PositionalSwapDescription] != DBNull.Value ? row[PositionalSwapDescription].ToString() : string.Empty;
                        DateTime positionalfirstResetDateValue = row[PositionalFirstResetDate] != DBNull.Value ? Convert.ToDateTime(row[PositionalFirstResetDate]) : DateTimeConstants.MinValue;
                        DateTime positionalorigTransDateValue = row[PositionalOrigTransDate] != DBNull.Value ? Convert.ToDateTime(row[PositionalOrigTransDate]) : DateTimeConstants.MinValue;
                        bool positionalisSwappedValue = row[PositionalIsSwapped] != DBNull.Value ? Convert.ToBoolean(row[PositionalIsSwapped].ToString()) : false;

                        double closingnotionalValue = row[ClosingNotionalValue] != DBNull.Value ? Convert.ToDouble(row[ClosingNotionalValue].ToString()) : 0d;
                        double closingbenchMarkRateValue = row[ClosingBenchMarkRate] != DBNull.Value ? Convert.ToDouble(row[ClosingBenchMarkRate].ToString()) : 0d;
                        double closingdifferentialValue = row[ClosingDifferential] != DBNull.Value ? Convert.ToDouble(row[ClosingDifferential].ToString()) : 0d;
                        double closingorigCostBasisValue = row[ClosingOrigCostBasis] != DBNull.Value ? Convert.ToDouble(row[ClosingOrigCostBasis].ToString()) : 0d;
                        int closingdayCountValue = row[ClosingDayCount] != DBNull.Value ? Convert.ToInt32(row[ClosingDayCount].ToString()) : 0;
                        string closingswapDescriptionValue = row[ClosingSwapDescription] != DBNull.Value ? row[ClosingSwapDescription].ToString() : string.Empty;
                        DateTime closingfirstResetDateValue = row[ClosingFirstResetDate] != DBNull.Value ? Convert.ToDateTime(row[ClosingFirstResetDate]) : DateTimeConstants.MinValue;
                        DateTime closingorigTransDateValue = row[ClosingOrigTransDate] != DBNull.Value ? Convert.ToDateTime(row[ClosingOrigTransDate]) : DateTimeConstants.MinValue;
                        bool closingisSwappedValue = row[ClosingIsSwapped] != DBNull.Value ? Convert.ToBoolean(row[ClosingIsSwapped].ToString()) : false;
                        string taxLotClosingIdValue = row[TaxLotClosingId].ToString();
                        string positionSide = row[PositionSide] != DBNull.Value ? row[PositionSide].ToString() : string.Empty;
                        int closingAlgo = row[ClosingAlgoIndex] != DBNull.Value ? Convert.ToInt32(row[ClosingAlgoIndex]) : (int)PostTradeEnums.CloseTradeAlogrithm.NONE;

                        Position position = new Position();

                        position.ID = positionaltaxLotIDValue;
                        position.ClosingID = closingtaxLotIDValue;
                        position.Symbol = symbolValue;
                        position.Side = TagDatabaseManager.GetInstance.GetOrderSideText(positionsideIDValue); ;
                        position.ClosingSide = TagDatabaseManager.GetInstance.GetOrderSideText(closingsideIDValue); ;
                        position.TradeDate = positiontradeDateValue;
                        position.ClosingTradeDate = closingtradeDateValue;
                        position.OpenAveragePrice = openaveragePriceValue;
                        position.ClosedAveragePrice = closedaveragePriceValue;
                        position.AccountID = accountIDValue;
                        position.AccountValue.ID = accountIDValue;
                        position.AccountValue.FullName = CachedDataManager.GetInstance.GetAccountText(accountIDValue);
                        position.StrategyID = level2IDValue;
                        position.Strategy = CachedDataManager.GetInstance.GetStrategyText(position.StrategyID);
                        position.AssetCategoryValue = (AssetCategory)assetIDValue;
                        position.UnderlyingName = CachedDataManager.GetInstance.GetUnderLyingText(underLyingIDValue);
                        position.Exchange = CachedDataManager.GetInstance.GetExchangeText(exchangeIDvalue);
                        position.CurrencyID = currencyIDValue;
                        position.Currency = CachedDataManager.GetInstance.GetCurrencyText(currencyIDValue);
                        position.PositionTotalCommissionandFees = positionalCommissionandFeesValue;
                        position.ClosingTotalCommissionandFees = closingCommissionandFeesValue;
                        position.Multiplier = multiplierValue;
                        position.IntClosingMode = intclosingModeValue;
                        position.PositionalTag = (PositionTag)intPositionTagvalue;
                        position.ClosingPositionTag = (PositionTag)intclosingtaxlottagvalue;
                        position.ClosedQty = closeQtyValue;
                        position.TaxLotClosingId = taxLotClosingIdValue;
                        position.ClosingTotalCommissionandFees = closingCommissionandFeesValue;
                        position.PositionSide = positionSide;
                        position.ClosingAlgo = closingAlgo;
                        if (position.ClosingMode != Prana.BusinessObjects.AppConstants.ClosingMode.Offset)
                        {
                            position.IsExpired_Settled = true;
                        }
                        if (position.ClosingMode != Prana.BusinessObjects.AppConstants.ClosingMode.CorporateAction)
                        {
                            switch (positionsideIDValue)
                            {
                                case Prana.BusinessObjects.FIXConstants.SIDE_Buy:
                                case Prana.BusinessObjects.FIXConstants.SIDE_Buy_Open:
                                    position.PositionalTag = Prana.BusinessObjects.AppConstants.PositionTag.Long;
                                    break;

                                case Prana.BusinessObjects.FIXConstants.SIDE_Sell_Open:
                                case Prana.BusinessObjects.FIXConstants.SIDE_SellShort:
                                case Prana.BusinessObjects.FIXConstants.SIDE_Sell:
                                    position.PositionalTag = Prana.BusinessObjects.AppConstants.PositionTag.Short;
                                    break;
                            }
                            switch (closingsideIDValue)
                            {
                                case Prana.BusinessObjects.FIXConstants.SIDE_Buy:
                                case Prana.BusinessObjects.FIXConstants.SIDE_Buy_Open:
                                case Prana.BusinessObjects.FIXConstants.SIDE_Buy_Closed:
                                    position.ClosingPositionTag = Prana.BusinessObjects.AppConstants.PositionTag.Long;
                                    break;

                                case Prana.BusinessObjects.FIXConstants.SIDE_Sell_Open:
                                case Prana.BusinessObjects.FIXConstants.SIDE_SellShort:
                                case Prana.BusinessObjects.FIXConstants.SIDE_Sell:
                                    position.ClosingPositionTag = Prana.BusinessObjects.AppConstants.PositionTag.Short;
                                    break;
                            }
                        }
                        if (positionalisSwappedValue)
                        {
                            Prana.BusinessObjects.SwapParameters positionswap = new Prana.BusinessObjects.SwapParameters();
                            position.PositionSwapParameters = positionswap;
                            positionswap.NotionalValue = positionalnotionalValue;
                            positionswap.BenchMarkRate = positionalbenchMarkRateValue;
                            positionswap.Differential = positionaldifferentialValue;
                            positionswap.OrigCostBasis = positionalorigCostBasisValue;
                            positionswap.DayCount = positionaldayCountValue;
                            positionswap.SwapDescription = positionalswapDescriptionValue;
                            positionswap.FirstResetDate = positionalfirstResetDateValue;
                            positionswap.OrigTransDate = positionalorigTransDateValue;
                        }
                        position.IsPositionSwapped = positionalisSwappedValue;
                        if (closingisSwappedValue)
                        {
                            Prana.BusinessObjects.SwapParameters closingswap = new Prana.BusinessObjects.SwapParameters();
                            position.PositionSwapParameters = closingswap;
                            closingswap.NotionalValue = closingnotionalValue;
                            closingswap.BenchMarkRate = closingbenchMarkRateValue;
                            closingswap.Differential = closingdifferentialValue;
                            closingswap.OrigCostBasis = closingorigCostBasisValue;
                            closingswap.DayCount = closingdayCountValue;
                            closingswap.SwapDescription = closingswapDescriptionValue;
                            closingswap.FirstResetDate = closingfirstResetDateValue;
                            closingswap.OrigTransDate = closingorigTransDateValue;
                        }
                        position.IsClosedSwapped = closingisSwappedValue;
                        Prana.BusinessLogic.Calculations.SetAveragePriceRealizedPNL(position);
                        NetPositions.Add(position);
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
            return NetPositions;
        }
        #endregion

        //Module: Close Trade/PM
        /// <summary>
        /// Saves the close trades data.
        /// </summary>
        /// <param name="netPositionList">The net position list.</param>
        /// <param name="thirdPartyID">The data source ID.</param>
        /// <param name="closeTradePreferences">The close trade preferences.</param>
        /// <param name="isAccountBasedData">if set to <c>true</c> [is account based data].</param>
        /// <returns></returns>
        public static int SaveCloseTradesData(List<Position> netPositionList, List<TaxLot> unSaveAllocatedTadeList)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                if (netPositionList.Count == 0 || unSaveAllocatedTadeList.Count != 2 * (netPositionList.Count))
                {
                    return 0;
                }

                int count = 0;

                using (SqlConnection conn = (SqlConnection)DatabaseManager.DatabaseManager.CreateConnection())
                {
                    conn.Open();
                    int chunksize = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("ChunkSizeForClosingPosition"));
                    List<List<TaxLot>> taxlotChunks = ChunkingManager.CreateChunksForClosing(unSaveAllocatedTadeList, chunksize);
                    List<List<Position>> positionChunks = ChunkingManager.CreateChunksForClosing(netPositionList, chunksize / 2);

                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "P_SaveClosingTaxlots";

                    for (var index = 0; index < taxlotChunks.Count; index++)
                    {
                        var taxlots = taxlotChunks[index];//list<taxlot>
                        var positions = positionChunks[index];//list<position>
                        using (SqlTransaction transaction = DatabaseManager.DatabaseManager.BeginTransaction(conn))
                        {
                            using (SqlBulkCopy copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, transaction))
                            {
                                try
                                {
                                    copy.BatchSize = chunksize;
                                    copy.BulkCopyTimeout = _heavySaveTimeout;
                                    copy.ColumnMappings.Add("TaxLotID", "TaxLotID");
                                    copy.ColumnMappings.Add("Symbol", "Symbol");
                                    copy.ColumnMappings.Add("TaxLotQty", "TaxLotOpenQty");
                                    copy.ColumnMappings.Add("AvgPrice", "AvgPrice");
                                    copy.ColumnMappings.Add("TimeOfSaveUTC", "TimeOfSaveUTC");
                                    copy.ColumnMappings.Add("GroupID", "GroupID");
                                    copy.ColumnMappings.Add("AUECModifiedDate", "AUECModifiedDate");
                                    copy.ColumnMappings.Add("Level1ID", "FundID");
                                    copy.ColumnMappings.Add("Level2ID", "Level2ID");
                                    copy.ColumnMappings.Add("OpenTotalCommissionandFees", "OpenTotalCommissionandFees");
                                    copy.ColumnMappings.Add("ClosedTotalCommissionandFees", "ClosedTotalCommissionandFees");
                                    copy.ColumnMappings.Add("PositionTag", "PositionTag");
                                    copy.ColumnMappings.Add("OrderSideTagValue", "OrderSideTagValue");
                                    copy.ColumnMappings.Add("TaxLotClosingId", "TaxLotClosingId_Fk");
                                    //Save LotID and ExternalTransID in closing taxlot also
                                    copy.ColumnMappings.Add("LotId", "LotId");
                                    copy.ColumnMappings.Add("ExternalTransId", "ExternalTransId");
                                    copy.ColumnMappings.Add("TradeAttribute1", "TradeAttribute1");
                                    copy.ColumnMappings.Add("TradeAttribute2", "TradeAttribute2");
                                    copy.ColumnMappings.Add("TradeAttribute3", "TradeAttribute3");
                                    copy.ColumnMappings.Add("TradeAttribute4", "TradeAttribute4");
                                    copy.ColumnMappings.Add("TradeAttribute5", "TradeAttribute5");
                                    copy.ColumnMappings.Add("TradeAttribute6", "TradeAttribute6");
                                    copy.ColumnMappings.Add("SettlementCurrencyID", "SettlCurrency");
                                    copy.ColumnMappings.Add("NotionalChange", "NotionalChange");
                                    copy.ColumnMappings.Add("FXRate", "FXRate");
                                    copy.ColumnMappings.Add("FXConversionMethodOperator", "FXConversionMethodOperator");
                                    copy.ColumnMappings.Add("NirvanaProcessDate", "NirvanaProcessDate");
                                    copy.ColumnMappings.Add("AdditionalTradeAttributes", "AdditionalTradeAttributes");
                                    copy.DestinationTableName = "PM_Taxlots_Intermediate";
                                    ds = GeneralUtilities.CreateTableStructureFromObject(unSaveAllocatedTadeList);
                                    ds.Tables[0].Columns["TaxLotClosingId"].DataType = typeof(Guid);
                                    ds.Tables[0].Columns["PositionTag"].DataType = typeof(Int32);
                                    GeneralUtilities.FillDataSetFromCollection(taxlots, ref ds, false, false);
                                    copy.WriteToServer(ds.Tables[0]);
                                    ds.Tables[0].Clear();
                                    copy.ColumnMappings.Clear();
                                    ds = null;

                                    var result = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, string.Empty, transaction);
                                    if (result > 0)
                                    {
                                        copy.BatchSize = chunksize / 2;
                                        copy.BulkCopyTimeout = _heavySaveTimeout;
                                        copy.ColumnMappings.Add("TaxLotClosingId", "TaxLotClosingID");
                                        copy.ColumnMappings.Add("ID", "PositionalTaxlotId");
                                        copy.ColumnMappings.Add("ClosingID", "ClosingTaxlotId");
                                        copy.ColumnMappings.Add("ClosedQty", "ClosedQty");
                                        copy.ColumnMappings.Add("IntClosingMode", "ClosingMode");
                                        copy.ColumnMappings.Add("TimeOfSaveUTC", "TimeOfSaveUTC");
                                        copy.ColumnMappings.Add("ClosingTradeDate", "AUECLocalDate");
                                        copy.ColumnMappings.Add("PositionSide", "PositionSide");
                                        copy.ColumnMappings.Add("OpenAveragePrice", "OpenPrice");
                                        copy.ColumnMappings.Add("ClosedAveragePrice", "ClosePrice");
                                        copy.ColumnMappings.Add("ClosingAlgo", "ClosingAlgo");
                                        copy.ColumnMappings.Add("NotionalChange", "NotionalChange");
                                        copy.ColumnMappings.Add("IsManualyExerciseAssign", "IsManualyExerciseAssign");
                                        copy.ColumnMappings.Add("IsCopyTradeAttrbsPrefUsed", "IsCopyTradeAttrbsPrefUsed");
                                        copy.DestinationTableName = "PM_TaxlotClosing";
                                        ds = GeneralUtilities.CreateTableStructureFromObject(netPositionList);
                                        ds.Tables[0].Columns["TaxLotClosingID"].DataType = typeof(Guid);
                                        GeneralUtilities.FillDataSetFromCollection(positions, ref ds, false, false);
                                        copy.WriteToServer(ds.Tables[0]);
                                        ds.Tables[0].Clear();
                                        ds = null;
                                        copy.ColumnMappings.Clear();
                                    }
                                    //if (index == 1)
                                    //{
                                    //    throw new NullReferenceException();
                                    //}
                                    transaction.Commit();
                                    count = count + positions.Count;
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    rowsAffected = count;
                                    throw new Exception("Not able to close data, failed while saving data in PM_Taxlots_Intermediate table or PM_TaxlotClosing , Please check", ex);
                                }
                                finally
                                {
                                    using (SqlConnection conn1 = (SqlConnection)DatabaseManager.DatabaseManager.CreateConnection())
                                    {
                                        conn1.Open();
                                        var sql = "delete from PM_Taxlots_Intermediate";
                                        SqlCommand cmd = new SqlCommand(sql, conn1);
                                        cmd.CommandTimeout = _heavySaveTimeout;
                                        cmd.ExecuteScalar();
                                    }
                                }
                            }
                        }

                    }
                }
                rowsAffected = count;
                CheckForCorruptedClosingData(netPositionList, unSaveAllocatedTadeList);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rowsAffected == 0)
                {
                    throw;
                }
                else
                {
                    return rowsAffected;
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// To check if closing data is corrupted due to unmatched position and taxlot list
        /// </summary>
        /// <param name="netPositionList"></param>
        /// <param name="unSaveAllocatedTadeList"></param>
        private static void CheckForCorruptedClosingData(List<Position> netPositionList, List<TaxLot> unSaveAllocatedTadeList)
        {
            try
            {
                bool isCorruptedTaxlotsInDB = false;
                bool isCollectionCorrupted = false;
                List<string> fundIds = new List<string>();
                DataSet dsPositonData = new DataSet();
                DataSet dsTaxlotData = new DataSet();

                if (unSaveAllocatedTadeList.Count != 2 * (netPositionList.Count))
                {
                    isCollectionCorrupted = true;
                    fundIds.Add("SelectFunds");
                }
                else
                {
                    // Get corrupted closing positions on minimum TimeOfSave
                    DateTime minDate = netPositionList.Min(x => x.TimeOfSaveUTC);
                    DateTime.TryParse(minDate.ToString("yyyy-MM-dd HH:mm:ss.000"), out minDate);
                    fundIds = GetCorruptedClosingTaxlots(minDate);
                    if (fundIds.Count > 0)
                        isCorruptedTaxlotsInDB = true;
                }
                if (isCollectionCorrupted || isCorruptedTaxlotsInDB)
                {
                    Guid fileID1 = Guid.NewGuid();
                    Guid fileID2 = Guid.NewGuid();

                    // Dump netPosition data in case if data is corrupted
                    dsPositonData = GeneralUtilities.CreateTableStructureFromObject(netPositionList);
                    GeneralUtilities.FillDataSetFromCollection(netPositionList, ref dsPositonData, false, false);

                    // Dump unSaveAllocatedTade data in case if data is corrupted
                    dsTaxlotData = GeneralUtilities.CreateTableStructureFromObject(unSaveAllocatedTadeList);
                    GeneralUtilities.FillDataSetFromCollection(unSaveAllocatedTadeList, ref dsTaxlotData, false, false);

                    if (dsPositonData.Tables.Count > 0)
                    {
                        DumpClosingData(dsPositonData.Tables[0], fileID1);
                    }
                    if (dsTaxlotData.Tables.Count > 0)
                    {
                        DumpClosingData(dsTaxlotData.Tables[0], fileID2);
                    }
                    StringBuilder msg = new StringBuilder();
                    if (isCollectionCorrupted)
                    {
                        msg.Append("Closing collection is corrupted.");
                    }
                    else
                    {
                        msg.Append("Closing data corrupted while saving data in database.");
                    }
                    SendMailForCorruptedData(fileID1.ToString(), fileID2.ToString(), msg.ToString(), fundIds);
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
        /// Dump closed taxlot data
        /// </summary>
        /// <param name="dtClosingData"></param>
        private static void DumpClosingData(DataTable dtClosingData, Guid fileID)
        {
            try
            {
                if (dtClosingData.Rows.Count > 0)
                {
                    string dumpedClosingDirectoryPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Closing";
                    string currentDate = DateTime.Today.ToString("dd-MM-yyyy");
                    string dumpedClosingFilePath = dumpedClosingDirectoryPath + @"\" + currentDate + "_" + fileID.ToString() + ".xml";

                    if (!Directory.Exists(dumpedClosingDirectoryPath))
                    {
                        Directory.CreateDirectory(dumpedClosingDirectoryPath);
                    }
                    if (File.Exists(dumpedClosingDirectoryPath))
                    {
                        DataSet ds = new DataSet(); ;
                        ds = XMLUtilities.ReadXmlUsingBufferedStream(dumpedClosingDirectoryPath);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            dtClosingData.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                        }
                    }
                    using (XmlTextWriter xmlWriter = new XmlTextWriter(dumpedClosingFilePath, Encoding.UTF8))
                    {
                        dtClosingData.WriteXml(xmlWriter);
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
        /// To send mail to recipient when corrupted closing data found
        /// </summary>
        /// <param name="fileID"></param>
        private static void SendMailForCorruptedData(string fileID1, string fileID2, string msg, List<string> fundNames)
        {
            try
            {
                string mailSubject = "Closing data corrupted.";
                StringBuilder mailBody = new StringBuilder();
                StringBuilder serverLogging = new StringBuilder();
                string currentDate = DateTime.Today.ToString("dd-MM-yyyy");
                string closingDirectoryPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Closing";

                string server = GeneralUtilities.GetIPAddress();
                string PM_Taxlots_FilePath = closingDirectoryPath + @"\" + currentDate + "_" + fileID1 + ".xml";
                string PM_TaxlotClosing_FilePath = closingDirectoryPath + @"\" + currentDate + "_" + fileID2 + ".xml";
                mailBody.AppendLine("Closing data corrupted on server " + server + "." + Environment.NewLine + "Reason: " + msg + Environment.NewLine + "Please check the files " + PM_Taxlots_FilePath + " and " + PM_TaxlotClosing_FilePath);
                serverLogging.AppendLine(msg + " Please check the files " + PM_Taxlots_FilePath + " and " + PM_TaxlotClosing_FilePath);

                bool isCorruptedClosing = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsCheckCorruptedClosing"));
                if (isCorruptedClosing)
                {
                    int emailInterval = CommonDataCache.CachedDataManager.GetInstance.EmailIntervalForStuckTrades;
                    string[] mailRecipients = ConfigurationManager.AppSettings["MailReceiverAddressForClosing"].Split(',');
                    string mailSender = ConfigurationManager.AppSettings["MailSenderAddress"];
                    string mailSenderName = ConfigurationManager.AppSettings["MailSenderName"];
                    string mailerPassword = ConfigurationManager.AppSettings["MailSenderPassword"];
                    int mailPort = int.Parse(ConfigurationManager.AppSettings["MailPort"]);
                    bool enableSSL = bool.Parse(ConfigurationManager.AppSettings["EnableSSL"]);
                    string mailHost = ConfigurationManager.AppSettings["MailServer"];
                    string mailFooter = ConfigurationManager.AppSettings["MailFooter"];
                    bool authenticationRequired = bool.Parse(ConfigurationManager.AppSettings["AuthenticationRequired"]);
                    string mailHeader = mailSubject;

                    EmailsHelper.MailSend(mailSubject + " " + DateTime.Now.ToString(), mailBody.ToString(), mailSender, mailSenderName, mailerPassword, mailRecipients, mailPort, mailHost, enableSSL, authenticationRequired);
                }

                Logger.HandleException(new Exception(serverLogging.ToString()), LoggingConstants.POLICY_LOGANDSHOW);

                MessageData e = new MessageData();
                e.EventData = fundNames;
                e.TopicName = Topics.Topic_ClosingCorrupted;
                CentralizePublish(e);
                //CentralizePublish(fundName, Topics.Topic_ClosingCorrupted);
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

        //Module: Close Trade/PM
        /// <summary>
        /// Saves the account wise daily PNL.
        /// </summary>
        /// <param name="netPositionList"></param>
        /// <returns></returns>
        public static int SaveAccountDailyPNL(int companyID)
        {
            int rowsAffected = 0;

            try
            {
                DateTime toDay = DateTime.Now.ToUniversalTime();
                object[] parameter = new object[3];
                string AllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(toDay);
                parameter[0] = AllAUECDatesString;
                parameter[1] = DateTime.Now.ToUniversalTime();
                parameter[2] = companyID;
                rowsAffected = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteScalar("PMSaveFundDailyPNLForDate", parameter));
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
            return rowsAffected;
        }

        //Module: Close Trade/PM
        /// <summary>
        /// Saves strategy wise PNL
        /// </summary>
        /// <param name="netPositionList"></param>
        /// <returns></returns>
        public static int SaveStrategyDailyPNL(int companyID)
        {
            int rowsAffected = 0;

            try
            {
                DateTime toDay = DateTime.Now.ToUniversalTime();
                object[] parameter = new object[3];
                string AllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(toDay);
                parameter[0] = AllAUECDatesString;
                parameter[1] = DateTime.Now.ToUniversalTime();
                parameter[2] = companyID;
                rowsAffected = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteScalar("PMSaveStrategyDailyPNLForDate", parameter));
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
            return rowsAffected;
        }

        #region OBSOLETE_2008_02_25
        //Bhupesh
        //Not used for now. As this functionality is used to generate DailyEquityValueReport which is not asked for in the Yunzei release.
        /// <summary>
        /// Saves the day's current equity value
        /// </summary>
        /// <param name="netPositionList"></param>
        /// <returns></returns>
        public static int SaveAccountEquityValue(int companyID)
        {
            int rowsAffected = 0;

            try
            {
                object[] parameter = new object[2];
                parameter[0] = DateTime.Now.ToUniversalTime();
                parameter[1] = companyID;
                rowsAffected = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteScalar("PMSaveFundEquityValueForDate", parameter));
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
            return rowsAffected;
        }
        #endregion

        private static List<string> GetGroupIDs(DataSet ds)
        {
            List<string> lstGroupID = new List<string>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    int groupID = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!dr[groupID].Equals(System.DBNull.Value))
                        {
                            string GroupID = dr[groupID].ToString();
                            lstGroupID.Add(GroupID);
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
            return lstGroupID;
        }

        /// <summary>
        /// Get taxlot closing ids
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="lstTaxLotClosingIds"></param>
        private static List<string> GetTaxlotClosingID(DataTable dt)
        {
            List<string> lstTaxLotClosingIds = new List<string>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (!dr[0].Equals(System.DBNull.Value))
                    {
                        string TaxLotClosingId = dr[0].ToString();
                        lstTaxLotClosingIds.Add(TaxLotClosingId);
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
            return lstTaxLotClosingIds;
        }

        public static List<string> UnWindClosingForTaxlots(string ClosingTaxlotID, bool isBasedOnTemplates)
        {
            List<string> lstGroupID = new List<string>();
            List<string> fundIds = new List<string>();
            bool isCorruptedTaxlotsInDB = false;
            ///Need to delete all positions which has closed this supplied taxlotid, i.e. consider it as fromtaxlotid and delete till a date.
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMDeleteFundClosingForTaxlots";
                queryData.CommandTimeout = 20000;
                queryData.DictionaryDatabaseParameter.Add("@TaxLotClosingIDString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@TaxLotClosingIDString",
                    ParameterType = DbType.String,
                    ParameterValue = ClosingTaxlotID
                });
                queryData.DictionaryDatabaseParameter.Add("@skipPM_TaxlotsUpdate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@skipPM_TaxlotsUpdate",
                    ParameterType = DbType.Boolean,
                    ParameterValue = isBasedOnTemplates
                });

                using (DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData))
                {
                    if (ds.Tables.Count > 0)
                    {
                        lstGroupID = GetGroupIDs(ds);
                    }
                }

                DateTime minDate = Convert.ToDateTime(ConfigurationHelper.Instance.GetAppSettingValueByKey("AsOfDateClosingCorruption"));
                DateTime.TryParse(minDate.ToString("yyyy-MM-dd HH:mm:ss.000"), out minDate);
                fundIds = GetCorruptedClosingTaxlots(minDate);
                if (fundIds.Count > 0)
                    isCorruptedTaxlotsInDB = true;

                if (isCorruptedTaxlotsInDB)
                {
                    Logger.HandleException(new Exception("Closing Data is corrupted."), LoggingConstants.POLICY_LOGANDSHOW);
                    MessageData e = new MessageData();
                    e.EventData = fundIds;
                    e.TopicName = Topics.Topic_ClosingCorrupted;
                    CentralizePublish(e);
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
            return lstGroupID;
        }

        /// <summary>
        /// this function checks if accountwise symbol is closed or corporate action is applied in future date
        /// symbol and account is fetched on the basis of taxlotID
        /// </summary>
        /// <param name="taxlotClosingIDWithClosingDate"></param>
        /// <returns></returns>
        public static Dictionary<string, StatusInfo> CheckClosingForFutureDate(string taxlotClosingIDWithClosingDate)
        {
            Dictionary<string, StatusInfo> taxlotsClosedOrCAAppliedinFutureDate = new Dictionary<string, StatusInfo>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMCheckTaxlotsClosedOrCAAppliedInFutureDate";
                queryData.CommandTimeout = 20000;
                queryData.DictionaryDatabaseParameter.Add("@taxlotClosingIDs", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@taxlotClosingIDs",
                    ParameterType = DbType.String,
                    ParameterValue = taxlotClosingIDWithClosingDate.ToString()
                });

                using (DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData))
                {
                    if (ds.Tables.Count > 0)
                    {
                        taxlotsClosedOrCAAppliedinFutureDate = GetSymbolAndAccountClosedOrCAAppliedInFutureDate(ds);
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
            return taxlotsClosedOrCAAppliedinFutureDate;
        }

        /// <summary>
        /// this funcation fills the dictionary to show info on UI
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private static Dictionary<string, StatusInfo> GetSymbolAndAccountClosedOrCAAppliedInFutureDate(DataSet ds)
        {
            Dictionary<string, StatusInfo> taxlotsClosedOrCAAppliedinFutureDate = new Dictionary<string, StatusInfo>();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string taxlotID = string.Empty;
                        if (!dr["TaxlotID"].Equals(System.DBNull.Value))
                        {
                            taxlotID = dr["TaxlotID"].ToString();
                        }

                        string symbol = string.Empty;
                        if (!dr["Symbol"].Equals(System.DBNull.Value))
                        {
                            symbol = dr["Symbol"].ToString();
                        }

                        string accountName = string.Empty;
                        if (!dr["FundName"].Equals(System.DBNull.Value))
                        {
                            accountName = dr["FundName"].ToString();
                        }

                        string auecModifiedDate = string.Empty;
                        if (!dr["AUECModifiedDate"].Equals(System.DBNull.Value))
                        {
                            auecModifiedDate = dr["AUECModifiedDate"].ToString();
                        }

                        int closingMode = 0;
                        if (!dr["ClosingMode"].Equals(System.DBNull.Value))
                        {
                            closingMode = Convert.ToInt16(dr["ClosingMode"].ToString());
                        }
                        if (!taxlotsClosedOrCAAppliedinFutureDate.ContainsKey(taxlotID))
                        {
                            StatusInfo info = new StatusInfo();
                            if (closingMode.Equals((int)ClosingMode.CorporateAction))
                            {
                                info.Status = PostTradeEnums.Status.CorporateAction;
                            }
                            else
                            {
                                info.Status = PostTradeEnums.Status.Closed;
                            }
                            info.Details = "Symbol: " + symbol + ", Account: " + accountName + ", Date: " + auecModifiedDate;
                            taxlotsClosedOrCAAppliedinFutureDate.Add(taxlotID, info);
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
            return taxlotsClosedOrCAAppliedinFutureDate;
        }

        public static void Removeposition(Position position)
        {
            try
            {
                object[] parameter = new object[1];
                parameter[0] = position.ID;
                DatabaseManager.DatabaseManager.ExecuteScalar("P_Removeposition", parameter);
            }
            #region Catch
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
            #endregion
        }

        public static void DeletePhysicalGeneratedTaxlots(string TaxlotID, string TaxlotClosingID)
        {
            ///Need to delete all positions which has closed this supplied taxlotid, i.e. consider it as fromtaxlotid and delete till a date.

            int deleted = int.MinValue;
            object[] parameter = new object[2];
            try
            {
                parameter[0] = TaxlotID;
                parameter[1] = TaxlotClosingID;
                deleted = int.Parse(DatabaseManager.DatabaseManager.ExecuteNonQuery("PMDeletePhysicalGeneratedTaxlots", parameter).ToString());
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

        internal static void UndoSplitTaxlot(string taxlotID)
        {
            int rowsAffected = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMUndoSplit";
                queryData.DictionaryDatabaseParameter.Add("@RowTaxlotID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@RowTaxlotID",
                    ParameterType = DbType.String,
                    ParameterValue = taxlotID
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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

        internal static List<string> GetIDsToUnwind(Dictionary<string, DateTime> dictSymbols)
        {
            List<string> listUnwindIds = new List<string>();
            List<string> lsttaxlotID = new List<string>();
            List<string> lsttaxlotClosingID = new List<string>();
            StringBuilder TaxlotClosingID = new StringBuilder();
            StringBuilder taxlotIds = new StringBuilder();
            DataSet ds = null;

            string SymbolsXml = string.Empty;
            List<DateSymbol> listSymbols = new List<DateSymbol>();
            try
            {
                foreach (KeyValuePair<string, DateTime> kp in dictSymbols)
                {
                    DateSymbol datesymbol = new DateSymbol();
                    datesymbol.Symbol = kp.Key;
                    datesymbol.FromDate = kp.Value;
                    listSymbols.Add(datesymbol);
                }
                if (listSymbols.Count > 0)
                {
                    SymbolsXml = XMLUtilities.SerializeToXML(listSymbols);

                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "P_DeleteClosingForSymbolAndDate";
                    queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@Xml",
                        ParameterType = DbType.String,
                        ParameterValue = SymbolsXml
                    });

                    ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    int taxlotClosingIDindex = 0;
                    int taxlotIDindex = 1;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!dr[taxlotClosingIDindex].Equals(System.DBNull.Value))
                        {
                            string taxlotClosingID = Convert.ToString(dr[taxlotClosingIDindex]);
                            if (!lsttaxlotClosingID.Contains(taxlotClosingID))
                            {
                                lsttaxlotClosingID.Add(taxlotClosingID);
                                TaxlotClosingID.Append(taxlotClosingID.ToString());
                                TaxlotClosingID.Append(",");
                            }
                        }
                        if (!dr[taxlotIDindex].Equals(System.DBNull.Value))
                        {
                            string taxlotID = dr[taxlotIDindex].ToString();
                            if (!lsttaxlotID.Contains(taxlotID))
                            {
                                lsttaxlotID.Add(taxlotID);
                                taxlotIds.Append(taxlotID);
                                taxlotIds.Append(",");
                            }
                        }
                    }
                    listUnwindIds.Add(TaxlotClosingID.ToString());
                    listUnwindIds.Add(taxlotIds.ToString());
                }
                else if(ds == null)
                {
                    throw new ArgumentNullException(nameof(ds));
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
            return listUnwindIds;
        }

        internal static List<TaxLot> GetPosition(string taxlotIDList)
        {
            List<TaxLot> taxlots = new List<TaxLot>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetPositionForTaxlot";
                queryData.CommandTimeout = 20000;
                queryData.DictionaryDatabaseParameter.Add("@taxlotID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@taxlotID",
                    ParameterType = DbType.String,
                    ParameterValue = taxlotIDList
                });

                using (DataSet productsDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData))
                {
                    taxlots = FillOpenPositions(productsDataSet);
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
                return null;
            }
            return taxlots;
        }

        /// <summary>
        /// Get total PNL for a symbol between a date range
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="symbol"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        internal static void GetPNLForSymbol(string accountName, string symbol, DateTime startDate, DateTime endDate, out Double accountRealizedPNL, out Double accountUnRealizedPNL, out Double SymbolRealizedPNL, out Double SymbolUnRealizedPNL)
        {
            accountRealizedPNL = 0;
            accountUnRealizedPNL = 0;
            SymbolRealizedPNL = 0;
            SymbolUnRealizedPNL = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetTotalPNLForFundAndSymbol";
                queryData.DictionaryDatabaseParameter.Add("@StartDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@StartDate",
                    ParameterType = DbType.Date,
                    ParameterValue = startDate
                });
                queryData.DictionaryDatabaseParameter.Add("@EndDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@EndDate",
                    ParameterType = DbType.Date,
                    ParameterValue = endDate
                });
                queryData.DictionaryDatabaseParameter.Add("@Symbol", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Symbol",
                    ParameterType = DbType.String,
                    ParameterValue = symbol
                });
                queryData.DictionaryDatabaseParameter.Add("@Fund", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Fund",
                    ParameterType = DbType.String,
                    ParameterValue = accountName
                });

                queryData.DictionaryDatabaseParameter.Add("@FundRealizedPNL", new DatabaseParameter()
                {
                    IsOutParameter = true,
                    ParameterName = "@FundRealizedPNL",
                    ParameterType = DbType.Double,
                    ParameterValue = 8,
                    OutParameterSize = sizeof(double)
                });
                queryData.DictionaryDatabaseParameter.Add("@FundUNRealizedPNL", new DatabaseParameter()
                {
                    IsOutParameter = true,
                    ParameterName = "@FundUNRealizedPNL",
                    ParameterType = DbType.Double,
                    ParameterValue = 8,
                    OutParameterSize = sizeof(double)
                });
                queryData.DictionaryDatabaseParameter.Add("@SymbolRealizedPNL", new DatabaseParameter()
                {
                    IsOutParameter = true,
                    ParameterName = "@SymbolRealizedPNL",
                    ParameterType = DbType.Double,
                    ParameterValue = 8,
                    OutParameterSize = sizeof(double)
                });
                queryData.DictionaryDatabaseParameter.Add("@SymbolUNRealizedPNL", new DatabaseParameter()
                {
                    IsOutParameter = true,
                    ParameterName = "@SymbolUNRealizedPNL",
                    ParameterType = DbType.Double,
                    ParameterValue = 8,
                    OutParameterSize = sizeof(double)
                });

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                if (queryData.DictionaryDatabaseParameter["@FundRealizedPNL"].ParameterValue != DBNull.Value)
                {
                    accountRealizedPNL = Convert.ToDouble(queryData.DictionaryDatabaseParameter["@FundRealizedPNL"].ParameterValue);
                }
                if (queryData.DictionaryDatabaseParameter["@FundUNRealizedPNL"].ParameterValue != DBNull.Value)
                {
                    accountUnRealizedPNL = Convert.ToDouble(queryData.DictionaryDatabaseParameter["@FundUNRealizedPNL"].ParameterValue);
                }
                if (queryData.DictionaryDatabaseParameter["@SymbolRealizedPNL"].ParameterValue != DBNull.Value)
                {
                    SymbolRealizedPNL = Convert.ToDouble(queryData.DictionaryDatabaseParameter["@SymbolRealizedPNL"].ParameterValue);
                }
                if (queryData.DictionaryDatabaseParameter["@SymbolUNRealizedPNL"].ParameterValue != DBNull.Value)
                {
                    SymbolUnRealizedPNL = Convert.ToDouble(queryData.DictionaryDatabaseParameter["@SymbolUNRealizedPNL"].ParameterValue);
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

        internal static DataSet FetchOpenPositionsPostVirtualUnwinding(string ClosingTaxlotID)
        {
            DataSet ds = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_VirtualUnwindAndFetchPositioinsForASymbol";
                queryData.CommandTimeout = 20000;
                queryData.DictionaryDatabaseParameter.Add("@TaxLotClosingIDString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@TaxLotClosingIDString",
                    ParameterType = DbType.String,
                    ParameterValue = ClosingTaxlotID
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return ds;
        }

        /// <summary>
        /// returns corrupted or missing taxlot entries while closing
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetCorruptedClosingTaxlots(DateTime StartTime)
        {
            //bool isCorruptedTaxlots = false;
            List<string> fundIds = new List<string>();
            DataSet dsCorruptedTaxlot = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCorruptedClosingData";
                queryData.DictionaryDatabaseParameter.Add("@StartTime", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@StartTime",
                    ParameterType = DbType.Date,
                    ParameterValue = StartTime.Date
                });

                dsCorruptedTaxlot = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (dsCorruptedTaxlot.Tables.Count > 0)
                {
                    if (dsCorruptedTaxlot.Tables[0].Rows.Count > 0)
                    {
                        //isCorruptedTaxlots = true;                       
                        fundIds = WriteDataTableToFile(dsCorruptedTaxlot.Tables[0]);
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
            //return isCorruptedTaxlots;
            return fundIds;
        }

        public static List<string> WriteDataTableToFile(DataTable submittedDataTable)
        {
            List<string> fundIds = new List<string>();
            try
            {
                var result = new StringBuilder();
                result.Append(DateTime.Now.Year.ToString() + '/' + DateTime.Now.Month.ToString() + '/' + DateTime.Now.Day.ToString() + ' ' + DateTime.Now.Hour.ToString() + ':' + DateTime.Now.Minute.ToString() + ':' + DateTime.Now.Second.ToString() + ':' + DateTime.Now.Millisecond.ToString());
                result.Append("\n");
                foreach (DataRow row in submittedDataTable.Rows)
                {
                    for (int i = 0; i < submittedDataTable.Columns.Count; i++)
                    {
                        result.Append(row[i].ToString());
                        result.Append(i == submittedDataTable.Columns.Count - 1 ? "\n" : ",");
                    }
                    if (!fundIds.Contains(Convert.ToString(row["FundID"])))
                        fundIds.Add(Convert.ToString(row["FundID"]));
                }

                //string timeToAppend = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                //string filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Log\\" + "CorruptedClosing_" + timeToAppend + ".txt";

                //StreamWriter objWriter = new StreamWriter(filePath, false);
                //objWriter.WriteLine(result.ToString());
                //objWriter.Close();
                Logger.LoggerWrite(result, LoggingConstants.CATEGORY_INFORMATION_REPORTER_CLOSINGCORRUPTION);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return fundIds;
        }

        static ProxyBase<IPublishing> _proxy;
        private static void CreatePublishingProxy()
        {
            try
            {
                if (_proxy == null)
                {
                    _proxy = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy", ex), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static readonly object _publishLock = new object();
        private static void CentralizePublish(MessageData msgData)
        {
            try
            {
                lock (_publishLock)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            _proxy.InnerChannel.Publish(msgData, msgData.TopicName);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
                    });
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

        /// <summary>
        /// Gets the closing transaction exceptions.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account ids.</param>
        /// <returns></returns>
        internal static List<Position> GetClosingTransactionExceptions(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            List<Position> netPositions = new List<Position>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetClosedPositionsExceptionalTransactions";
                queryData.CommandTimeout = 20000;
                queryData.DictionaryDatabaseParameter.Add("@FromDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FromDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = fromDate
                });
                queryData.DictionaryDatabaseParameter.Add("@ToDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ToDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = toDate
                });
                queryData.DictionaryDatabaseParameter.Add("@FundIds", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundIds",
                    ParameterType = DbType.String,
                    ParameterValue = accountIDs
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    netPositions = GetPositionsFromSqlDataReader(reader, ApplicationConstants.TaxLotState.NotChanged);

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return netPositions;
        }
    }
}