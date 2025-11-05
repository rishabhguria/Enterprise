using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Utilities
{
    public static class TestDataConstants
    {
        /// <summary>
        /// Asset Name of trade symbol
        /// </summary>
        public const string COL_ASSET_NAME = "AssetName";

        /// <summary>
        /// Underlying Name of trade symbol.
        /// </summary>
        public const string COL_UNDERLYING_NAME = "UnderLyingName";

        /// <summary>
        /// Exchange currency code of trade symbol.
        /// </summary>
        public const string COL_EXCHANGE_SYMBOL_NAME = "DisplayName";

        /// <summary>
        /// Clearance time for rollover of  that particular trade.
        /// </summary>
        public const string COL_CLEARANCE_TIME = "ClearanceTime";

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
        /// Auto Close Stategy checkbox in Closing Preference
        /// </summary>
        public const string COL_EXERCISE_ASSIGN_CHECKSIDE = "Exercise/Assign CheckSide Validation";

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
        /// The col_ account
        /// </summary>
        public const string COL_Account = "Account";

        /// <summary>
        /// The col_ symbol
        /// </summary>
        public const string COL_IsShortLocate = "ShortLocate";

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
        /// The col_ borrow_quantityy
        /// </summary>
        public const string COL_Borrow_QUANTITY = "Quantity";

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
        public const string COL_DEFAULT_ACCOUNT = "Allocation1";

        /// <summary>
        /// The col_ account
        /// </summary>
        public const string COL_ACCOUNT_PERCENTAGE_VALUE = "Account %";

        /// <summary>
        /// The col custom account
        /// </summary>
        public const string COL_CUSTOM_ALLOCATION_ACCOUNT_VALUE = "CustomAllocationAccount";

        /// <summary>
        /// The col custom %
        /// </summary>
        public const string COL_CUSTOM_ALLOCATION_ACCOUNT_PERCENT_VALUE = "CustomAllocationAllocationPercent";

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
        ///The col custom 
        /// </summary>
        public const string COL_ISCUSTOM = "Custom";

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
        /// The col handling instructions for MTT
        /// </summary>
        public const string COL_HANDLING_INSTRUCTIONSMTT = "Handling Instructions";

        /// <summary>
        /// The col execution instructions
        /// </summary>
        public const string COL_EXECUTION_INSTRUCTIONS = "ExecutionInstructions";

        /// <summary>
        /// The col execution instructions MTT
        /// </summary>
        public const string COL_EXECUTION_INSTRUCTIONSMTT = "Execution Instructions";

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
        /// The COL_Commission Basis
        /// </summary>
        public const string COL_COMMISSION_TYPE = "Commission Basis";

        /// <summary>
        /// The col soft basis
        /// </summary>
        public const string COL_SOFT_BASIS = "SoftBasis";

        /// <summary>
        /// The Col Soft 
        /// </summary>
        public const string COL_SOFTTYPE_BASIS = "Soft Basis";

        /// <summary>
        /// The col soft rate
        /// </summary>
        public const string COL_SOFT_RATE = "SoftRate";

        /// <summary>
        /// The col soft rate MTT
        /// </summary>
        public const string COL_SOFT_RATEMTT = "Soft Rate";

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
        /// The col Algo strategy
        /// </summary>
        public const string COL_ALGO_STRATEGY = "SelectAlgoStrategy";

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
        /// Column to select which option to select in adjust position from PM
        /// </summary>
        public const string Col_AdjustPosition = "AdjustPosition";
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
        /// The col_Default Symbology
        /// </summary>
        public const string COL_SYMBOLOGY = "Default Symbology";

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
        /// The col_ trade attribute 1 MTT
        /// </summary>
        public const string COL_TRADE_ATTRIBUTE1_MTT = "Trade Attribute 1";

        /// <summary>
        /// The col_ trade attribute 2 MTT
        /// </summary>
        public const string COL_TRADE_ATTRIBUTE2_MTT = "Trade Attribute 2";

        /// <summary>
        /// The col_ trade attribute 3 MTT
        /// </summary>
        public const string COL_TRADE_ATTRIBUTE3_MTT = "Trade Attribute 3";

        /// <summary>
        /// The col_ trade attribute 4 MTT
        /// </summary>
        public const string COL_TRADE_ATTRIBUTE4_MTT = "Trade Attribute 4";

        /// <summary>
        /// The col_ trade attribute 5 MTT
        /// </summary>
        public const string COL_TRADE_ATTRIBUTE5_MTT = "Trade Attribute 5";

        /// <summary>
        /// The col_ trade attribute 6 MTT
        /// </summary>
        public const string COL_TRADE_ATTRIBUTE6_MTT = "Trade Attribute 6";



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
        /// The col_ ThirdParty
        /// </summary>
        public const string COL_Import_Type = "Import Type";

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
        /// The col_borrower_id
        /// </summary>

        public const string COL_Borrower_ID = "Borrower ID";


        /// <summary>
        /// The col_CommissionRule
        /// </summary>
        public const string COL_COMMISSION_RULE = "CommissionRule";

        /// <summary>
        /// The col commission rate
        /// </summary>
        public const string COL_COMMISSION_RATE = "CommissionRate";

        /// <summary>
        /// The col Commission rate MTT
        /// </summary>
        public const string COL_COMMISSION_RATEMTT = "Commission Rate";

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
        /// The COL_SM_SHAREOUTSTANDING_USED
        /// </summary>
        public const string COL_SM_SHAREOUTSTANDING = "SM Shares Outstanding";

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

        public const string CAP_FAILED = "Failed";

        public const string NOT_RUN = "Not Run";

        public const string CAP_TOTAL_TIME_TAKEN = "Total Time Taken(hh:mm:ss)";

        public const string CAP_SYSTEM_IP = "System IP";

        public const string CAP_AUTOMATION_CODE_REVISION = "Automation Code Revision";

        public const string CAP_PRANA_CODE_REVISION = "Prana Code Revision";
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

        public const string CAP_RUNNING_TIME = "Running Time(mm:ss)";

        public const string CAP_TEST_REPORT = "TestReport";

        public const string CAP_TEST_SUMMARY = "Test Summary";

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
        /// The Col Select Corporate action
        /// </summary>
        public const string COL_Corporate_Action = "Corporate Action";

        /// <summary>
        /// The Col Select Counter Party
        /// </summary>
        public const string COL_Counter_Party = "Counter Party";

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

        public const string DUMP_PATH = "\\dump\\";

        public const string COL_ALLOW_TRADE = "AllowTrade";
        public const string COL_ALERT_TYPE_V = "Alert Type";

        public const int COMPLIANCE_PROCESS_WAIT = 5000;

        /// <summary>
        /// The col for from in journal exceptions of General Ledger
        /// </summary>
        public const string COLUMN_FROM = "FromDate";

        /// <summary>
        /// The col for to in journal exceptions of General Ledger
        /// </summary>
        public const string COLUMN_TO = "ToDate";




        //----------------------------Items for Rebalancer-------------------------------------------//

        //The col ACCOUNTGROUPS
        public const string COL_ACCOUNTGROUPS = "Account/Groups";

        //The col ACCOUNTPOSITIONS
        public const string COL_ACCOUNTPOSITIONS = "Account Positions";

        //The col CALCULATIONLEVEL
        public const string COL_CALCULATIONLEVEL = "Calculation Level";

        //The col REFRESHSIDE
        public const string COL_REFRESHSIDE = "Refresh";

        //Cash flow
        public const string Text_CASHFLOW = "Cash Flow(USD)";

        //Cash flow impact on NAV
        public const string COL_CASHFLOWIMPACTONNAV = "Cash Flow Impact on NAV";

        //Select model
        public const string COL_SELECTMODEL = "Select Model";

        //Rounding type
        public const string COL_ROUNDINGTYPE = "Rounding Type";

        //Round Lots
        public const string COL_ROUNDLOT = "Round Lot";

        //Round Lots Preference
        public const string COL_ROUNDLOTPREFERENCE = "RoundLotPreference";

        //Reinvest Cash Preference
        public const string COL_REINVESTCASH = "ReinvestCash";

        //Sell to Raise Cash Preference
        public const string COL_SELLTORAISECASH = "Sell to Raise Cash";

        //No Shorting Preference
        public const string COL_NOSHORTING = "No Shorting";

        //Allow Negative Cash Preference
        public const string COL_ALLOWNEGATIVECASH = "Allow Negative Cash";

        //Expand Rebalance Across Securities
        public const string COL_EXPANDREBALANCEACROSSSECURITY = "Rebalance Across Securities";

        //The Ticker symbol
        public const string Text_TICKER = "Ticker";

        //The BloombergSymbol
        public const string Text_BLOOMBERG = "Bloomberg";

        //The IncreaseDecreaseSet DropDown
        public const string COL_IncreaseDecreaseSet = "Set/Inc/Dec";

        //The Target %
        public const string Text_TARGET = "Target%";

        //Price
        public const string Text_PRICE = "Price";

        //Fx Rate
        public const string Text_Fx = "Fx";

        //The Account Or Group Drop Down
        public const string COL_AccountOrGroup = "Account/Group";

        // Trading Rules Reinvest Cash
        public const string COL_ReinvestCheck = "ReinvestCash";

        //Trading Rules Sell to Raise Cash
        public const string COL_SellToRaiseCash = "SellToRaiseCash";

        //Trading Rules Allow Negative Cash 
        public const string COL_AllowNegCash = "AllowNegCash";

        //Trading Rules No Shortning
        public const string COL_NoShortning = "NoShortning";

        //Input for click Clear 
        public const string COL_ClearCalculations = "Clear Calculations";

        //Click for Modify the rebalance 
        public const string COL_ModifyRebalance = "Modify Rebalance";

        //Attribute Renaming Columns in allocation.
        public const string Col_AttributeValue = "AttributeValue";

        //Attribute Renaming Columns in allocation.
        public const string Col_AttributeName = "AttributeName";

        //Attribute Renaming Columns in allocation.
        public const string Col_KeepRecord = "KeepRecord";

        //Attribute Renaming Columns in allocation.
        public const string Col_DefaultValues = "DefaultValues";

        //Pop up quantity on TT for new order prompt
        public const string COL_PopUpQuantity = "PopUpQuantity";

        //Enabling TT Pop up while done away.
        public const string Col_PopUpTargetQty = "PopUpTargetQty";

        //Enabling Confirmation Popup on new order from TT compliance Preference
        public const string COL_PopUpNewOrder = "PopupNewOrder";

        //Enabling Confirmation Popup on CXL from TT compliance Preference
        public const string COL_PopUpCXL = "PopUpCXL";

        //Enabling Confirmation Popup on CXL/Replace from TT compliance Preference
        public const string COL_PopUpReplace = "PopUpReplace";

        //Click on Edit order 
        public const string COL_EditOrder = "EditOrder";
        //Recon module attributes
        public const string Col_ReconFrom = "ReconFrom";

        //Recon module attributes
        public const string Col_ReconClient = "ReconClient";

        public const string COL_IsBreakRealizedPNLSubAccount = "IsBreakRealizedPNLSubAccount";

        //Modules enable disable
        public const string COL_isEnableModule = "isEnableModule";

        //Modules enable disable
        public const string COL_moduleName = "moduleName";

        //Trading Compliance Rules Attributes
        public const string Col_OverSellTradingRule = "OverSellTradingRule";

        //Trading Compliance Rules Attributes
        public const string Col_OverBuyTradingRule = "OverBuyTradingRule";

        //Trading Compliance Rules Attributes
        public const string Col_FatFingerTradingRule = "FatFingerTradingRule";

        //Trading Compliance Rules Attributes
        public const string Col_UnallocatedTradeAlert = "UnallocatedTradeAlert";

        //Trading Compliance Rules Attributes
        public const string Col_DuplicateTradeAlert = "DuplicateTradeAlert";

        //Trading Compliance Rules Attributes
        public const string Col_PendingNewOrderAlert = "PendingNewOrderAlert";

        //Trading Compliance Rules Attributes
        public const string Col_FatFingerPercent = "FatFingerPercent";

        //Trading Compliance Rules Attributes
        public const string Col_DuplicateTradeAlertTime = "DuplicateTradeAlertTime";

        //Trading Compliance Rules Attributes
        public const string Col_PendingNewOrderAlertTime = "PendingNewOrderAlertTime";

        //Trading Compliance Rules Attributes
        public const string Col_FatFingerAccountOrMasterFund = "FatFingerAccountOrMasterFund";

        //Trading Compliance Rules Attributes
        public const string Col_IsAbsoluteAmountOrDefinePercent = "IsAbsoluteAmountOrDefinePercent";

        //Trading Compliance Rules Attributes
        public const string Col_IsInMarketIncluded = "IsInMarketIncluded";

        //Trading Compliance Rules Attributes
        public const string Col_IsSharesOutstandingRule = "IsSharesOutstandingRule";

        //Trading Compliance Rules Attributes
        public const string Col_SharesOutstandingAccountOrMF = "SharesOutstandingAccountOrMF";

        //Trading Compliance Rules Attributes
        public const string Col_SharesOutstandingPercent = "SharesOutstandingPercent";

        //Recon module attributes
        public const string Col_ReconType = "ReconType";

        //Recon module attributes
        public const string Col_ReconFormat = "ReconFormat";

        //Recon module attributes
        public const string Col_ImportFile = "ImportFile";

        public const string ReconExceptionReportPath = "5\\Position\\Test";

        //TT General Preferences Attributes 
        public const string Col_RestrictedSecuritiesList = "RestrictedSecuritiesList";

        //TT General Preferences Attributes
        public const string Col_AllowedSecuritiesList = "AllowedSecuritiesList";

        //TT General Preferences Attributes
        public const string Col_AllowAllUserToCancelReplaceRemove = "AllowAllUserToCancelReplaceRemove";

        //TT General Preferences Attributes
        public const string Col_MasterUserPermission = "MasterUserPermission";

        //TT General Preferences Attributes
        public const string Col_AllowAllUserToTransferTrade = "AllowAllUserToTransferTrade";

        //ExportedSymbolListFileName
        public const string Export_SymbolList = "SymbolList.xls";

        //switch between Ticker and Bloomber on Restricted/allowed List
        public const string Col_Switch = "SwitchTo";
        //ThirdParty Attributes
        public const string COL_ThirdPartyName = "Party Name";

        //TT UI Preferences Attributes
        public const string Col_Default_Quantity = "Default Quantity";

        //$Amount Permission in Permission Level in admin 
        public const string Col_TT = "TT";

        //$Amount Permission in Permission Level in admin 
        public const string Col_PTT = "PTT";



        //short locate import override Attributes
        public const string COL_isImportOverrideOnShortLocate = "isImportOverrideOnShortLocate";

        //short locate structure Attributes
        public const string COL_isShowmasterFundOnShortLocate = "isShowmasterFundOnShortLocate";
        /// <summary>
        /// Select File format on short locate
        /// </summary>
        public const string COL_FileFormat = "FileFormat";

        /// <summary>
        /// Select File format on short locate
        /// </summary>
        public const string COL_ClearFilters = "ClearFilters";

        /// <summary>
        /// Select fees either YTD or 1 Day
        /// </summary>
        public const string COL_Fees = "Fees";

        /// <summary>
        /// Select Alert
        /// </summary>
        public const string COL_Alert = "Alert";

        /// <summary>
        /// Select Rebate fees either BPS or %
        /// </summary>
        public const string COL_RebateFees = "RebateFees";

        /// <summary>
        /// Select field to apply decimal filter
        /// </summary>
        public const string COL_LastPX_Decimal = "LastPX_Decimal";

        /// <summary>
        /// Select field to apply decimal filter
        /// </summary>
        public const string COL_RebateFees_Decimal = "RebateFees_Decimal";

        /// <summary>
        /// Select field to apply decimal filter
        /// </summary>
        public const string COL_TotalAmount_Decimal = "TotalAmount_Decimal";

        /// <summary>
        /// The COL_Expiration_Date
        /// </summary>
        public const string COL_Expiration_Date = "Expiration Date";

        /// <summary>
        /// The Col_UncheckColumn
        /// </summary>
        public const string COL_UncheckColumn = "Uncheck Column";

        /// <summary>
        ///  Old Master Strategy Name in Admin
        /// </summary>
        public const string COL_Old_MasterStrategy = "OldMasterStrategy";

        /// <summary>
        /// New Master Strategy Name in Admin
        /// </summary>
        public const string COL_New_MasterStrategy = "NewMasterStrategy";

        /// <summary>
        /// The COL_Option
        /// </summary>
        public const string COL_Option = "Option";

        /// <summary>
        /// The COL_FeeType
        /// </summary>
        public const string COL_FeeType = "FeeType";

        /// <summary>
        /// The COL_Rate(%)
        /// </summary>
        public const string COL_Rate = "Rate(%)";

        /// <summary>
        /// Security Info on SM
        /// </summary>
        public const string COL_Shares_Outstanding = "SharesOutstanding";

        /// <summary>
        /// Shares Outstanding Pop Up Qty on TT 
        /// </summary>
        public const string Col_Shares_Outstanding_PopUp_Qty = "SharesOutstandingPopUpQty";

        /// <summary>
        /// Shares Outstanding Pop Up (To define whether click on Yes or No)
        /// </summary>
        public const string Col_Shares_Outstanding_PopUp = "SharesOutstandingPopUp";

        /// <summary>
        /// Restricted List / Allowed List Pop Up 
        /// </summary>
        public const string Col_Res_Allowed_List_PopUp = "ResAllowedListPopUp";

        /// <summary>
        /// Target allocation percentage
        /// </summary>
        public const string COL_TARGET_ALLOCATION_PERCENTAGE = "Target Allocation Percentage";

        public const string Col_Broker = "Borrow Broker";

        /// <summary>
        /// The col_  prefrence name present
        /// </summary>
        public const string COL_Is_PrefPresent = "IsPresent";

        /// <summary>
        /// Col to set whether to recalculate commission or not.
        /// </summary>
        public const string COL_Recalculate_Comm_PopUp = "RecalculateCommPopUp";
        // <summary>
        /// The col_ NewRow
        /// </summary>
        public const string COL_New = "New";

        /// <summary>
        /// Row Number matching
        /// </summary>
        public const string ROW_COUNT_CHECK = "isRowCountCheck";

        /// <summary>
        /// EXCHANGE_IDENTIFIER
        /// </summary>
        public const string EXCHANGE_IDENTIFIER = "ExchangeIdentifier";

        /// <summary>
        /// Multiplier
        /// </summary>
        public const string COL_MULTIPLIER = "Multiplier";

        /// <summary>
        /// The col cusip symbol
        /// </summary>
        public const string COL_CUSIP_SYMBOL = "Cusip Symbol";

        /// <summary>
        /// The col isin symbol
        /// </summary>
        public const string COL_ISIN_SYMBOL = "ISIN Symbol";
        /// <summary>
        /// The col UDA Security Type 
        /// </summary>
        public const string UDA_Security_Type = "UDA Security Type";

        /// <summary>
        /// The col UDA Country 
        /// </summary>
        public const string UDA_Country = "UDA Country";

        /// <summary>
        /// The col UDA Asset Class 
        /// </summary>
        public const string UDA_Asset_Class = "UDA Asset Class";

        /// <summary>
        /// The col UDA Sector
        /// </summary>
        public const string UDA_Sector = "UDA Sector";


        /// <summary>
        /// The col UDA Sub Sector
        /// </summary>
        public const string UDA_Sub_Sector = "UDA Sub Sector";

        /// <summary>
        /// The col Put/Call
        /// </summary>
        public const string Col_Put_Call = "Put/Call";

        /// <summary>
        /// The col Strike Price
        /// </summary>
        public const string Col_Strike_Price = "Strike Price";

        /// <summary>
        /// The col IDCO OPtion-22
        /// </summary>
        public const string Col_IDCO_Option = "IDCOOption-22";

        /// <summary>
        /// The col OSI Option-21
        /// </summary>
        public const string Col_OSI_Option = "OSIOption-21";

        /// <summary>
        /// The col_Comments
        /// </summary>
        public const string COL_COMMENTS = "Comments";

        /// <summary>
        /// The col_FactSet Symbol
        /// </summary>
        public const string COL_FactSet_Symbol = "FactSet Symbol";

        /// <summary>
        /// The col_ACTIV Symbol
        /// </summary>
        public const string COL_ACTIV_Symbol = "ACTIV Symbol";

        /// <summary>
        /// The col_Issue Date
        /// </summary>
        public const string COL_Issue_Date = "Issue Date";

        /// <summary>
        /// The col_Accrual basis
        /// </summary>
        public const string COL_Accrual_Basis = "Accrual Basis";

        /// <summary>
        /// The col_Coupon frequency
        /// </summary>
        public const string COL_Coupon_Frequency = "Coupon Frequency";

        /// <summary>
        /// The col_Coupon
        /// </summary>
        public const string COL_Coupon = "Coupon";

        /// <summary>
        /// The col_Bond Type
        /// </summary>
        public const string COL_Bond_Type = "Bond Type";

        /// <summary>
        /// The col_First Coupan Date
        /// </summary>
        public const string COL_First_Coupan_Date = "First Coupan Date";

        /// <summary>
        /// The col_Days To Settlement
        /// </summary>
        public const string COL_Days_To_Settlement = "Days To Settlement";

        /// <summary>
        /// The col_IsZero
        /// </summary>
        public const string COL_IsZero = "IsZero";

        /// <summary>
        /// The col_Collateral type
        /// </summary>
        public const string COL_Collateral_Type = "Collateral Type";

        /// <summary>
        /// The col Leveraged factor
        /// </summary>
        public const string COL_Leveraged_FACTOR = "LeveragedFactor";

        /// <summary>
        /// The col Strike price Multiplier factor
        /// </summary>
        public const string COL_Strike_Price_Multiplier = "Strike Price Multiplier";

        /// <summary>
        /// The col Proxy Symbol
        /// </summary>
        public const string COL_Proxy_Symbol = "Proxy Symbol";

        //////CheckPMLiveFeed ..//////////////////////////////////////////////////////////////////

        /// <summary>
        /// The col COL_Px_Ask
        /// </summary>
        public const string COL_PX_ASK = "Px Ask";
        /// <summary>
        /// The col Proxy COL_Px_Mid
        /// </summary>
        public const string COL_PX_MID = "Px Mid";
        /// <summary>
        /// The col Proxy Px Bid
        /// </summary>
        public const string COL_PX_BID = "Px Bid";
        /// <summary>
        /// The col Proxy Px_Last
        /// </summary>
        public const string COL_PX_LAST = "Px Last";
        /// <summary>
        /// The col Px Selected Feed (Local)
        /// </summary>
        public const string COL_PX_SELECTED_FEED_LOCAL = "Px Selected Feed (Local)";
        /// <summary>
        /// The col ProxyPx Selected Feed (Base)
        /// </summary>
        public const string COL_PX_SELECTED_FEED_BASE = "Px Selected Feed (Base)";
        /// <summary>
        /// The col Proxy "Pricing Source
        /// </summary>
        public const string COL_PRICING_SOURCE = "Pricing Source";

        /// <summary>
        /// To press button accordingly
        /// </summary>
        public const string COL_Btn = "Button";
        /// <summary>
        /// To get value for Merge Blotter oRderr
        /// </summary>
        public const string COL_ALLOWMERGE = "Allow Merge";
        /// <summary>
        /// To get value for Merge Blotter oRderr
        /// </summary>
        public const string COL_BLOTTERBTTN = "ButtonClick";
        /// <summary>
        /// The col for setting the tab name in blotter
        /// </summary>
        public const string BUTTON_NAME = "Button Name";
        /// The col for Alloctn
        /// </summary>
        public const string COL_ALLOCATION = "Allocation";





        /// COLUMNS FOR CUSTOM CASH FLOW
        //ALLOW CUSTOM CASH FLOW => VALUE TRUE FOR CUSTOM CASH FLOW
        /// </summary>
        public const string COL_ALLOW_CUSTOMCASHFLOW = "Custom Cash Flow";
        //Comma separated string of accounts
        /// </summary>
        public const string COL_ACCOUNTSLIST = "Accounts";
        // CASH FLOW => VALUE Comma separated string of Account Wise cashflow
        /// </summary>
        public const string COL_CASHFLOWACCOUNTWISE = "Cash Flow Account Wise";
        //ALLOW CUSTOM CASH FLOW => VALUE TRUE FOR CUSTOM CASH FLOW
        /// </summary>
        public const string COL_MFGROUPS = "MasterFund Groups";

        /// <summary>
        /// Select Symbol for RoundLot
        /// </summary>
        public const string COL_SYM = "Symbol Update";

        /// <summary>
        /// Alert Type Permisssion
        /// </summary>
        public const string COL_RULENAME = "Rule Name";
        /// <summary>
        /// Alert Type Permisssion
        /// </summary>
        public const string COL_ALERT_PERMISSION_TYPE = "Alert Type Permission";
        /// <summary>
        /// Alert Type Permisssion
        public const string COL_CLICKONMUL = "ClickonMultiple";
        /// <summary>
        /// Alert Type Permisssion
        ///  /// The col Alert Type in Basket compliance Popup
        /// </summary>
        public const string COL_ALERT_TYPE = "Alert Type";
        /// <summary>
        /// The col User Notes in Basket compliance Popup
        /// </summary>
        public const string COL_USER_NOTES = "User Notes";
        /// The col User Notes in Basket compliance Popup
        /// </summary>
        public const string USERNOTES = "User Soft Note comment";
        /// The col User Notes in Basket compliance Popup
        /// </summary>
        public const string USERNOTES_YESORNO = "Write UserNote";


        /////THE COL for COmpliance Engine Approvalor reJECT
        public const string COL_APPROVEORREJECTALL = "ApproveorRejectAll";


        /// <summary>
        /// To Update RoundLot
        /// </summary>
        public const string COL_ROUNDLOTSM = "RoundLot";



        /// The col for Trade done by user
        /// </summary>
        public const string COL_TRADEORDER = "Trade";

        ///the column for taking import path for custom add cash
        /// /// </summary>
        public const string CUSTOMCASHIMPORT = "Import Path";

        /// The Expnl update config key
        /// </summary>
        public const string CALCULATEFXGAINLOSSONFUTURES = "CalculateFxGainLossOnFutures";
        /// The Expnl update config key
        /// </summary>
        public const string ISM2MINCLUDEDINCASH = "IsM2MIncludedInCash";
        /// The Expnl update config VALUE
        /// </summary>
        public const string KEYVALUETRUE = "True";
        /// The Expnl update config VALUE
        /// </summary>
        public const string KEYVALUEFALSE = "false";

        /// The PRANA update config key
        /// </summary>
        public const string ISDEFAULTFILTERTOSHOWACCOUNTWISEDATAONDAILYVALUATION = "IsDefaultFilterToShowAccountWiseDataOnDailyValuation";

        /// The PRANA update config key
        /// </summary>
        public const string ISFILTERINGACCOUNTWISEDATAALLOWEDONDAILYVALUATION = "IsFilteringAccountWiseDataAllowedOnDailyValuation";

        /// The PRANA update config key
        /// </summary>
        public const string ISCSVEXPORTENABLED_PTT = "IsCSVExportEnabled_PTT";

        /// The PRANA update config key
        /// </summary>
        public const string CSVEXPORTFILEPATH_PTT = "CSVExportFilePath_PTT";

        /// The PRANA update config key
        /// </summary>
        public const string ISGROUPINGREQBLOTTERSTAGEIMPORT = "IsGroupingReqBlotterStageImport";

        /// Update COL_MARKPRICE_SYMBOL
        /// </summary>
        public const string COL_MARKPRICE_SYMBOL = "Enter Symbol";


        ///
        /// </summary>
        public const string COL_CLICK_ON_COPY = "Click On Copy";
        /// Blank mp-Symbol
        /// </summary>
        public const string COL_Symbol_DATA = "Symbol";
        /// Blank mp-Bloomberg Symbol
        /// </summary>
        public const string COL_BLOOMBERGSymbol_DATA = "Bloomberg Symbol";
        ///  Account
        /// </summary>
        public const string COL_ACCOUNT_DATA = "Account";

        /// Blank Account
        /// </summary>
        public const string COL_MARKPRICE_DATA = "MarkPrice";

        /// <summary>
        /// The Col_Copy_From 
        /// </summary>
        public const string COL_COPY_TO = "Copy To";


        /// CreateOrderPTT POPUPCOMPLIANCE-.SOFTWITHNOTES COMMENT VALUE
        /// </summary>
        public const string COL_SOFTNOTESCOUNT = "SoftNotesCount";
        /// CreateOrderPTT POPUPCOMPLIANCE-.SOFTWITHNOTES COMMENT VALUE
        /// </summary>
        public const string COL_SOFTNOTESHANDLE = "SoftNotesComment";
        /// CreateOrderPTT POPUPCOMPLIANCE-NO CLICK
        /// </summary>
        public const string COL_TRADENOTALLOW = "DoNotAllowTrade";
        ///
        /// The setcompliancepermission userid</summary>
        public const string USERID = "UserID";

        /// <summary>
        /// The col_ Increase quantity
        /// </summary>
        public const string COL_INCQUANTITY = "Increase On Quantity";

        /// <summary>
        /// The col_ Increase Stop
        /// </summary>
        public const string COL_NUMINCSTOP = "Increase On Stop";

        /// <summary>
        /// The col_ Increase Limit
        /// </summary>
        public const string COL_NUMINCLIMIT = "Increase On Limit";

        /// <summary>
        /// The col_ Apply RoundLots
        /// </summary>
        public const string COL_APPLYROUNDLOT = "Apply Round Lots";

        /// <summary>
        /// The col_ Increase/Decrease Quantity
        /// </summary>
        public const string COL_INC_DEC_QUANTITY = "Increase/Decrease Quantity";

        /// <summary>
        /// The col_ Increase/Decrease Quantity
        /// </summary>
        public const string COL_ROUND_LOTS = "Round Lots";

        /// column for testing type </summary>
        public const string TESTINGTYPE = "Testing Type";
        // Main testing column
        /// column for testing type </summary>
        public const string TESTINGCOLUMN = "Testing Column";

        public const string OTHERITEMSIMPACTINGNAV = "Other Items Impacting NAV";

        public const string NAVCALCULATIONPREFERENCE = "NAV Calculation Preference";

        public const string ACCOUNTGROUPSVISIBILITY = "Account/Groups Visibility";

        public const string ACCOUNTDIVIDEGROUPSINVISIBILITY = "Account/Groups Invisibility";

        public const string ALWAYSASKFORSAVINGORDERS = "Always Ask for Saving Orders";

        public const string ROUNDINGTYPE = "Rounding Type";

        public const string ALLOWCAMELCASE = "AllowCamelCase";

        public const string ACTIVSYMBOL = "ACTIV Symbol";

        public const string ADDCUSTOMGROUP = "Add Custom Group";
        /// column for testing type </summary>
        public const string TESTINGCOLUMNVAL = "Testing Col Value";
        /// column for testing type </summary>
        public const string DUPLICATEROW = "DuplicateRow";
        /// column for testing type </summary>
        public const string DUPLICATECOUNTONUI = "DuplicateCountOnUI";
        // COLUMN UPDATE DEFAULT UDA IN SM
        public const string USERDEFAULTUDA = "UseDefaultUDA";

        // COLUMN UPDATE EDITSMGRIDUI
        public const string USEUNDERLYINGROOTORROOT = "UseUDAFromUnderlyingOrRoot";
        // COLUMN UPDATE EDITSMGRIDUI
        public const string ISNDF = "Is NDF";
        // COLUMN UPDATE EDITSMGRIDUI
        public const string ISZERO = "IsZero";
        // COLUMN UPDATE EDITSMGRIDUI
        public const string ISSECAPPROVED = "IsSecApproved";


        /// column for removing Old CA WHILE REPLACING ORDER </summary>
        public const string CLEAROLDCA = "ClearOldCA";

        public const string COL_PERCENTAGE_CHANGE = "% Change";

        /// </summary>
        /// Column for verify count and sum of selected account and target qty resp. 
        /// </summary>
        public const string COUNT_ACCOUNT_SUM_TARGETQTY = "Count(Orders),Sum(TargetQty)";

        // /// column for editing interceptor file</summary>
        public const string SYMBOLWITHDATA = "Interceptor Symbol Data";

        //const string for export 
        /// column for SAVE LAYOUT ON THE SUMMARY TAB </summary>
        public const string SAVELAYOUT = "Save Layout";

        /// column for removing filter on teh summary tab </summary>
        public const string REMOVEFILTER = "Remove Filter";
        /// <summary>
        /// The pre written file content of symbol(esper file)
        /// </summary>
        public const string FILE_CONTENT = "symbol, underlyingSymbol, askPrice, bidPrice, lowPrice, highPrice, openPrice, closePrice, lastPrice, selectedFeedPrice, conversionMethod, markPrice, delta, beta5YearMonthly, assetId, openInterest, avgVolume20Days, sharesOutstanding";
        /// <summary>
        /// RestartRelease and Services
        /// </summary>
        /// 
        public const string RESTARTBASKET = "Restart Basket";

        public const string COL_BULKALERT = "Bulk Update Alert";

        public const string COL_CANCELWITHOUTREPLACE = "CancelWithoutReplace";

        public const string COL_BULKOREDITTARDE = "BULK_OR_EDITTRADE";

        public const string COL_CLRBtn = "Click on Clear";

        public const string COL_CLICK_CLEAR_AFTER_ENTERING_VALUES = "ClickClearNotUpdate";

        public const string COL_INFORMATION = "INFORMATIONRELATEDPOPUP_YESORNO";

        public const string COL_MTTPOPUP = "MTT POP UP?";

        /// </summary>
        /// Column for Column Order Pending New Popup 
        /// </summary>
        public const string COL_ORDER_PENDING_NEW_POPUP = "OrderPendingNewPopup";

        /// <summary>
        /// The col_MasterFunds
        /// </summary>
        public const string COL_MASTERFUNDS = "Master Funds";

        public const string COL_ALLOCATIONSCHEMENAME = "Allocation Scheme Name";

        /// </summary>
        /// Column for Column Duplicate Trade
        /// </summary>
        public const string COL_DUPLICATE_TRADE_POPUP = "Allow Duplicate Trade";

        /// <summary>
        /// path of original Prana Preference folder
        /// </summary>
        public const string PREFERENCE_PATH = @"E:\DistributedAutomation\TestAutomationDev\Release\Client Release\Prana Preferences";

        /// <summary>
        /// path of expected Prana Preference folder 
        /// </summary>
        public const string PREFERENCE_PATH_COMPARE = @"E:\DistributedAutomation\TestAutomationDev\Release\Client Release\Prana Preferences_Merger";

        /// <summary>
        /// Column name to compare preferences
        /// </summary>
        public const string COMPARE_FOLDER = "CompareFolder";

        /// <summary>
        /// path of export result of prferences
        /// </summary>
        public const string EXPORT_PATH = @"E:\DistributedAutomation\TestAutomationDev\Release\Backup\PreferenceResult.xlsx";

        public const string COL_ALLOWTRANSFERTOUSER = "Allow Transfer";

        public const string COL_LOWERSTRIPMESSAGE = "LowerStripMessage";

        public const string COL_TARGETALLOCATIONQUANTITY = "Target Allocation Quantity";

        public const string COL_ALGOTYPE = "Algo Type";
        //REPORT MEMBER NAME
        public const string COL_MEMBER = "Member";

        public const string COL_CUSTOMACCOUNT = "Custom Account";

        public const string COL_DEFAULTIF = "Day";

        public const string COL_DEFAULORDERTYPE = "Market";

        public const string COL_DEFAULTTRADER = "GCCAP1";

        public const string COL_TRVACCOUNTNAME = "TRVAccountName";

        public const string COL_TRVTRADEQUANTITY = "TRVTradeQuantity";

        public const string COL_TRVCURRENTPOSITION = "TRVCurrentPosition";

        public const string COL_TRVNAV = "TRVNav";

        public const string COL_TAG = "TAG";

        public const string COL_VALUE = "TAG VALUE";
        public const string COL_ORIGINALDATE = "OriginalPurchaseDate";
        public const string COL_CLICK_ON_COMMIT = "ClickOnCommit";
        public const string COL_WARNING_RESPONSE = "Click Commit & Send/Review";
        public const string COL_VERIFY_CLICKABILITY = "VerifyUnclickabilityOutsidePopup";
        public const string COL_SORTINGCOLUMNNAME = "SortingColumnName";
        public const string COL_SORT_ASC_DSC = "Sort ASC OR DSC";
        /// <summary>
        /// The col update preference
        /// </summary>
        public const string COL_UPDATE_PREFERENCE = "Update Preference";


        public const string COL_SEND_TO_STAGING = "SendToStaging";


        /// <summary>
        /// The col BPS Set
        /// </summary>
        public const string COL_BPS_SET = "BPS SET";
        public const string COL_AUTO_OPTN_EX_VALUE = "Auto Option Excercise Value";

        public const string COL_STRIKEPRICE = "Strike Price";

        public const string COL_PREFCHECK = "LongShortPreferenceChecked";

        public const string COL_SETRESPONSE = "SetResponse";

        public const string COL_CHECKBOX = "checkBox";

        public const string COL_TRUEVALUE = "true";

        public const string COL_FALSEVALUE = "false";

        public const string COL_BBGSYMBOLOGYUSED = "BBG Symbol Used";


        /// <summary>
        /// The col User Adjusted Nav Set
        /// </summary>
        public const string COL_USERADJUSTEDNAV = "User Adjusted Nav";

        public const string COL_CHECK_ACCOUNTVALUE = "IsAccountBlank";
        public const string COL_SEARCH_BEFORE_EDIT = "SearchBeforeEdit";

        public static string COL_VERIFY_ON_SM = "Verify On SM";

        public static string COLLIST_TO_VERIFY_ON_SM = "Columns To Verify On SM";
        public static string COL_SAVETHEDATA = "SaveTheData";
        public static string COL_STATUSMESSAGE = "StatusMessage";
        public static string COL_VERIFYROUNDLOTS = "Verify RoundLots In Input Parameters";


        public static string COL_CANCEL_IMPORT_BOX = "CancelImportBox";
        public static string COL_OPEN_IMPORT_BOX = "OpenImportBox";
        public static string COL_BPSORPERCENT = "BPS/%";
        public static string COL_AACOUNTORGRP = "AccountOrGroup";


        public static string COL_CLICK_CONTINUE = "ClickContinue";
        public static string COL_CLICK_ABORT = "ClickAbort";
        public static string COL_CLICK_SECURITYMASTER = "ClickSecurityMaster";
        public static string COL_ABORTPROCESSRESPONSE = "AbortImportProcess(Yes/No)";
        public static string COL_MESSAGEBOX = "Verify Message Sum of updated accounts not equal";

        public static string COL_MARKETTYPE = "Market Type";
        public static string COL_BLOCKDATA = "Block Data";
        public static string WINAPP = "WINAPP";
        public static string TAFX = "TAFX";
        public static string COL_ORDERBUTTONTYPE = "OrderButtonType";
        public static string COL_SELECT_ACTION = "SELECT";
        public static string COL_EDIT_ACTION = "EDIT";
        public static string COL_BTNCLICK_ACTION = "BTNCLICK";

        public static string BTN_TRADE = "TRADE";
        public static string BTN_CREATE_ORDER = "CREATE ORDER";
        public static string BTN_PREF = "PREFERENCES";
        public static string BTN_EXPORT = "EXPORT";

        public static string Allocate = "Allocate";
        public static string Cancel = "Cancel";
        public static string Replace = "Replace";
        public static string ReloadOrder = "Reload Order";
        public static string TransferToUser = "Transfer to User";
        public static string Add_Modify_Fills = "Add/Modify Fills";
        public static string Save_Layout = "Save Layout";
        public static string GoToAllocation = "Go to Allocation";
        public static string RemoveExecution = "Remove Execution";
        public static string RemoveFilter = "Remove Filter";
        public static string Trade_New_Sub = "Trade (New Sub)";
        public static string Cancel_All_Subs = "Cancel (All Subs)";
        public static string Edit_Order_s = "Edit Order(s)";
        public static string Remove_Order = "Remove Order";
        public static string Transfer_to_User = "Transfer to User";
        public static string ColSaveTransaction = "SaveTransaction";
        public static string Audit_Trail = "Audit Trail";
        public static string ViewAllocation = "View Allocation";


        public static string COL_RoundLotStrip = "RoundLotStrip";
        public static string COL_ORDERBUTTON = "Order type button";
        public static string COL_TT_Control = "TT_Open";

        //button status on TT
        public static string COL_Button_Status = "Button_Status";
        public static string COL_HIDETARGETQUANTITY = "ShowHideTargetQuantity";
        public static string COL_Position = "Position";

        public static string COL_Allow_Remove = "Allow Remove";

       
        public static string PORTFOLIONAME = "Portfolio Name";
        public static string PORTFOLIOTYPE = "Portfolio Type";
        public static string POSITIONTYPE = "Position Type";
        public static string MODELTYPE = "Model Type";
        public static string ACCOUNTNAME = "Account";
        public static string MASTERFUNDNAME = "MasterFund";
        public static string CUSTOMGROUP = "Custom Group";
        public static string IMPORTMODELPORTFOLIOPATH = "ImportModelPortfolioPath";
        public static string COL_REBALANCEACROSSSECURITIESPROPERTIES = "/=/-/=:";
        public static string COL_DB_Check = "DB_Check";

        public static string Mtt = "MultiTradingTicket";
        public static string COL_VERIFYSUGGESTIONS = "VerifySuggestions";
        public static string COL_PRESSPRICEBTN = "PriceButtonPressCount";

        public static string COL_CopyFrom = "Copy From";

        public static string COL_CopyTo = "Copy To";
        public static string COL_IncludeGrouping = "Include Grouping";

        public static string COL_IncludeFilters = "Include Filters";

        public static string COL_DefaultView = "Default View";
        public static string COL_ViewSuccessfullPopUp = "View Successful PopUp";

        public static string COL_Symbol_Name = "Symbol Name";
        public static string COL_Tab_Valid = "Tab Validity";

        public static string COL_DONEAWAYCLICKONNO = "DoneAwayClickOnNO";
        public static string COL_SENDCLICKONNO = "SendClickOnNO";

        public static string COL_COMMISSIONCALCULATION = "CommissionCalculation";

        public static string COL_Last = "Last";
        public static string COL_Change = "Change";
        public static string COL_Bid = "Bid";
        public static string COL_Ask = "Ask";
        public static string COL_COMPLIANCETRADER = "Trdaer";
        public static string COL_COMPLIANCEMANUALTRADER = "Manual Trader";
        public static string COL_COMPLIANCESTAGING = "Staging";

        public static string COL_TestCaseID = "TestCaseID";
        public static string COL_StepsToRunOnEnterprise = "StepsToRunOnEnterprise";
        public static string COL_StepsToRunOnSamsara = "TStepsToRunOnSamsara";
     
        public static string COL_pick = "SymbolToPickFromSuggesstions";
        public static string COL_TypedSymbolPresent = "TypedSymbolAvailableOnSuggestion";
        public static string TT = "TradingTicket";

        public static string PendingApprovalPopUp = "PendingApprovalPopUp";

        public static string WrongFormatPopUp = "WrongFormatPopUp";


        // for Custodian broker column in PTT
        public static string Cusdtodian_Broker = "Use Custodian Broker";

        // for Quantity in Upload Stage order
        public static string Order_Quantity = "Order Quantity";

        // for Count in Upload Stage order
        public static string Order_Count = "Order Count";

        // for Count in View allocation Verification in Blotter from allocate option
        public static string ViewAl_Verify = "ViewAllocationVerfication";

        public static string Col_CustomTabName = "CustomTabName";
        public static string Col_CancelAllSubsOnHeader = "CancelAllSubsOnHeader";
        public static string Col_PendingApprovalPopUp = "PendingApprovalPopUp";

        // Approve or reject popup for any action
        public static string ApprovalForAction = "ApprovalForAction";
        public static string Broker_Enable = "Broker_Enable?";
        //SimulatorActions
        public const string SetResponseTo = "SetResponseTo";
        public const string Manual = "Manual";
        public const string Auto = "Auto";
        public const string DoneForDay = "DoneForDay";
        public const string Clear = "Clear";
        public const string VerifyVisibility = "VerifyVisibility";

        //Padlock of Custodian Broker
        public static string PadLock_Visiblity = "Padlock Visiblity";
        public static string Padlock_Unlock = "Padlock Unlock";

        public static string Cusdtodian_BrokerRB = "Use Custodian Broker";
        public static string SM_FromTT = "FromTT?";
        public static string RestartServer = "RestartServer";
        public static string RestartClient = "RestartClient";
        public static string RestartReleasesAndServices = "RestartReleasesAndServices";
        //Set cash target and set cash target % of Cash Specific Rule
        public const string Col_SetCashTarget = "SetCashTarget";
        public const string Col_SetCashTargetPercentage = "SetCashTarget%";
        public const string Col_SellTRaiseAndSetCTarget = "SellToRaiseCashAndSetCashTarget";
        public const string Col_AllowNegCashAndSetCTarget = "AllowNegCashAndSetCTarget";
        public const string Col_AllowNegCashAndSellToRaise = "AllowNegCashAndSellToRaise";
        public const string Col_DisableCT = "DisableCT";
        public const string Col_Multiplier = "Multiplier";
        public const string COL_CUSIP = "CUSIP";
        public const string COL_BLOOMBERG = "BB Code";
        public const string COL_MODULENAME = "ModuleName";
        public const string COL_CASEID = "CaseID";
        public static string LoginClient = "LoginClient";

        // Broker Padlock 
        public static string Broker_PadLock = "Broker PadLock?";

        /// <summary>
        /// The col Subject of Email
        /// </summary>
        public static string COL_SUBJECT = "Subject";

        /// <summary>
        /// The col path to verify Email content
        /// </summary>
        public static string COL_PATH = "Path";


        public static string COL_DASHBOARDPARENTCHILD = "DashboardName";

        public static string COL_LINKINGTABNAME = "LinkingTabName";

        public static string COL_EXTRACTTABNAME = "ExtractTabName";

        public static string COL_COLOR = "Color";

        public static string COL_POSITION_ACTION = "PositionAction";
        public static string COL_VERIFYOPTIONLIST =    "VerifyOptionsList";
        public static string COL_UNLINKBEFOREVERIFY = "UnlinkBeforeVerify";
        public static string COL_AFTEREXTRACTDN = "AfterExtractDashboardName";

        //
        public static string COL_FROMDATE = "FromDate";
        public static string COL_TODATE = "ToDate";

        // Rules for replacing trade from TT and Limit Price Rules
        public static string COL_LimitPriceRulesForStageOrdersOnly = "LimitPriceRulesForStageOrdersOnly";
        public static string COL_LimitRulesForSubOrders = "LimitRulesForSubOrders";
        public static string COL_LimitRulesForOrdersExceptStagedAndSubOrders = "LimitRulesForOrdersExceptStagedAndSubOrders";
        public static string COL_LimitPriceRules = "LimitPriceRules";
        public static string RESET_COL = "RESET";

        // Commission Rule Recalculate
        public static string COL_CommissionRuleRecalculate = "CommissionRuleRecalculate";
        public static string COL_CONFIRM = "Cancel";

        public static string COL_SourcePath = "SourcePath";
        public static string COL_DestPath = "DestinationPath";

        // EXPNL update config key
        public const string FETCHDATABYDATE = "FetchDataByDate";
        public const string EQUITYSWAPSMARKETVALUEASEQUITY = "EquitySwapsMarketValueAsEquity";

        // Server update config key
        public const string MATCHCLOSINGTRANSACTIONATPORTFOLIOONLY = "MatchClosingTransactionAtPortfolioOnly";
        public const string USECACHEINALLOCATION = "UseCacheInAllocation";
        public const string CREATECACHEINALLOCATION = "CreateCacheInAllocation";
        public static string VerifyTradeColourSimulator = "VerifyTradeColor";
        public static string VerifyAcknowledgeTradeColourSimulator = "VerifyAcknowledgeColour";
        public static string VerifyExecutedTradeColourSimulator = "VerifyExecutedColour";
        public static string VerifyRejectedTradeColourSimulator = "VerifyTradeRejectedColour";

        // ShortLoacte Perferences
        public static string IsImportOverrideOnShortLocate = "IsImportOverrideOnShortLocate";
        public static string CheckBox = "CheckBox";

        public static string Action_on_toolbar = "Action on toolbar";

        //Blotter strip actions
        public static string Refresh_Data = "Refresh Data";
        public static string AddWorkingTab = "Add Tab (Working)";
        public static string AddOrderTab = "Add Tab (Order)";
        public static string SaveLayout = "Save All Layout";
        public static string RemoveOrders = "Remove Orders";
        public static string CancelAllSubs = "Cancel All Subs";
        public static string Rollover_All_Subs = "Rollover All Subs";
        public static string LinkTab = "Link Tab";

        //TT general preferences
        public static string CLEARCOLVALUE = "ClearColumnValue";
        public static string Use_Tolerance = "Use Tolerance";
        public static string In_Percentage_In_BPS = "In Percentage/In BPS";

        //Account Permission
        public static string Col_Allocate_Unallocate = "Allocate/Unallocate";
        public const string COL_ButtonNo = "ButtonNo";
        public const string COL_ButtonYes = "ButtonYes";
        public const string COL_OptMonthly = "OptMonthly";


        public static string Col_MenuItemBuy = "MenuItemBuy";
        public const string COL_MenuItemSellShort = "MenuItemSellShort";
        public const string COL_MenuItemAdjustPosition = "MenuItemAdjustPosition";
        public const string COL_MenuItemDeleteSymbol = "MenuItemDeleteSymbol";
        public const string COL_MenuItemRemoveFilter = "MenuItemRemoveFilter";
        public const string COL_MenuItemSaveLayout = "MenuItemSaveLayout";

        public const string COL_Fx_Rate = "Fx Rate";
        public const string COL_Fx_Opertor = "Fx Operator";

        public const string COL_AllocateButtonNotPresent = "AllocateButtonNotPresent";

        public const string COL_FILEPATH = "File Path";
        public const string COL_USER = "User";
        public const string COL_OpenModuleConfigName = "OpenModuleConfigName";

        public const string COL_MFPREF = "NewMF4,NewMF1,NewMF2,NewMF3,NewMF";


        public const string COL_EMPTYGRIDDATA = "EmptyGridData";
        public const string COL_ROWINDEXWISE = "ClickonRowIndexWise";
        public const string COL_PENDINGAPPROVALPOPUP = "PendingApprovalPopup";

        public const string Col_CancelOrderPopup = "CancelOrderPopup(Yes/No)";
        public const string grdNetPosition = "grdNetPosition";

        public const string Sheet_CaseWiseFixingApproach = "CaseWiseFixingApproach";
        public const string Sheet_ColumnValueReplace = "ColumnValueReplace";
        public const string Sheet_StepWiseMainColumns = "StepWiseMainColumns";
    }
}