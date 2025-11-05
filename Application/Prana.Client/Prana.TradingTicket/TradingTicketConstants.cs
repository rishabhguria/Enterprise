namespace Prana.TradingTicket
{
    public class TradingTicketConstants
    {
        #region Tab keys
        public const string TAB_SWAP_KEY = "tabSwap";
        public const string TAB_ALGO_KEY = "tabAlgo";
        public const string TAB_OTHER_KEY = "tabOther";
        public const string TAB_SETTLEMENT_KEY = "tabSettlement";
        public const string TAB_COMMISION_KEY = "tabCommision";
        public const string TAB_BorrowParameter_Key = "tabBorrowParameter";
        public const string MULTI_TRADING_TICKET_FILE = "MultiTradingTicket.xml";
        public const string PTT = "PTT";
        public const string REBAL = "Rebal";
        public const string CUSTOM_FIXED = "Custom Fixed";
        public const string PRICING_SERVICE_ADDRESS = "PricingServiceEndpointAddress";

        #endregion

        #region Caption

        public const string CAPTION_SYMBOL = "Symbol";
        public const string CAPTION_TradeAttribute1 = "Trade Attribute 1";
        public const string CAPTION_TradeAttribute2 = "Trade Attribute 2";
        public const string CAPTION_TradeAttribute3 = "Trade Attribute 3";
        public const string CAPTION_TradeAttribute4 = "Trade Attribute 4";
        public const string CAPTION_TradeAttribute5 = "Trade Attribute 5";
        public const string CAPTION_TradeAttribute6 = "Trade Attribute 6";
        public const string CAPTION_ORDER_SIDE = "Order Side";
        public const string CAPTION_QUANTITY = "Quantity";
        public const string CAPTION_TARGET_QUANTITY = "Target Quantity";
        public const string CAPTION_ORDER_TYPE = "Order Type";
        public const string CAPTION_TIF = "TIF";
        public const string CAPTION_ALLOCATION = "Allocation";
        public const string CAPTION_PRICE = "Price";
        public const string CAPTION_BROKER = "Broker";
        public const string CAPTION_STOP = "Stop";
        public const string CAPTION_LIMIT = "Limit";
        public const string CAPTION_TRADE_DATE = "Trade Date";
        public const string CAPTION_FX_RATE = "FX Rate";
        public const string CAPTION_FX_OPERATOR = "FX Operator";
        public const string CAPTION_STRATEGY = "Strategy";
        public const string CAPTION_NOTES = "Notes";
        public const string CAPTION_BROKER_NOTES = "Broker Notes";
        public const string CAPTION_COMMISSION_BASIS = "Commission Basis";
        public const string CAPTION_COMMISSION_RATE = "Commission Rate";
        public const string CAPTION_SOFT_BASIS = "Soft Basis";
        public const string CAPTION_SOFT_RATE = "Soft Rate";
        public const string CAPTION_VENUE = "Venue";
        public const string CAPTION_EXECUTION_INSTRUCTIONS = "Execution Instructions";
        public const string CAPTION_HANDLING_INSTRUCTIONS = "Handling Instructions";
        public const string CAPTION_TRADER = "Trader";
        public const string CAPTION_SETTLEMENT_CURRENCY = "Settlement Currency";
        public const string CAPTION_DOLLAR_QUANTITY = "$Amount";
        public const string CAPTION_DOLLAR_TARGET_QUANTITY = "Target $Amount";
        public const string CAPTION_FACTSETSYMBOL = "FactSet Symbol";
        public const string CAPTION_ACTIVSYMBOL = "ACTIV Symbol";

        //Multi Trading Ticket
        public const string CAPTION_COMMITTEDQUANTITY = "Committed Quantity";
        public const string CAPTION_MANUALLYEXECUTEDQUANTITY = "Executed Quantity(Manual)";
        public const string CAPTION_MARKETPRICE = "Market Price";
        public const string CAPTION_PREVIOUSLYSENTQUANTITY = "Previous Executed/Sent Quantity";
        public const string CAPTION_PERCENTAGECOMMITTEDQUANTITY = "% Committed Qty";
        public const string CAPTION_SELECTCHECKBOX = "Select Trades";
        public const string CAPTION_SELECTMANUALDATAENTRY = "Manual Data Entry Enabled";
        public const string CAPTION_TOTALQUANTITY = "Total Quantity";
        public const string CAPTION_UNCOMMITTEDQUANTITY = "Uncommitted Quantity Left";
        public const string CAPTION_LIMITPRICE = "Limit Price";
        public const string CAPTION_ALGOTYPE = "Algo Type";
        public const string CAPTION_NA_SLASH = "N/A";
        public const string CAPTION_NA_DOT = "N.A.";
        public const string CAPTION_NONE = "None";
        public const string CAPTION_SELECT_EXPIRY_DATE = "Select Expiry Date";
        public const string CAPTION_ACCOUNTBROKERMAPPING = "Account <> Broker Mapping";
        public const string CAPTION_ACCOUNTBROKER = "Account(s)<>Broker(s)";

        #endregion

        #region Literals
        public const string C_LIT_AUTO = "Auto";
        public const string C_LIT_UNALLOCATED = "Unallocated";
        public const string LIT_SPACE = " ";
        public const string LIT_LIMIT = "Limit";
        public const string LIT_STOP = "Stop";
        public const string LIT_STOP_LIMIT = "Stop Limit";
        public const string LIT_BUY = "Buy";
        public const string LIT_BUY_TO_COVER = "Buy To Cover";
        public const string LIT_BUY_TO_CLOSE = "Buy to Close";
        public const string LIT_CREATE_SUB = "CreateSub";
        public const string LIT_OPEN = "Open";
        public const string LIT_CLOSE = "Close";
        public const string LIT_DISPLAY = "DISPLAY";
        public const string LIT_VALUE = "VALUE";
        public const string LIT_NAME = "Name";
        public const string LIT_TAG_VALUE = "TagValue";
        public const string LIT_VALUE_SMALL = "Value";
        public const string LIT_STRATEGY_ID = "StrategyID";
        public const string LIT_DATA_VALUE = "DataValue";
        public const string LIT_DISPLAY_TEXT = "DisplayText";
        public const string LIT_CHECK_STATE = "CheckState";
        public const string LIT_TAG = "Tag";
        public const string LIT_FULLNAME = "FullName";
        public const string LIT_APPEARANCE = "Appearance";
        public const string LIT_KEY = "Key";
        public const string LIT_SEARCH_CRITERIA = "SearchCriteria";
        public const string LIT_EXECUTION_INS = "Execution Instructions";
        public const string LIT_SECMASTER = "SecMaster";
        public const string LIT_SYMBOL = "Symbol";
        public const string LIT_ACTION = "Action";
        public const string LIT_USE_TICKER = "Use Ticker";
        public const string LIT_USE_BLOOMBERG = "Use Bloomberg";
        public const string LIT_EVENT_ARGS = "EventArgs";
        public const string DROPDWOMINDEXCLOUMN = "DropDownIndexCloumn";
        public const string LIT_SYMBOLOGY_CODE = "SymbologyCode";
        public const string LIT_PRICE = "Price";
        public const string LIT_SEDOL = "SEDOL";
        public const string LIT_CUSIP = "CUSIP";
        public const string LIT_CINS = "CINS";
        public const string LIT_RIC = "RIC";
        public const string LIT_ISIN = "ISIN";
        public const string LIT_OSI = "OSI";
        public const string LIT_UNALLOCATED = "Unallocated";
        public const string LIT_PERCENTAGE = "%";
        public const string LIT_NUMBER = "N";
        public const string LIT_QUANTITY = "Q";
        public const string LIT_DOLLAR = "$";
        public const string LIT_NOT_AVAILABLE = "Not available";
        public const string LIT_ALLOCATION_END_POINT_ADDRESS_NEW = "TradeAllocationServiceNewEndpointAddress";
        public const string LIT_PRICING_SERVICE_ADDRESS = "PricingServiceEndpointAddress";
        public const string LIT_CUSTOM = "Custom";
        public const string LIT_ERROR = "Error";
        public const string LIT_FONT_NAME = "Segoe UI";
        #endregion

        #region Messages
        public const string MSG_PRICE_GREATER_THAN_AMOUNT = "Price is greater than $Amount, quantity can not be generated";
        public const string MSG_PRICE_GREATER_THAN_TARGET_AMOUNT = "Price is greater than target $Amount, quantity can not be generated";
        public const string MSG_QUANTITY_TO_PRICE = "You are buying 0 Quantity";
        public const string MSG_VALID_PRICE_IN_DOLLARS = "Please enter a valid Price";
        public const string MSG_VALID_AMOUNT_IN_DOLLARS = "Please enter a valid $Amount";
        public const string MSG_BUY = "Buy";
        public const string MSG_SELL = "Sell";
        public const string MSG_VALID_QUANTITY = "Please enter a value for quantity";
        public const string MSG_NUMERIC_QUANTITY = "Please enter a numeric value for quantity";
        public const string MSG_GREATER_QUANTITY = "Input Quantity cannot be greater than Balance Quantity";
        public const string MSG_LESSER_QUANTITY = "Input Quantity cannot be less than Working Quantity";
        public const string MSG_TARGET_QUANTITY_CANNOT_ZERO_FOR_LIVE = "Target Quantity can not be zero for Live order";
        public const string MSG_GREATHER_EXQTY_QUANTITY = "Quantity cannot be less than Target Quantity";
        public const string MSG_GREATHER_INMARKET_QUANTITY = "Order Quantity cannot be less than In-Market Quantity";
        public const string MSG_GREATHER_TARGET_QUANTITY = "Order Quantity should be greater than Target Quantity";
        public const string MSG_GREATHER_TARGET_QUANTITY_NEW = "New Parent order quantity, cannot be less than what is already executed";
        public const string MSG_NOTIONAL = " - Notional Value :";
        public const string MSG_REMOVE_PTT = "Changing symbol will remove PTT details from Order, Do you want to continue?";
        public const string MSG_REMOVE_OTCTEMPLATEUI = "Changing OTC type will close the Template UI, Do you want to continue?";
        public const string MSG_PLEASE_TRY_AGAIN = "Please try again";
        public const string MSG_CURRENCY_INFORMATION_NOT_AVAILABLE = "Currency information for Symbol not available";
        public const string MSG_SELECT_STRATEGY = "Please select a strategy";
        public const string MSG_SECURITY_NOT_APPROVED = "Security not approved. Click \"Security Master\" button for approve it";
        public const string MSG_SECURITY_NOT_VALID = "Security is not valid";
        public const string MSG_SECURITY_VALID_DETAILS_UPDATED = "Security Details Updated";
        public const string MSG_SECURITY_NOT_VALIDATED = "Security not validated";
        public const string MSG_ALLOW_TRADE = "Do you want to allow the trade?";
        public const string MSG_SYMBOL_CANNOT_VERIFIED = "Symbol cannot be verified. Please check if DataManager is connected";
        public const string MSG_SECURITY_NOT_APPROVED_BEFORE_TRADE = "Security not approved, first approve security before trade.";
        public const string MSG_SYMBOL_NOT_VALIDATED = "Symbol could not be validated";
        public const string MSG_PROCEED_TO_LOCKING = "The ability to trade on a account can only be granted to one user at a time, would you like to proceed in locking ";
        public const string MSG_COULD_NOT_VALIDATE_COMPLIANCE_ALERT = "Could not validate compliance rules";
        public const string MSG_STAGING_BLOCKED = "Staging Blocked.";
        public const string MSG_STAGING_ALLOWED_RULE_OVERRIDEN = "Staging Allowed as rule was overridden by user";
        public const string MSG_STAGING_ALLOWED = "Staging Allowed.";
        public const string MSG_PTT_ORDER_GENRATED_AND_REMOVED = "The order created by the Percent Trading Tool has been generated and the allocation has been removed from the trading ticket.";
        public const string MSG_PTT_ORDER_REMOVE = "Changing order side will remove PTT details from Order, Do you want to continue?";
        public const string MSG_DATA_MANAGER_NOT_CONNECTED = "Data manager not connected";
        public const string MSG_COULD_NOT_VALIDATED_PROCEED_ANAWAYS = "could not be validated, Do you want to proceed?";
        public const string MSG_TRADE_SERVER_CONNECTED = "Trade Server : Connected";
        public const string MSG_TRADE_SERVER_DISCONNECTED = "Trade Server : Disconnected";
        public const string MSG_MISSING_SEDOL_IN_SECURITY_MASTER_UPDATE = "Missing SEDOL in security master. Would you like to update?";
        public const string MSG_MISSING_CUSIP_IN_SECURITY_MASTER_UPDATE = "Missing CUSIP in security master. Would you like to update?";
        public const string MSG_MISSING_RIC_IN_SECURITY_MASTER_UPDATE = "Missing RIC in security master. Would you like to update?";
        public const string MSG_MISSING_ISIN_IN_SECURITY_MASTER_UPDATE = "Missing ISIN in security master. Would you like to update?";
        public const string MSG_MISSING_OSI_IN_SECURITY_MASTER_UPDATE = "Missing OSI in security master. Would you like to update?";
        public const string MSG_BBGID_IS_INVALID = "BBGID is invalid. Length of BBGID should be 12.";
        public const string MSG_VALIDATING_SYMBOL = "Validating Symbol";
        public const string MSG_SYMBOL_MAY_NOT_EXIXSTS_ADD_SYMBOL_LOOKUP = "Symbol may not exist. You can add it by clicking \"Security Master\".";
        public const string MSG_SYMBOL_FAILED_TO_VALIDATE = "Failed to validate the security, please add it to the security master to proceed further";
        public const string MSG_SYMBOL_FAILED_TO_VALIDATE_QTT = "Could not validate symbol:";
        public const string MSG_PLEASE_CHECK_IF_YOU_HAVE_PERMISSION_TO_TRADE = "Please check if you have permission to trade this Symbol";
        public const string MSG_AT_RATE = "at rate";
        public const string MSG_SELECT_A_VALID_ORDER_SIDE = "Select a valid OrderSide";
        public const string MSG_SELECT_A_VALID_ORDER_TYPE = "Select a valid OrderType";
        public const string MSG_SELECT_A_VALID_COUNTER_PARTY = "Select a valid Broker";
        public const string MSG_SELECT_A_VALID_VENUE = "Select a valid Venue";
        public const string MSG_SELECT_A_VALID__ACCOUNT = "Select a valid Account";
        public const string MSG_SELECT_A_VALID_TRADING_ACCOUNT = "Select a valid Trader";
        public const string MSG_SELECT_A_VALID_TIF = "Select a valid TIF";
        public const string MSG_SELECT_A_VALID_HANDLING_INSTRUCTION = "Select a valid Handling Instruction";
        public const string MSG_SELECT_A_VALID_EXECUTION_INSTRUCTION = "Select a valid Execution Instruction";
        public const string MSG_SELECT_A_VALID_SETTLEMENT_CURRENCY = "Select a valid Settlement Currency";
        public const string MSG_MANUAL_ENTRIES_NOT_ALLOWED_FOR_STAGED_ORDERS = "Manual Entries Not Allowed for Staged Orders";
        public const string MSG_NAV_IS_LOCKED_FOR_SELECTED_ACCOUNT = "NAV is locked for selected account. You can not allow to trade on this trade date.";
        public const string MSG_SELECT_A_ACCOUNT = "Please select a account";
        public const string MSG_TARGET_QUANTITY_CAN_NOT_BE_GREATHER_THAN_HUNDRED = "Target quantity can not be greater than 100 when entered as percentage";
        public const string MSG_IS_CURRENTLY_LOCKED_BY_ANOTHER_USER = " is currently locked by another user, please refer to the Account Lock screen for more information.";
        public const string MSG_HAS_BEEN_ACQUIRED_YOU_MAY_CONTINUE = " has been acquired, you may continue.";
        public const string MSG_THE_LOCK_FOR = "The lock for ";
        public const string MSG_FIX_CONNECTION_DOWN = "We are experiencing temporary instability on our FIX connections. We are working on it and should be up shortly";//
        public const string MSG_SOME_VALUE_ARE_INVALID = "Some values are invalid thus we cannot pass the trade";
        public const string MSG_VALUE_IS_INVALID_FOR_FIELD = "Value for this field is incorrect";
        public const string MSG_TRADE_SENT_SUCCESSFULLY = "Trade sent to the broker successfully";
        public const string MSG_QUANTITY_CAN_NOT_BE_GREATHER_THAN = "Quantity can not be greater than ";
        public const string MSG_THIS_IS_A_AUTO_CALCULATE_FIELD = "This is a auto calculate field and will update on change in dependent column";
        public const string MSG_PLEASE_ENTER_VALID_SYMBOL = "Please enter a valid symbol!!";
        public const string MSG_GREATHER_EXECUTED_QUANTITY = "Executed Quantity cannot be greater than Quantity!";
        public const string MSG_ROW_UPDATE = "The bulk update section will apply bulk changes to all selected trades.";
        public const string MSG_BULK_UPDATE = "Order Side could not be updated for some order(s) which had sub-orders created";
        public const string MSG_PLEASE_CHECK_A_ROW_FOR_MULTICHANGE = "Please select a row to apply bulk changes.";
        public const string MSG_LAYOUT_LOAD_ERROR = "Error while loading layout for the Grid. This error can generally be resolved by removing the preferences file OR by saving the layout again.";
        public const string MSG_TT_VALIDATION = "TT: Validation";
        public const string MSG_PLEASE_CHECK_A_ROW_TO_TRADE = "Please check any row to trade";
        public const string MSG_DATA_MANAGER_NOT_CONNECTED_CANNOT_FETCH_LIVE_PRICES = "Data manager not connected, Live price not available.";
        public const string MSG_PRICES_NOT_AVAILABLE = "Price is required for Compliance check";
        public const string MSG_PRICES_FETCHED = "Prices Fetched";
        public const string MSG_PRICES_FETCHING = "Fetching the prices...";
        public const string MSG_VALID_GTDDATE = "Please select a valid expiry date for the GTD order.";
        public const string MSG_VALID_EXPIRY_DATE_GREATER_THANEUALTO_CURRENTDATE = "The expiry date selected is already past date. Please select a valid Expiry Date.";
        public const string MSG_CANNOT_BOOK_DONEAWAY_TRADES_FOR_MULTIDAY = "Cant book done away trades for GTD/GTC orders.";

        #endregion

        #region Header
        public const string HEADER_PRICES_NOT_AVAILABLE = "Prices are not available";
        public const string HEADER_SECURITY_APPROVAL = "Security Approval";
        public const string HEADER_ALERT = "Alert";
        public const string HEADER_TRADING_TICKET = "TradingTicket";
        public const string HEADER_MESSAGE = "Message";
        public const string HEADER_TRADING_TICKET_ALERT = "Trading Ticket Alert";
        public const string HEADER_PRANA_WARNING = "Prana Warning";
        public const string HEADER_WARNING = "Warning";
        public const string HEADER_ACCOUNT_LOCK = "Account Lock";
        #endregion

        #region Column Properties
        public const string COLUMN_LIMITPRICE = "LimitPrice";
        public const string COLUMN_MARKETPRICE = "MarketPrice";
        public const string COLUMN_SELECTCHECKBOX = "SelectCheckbox";
        public const string COLUMN_COMMITTEDQUANTITY = "CommittedQty";
        public const string COLUMN_MANUALLYEXECUTEDQUANTITY = "ManuallyExecutedQuantity";
        public const string COLUMN_PREVIOUSLYSENTQUANTITY = "PrevExecQty";
        public const string COLUMN_PERCENTAGECOMMITTEDQUANTITY = "PercentCommittedQty";
        public const string COLUMN_SELECTMANUALDATAENTRY = "SelectManualDataEntry";
        public const string COLUMN_TOTALQUANTITY = "TotalQty";
        public const string COLUMN_UNCOMMITTEDQUANTITY = "UncommittedQty";
        public const string COLUMN__DUMMYCHECKBOX = "DummyCheckBox";
        public const string COLUMN_SETTLEMENT_CURRENCYNAME = "SettlementCurrency";
        public const string COLUMN_ACCOUNTBROKERMAPPING = "Account<>BrokerMapping";
        #endregion

        #region Dynamic Message
        public const string MSG_NOTIONAL_AS_QUANTITY_MESSAGE = "You want to {0} {1} Quantitiy of {2}";
        public const string MSG_FX_SYMBOL_DESCRIPTION_MESSAGE = "Amount to {0} in {1} {2} Amount to {3} in {4} {5} at rate {6}";
        #endregion

        public const string LABEL_CAPTION_TARGETQUANTITY = "Executed Quantity";
        public const string LABEL_CAPTION_QUANTITY = "Update Quantity";
        public const string BUTTON_CAPTION_REPLACE = "Replace";
        public const string BUTTON_CAPTION_DONE_AWAY = "&Done Away";
        public const string LABEL_SELECT = "Select";
        public const string ALLOCATION_PREF_CUSTOM = "*Custom#_";
        public const string MTT_BULK_UPDATE_SYMBOL = "mtt";
        public const int DECIMAL_PLACE_FOR_PRICING = 6;

    }
}
