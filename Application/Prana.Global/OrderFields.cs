using System.Collections;
using System.Collections.Generic;

namespace Prana.Global
{
    public class OrderFields
    {
        #region Column Header Caption
        public const string CAPTION_PARENT_CL_ORDERID = "POrder ID";
        public const string CAPTION_SYMBOL = "Symbol";
        public const string CAPTION_ORDER_ID = "OrderID";
        public const string CAPTION_CANCEL_ORDER_ID = "Cl Ord ID";
        public const string CAPTION_STAGED_ORDERID = "Staged Ord ID";
        public const string CAPTION_ORDER_STATUS = "Status";
        public const string CAPTION_ORDER_STATUS_WITHROLLOVER = "Status (With Rollover)";
        public const string CAPTION_ORDER_SIDE = "Side";
        public const string CAPTION_ORDER_TYPE = "Order Type";
        public const string CAPTION_QUANTITY = "Tgt Qty";
        public const string CAPTION_QUANTITY_PERCENTAGE = "%Qty";
        public const string CAPTION_VENUE = "Venue";
        public const string CAPTION_VENUE_ID = "VenueID";
        public const string CAPTION_COUNTERPARTY_NAME = "Broker";
        public const string CAPTION_COUNTERPARTY_ID = "CounterPartyID";
        public const string CAPTION_ASSETCATEGORY = "AssetCategory";
        public const string CAPTION_TRADINGACCOUNT_ID = "TradingAccountID";
        public const string CAPTION_TRADING_ACCOUNT = "Trad Acc";
        public const string CAPTION_USER_ID = "UserID";
        public const string CAPTION_USER = "User";
        public const string CAPTION_ORIGINAL_USER = "Actual User";
        public const string CAPTION_MODIFIED_USER = "Modified User";
        public const string CAPTION_SENDER_SUBID = "Sender Sub ID";
        public const string CAPTION_SENDER_COMPID = "Sender Comp ID";
        public const string CAPTION_TARGET_COMPID = "Target Comp ID";
        public const string CAPTION_SENDING_TIME = "Sending Time";
        public const string CAPTION_PRICE = "Price";
        public const string CAPTION_EXECUTIONID = "ExecID";
        public const string CAPTION_LASTPRICE = "Last Px";
        public const string CAPTION_AVGPRICE = "Avg Price";
        public const string CAPTION_LAST_MARKET = "Last Mkt";
        public const string CAPTION_EXECUTED_QTY = "Executed Qty";
        public const string CAPTION_LAST_SHARES = "Fill";
        public const string CAPTION_LEAVES_QUANTITY = "Working Qty";
        public const string CAPTION_TEXT = "Details";
        public const string CAPTION_REJECT_REASON = "RejectReason";
        public const string CAPTION_STOP_PRICE = "Stop Price";
        public const string CAPTION_ORIG_CL_ORDER_ID = "OrigClx OrderID";
        public const string CAPTION_PEG_DIFF = "PegDiff";
        public const string CAPTION_DISCR_OFFSET = "Discr Offset";
        public const string CAPTION_ASSET_ID = "AssetID";
        public const string CAPTION_ASSET_NAME = "Asset Class";
        public const string CAPTION_ASSET_CLASS = "Asset Class";
        public const string CAPTION_UNDERLYING_ID = "UnderLyingID";
        public const string CAPTION_UNDERLYING_NAME = "Underlying";
        public const string CAPTION_CANCEL_EVENT = "Cancel All";
        public const string CAPTION_AUEC_ID = "AUECID";
        public const string CAPTION_AUEC_NAME = "AUEC";
        public const string CAPTION_TRANSACTION_TIME = "Transaction Time";
        public const string CAPTION_TRANSACTION_Source = "Transaction Source";
        public const string CAPTION_TIF = "Time In Force";
        public const string CAPTION_HANDLING_INST = "Handling Instructions";
        public const string CAPTION_EXECUTION_INST = "Execution Instructions";
        public const string CAPTION_CURRENCY = "Currency";
        public const string CAPTION_BASECURRENCYPRICE = "BaseCurrencyPrice";
        public const string CAPTION_SHORTEXPOSURE = "ShortExposure";
        public const string CAPTION_LONGEXPOSURE = "LongExposure";
        public const string CAPTION_NETEXPOSURE = "NetExposure";
        public const string CAPTION_PNL = "PNL";
        public const string CAPTION_WORKING_QTY = "Working Qty";
        public const string CAPTION_Prana_MSG_TYPE = "Order Type";
        public const string CAPTION_CLEARANCE_TIME = "CLEARANCE TIME";
        public const string CAPTION_STRIKE_PRICE = "Strike Price";
        public const string CAPTION_PUT_CALL = "Put/Call";
        public const string CAPTION_MATURITY_DAY = "Maturity Day";
        public const string CAPTION_SECURITY_TYPE = "Security Type";
        public const string CAPTION_OPEN_CLOSE = "Open/Close";
        public const string CAPTION_ORDER_SEQUENCE_NO = "OrderFill Sequence No";
        public const string CAPTION_NETAMOUNTWITHCOMMISSION = "Net Amount";
        public const string CAPTION_NOTIONAL_VALUE = "Notional Value";
        public const string CAPTION_NOTIONAL_VALUE_BASE = "NotionalValueBase";
        public const string CAPTION_PERCENTAGE_EXECUTED = "% Executed";
        public const string CAPTION_BENCHMARK_PRICE = "Benchmark Price";
        public const string CAPTION_BENCHMARK_VALUE = "Bm - Value";
        public const string CAPTION_DRIFT_NOTIONAL = "Drift - Notional";
        public const string CAPTION_DRIFT_BASIS_POINT = "Drift - BP";
        public const string CAPTION_LAST_TRADED_PRICE = "LastTraded Price";
        public const string CAPTION_PERCENTAGEOF_BASKET = "% Basket";
        public const string CAPTION_ADV = "ADV";
        public const string CAPTION_EXCHANGE = "Exchange";
        public const string CAPTION_PERCENTAGE_OF_ADV = "%ADV";
        public const string CAPTION_SENDQTY = "SendQty";
        public const string CAPTION_UNSENTQTY = "Reserve Qty";
        public const string CAPTION_BIDPRICE = "BidPrice";
        public const string CAPTION_ASKPRICE = "AskPrice";
        public const string CAPTION_ALLOCATEDQTY = "AllocatedQty";
        public const string CAPTION_UNALLOCATEDQTY = "UnAllocatedQty";
        public const string CAPTION_LEVEL2NAME = "Strategy";
        public const string CAPTION_LEVEL2ID = "StrategyID";
        public const string CAPTION_LEVEL1NAME = "Account";
        public const string CAPTION_LEVEL1ID = "AccountID";
        public const string CAPTION_ALLOCATIONFUND = "Allocation Account";
        public const string CAPTION_COMMISSION = "Commission";
        public const string CAPTION_SOFTCOMMISSION = "Soft Commission";
        public const string CAPTION_FEES = "Fees";
        public const string CAPTION_AUECLOCALDATE = "TradeDate";
        public const string CAPTION_ALLOCATIONDATE = "AllocationDate";
        public const string CAPTION_AVG_FX_RATE = "Avg Fx Rate";
        public const string CAPTION_FX_RATE = "FX Rate";
        public const string CAPTION_FX_CONVERSION_METHOD_OPERATOR = "FX Conversion Operator";
        public const string CAPTION_GROUPID = "GroupID";
        public const string CAPTION_EXPIRATIONDATE = "Expiration Date";
        public const string CAPTION_LONGNAME = "Description";
        public const string CAPTION_PROCESS_DATE = "Process Date";
        public const string CAPTION_TOTALCOMMISSIONANDFEES = "TotalCommissionandFees";
        public const string CAPTION_COMPANYNAME = "Company";
        public const string CAPTION_DESCRIPTION = "Description";
        public const string CAPTION_ALGOSTRATEGYNAME = "Algo";
        public const string CAPTION_LIMITPX = "Limit Price";
        public const string CAPTION_GRID_LAST_SHARES = "Last Shares";
        public const string CAPTION_UNDERLYING_COUNTRY = "Underlying Country";
        public const string CAPTION_BLOOMBERG_SYMBOL = "Bloomberg Symbol";
        public const string CAPTION_BLOOMBERG_SYMBOL_WithExchangeCode = "Bloomberg Symbol(with Exchange Code)";
        public const string CAPTION_ACTIV_SYMBOL = "ACTIV Symbol";
        public const string CAPTION_FACTSET_SYMBOL = "FactSet Symbol";
        public const string CAPTION_INTERNAL_COMMENTS = "Internal Comments";
        public const string CAPTION_CALCULATION_BASIS = "Calculation Basis";
        public const string CAPTION_AVG_FILL_PRICE_BASE = "Avg Fill Price (Base)";
        public const string CAPTION_PEG = "Peg";
        public const string CAPTION_LEAVES = "Leaves";
        public const string CAPTION_UNCOMMITTED_QTY = "Uncommitted Qty";
        public const string CAPTION_CURRENT_USER = "Current User";
        public const string CAPTION_ACTUAL_USER = "Actual User";
        public const string CAPTION_TOTAL_EXECUTED_QTY = "Total Executed Qty";
        public const string CAPTION_ORIGINAL_QTY = "Original Qty";
        public const string CAPTION_AVG_FILL_PRICE_LOCAL = "Avg Fill Price (Local)";
        public const string CAPTION_COUNTER_PARTY = "Counter Party";
        public const string CAPTION_LASTPRICE_BLOTTER = "Last Price";
        public const string CAPTION_NOTIONAL_VALUE_LOCAL = "Notional Value (Local)";
        public const string CAPTION_NOTIONAL_VALUE_BASE_BLOTTER = "Notional Value (Base)";
        public const string CAPTION_TARGET_QUANTITY = "Target Quantity";
        public const string CAPTION_TARGET_QTY = "Target Qty";
        public const string CAPTION_ALLOCATION_STATUS = "Allocation Status";
        public const string CAPTION_ALLOCATION_SCHEME_NAME = "Allocation Scheme Name";
        public const string CAPTION_DAY_AVERAGE_PRICE = "Day Average Price";
        public const string CAPTION_DAY_EXECUTED_QUANTITY = "Day Executed Qty";
        public const string CAPTION_START_OF_DAY_QUANTITY = "Start of Day Qty";
        public const string CAPTION_TRADER = "Trader";
        public const string CAPTION_MASTER_FUND = "Master Fund";
        public const string CAPTION_ACCOUNT = "Account";
        public const string CAPTION_TICKERSYMBOL = "Ticker";
        public const string CAPTION_EXPIRETIME = "Expiry Date";
        public const string CAPTION_CUSIPSYMBOL = "CUSIP";
        public const string CAPTION_SEDOLSYMBOL = "SEDOL";
        public const string CAPTION_BLOOMBERGSYMBOL = "BB Code";
        public const string CAPTION_BLOOMBERGSYMBOL_WithExchangeCode = "Bloomberg Symbol(with Exchange Code)";
        public const string CAPTION_BLOOMBERGSYMBOL_WithCompositeCode = "Bloomberg Symbol(with Composite Code)";
        public const string CAPTION_ISINSYMBOL = "ISIN";
        public const string CAPTION_RICSYMBOL = "RIC";
        public const string CAPTION_OSIOPTIONSYMBOL = "OSI-21";
        public const string CAPTION_IDCOOPTIONSYMBOL = "IDCO-22";
        public const string CAPTION_OPRAOPTIONSYMBOL = "OPRA";
        public const string CAPTION_UNDERLYINGSYMBOL = "Underlying Symbol";
        public const string CAPTION_ACCRUALBASIS = "Accrual Basis";
        public const string CAPTION_COMMISSION_RATE = "Commission Rate";
        public const string CAPTION_COMMISSION_AMT = "Commission Amount";
        public const string CAPTION_CALC_BASIS = "Commission Basis";
        public const string CAPTION_IMPORTED_FILE_NAME = "Imported File Name";
        public const string CAPTION_MULTI_TRADE_NAME = "Multi Trade Name";
        public const string CAPTION_SOFT_COMMISSION_RATE = "Soft Commission Rate";
        public const string CAPTION_SOFT_COMMISSION_AMT = "Soft Commission Amount";
        public const string CAPTION_SOFT_COMMISSION_CALC_BASIS = "Soft Commission Basis";
        public const string CAPTION_STAMPDUTY = "Stamp Duty";
        public const string CAPTION_TRANSACTIONLEVY = "Transaction Levy";
        public const string CAPTION_TAXONCOMMISSIONS = "Tax On Commissions";
        public const string CAPTION_SECFEE = "SEC Fee";
        public const string CAPTION_OCCFEE = "OCC Fee";
        public const string CAPTION_ORFFEE = "ORF Fee";
        public const string CAPTION_CLEARINGFEE = "AUEC Fee1";
        public const string CAPTION_MISCFEES = "AUEC Fee2";
        public const string CAPTION_OPTION_PREMIUM_ADJUSTMENT = "Option Premium Adjustment";
        public const string CAPTION_CLOSING_ALGO = "Closing Method";
        public const string CAPTION_OTHERBROKERFEES = "Other Broker Fees";
        public const string CAPTION_CLEARINGBROKERFEE = "Clearing Broker Fees";
        public const string CAPTION_SOFTCOMMISSIONPERSHARE = "Soft Commission/Share";
        public const string CAPTION_TOTALCOMMISSIONPERSHARE = "Total Commission/Share";
        public const string CAPTION_BORROWERID = "Borrower ID";
        public const string CAPTION_BORROWERRATE = "Borrow Rate";
        public const string CAPTION_BORROWERRATEBPS = "Borrow Rate(BPS)";
        public const string CAPTION_BORROWERRATECENT = "Borrow Rate(%)";
        public const string CAPTION_BORROWERBROKER = "Borrow Broker";
        public const string CAPTION_SETTLEMENT_CURRENCY = "Settlement Currency";
        public const string CAPTION_AVGPRICEBASE = "Avg Price(Base)";
        public const string CAPTION_UNITCOSEBASE = "Unit Cost(Base)";
        public const string CAPTION_TRADE_ATTRIBUTE_1 = "Trade Attribute 1";
        public const string CAPTION_TRADE_ATTRIBUTE_2 = "Trade Attribute 2";
        public const string CAPTION_TRADE_ATTRIBUTE_3 = "Trade Attribute 3";
        public const string CAPTION_TRADE_ATTRIBUTE_4 = "Trade Attribute 4";
        public const string CAPTION_TRADE_ATTRIBUTE_5 = "Trade Attribute 5";
        public const string CAPTION_TRADE_ATTRIBUTE_6 = "Trade Attribute 6";
        public const string CAPTION_TRADE_ATTRIBUTE = "Trade Attribute ";
        public const string CAPTION_FILL = "Fill";
        public const string CAPTION_REBALANCER_FILE_NAME = "Rebalancer File Name";
        public const string CAPTION_EXECUTION_TIME_LAST_FILL = "Execution Time (Last Fill)";
        public const string CAPTION_PERCENTAGE_COMPLETED = "% Completed";
        #endregion

        #region Column Headers Actual names
        /// <summary>
        /// Indexes used while picking up AUEC-CV permissions from DB
        /// </summary>
        public const int DS_INDEX_PERMITTED_AUCECV = 0;
        public const int DS_INDEX_ORDER_SIDE = 1;
        public const int DS_INDEX_ORDER_TYPE = 2;
        public const int DS_INDEX_ORDER_HANDLINGINST = 3;
        public const int DS_INDEX_ORDER_EXECUTIONINST = 4;
        public const int DS_INDEX_ORDER_TIF = 5;

        public const string PROPERTY_CHKBOX = "checkBox";
        public const string PROPERTY_PARENT_CL_ORDERID = "ParentClOrderID";
        public const string PROPERTY_SYMBOL = "Symbol";
        public const string PROPERTY_ORDER_ID = "OrderID";
        public const string PROPERTY_CANCEL_ORDER_ID = "ClOrderID";
        public const string PROPERTY_STAGED_ORDERID = "StagedOrderID";
        public const string PROPERTY_ORDER_STATUS = "OrderStatus";
        public const string PROPERTY_ORDER_STATUS_WITHOUTROLLOVER = "OrderStatusWithoutRollover";
        public const string PROPERTY_ORDER_STATUSTAGVALUE = "OrderStatusTagValue";
        public const string PROPERTY_ORDER_SIDE = "OrderSide";
        public const string PROPERTY_ORDER_SIDETAGVALUE = "OrderSideTagValue";
        public const string PROPERTY_ORDER_SIDEID = "SideID";
        public const string PROPERTY_ORDER_TYPE = "OrderType";
        public const string PROPERTY_ORDER_TYPE_ID = "OrderTypeID";
        public const string PROPERTY_ORDER_TYPETAGVALUE = "OrderTypeTagValue";
        public const string PROPERTY_QUANTITY = "Quantity";
        public const string PROPERTY_QUANTITY_PERCENTAGE = "PercentageQty";
        public const string PROPERTY_SETTLEMENTCURRENCY = "SettlCurrency";
        public const string PROPERTY_SETTLEMENTCURRENCYID = "SettlementCurrencyID";
        public const string PROPERTY_SETTLCURRENCYID = "SettlCurrencyID";
        public const string PROPERTY_SETTLEMENTCURRENCYNAME = "SettlCurrencyName";
        public const string PROPERTY_AVGPRICEBASE = "AvgPriceBase";
        public const string PROPERTY_UNITCOSTBASE = "UnitCostBase";
        public const string PROPERTY_VENUE = "Venue";
        public const string PROPERTY_VENUE_ID = "VenueID";
        public const string PROPERTY_COUNTERPARTY_NAME = "CounterPartyName";
        public const string PROPERTY_COUNTERPARTY_ID = "CounterPartyID";
        public const string PROPERTY_TRADINGACCOUNT_ID = "TradingAccountID";
        public const string PROPERTY_TRADING_ACCOUNT = "TradingAccountName";
        public const string PROPERTY_USER_ID = "CompanyUserID";
        public const string PROPERTY_USER = "CompanyUserName";
        public const string PROPERTY_MODIFIED_USER = "ModifiedUser";
        public const string PROPERTY_CURRENT_USER = "CurrentUser";
        public const string PROPERTY_ACTUAL_USER_ID = "ActualCompanyUserID";
        public const string PROPERTY_ACTUAL_USER = "ActualCompanyUserName";
        public const string PROPERTY_SENDER_SUBID = "SenderSubID";
        public const string PROPERTY_SENDER_COMPID = "SenderCompID";
        public const string PROPERTY_TARGET_COMPID = "TargetCompID";
        public const string PROPERTY_SENDING_TIME = "SendingTime";
        public const string PROPERTY_PRICE = "Price";
        public const string PROPERTY_EXECUTIONID = "ExecID";
        public const string PROPERTY_LASTPRICE = "LastPrice";
        public const string PROPERTY_AVGPRICE = "AvgPrice";
        public const string PROPERTY_LAST_MARKET = "LastMarket";
        public const string PROPERTY_EXECUTED_QTY = "CumQty";
        public const string PROPERTY_LAST_SHARES = "LastShares";
        public const string PROPERTY_LEAVES_QUANTITY = "LeavesQty";
        public const string PROPERTY_REMAINING_QUANTITY = "RemainingQty";
        public const string PROPERTY_TEXT = "Text";
        public const string PROPERTY_CLIENTORDERID = "ClientOrderID";
        public const string PROPERTY_REJECT_REASON = "RejectReason";
        public const string PROPERTY_STOP_PRICE = "StopPrice";
        public const string PROPERTY_ORIG_CL_ORDER_ID = "OrigClOrderID";
        public const string PROPERTY_PEG_DIFF = "PegDifference";
        public const string PROPERTY_ASSET_ID = "AssetID";
        public const string PROPERTY_ASSET_NAME = "AssetName";
        public const string PROPERTY_UNDERLYING_ID = "UnderLyingID";
        public const string PROPERTY_UNDERLYING_NAME = "UnderlyingName";
        public const string PROPERTY_SELECT_ALL = "Select All";
        public const string PROPERTY_CANCEL_EVENT = "Cancel All";
        public const string PROPERTY_AUEC_ID = "AUECID";
        public const string PROPERTY_AUEC_NAME = "AUEC";
        public const string PROPERTY_TRANSACTION_TIME = "TransactionTime";
        public const string PROPERTY_SETTLEMENT_DATE = "SettlementDate";
        public const string PROPERTY_TIF = "TIFText";
        public const string PROPERTY_TIFID = "TimeInForceID";
        public const string PROPERTY_TIF_TAGVALUE = "TIF";
        public const string PROPERTY_Transaction_Source = "TransactionSource";
        public const string PROPERTY_HANDLING_INST = "HandlingInstructionText";
        public const string PROPERTY_HANDLING_INSTID = "HandlingInstructionsID";
        public const string PROPERTY_HANDLING_INST_TagValue = "HandlingInstruction";
        public const string PROPERTY_EXECUTION_INSTID = "ExecutionInstructionID";
        public const string PROPERTY_EXECUTION_INST = "ExecutionInstructionText";
        public const string PROPERTY_EXECUTION_INST_TagValue = "ExecutionInstruction";
        public const string PROPERTY_CURRENCYNAME = "CurrencyName";
        public const string PROPERTY_DESCRIPTION = "Description";
        public const string PROPERTY_BASECURRENCYPRICE = "BaseCurrencyPrice";
        public const string PROPERTY_SHORTEXPOSURE = "ShortExposure";
        public const string PROPERTY_LONGEXPOSURE = "LongExposure";
        public const string PROPERTY_NETEXPOSURE = "NetExposure";
        public const string PROPERTY_PNL = "PNL";
        public const string PROPERTY_PRANA_MSG_TYPE = "PranaMsgType";
        public const string PROPERTY_CLEARANCE_TIME = "CLEARANCE TIME";
        public const string PROPERTY_STRIKE_PRICE = "StrikePrice";
        public const string PROPERTY_PUT_CALL = "PutOrCall";
        public const string PROPERTY_MATURITY_DAY = "MaturityDay";
        public const string PROPERTY_SECURITY_TYPE = "SecurityType";
        public const string PROPERTY_ORDER_SEQUENCE_NO = "OrderSeqNumber";
        public const string PROPERTY_UNSENT_QTY = "UnsentQty";
        public const string PROPERTY_SENDQTY = "SendQty";
        public const string PROPERTY_UNDERLYINGSYMBOL = "UnderlyingSymbol";
        public const string PROPERTY_EXCHANGE = "ExchangeName";
        public const string PROPERTY_EXCHANGEID = "ExchangeID";
        public const string PROPERTY_CURRENCYID = "CurrencyID";
        public const string PROPERTY_LONGNAME = "LongName";
        public const string PROPERTY_SECTOR = "Sector";
        public const string PROPERTY_LEVEL2NAME = "Level2Name";
        public const string PROPERTY_LEVEL2ID = "Level2ID";
        public const string PROPERTY_LEVEL1NAME = "Level1Name";
        public const string PROPERTY_LEVEL1ID = "Level1ID";
        public const string PROPERTY_ALLOCATIONFUND = "AllocationAccount";
        public const string PROPERTY_BIDPRICE = "BidPrice";
        public const string PROPERTY_ASKPRICE = "AskPrice";
        public const string PROPERTY_CMTA = "CMTA";
        public const string PROPERTY_CMTAID = "CMTAID";
        public const string PROPERTY_GIVEUP = "GiveUp";
        public const string PROPERTY_GIVEUPID = "GiveUpID";
        public const string PROPERTY_NOTIONALVALUE = "NotionalValue";
        public const string PROPERTY_NOTIONALVALUEBASE = "NotionalValueBase";
        public const string PROPERTY_ALGOSTRATEGYNAME = "AlgoStrategyName";
        public const string PROPERTY_COMMISSION = "Commission";
        public const string PROPERTY_SOFTCOMMISSION = "SoftCommission";
        public const string PROPERTY_FEES = "Fees";
        public const string PROPERTY_AUECLOCALDATE = "AUECLocalDate";
        public const string PROPERTY_PROCESSDATE = "ProcessDate";
        public const string PROPERTY_ORIGINAL_PURCHASEDATE = "OriginalPurchaseDate";
        public const string PROPERTY_ALLOCATIONDATE = "AllocationDate";
        public const string PROPERTY_MASTERFUNDID = "MasterFundID";
        public const string PROPERTY_MASTERFUND = "MasterFund";
        public const string PROPERTY_AVGFXRATE = "FXRate";
        public const string PROPERTY_FXRATE = "FXRate";
        public const string PROPERTY_FXCONVERSIONMETHODOPERATOR = "FXConversionMethodOperator";
        public const string PROPERTY_ISDEFAULTALLLOCATIONRULE = "IsDefaultAllocationRule";
        public const string PROPERTY_VSCURRENCYID = "VsCurrencyID";
        public const string PROPERTY_LEADCURRENCYID = "LeadCurrencyID";
        public const string PROPERTY_TICKERSYMBOL = "TickerSymbol";
        public const string PROPERTY_REUTERSSYMBOL = "ReutersSymbol";
        public const string PROPERTY_ISINSYMBOL = "ISINSymbol";
        public const string PROPERTY_SEDOLSYMBOL = "SEDOLSymbol";
        public const string PROPERTY_CUSIPSYMBOL = "CusipSymbol";
        public const string PROPERTY_BLOOMBERGSYMBOL = "BloombergSymbol";
        public const string PROPERTY_BLOOMBERGSYMBOLEXCODE = "BloombergSymbolWithExchangeCode";
        public const string PROPERTY_FACTSETSYMBOL = "FactSetSymbol";
        public const string PROPERTY_ACTIVSYMBOL = "ActivSymbol";
        public const string PROPERTY_COMPANYNAME = "CompanyName";
        public const string PROPERTY_OSIOPTIONSYMBOL = "OSISymbol";
        public const string PROPERTY_IDCOOPTIONSYMBOL = "IDCOSymbol";
        public const string PROPERTY_OPRAOPTIONSYMBOL = "OPRAOptionSymbol";
        public const string PROPERTY_MASTERStrategyID = "MasterStrategyID";
        public const string PROPERTY_MASTERSTRATEGY = "MasterStrategy";
        public const string PROPERTY_OTHERBROKERFEES = "OtherBrokerFees";
        public const string PROPERTY_CLEARINGBROKERFEE = "ClearingBrokerFee";
        public const string PROPERTY_TOTALCOMMISSIONANDFEES = "TotalCommissionandFees";
        public const string PROPERTY_STAMPDUTY = "StampDuty";
        public const string PROPERTY_TRANSACTIONLEVY = "TransactionLevy";
        public const string PROPERTY_CLEARINGFEE = "ClearingFee";
        public const string PROPERTY_TAXONCOMMISSIONS = "TaxOnCommissions";
        public const string PROPERTY_MISCFEES = "MiscFees";
        public const string PROPERTY_SECFEE = "SecFee";
        public const string PROPERTY_OCCFEE = "OccFee";
        public const string PROPERTY_ORFFEE = "OrfFee";
        public const string PROPERTY_OPTIONPREMIUMADJUSTMENT = "OptionPremiumAdjustment";
        public const string PROPERTY_CLOSINGALGO = "ClosingAlgo";
        public const string PROPERTY_CLOSINGALGOTEXT = "ClosingAlgoText";
        public const string PROPERTY_ACCRUALBASIS = "AccrualBasis";
        public const string PROPERTY_COMMISSIONRATE = "CommissionRate";
        public const string PROPERTY_COMMISSIONAMT = "CommissionAmt";
        public const string PROPERTY_CALCBASIS = "CalcBasis";
        public const string PROPERTY_TRADEATTRIBUTE1 = "TradeAttribute1";
        public const string PROPERTY_TRADEATTRIBUTE2 = "TradeAttribute2";
        public const string PROPERTY_TRADEATTRIBUTE3 = "TradeAttribute3";
        public const string PROPERTY_TRADEATTRIBUTE4 = "TradeAttribute4";
        public const string PROPERTY_TRADEATTRIBUTE5 = "TradeAttribute5";
        public const string PROPERTY_TRADEATTRIBUTE6 = "TradeAttribute6";
        public const string PROPERTY_TRADEATTRIBUTE = "TradeAttribute";
        public const string PROPERTY_INTERNALCOMMENTS = "InternalComments";
        public const string PROPERTY_IMPORTFILENAME = "ImportFileName";
        public const string PROPERTY_MULTITRADENAME = "MultiTradeName";
        public const string PROPERTY_SOFTCOMMISSIONRATE = "SoftCommissionRate";
        public const string PROPERTY_SOFTCOMMISSIONAMT = "SoftCommissionAmt";
        public const string PROPERTY_SOFTCOMMISSIONCALCBASIS = "SoftCommissionCalcBasis";
        public const string PROPERTY_NETAMOUNTWITHCOMMISSSION = "NetAmountWithComm";
        public const string PROPERTY_SOFTCOMMISSIONPERSHARE = "SoftCommissionPerShare";
        public const string PROPERTY_TOTALCOMMISSIONPERSHARE = "TotalCommissionPerShare";
        public const string PROPERTY_ALLOCATIONSTATUS = "AllocationStatus";
        public const string PROPERTY_DAY_AVERAGE_PRICE = "DayAvgPx";
        public const string PROPERTY_DAY_EXECUTED_QUANTITY = "DayCumQty";
        public const string PROPERTY_START_OF_DAY_QUANTITY = "DayOrderQty";
        public const string PROPERTY_ACCOUNT = "Account";
        public const string PROPERTY_ALLOCATION_SCHEME_NAME = "AllocationSchemeName";
        public const string PROPERTY_PERCENTEXECUTED = "PercentExecuted";
        public const string PROPERTY_PERCENTAGE = "Percentage";
        public const string PROPERTY_UNEXECUTED_QUANTITY = "UnexecutedQuantity";
        public const string PROPERTY_QUANTITY_MTT = "Remaining Quantity";
        public const string PROPERTY_NET_COMMISSION_FEES_LOCAL = "NetCommission/Fees(Local)";
        public const string PROPERTY_NET_COMMISSION_FEES_BASE = "NetCommission/Fees(Base)";
        public const string PROPERTY_STRATEGY = "Strategy";
        public const string PROPERTY_SOURCE = "Source";
        public const string PROPERTY_MULTIPLE = "Multiple";
        public const string PROPERTY_DASH = "-";
        public const string PROPERTY_BORROWERBROKER = "BorrowerBroker";
        public const string PROPERTY_BORROWERID = "BorrowerID";
        public const string PROPERTY_ShortRebate = "ShortRebate";
        public const string PROPERTY_EXECTYPE = "ExecType";
        public const string PROPERTY_IMPORTSTATUS = "ImportStatus";
        public const string PROPERTY_EXPIRATIONDATE = "ExpirationDate";
        public const string PROPERTY_PST_Allocation_Preference_ID = "OriginalAllocationPreferenceID";
        public const string PROPERTY_CounterCurrencyAmount = "CounterCurrencyAmount";
        public const string PROPERTY_CounterCurrency = "CounterCurrency";
        public const string PROPERTY_REBALANCER_FILE_NAME = "RebalancerFileName";
        public const string PROPERTY_EXECUTION_TIME_LAST_FILL = "ExecutionTimeLastFill";
        public const string PROPERTY_PERCENT_COMPLETED = "PercentCompleted";
        public const string PROPERTY_EXPIRETIME = "ExpireTime";
        #endregion

        #region need to move on respective modules
        private static ArrayList blotterGridColumnList = null;
        private static Dictionary<string, string> _columnsForMultiTradingTicket = null;
        private static Dictionary<string, string> _editableColumnsForMultiTradingTicket = null;
        private static Dictionary<string, string> _editableDefaultVisibleForMultiTradingTicket = null;
        private static List<string> basketOrderColumns = null;
        private static List<string> basketFillReportColumns = null;
        private static List<string> basketPostTradeColumns = null;
        private static List<string> basketPreTradeColumns = null;
        private static List<string> basketUpLoadnecessaryColumns = null;
        private static Dictionary<string, string> _dictDisplayableBasketColumns;
        private static ArrayList _importTradesColumnList = null;

        public enum BlotterTypes
        {
            Orders,
            WorkingSubs,
            Summary,
            SubOrders,
            DynamicTab,
            DynamicTabOrders
        }

        public enum PranaMsgTypes
        {
            InternalOrder = 0,
            ORDNewIndependent = 1,
            ORDNewSub = 2,
            ORDStaged = 3,
            ORDManual = 4,
            ORDManualSub = 5,
            ORDCancelStaged = 6,
            MsgList = 7,
            MsgTransferUser = 8,
            MsgDropCopy = 9,
            MsgDropCopy_PM = 10,
            BasketOrder = 11,
            CreatePosition = 12,
            ImportPosition = 13,
            ORDNewSubChild= 14
        }

        public static ArrayList DisplayableBlotterColumnList
        {
            get
            {
                if (blotterGridColumnList == null)
                {
                    blotterGridColumnList = new ArrayList();
                    blotterGridColumnList.Add(PROPERTY_PARENT_CL_ORDERID);
                    blotterGridColumnList.Add(PROPERTY_SYMBOL);
                    blotterGridColumnList.Add(PROPERTY_ORDER_SIDE);
                    blotterGridColumnList.Add(PROPERTY_ORDER_TYPE);
                    blotterGridColumnList.Add(PROPERTY_QUANTITY);
                    blotterGridColumnList.Add(PROPERTY_PRICE);
                    blotterGridColumnList.Add(PROPERTY_EXECUTED_QTY);
                    blotterGridColumnList.Add(PROPERTY_AVGPRICE);
                    blotterGridColumnList.Add(PROPERTY_ORDER_STATUS);
                    blotterGridColumnList.Add(PROPERTY_ORDER_STATUS_WITHOUTROLLOVER);
                    blotterGridColumnList.Add(PROPERTY_LASTPRICE);
                    blotterGridColumnList.Add(PROPERTY_LAST_SHARES);
                    blotterGridColumnList.Add(PROPERTY_LEAVES_QUANTITY);
                    blotterGridColumnList.Add(PROPERTY_COUNTERPARTY_NAME);
                    blotterGridColumnList.Add(PROPERTY_VENUE);
                    blotterGridColumnList.Add(PROPERTY_ORDER_ID);
                    blotterGridColumnList.Add(PROPERTY_USER);
                    blotterGridColumnList.Add(PROPERTY_TRADING_ACCOUNT);
                    blotterGridColumnList.Add(PROPERTY_STOP_PRICE);
                    blotterGridColumnList.Add(PROPERTY_PEG_DIFF);
                    blotterGridColumnList.Add(PROPERTY_NOTIONALVALUE);
                    blotterGridColumnList.Add(PROPERTY_NOTIONALVALUEBASE);
                    blotterGridColumnList.Add(PROPERTY_ASSET_NAME);
                    blotterGridColumnList.Add(PROPERTY_UNDERLYING_NAME);
                    blotterGridColumnList.Add(PROPERTY_AUEC_NAME);
                    blotterGridColumnList.Add(PROPERTY_STRIKE_PRICE);
                    blotterGridColumnList.Add(PROPERTY_PUT_CALL);
                    blotterGridColumnList.Add(PROPERTY_TRANSACTION_TIME);
                    blotterGridColumnList.Add(PROPERTY_UNSENT_QTY);
                    blotterGridColumnList.Add(PROPERTY_UNDERLYINGSYMBOL);
                    blotterGridColumnList.Add(PROPERTY_ALGOSTRATEGYNAME);
                    blotterGridColumnList.Add(PROPERTY_AVGFXRATE);
                    blotterGridColumnList.Add(PROPERTY_PROCESSDATE);
                    blotterGridColumnList.Add(PROPERTY_COMMISSIONAMT);
                    blotterGridColumnList.Add(PROPERTY_COMMISSIONRATE);
                    blotterGridColumnList.Add(PROPERTY_CALCBASIS);
                    blotterGridColumnList.Add(PROPERTY_IMPORTFILENAME);
                    blotterGridColumnList.Add(PROPERTY_BLOOMBERGSYMBOL);
                    blotterGridColumnList.Add(PROPERTY_BLOOMBERGSYMBOLEXCODE);
                    blotterGridColumnList.Add(PROPERTY_ACTIVSYMBOL);
                    blotterGridColumnList.Add(PROPERTY_FACTSETSYMBOL);
                    blotterGridColumnList.Add(PROPERTY_SEDOLSYMBOL);
                    blotterGridColumnList.Add(PROPERTY_COMPANYNAME);
                    blotterGridColumnList.Add(PROPERTY_TRADEATTRIBUTE1);
                    blotterGridColumnList.Add(PROPERTY_TRADEATTRIBUTE2);
                    blotterGridColumnList.Add(PROPERTY_TRADEATTRIBUTE3);
                    blotterGridColumnList.Add(PROPERTY_TRADEATTRIBUTE4);
                    blotterGridColumnList.Add(PROPERTY_TRADEATTRIBUTE5);
                    blotterGridColumnList.Add(PROPERTY_TRADEATTRIBUTE6);
                    for (int i = 7; i <= 45; i++)
                    {
                        blotterGridColumnList.Add(PROPERTY_TRADEATTRIBUTE + i);
                    }
                    blotterGridColumnList.Add(PROPERTY_INTERNALCOMMENTS);
                    blotterGridColumnList.Add(PROPERTY_AVGPRICEBASE);
                    blotterGridColumnList.Add(PROPERTY_ALLOCATIONSTATUS);
                    blotterGridColumnList.Add(PROPERTY_ACCOUNT);
                    blotterGridColumnList.Add(PROPERTY_ALLOCATION_SCHEME_NAME);
                    blotterGridColumnList.Add(PROPERTY_PERCENTEXECUTED);
                    blotterGridColumnList.Add(PROPERTY_MASTERFUND);
                    blotterGridColumnList.Add(PROPERTY_DAY_AVERAGE_PRICE);
                    blotterGridColumnList.Add(PROPERTY_DAY_EXECUTED_QUANTITY);
                    blotterGridColumnList.Add(PROPERTY_START_OF_DAY_QUANTITY);
                    blotterGridColumnList.Add(PROPERTY_STRATEGY);
                    blotterGridColumnList.Add(PROPERTY_UNEXECUTED_QUANTITY);
                    blotterGridColumnList.Add(PROPERTY_NET_COMMISSION_FEES_LOCAL);
                    blotterGridColumnList.Add(PROPERTY_NET_COMMISSION_FEES_BASE);
                    blotterGridColumnList.Add(PROPERTY_SOURCE);
                    blotterGridColumnList.Add(PROPERTY_SETTLEMENTCURRENCYID);
                    blotterGridColumnList.Add(PROPERTY_FXRATE);
                    blotterGridColumnList.Add(PROPERTY_FXCONVERSIONMETHODOPERATOR);
                    blotterGridColumnList.Add(PROPERTY_TIF_TAGVALUE);
                    blotterGridColumnList.Add(PROPERTY_Transaction_Source);
                    blotterGridColumnList.Add(PROPERTY_CounterCurrency);

                    blotterGridColumnList.Add(PROPERTY_CounterCurrencyAmount);
                    blotterGridColumnList.Add(PROPERTY_REBALANCER_FILE_NAME);
                    blotterGridColumnList.Add(PROPERTY_ACTUAL_USER);
                    blotterGridColumnList.Add(PROPERTY_BORROWERBROKER);
                    blotterGridColumnList.Add(PROPERTY_BORROWERID);
                    blotterGridColumnList.Add(PROPERTY_ShortRebate);
                    blotterGridColumnList.Add(PROPERTY_EXECUTION_TIME_LAST_FILL);
                    blotterGridColumnList.Add(PROPERTY_PERCENT_COMPLETED);
                    blotterGridColumnList.Add(PROPERTY_EXPIRETIME);
                }
                return blotterGridColumnList;
            }
        }

        public static Dictionary<string, string> DisplayableColumnsForMultiTradingTicket
        {
            get
            {
                //When adding columns here please specify in the multiple trades trading ticket for the control which should be binded to the ticket, or if the column is readonly
                if (_columnsForMultiTradingTicket == null)
                {
                    _columnsForMultiTradingTicket = new Dictionary<string, string>();
                    _columnsForMultiTradingTicket.Add(PROPERTY_ASKPRICE, CAPTION_ASKPRICE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_ASSET_NAME, CAPTION_ASSET_NAME);
                    _columnsForMultiTradingTicket.Add(PROPERTY_AUEC_NAME, CAPTION_AUEC_NAME);
                    _columnsForMultiTradingTicket.Add(PROPERTY_AUECLOCALDATE, CAPTION_AUECLOCALDATE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_AVGPRICE, CAPTION_AVGPRICE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_BIDPRICE, CAPTION_BIDPRICE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_BLOOMBERGSYMBOL, CAPTION_BLOOMBERGSYMBOL);
                    _columnsForMultiTradingTicket.Add(PROPERTY_FACTSETSYMBOL, CAPTION_FACTSET_SYMBOL);
                    _columnsForMultiTradingTicket.Add(PROPERTY_ACTIVSYMBOL, CAPTION_ACTIV_SYMBOL);
                    _columnsForMultiTradingTicket.Add(PROPERTY_COMMISSIONRATE, CAPTION_COMMISSION_RATE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_CALCBASIS, CAPTION_CALC_BASIS);
                    _columnsForMultiTradingTicket.Add(PROPERTY_COUNTERPARTY_NAME, CAPTION_COUNTERPARTY_NAME);
                    _columnsForMultiTradingTicket.Add(PROPERTY_CURRENCYNAME, CAPTION_CURRENCY);
                    _columnsForMultiTradingTicket.Add(PROPERTY_CUSIPSYMBOL, CAPTION_CUSIPSYMBOL);
                    _columnsForMultiTradingTicket.Add(PROPERTY_DESCRIPTION, CAPTION_DESCRIPTION);
                    _columnsForMultiTradingTicket.Add(PROPERTY_EXECUTION_INST_TagValue, CAPTION_EXECUTION_INST);
                    _columnsForMultiTradingTicket.Add(PROPERTY_FXRATE, CAPTION_FX_RATE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_HANDLING_INST_TagValue, CAPTION_HANDLING_INST);
                    _columnsForMultiTradingTicket.Add(PROPERTY_IDCOOPTIONSYMBOL, CAPTION_IDCOOPTIONSYMBOL);
                    _columnsForMultiTradingTicket.Add(PROPERTY_IMPORTFILENAME, CAPTION_IMPORTED_FILE_NAME);
                    _columnsForMultiTradingTicket.Add(PROPERTY_ISINSYMBOL, CAPTION_ISINSYMBOL);
                    _columnsForMultiTradingTicket.Add(PROPERTY_LEVEL1ID, CAPTION_LEVEL1NAME);
                    _columnsForMultiTradingTicket.Add(PROPERTY_LEVEL2ID, CAPTION_LEVEL2NAME);
                    _columnsForMultiTradingTicket.Add(PROPERTY_NOTIONALVALUE, CAPTION_NOTIONAL_VALUE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_OPRAOPTIONSYMBOL, CAPTION_OPRAOPTIONSYMBOL);
                    _columnsForMultiTradingTicket.Add(PROPERTY_ORDER_SIDE, CAPTION_ORDER_SIDE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_ORDER_TYPE, CAPTION_ORDER_TYPE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_OSIOPTIONSYMBOL, CAPTION_OSIOPTIONSYMBOL);
                    _columnsForMultiTradingTicket.Add(PROPERTY_REUTERSSYMBOL, CAPTION_RICSYMBOL);
                    _columnsForMultiTradingTicket.Add(PROPERTY_SEDOLSYMBOL, CAPTION_SEDOLSYMBOL);
                    _columnsForMultiTradingTicket.Add(PROPERTY_STOP_PRICE, CAPTION_STOP_PRICE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_STRIKE_PRICE, CAPTION_STRIKE_PRICE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_SYMBOL, CAPTION_SYMBOL);
                    _columnsForMultiTradingTicket.Add(PROPERTY_TICKERSYMBOL, CAPTION_TICKERSYMBOL);
                    _columnsForMultiTradingTicket.Add(PROPERTY_TIF_TAGVALUE, CAPTION_TIF);
                    _columnsForMultiTradingTicket.Add(PROPERTY_TOTALCOMMISSIONANDFEES, CAPTION_TOTALCOMMISSIONANDFEES);
                    _columnsForMultiTradingTicket.Add(PROPERTY_TRADING_ACCOUNT, CAPTION_TRADER);
                    _columnsForMultiTradingTicket.Add(PROPERTY_USER, CAPTION_USER);
                    _columnsForMultiTradingTicket.Add(PROPERTY_VENUE, CAPTION_VENUE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_SOFTCOMMISSIONRATE, CAPTION_SOFT_COMMISSION_RATE);
                    _columnsForMultiTradingTicket.Add(PROPERTY_SOFTCOMMISSIONCALCBASIS, CAPTION_SOFT_COMMISSION_CALC_BASIS);
                }
                return _columnsForMultiTradingTicket;
            }
        }

        public static Dictionary<string, string> EditableColumnsForMultitrade
        {
            get
            {
                if (_editableColumnsForMultiTradingTicket == null)
                {
                    _editableColumnsForMultiTradingTicket = new Dictionary<string, string>();
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_COMMISSIONRATE, CAPTION_COMMISSION_RATE);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_CALCBASIS, CAPTION_CALC_BASIS);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_COUNTERPARTY_NAME, CAPTION_COUNTERPARTY_NAME);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_DESCRIPTION, CAPTION_DESCRIPTION);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_EXECUTION_INST_TagValue, CAPTION_EXECUTION_INST);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_FXRATE, CAPTION_FX_RATE);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_HANDLING_INST_TagValue, CAPTION_HANDLING_INST);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_LEVEL1ID, CAPTION_LEVEL1NAME);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_LEVEL2ID, CAPTION_LEVEL2NAME);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_ORDER_TYPE, CAPTION_ORDER_TYPE);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_ORDER_SIDE, CAPTION_ORDER_SIDE);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_TIF_TAGVALUE, CAPTION_TIF);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_TRADING_ACCOUNT, CAPTION_TRADING_ACCOUNT);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_VENUE, CAPTION_VENUE);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_SOFTCOMMISSIONRATE, CAPTION_SOFT_COMMISSION_RATE);
                    _editableColumnsForMultiTradingTicket.Add(PROPERTY_SOFTCOMMISSIONCALCBASIS, CAPTION_SOFT_COMMISSION_CALC_BASIS);
                }
                return _editableColumnsForMultiTradingTicket;
            }
        }

        public static Dictionary<string, string> EditableDefaultVisisbleForMultiTrade
        {
            get
            {
                if (_editableDefaultVisibleForMultiTradingTicket == null)
                {
                    _editableDefaultVisibleForMultiTradingTicket = new Dictionary<string, string>();
                    _editableDefaultVisibleForMultiTradingTicket.Add(PROPERTY_COUNTERPARTY_NAME, CAPTION_COUNTERPARTY_NAME);
                    _editableDefaultVisibleForMultiTradingTicket.Add(PROPERTY_ORDER_TYPE, CAPTION_ORDER_TYPE);
                    _editableDefaultVisibleForMultiTradingTicket.Add(PROPERTY_VENUE, CAPTION_VENUE);
                }
                return _editableDefaultVisibleForMultiTradingTicket;
            }
        }

        public enum ServerDisplayColumns
        {
            CounterPartyName,
            Venue,
            Symbol,
            OrderSide,
            OrderType,
            ClOrderID,
            OrderID,
            TradingAccountName,
            Quantity,
            Price,
            AvgPrice,
            CumQty,
            LeavesQty,
            LastShares,
            ListID,
            PranaMsgType,
            MsgType,
            OrderStatus,
            SendingTime
        }

        #region Basket Trading Display Columns
        public static Dictionary<string, string> ColumnNameHeaderCollection
        {
            get
            {
                if (_dictDisplayableBasketColumns == null)
                {
                    _dictDisplayableBasketColumns = new Dictionary<string, string>();
                    _dictDisplayableBasketColumns.Add(CAPTION_ASSET_NAME, PROPERTY_ASSET_NAME);
                    _dictDisplayableBasketColumns.Add(CAPTION_AVGPRICE, PROPERTY_AVGPRICE);
                    _dictDisplayableBasketColumns.Add(CAPTION_COUNTERPARTY_NAME, PROPERTY_COUNTERPARTY_NAME);
                    _dictDisplayableBasketColumns.Add(CAPTION_EXECUTED_QTY, PROPERTY_EXECUTED_QTY);
                    _dictDisplayableBasketColumns.Add(CAPTION_HANDLING_INST, PROPERTY_HANDLING_INST);
                    _dictDisplayableBasketColumns.Add(CAPTION_EXECUTION_INST, PROPERTY_EXECUTION_INST);
                    _dictDisplayableBasketColumns.Add(CAPTION_LAST_MARKET, PROPERTY_LAST_MARKET);
                    _dictDisplayableBasketColumns.Add(CAPTION_LAST_SHARES, PROPERTY_LAST_SHARES);
                    _dictDisplayableBasketColumns.Add(CAPTION_LASTPRICE, PROPERTY_LASTPRICE);
                    _dictDisplayableBasketColumns.Add(CAPTION_CANCEL_ORDER_ID, PROPERTY_CANCEL_ORDER_ID);
                    _dictDisplayableBasketColumns.Add(CAPTION_PARENT_CL_ORDERID, PROPERTY_PARENT_CL_ORDERID);
                    _dictDisplayableBasketColumns.Add(CAPTION_LEAVES_QUANTITY, PROPERTY_LEAVES_QUANTITY);
                    _dictDisplayableBasketColumns.Add(CAPTION_ORDER_SIDE, PROPERTY_ORDER_SIDE);
                    _dictDisplayableBasketColumns.Add(CAPTION_ORDER_STATUS, PROPERTY_ORDER_STATUS);
                    _dictDisplayableBasketColumns.Add(CAPTION_ORDER_TYPE, PROPERTY_ORDER_TYPE);
                    _dictDisplayableBasketColumns.Add(CAPTION_PRICE, PROPERTY_PRICE);
                    _dictDisplayableBasketColumns.Add(CAPTION_QUANTITY, PROPERTY_QUANTITY);
                    _dictDisplayableBasketColumns.Add(CAPTION_SYMBOL, PROPERTY_SYMBOL);
                    _dictDisplayableBasketColumns.Add(CAPTION_TARGET_COMPID, PROPERTY_TARGET_COMPID);
                    _dictDisplayableBasketColumns.Add(CAPTION_TEXT, PROPERTY_TEXT);
                    _dictDisplayableBasketColumns.Add(CAPTION_TIF, PROPERTY_TIF);
                    _dictDisplayableBasketColumns.Add(CAPTION_TRADING_ACCOUNT, PROPERTY_TRADING_ACCOUNT);
                    _dictDisplayableBasketColumns.Add(CAPTION_UNDERLYING_NAME, PROPERTY_UNDERLYING_NAME);
                    _dictDisplayableBasketColumns.Add(CAPTION_USER, PROPERTY_USER);
                    _dictDisplayableBasketColumns.Add(CAPTION_VENUE, PROPERTY_VENUE);
                    _dictDisplayableBasketColumns.Add(CAPTION_STOP_PRICE, PROPERTY_STOP_PRICE);
                    _dictDisplayableBasketColumns.Add(CAPTION_SENDQTY, PROPERTY_SENDQTY);
                    _dictDisplayableBasketColumns.Add(CAPTION_UNSENTQTY, PROPERTY_UNSENT_QTY);
                    _dictDisplayableBasketColumns.Add(CAPTION_NOTIONAL_VALUE, CAPTION_NOTIONAL_VALUE);
                    _dictDisplayableBasketColumns.Add(CAPTION_NOTIONAL_VALUE_BASE, CAPTION_NOTIONAL_VALUE_BASE);
                    _dictDisplayableBasketColumns.Add(CAPTION_PERCENTAGE_EXECUTED, CAPTION_PERCENTAGE_EXECUTED);
                    _dictDisplayableBasketColumns.Add(CAPTION_BENCHMARK_PRICE, CAPTION_BENCHMARK_PRICE);
                    _dictDisplayableBasketColumns.Add(CAPTION_BENCHMARK_VALUE, CAPTION_BENCHMARK_VALUE);
                    _dictDisplayableBasketColumns.Add(CAPTION_DRIFT_NOTIONAL, CAPTION_DRIFT_NOTIONAL);
                    _dictDisplayableBasketColumns.Add(CAPTION_DRIFT_BASIS_POINT, CAPTION_DRIFT_BASIS_POINT);
                    _dictDisplayableBasketColumns.Add(CAPTION_PERCENTAGEOF_BASKET, CAPTION_PERCENTAGEOF_BASKET);
                    _dictDisplayableBasketColumns.Add(CAPTION_ADV, CAPTION_ADV);
                    _dictDisplayableBasketColumns.Add(CAPTION_PERCENTAGE_OF_ADV, CAPTION_PERCENTAGE_OF_ADV);
                    _dictDisplayableBasketColumns.Add(CAPTION_LAST_TRADED_PRICE, CAPTION_LAST_TRADED_PRICE);
                    _dictDisplayableBasketColumns.Add(CAPTION_ASKPRICE, PROPERTY_ASKPRICE);
                    _dictDisplayableBasketColumns.Add(CAPTION_BIDPRICE, PROPERTY_BIDPRICE);
                    _dictDisplayableBasketColumns.Add(CAPTION_QUANTITY_PERCENTAGE, PROPERTY_QUANTITY_PERCENTAGE);
                    _dictDisplayableBasketColumns.Add(CAPTION_EXCHANGE, PROPERTY_EXCHANGE);
                    _dictDisplayableBasketColumns.Add(CAPTION_LEVEL2NAME, PROPERTY_LEVEL2NAME);
                    _dictDisplayableBasketColumns.Add(CAPTION_LEVEL1NAME, PROPERTY_LEVEL1NAME);
                    _dictDisplayableBasketColumns.Add(CAPTION_ALLOCATIONFUND, PROPERTY_ALLOCATIONFUND);
                    _dictDisplayableBasketColumns.Add(CAPTION_COMMISSION, PROPERTY_COMMISSION);
                    _dictDisplayableBasketColumns.Add(CAPTION_SOFTCOMMISSION, PROPERTY_SOFTCOMMISSION);
                    _dictDisplayableBasketColumns.Add(CAPTION_FEES, PROPERTY_FEES);
                    _dictDisplayableBasketColumns.Add(CAPTION_AUECLOCALDATE, PROPERTY_AUECLOCALDATE);
                    _dictDisplayableBasketColumns.Add(CAPTION_PROCESS_DATE, PROPERTY_PROCESSDATE);
                    _dictDisplayableBasketColumns.Add(CAPTION_NETAMOUNTWITHCOMMISSION, PROPERTY_NETAMOUNTWITHCOMMISSSION);
                    _dictDisplayableBasketColumns.Add(CAPTION_BLOOMBERGSYMBOL, PROPERTY_BLOOMBERGSYMBOL);
                    _dictDisplayableBasketColumns.Add(CAPTION_AVGPRICEBASE, PROPERTY_AVGPRICEBASE);
                    _dictDisplayableBasketColumns.Add(CAPTION_SEDOLSYMBOL, PROPERTY_SEDOLSYMBOL);
                    _dictDisplayableBasketColumns.Add(CAPTION_COMPANYNAME, PROPERTY_COMPANYNAME);
                    _dictDisplayableBasketColumns.Add(CAPTION_EXECUTION_TIME_LAST_FILL, PROPERTY_EXECUTION_TIME_LAST_FILL);
                    _dictDisplayableBasketColumns.Add(CAPTION_PERCENTAGE_COMPLETED, PROPERTY_PERCENT_COMPLETED);
                }
                return _dictDisplayableBasketColumns;
            }
        }

        public static List<string> BasketDisplayAbleColumnList
        {
            get
            {
                if (basketOrderColumns == null)
                {
                    basketOrderColumns = new List<string>();
                    basketOrderColumns.Add(CAPTION_SYMBOL);
                    basketOrderColumns.Add(CAPTION_ORDER_SIDE);
                    basketOrderColumns.Add(CAPTION_ORDER_TYPE);
                    basketOrderColumns.Add(CAPTION_COUNTERPARTY_NAME);
                    basketOrderColumns.Add(CAPTION_VENUE);
                    basketOrderColumns.Add(CAPTION_QUANTITY);
                    basketOrderColumns.Add(CAPTION_PRICE);
                    basketOrderColumns.Add(CAPTION_LEVEL2NAME);
                    basketOrderColumns.Add(CAPTION_LEVEL1NAME);
                    basketOrderColumns.Add(CAPTION_CANCEL_ORDER_ID);
                    basketOrderColumns.Add(CAPTION_PARENT_CL_ORDERID);
                    basketOrderColumns.Add(CAPTION_EXECUTED_QTY);
                    basketOrderColumns.Add(CAPTION_AVGPRICE);
                    basketOrderColumns.Add(CAPTION_ORDER_STATUS);
                    basketOrderColumns.Add(CAPTION_LASTPRICE);
                    basketOrderColumns.Add(CAPTION_LAST_SHARES);
                    basketOrderColumns.Add(CAPTION_LEAVES_QUANTITY);
                    basketOrderColumns.Add(CAPTION_QUANTITY_PERCENTAGE);
                    basketOrderColumns.Add(CAPTION_USER);
                    basketOrderColumns.Add(CAPTION_TRADING_ACCOUNT);
                    basketOrderColumns.Add(CAPTION_BIDPRICE);
                    basketOrderColumns.Add(CAPTION_ASKPRICE);
                    basketOrderColumns.Add(CAPTION_STOP_PRICE);
                    basketOrderColumns.Add(CAPTION_HANDLING_INST);
                    basketOrderColumns.Add(CAPTION_EXECUTION_INST);
                    basketOrderColumns.Add(CAPTION_TIF);
                    basketOrderColumns.Add(CAPTION_SENDQTY);
                    basketOrderColumns.Add(CAPTION_UNSENTQTY);
                    basketOrderColumns.Add(CAPTION_AUECLOCALDATE);
                    basketOrderColumns.Add(CAPTION_COMMISSION);
                    basketOrderColumns.Add(CAPTION_FEES);
                }
                return basketOrderColumns;
            }
        }

        /// <summary>
        /// Post Trade Columns
        /// </summary>
        public static List<string> BasketPostTradeColumns
        {
            get
            {
                if (basketPostTradeColumns == null)
                {
                    basketPostTradeColumns = new List<string>();
                    basketPostTradeColumns.Add(CAPTION_SYMBOL);
                    basketPostTradeColumns.Add(CAPTION_ORDER_SIDE);
                    basketPostTradeColumns.Add(CAPTION_QUANTITY);
                    basketPostTradeColumns.Add(CAPTION_EXECUTED_QTY);
                    basketPostTradeColumns.Add(CAPTION_AVGPRICE);
                    basketPostTradeColumns.Add(CAPTION_NOTIONAL_VALUE);
                    basketPostTradeColumns.Add(CAPTION_PERCENTAGE_EXECUTED);
                    basketPostTradeColumns.Add(CAPTION_BENCHMARK_PRICE);
                    basketPostTradeColumns.Add(CAPTION_BENCHMARK_VALUE);
                    basketPostTradeColumns.Add(CAPTION_DRIFT_NOTIONAL);
                    basketPostTradeColumns.Add(CAPTION_DRIFT_BASIS_POINT);
                    basketPreTradeColumns.Add(CAPTION_AUECLOCALDATE);
                    basketPostTradeColumns.Add(CAPTION_COMMISSION);
                    basketPostTradeColumns.Add(CAPTION_FEES);
                }
                return basketPostTradeColumns;
            }
        }

        /// <summary>
        /// Basket Pre TradeColumns
        /// </summary>
        public static List<string> BasketPreTradeColumns
        {
            get
            {
                if (basketPreTradeColumns == null)
                {
                    basketPreTradeColumns = new List<string>();
                    basketPreTradeColumns.Add(CAPTION_SYMBOL);
                    basketPreTradeColumns.Add(CAPTION_QUANTITY);
                    basketPreTradeColumns.Add(CAPTION_LAST_TRADED_PRICE);
                    basketPreTradeColumns.Add(CAPTION_NOTIONAL_VALUE);
                    basketPreTradeColumns.Add(CAPTION_PERCENTAGEOF_BASKET);
                    basketPreTradeColumns.Add(CAPTION_ADV);
                    basketPreTradeColumns.Add(CAPTION_PERCENTAGE_OF_ADV);
                    basketPreTradeColumns.Add(CAPTION_AUECLOCALDATE);
                }
                return basketPreTradeColumns;
            }
        }

        /// <summary>
        /// Necessary Columns
        /// </summary>
        public static List<string> BasketUpLoadNecessaryColumnList
        {
            get
            {
                if (basketUpLoadnecessaryColumns == null)
                {
                    basketUpLoadnecessaryColumns = new List<string>();
                    basketUpLoadnecessaryColumns.Add(CAPTION_SYMBOL);
                    basketUpLoadnecessaryColumns.Add(CAPTION_QUANTITY);
                }
                return basketUpLoadnecessaryColumns;
            }
        }

        public static List<string> BasketFillReportColumn
        {
            get
            {
                if (basketFillReportColumns == null)
                {
                    basketFillReportColumns = new List<string>();
                    basketFillReportColumns.Add(CAPTION_SYMBOL);
                    basketFillReportColumns.Add(CAPTION_ORDER_SIDE);
                    basketFillReportColumns.Add(CAPTION_ORDER_TYPE);
                    basketFillReportColumns.Add(CAPTION_PRICE);
                    basketFillReportColumns.Add(CAPTION_EXECUTED_QTY);
                    basketFillReportColumns.Add(CAPTION_AVGPRICE);
                    basketFillReportColumns.Add(CAPTION_ORDER_STATUS);
                    basketFillReportColumns.Add(CAPTION_LASTPRICE);
                    basketFillReportColumns.Add(CAPTION_LAST_SHARES);
                    basketFillReportColumns.Add(CAPTION_LEAVES_QUANTITY);
                    basketFillReportColumns.Add(CAPTION_TEXT);
                    basketFillReportColumns.Add(CAPTION_COMMISSION);
                    basketFillReportColumns.Add(CAPTION_FEES);
                }
                return basketFillReportColumns;
            }
        }
        #endregion

        public enum CounterPartyUpGridGridColumns
        {
            Symbol,
            OrderSide,
            OrderType,
            CounterPartyName,
            Venue,
            Price,
            StopPrice,
            Quantity,
            ClOrderID,
            Text
        }

        public static ArrayList ImportTradeColumnList
        {
            get
            {
                if (_importTradesColumnList == null)
                {
                    _importTradesColumnList = new ArrayList();
                    _importTradesColumnList.Add(PROPERTY_SYMBOL);
                    _importTradesColumnList.Add(PROPERTY_ORDER_SIDE);
                    _importTradesColumnList.Add(PROPERTY_QUANTITY);
                    _importTradesColumnList.Add(PROPERTY_PRICE);
                    _importTradesColumnList.Add(PROPERTY_LASTPRICE);
                    _importTradesColumnList.Add(PROPERTY_LAST_SHARES);
                    _importTradesColumnList.Add(PROPERTY_COUNTERPARTY_NAME);
                    _importTradesColumnList.Add(PROPERTY_VENUE);
                    _importTradesColumnList.Add(PROPERTY_ORDER_ID);
                    _importTradesColumnList.Add(CAPTION_LAST_MARKET);
                    _importTradesColumnList.Add(PROPERTY_TRADING_ACCOUNT);
                    _importTradesColumnList.Add(PROPERTY_TEXT);
                    _importTradesColumnList.Add(PROPERTY_ASSET_NAME);
                    _importTradesColumnList.Add(PROPERTY_UNDERLYING_NAME);
                    _importTradesColumnList.Add(PROPERTY_AUEC_NAME);
                    _importTradesColumnList.Add(PROPERTY_TRANSACTION_TIME);
                    _importTradesColumnList.Add(PROPERTY_SENDING_TIME);
                }
                return _importTradesColumnList;
            }
        }
        #endregion

        #region ExPNL constants
        public const string CONST_DayPnLLong = "DayPnLLong";
        public const string CONST_DayPnLShort = "DayPnLShort";
        public const string CONST_DayPnL = "DayPnL";
        public const string CONST_LongExposure = "LongExposure";
        public const string CONST_ShortExposure = "ShortExposure";
        public const string CONST_NetExposure = "NetExposure";
        public const string CONST_NetAssetValue = "NetAssetValue";
        public const string CONST_LongNotionalValue = "LongNotionalValue";
        public const string CONST_ShortNotionalValue = "ShortNotionalValue";
        public const string CONST_NetNotionalValue = "NetNotionalValue";
        public const string CONST_Level1ID = "Level1ID";
        public const string CONST_PNLContributionPercentageSummary = "PNLContributionPercentageSummary";
        public const string CONST_LongMarketValue = "LongMarketValue";
        public const string CONST_ShortMarketValue = "ShortMarketValue";
        public const string CONST_NetMarketValue = "NetMarketValue";
        public const string CONST_CashProjected = "CashProjected";
        public const string CONST_NetPosition = "NetPosition";
        public const string CONST_PositionSide = "PositionSide";
        public const string CONST_NAVString = "NAVString";
        public const string CONST_LongPercentExposure = "LongPercentExposure";
        public const string CONST_ShortPercentExposure = "ShortPercentExposure";
        public const string CONST_NetPercentExposure = "NetPercentExposure";
        public const string CONST_CostBasisPNL = "CostBasisPNL";
        public const string CONST_NetExposureBasisQty = "NetExposureBasisQty";
        #endregion

        #region Admin
        /// <summary>
        /// Bharat Kumar Jangir (04 March 2014)
        /// Fees Names Mapping - UI Names Vs Code Names
        /// </summary>
        public static Dictionary<string, string> FeeNamesCollection
        {
            get
            {
                Dictionary<string, string> _feeNameCollection = new Dictionary<string, string>();
                _feeNameCollection.Add(PROPERTY_STAMPDUTY, CAPTION_STAMPDUTY);
                _feeNameCollection.Add(PROPERTY_TRANSACTIONLEVY, CAPTION_TRANSACTIONLEVY);
                _feeNameCollection.Add(PROPERTY_TAXONCOMMISSIONS, CAPTION_TAXONCOMMISSIONS);
                _feeNameCollection.Add(PROPERTY_SECFEE, CAPTION_SECFEE);
                _feeNameCollection.Add(PROPERTY_OCCFEE, CAPTION_OCCFEE);
                _feeNameCollection.Add(PROPERTY_ORFFEE, CAPTION_ORFFEE);
                _feeNameCollection.Add(PROPERTY_CLEARINGFEE, CAPTION_CLEARINGFEE);
                _feeNameCollection.Add(PROPERTY_MISCFEES, CAPTION_MISCFEES);
                return _feeNameCollection;
            }
        }
        #endregion
    }
}