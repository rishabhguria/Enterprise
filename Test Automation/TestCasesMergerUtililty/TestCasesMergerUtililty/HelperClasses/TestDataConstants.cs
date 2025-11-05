using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestCasesMergerUtililty.HelperClasses
{
    public static class TestDataConstants
    {
        public static string MASTER_FOLDER = @"D:\DistributedAutomation_Master";

        public static string SLAVE_WITH_READ = @"D:\DistributedAutomation_SlaveWithRead";

        /// <summary>
        /// PATH to Files
        /// </summary>
        public static string SLAVE_WITH_WRITE = @"D:\DistributedAutomation_SlaveWithWrite";

        /// <summary>
        /// SVN Revision number Description 
        /// </summary>
        public static string SVN_REVISION_NUMBER = "";

        /// <summary>
        /// BUILD Number Description 
        /// </summary>
        public static string BUILD_NUMBER = "";


        /// <summary>
        /// Run Description 
        /// </summary>
        public static string RUN_DESCR ="";

        /// <summary>
        /// Run Description 
        /// </summary>
        //public const string CURR_TESTLOG_PATH = "TestAutomationV1.13_Results_Current";


        /// <summary>
        /// Run Description 
        /// </summary>
        public static string MASTER_TESTLOG_PATH = "";

        /// <summary>
        /// Global Algorithm combobox in Closing Preference
        /// </summary>
        public const string COL_GLOBAL_ALGO = "Global Algorithm";

        /// <summary>
        /// Secondary Sort Criteria combobox in Closing Preference
        /// </summary>
        public const string COL_SECONDARY_SORT_CRITERIA = "Secondary Sort Criteria";

        /// <summary>
        /// Override Global Algorithm checkbox in Closing Preference
        /// </summary>
        public const string COL_OVERRIDE_GLOBAL_ALGO = "Override Global Algorithm";

        /// <summary>
        /// Can close Sell Short with Buy and Buy to Open checkbox in Closing Preference
        /// </summary>
        public const string COL_CLOSE_SHORT_BUY_BTO = "Can close Sell Short with Buy and Buy to Open";

        /// <summary>
        /// Can close Sell with But to Close checkbox in Closing Preference
        /// </summary>
        public const string COL_CLOSE_SELL_BTC = "Can close Sell with Buy to Close";

        /// <summary>
        /// Auto Close Stategy checkbox in Closing Preference
        /// </summary>
        public const string COL_AUTO_CLOSE_STRATEGY = "Auto Close Stategy";

        /// <summary>
        ///Long Term Tax Rate (%) textbox in Closing Preference
        /// </summary>
        public const string COL_LONG_TERM_TAXRATE = "Long Term Tax Rate (%)";

        /// <summary>
        ///Short Term Tax Rate (%) textbox in Closing Preference
        /// </summary>
        public const string COL_SHORT_TERM_TAXRATE = "Short Term Tax Rate (%)";

        /// <summary>
        ///Quantity round off digits textbox in Closing Preference
        /// </summary>
        public const string COL_QTY_ROUNDOFF_DIGITS = "Quantity round off digits";

        /// <summary>
        ///Price round off digits textbox in Closing Preference
        /// </summary>
        public const string COL_PRICE_ROUNDOFF_DIGITS = "Price round off digits";

        /// <summary>
        /// Closing Field combobox in Closing Preference
        /// </summary>
        public const string COL_CLOSING_FIELD1 = "Closing Field";

        /// <summary>
        /// Maximum wait for fetching data on GL grid
        /// </summary>
        public const int GL_DATA_FETCHING_TIME = 1000000;

        /// <summary>
        /// The col FilterList
        /// </summary>
        public const string COL_FILTERLIST = "FilterList";

        /// <summary>
        /// The text property
        /// </summary>
        public const string TEXT_PROPERTY = "Text";

        /// <summary>
        /// The col notional value
        /// </summary>
        public const string COL_NOTIONAL_VALUE = "NotionalValue";

        /// <summary>
        /// The col interest
        /// </summary>
        public const string COL_INTEREST_ = "InterestRate";

        /// <summary>
        /// The col spread
        /// </summary>
        public const string COL_SPREAD_BP = "Spread(B.P)";

        /// <summary>
        /// The col daycount
        /// </summary>
        public const string COL_DAYCOUNT = "DayCount";

        /// <summary>
        /// The col original cost basic
        /// </summary>
        public const string COL_ORIGINAL_COST_BASIC = "OriginalCostBasis";

        /// <summary>
        /// The col name for filters
        /// </summary>
        public const string COL_NAME = "ColumnName";

        /// <summary>
        /// The col compression
        /// </summary>
        public const string COL_COMPRESSION = "Compression";

        /// <summary>
        ///  COL_SYNC_FILTER
        /// </summary>
        public const string COL_SYNC_FILTER = "SyncFilter";

        /// <summary>
        /// The col exchange selector
        /// </summary>
        public const string COL_EXCHANGE_SELECTOR = "Exchange Selector";

        /// <summary>
        /// The col exchange list
        /// </summary>
        public const string COL_EXCHANGE_LIST = "Exchange List";

        /// <summary>
        /// The col asset selector
        /// </summary>
        public const string COL_ASSET_SELECTOR = "Asset Selector";

        /// <summary>
        /// The col asset list
        /// </summary>
        public const string COL_ASSET_LIST = "Asset List";

        /// <summary>
        /// The col pr selector
        /// </summary>
        public const string COL_PR_SELECTOR = "PR Selector";

        /// <summary>
        /// The col pr list
        /// </summary>
        public const string COL_PR_LIST = "PR List";

        /// <summary>
        /// The col order side selector
        /// </summary>
        public const string COL_ORDER_SIDE_SELECTOR = "Order Side Selector";

        /// <summary>
        /// The col order side list
        /// </summary>
        public const string COL_ORDER_SIDE_LIST = "Order Side List";

        /// <summary>
        /// The col user name
        /// </summary>
        public const string COL_USERNAME = "userName";

        /// <summary>
        /// COL_SECONDARYSORT_CRITERIA
        /// </summary>
        public const string COL_SECONDARYSORT_CRITERIA = "SecondarySortCriteria";

        /// <summary>
        /// COL_CLOSING_FIELD
        /// </summary>
        public const string COL_CLOSING_FIELD = "ClosingField";

        /// <summary>
        /// COL_AUTOCLOSE_STRATEGY
        /// </summary>
        public const string COL_AUTOCLOSE_STRATEGY = "AutoCloseStrategy";

        /// <summary>
        /// COL_CLOSESSW_BUYBTW
        /// </summary>
        public const string COL_CLOSESSW_BUYBTW = "CloseSSw/BuyBTO";

        /// <summary>
        /// The col_ symbol
        /// </summary>
        public const string COL_SYMBOL = "Symbol";

        /// <summary>
        /// The col_ Prorata
        /// </summary>
        public const string COL_PRORATA = "Prorata";

        /// <summary>
        /// The col_ Pro-rata (NAV)
        /// </summary>
        public const string COL_PRORATA_NAV = "Pro-rata (NAV)";

        /// <summary>
        /// The col_ leveling
        /// </summary>
        public const string COL_LEVELING = "Leveling";

        /// <summary>
        /// The col settlement price
        /// </summary>
        public const string COL_SETTLEMENT_PRICE = "Settlement Price";

        /// <summary>
        /// The col settle by broker
        /// </summary>
        public const string COL_SETTLE_BY_BROKER = "SettleByBroker";

        /// <summary>
        /// The col settlement currency
        /// </summary>
        public const string COL_SETTLEMENT_CURRENCY = "SettlementCurrency";

        /// <summary>
        /// The col settlement fx rate
        /// </summary>
        public const string COL_SETTLEMENT_FX_RATE = "SettlementFXRate";

        /// <summary>
        /// The col settlement amount
        /// </summary>
        public const string COL_SETTLEMENT_AMOUNT = "SettlementAmount";

        /// <summary>
        /// The col settlement fx operator
        /// </summary>
        public const string COL_SETTLEMENT_FX_OPERATOR = "SettlementFXOperator";

        /// <summary>
        /// The col stop
        /// </summary>
        public const string COL_STOP = "Stop";

        /// <summary>
        /// The col_ Date_Type
        /// </summary>
        public const string COL_DATE_TYPE = "Date Type";

        /// <summary>
        /// The col limit
        /// </summary>
        public const string COL_LIMIT = "Limit";

        /// <summary>
        /// The col_ side
        /// </summary>
        public const string COL_SIDE = "Side";

        /// <summary>
        /// The col_ deal in
        /// </summary>
        public const string COL_DEAL_IN = "Deal In";

        /// <summary>
        /// The col_ quantity
        /// </summary>
        public const string COL_QUANTITY = "Quantity";

        /// <summary>
        /// The col total quantity
        /// </summary>
        public const string COL_TOTAL_QUANTITY = "Total Quantity";

        /// <summary>
        /// The col allocation
        /// </summary>
        public const string COL_ALLOCATION_PERCENT = "Allocation %";

        /// <summary>
        /// The col roundlots
        /// </summary>
        public const string COL_ROUNDLOTS = "RoundLots";

        /// <summary>
        /// The col trade type
        /// </summary>
        public const string COL_TRADE_TYPE = "Trade Type";

        /// <summary>
        /// The col_ account
        /// </summary>
        public const string COL_ACCOUNT = "Account";

        /// <summary>
        /// The col_ account
        /// </summary>
        public const string COL_ACCOUNT_PERCENTAGE_VALUE = "Account %";

        /// <summary>
        /// The Col-Local_Currency
        /// </summary>
        public const string COL_LOCAL_CURRENCY = "Local Currency";

        /// <summary>
        /// The Col Custom View Name
        /// </summary>
        public const string COL_CUSTOM_VIEW_NAME = "CustomViewName";

        /// <summary>
        /// The button_contains
        /// </summary>
        public const string COL_CONTAINS = "Contains";

        /// <summary>
        /// The button_exact
        /// </summary>
        public const string COL_EXACT = "Exact";

        /// <summary>
        /// The button_starts_with
        /// </summary>
        public const string COL_STARTS_WITH = "Starts With";
        /// <summary>
        /// The col_ AvgPrice
        /// </summary>
        public const string COL_AVG_PRICE = "AvgPrice";

        /// <summary>
        /// The col_ is_force_allocation
        /// </summary>
        public const string COL_IS_FORCE_ALLOCATION = "IsForceAllocation";

        /// <summary>
        /// The col_ executed_ quantity
        /// </summary>
        public const string COL_EXECUTED_QUANTITY = "Executed Quantity";

        /// <summary>
        /// The col trader
        /// </summary>
        public const string COL_TRADER = "Trader";

        /// <summary>
        /// The col handling instructions
        /// </summary>
        public const string COL_HANDLING_INSTRUCTIONS = "HandlingInstructions";

        /// <summary>
        /// The col execution instructions
        /// </summary>
        public const string COL_EXECUTION_INSTRUCTIONS = "ExecutionInstructions";

        /// <summary>
        /// The col_ average_ price
        /// </summary>
        public const string COL_AVERAGE_PRICE = "Average Price";

        /// <summary>
        /// The col_ TradeQuantityIn
        /// </summary>
        public const string COL_TARGET_QUANTITY_IN = "TargetQuantityIn";

        /// <summary>
        /// The col_ action
        /// </summary>
        public const string COL_ACTION = "Action";

        /// <summary>
        /// The col_ chk box option
        /// </summary>
        public const string COL_CHKBOX_OPTION = "ChkBoxOption";

        /// <summary>
        /// The col_ chk box BOOK AS SWAP
        /// </summary>
        public const string COL_CHKBOX_BOOK_AS_SWAP = "ChkBoxBookAsSwap";

        /// <summary>
        /// The col_interest rate
        /// </summary>
        public const string COL_INTEREST_RATE = "Interest Rate";

        /// <summary>
        /// The col_ FIRST RESET DATE
        /// </summary>
        public const string COL_FIRST_RESET_DATE = "First Reset Date";

        /// <summary>
        /// The col_ executed qty
        /// </summary>
        public const string COL_EXECUTED_QTY = "Executed Qty";

        /// <summary>
        /// The col_ average_ price_base
        /// </summary>
        public const string COL_AVERAGE_PRICE_BASE = "Avg Price(Base)";

        /// <summary>
        /// The col_ average_ price__base
        /// </summary>
        public const string COL_AVERAGE_PRICE__BASE = "[Avg Price( Base)]";

        /// <summary>
        /// The col_ preference
        /// </summary>
        public const string COL_PREFERENCE = "Preference";

        /// <summary>
        /// 
        /// </summary>
        public const string COL_ACCOUNT_PERCENTAGE = "Account Percentage";

        /// <summary>
        /// The col_ preference type
        /// </summary>
        public const string COL_PREFERENCE_TYPE = "PreferenceType";

        /// <summary>
        /// The col_ pttprefneeded
        /// </summary>
        public const string COL_PTTPREFNEEDED = "PTTPrefNeeded";

        /// <summary>
        /// The col_ rangetype
        /// </summary>
        public const string COL_RANGETYPE = "RangeType";

        /// <summary>
        /// The col_ from
        /// </summary>
        public const string COL_FROM = "From";

        /// <summary>
        /// The col_ to
        /// </summary>
        public const string COL_TO = "To";

        /// <summary>
        /// COL ACCOUNT FILTER
        /// </summary>
        public const string COL_ACCOUNT_FILTER = "Account Filter";

        /// <summary>
        /// The col_ current
        /// </summary>
        public const string COL_CURRENT = "Current";

        /// <summary>
        /// The col_ account_ name
        /// </summary>
        public const string COL_ACCOUNT_NAME = "Account Name";

        /// <summary>
        /// The col_ account_ name
        /// </summary>
        public const string COL_ALLOCATION_PERCENTAGE = "Allocation %";

        /// <summary>
        /// The col_ account_ name
        /// </summary>
        public const string COL_ACCOUNT_QUANTITY = "Account Quantity";

        /// <summary>
        /// The col_ commission
        /// </summary>
        public const string COL_COMMISSION = "Commission";

        /// <summary>
        /// The col_ CommissionBasis
        /// </summary>
        public const string COL_COMMISSION_BASIS = "CommissionBasis";

        /// <summary>
        /// The col soft basis
        /// </summary>
        public const string COL_SOFT_BASIS = "SoftBasis";

        /// <summary>
        /// The col soft rate
        /// </summary>
        public const string COL_SOFT_RATE = "SoftRate";

        /// <summary>
        /// The col_ Softcommission
        /// </summary>
        public const string COL_SOFTCOMMISSION = "SoftCommission";

        /// <summary>
        /// The col_ Soft commission
        /// </summary>
        public const string COL_SOFT_COMMISSION = "Soft Commission";

        /// <summary>
        /// The col_ trade
        /// </summary>
        public const string COL_TRADE = "Trade";

        /// <summary>
        /// The col_ tradeid
        /// </summary>
        public const string COL_TRADEID = "TradeID";

        /// <summary>
        /// The col_ orderside
        /// </summary>
        public const string COL_ORDERSIDE = "OrderSide";

        /// <summary>
        /// The col_ orderstatus
        /// </summary>
        public const string COL_ORDERSTATUS = "OrderStatus";

        /// <summary>
        /// The col_ fill_quantity
        /// </summary>
        public const string COL_FILL_QUANTITY = "Fill";

        /// <summary>
        /// The col_ status
        /// </summary>
        public const string COL_STATUS = "Status";

        /// <summary>
        /// The col_ working_ quantity
        /// </summary>
        public const string COL_WORKING_QUANTITY = "Working Quantity";

        /// <summary>
        /// The col_ cumqty
        /// </summary>
        public const string COL_CUMQTY = "CumQty";

        /// <summary>
        /// The col_ accountname
        /// </summary>
        public const string COL_ACCOUNTNAME = "AccountName";

        /// <summary>
        /// The col_strategyname
        /// </summary>
        public const string COL_STRATEGYNAME = "StrategyName";

        /// <summary>
        /// The col_ percentage
        /// </summary>
        public const string COL_PERCENTAGE = "Percentage";

        /// <summary>
        /// The col_ from_ date
        /// </summary>
        public const string COL_FROM_DATE = "From Date";

        /// <summary>
        /// The col_ to_ date
        /// </summary>
        public const string COL_TO_DATE = "To Date";

        /// <summary>
        /// The col_ revaluationdate
        /// </summary>
        public const string COL_REVALUATIONDATE = "RevaluationDate";

        /// <summary>
        /// The col_ verifyactivityneeded
        /// </summary>
        public const string COL_VERIFYACTIVITYNEEDED = "VerifyActivityNeeded";

        /// <summary>
        /// The col_ verifyjournalneeded
        /// </summary>
        public const string COL_VERIFYJOURNALNEEDED = "VerifyJournalNeeded";

        /// <summary>
        /// The col_ verify revaluation needed
        /// </summary>
        public const string COL_VERIFYREVALUATIONNEEDED = "VerifyRevaluationNeeded";

        /// <summary>
        /// The col_ level1 name
        /// </summary>
        public const string COL_LEVEL1NAME = "Level1Name";

        /// <summary>
        /// The col_ taxlotqty
        /// </summary>
        public const string COL_TAXLOTQTY = "TaxLotQty";

        /// <summary>
        /// The col_ accountvalue
        /// </summary>
        public const string COL_ACCOUNTVALUE = "AccountValue";

        /// <summary>
        /// The col_ closedqty
        /// </summary>
        public const string COL_CLOSEDQTY = "ClosedQty";

        /// <summary>
        /// The col_ qty_closed
        /// </summary>
        public const string COL_QTY_CLOSED = "Qty Closed";

        /// <summary>
        /// The col_ method
        /// </summary>
        public const string COL_METHOD = "Method";

        /// <summary>
        /// The col_algorithm
        /// </summary>
        public const string COL_ALGORITHM = "Algorithm";

        /// <summary>
        /// The col_ manual
        /// </summary>
        public const string COL_MANUAL = "Manual";

        /// <summary>
        /// COL_AUTOMATIC
        /// </summary>
        public const string COL_AUTOMATIC = "Automatic";

        /// <summary>
        /// The col_ long_quantity
        /// </summary>
        public const string COL_LONG_QUANTITY = "Long Quantity";

        /// <summary>
        /// The Col_Long_Debit_Limit
        /// </summary>
        public const string COL_LONG_DEBIT_LIMIT = "Long Debit Limit";

        /// <summary>
        /// The Col_Short_Credit_Limit
        /// </summary>
        public const string COL_SHORT_CREDIT_LIMIT = "Short Credit Limit";

        /// <summary>
        /// The Col_Long_Debit_Balance.
        /// </summary>
        public const string COL_LONG_DEBIT_BALANCE = "Long Debit Balance";

        /// <summary>
        /// The COl_Short_Credit_Balance.
        /// </summary>
        public const string COL_SHORT_CREDIT_BALANCE = "Short Credit Balance";

        /// <summary>
        /// The col_ long_side
        /// </summary>
        public const string COL_LONG_SIDE = "Long Side";

        /// <summary>
        /// The col_ short_quantity
        /// </summary>
        public const string COL_SHORT_QUANTITY = "Short Quantity";

        /// <summary>
        /// The col_ short_ side
        /// </summary>
        public const string COL_SHORT_SIDE = "Short Side";

        /// <summary>
        /// The col_ UnitCost
        /// </summary>
        public const string COL_UNITCOST = "UnitCost";

        /// <summary>
        /// The col_long_unit_cost
        /// </summary>
        public const string COL_LONG_UNIT_COST = "Long Unit Cost";

        /// <summary>
        /// The col_short_unit_cost
        /// </summary>
        public const string COL_SHORT_UNIT_COST = "Short Unit Cost";

        /// <summary>
        /// The col_ ClosingAlgo
        /// </summary>
        public const string COL_CLOSINGALGO = "ClosingAlgo";

        /// <summary>
        /// The col_closing_method
        /// </summary>
        public const string COL_CLOSING_METHOD = "Closing Method";

        /// <summary>
        /// The col_side2
        /// </summary>
        public const string COL_SIDE2 = "Side2";

        /// <summary>
        /// The col_strategy2
        /// </summary>
        public const string COL_STRATEGY2 = "Strategy2";

        /// <summary>
        /// The col strategy
        /// </summary>
        public const string COL_STRATEGY = "Strategy";

        /// <summary>
        /// The col_trade_date
        /// </summary>
        public const string COL_TRADE_DATE = "Trade Date";

        /// <summary>
        /// The col_trade date
        /// </summary>
        public const string COL_TRADEDATE = "TradeDate";

        /// <summary>
        /// The settlement date
        /// </summary>
        public const string COL_SETTLEMENT_DATE = "SettlementDate";

        /// <summary>
        /// All constant
        /// </summary>
        public const string COL_ALL = "All";

        /// <summary>
        /// The col_process date
        /// </summary>
        public const string COL_PROCESSDATE = "ProcessDate";

        /// <summary>
        /// The col_ date
        /// </summary>
        public const string COL_DATE = "Date";

        /// <summary>
        /// The col_ venue
        /// </summary>
        public const string COL_VENUE = "Venue";

        /// <summary>
        /// The col_ TIF
        /// </summary>
        public const string COL_TIF = "TIF";

        /// <summary>
        /// The col_ order type
        /// </summary>
        public const string COL_ORDER_TYPE = "Order Type";

        /// <summary>
        /// The col_ broker
        /// </summary>
        public const string COL_BROKER = "Broker";

        /// <summary>
        /// The col broker notes
        /// </summary>
        public const string COL_BROKER_NOTES = "Broker Notes";


        /// <summary>
        /// The col notes
        /// </summary>
        public const string COL_NOTES = "Notes";

        /// <summary>
        /// The col_ AccruedInterest
        /// </summary>
        public const string COL_ACCRUED_INTEREST = "AccruedInterest";

        /// <summary>
        /// The col_ auto allocation
        /// </summary>
        public const string COL_AUTOALLOCATION = "AutoAllocation";

        /// <summary>
        /// The col_ beta
        /// </summary>
        public const string COL_BETA = "Beta";

        /// <summary>
        /// The col default symbology
        /// </summary>
        public const string COL_DEFAULT_SYMBOLOGY = "DefaultSymbology";

        /// <summary>
        /// The col default option type
        /// </summary>
        public const string COL_DEFAULT_OPTION_TYPE = "DefaultOptionType";

        /// <summary>
        /// The col show option details
        /// </summary>
        public const string COL_SHOW_OPTION_DETAILS = "ShowOptionDetails";

        /// <summary>
        /// The col percentage executed
        /// </summary>
        public const string COL_PERCENTAGE_EXECUTED = "% Executed";

        /// <summary>
        /// The col remaining qty
        /// </summary>
        public const string COL_REMAINING_QTY = "Remaining Qty";

        /// <summary>
        /// The col_ ApplyOnDefaultRule
        /// </summary>
        public const string COL_APPLY_ON_DEFAULT_RULE = "ApplyOnDefaultRule";

        /// <summary>
        /// The col_ ApplyOnSelectedPreferences
        /// </summary>
        public const string COL_APPLY_ON_SELECTED_PREFERENCE = "ApplyOnSelectedPreferences";

        /// <summary>
        /// The col default notes
        /// </summary>
        public const string COL_DEFAULT_NOTES = "DefaultNotes";

        /// <summary>
        /// The col keep trading ticket
        /// </summary>
        public const string COL_KEEP_TRADING_TICKET_OPEN = "KeepTradingTicketOpenAfterTrade";

        /// <summary>
        /// The col clean details after trade
        /// </summary>
        public const string COL_CLEAN_DETAILS_AFTER_TRADE = "CleanDetailsAfterTrade";

        /// <summary>
        /// The col default broker notes
        /// </summary>
        public const string COL_DEFAULT_BROKER_NOTES = "DefaultBrokerNotes";

        /// <summary>
        /// The col_  prefrence name
        /// </summary>
        public const string COL_PREFERENCE_NAME = "PreferenceName";

        /// <summary>
        /// The col_ price
        /// </summary>
        public const string COL_PRICE = "Price";

        /// <summary>
        /// The col_ AllocationMethod
        /// </summary>
        public const string COL_ALLOCATION_METHOD = "AllocationMethod";

        /// <summary>
        /// The col_ ProrataAccounts
        /// </summary>
        public const string COL_PRORATA_ACCOUNTS = "ProrataAccounts";

        /// <summary>
        /// The col_ RemainderAllocationTo
        /// </summary>
        public const string COL_REMAINDER_ALLOCATION_TO = "RemainderAllocationTo";

        /// <summary>
        /// The col transaction time
        /// </summary>
        public const string COL_TRANSACTION_TIME = "Transaction Time";

        /// <summary>
        /// The col_ MatchClosingTransactions
        /// </summary>
        public const string COL_MATCH_CLOSING_TRANSACTIONS = "MatchClosingTransactions";

        /// <summary>
        /// The col_ AccountsForProrata
        /// </summary>
        public const string COL_ACCOUNTS_FOR_PRORATA = "AccountsForProrata";

        /// <summary>
        /// The col_ DateUpToDays
        /// </summary>
        public const string COL_DATE_UPTO_DAYS = "DateUpToDays";

        /// <summary>
        /// The col transaction levy
        /// </summary>
        public const string COL_TRANSACTION_LEVY = "TransactionLevy";

        /// <summary>
        /// The control transaction levy
        /// </summary>
        public const string CTRL_TRANSACTION_LEVY = "TranscationLevy";

        /// <summary>
        /// The col_ SwapDescription
        /// </summary>
        public const string COL_SWAP_DESCRIPTION = "SwapDescription";

        /// <summary>
        /// The col_ MasterFundName
        /// </summary>
        public const string COL_MASTER_FUND_NAME = "MasterFundName";

        /// <summary>
        /// The col day executed qty/
        /// </summary>
        public const string COL_DAY_EXECUTED_QTY = "Day Executed Qty";

        /// <summary>
        /// The col_ TargetPercentage
        /// </summary>
        public const string COL_TARGET_PERCENTAGE = "TargetPercentage";

        /// <summary>
        /// The col_ EnableMasterFund
        /// </summary>
        public const string COL_ENABLE_MASTER_FUND = "EnableMasterFund";

        /// <summary>
        /// The col_ FixedPreference
        /// </summary>
        public const string COL_FIXED_PREFERENCE = "FixedPreference";

        /// <summary>
        /// The col_ DateUptoDays
        /// </summary>
        public const string COL_DATE_UP_TO_DAYS = "DateUptoDays";

        /// <summary>
        /// The col_ match closing
        /// </summary>
        public const string COL_MATCH_CLOSING = "MatchClosing";

        /// <summary>
        /// The col commission alert on broker change
        /// </summary>
        public const string COL_COMMISSION_ALERT_ON_BROKER = "CommissionAlertOnBrokerChange";

        /// <summary>
        /// The col commission alert on allocation
        /// </summary>
        public const string COL_COMMISSION_ALERT_ON_ALLOCATION = "CommissionAlertOnAllocation";

        /// <summary>
        /// The col recalculate commission on broker change
        /// </summary>
        public const string COL_RECALCULATE_COMM_BROKER = "RecalculateCommissionOnBrokerChange";

        /// <summary>
        /// The col recalculate commission on allocation
        /// </summary>
        public const string COL_RECALCULATE_COMM_ALLOCATION = "RecalculateCommissionOnAllocation";

        /// <summary>
        /// The col_ FilterTabName
        /// </summary>
        public const string COL_FILTER_TAB_NAME = "FilterTabName";

        /// <summary>
        /// The col_ Target%AsOf
        /// </summary>
        public const string COL_TARGET_PERCENTAGE_AS_OF = "Target%AsOf";

        /// <summary>
        /// The col_ Match Closing Transaction
        /// </summary>
        public const string COL_MATCH_CLOSING_TRANSACTION = "MatchClosingTransaction";

        /// <summary>
        /// The col_ ScrolltoColumn
        /// </summary>
        public const string COL_SCROLLTOCOLUMN = "ScrollToColumn";

        /// <summary>
        /// The col_ ScrolltoColumn
        /// </summary>
        public const string COL_SCROLLTOCOLUMNNAME = "ScrollToColumnName";

        /// <summary>
        /// The col_ ScrolltoRow
        /// </summary>
        public const string COL_SCROLLTOROW = "ScrollToRow";

        /// <summary>
        /// The col_ Level
        /// </summary>
        public const string COL_LEVEL = "Level";

        /// <summary>
        /// The col_ PB
        /// </summary>
        public const string COL_PB = "PB";

        /// <summary>
        /// The col_ MasterFund
        /// </summary>
        public const string COL_MASTER_FUND = "MasterFund";

        /// <summary>
        /// The col_ DataSource
        /// </summary>
        public const string COL_DATASOURCE = "DataSource";

        /// <summary>
        /// The col_from_currency
        /// </summary>
        public const string COL_FROM_CURRENCY = "From Currency";

        /// <summary>
        /// The col_to_currency
        /// </summary>
        public const string COL_TO_CURRENCY = "To Currency";

        /// <summary>
        /// The col_ factor
        /// </summary>
        public const string COL_FACTOR = "Factor";

        /// <summary>
        /// The col_ trading account
        /// </summary>
        public const string COL_TRADING_ACCOUNT = "TradingAccount";

        /// <summary>
        /// The col_ Accounts
        /// </summary>
        public const string COL_ACCOUNTS = "Accounts";

        /// <summary>
        /// The col_ PrimeBroker
        /// </summary>
        public const string COL_PRIME_BROKER = "PrimeBroker";

        /// <summary>
        /// The col average fill price local
        /// </summary>
        public const string COL_AVG_FILL_PRICE_LOCAL = "Avg Fill Price (Local)";

        /// <summary>
        /// The col last fill price local
        /// </summary>
        public const string COL_LAST_FILL_PRICE_LOCAL = "Last Fill Price (Local)";

        /// <summary>
        /// The col_ asset class
        /// </summary>
        public const string COL_ASSET_CLASS = "AssetClass";

        /// <summary>
        /// The col_ trade attribute 1
        /// </summary>
        public const string COL_TRADE_ATTRIBUTE1 = "TradeAttribute1";

        /// <summary>
        /// The col_ trade attribute 2
        /// </summary>
        public const string COL_TRADE_ATTRIBUTE2 = "TradeAttribute2";

        /// <summary>
        /// The col_ trade attribute 3
        /// </summary>
        public const string COL_TRADE_ATTRIBUTE3 = "TradeAttribute3";

        /// <summary>
        /// The col_ trade attribute 4
        /// </summary>
        public const string COL_TRADE_ATTRIBUTE4 = "TradeAttribute4";

        /// <summary>
        /// The col_ trade attribute 5
        /// </summary>
        public const string COL_TRADE_ATTRIBUTE5 = "TradeAttribute5";

        /// <summary>
        /// The col_ trade attribute 6
        /// </summary>
        public const string COL_TRADE_ATTRIBUTE6 = "TradeAttribute6";

        /// <summary>
        /// The col_ ThirdParty
        /// </summary>
        public const string COL_UPLOAD_THIRDPARTY = "Upload ThirdParty";

        /// <summary>
        /// The no_ of_times_backspace
        /// </summary>
        public const int NO_OF_TIMES_BACKSPACE = 30;

        /// <summary>
        /// The col_Description
        /// </summary>
        public const string COL_DESCRIPTION = "Description";

        /// <summary>
        /// The col sedol symbol
        /// </summary>
        public const string COL_SEDOL_SYMBOL = "Sedol Symbol";

        /// <summary>
        /// The col bloomberg symbol
        /// </summary>
        public const string COL_BLOOMBERG_SYMBOL = "Bloomberg Symbol";

        /// <summary>
        /// The col_FxRate
        /// </summary>
        public const string COL_FXRATE = "FxRate";

        /// <summary>
        /// The col_CommissionRule
        /// </summary>
        public const string COL_COMMISSION_RULE = "CommissionRule";

        /// <summary>
        /// The col commission rate
        /// </summary>
        public const string COL_COMMISSION_RATE = "CommissionRate";

        /// <summary>
        /// The col_Use/Select/Specify
        /// </summary>
        public const string COL_USE_SELECT_SPECIFY = "Use/Select/Specify";

        /// <summary>
        /// The col_ FxRateOperator
        /// </summary>
        public const string COL_FXRATE_OPERATOR = "FxRateOperator";

        /// <summary>
        /// The col_ CopyPrefFrom
        /// </summary>
        public const string COL_COPY_PREF_FROM = "CopyFromPreference";

        /// <summary>
        /// The col export folder path
        /// </summary>
        public const string COL_EXPORT_FOLDER_PATH = "ExportFolderPath";

        /// <summary>
        /// The col folder name
        /// </summary>
        public const string COL_FOLDER_NAME = "FolderName";

        /// <summary>
        /// The col import file path
        /// </summary>
        public const string COL_IMPORT_FILE_PATH = "ImportFilePath";

        /// <summary>
        /// The col rename preference from
        /// </summary>
        public const string COL_OLD_PREF_NAME = "OldPreferenceName";

        /// <summary>
        /// The col_ NewPrefName
        /// </summary>
        public const string COL_NEW_PREF_NAME = "NewPreferenceName";

        /// <summary>
        /// The col_InternalComments
        /// </summary>
        public const string COL_INTERNAL_COMMENTS = "InternalComments";

        /// <summary>
        /// The col_ClientProdId
        /// </summary>
        public const string COL_CLIENTPRODID = "ClientProdId";

        /// <summary>
        /// The col_search_Type
        /// </summary>
        public const string COL_SEARCH_TYPE = "Search Type";

        /// <summary>
        /// The col_UDA1
        /// </summary>
        public const string COL_UDA1 = "Custom UDA1";

        /// <summary>
        /// The col_Ticker
        /// </summary>
        public const string COL_TICKER = "Ticker";

        /// <summary>
        /// The col_TickerSymbol
        /// </summary>
        public const string COL_TICKERSYMBOL = "TickerSymbol";

        /// <summary>
        /// The col_ForwardPoints
        /// </summary>
        public const string COL_FORWARDPOINTS = "Forward Points";

        /// <summary>
        /// The col_UserPx
        /// </summary>
        public const string COL_USERPX = "User Px";

        /// <summary>
        /// The col_LastPriceUsed
        /// </summary>
        public const string COL_LAST_PRICE_USED = "LastPriceUsed";

        /// <summary>
        /// The col_Volatility_Used
        /// </summary>
        public const string COL_VOLATILITY_USED = "VolatilityUsed";

        /// <summary>
        /// The col_IntRate_Used
        /// </summary>
        public const string COL_INTRATE_USED = "IntRateUsed";

        /// <summary>
        /// The col_Dividend_Used
        /// </summary>
        public const string COL_DIVIDEND_USED = "DividendUsed";

        /// <summary>
        /// The col_SharesOutstanding_Used
        /// </summary>
        public const string COL_SHAREOUTSTANDING_USED = "SharesOutStandingUsed";

        /// <summary>
        /// The col_ForwardsPoints_Used
        /// </summary>
        public const string COL_FORWARDPOINTS_USED = "ForwardPointsUsed";

        /// <summary>
        /// The col_UserVolatility
        /// </summary>

        public const string COL_USERVOL = "User Volatility";

        /// <summary>
        /// The col_UserDividendIssue
        /// </summary>

        public const string COL_USER_DIVIDEND_ISSUE = "User Dividend Issue";

        /// <summary>
        /// The col_UserDividendYield
        /// </summary>

        public const string COL_USER_DIVIDEND_YIELD = "User Dividend Yield";

        /// <summary>
        /// The col_UserInterestRate
        /// </summary>

        public const string COL_USER_INTEREST_RATE = "User Interest Rate";

        /// <summary>
        /// The col_SharesOutstanding
        /// </summary>
        public const string COL_SHARES_OUT_STANDING = "Shares Outstanding";

        /// <summary>
        /// The col_SharesOutstanding
        /// </summary>
        public const string COL_USER_SHARES_OUTSTANDING = "User Shares Outstanding";

        /// <summary>
        /// The col_SystemString
        /// </summary>
        public const string COL_SYSTEMSTRING = "System.String,System.Object";

        /// <summary>
        /// The col_SystemObject
        /// </summary>
        public const string COL_SYSTEMOBJECT = "System.Object";

        /// <summary>
        /// The col_Asset
        /// </summary>
        public const string COL_ASSET = "Asset";

        /// <summary>
        /// The col_AssetID
        /// </summary>
        public const string COL_ASSETID = "AssetID";

        /// <summary>
        /// The col_Underlying
        /// </summary>
        public const string COL_UNDERLYING = "Underlying";

        /// <summary>
        /// The col_UnderlyingID
        /// </summary>
        public const string COL_UNDERLYINGID = "UnderlyingID";

        /// <summary>
        /// The col_OptionPrice
        /// </summary>
        public const string COL_OPTIONPRICE = "Option Price";

        /// <summary>
        /// The col_Option Override Selected feed price with
        /// </summary>
        public const string COL_OPTION_OVERRIDE_SELECTED_FEED_PRICE = "Option Override Selected feed price with";

        /// <summary>
        /// The col_Asset Override Selected feed price with
        /// </summary>
        public const string COL_ASSET_OVERRIDE_SELECTED_FEED_PRICE = "Asset Override Selected feed price with";

        /// <summary>
        /// The col_AssetPrice
        /// </summary>
        public const string COL_ASSETPRICE = "Asset Price";

        /// <summary>
        /// The col_Currency
        /// </summary>
        public const string COL_CURRENCY = "Currency";
        /// <summary>
        /// The col_Currency
        /// </summary>
        public const string COL_ACTIVITY_TYPE = "ActivityType";

        /// <summary>
        /// The col_Allocation Scheme Key
        /// </summary>
        public const string COL_ALLOCATION_SCHEME_KEY = "Allocation Scheme Key";

        /// <summary>
        /// The col_Use Default Data
        /// </summary>
        public const string COL_USE_DEFAULT_DELTA = "Use Default Delta";
        /// <summary>
        /// The col_CurrencyID
        /// </summary>
        public const string COL_CURRENCYID = "CurrencyID";

        /// <summary>
        /// The col_OpenPSTFromPM
        /// </summary>
        public const string COL_OPENPSTFROMPM = "OpenPSTFromPM";

        /// <summary>
        /// The col_PSTPosition
        /// </summary>
        public const string COL_PSTPOSITION = "PSTPosition";

        /// <summary>
        /// The col_Increase
        /// </summary>
        public const string COL_INCREASE = "increase";

        /// <summary>
        /// The col_Increase
        /// </summary>
        public const string COL_DECREASE = "decrease";

        /// <summary>
        /// The col_PSTFunctionalities
        /// </summary>
        public const string COL_PSTFUNCTIONALITIES = "PSTFunctionalities";

        /// <summary>
        /// The col_IsPSTOpenedFromPM
        /// </summary>
        public const string COL_ISPSTOPENEDFROMPM = "IsPSTOpenedFromPM";

        /// <summary>
        /// The col_PSTInputData
        /// </summary>
        public const string COL_PSTINPUTDATA = "PSTInputData";

        /// <summary>
        /// The col_OrderSide
        /// </summary>
        public const string COL_ORDER_SIDE = "Order Side";

        /// <summary>
        /// The col_Add_set
        /// </summary>
        public const string COL_ADD_SET = "Add/Set";

        /// <summary>
        /// The col_Target
        /// </summary>
        public const string COL_TARGET = "Target";

        /// <summary>
        /// The col_Type
        /// </summary>
        public const string COL_TYPE = "Type";

        /// <summary>
        /// The col_MasterFund
        /// </summary>
        public const string COL_MASTERFUND_fUND = "MasterFund/Account";

        /// <summary>
        /// The col_OF/TO
        /// </summary>
        public const string COL_OFTO = "OF/TO";

        /// <summary>
        /// The col_ConsolidationValue
        /// </summary>
        public const string COL_CONSOLIDATION_VALUE = "Consolidation Value";

        /// <summary>
        /// The combined account total
        /// </summary>
        public const string COL_COMBINED_ACCOUNT_TOTAL = "Combined Account Total";

        /// <summary>
        /// The col_Calculate
        /// </summary>
        public const string COL_CALCULATE = "Calculate";

        /// <summary>
        /// The col_ qtypercent
        /// </summary>
        public const string COL_QTYPERCENT = "QtyPercent";

        /// <summary>
        /// The col_Verify
        /// </summary>
        public const string COL_VERIFY = "Verify";

        /// <summary>
        /// The col_CalculatedValues
        /// </summary>
        public const string COL_CALCULATEDVALUES = "CalculatedValues";

        /// <summary>
        /// The col_PSTVerifyData
        /// </summary>
        public const string COL_PSTVERIFYDATA = "PSTVerifyData";

        /// <summary>
        /// The const_ Records
        /// </summary>
        public const string CONST_RECORDS = "Records";

        /// <summary>
        /// The const_ViewableRecordCollection
        /// </summary>
        public const string CONST_VIEWABLERECORDCOLLECTION = "ViewableRecordCollection";

        /// <summary>
        /// The col_Select Date
        /// </summary>
        public const string COL_SELECT_DATE = "Select Date";

        /// <summary>
        /// The col_Copy Date
        /// </summary>
        public const string COL_COPY_DATE = "Copy Date";

        /// <summary>
        /// The Col_Copy_From 
        /// </summary>
        public const string COL_COPY_FROM = "Copy From";

        /// <summary>
        /// The col_Select File
        /// </summary>
        public const string COL_SELECT_FILE = "Select File";

        /// <summary>
        /// The col principal amount local
        /// </summary>
        public const string COL_PRINCIPAL_AMOUNT_LOCAL = "Principal Amount (Local)";

        /// <summary>
        /// The col principal amount base
        /// </summary>
        public const string COL_PRINCIPAL_AMOUNT_BASE = "Principal Amount (Base)";

        /// <summary>
        /// The col balances as on date 
        /// </summary>
        public const string COL_BALANCES_AS_ON_DATE = "Balances as on date";

        /// <summary>
        /// The col base currency.
        /// </summary>
        public const string COL_BASE_CURRENCY = "Base Currency";

        /// <summary>
        /// The Base cash value.
        /// </summary>

        public const string COL_CASH_VALUE_BASE = "Cash Value Base";

        /// <summary>
        /// The col_Select Record
        /// </summary>
        public const string COL_SELECT_RECORD = "Select Record";

        /// <summary>
        /// The col_ DisabledCheckSideAssets
        /// </summary>
        public const string COL_DISABLED_CHECKSIDE_ASSETS = "DisabledCheckSideAssets";

        /// <summary>
        /// The col_ PrecisionValue
        /// </summary>
        public const string COL_PRECISION_VALUE = "PrecisionValue";

        /// <summary>
        /// The col_ UseCommissionInNetAmount
        /// </summary>
        public const string COL_USE_COMMISSION_IN_NET_AMOUNT = "UseCommissionInNetAmount";

        /// <summary>
        /// The col_ Prorata Scheme Name
        /// </summary>
        public const string COL_PRORATA_SCHEME_NAME = "ProrataSchemeName";

        /// <summary>
        /// The col_ Prorata Allocation Scheme Basis 
        /// </summary>
        public const string COL_PRORATA_ALLOCATION_SCHEME_BASIS = "ProrataAllocationSchemeBasis";

        /// <summary>
        /// The col_ Show advanced Prorata UI 
        /// </summary>
        public const string COL_SHOW_ADVANCED_PRORATA_UI = "ShowAdvancedProrataUI";

        /// <summary>
        /// The col_ Custom 
        /// </summary>
        public const string COL_CUSTOM = "Custom";

        /// <summary>
        /// The col_ ValidateCheckSide
        /// </summary>
        public const string COL_VALIDATE_CHECKSIDE = "ValidateCheckSide";

        /// <summary>
        /// The col_ enable 1 master fund and 1 symbol
        /// </summary>
        public const string COL_1_MASTER_FUND_1_SYMBOL = "1MasterFund/1Symbol";

        /// <summary>
        /// The col_ SaveWithoutState
        /// </summary>
        public const string COL_SAVE_WITHOUT_STATE = "SaveWithoutState";

        /// <summary>
        /// The col_ SaveWithState
        /// </summary>
        public const string COL_SAVE_WITH_STATE = "SaveWithState";

        /// <summary>
        /// The col_ ClearQuantities
        /// </summary>
        public const string COL_CLEAR_QUANTITIES = "ClearQuantities";

        /// <summary>
        /// The col_ ReapplyAllocation
        /// </summary>
        public const string COL_REAPPLY_ALLOCATION = "ReapplyAllocation";

        /// <summary>
        /// The col_ DefaultAllocation
        /// </summary>
        public const string COL_DEFAULT_ALLOCATION = "DefaultAllocation";

        /// <summary>
        /// The col_ IsPTTPreference
        /// </summary>
        public const string COL_IS_CUSTOM_PREFERENCE = "IsCustomPreference";

        /// <summary>
        /// The col_ Strategy name
        /// </summary>
        public const string COL_STRATEGY_NAME = "Strategy Name";

        /// <summary>
        /// The col_ Strategy Percentage
        /// </summary>
        public const string COL_STRATEGY_PERCENTAGE = "Strategy Percentage";

        /// <summary>
        /// The col_ Strategy Quantity
        /// </summary>
        public const string COL_STRATEGY_QUANTITY = "Strategy Quantity";

        /// <summary>
        /// The const_ Default_Start_Date
        /// </summary>
        public const string CONST_DEFAULT_START_DATE = "01/01/1800";

        public const string COL_TICKERID = "TickerID";

        public const string CAP_EMAIL = "email";

        public const string CAP_REPORT_URL = "ReportUrl";

        public const string CAP_PRICING_PATH = "PricingServerPath";

        public const string CAP_SERVERPATH = "TradeServerPath";

        public const string CAP_EXPNL_PATH = "ExpnlServerPath";

        public const string CAP_CLIENT_PATH = "ClientReleasePath";

        public const string CAP_COMPRESSION = "compression";

        public const string CAP_SEND_EMAIL = "SendEmailNotifcations";

        /// <summary>
        /// The col_TickerSymbol
        /// </summary>
        public const string COL_TICKER_SYMBOL = "Ticker Symbol";

        public const string CAP_AUTOMATION_FOLDER = "\\AutomationExports";

        public const string COL_CLOSING_DATE = "Closing Date";

        public const string COL_CLOSING_COST_BASIS = "Closing Cost Basis";

        public const string COL_SPREAD = "Spread(B.P)";

        public const string COL_SHOW_DESCRIPTION = "ShowDescription";

        public const string COL_DAY_COUNT = "Day Count";

        /// <summary>
        /// The col original trade date
        /// </summary>
        public const string COL_ORIGINAL_TRADE_DATE = "Original T.Date";

        public const string CONST_VALUE = "Value";

        public const string CONST_ACCOUNT = "Account";

        public const string CONST_STRATEGY = "Strategy";

        public const string CONST_CLOSING_METHODOLOGY = "Closing Methodology";

        public const string CONST_CLOSING_FIELD = "Closing Field";

        #region Test Reports Constants

        public const string PIE_PATH = "c:chartSpace/c:chart/c:plotArea/c:pie3DChart/c:ser";

        public const string CAP_RELEASE_BRANCH = "Release Branch";

        public const string CAP_PRANA_VERSION = "Prana Version";

        public const string RELEASE_BRANCH_VALUE = "Prod";

        public const string CAP_RELEASE_DATE_TIME = "Release Date/Time";

        public const string CAP_TOTAL_TEST_CASES = "Total Test Cases";

        public const string CAP_PASSED = "Passed";

        public const string CAP_NOT_RUN = "Not Run";

        public const string CAP_FAILED = "Failed";

        public const string CAP_TOTAL_TIME_TAKEN = "Total Time Taken";

        public const string CAP_SYSTEM_IP = "Master System IP";

        public const string CAP_AUTOMATION_BUILD_NUMBER = "Build No.";

        public const string CAP_PRANA_CODE_REVISION = "Prana Code Revision";

        public const string CAP_NOT_RUN_SLAVE = "Not Run Slave";
        /// <summary>
        /// The Start Time
        /// </summary>
        public const string TestCase_Start_Time = "Start Time";

        /// <summary>
        /// The End Time
        /// </summary>
        public const string TestCase_End_Time = "End Time";

        public const string CAP_MODULE_NAME = "Module Name";

        public const string CAP_TEST_CASE_ID = "Test Case Id";

        public const string CAP_TEST_CASE_DESCRIPTION = "Test Case Description";

        public const string CAP_CATEGORY = "Category";

        public const string CAP_ERROR = "Error";

        public const string CAP_RESULT = "Result";

        public const string CAP_RUNNING_TIME = "Running Time";

        public const string CAP_TEST_REPORT = "TestReport";

        public const string CAP_TEST_SUMMARY = "Test Summary";

        public static string Release_Version = "";

        public const string CAP_FAIL_COUNT = "Fail Count";

        #endregion

        /// <summary>
        /// col cash sub account
        /// </summary>
        public const string COL_CASH_SUBACCOUNT = "Cash Sub-Account";

        /// <summary>
        ///  COL_CASH_LOCAL
        /// </summary>
        public const string COL_CASH_LOCAL = "Cash (Local)";

        /// <summary>
        ///  COL_CASH_BASE
        /// </summary>
        public const string COL_CASH_BASE = "Cash (Base)";

        /// <summary>
        ///  COL_CURRENCY_LOCAL
        /// </summary>
        public const string COL_CURRENCY_LOCAL = "Currency (Local)";

        /// <summary>
        ///  COL_CURRENCY_BASE
        /// </summary>
        public const string COL_CURRENCY_BASE = "Currency (Base)";


        /// <summary>
        ///  COL_CURRENCY_BASE
        /// </summary>
        public const string COL_BALANCE_TYPE = "BalanceType";

        /// <summary>
        /// The col transaction entry id
        /// </summary>
        public const string COL_TRANSACTION_ENTRY_ID = "TransactionEntryID";

        /// <summary>
        /// The col account factor
        /// </summary>
        public const string COL_ACCOUNT_FACTOR = "AccountFactor";

        /// <summary>
        /// The col for clicking yes/no on run revaluation popup
        /// </summary>
        public const string COL_IS_RUNREVAL_ON_GETACCOUNTBALANCES = "IsRunRevalOnGetAccountBalances";

        /// <summary>
        /// The col for Average Price Rounding
        /// </summary>
        public const string COL_AVG_PRICE_ROUNDING = "AvgPriceRounding";

        public const string COL_INCREASE_DECIMAL_DIGITS = "Increase Decimal Digits";
        public const string COL_ADD_SUBTRACT_SET = "Add/Subtract/Set";
        public const string COL_REMOVE_ACCOUNT_WITH_ZERO_NAV = "ChkRemoveAccountsWithZeroNAV";


        public const string COL_LOGINUSER = "LoginUserName";
        public const string COL_LOGINPASSWORD = "LoginPassword";


        public const string COL_USE_SHORTLONG_PREFERENCE = "UseLongShortPreference";
        public const string COL_CHECK_USE_PRORATA_PERCERNTAGE = "UseProrataPercentage";
        public const string COL_SHORT_CHECK_USE_PRORATA_PERCERNTAGE = "UseShortProrataPercentage";
        public const string COL_TOTAL_PERCENTAGE = "Total Percentage";
        public const string COL_ACCOUNT_SHORT = "AccountShort";
        public const string COL_ACCOUNT_SHORT_PERCENTAGE = "Account Short Percentage";

        /// <summary>
        /// The col for setting the value of Scheme Name
        /// </summary>
        public const string COL_SCHEME_NAME = "SchemeName";

        /// <summary>
        /// The col for setting the value of Scheme Basis
        /// </summary>
        public const string COL_SCHEME_BASIS = "SchemeBasis";

        /// <summary>
        /// The col for setting the tab name in blotter
        /// </summary>
        public const string TAB_NAME = "TabName";

        /// <summary>
        /// The col for verifying the tab name in blotter
        /// </summary>
        public const string VERIFY_TAB_NAME = "VerifyTabName";

        /// <summary>
        /// The col for verifying the tab name in blotter
        /// </summary>
        public const string REMOVE_TAB_NAME = "TabName";
        /// <summary>
        /// The col for setting the Column name in ColumnChooser
        /// </summary>
        public const string COLUMN_NAME = "Column Name";

        /// <summary>
        /// The col for renaming the old tab name in blotter
        /// </summary>
        public const string RENAME_OLD_TAB_NAME = "OldName";

        /// <summary>
        /// The col for renaming the new tab name in blotter
        /// </summary>
        public const string RENAME_NEW_TAB_NAME = "NewName";

        public const string DUMP_PATH = "\\JavaModules\\prana-esperCalculator\\Export\\dump\\";

        public const string COL_ALLOW_TRADE = "AllowTrade";

        public const int COMPLIANCE_PROCESS_WAIT = 5000;

        /// <summary>
        /// The col for from in journal exceptions of General Ledger
        /// </summary>
        public const string COLUMN_FROM = "FromDate";

        /// <summary>
        /// The col for to in journal exceptions of General Ledger
        /// </summary>
        public const string COLUMN_TO = "ToDate";

        public static string Release_VersionForCharts = "";

        public static string SenderEmailIds = "buildserver@nirvanasolutions.com";
        public static string ReceiverEmailIds = "sumit.chand@nirvanasolutions.com,raj.mishra@nirvanasolutions.com";
        public static string CcEmailIds = "raj.mishra@nirvanasolutions.com";
        public static string EmailSubject = "Prana Automated Testing Report - " + TestDataConstants.RUN_DESCR;
        public static string EmailBody = "This is automated testing report for " + TestDataConstants.RUN_DESCR + "version. Please see the test cases to dig into the details. We welcome all comments/suggestions to improve it further.";
        public static string SlaveSystemIpTag = "Slave System IP";
        public static string DELETEBEFORECOPY = "";

        public static string PASS_RESULT = "Pass";

        public static string FAIL_RESULT = "Fail";

        public static string NOTRUN_RESULT = "Not Run";

        public static string NOTABLETORUN_RESULT = "Not able to run";


    }
}
