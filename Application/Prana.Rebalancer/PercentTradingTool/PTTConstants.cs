namespace Prana.Rebalancer.PercentTradingTool
{
    public class PTTConstants
    {
        #region columns
        public const string COL_SYMBOL = "Symbol";
        public const string COL_ORDERSIDE = "OrderSide";
        public const string COL_TARGET = "Target";
        public const string COL_ADDORSET = "AddOrSet";
        public const string COL_MASTERFUNDORACCOUNT = "MasterFundOrAccount";
        public const string COL_COMBINEDACCOUNTSTOTALVALUE = "CombinedAccountsTotalValue";
        public const string COL_SELECTEDFEEDPRICE = "SelectedFeedPrice";
        public const string COL_TYPE = "Type";
        public const string COL_ACCOUNT = "Account";
        public const string COL_ACCOUNTID = "AccountId";
        public const string COL_FXRATE = "FxRate";
        public const string COL_STARTINGPOSITION = "StartingPosition";
        public const string COL_STARTINGVALUE = "StartingValue";
        public const string COL_ACCOUNTNAV = "AccountNAV";
        public const string COL_STARTINGPERCENTAGE = "StartingPercentage";
        public const string COL_PERCENTAGETYPE = "PercentageType";
        public const string COL_TRADEQUANTITY = "TradeQuantity";
        public const string COL_ENDINGPERCENTAGE = "EndingPercentage";
        public const string COL_ENDINGPOSITION = "EndingPosition";
        public const string COL_ENDINGVALUE = "EndingValue";
        public const string COL_PERCENTAGEALLOCATION = "PercentageAllocation";
        public const string COL_PTTID = "PTTId";
        public const string COL_MASTERFUND = "MasterFund";
        public const string COL_MASTERFUNDNAME = "MasterFundName";
        public const string COL_TOTALPERCENTAGE = "TotalPercentage";
        public const string COL_ACCOUNTNAME = "AccountName";
        public const string COL_ACCOUNTFACTOR = "AccountFactor";
        public const string COL_PERCENTAGE = "Percentage";
        public const string COL_IS_PRORATA_PERCENTAGE = "IsProrataPrefChecked";
        public const string COL_MASTERFUNDID = "MasterFundId";
        public const string COL_USE_PRORATA_PREF = "UseProrataPreference";
        public const string COL_PERCENT_IN_MASTERFUND = "PercentInMasterFund";
        public const string COL_ORDERSIDEID = "OrderSideID";
        public const string COL_PREFERENCETYPE = "PreferenceType";
        public const string COL_ROUNDLOT = "RoundLots";
        public const string COL_ISUSEROUNDLOT = "IsUseRoundLot";
        public const string COL_ISCUSTODIANBROKER = "IsUseCustodianBroker";
        #endregion

        #region captions
        public const string CAP_SYMBOL = "Symbol";
        public const string CAP_ORDERSIDE = "Order Side";
        public const string CAP_TARGET = "Target";
        public const string CAP_ADDORSET = "+ /- / =";
        public const string CAP_MASTERFUNDORACCOUNT = "MasterFund/Account";
        public const string CAP_COMBINED_ACCOUNT_TOTAL = "Combined Account Total";
        public const string CAP_SELECTEDFEEDPRICE = "Price";
        public const string CAP_ACCOUNT = "Account";
        public const string CAP_ACCOUNTID = "Account Name";
        public const string CAP_FXRATE = "Fx Rate";
        public const string CAP_STARTINGPOSITION = "Starting Position";
        public const string CAP_STARTINGVALUE = "Starting Value";
        public const string CAP_ACCOUNTNAV = "Account NAV";
        public const string CAP_STARTINGPERCENTAGE = "Starting %";
        public const string CAP_STARTINGBASISPOINT = "Starting BP";
        public const string CAP_TRADEQUANTITY = "Trade Quantity";
        public const string CAP_PERCENTAGECHANGE = "% Change";
        public const string CAP_BASISPOINTCHANGE = "BP Change";
        public const string CAP_ENDINGPERCENTAGE = "Ending %";
        public const string CAP_ENDINGBASISPOINT = "Ending BP";
        public const string CAP_ENDINGPOSITION = "Ending Position";
        public const string CAP_ENDINGVALUE = "Ending Value";
        public const string CAP_PERCENTAGEALLOCATION = "% Allocation";
        public const string CAP_TYPE = "Type";
        public const string CAP_MASTERFUNDID = "MasterFund Name";
        public const string CAP_ACCOUNTTEXT = "Accounts";
        public const string CAP_MASTERFUNDTEXT = "Master Funds";
        public const string CAP_MASTERFUND = "Master Fund";
        public const string CAP_TOTALPERCENTAGE = "Total Percentage";
        public const string CAP_USE_PRORATA_PREF = "Use Prorata Preference";
        public const string CAP_ACCOUNT_FACTOR = "Account Factor";
        public const string CAP_PTTMODULE = "% Trading Tool";
        public const string CAP_MASTERFUND_PREF_TABLENAME = "MasterFundPreference";
        public const string CAP_ACCOUNT_PREF_TABLE_NAME = "AccountPreference";
        public const string CAP_MASTERFUNDNAV = "MasterFund NAV";
        public const string CAP_ROUNDLOT = "Round Lots";
        public const string CAP_CUSTODIANBROKER = "Use Custodian Broker";
        #endregion

        #region literals
        public const string LIT_LAST = "Last";
        public const string LIT_SYMBOL = "Symbol";
        public const string LIT_ASK = "Ask";
        public const string LIT_BID = "Bid";
        public const string LIT_VWAP = "VWAP";
        public const string LIT_DASH = " - ";
        public const string LIT_INPUT_PARAMETERS = "Input Parameters";
        public const string LIT_CALCULATED_VALUES = "Calculated Values";
        public const string LIT_DEFAULT_FEED_VALUES = "0.00";
        public const string LIT_TEXTBOX = "TextBox";
        public const string LIT_XAMCOMBOEDITOR = "XamComboEditor";
        public const string LIT_SELECT_ALL = "Select All";
        public const string LIT_UNSELECT_ALL = "Unselect All";
        #endregion

        #region messages
        public const string MSG_CANNOT_STAGE = "The trade couldn't be staged";
        public const string MSG_STAGE_SUCCESS = "Order staged successfully";
        public const string MSG_NO_TRADE_COMP_CHECK = "No trade for compliance check";
        public const string MSG_LIVE_FEED_DISCONNECTED = "Unable to fetch data from Live Feed";
        public const string MSG_WAITING_FOR_VALIDATION = "Waiting for symbol validation";
        public const string MSG_IS_RECALCULATE_REQUIRED = "Changes were made in the calculated values. Do you want to re-calculate?";
        public const string MSG_TIME_CALC_COMPLETED = "Calculation completed at : ";
        public const string MSG_NO_CALCULATED_DATA = "Data is not calculated because none of the accounts have a positive NAV";
        public const string MSG_CONFLICTING_POSITIONS = "Position can't be long and short at same time.";
        public const string MSG_TRADEQAUNTITY_ZERO = "Allocation preference cannot be created because trade quantity is less than or equal to 0";
        public const string MSG_NEGATIVE_NAV_FOR_ACCOUNTS = "Showing 0 NAV for accounts having negative NAV";
        public const string MSG_INVALID_ASSET = "The symbol entered is not an Equity";
        public const string MSG_CALCULATION_NEEDED = "Changes in input parameters require calculation. Please click on CALCULATE again";
        public const string MSG_INVALIDPREFERENCESET = "Certain Master Fund's Allocation % cannot be defined. Please allocate, if required, to the specific accounts that have 0 Master Fund Ending %.";
        public const string MSG_CONFLICTING_POSITIONS_FOR_CHILD_ACCOUNTS = "Position can't be long and short at same time in accounts belonging to same master fund.";
        public const string MSG_INVALID_TOTALPERCENTAGE = "Sum of updated Accounts not equal to 100";
        public const string MSG_NO_PERMISSION_FOR_ORDERSIDE = "Generated order side is not permitted for this Asset class";
        public const string MSG_NO_ACCOUNTFACTOR_FOR_COMBINEACCOUNTTOTAL_YES = "Since Combine Account Total is set to Yes, all accounts will use 1 as the Account factor.";

        #endregion
    }
}
