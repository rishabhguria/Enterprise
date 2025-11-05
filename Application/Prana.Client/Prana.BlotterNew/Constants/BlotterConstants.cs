using Prana.Global;
using System.Collections;

namespace Prana.Blotter
{
    internal class BlotterConstants
    {
        public const string PROPERTY_ULTRA_GRID_GROUP_BY_ROW = "Infragistics.Win.UltraWinGrid.UltraGridGroupByRow";
        public const string PROPERTY_BTN_REFRESH = "btnRefresh";
        public const string PROPERTY_BTN_REMOVE_ORDERS = "btnRemoveOrders";
        public const string PROPERTY_BTN_CANCEL_ALL_SUBS = "btnCancelAllSubs";
        public const string PROPERTY_BTN_ROLLOVER_ALL_SUBS = "btnRolloverAllSubs";
        public const string PROPERTY_BTN_PREFERENCES = "btnPreferences";
        public const string PROPERTY_BTN_EXPORT_TO_EXCEL = "btnExportToExcel";
        public const string PROPERTY_BTN_SAVE_ALL_LAYTOUT = "btnSaveAllLayout";
        public const string PROPERTY_BTN_ADD_TAB = "btnAddTab";
        public const string PROPERTY_BTN_ADD_ORDER_TAB = "btnAddOrderTab";
        public const string PROPERTY_BTN_LINK_UNLINK_TAB = "btnLinkUnlikTab";
        public const string PROPERTY_NIRVANA_BLOTTER = "Nirvana Blotter";
        public const string PROPERTY_MANUAL = "Manual";
        public const string PROPERTY_FIX = "Fix";
        public const string PROPERTY_MERGE_ORDERS = "btnMergeButtons";
        public const string PROPERTY_UPLOAD_STAGE_ORDERS = "btnUploadStageOrders";


        public const string ORDER_PLEASE_SELECT_MESSAGE = "Please select at least one Order";

        public const string CAPTION_LINK_TAB = "Link Tab";
        public const string CAPTION_MERGE_ORDERS = "Merge Orders";
        public const string CAPTION_UNLINK_TAB = "Unlink Tab";
        public const string CAPTION_UNEXECUTED_QUANTITY = "Unexecuted Qty";
        public const string CAPTION_ALLOCATION_PERCENTAGE = "Current Allocation %";
        public const string CAPTION_PERCENTAGE_ON_TOTAL_QTY = "% Executed Quantity";
        public const string CAPTION_TARGET_ALLOCATION_PERCENTAGE = "Target Allocation %";
        public const string CAPTION_TARGET_ALLOCATION_QTY = "Target Allocation Quantity";
        public const string CAPTION_ORDER_ID = "Order ID";
        public const string CAPTION_FUND_ID = "FundID";
        public const string CAPTION_ORDER_QTY = "OrderQuantity";
        public const string CAPTION_ALLOCATED_QUANTITY = "Current Allocated Quantity";
        public const string CAPTION_LAST_FILL_PRICE_LOCAL = "Last Fill Price (Local)";
        public const string CAPTION_PRINCIPAL_AMOUNT_BASE = "Principal Amount (Base)";
        public const string CAPTION_PRINCIPAL_AMOUNT_LOCAL = "Principal Amount (Local)";
        public const string CAPTION_CounterCurrencyAmount = "Counter Currency Amount";
        public const string CAPTION_CounterCurrency = "Counter Currency";
        public const string CAPTION_ORDER_SEPARATOR = "_Order_";
        public const string CAPTION_SUBORDER_SEPARATOR = "_SubOrder_";
        public const string CAPTION_MULTIPLE = "Multiple";
        public const string PROPERTY_DASH = "-";

        public const string TAB_NAME_ORDERS = "Orders";
        public const string TAB_NAME_WORKINGSUBS = "WorkingSubs";
        public const string TAB_NAME_SUMMARY = "Summary";
        public const string TAB_NAME_SUBORDERS = "SubOrders";
        public const string TAB_NAME_Dynamic_Order = "Dynamic_Order_";

        public const int ErrorCode_UserNotAllowed = 1;
        public const int ErrorCode_Cancelled = 2;
        public const int ErrorCode_TargetQtyFull = 3;

        #region Message Constants
        public const string MSG_NO_ALLOCATION_TO_VIEW = "No allocation to view";
        public const string MSG_VALUE_IS_INVALID_FOR_FIELD = "Value for this field is incorrect";
        #endregion

        public const string LIT_ALLOCATION_END_POINT_ADDRESS_NAME = "TradeAllocationServiceNewEndpointAddress";

        #region Merge Orders Constants

        internal const string MSG_IN_CASE_OF_MERGING_POSSIBLE = "Are you sure you want to merge these orders? This action cannot be undone.";
        internal const string MSG_SELECT_TWO_OR_MORE_ROWS = "Select two or more rows for Merging";
        internal const string MSG_ONE_OR_MORE_ORDERS_IN_MARKET = "Cannot merge orders because one or more orders have been sent to the market";
        internal const string MSG_CRITERIA_SYMBOL_SIDE_BROKER_NOT_MATCHING = "Cannot merge because the selected orders have either different Symbol, Side, Broker or Trading Account";
        internal const string MSG_CRITERIA_VENUE_NOT_MATCHING = "Cannot merge because the selected orders have different Venue";
        internal const string MSG_CRITERIA_ALGO_STRATEGY_ID_NOT_MATCHING = "Cannot merge because the selected orders have different Algo Strategy Id";
        internal const string MSG_CRITERIA_ALGO_PROPERTIES_NOT_MATCHING = "Cannot merge because the selected orders have different Algo Properties";
        internal const string MSG_MERGE_SUCCESSFUL = "stage orders merged successfully.";
        internal const string MSG_MERGE_ERROR_OCCURED = "Error occurred in merging order";
        internal const string MSG_MERGE_CANCELLED = "Merging has been cancelled";
        internal const string MSG_ORDERS_HAS_BEEN_REMOVED_OR_MERGED = "The order you selected is either Removed or Merged by other user";
        internal const string MSG_DIFFERENT_ALLOCATION_IN_ORDERS = "Cannot merge because the selected orders have different allocation preferences.";
        internal const string MSG_ORDER_HAVE_MULTIPLE_BROKER = "Cannot merge selected orders because one or more orders have multiple brokers selected";
        internal const string CONST_BLANK_SPACE = " ";
        internal const string CONST_LIMIT = "Limit";
        internal const string CONST_STOP_LIMIT = "Stop Limit";
        #endregion

        //Order Blotter Columns
        private static ArrayList _orderBlotterColumns;
        internal static ArrayList OrderBlotterColumns
        {
            get
            {
                if (_orderBlotterColumns == null || (_orderBlotterColumns != null && _orderBlotterColumns.Count > 0))
                {
                    _orderBlotterColumns = new ArrayList();
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_TRANSACTION_TIME);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_SYMBOL);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_ORDER_SIDE);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_ORDER_STATUS);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_QUANTITY);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_LEAVES_QUANTITY);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_EXECUTED_QTY);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_PERCENTEXECUTED);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_DAY_EXECUTED_QUANTITY);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_UNSENT_QTY);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_LASTPRICE);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_AVGPRICE);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_COUNTERPARTY_NAME);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_ACCOUNT);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_ALLOCATION_SCHEME_NAME);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_ORDER_TYPE);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_NOTIONALVALUE);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_NOTIONALVALUEBASE);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_SETTLEMENTCURRENCYID);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_REBALANCER_FILE_NAME);
                    _orderBlotterColumns.Add(OrderFields.PROPERTY_EXECUTION_TIME_LAST_FILL);
                }
                return _orderBlotterColumns;
            }
        }

        //Working Sub Blotter Columns
        private static ArrayList _workingSubBlotterColumns;
        internal static ArrayList WorkingSubBlotterColumns
        {
            get
            {
                if (_workingSubBlotterColumns == null || (_workingSubBlotterColumns != null && _workingSubBlotterColumns.Count > 0))
                {
                    _workingSubBlotterColumns = new ArrayList();
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_TRANSACTION_TIME);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_SYMBOL);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_ORDER_SIDE);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_ORDER_STATUS_WITHOUTROLLOVER);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_QUANTITY);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_LEAVES_QUANTITY);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_EXECUTED_QTY);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_UNEXECUTED_QUANTITY);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_LAST_SHARES);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_LASTPRICE);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_AVGPRICE);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_COUNTERPARTY_NAME);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_ACCOUNT);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_ALLOCATION_SCHEME_NAME);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_ALLOCATIONSTATUS);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_USER);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_NOTIONALVALUE);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_NOTIONALVALUEBASE);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_SETTLEMENTCURRENCYID);
                    _workingSubBlotterColumns.Add(OrderFields.PROPERTY_EXECUTION_TIME_LAST_FILL);
                }
                return _workingSubBlotterColumns;
            }
        }

        //Summary Blotter Columns
        private static ArrayList _summaryBlotterColumns;
        internal static ArrayList SummaryBlotterColumns
        {
            get
            {
                if (_summaryBlotterColumns == null || (_summaryBlotterColumns != null && _summaryBlotterColumns.Count > 0))
                {
                    _summaryBlotterColumns = new ArrayList();
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_TRANSACTION_TIME);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_SYMBOL);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_ORDER_SIDE);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_ORDER_STATUS);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_QUANTITY);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_LEAVES_QUANTITY);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_EXECUTED_QTY);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_LAST_SHARES);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_LASTPRICE);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_AVGPRICE);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_COUNTERPARTY_NAME);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_NOTIONALVALUE);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_NOTIONALVALUEBASE);
                    _summaryBlotterColumns.Add(OrderFields.PROPERTY_SETTLEMENTCURRENCYID);
                }
                return _summaryBlotterColumns;
            }
        }
    }
}