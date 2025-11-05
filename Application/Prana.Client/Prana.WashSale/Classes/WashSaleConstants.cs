using System;

namespace Prana.WashSale.Classes
{
    internal class WashSaleConstants
    {
        public const string CAPS_ID = "ID";
        public const string CONST_TAXLOT_ID = "TaxlotID";
        public const string CONST_TYPE_OF_TRANSACTION = "TypeofTransaction";
        public const string CAPS_TYPE_OF_TRANSACTION = "Type of Transaction";
        public const string CONST_TRADE_DATE = "TradeDate";
        public const string CAPS_TRADE_DATE = "Trade Date";
        public const string CONST_ORIGINAL_PURCHASE_DATE = "OriginalPurchaseDate";
        public const string CAPS_ORIGINAL_PURCHASE_DATE = "Original Purchase Date";
        public const string CONST_ACCOUNT = "Account";
        public const string CONST_SIDE = "Side";
        public const string CONST_ASSET = "Asset";
        public const string CONST_CURRENCY = "Currency";
        public const string CONST_BROKER = "Broker";
        public const string CONST_SYMBOL = "Symbol";
        public const string CONST_BLOOMBERG_SYMBOL = "BloombergSymbol";
        public const string CAPS_BLOOMBERG_SYMBOL = "Bloomberg Symbol";
        public const string CONST_CUSIP = "CUSIP";
        public const string CONST_ISSUER = "Issuer";
        public const string CONST_UNDERLYING_SYMBOL = "UnderlyingSymbol";
        public const string CAPS_UNDERLYING_SYMBOL = "Underlying Symbol";
        public const string CONST_QUANTITY = "Quantity";
        public const string CONST_UNIT_COST_LOCAL = "UnitCostLocal";
        public const string CAPS_UNIT_COST_LOCAL = "Unit Cost (Local)";
        public const string CONST_TOTAL_COST_LOCAL = "TotalCostLocal";
        public const string CAPS_TOTAL_COST_LOCAL = "Total Cost (Local)";
        public const string CONST_TOTAL_COST = "TotalCost";
        public const string CAPS_TOTAL_COST = "Total Cost";

        public const string CONST_WASH_SALE_ADJUSTED_REALIZED_LOSS = "WashSaleAdjustedRealizedLoss";
        public const string CAPS_WASH_SALE_ADJUSTED_REALIZED_LOSS = "Wash Sale Adjusted Realized Loss";

        public const string CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD = "WashSaleAdjustedHoldingsPeriod";
        public const string CAPS_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD = "Wash Sale Adjusted Holdings Period";

        public const string CONST_WASH_SALE_ADJUSTED_COST_BASIS = "WashSaleAdjustedCostBasis";
        public const string CAPS_WASH_SALE_ADJUSTED_COST_BASIS = "Wash Sale Adjusted Cost Basis";

        public const string CONST_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE = "WashSaleAdjustedHoldingsStartDate";
        public const string CAPS_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE = "Wash Sale Adjusted Holdings Start Date";

        public const int CONST_WASHSALE_SQL_TIMEOUT = 5000;

        public const string CONST_TAXLOT = "Taxlot";
        public const string CONST_TRADE = "Trade";
        public const string CONST_WASHSALE_POPUP_MESSAGE = "Unsaved changes on the UI. What do you want to do?";
        public const string CONST_WASHSALE_POPUP_MESSAGE_TITLE = "Warning";
        public const string CONST_WASHSALE_CONNECTION_STRING = "PranaConnectionString";
        public const string CONST_WASHSALE_DATA_LOADED = " Data loaded";
        public const string CONST_WASHSALE_LOADING_DATA = " Loading data...";
        public const string CONST_WASHSALE_DATA_SAVED = " Data saved";
        public const string CONST_WASHSALE_SAVING_DATA = " Saving data...";
        public const string CONST_WASHSALE_EXPORTING_DATA = " Exporting data...";
        public const string CONST_WASHSALE_EXPORT_DATA = " Data exported";
        public const string CONST_WASHSALE_BLANK_EXPORT_DATA = " Nothing to export";
        public const string CONST_WASHSALE_TABLENAME = "T_WashSaleOnBoarding";
        public const string CONST_WASHSALE_SAVETABLE_STOREDPROCEDURE = "dbo.P_SaveWashSaleTaxlots";
        public const string CONST_WASHSALE_GETOPENTAXLOT_STOREDPROCEDURE = "P_GetWashSaleOpenTaxlots";
        public const string CONST_EQUITY = "Equity";
        public const string CONST_EQUITYOPTION = "EquityOption";
        public const string CONST_PRIVATEEQUITY = "PrivateEquity";
        public const string CONST_FIXEDINCOME = "FixedIncome";
        public const string CONST_EQUITYSWAP = "EquitySwap";
        public const string CONST_ALL_ASSET_SELECTED = "All Asset(s) Selected";
        public const string CONST_ALL_CURRENCY_SELECTED = "All Currency(s) Selected";
        public const string CONST_WASHSALE_UPLOAD_POPUPMESSAGE = "Uploaded file schema is not as per requirement";
        public const string CONST_WASHSALE_UPLOAD_POPUPMESSAGE_TITLE = "Nirvana Alert";
        public const string CONST_BLANK = "";
        public const string CONST_WASHSALE_GRID_ERROR_MESSAGE = " Error in the data grid";
        public const string CONST_WASHSALE_GRID_ERROR_INDICATOR_MESSAGE = "Only numeric values allowed";
        public const string CONST_WASHSALE_GRID_ERROR_INDICATOR_MESSAGE_HOLDING_DATE = "Only whole number allowed";
        public const string CONST_WASHSALE_GRID_ERROR_INDICATOR_MESSAGE_VALUE_CANNOT_BE_GREATER_THAN_10000 = "Value cannot be greater than 10000";
        public const string CONST_WASHSALE_MESSAGEBOX_CAPTION = "Nirvana Wash Sale Trades";
        public const string CONST_WASHSALE_GRID_ERROR_MESSAGEBOX = "Unsaved changes contain error(s), please resolve";
        public const string CONST_BLANK_SPACE = " ";
        public const string CONST_DATA_UPLOADED = " Data uploaded";
        public const string CONST_DATA_UPLOADING = " Data uploading ...";
        public const string CONST_GRAND_SUMMARY = "Grand Summaries";
        public const string CONST_FILE_USED_BY_ANOTHER_PROECESS = "File is used by another process !!!";
        public const string CONST_EXPORT_CANCEL = " Export operation cancelled";
        public static string doubleMinValString = double.MinValue.ToString();
        public static string doubleEpsilonValString = Double.Epsilon.ToString();
        public static string intMinValString = int.MinValue.ToString();
        public const string SUMMARY_MULTIPLE = "Multiple";
        public const string SUMMARY_UNDEFINED = "Undefined";
        public const string SUMMARY_DASH = "-";
        public const string CONST_FORMAT_TEXT = "{0:#,##,###0}";              // will show 0 value too
        public const string CONST_FORMAT_TEXT_NEW = "{0:#,##,###0.##}";       // will show 0 value and decimal values
        public const string CONST_FORMAT_DATE = "{0:MM/dd/yyyy}";
        public const string CONST_FORMAT_COL_DECIMAL = "{0:#,##,###0.00}";
        public const string CONST_FORMAT_DEFAULT = "{0}";
        public const string CONST_FORMAT_TWO_DECIMAL = "#,##,###.## ; -#,##,###.## ; 0 ;* @";
        public const string CONST_FORMAT_WASHSALE_COLUMNS = "{0:#,###.##}";  //Will not show 0 value
        public const string CONST_FORMAT_WASHSALE_EXPORT = "#,##0.00";
        public const string CONST_DATE_FORMAT = "MM/dd/yyyy";
        public const string CONST_BACKSPACE = "\b";
        public const string CONST_CUT_KEYBOARD_SHORTCUT = "\u0018";
        public const string CONST_COPY_KEYBOARD_SHORTCUT = "\u0003";
        public const string CONST_PASTE_KEYBOARD_SHORCUT = "\u0016";
    }
}
