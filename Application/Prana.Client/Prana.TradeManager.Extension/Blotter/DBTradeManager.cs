using Newtonsoft.Json.Linq;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Prana.TradeManager.Extension
{
    public class DBTradeManager
    {
        public DBTradeManager()
        {
        }

        private static readonly DBTradeManager instance = new DBTradeManager();
        public static DBTradeManager GetInstance()
        {
            return instance;
        }

        #region GetBlotterLaunchData
        public OrderBindingList GetBlotterLaunchData(int userID)
        {
            OrderBindingList _orderCollection = new OrderBindingList();
            Logger.LogMsg(LoggerLevel.Information, "GetBlotterLaunchData is invoked.");
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetBlotterLaunchDataNew";
            queryData.CommandTimeout = 200;
            string auecDatesString = TimeZoneHelper.GetInstance().GetAllAUECDateInUseAUECStr(DateTime.UtcNow);
            queryData.DictionaryDatabaseParameter.Add("@AllAUECDatesString", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@AllAUECDatesString",
                ParameterType = DbType.String,
                ParameterValue = auecDatesString
            });
            queryData.DictionaryDatabaseParameter.Add("@userID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@userID",
                ParameterType = DbType.Int32,
                ParameterValue = userID
            });
            queryData.DictionaryDatabaseParameter.Add("@isAllowMultidayStagedOrders", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@isAllowMultidayStagedOrders",
                ParameterType = DbType.Boolean,
                ParameterValue = true
            });
            Logger.LogMsg(LoggerLevel.Information, "Query for Blotter:");
            Logger.LogMsg(LoggerLevel.Information, $" exec P_GetBlotterLaunchDataNew @AllAUECDatesString=N'{auecDatesString}',@userID={userID},@isAllowMultidayStagedOrders={1}");

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        _orderCollection.Add(FillOrderDetails(row, 0));
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
            return _orderCollection;
        }

        private OrderSingle FillOrderDetails(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            OrderSingle _order = null;
            try
            {
                if (row != null)
                {
                    _order = new OrderSingle();
                    int CLORDERID = offset + 0;
                    int PARENTCLORDERID = offset + 1;
                    int LAST_PRICE = offset + 2;
                    int AVGPRICE = offset + 3;
                    int LEAVESQTY = offset + 4;
                    int CUMQTY = offset + 5;
                    int ORDERSTATUS = offset + 6;
                    int LASTSHARES = offset + 7;
                    int QUANTITY = offset + 8;
                    int SYMBOL = offset + 9;
                    int ORDERSIDE = offset + 10;
                    int ORDERTYPE = offset + 11;
                    int PRICE = offset + 12;
                    int ORIGCLORDERID = offset + 13;
                    int EXECUTION_ID = offset + 14;
                    int CLIENTTIME = offset + 15;
                    int COUNTERPARTYID = offset + 16;
                    int VENUEID = offset + 17;
                    int AUECID = offset + 18;
                    int ASSETID = offset + 19;
                    int UNDERLYINGID = offset + 20;
                    //21: For Country Flag Image
                    int STAGEDORDERID = offset + 22;
                    int TRADINGACCOUNTID = offset + 23;
                    int USERID = offset + 24;
                    int Prana_MSG_TYPE = offset + 25;
                    int DISCR_OFFSET = offset + 26;
                    int PEG_DIFF = offset + 27;
                    int STOP_PRICE = offset + 28;
                    //29: Clearance Time
                    int MATURITY_YEARMONTH = offset + 30;
                    int STRIKE_PRICE = offset + 31;
                    int PUT_CALL = offset + 32;
                    int SECURITY_TYPE = offset + 33;
                    //34: OpenClose
                    int EXEC_INST = offset + 35;
                    int TIMEINFORCE = offset + 36;
                    int HANDLINGINST = offset + 37;
                    int MESSAGETYPE = offset + 38;
                    int CMTA = offset + 39;
                    int GIVEUPID = offset + 40;
                    int UNDERLYINGSYMBOL = offset + 41;
                    int ORDER_ID = offset + 42;
                    int ALGOSTRATEGYID = offset + 43;
                    int ALGOSTRATEGYPARAMETERS = offset + 44;
                    int OriginatorTypeID = offset + 45;
                    int clientOrderID = offset + 46;
                    int AUECLocalDate = offset + 47;
                    int SettlementDate = offset + 48;
                    int SenderSubID = offset + 49;
                    int CurrencyID = offset + 50;
                    int AvgFxRateForTrade = offset + 51;
                    int Multiplier = offset + 52;
                    int ProcessDate = offset + 53;
                    int AccountID = offset + 54;
                    int StategyId = offset + 55;
                    int OrderSeqNumber = offset + 56;
                    int Calcbasis = offset + 57;
                    int CommissionRate = offset + 58;
                    int CommissionAmt = offset + 59;
                    int ImportFileName = offset + 60;
                    int ImportFileID = offset + 61;
                    int BloombergSymbol = offset + 62;
                    int SoftCommissionCalcBasis = offset + 63;
                    int SoftCommissionRate = offset + 64;
                    int SoftCommissionAmt = offset + 65;
                    int TradeAttribute1 = offset + 66;
                    int TradeAttribute2 = offset + 67;
                    int TradeAttribute3 = offset + 68;
                    int TradeAttribute4 = offset + 69;
                    int TradeAttribute5 = offset + 70;
                    int TradeAttribute6 = offset + 71;
                    int InternalComments = offset + 72;
                    int settlementCurrency = offset + 73;
                    int FxRateCalc = offset + 75;
                    #region Swap Parameters
                    int IsSwapped = offset + 76;
                    int NotionalValue = offset + 77;
                    int BenchMarkRate = offset + 78;
                    int Differential = offset + 79;
                    int OrigCostBasis = offset + 80;
                    int DayCount = offset + 81;
                    int SwapDescription = offset + 82;
                    int FirstResetDate = offset + 83;
                    int OrigTransDate = offset + 84;
                    int ResetFrequency = offset + 85;
                    int ClosingPrice = offset + 86;
                    int ClosingDate = offset + 87;
                    int TransDate = offset + 88;
                    #endregion

                    //89: Should Override Notional
                    //90: Should Override CostBasis

                    // Added change type field
                    int ChangeType = offset + 91;
                    int text = offset + 92;
                    int OriginalAllocationPreferenceID = offset + 93;
                    int TransactionSourcetag = offset + 94;
                    int LeadCurrencyID = offset + 95;
                    int VsCurrencyID = offset + 96;
                    int allocationState = offset + 97;
                    int accountIDs = offset + 98;
                    int strategyIDs = offset + 99;
                    int allocationSchemeName = offset + 100;
                    int rebalancerFileName = offset + 101;
                    int sedolSymbol = offset + 102;
                    int companyName = offset + 103;
                    int actualCompanyUserID = offset + 104;
                    int BorrowerID = offset + 105;
                    int BorrowBroker = offset + 106;
                    int ShortRebate = offset + 107;
                    int NirvanaLocateID = offset + 108;
                    int IsManualOrder = offset + 109;
                    int activSymbol = offset + 110;
                    int factsetSymbol = offset + 111;
                    int executionTimeLastFill = offset + 112;
                    int expireTime = offset + 113;
                    int isUseCustodianBroker = offset + 116;
                    int BloombergExchangeCode = offset + 117;
                    int originalPurchaseDate = offset + 118;
                    int tradeAttributes = offset + 119;
                    int isMultiBrokerTrade = offset + 120;

                    _order.CalcBasis = (CalculationBasis)int.Parse(row[Calcbasis].ToString());
                    if (!row[CommissionAmt].ToString().Equals(string.Empty))
                    {
                        _order.CommissionAmt = double.Parse(row[CommissionAmt].ToString(), System.Globalization.NumberStyles.Float);
                    }
                    else
                    {
                        _order.CommissionAmt = 0.0;
                    }
                    _order.CommissionRate = double.Parse(row[CommissionRate].ToString(), System.Globalization.NumberStyles.Float);

                    _order.SoftCommissionCalcBasis = (CalculationBasis)int.Parse(row[SoftCommissionCalcBasis].ToString());
                    if (!row[SoftCommissionAmt].ToString().Equals(string.Empty))
                    {
                        _order.SoftCommissionAmt = double.Parse(row[SoftCommissionAmt].ToString(), System.Globalization.NumberStyles.Float);
                    }
                    else
                    {
                        _order.SoftCommissionAmt = 0.0;
                    }
                    _order.SoftCommissionRate = double.Parse(row[SoftCommissionRate].ToString(), System.Globalization.NumberStyles.Float);

                    _order.ClOrderID = row[CLORDERID].ToString();
                    _order.ParentClOrderID = row[PARENTCLORDERID].ToString();
                    _order.Price = double.Parse(row[PRICE].ToString(), System.Globalization.NumberStyles.Float);
                    _order.LastPrice = double.Parse(row[LAST_PRICE].ToString(), System.Globalization.NumberStyles.Float);
                    _order.LeavesQty = double.Parse(row[LEAVESQTY].ToString(), System.Globalization.NumberStyles.Float);
                    _order.CumQty = double.Parse(row[CUMQTY].ToString(), System.Globalization.NumberStyles.Float);
                    _order.LastShares = double.Parse(row[LASTSHARES].ToString(), System.Globalization.NumberStyles.Float);
                    _order.OrderStatusTagValue = row[ORDERSTATUS].ToString().Trim();
                    _order.AvgPrice = Double.Parse(row[AVGPRICE].ToString(), System.Globalization.NumberStyles.Float);
                    _order.Quantity = Double.Parse(row[QUANTITY].ToString(), System.Globalization.NumberStyles.Float);
                    _order.Symbol = row[SYMBOL].ToString();
                    _order.ExecID = row[EXECUTION_ID].ToString();
                    _order.OrderSideTagValue = row[ORDERSIDE].ToString().Trim();
                    _order.OrderTypeTagValue = row[ORDERTYPE].ToString().Trim();
                    _order.OrigClOrderID = row[ORIGCLORDERID].ToString();
                    _order.CounterPartyID = int.Parse(row[COUNTERPARTYID].ToString(), System.Globalization.NumberStyles.Integer);
                    _order.VenueID = int.Parse(row[VENUEID].ToString(), System.Globalization.NumberStyles.Integer);
                    _order.AUECID = int.Parse(row[AUECID].ToString(), System.Globalization.NumberStyles.Integer);
                    _order.ClientTime = row[CLIENTTIME].ToString();
                    DateTime transTime = DateTime.Parse(row[AUECLocalDate].ToString());
                    _order.TransactionTime = transTime;
                    _order.ExecutionTimeLastFill = row[executionTimeLastFill].ToString();
                    if (_order.ExecutionTimeLastFill == string.Empty)
                        _order.ExecutionTimeLastFill = row[executionTimeLastFill].ToString();
                    else
                    {
                        DateTime dtExecutionTimeLastFill;
                        if (!(_order.ExecutionTimeLastFill.ToString().Contains("/")))
                        {
                            dtExecutionTimeLastFill = DateTime.ParseExact(_order.ExecutionTimeLastFill, DateTimeConstants.NirvanaDateTimeFormat, null);
                        }
                        else
                        {
                            dtExecutionTimeLastFill = DateTime.Parse(_order.ExecutionTimeLastFill);
                        }
                        _order.ExecutionTimeLastFill = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dtExecutionTimeLastFill, CachedDataManager.GetInstance.GetAUECTimeZone(_order.AUECID)).ToString(DateTimeConstants.NirvanaDateTimeFormat);
                    }
                    _order.AssetID = int.Parse(row[ASSETID].ToString(), System.Globalization.NumberStyles.Integer);
                    _order.UnderlyingID = int.Parse(row[UNDERLYINGID].ToString(), System.Globalization.NumberStyles.Integer);
                    _order.StagedOrderID = row[STAGEDORDERID].ToString();
                    _order.TradingAccountID = Int32.Parse(row[TRADINGACCOUNTID].ToString());
                    _order.CompanyUserID = Int32.Parse(row[USERID].ToString());
                    _order.PranaMsgType = Int32.Parse(row[Prana_MSG_TYPE].ToString());
                    _order.DiscretionOffset = Double.Parse(row[DISCR_OFFSET].ToString());
                    _order.PegDifference = Double.Parse(row[PEG_DIFF].ToString());
                    _order.StopPrice = Double.Parse(row[STOP_PRICE].ToString());
                    _order.MaturityMonthYear = row[MATURITY_YEARMONTH].ToString();
                    _order.StrikePrice = Double.Parse(row[STRIKE_PRICE].ToString());
                    _order.SecurityType = row[SECURITY_TYPE].ToString();
                    _order.PutOrCalls = row[PUT_CALL].ToString();
                    _order.ExecutionInstruction = row[EXEC_INST].ToString().Trim();
                    _order.TIF = row[TIMEINFORCE].ToString().Trim();
                    _order.HandlingInstruction = row[HANDLINGINST].ToString().Trim();
                    _order.MsgType = row[MESSAGETYPE].ToString();
                    _order.Level1ID = int.Parse(row[AccountID].ToString(), System.Globalization.NumberStyles.Integer);
                    if (row[StategyId] != DBNull.Value)
                    {
                        _order.Level2ID = int.Parse(row[StategyId].ToString(), System.Globalization.NumberStyles.Integer);
                    }
                    if (row[OrderSeqNumber] != DBNull.Value)
                    {
                        _order.OrderSeqNumber = Int64.Parse(row[OrderSeqNumber].ToString());
                    }
                    if (row[CMTA].ToString() != string.Empty)
                    {
                        _order.CMTAID = int.Parse(row[CMTA].ToString());
                    }
                    if (row[GIVEUPID].ToString() != string.Empty)
                    {
                        _order.GiveUpID = int.Parse(row[GIVEUPID].ToString());
                    }
                    if (row[UNDERLYINGSYMBOL] != DBNull.Value)
                    {
                        _order.UnderlyingSymbol = row[UNDERLYINGSYMBOL].ToString();
                    }
                    if (row[ORDER_ID] != DBNull.Value)
                    {
                        _order.OrderID = row[ORDER_ID].ToString();
                    }
                    if (row[ALGOSTRATEGYID] != DBNull.Value && row[ALGOSTRATEGYID].ToString() != string.Empty)
                    {
                        _order.AlgoStrategyID = row[ALGOSTRATEGYID].ToString();
                        _order.AlgoStrategyName = AlgoStrategyNamesDetails.GetAlgoStrategyText(_order.AlgoStrategyID, _order.CounterPartyID);
                    }
                    if (row[ALGOSTRATEGYPARAMETERS] != DBNull.Value)
                    {
                        _order.AlgoProperties = new OrderAlgoStartegyParameters(row[ALGOSTRATEGYPARAMETERS].ToString());
                    }
                    if (row[OriginatorTypeID] != DBNull.Value)
                    {
                        _order.OriginatorType = int.Parse(row[OriginatorTypeID].ToString());
                    }
                    if (row[clientOrderID] != DBNull.Value)
                    {
                        _order.ClientOrderID = row[clientOrderID].ToString();
                    }
                    if (row[AUECLocalDate] != DBNull.Value)
                    {
                        _order.AUECLocalDate = Convert.ToDateTime(row[AUECLocalDate]);
                    }
                    if (row[SettlementDate] != DBNull.Value)
                    {
                        _order.SettlementDate = Convert.ToDateTime(row[SettlementDate]);
                    }
                    if (row[SenderSubID] != DBNull.Value)
                    {
                        _order.SenderSubID = row[SenderSubID].ToString();
                    }
                    if (row[CurrencyID] != DBNull.Value)
                    {
                        _order.CurrencyID = int.Parse(row[CurrencyID].ToString());
                    }
                    if (row[AvgFxRateForTrade] != DBNull.Value)
                    {
                        _order.FXRate = double.Parse(row[AvgFxRateForTrade].ToString());
                    }
                    if (row[Multiplier] != DBNull.Value)
                    {
                        _order.ContractMultiplier = double.Parse(row[Multiplier].ToString());
                    }
                    if (row[ProcessDate] != DBNull.Value)
                    {
                        _order.ProcessDate = Convert.ToDateTime(row[ProcessDate].ToString());
                    }
                    if (row[ImportFileName] != DBNull.Value)
                    {
                        _order.ImportFileName = row[ImportFileName].ToString();
                    }
                    if (row[ImportFileID] != DBNull.Value)
                    {
                        _order.ImportFileID = int.Parse(row[ImportFileID].ToString());
                    }
                    if (row[BloombergSymbol] != DBNull.Value)
                    {
                        _order.BloombergSymbol = row[BloombergSymbol].ToString();
                    }
                    if (row[BloombergExchangeCode] != DBNull.Value)
                    {
                        _order.BloombergSymbolWithExchangeCode = row[BloombergExchangeCode].ToString();
                    }
                    if (row[activSymbol] != DBNull.Value)
                    {
                        _order.ActivSymbol = row[activSymbol].ToString();
                    }
                    if (row[factsetSymbol] != DBNull.Value)
                    {
                        _order.FactSetSymbol = row[factsetSymbol].ToString();
                    }
                    if (row[TradeAttribute1] != DBNull.Value)
                    {
                        _order.TradeAttribute1 = row[TradeAttribute1].ToString();
                    }
                    if (row[TradeAttribute2] != DBNull.Value)
                    {
                        _order.TradeAttribute2 = row[TradeAttribute2].ToString();
                    }
                    if (row[TradeAttribute3] != DBNull.Value)
                    {
                        _order.TradeAttribute3 = row[TradeAttribute3].ToString();
                    }
                    if (row[TradeAttribute4] != DBNull.Value)
                    {
                        _order.TradeAttribute4 = row[TradeAttribute4].ToString();
                    }
                    if (row[TradeAttribute5] != DBNull.Value)
                    {
                        _order.TradeAttribute5 = row[TradeAttribute5].ToString();
                    }
                    if (row[TradeAttribute6] != DBNull.Value)
                    {
                        _order.TradeAttribute6 = row[TradeAttribute6].ToString();
                    }
                    if (row[InternalComments] != DBNull.Value)
                    {
                        _order.InternalComments = row[InternalComments].ToString();
                    }
                    if (row[settlementCurrency] != DBNull.Value)
                    {
                        _order.SettlementCurrencyID = Int32.Parse(row[settlementCurrency].ToString());
                    }
                    if (row[FxRateCalc] != DBNull.Value)
                    {
                        _order.FXConversionMethodOperator = row[FxRateCalc].ToString();
                    }

                    #region Swap Parameters
                    if (row[IsSwapped] != DBNull.Value)
                    {
                        bool IsSwap = bool.Parse(row[IsSwapped].ToString());
                        if (IsSwap)
                        {
                            SwapParameters swapParameters = new SwapParameters();
                            if (row[NotionalValue] != DBNull.Value)
                            {
                                swapParameters.NotionalValue = double.Parse(row[NotionalValue].ToString());
                            }
                            if (row[BenchMarkRate] != DBNull.Value)
                            {
                                swapParameters.BenchMarkRate = double.Parse(row[BenchMarkRate].ToString());
                            }
                            if (row[Differential] != DBNull.Value)
                            {
                                swapParameters.Differential = double.Parse(row[Differential].ToString());
                            }
                            if (row[OrigCostBasis] != DBNull.Value)
                            {
                                swapParameters.OrigCostBasis = double.Parse(row[OrigCostBasis].ToString());
                            }
                            if (row[DayCount] != DBNull.Value)
                            {
                                swapParameters.DayCount = int.Parse(row[DayCount].ToString());
                            }
                            if (row[SwapDescription] != DBNull.Value)
                            {
                                swapParameters.SwapDescription = row[SwapDescription].ToString();
                            }
                            if (row[FirstResetDate] != DBNull.Value)
                            {
                                swapParameters.FirstResetDate = DateTime.Parse(row[FirstResetDate].ToString());
                            }
                            if (row[OrigTransDate] != DBNull.Value)
                            {
                                swapParameters.OrigTransDate = DateTime.Parse(row[OrigTransDate].ToString());
                            }
                            if (row[ResetFrequency] != DBNull.Value)
                            {
                                swapParameters.ResetFrequency = row[ResetFrequency].ToString();
                            }
                            if (row[ClosingPrice] != DBNull.Value)
                            {
                                swapParameters.ClosingPrice = double.Parse(row[ClosingPrice].ToString());
                            }
                            if (row[ClosingDate] != DBNull.Value)
                            {
                                swapParameters.ClosingDate = DateTime.Parse(row[ClosingDate].ToString());
                            }
                            if (row[TransDate] != DBNull.Value)
                            {
                                swapParameters.TransDate = DateTime.Parse(row[TransDate].ToString());
                            }
                            _order.SwapParameters = swapParameters;
                        }
                    }
                    #endregion

                    if (row[ChangeType] != DBNull.Value)
                    {
                        _order.ChangeType = Int32.Parse(row[ChangeType].ToString());
                    }
                    if (row[text] != DBNull.Value)
                    {
                        _order.Text = row[text].ToString();
                    }
                    if (row[OriginalAllocationPreferenceID] != DBNull.Value)
                    {
                        _order.OriginalAllocationPreferenceID = int.Parse(row[OriginalAllocationPreferenceID].ToString());
                    }
                    if (row[TransactionSourcetag] != DBNull.Value)
                    {
                        _order.TransactionSourceTag = int.Parse(row[TransactionSourcetag].ToString());
                        _order.TransactionSource = ((TransactionSource)_order.TransactionSourceTag);
                    }
                    if (row[LeadCurrencyID] != DBNull.Value)
                    {
                        _order.LeadCurrencyID = Int32.Parse(row[LeadCurrencyID].ToString());
                    }
                    if (row[VsCurrencyID] != DBNull.Value)
                    {
                        _order.VsCurrencyID = Int32.Parse(row[VsCurrencyID].ToString());
                    }
                    if (row[IsManualOrder] != DBNull.Value)
                    {
                        _order.IsManualOrder = Boolean.Parse(row[IsManualOrder].ToString());
                    }

                    //Forcefully updated value of Account, Master fund, Strategy and Allocation Status value to Dash (-) in case of Order status is Pending New or Rejected. 
                    //Because in case of Pending new and Rejected case, These groups are not visible in Allocation.
                    if (_order.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingNew && _order.OrderStatusTagValue != FIXConstants.ORDSTATUS_Rejected)
                    {
                        if (row[allocationState] != DBNull.Value)
                            _order.AllocationStatus = row[allocationState].ToString();

                        if (row[allocationSchemeName] != DBNull.Value)
                            _order.AllocationSchemeName = row[allocationSchemeName].ToString();

                        if (row[accountIDs] != DBNull.Value)
                        {
                            //Account
                            List<string> accounts = row[accountIDs].ToString().Split(',').ToList();
                            _order.Account = accounts.Distinct().Count() > 1 ? OrderFields.PROPERTY_MULTIPLE : CachedDataManager.GetInstance.GetAccountText(Int32.Parse(accounts[0].ToString()));

                            //MasterFund
                            List<int> masterFunds = new List<int>();
                            accounts.ForEach(accountID =>
                            {
                                int masterFundID = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(Int32.Parse(accountID));
                                if (!masterFunds.Contains(masterFundID))
                                    masterFunds.Add(masterFundID);
                            });

                            if (masterFunds.Distinct().Count() > 1)
                                _order.MasterFund = OrderFields.PROPERTY_MULTIPLE;
                            else
                            {
                                string masterFundName = CachedDataManager.GetInstance.GetMasterFund(masterFunds[0]);
                                _order.MasterFund = String.IsNullOrEmpty(masterFundName) ? OrderFields.PROPERTY_DASH : masterFundName;
                            }
                        }
                        else
                        {
                            _order.Account = OrderFields.PROPERTY_DASH;
                            _order.MasterFund = CachedDataManager.GetInstance.IsShowMasterFundonTT() && !string.IsNullOrEmpty(_order.TradeAttribute6) ? _order.TradeAttribute6 : OrderFields.PROPERTY_DASH;
                            _order.AllocationSchemeName = OrderFields.PROPERTY_DASH;
                        }

                        //Strategy
                        if (row[strategyIDs] != DBNull.Value)
                        {
                            List<string> strategyValues = row[strategyIDs].ToString().Split(',').Distinct().ToList();
                            _order.Strategy = strategyValues.Count > 1 ? OrderFields.PROPERTY_MULTIPLE : CachedDataManager.GetInstance.GetStrategyText(Convert.ToInt32(strategyValues[0]));
                        }
                        else
                            _order.Strategy = OrderFields.PROPERTY_DASH;
                    }
                    else
                    {
                        _order.AllocationSchemeName = _order.AllocationStatus = _order.Strategy = _order.Account = _order.MasterFund = OrderFields.PROPERTY_DASH;
                    }

                    if (row[rebalancerFileName] != DBNull.Value)
                    {
                        _order.RebalancerFileName = row[rebalancerFileName].ToString();
                    }
                    if (row[sedolSymbol] != DBNull.Value)
                    {
                        _order.SEDOLSymbol = row[sedolSymbol].ToString();
                    }
                    if (row[companyName] != DBNull.Value)
                    {
                        _order.CompanyName = row[companyName].ToString();
                    }
                    if (row[actualCompanyUserID] != DBNull.Value)
                    {
                        _order.ActualCompanyUserID = Convert.ToInt32(row[actualCompanyUserID].ToString());
                    }
                    if (row[BorrowerID] != DBNull.Value)
                    {
                        _order.BorrowerID = row[BorrowerID].ToString();
                    }
                    if (row[BorrowBroker] != DBNull.Value)
                    {
                        _order.BorrowerBroker = row[BorrowBroker].ToString();
                    }
                    if (row[ShortRebate] != DBNull.Value)
                    {
                        _order.ShortRebate = Convert.ToDouble(row[ShortRebate].ToString());
                    }
                    if (row[NirvanaLocateID] != DBNull.Value)
                    {
                        _order.NirvanaLocateID = Convert.ToInt32(row[NirvanaLocateID].ToString());
                    }
                    if (row[expireTime] != DBNull.Value && !string.IsNullOrEmpty(row[expireTime].ToString()))
                    {
                        DateTime _expireTime = DateTime.ParseExact(row[expireTime].ToString(), DateTimeConstants.NirvanaDateTimeFormat, null);
                        _order.ExpireTime = _expireTime.ToString();
                    }                   
                    _order.IsUseCustodianBroker = bool.Parse(row[isUseCustodianBroker].ToString());
                    if (_order.IsUseCustodianBroker && _order.Account == OrderFields.PROPERTY_MULTIPLE && _order.CounterPartyID == int.MinValue)
                    {
                        _order.CounterPartyName = OrderFields.PROPERTY_MULTIPLE;
                    }
                    else if (_order.CounterPartyID != int.MinValue)
                    {
                        _order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(_order.CounterPartyID);
                    }
                    if (row[originalPurchaseDate] != DBNull.Value)
                    {
                        DateTime dt = DateTimeConstants.MinValue;
                        _order.OriginalPurchaseDate = dt;
                        if (DateTime.TryParse(row[originalPurchaseDate].ToString(), out dt))
                        {
                            _order.OriginalPurchaseDate = dt;
                        }

                    }
                    if (row[tradeAttributes] != DBNull.Value)
                    {
                        string json = row[tradeAttributes].ToString();
                        _order.SetTradeAttribute(json);
                    }
                    if(row[isMultiBrokerTrade] != DBNull.Value)
                    {
                        _order.IsMultiBrokerTrade = Convert.ToBoolean(row[isMultiBrokerTrade]);
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
            return _order;

        }
        #endregion

        #region Clearance Data
        public GenericRepository<ClearanceData> GetClearanceData(int companyID, ref Dictionary<int, bool> dictRolloverPermittedAUEC)
        {
            GenericRepository<ClearanceData> clearanceCompleteData = new GenericRepository<ClearanceData>();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetBlotterClearanceForCompany", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        StringBuilder sb = null;
                        ClearanceData clearanceData = FillClearanceData(row, 0);

                        //Rollover permitted AUEC adding these in Dictionary
                        if (!dictRolloverPermittedAUEC.ContainsKey(clearanceData.AUECID))
                            dictRolloverPermittedAUEC.Add(clearanceData.AUECID, clearanceData.PermitRollover);

                        ClearanceData clearanceDataOld = clearanceCompleteData.GetItem(clearanceData.GetKey());
                        if (clearanceDataOld == null)
                        {
                            sb = new StringBuilder(clearanceData.AUECID.ToString());
                            sb.Append(",");
                            clearanceData.AUECIDStr = sb.ToString();
                            clearanceCompleteData.Add(clearanceData);
                        }
                        else
                        {
                            sb = new StringBuilder(clearanceDataOld.AUECIDStr);
                            sb.Append(clearanceData.AUECID.ToString());
                            sb.Append(",");
                            clearanceDataOld.AUECIDStr = sb.ToString();
                        }
                    }
                }
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
            return clearanceCompleteData;
        }

        private ClearanceData FillClearanceData(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            ClearanceData _clearanceData = null;
            try
            {
                if (row != null)
                {
                    _clearanceData = new ClearanceData();

                    int AUEC = 0 + offset;
                    int AUECID = 1 + offset;
                    int STARTTIME = 2 + offset;
                    int ENDTIME = 3 + offset;
                    int CLEARANCETIME = 4 + offset;
                    int CLEARANCETIMEID = 5 + offset;
                    int COMPANYAUECID = 6 + offset;
                    int EXCHANGEIDENTIFIER = 7 + offset;
                    int PERMITROLLOVER = 8 + offset;

                    if (row != null)
                    {
                        _clearanceData.AUEC = Convert.ToString(row[AUEC]);
                        _clearanceData.AUECID = Convert.ToInt32(row[AUECID]);
                        _clearanceData.ExchangeRegularTradingStartTime = Convert.ToString(row[STARTTIME]);
                        _clearanceData.ExchangeRegularTradingEndTime = Convert.ToString(row[ENDTIME]);
                        //https://jira.nirvanasolutions.com:8443/browse/PRANA-22503
                        _clearanceData.ClearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(Convert.ToDateTime(row[CLEARANCETIME]), CachedDataManager.GetInstance.GetAUECTimeZone(_clearanceData.AUECID));
                        _clearanceData.ClearanceTimeID = Convert.ToInt32(row[CLEARANCETIMEID]);
                        _clearanceData.CompanyAUECID = Convert.ToInt32(row[COMPANYAUECID]);
                        _clearanceData.ExchangeIdentifier = Convert.ToString(row[EXCHANGEIDENTIFIER]);
                        _clearanceData.PermitRollover = Convert.ToBoolean(row[PERMITROLLOVER]);
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
            return _clearanceData;
        }

        public BlotterClearanceCommonData GetCompanyClearanceCommonData(int companyID)
        {
            BlotterClearanceCommonData blotterClearanceCommonData = new BlotterClearanceCommonData();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClearanceCommonData", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value)
                        {
                            blotterClearanceCommonData.TimeZone = row[0].ToString();
                        }
                        if (row[1] != DBNull.Value)
                        {
                            blotterClearanceCommonData.AutoClearing = Convert.ToBoolean(row[1].ToString());
                        }
                        if (row[2] != DBNull.Value)
                        {
                            blotterClearanceCommonData.BaseTime = Convert.ToDateTime(row[2].ToString());
                        }
                        if (row[3] != DBNull.Value)
                        {
                            blotterClearanceCommonData.RolloverPermittedUserID = Convert.ToInt32(row[3].ToString());
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
            return blotterClearanceCommonData;
        }
        #endregion

        #region Get Cached Replace Orders From DB
        public Dictionary<string, OrderSingle> GetReplacedAlgoOrdersFromDB(DateTime _date, int userID)
        {
            Dictionary<string, OrderSingle> _orderCollection = new Dictionary<string, OrderSingle>();
            object[] parameter = new object[2];
            string day = _date.ToString();
            parameter[0] = day;
            parameter[1] = userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAlgoSyntheticReplaceOrderRequest", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        OrderSingle dbOrder = FillAlgoOrderDetails(row, 0);
                        if (dbOrder.AlgoSyntheticRPLParent != null)
                        {
                            if (!_orderCollection.ContainsKey(dbOrder.AlgoSyntheticRPLParent))
                            {
                                _orderCollection.Add(dbOrder.AlgoSyntheticRPLParent, dbOrder);
                            }
                        }
                    }
                }
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
            return _orderCollection;
        }

        private OrderSingle FillAlgoOrderDetails(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            OrderSingle _order = null;
            try
            {
                if (row != null)
                {
                    _order = new OrderSingle();
                    int ALGOSYNTHETICRPLPARENT = offset + 0;

                    int CLORDERID = offset + 1;
                    int DISCR_OFFSET = offset + 3;
                    int EXEC_INST = offset + 4;
                    int HANDLINGINST = offset + 5;
                    int MESSAGETYPE = offset + 6;
                    int ORDER_ID = offset + 7;
                    int ORDERSIDE = offset + 8;
                    int ORDERSTATUS = offset + 9;
                    int ORDERTYPE = offset + 10;
                    int ORIGCLORDERID = offset + 11;
                    int PEG_DIFF = offset + 12;
                    int PNP = offset + 13;
                    int PRICE = offset + 14;
                    int QUANTITY = offset + 15;
                    int STOP_PRICE = offset + 16;
                    int SYMBOL = offset + 17;
                    int TARGETCOMPID = offset + 18;
                    int TARGETSUBID = offset + 19;
                    int TIF = offset + 20;
                    int VENUEID = offset + 21;
                    int PARENTCLORDERID = offset + 22;
                    int LOCATE_REQ = offset + 23;
                    int BORROWERID = offset + 24;
                    int SHORT_REBATE = offset + 25;
                    int TRADINGACCOUNTID = offset + 26;
                    int USERID = offset + 27;
                    int COUNTERPARTYID = offset + 28;
                    int AUECID = offset + 29;
                    int STAGEDORDERID = offset + 30;
                    int Prana_MSG_TYPE = offset + 31;
                    int TEXT = offset + 32;
                    int STRATEGYID = offset + 33;
                    int FUNDID = offset + 34;
                    int SECURITY_TYPE = offset + 35;
                    int PUT_CALL = offset + 36;
                    int MATURITY_YEARMONTH = offset + 37;
                    int STRIKE_PRICE = offset + 38;
                    int OPEN_CLOSE = offset + 39;
                    int PARENTCLIENTORDERID = offset + 40;
                    int CLIENTORDERID = offset + 41;
                    int CMTA = offset + 42;
                    int GIVEUPID = offset + 43;
                    int UNDERLYINGSYMBOL = offset + 44;
                    int EXPIRATIONDATE = offset + 45;
                    int SETTLEMENTDATE = offset + 46;
                    int ALGOSTRATEGYID = offset + 47;
                    int ALGOSTRATEGYPARAMETERS = offset + 48;
                    int ASSETID = offset + 49;
                    int UNDERLYINGID = offset + 50;
                    int FLAG = offset + 51;

                    if (row[ALGOSYNTHETICRPLPARENT] != DBNull.Value)
                    {
                        _order.AlgoSyntheticRPLParent = row[ALGOSYNTHETICRPLPARENT].ToString();
                    }

                    _order.ClOrderID = row[CLORDERID].ToString();
                    if (row[DISCR_OFFSET] != DBNull.Value)
                    {
                        _order.DiscretionOffset = Double.Parse(row[DISCR_OFFSET].ToString());
                    }
                    _order.HandlingInstruction = row[HANDLINGINST].ToString().Trim();
                    _order.ExecutionInstruction = row[EXEC_INST].ToString().Trim();
                    _order.MsgType = row[MESSAGETYPE].ToString();
                    if (row[ORDER_ID] != DBNull.Value)
                    {
                        _order.OrderID = row[ORDER_ID].ToString();
                    }
                    _order.OrderSideTagValue = row[ORDERSIDE].ToString().Trim();
                    _order.OrderStatusTagValue = row[ORDERSTATUS].ToString().Trim();
                    _order.OrderTypeTagValue = row[ORDERTYPE].ToString().Trim();
                    _order.OrigClOrderID = row[ORIGCLORDERID].ToString();
                    if (row[PEG_DIFF] != DBNull.Value)
                    {
                        _order.PegDifference = Double.Parse(row[PEG_DIFF].ToString());
                    }
                    if (row[PNP] != DBNull.Value)
                    {
                        _order.PNP = row[PNP].ToString();
                    }
                    _order.Price = double.Parse(row[PRICE].ToString(), System.Globalization.NumberStyles.Float);
                    _order.Quantity = Double.Parse(row[QUANTITY].ToString(), System.Globalization.NumberStyles.Float);
                    if (row[STOP_PRICE] != DBNull.Value)
                    {
                        _order.StopPrice = Double.Parse(row[STOP_PRICE].ToString());
                    }
                    _order.Symbol = row[SYMBOL].ToString();
                    if (row[TARGETCOMPID] != DBNull.Value)
                    {
                        _order.TargetCompID = row[TARGETCOMPID].ToString();
                    }
                    if (row[TARGETSUBID] != DBNull.Value)
                    {
                        _order.TargetSubID = row[TARGETSUBID].ToString();
                    }
                    _order.TIF = row[TIF].ToString().Trim();
                    _order.VenueID = int.Parse(row[VENUEID].ToString(), System.Globalization.NumberStyles.Integer);
                    _order.ParentClOrderID = row[PARENTCLORDERID].ToString();
                    if (row[LOCATE_REQ] != DBNull.Value)
                    {
                        _order.LocateReqd = bool.Parse(row[LOCATE_REQ].ToString());
                    }
                    if (row[BORROWERID] != DBNull.Value)
                    {
                        _order.BorrowerID = row[BORROWERID].ToString();
                    }
                    if (row[SHORT_REBATE] != DBNull.Value)
                    {
                        _order.ShortRebate = double.Parse(row[SHORT_REBATE].ToString());
                    }

                    _order.TradingAccountID = Int32.Parse(row[TRADINGACCOUNTID].ToString());
                    _order.CompanyUserID = Int32.Parse(row[USERID].ToString());
                    _order.CounterPartyID = int.Parse(row[COUNTERPARTYID].ToString(), System.Globalization.NumberStyles.Integer);
                    _order.AUECID = int.Parse(row[AUECID].ToString(), System.Globalization.NumberStyles.Integer);
                    if (row[STAGEDORDERID] != DBNull.Value)
                    {
                        _order.StagedOrderID = row[STAGEDORDERID].ToString();
                    }

                    _order.PranaMsgType = Int32.Parse(row[Prana_MSG_TYPE].ToString());
                    if (row[TEXT] != DBNull.Value)
                    {
                        _order.Text = row[TEXT].ToString();
                    }

                    _order.Level2ID = int.Parse(row[STRATEGYID].ToString());
                    _order.Level1ID = int.Parse(row[FUNDID].ToString());
                    _order.SecurityType = row[SECURITY_TYPE].ToString();
                    _order.PutOrCalls = row[PUT_CALL].ToString();
                    if (row[MATURITY_YEARMONTH] != DBNull.Value)
                    {
                        _order.MaturityMonthYear = row[MATURITY_YEARMONTH].ToString();
                    }
                    if (row[STRIKE_PRICE] != DBNull.Value)
                    {
                        _order.StrikePrice = Double.Parse(row[STRIKE_PRICE].ToString());
                    }
                    if (row[OPEN_CLOSE] != DBNull.Value)
                    {
                        _order.OpenClose = row[OPEN_CLOSE].ToString().Trim();
                    }
                    if (row[PARENTCLIENTORDERID] != DBNull.Value)
                    {
                        _order.ParentClientOrderID = row[PARENTCLIENTORDERID].ToString();
                    }
                    if (row[CLIENTORDERID] != DBNull.Value)
                    {
                        _order.ClientOrderID = row[CLIENTORDERID].ToString();
                    }

                    if (row[CMTA].ToString() != string.Empty)
                    { _order.CMTAID = int.Parse(row[CMTA].ToString()); }
                    if (row[GIVEUPID].ToString() != string.Empty)
                    { _order.GiveUpID = int.Parse(row[GIVEUPID].ToString()); }
                    if (row[UNDERLYINGSYMBOL] != DBNull.Value)
                    { _order.UnderlyingSymbol = row[UNDERLYINGSYMBOL].ToString(); }
                    _order.ExpirationDate = Convert.ToDateTime(row[EXPIRATIONDATE].ToString());
                    _order.SettlementDate = Convert.ToDateTime(row[SETTLEMENTDATE].ToString());
                    if (row[ALGOSTRATEGYID] != DBNull.Value)
                    { _order.AlgoStrategyID = row[ALGOSTRATEGYID].ToString(); }
                    if (row[ALGOSTRATEGYPARAMETERS] != DBNull.Value)
                    { _order.AlgoProperties = new OrderAlgoStartegyParameters(row[ALGOSTRATEGYPARAMETERS].ToString()); }
                    if (row[ASSETID] != DBNull.Value)
                    { _order.AssetID = int.Parse(row[ASSETID].ToString()); }
                    if (row[ASSETID] != DBNull.Value)
                    {
                        _order.UnderlyingID = int.Parse(row[UNDERLYINGID].ToString(), System.Globalization.NumberStyles.Integer);
                    }
                    if (row[FLAG].ToString() != string.Empty)
                    { _order.Flag = (byte[])(row[FLAG]); }
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
            return _order;
        }
        #endregion

        public List<string>[] getSubTradeAttributesList()
        {
            List<string>[] lists = new List<string>[6];

            try
            {
                for (int i = 1; i <= 6; i++)
                {
                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "P_GetSubTradeAttribute" + i + "List";

                    lists[i - 1] = new List<string>();
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            lists[i - 1].Add(row[0].ToString());
                        }
                    }
                }
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
            return lists;
        }

        /// <summary>
        /// Gets the name of the blotter linked tab.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public string GetBlotterLinkedTabName(int userId)
        {
            string linkedTabName = string.Empty;
            object[] parameter = new object[1];
            parameter[0] = userId;
            try
            {
                linkedTabName = DatabaseManager.DatabaseManager.ExecuteScalar("P_GetLinkedTab_Blotter", parameter, ApplicationConstants.PranaConnectionString).ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return linkedTabName;
        }

        /// <summary>
        /// Sets the name of the blotter linked tab.
        /// </summary>
        /// <param name="linkedTabName">Name of the linked tab.</param>
        /// <param name="userId">The user identifier.</param>
        public void SetBlotterLinkedTabName(string linkedTabName, int userId)
        {
            try
            {
                object[] parameter = new object[2];
                parameter[0] = linkedTabName;
                parameter[1] = userId;

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SetLinkedTab_Blotter", parameter, ApplicationConstants.PranaConnectionString);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}