using System;

namespace Prana.Allocation.Client.Constants
{
    public class AllocationUIConstants
    {
        /// <summary>
        /// The default width
        /// </summary>
        public const string COUPON_RATE = "CouponRate";

        /// <summary>
        /// The default width
        /// </summary>
        public const int DEFAULT_WIDTH = 800;

        /// <summary>
        /// The default height
        /// </summary>
        public const int DEFAULT_HEIGHT = 600;

        /// <summary>
        /// The Account
        /// </summary>
        public const String ACCOUNT = "Account";
        /// <summary>
        /// The Account identifier
        /// </summary>
        public const String ACCOUNT_ID = "AccountId";
        /// <summary>
        /// The percentage
        /// </summary>
        public const String PERCENTAGE = "_%";
        /// <summary>
        /// The quantity
        /// </summary>
        public const String QUANTITY = "_Qty";
        /// <summary>
        /// The strateg y_ prefix
        /// </summary>
        public const String STRATEGY_PREFIX = "S_";

        /// <summary>
        /// The save_ layout
        /// </summary>
        public const String SAVE_LAYOUT = "SaveLayout";

        /// <summary>
        /// The total _ percentage
        /// </summary>
        public const String TOTAL_PERCENTAGE = "Percentage";

        /// <summary>
        /// The total _ quantity
        /// </summary>
        public const String TOTAL_QUANTITY = "Quantity";

        /// <summary>
        /// The remainin g_ percentage
        /// </summary>
        public const String REMAINING_PERCENTAGE = "RemainingPercentage";

        /// <summary>
        /// The remainin g_ quantity
        /// </summary>
        public const String REMAINING_QUANTITY = "RemainingQuantity";

        /// <summary>
        /// Total
        /// </summary>
        public const String TOTAL_NAME = "Total";

        /// <summary>
        /// Remaining
        /// </summary>
        public const String REMAINING_NAME = "Remaining";

        /// <summary>
        /// Selected Trade
        /// </summary>
        public const String SELECTED_TRADE = "SelecedTrade";

        /// <summary>
        /// Total No. of Trades
        /// </summary>
        public const String TOTAL_NO_OF_TRADES = "TotalNoOfTrades";

        /// <summary>
        /// The Group Keyword
        /// </summary>
        public const String GROUP = "Group";

        /// <summary>
        /// Header for Percentage
        /// </summary>
        public const String PERCENTAGE_HEADER = "%";

        /// <summary>
        /// Header for Quantity
        /// </summary>
        public const String QUANTITY_HEADER = "Qty";

        /// <summary>
        /// Spacer column key
        /// </summary>
        public const String SPACER = "_Spacer";

        /// <summary>
        /// The layout version allocationgrid
        /// </summary>
        public const String LAYOUT_VERSION_ALLOCATIONGRID = "<Version>V1.9.00</Version>";

        /// <summary>
        /// The layou t_ allocatio n_ UI
        /// </summary>
        public const String LAYOUT_ALLOCATION_UI = "AllocationUI";

        /// <summary>
        /// Gets or sets the selec t_ string.
        /// </summary>
        /// <value>
        /// The selec t_ string.
        /// </value>
        public const String SELECT_STRING = "-Select-";

        /// <summary>
        /// The field chooser unallocated textbox
        /// </summary>
        public const String FIELD_CHOOSER_UNALLOCATED_TEXTBOX = "FieldChooserUnAllocatedTextBox";

        /// <summary>
        /// The field chooser allocated textbox
        /// </summary>
        public const String FIELD_CHOOSER_ALLOCATED_TEXTBOX = "FieldChooserAllocatedTextBox";

        /// <summary>
        /// The fund name, it is used for fixed preferences only
        /// </summary>
        public const String FUND_NAME = "FundName";

        #region Fields Name

        //Unbound fields
        public const String NETAMOUNT_LOCAL = "NetAmountLocal";
        public const String NETAMOUNT_BASE = "NetAmountBase";
        public const String PRINCIPAL_AMOUNT_LOCAL = "PrincipalAmountLocal";
        public const String PRINCIPAL_AMOUNT_BASE = "PrincipalAmountBase";
        public const String COUNTER_CURRENCY = "CounterCurrency";
        public const String COUNTER_CURRENCY_AMOUNT = "CounterCurrencyAmount";
        public const String AVGPRICE_BASE = "AvgPriceBase";
        public const String ASSET_CATEGORY = "AssetCategory";
        public const String SETTLEMENT_CURRENCY = "SettlementCurrency";
        public const String IMPORT_FILE_NAME = "ImportFileName";
        public const String ACCOUNT_NAME = "AccountName";
        public const String STRATEGY_NAME = "StrategyName";
        public const String EXECUTION_TIME = "ExecutionTime";

        //Bound fields
        public const String ASSET_NAME = "AssetName";
        public const String SETTLEMENT_CURRENCYID = "SettlementCurrencyID";
        public const String AVGPRICE = "AvgPrice";
        public const String ORDERSIDE_TAGVALUE = "OrderSideTagValue";
        public const String CONTRACT_MULTIPLIER = "ContractMultiplier";
        public const String CUMQTY = "CumQty";
        public const String ASSETID = "AssetID";
        public const String SYMBOL = "Symbol";
        public const String ALLOCATED_QTY = "AllocatedQty";
        public const String TOTAL_COMMISSION_AND_FEES = "TotalCommissionandFees";
        public const String UNDERLYING_NAME = "UnderlyingName";
        public const String EXCHANGE_NAME = "ExchangeName";
        public const String CURRENCY_NAME = "CurrencyName";
        public const String COMPANY_USER_NAME = "CompanyUserName";
        public const String TOTAL_COMMISSION_PER_SHARE = "TotalCommissionPerShare";
        public const String TOTAL_COMMISSION = "TotalCommission";
        public const String COMPANY_NAME = "CompanyName";
        public const String UNDERLYING_SYMBOL = "UnderlyingSymbol";
        public const String BLOOMBERG_SYMBOL = "BloombergSymbol";
        public const String BLOOMBERG_SYMBOL_WITH_EXCHANGE_CODE = "BloombergSymbolWithExchangeCode";
        public const String SEDOL_SYMBOL = "SedolSymbol";
        public const String CUSIP_SYMBOL = "CusipSymbol";
        public const String OSI_SYMBOL = "OSISymbol";
        public const String IDCO_SYMBOL = "IDCOSymbol";
        public const String CLOSING_ALGO_TEXT = "ClosingAlgoText";
        public const String NAV_LOCK_STATUS = "NavLockStatus";
        public const String CHANGE_TYPE = "ChangeType";
        public const String GROUP_ID = "GroupID";
        public const String CLOSING_STATUS = "ClosingStatus";
        public const String COMMISSION = "Commission";
        public const String SOFT_COMMISSION = "SoftCommission";
        public const String IS_MANUAL_GROUP = "IsManualGroup";
        public const String ACCRUED_INTEREST = "AccruedInterest";
        public const String ORF_FEE = "OrfFee";
        public const String OCC_FEE = "OccFee";
        public const String SEC_FEE = "SecFee";
        public const String MISC_FEES = "MiscFees";
        public const String CLEARING_FEE = "ClearingFee";
        public const String TAX_ON_COMMISSIONS = "TaxOnCommissions";
        public const String TRANSACTION_LEVY = "TransactionLevy";
        public const String STAMP_DUTY = "StampDuty";
        public const String CLEARING_BROKER_FEE = "ClearingBrokerFee";
        public const String OTHER_BROKER_FEES = "OtherBrokerFees";
        public const String COMMISSION_PER_SHARE = "CommissionPerShare";
        public const String SOFT_COMMISSION_PER_SHARE = "SoftCommissionPerShare";
        public const String FXRATE = "FXRate";
        public const String TAXLOT_QTY = "TaxLotQty";
        public const String ACCOUNT_ID_COLUMN = "AccountID";
        public const String ALLOCATION_SCHEME_ID = "AllocationSchemeID";
        public const String ALLOCATIONS = "Allocations";
        public const String AUTO_GROUPED = "AutoGrouped";
        public const String ASSET_ID = "AssetID";
        public const String CURRENCY_ID = "CurrencyID";
        public const String EXCHANGE_ID = "ExchangeID";
        public const String EXTERNAL_TRANS_ID = "ExternalTransId";
        public const String COMPANY_USER_ID = "CompanyUserID";
        public const String INT_PRANA_MSG_TYPE = "IntPranaMsgType";
        public const String IS_GROUP_ALLOCATED_TO_ONE_TAXLOT = "IsGroupAllocatedToOneTaxLot";
        public const String IS_CAL_COUNTERPARTY_COMMISSION = "IsCalCounterPartyCommission";
        public const String IS_RECALCULATE_COMMISSION = "IsRecalculateCommission";
        public const String IS_COMMISSION_CALCULATED = "IsCommissionCalculated";
        public const String IS_SOFT_COMMISSION_CHANGED = "IsSoftCommissionChanged";
        public const String IS_COMMISSION_CHANGED = "IsCommissionChanged";
        public const String COMMISSION_SOURCE = "CommissionSource";
        public const String SOFT_COMMISSION_SOURCE = "SoftCommissionSource";
        public const String IS_CURRENCY_FUTURE = "IsCurrencyFuture";
        public const String IS_MODIFIED = "IsModified";
        public const String LEAD_CURRENCY_ID = "LeadCurrencyID";
        public const String STATE_ID = "StateID";
        public const String STRATEGY_ID = "StrategyID";
        public const String TRADING_ACCOUNT_ID = "TradingAccountID";
        public const String UNDERLYING_ID = "UnderlyingID";
        public const String USER_ID = "UserID";
        public const String VENUE_ID = "VenueID";
        public const String VS_CURRENCY_ID = "VsCurrencyID";
        public const String LOT_ID = "LotId";
        public const String COUNTERPARTY_ID = "CounterPartyID";
        public const String FIRST_COUPON_DATE = "FirstCouponDate";
        public const String FIXING_DATE = "FixingDate";
        public const String FOREX_REQ = "ForexReq";
        public const String FREQ = "Freq";
        public const String GROUP_ALLOCATION_STATUS = "GroupAllocationStatus";
        public const String IS_ZERO = "IsZero";
        public const String IS_PRORATA_ACTIVE = "ISProrataActive";
        public const String ISSUE_DATE = "IssueDate";
        public const String IS_SWAPPED = "IsSwapped";
        public const String LIST_ID = "ListID";
        public const String MATURITY_DATE = "MaturityDate";
        public const String NOT_ALL_EXECUTED = "NotAllExecuted";
        public const String NOTIONAL_CHANGE = "NotionalChange";
        public const String ORDER_COUNT = "OrderCount";
        public const String ORDER_TYPE_TAG_VALUE = "OrderTypeTagValue";
        public const String OTHER_CHK_BOX = "OtherChkBox";
        public const String PARENT_TAXLOT_PK = "ParentTaxlot_PK";
        public const String PERSISTENCE_STATUS = "PersistenceStatus";
        public const String POSITION_TAG_VALUE = "PositionTagValue";
        public const String PUT_CALL = "PutCall";
        public const String PUT_OR_CALL = "PutOrCall";
        public const String REUTERS_SYMBOL = "ReutersSymbol";
        public const String ROUNDLOT = "RoundLot";
        public const String STATE = "State";
        public const String STRIKE_PRICE = "StrikePrice";
        public const String SWAP_PARAMETERS = "SwapParameters";
        public const String TAXLOT_CLOSING_ID = "TaxLotClosingId";
        public const String TAXLOT_IDS_WITH_ATTRIBUTES = "TaxLotIdsWithAttributes";
        public const String UPDATED = "Updated";
        public const String WORK_FLOW_STATE = "WorkflowState";
        public const String NIRVANA_PROCESS_DATE = "NirvanaProcessDate";
        public const String COMMISSION_TEXT = "CommissionText";
        public const String CORP_ACTION_ID = "CorpActionID";
        public const String LEVEL1_ID = "Level1ID";
        public const String LEVEL2_ID = "Level2ID";
        public const String FX_CONVERSION_METHOD_OPERATOR = "FXConversionMethodOperator";
        public const String M2M_PROFIT_LOSS = "M2MProfitLoss";
        public const String IS_PREALLOCATED = "IsPreAllocated";
        public const String AUEC_LOCAL_DATE = "AUECLocalDate";
        public const String ORDER_SIDE = "OrderSide";
        public const String TRADING_ACCOUNT_NAME = "TradingAccountName";
        public const String ISNDF = "IsNDF";
        public const String AUECID = "AUECID";
        public const String UNALLOCATED_QTY = "UnAllocatedQty";
        public const String COUNTERPARTY_NAME = "CounterPartyName";
        public const String ISIN_SYMBOL = "ISINSymbol";
        public const String TAXLOT_FIELD_LAYOUT_NAME = "TaxLot";
        public const String ALLOCATION_GROUP_FIELD_LAYOUT_NAME = "AllocationGroup";
        public const String ALLOCATION_ORDER_FIELD_LAYOUT_NAME = "AllocationOrder";
        public const String LEVEL1_ALLOCATION_FIELD_LAYOUT_NAME = "Level1Allocation";
        public const String CONST_FIELD_GROUP_SELECTOR = "fieldGroupSelector";
        public const String TAXLOTS = "TaxLots";
        public const String ORDERS = "Orders";
        public const String LEVEL1_NAME = "Level1Name";
        public const String LEVEL2_NAME = "Level2Name";
        public const String ALLOCATED_EQUAL_TOTAL_QTY = "AllocatedEqualTotalQty";
        public const String COMMISION_CALCULATION_TIME = "CommissionCalculationTime";
        public const String IS_ANOTHER_TAXLOT_ATTRIBUTES_UPDATED = "IsAnotherTaxlotAttributesUpdated";
        public const String TAXLOT_PERCENTAGE = "Percentage";
        public const String ORDER_ID = "ClOrderID";
        public const String MULTITRADE_NAME = "MultiTradeName";
        public const String PRANA_MSG_TYPE = "PranaMsgType";
        public const String TAXLOT_ID = "TaxLotID";
        public const String EXPIRATION_DATE = "ExpirationDate";
        public const String SETTLEMENT_DATE = "SettlementDate";
        public const String ORIGINAL_PURCHASE_DATE = "OriginalPurchaseDate";
        public const String PROCESS_DATE = "ProcessDate";
        public const String TradeAttribute = "TradeAttribute";
        public const String TradeAttribute1 = "TradeAttribute1";
        public const String TradeAttribute2 = "TradeAttribute2";
        public const String TradeAttribute3 = "TradeAttribute3";
        public const String TradeAttribute4 = "TradeAttribute4";
        public const String TradeAttribute5 = "TradeAttribute5";
        public const String TradeAttribute6 = "TradeAttribute6";
        public const String TradeAttribute7 = "TradeAttribute7";
        public const String TradeAttribute8 = "TradeAttribute8";
        public const String TradeAttribute9 = "TradeAttribute9";
        public const String TradeAttribute10 = "TradeAttribute10";
        public const String TradeAttribute11 = "TradeAttribute11";
        public const String TradeAttribute12 = "TradeAttribute12";
        public const String TradeAttribute13 = "TradeAttribute13";
        public const String TradeAttribute14 = "TradeAttribute14";
        public const String TradeAttribute15 = "TradeAttribute15";
        public const String TradeAttribute16 = "TradeAttribute16";
        public const String TradeAttribute17 = "TradeAttribute17";
        public const String TradeAttribute18 = "TradeAttribute18";
        public const String TradeAttribute19 = "TradeAttribute19";
        public const String TradeAttribute20 = "TradeAttribute20";
        public const String TradeAttribute21 = "TradeAttribute21";
        public const String TradeAttribute22 = "TradeAttribute22";
        public const String TradeAttribute23 = "TradeAttribute23";
        public const String TradeAttribute24 = "TradeAttribute24";
        public const String TradeAttribute25 = "TradeAttribute25";
        public const String TradeAttribute26 = "TradeAttribute26";
        public const String TradeAttribute27 = "TradeAttribute27";
        public const String TradeAttribute28 = "TradeAttribute28";
        public const String TradeAttribute29 = "TradeAttribute29";
        public const String TradeAttribute30 = "TradeAttribute30";
        public const String TradeAttribute31 = "TradeAttribute31";
        public const String TradeAttribute32 = "TradeAttribute32";
        public const String TradeAttribute33 = "TradeAttribute33";
        public const String TradeAttribute34 = "TradeAttribute34";
        public const String TradeAttribute35 = "TradeAttribute35";
        public const String TradeAttribute36 = "TradeAttribute36";
        public const String TradeAttribute37 = "TradeAttribute37";
        public const String TradeAttribute38 = "TradeAttribute38";
        public const String TradeAttribute39 = "TradeAttribute39";
        public const String TradeAttribute40 = "TradeAttribute40";
        public const String TradeAttribute41 = "TradeAttribute41";
        public const String TradeAttribute42 = "TradeAttribute42";
        public const String TradeAttribute43 = "TradeAttribute43";
        public const String TradeAttribute44 = "TradeAttribute44";
        public const String TradeAttribute45 = "TradeAttribute45";
        public const String COMM_SOURCE = "CommSource";
        public const String SOFT_COMM_SOURCE = "SoftCommSource";
        public const String VENUE = "Venue";
        public const String TRANSACTION_TYPE = "TransactionType";
        public const String ALLOCATION_SCHEME_NAME = "AllocationSchemeName";
        public const String ORDER_TYPE = "OrderType";
        public const String COMMISSION_AMT = "CommissionAmt";
        public const String SOFT_COMMISSION_AMT = "SoftCommissionAmt";
        public const String COMMISSION_RATE = "CommissionRate";
        public const String SOFT_COMMISSION_RATE = "SoftCommissionRate";
        public const String CALCBASIS = "CalcBasis";
        public const String SOFT_COMMISSION_CALCBASIS = "SoftCommissionCalcBasis";
        public const String COUPON = "Coupon";
        public const String ALLOCATION_DATE = "AllocationDate";
        public const String TRANSACTION_SOURCE = "TransactionSource";
        public const String TRANSACTION_SOURCE_TAG = "TransactionSourceTag";
        public const String ERROR_MESSAGE = "ErrorMessage";
        public const String GROUP_STATUS = "GroupStatus";
        public const String ACCRUAL_BASIS = "AccrualBasis";
        public const String BOND_TYPE = "BondType";
        public const String PUTCALL = "PutOrCalls";
        public const String ALLOCATION_BASED_ON = "AllocationBasedOn";
        public const String RIC = "RIC";
        public const String LONG_NAME = "LongName";
        public const String TRADE_TYPE = "TradeType";
        public const String ISIN = "ISIN";
        public const String SEDOL = "SEDOL";
        public const String CUSIP = "CUSIP";
        public const String BLOOMBERG = "Bloomberg";
        public const String OSIOPTION_SYMBOL = "OSIOptionSymbol";
        public const String IDCOOPTION_SYMBOL = "IDCOOptionSymbol";
        public const String ALLOCATION_SCHEME_KEY = "AllocationSchemeKey";
        public const String SIDE = "Side";
        public const String TOTAL_QTY = "TotalQty";
        public const String ROW_INDEX = "RowIndex";

        public const String EXCHANGE_OPERATOR = "ExchangeOperator";
        public const String EXCHANGE_LIST = "ExchangeList";
        public const String ORDER_SIDE_OPERATOR = "OrderSideOperator";
        public const String ORDER_SIDE_LIST = "OrderSideList";
        public const String ASSET_OPERATOR = "AssetOperator";
        public const String ASSET_LIST = "AssetList";
        public const String PR_OPERATOR = "PROperator";
        public const String PR_LIST = "PRList";
        public const String CONST_ALL = "All";
        public const String PROXY_SYMBOL = "ProxySymbol";
        public const String INTERNAL_COMMENTS = "InternalComments";
        public const String UNDERLYING_DELTA = "UnderlyingDelta";
        public const String OPTION_PREMIUM_ADJUSTMENT = "OptionPremiumAdjustment";
        public const String IS_SELECTED = "IsSelected";
        public const String EXTERNAL_TRANSACTION_ID = "ExternalTransactionID";
        public const String OriginalAllocationPreferenceID = "OriginalAllocationPreferenceID";
        public const String ORIGINAL_CUM_QTY = "OriginalCumQty";
        public const String CUM_QTY_FOR_SUB_ORDER = "CumQtyForSubOrder";
        public const String CUM_QTY_FORLIVEORDER = "CumQtyForLiveOrder";
        public const String MASTER_FUND_NAME = "MasterFundName";
        public const String TARGET_RATIO_PCT = "TargetRatioPct";
        public const String COMPANY_MASTER_FUND_ID = "CompanyMasterFundID";
        public const String CHANGE_COMMENT = "ChangeComment";
        public const String IS_STAGE_REQUIRED = "IsStageRequired";
        public const String IS_MANUAL_ORDER = "IsManualOrder";
        public const String BorrowerID = "BorrowerID";
        public const String ShortRebate = "ShortRebate";
        public const String BorrowerBroker = "BorrowerBroker";
        public const String ModifiedUserId = "ModifiedUserId";
        public const String FACTSET_SYMBOL = "FactSetSymbol";
        public const String ACTIV_SYMBOL = "ActivSymbol";
        public const String ExecutionTimeLastFill = "ExecutionTimeLastFill";
        public const String AVG_PRICE_FOR_COMPLIANCE = "AvgPriceForCompliance";
        public const String IsOverbuyOversellAccepted = "IsOverbuyOversellAccepted";
        public const String Is_Samsara_User = "IsSamsaraUser";
        public const String IS_MULTIBROKER_TRADE = "IsMultiBrokerTrade";
        #endregion

        #region captions
        public const String CAPTION_COUNTERPARTY_NAME = "Broker";
        public const String CAPTION_GROUP_ID = "GroupID";
        public const String CAPTION_AUEC_ID = "AUECID";
        public const String CAPTION_IS_PRORATA_ACTIVE = "ISProrata Active";
        public const String CAPTION_IS_MANUAL_GROUP = "IsManual Group";
        public const String CAPTION_ISNDF = "ISNDF";
        public const String CAPTION_FXRATE = "FX Rate";
        public const String CAPTION_ISIN_SYMBOL = "ISIN";
        public const String CAPTION_OSI_SYMBOL = "OSISSymbol";
        public const String CAPTION_IDCO_SYMBOL = "IDCOSymbol";
        public const String CAPTION_M2M_PROFIT_LOSS = "M2MProfitLoss";
        public const String CAPTION_SEC_FEE = "SEC Fee";
        public const String CAPTION_OCC_FEE = "OCC Fee";
        public const String CAPTION_ORF_FEE = "ORF Fee";
        public const String CAPTION_UNALLOCATED_QTY = "UnAllocatedQty";
        public const String CAPTION_FX_CONVERSION_OPERATOR = "FX Conversion Operator";
        public const String CAPTION_IS_SWAPPED = "IsSwapped";
        public const String CAPTION_COMPANY_NAME = "Company";
        public const String CAPTION_IS_PREALLOCATED = "IsPreAllocated";
        public const String CAPTION_AUEC_LOCAL_DATE = "Trade Date";
        public const String CAPTION_ORDER_SIDE = "Side";
        public const String CAPTION_UNDERLYING_NAME = "Underlying";
        public const String CAPTION_EXCHANGE_NAME = "Exchange";
        public const String CAPTION_CURRENCY_NAME = "Currency";
        public const String CAPTION_BLOOMBERG_SYMBOL = "Bloomberg";
        public const String CAPTION_BLOOMBERG_SYMBOL_WITH_EXCHANGE_CODE = "Bloomberg Symbol(with Exchange Code)";
        public const String CAPTION_COMPANY_USER_NAME = "User";
        public const String CAPTION_TRADING_ACCOUNT_NAME = "Trading Account";
        public const String CAPTION_CUMQTY = "Executed Qty";
        public const String CAPTION_CLEARING_FEE = "AUEC Fee1";
        public const String CAPTION_MISC_FEES = "AUEC Fee2";
        public const String CAPTION_CLOSING_ALGO_TEXT = "Closing Method";
        public const String CAPTION_ACCOUNT_NAME = "Account Name";
        public const String CAPTION_CLIENT_NAME = "Client";
        public const String CAPTION_MASTER_FUND = "Master Fund";
        public const String CAPTION_STRATEGY_NAME = "Strategy Name";
        public const String CAPTION_EXECUTION_TIME = "Execution Time";
        public const String CAPTION_IMPORT_FILE_NAME = "Import File Name";
        public const String CAPTION_NETAMOUNT_LOCAL = "Net Amount(Local)";
        public const String CAPTION_NETAMOUNT_BASE = "Net Amount(Base)";
        public const String CAPTION_NETAMOUNT_SETTLEMENT = "Net Amount(Settlement)";
        public const String CAPTION_PRINCIPAL_AMOUNT_LOCAL = "Principal Amount(Local)";
        public const String CAPTION_PRINCIPAL_AMOUNT_BASE = "Principal Amount(Base)";
        public const String CAPTION_COUNTER_CURRENCY = "Counter Currency";
        public const String CAPTION_COUNTER_CURRENCY_AMOUNT = "Counter Currency Amount";
        public const String CAPTION_AVGPRICE_BASE = "Avg Price(Base)";
        public const String CAPTION_ASSET_CATEGORY = "Asset Category";
        public const String CAPTION_SETTLEMENT_CURRENCY = "Settlement Currency";
        public const String CAPTION_PRANA_MSG_TYPE = "Source";
        public const String CAPTION_LOT_ID = "Lot ID";
        public const String CAPTION_OTHER_BROKER_FEES = "Other Broker Fees";
        public const String CAPTION_CLEARING_BROKER_FEE = "Clearing Broker Fees";
        public const String CAPTION_STAMP_DUTY = "Stamp Duty";
        public const String CAPTION_EXTERNAL_TRANS_ID = "External Transaction ID";
        public const String CAPTION_TAXLOT_QTY = "Taxlot Qty";
        public const String CAPTION_TradeAttribute = "Trade Attribute ";
        public const String CAPTION_TradeAttribute1 = "Trade Attribute 1";
        public const String CAPTION_TradeAttribute2 = "Trade Attribute 2";
        public const String CAPTION_TradeAttribute3 = "Trade Attribute 3";
        public const String CAPTION_TradeAttribute4 = "Trade Attribute 4";
        public const String CAPTION_TradeAttribute5 = "Trade Attribute 5";
        public const String CAPTION_TradeAttribute6 = "Trade Attribute 6";
        public const String CAPTION_AVGPRICELOCAL = "Avg Price(Local)";
        public const String CAPTION_TOTALCOMMISSIONANDFEE = "Total Commission And Fee";
        public const String CAPTION_ISANOTHERTAXLOTUPDATED = "IsAnotherTaxlotAttributesUpdated";
        public const String CAPTION_TICKER_SYMBOL = "Ticker Symbol";
        public const String CAPTION_FACTSET_SYMBOL = "FactSet Symbol";
        public const String CAPTION_ACTIV_SYMBOL = "ACTIV Symbol";
        public const String CAPTION_DESCRIPTION = "Description";
        public const String CAPTION_TOTAL_QUANTITY = "Total Quantity";
        public const String CAPTION_ALLOCATION_PERCENTAGE = "Allocation %";
        public const String CAPTION_ROUNDLOTS = "RoundLots";
        public const String CAPTION_TRADE_TYPE = "Trade Type";
        public const String CAPTION_PB = "PB";
        public const String CAPTION_SEDOL_SYMBOL = "Sedol Symbol";
        public const String CAPTION_BLOOMBERG_WITH_SYMBOL = "Bloomberg Symbol";
        public const String CAPTION_ACCRUED_INTEREST = "Accrued Interest";

        public const String CAPTION_CHANGE_TYPE = "Change Type";
        public const String CAPTION_CLOSING_STATUS = "Closing Status";
        public const String CAPTION_OPTION_PREMIUM_ADJUSTMENT = "Option Premium Adjustment";
        public const String CAPTION_SOFT_COMMISSION = "Soft Commission";
        public const String CAPTION_TAX_ON_COMMISSIONS = "Tax On Commissions";
        public const String CAPTION_TRANSACTION_LEVY = "Transaction Levy";

        public const String CAPTION_EXCHANGE_SELECTOR = "Exchange Selector";
        public const String CAPTION_ORDER_SIDE_SELECTOR = "Order Side Selector";
        public const String CAPTION_ASSET_SELECTOR = "Asset Selector";
        public const String CAPTION_PR_SELECTOR = "PR Selector";
        public const String CAPTION_TRANSACTIONSOURCE = "Transaction Source";
        public const String CAPTION_ALLOCATION_SCHEME_KEY = "Allocation Scheme Key";

        public const String CAPTION_PerShare = "Per Share";
        public const String CAPTION_BasisPoints = "Basis Points";
        public const String CAPTION_PerContract = "Per Contract";
        public const String CAPTION_FlatAmount = "Per Trade/Taxlot";
        public const String CAPTION_BorrowID = "Borrower Id";
        public const String CAPTION_ShortRebate = "Borrow Rate";
        public const String CAPTION_BorrowRateBPS = "Borrow Rate(BPS)";
        public const String CAPTION_BorrowRateCENT = "Borrow Rate(%)";
        public const String CAPTION_BorrowBroker = "Borrow Broker";
        #endregion

        #region Messages
        public const String MSG_AUTO_CALCULATE_FIELD = "This is a auto calculate field and will update on change in dependent column";
        #endregion

    }
}
