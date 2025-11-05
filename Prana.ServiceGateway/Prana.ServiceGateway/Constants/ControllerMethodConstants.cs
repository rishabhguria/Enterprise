namespace Prana.ServiceGateway.Constants
{
    public class ControllerMethodConstants
    {
        #region AuthenticateUserController Methods

        public const string CONST_METHOD_LOGINUSER = "LoginUser";
        public const string CONST_METHOD_GETSTATUSFRLOGIN = "GetStatusForLogin";
        public const string CONST_METHOD_LOGOUTUSER = "LogoutUser";
        public const string CONST_METHOD_FORCELOGOUTUSER = "ForceLogoutUser";
        public const string CONST_METHOD_VALIDATETOKEN = "ValidateToken";
        public const string CONST_REQUESTED_ID = "RequestID: ";
        public const string CONST_COMPANY_USER_ID = "companyUserId";
        public const string CONST_GETCONNECTIONSTATUS = "GetConnectionStatus";
        public const string CONST_UPDATECACHEFORLOGINUSER = "UpdateCacheForLoginUser";
        public const string CONST_VALIDATEALREADYLOGGEDINUSER = "ValidateAlreadyLoggedInUser";
        public const string CONST_TOUCH_BASE_URL = "TouchBaseURL";
        public const string CONST_IS_SESSION_ALIVE = "IsAlive";
        public const string CONST_METHOD_GET_BLOOMBERG_TOKEN = "ProcessBloombergAuthentication";
        public const string CONST_METHOD_GET_TOUCH_OTK = "GetTouchOtk";

        #endregion

        #region BlotterController Methods

        public const string CONST_METHOD_GET_BLOTTER_DATA = "GetBlotterData/{blotterId}/{isComTransTradeRulesRequired}";
        public const string CONST_ACTION_GET_BLOTTER_DATA = "GetBlotterData";
        public const string CONST_METHOD_CANCEL_ALL_SUBS = "CancelAllSubOrders";
        public const string CONST_METHOD_ROLLOVER_ALL_SUBS = "RolloverAllSubOrders";
        public const string CONST_METHOD_REMOVE_ORDERS = "RemoveOrders";
        public const string CONST_METHOD_FREEZE_ORDERS_PCUI = "FreezeOrdersInPendingComplianceUI";
        public const string CONST_METHOD_UNFREEZE_ORDERS_PCUI = "UnfreezeOrdersInPendingComplianceUI";
        public const string CONST_METHOD_GET_BLOTTER_PPREF_DATA = "GetBlotterPreferanceData";
        public const string CONST_METHOD_REMOVE_MANUAL_EXECUTION = "RemoveManualExecution";
        public const string CONST_METHOD_GET_BLOTTER_MANUAL_FILLS = "GetBlotterManualFills";
        public const string CONST_METHOD_SAVE_ADD_MODIFY_FILLS = "SaveAddModifyFills";
        public const string CONST_METHOD_GET_ALLOCATION_DETAILS = "GetAllocationDetails";
        public const string CONST_METHOD_SAVE_ALLOCATION_DETAILS = "SaveAllocationDetails";
        public const string CONST_METHOD_RENAME_BLOTTER_CUSTOM_TAB = "RenameBlotterCustomTab";
        public const string CONST_METHOD_REMOVE_BLOTTER_CUSTOM_TAB = "RemoveBlotterCustomTab";
        public const string CONST_METHOD_GET_PST_ALLOCATION_DETAILS = "GetPstAllocationDetails";
        public const string CONST_ACTION_GET_PST_DATA = "GetPstData";
        public const string CONST_METHOD_GET_PST_DATA = "GetPstData/{allocationPrefID}/{symbol}/{OrderSideId}";
        public const string CONST_METHOD_TRANSFER_USER = "TransferUser";
        public const string CONST_METHOD_GET_ORDER_DETAILS_FOR_EDIT_TRADE_ATTRIBUTES = "GetOrderDetailsForEditTradeAttributes";
        public const string CONST_METHOD_SAVE_EDITED_TRADE_ATTRIBUTES = "SaveEditedTradeAttributes";
        #endregion

        #region CommonDataController Methods

        public const string CONST_APPLICATION_JSON = "application/json";
        public const string CONST_CONTROLLER = "[controller]";
        public const string CONST_METHOD_GET_BROKER_NAME = "GetBrokerNames";
        public const string CONST_METHOD_GET_ALLOCATION_PREFERENCES_DETAILS = "GetAllocationPreferencesDetails";
        public const string CONST_METHOD_GET_ORDER_TYPE = "GetOrderTypes";
        public const string CONST_METHOD_GET_ORDER_SIDES = "GetOrderSides";
        public const string CONST_METHOD_GET_TIF_DATA = "GetTIFData";
        public const string CONST_METHOD_GET_ALLOCATION_DATA = "GetAllocationData";
        public const string CONST_METHOD_GET_TRADING_TICKET_UI_PREF = "GetTradingTicketUIPrefs";
        public const string CONST_METHOD_GET_COMPANY_TRANSFER_TRADE_RULES = "GetCompanyTransferTradeRules";
        public const string CONST_METHOD_GET_COMPANY_TRADING_PREFERENCES = "GetCompanyTradingPreferences";
        public const string CONST_METHOD_GET_TRADING_TICKET_DATA = "GetTradingTicketData/{tradingTicketId}";
        public const string CONST_METHOD_GET_TRADING_TICKET_CACHE_DATA = "GetTradingTicketCacheData";
        public const string CONST_GET_TRADING_ATTRUBUTES = "GetTradingAttributes";

        #endregion

        #region LayoutController Methods

        public const string CONST_METHOD_SAVE_LAYOUT = "SaveLayout";
        public const string CONST_METHOD_LOAD_LAYOUT = "LoadLayout";
        public const string CONST_METHOD_SAVE_BLOTTER_ALL_GRIDS_LAYOUT = "SaveBlotterAllGridsLayout";
        public const string CONST_METHOD_LOAD_BLOTTER_ALL_GRIDS_LAYOUT = "LoadBlotterAllGridsLayout";
        public const string CONST_METHOD_SAVE_OR_UPDATE_RTPNL_LAYOUT = "SaveOrUpdateRtpnlLayout";
        public const string CONST_METHOD_LOAD_RTPNL_LAYOUT = "LoadRtpnlLayout";
        public const string REMOVE_PAGES_FOR_AN_USER = "RemovePagesForAnUser/{moduleName}";

        #endregion

        #region LiveFeedController Methods
        public const string CONST_METHOD_SUBSCRIBE_LIVEFEED = "SubscribeLiveFeed";
        public const string CONST_METHOD_UNSUBSCRIBE_LIVEFEED = "UnSubscribeLiveFeed";
        public const string CONST_METHOD_UPDATE_MARKET_DATA_TOKEN_REQUEST = "UpdateMarketDataTokenRequest";
        public const string CONST_METHOD_REQ_MULTIPLE_SYMBOLS_LIVE_FEED_SNAPSHOT_DATA = "ReqMultipleSymbolsLiveFeedSnapshotData";

        #endregion

        #region LoggingController Methods

        public const string CONST_CONTROLLER_ACTION = "[controller]/[action]";

        #endregion

        #region TradingController Methods

        public const string CONST_METHOD_VALIDATE_MULTIPLE_SYMBOLS = "ValidateMultipleSymbols";
        public const string CONST_METHOD_GET_SYMBOL_WISE_SHORT_LOCATE_ORDERS = "GetSymbolWiseShortLocateOrders";
        public const string CONST_METHOD_DETERMINE_SECURITY_BORROW_TYPE = "DetermineSecurityBorrowType";
        public const string CONST_METHOD_SYMBOL_SEARCH = "SymbolSearch";
        public const string CONST_METHOD_SM_SAVE_NEW_SYMBOL = "SMSaveNewSymbol";
        public const string CONST_METHOD_SM_SYMBOL_SEARCH = "SMSymbolSearch";
        public const string CONST_METHOD_CREATE_OPTION_SYMBOL = "CreateOptionSymbol";
        public const string CONST_METHOD_GET_CUSTOM_ALLOCATION_DETAILS = "GetCustomAllocationDetails";
        public const string CONST_METHOD_GET_SAVED_CUSTOM_ALLOCATION_DETAILS = "GetSavedCustomAllocationDetails";
        public const string CONST_METHOD_GET_SAVED_CUSTOM_ALLOCATION_DETAILS_BULK = "GetSavedCustomAllocationDetailsBulk";
        public const string CONST_METHOD_BOOK_AS_SWAP_REPLACE = "BookAsSwapReplace";
        public const string CONST_METHOD_SEND_REPLACE_ORDER = "SendReplaceOrder";
        public const string CONST_METHOD_SEND_LIVE_ORDER = "SendLiveOrder";
        public const string CONST_METHOD_SEND_MANUAL_ORDER = "SendManualOrder";
        public const string CONST_METHOD_SEND_STAGE_ORDER = "SendStageOrder";
        public const string CONST_METHOD_GET_ALGO_STRATEGIES_FROM_BROKER = "GetAlgoStrategiesFromBroker";
        public const string CONST_METHOD_GET_SYMBOL_ACCOUNT_POSITION = "GetSymbolAccountPosition";
        public const string CONST_METHOD_GET_COMPANY_USER_HOT_KEY_PREFERENCES = "GetCompanyUserHotKeyPreferences";
        public const string CONST_METHOD_UPDATE_COMPANY_USER_HOT_KEY_PREFERENCES = "UpdateCompanyUserHotKeyPreferences";
        public const string CONST_METHOD_GET_COMPANY_USER_HOT_KEY_PREFERENCES_DETAILS = "GetCompanyUserHotKeyPreferencesDetails";
        public const string CONST_METHOD_UPDATE_COMPANY_USER_HOT_KEY_PREFERENCES_DETAILS = "UpdateCompanyUserHotKeyPreferencesDetails";
        public const string CONST_METHOD_SAVE_COMPANY_USER_HOT_KEY_PREFERENCES_DETAILS = "SaveCompanyUserHotKeyPreferencesDetails";
        public const string CONST_METHOD_DELETE_COMPANY_USER_HOT_KEY_PREFERENCES_DETAILS = "DeleteCompanyUserHotKeyPreferencesDetails";
        public const string CONST_METHOD_GET_PREFERENCES = "GetPreferences";
        public const string CONST_METHOD_GET_SM_DATA = "GetSMData";
        public const string CONST_METHOD_CREATE_POPUP_TEXT = "CreatePopUpText";
        public const string CONST_METHOD_UNSUBSCRIBE_SYBMOL_COMPRESSION_FEED = "UnSubscribeSymbolCompressionFeed";
        public const string CONST_METHOD_BROKER_CONNECTION_AND_VENUES = "GetBrokerConnectionAndVenuesData/{tradingTicketId}";
        public const string CONST_METHOD_PST_ACCOUNTS_NAV = "PSTAccountsNav";
        public const string CONST_METHOD_CHECK_COMPLIANCE_FR_BASKET = "CheckComplianceFrBasket";
        public const string CONST_METHOD_SEND_PST_ORDERS = "SendPstOrders";
        public const string CONST_METHOD_CREATE_PST_ALLOCATION_PREF = "CreatePstAllocatonPreference";
        public const string CONST_GET_TRADE_ATTRIBUTES_LABELS = "GetTradeAttributesLabels";
        public const string CONST_GET_TRADE_ATTRIBUTES_VALUES = "GetTradeAttributesValues";
        public const string CONST_METHOD_GET_BROKER_CONNECTION_CACHE_DATA = "GetBrokerConnectionCacheData";
        public const string CONST_METHOD_SEND_ORDERS_TO_MARKET = "SendOrdersToMarket";
        public const string CONST_METHOD_VALIDATE_SYMBOL_UNIFIED = "ValidateSymbolUnified";
        public const string CONST_METHOD_DETERMINE_MULTIPLE_SECURITY_BORROW_TYPE = "DetermineMultipleSecurityBorrowType";
        #endregion

        #region ReportsPortalController Methods

        public const string CONST_METHOD_AUTHENTICATE = "Authenticate";
        public const string CONST_METHOD_GET_CUTOFFDATE = "GetCutOffDate";
        public const string CONST_METHOD_GET_USER_FUND = "GetUserSelectedFund";
        public const string CONST_METHOD_DEFAULT_LAYOUT = "DefaultLayout";
        public const string CONST_METHOD_REPORT_APPROVAL_LOG = "ReportsApprovalLogData";
        public const string CONST_METHOD_GET_USER_PREFRENCES = "GetUserPreferences";
        public const string CONST_METHOD_SET_USER_PREFRENCES = "SetUserPreferences";
        public const string CONST_METHOD_SET_USER_LAYOUT_PREFERENCES = "SetUserDefaultLayoutPreferences";
        public const string CONST_METHOD_POST_GenerateReport = "GenerateReport";
        public const string CONST_METHOD_POST_UpdateReports = "UpdateReports";
        public const string CONST_METHOD_POST_UpdateReportSessions = "UpdateReportSessions";
        public const string CONST_METHOD_POST_GetNewlyApprovedReportsAndStatus = "GetNewlyApprovedReportsAndStatus";
        public const string CONST_METHOD_POST_RemoveReport = "RemoveReport";
        public const string CONST_METHOD_POST_CancelApproval = "CancelApproval";
        public const string CONST_METHOD_POST_SetReportsApprovalLogDate = "SetReportsApprovalLogDate";
        public const string CONST_METHOD_POST_ZipReportFiles = "ZipReportFiles";
        public const string CONST_METHOD_POST_DownloadReportsZip = "DownloadReportsZip";
        public const string CONST_METHOD_POST_DownloadExcelFile = "DownloadExcelFile";
        public const string CONST_METHOD_POST_SaveDefaultLayout = "SaveDefaultLayout";
        public const string CONST_METHOD_POST_EntitySelect = "EntitySelect";
        public const string CONST_METHOD_POST_GetAllMasterfunds = "GetAllMasterfunds";
        //public const string CONST_TOUCH_URL = "TouchUrl";
        public const string CONST_REPORT_PORTAL_API = "/ReportsPortalApi/";
        public const string CONST_SSO = "/SSO/";

        #endregion

        #region WatchlistController Method

        public const string CONST_METHOD_GET_TAB_NAMES = "GetTabNames";
        public const string CONST_METHOD_GET_TABWISE_SYMBOLS = "GetTabWiseSymbols";
        public const string CONST_METHOD_ADD_TAB = "AddTab";
        public const string CONST_METHOD_RENAME_TAB = "RenameTab";
        public const string CONST_METHOD_DELETE_TAB = "DeleteTab";
        public const string CONST_METHOD_ADD_SYMBOL_IN_TAB = "AddSymbolInTab";
        public const string CONST_METHOD_REMOVE_SYMBOL_FROM_TAB = "RemoveSymbolFromTab";

        #endregion

        #region ComplianceController Methods
        public const string CONST_METHOD_SEND_COMPLIANCE_DATA = "SendComplianceData";
        public const string CONST_METHOD_SEND_COMPLIANCE_DATA_FOR_STAGE = "SendComplianceDataForStage";
        public const string CONST_METHOD_RECEIVED_COMPLIANCE_DATA = "ComplianceAlertsDataReceived";
        public const string CONST_METHOD_RECEIVED_COMPLIANCE_DATA_SYNC = "ComplianceAlertsDataReceivedSync";
        #endregion

        #region PMContoller Methods

        public const string CONST_REQUEST_CONTINUOUS_DATA = "RequestContinousData";

        #endregion

        #region RtpnlController Methods

        public const string CONST_ACTION__GET_RTPNL_DATA = "GetRtpnlData";
        public const string CONST_METHOD_GET_RTPNL_DATA = "GetRtpnlData/{widgetType}";
        public const string CONST_METHOD_SAVEUPDATE_CONFIG_DETAILS = "SaveUpdateConfigDetails";
        public const string CONST_METHOD_GET_RTPNL_WIDGET_COUNT = "GetRtpnlWidgetCount";
        public const string CONST_METHOD_GET_RTPNL_WIDGET_DATA = "GetRtpnlWidgetData";
        public const string CONST_METHOD_GET_RTPNL_WIDGET_CONFIG_DATA = "GetRtpnlWidgetConfigData";
        public const string CONST_METHOD_DELETE_OPENFIN_PAGES = "DeleteOpenfinPage";
        public const string CONST_METHOD_SAVE_CONFIG_DATA_FOR_EXTRACT = "SaveConfigDataForExtract";
        public const string CONST_ACTION__CHECK_CALCULATION_SERVICE_RUNNING = "CheckCalculationServiceRunning";
        public const string CONST_ACTION_REGISTERRTPNLUSER = "RegisterRTPNLUser";
        public const string CONST_ACTION_DEREGISTERRTPNLUSER = "DeRegisterRTPNLUser";
        public const string CONST_METHOD_DELETE_REMOVED_WIDGET_CONFIG_DETAILS = "DeleteRemovedWidgetConfigDetails";
        public const string CONST_METHOD_CHECK_MASTERFUND_CACHE = "CheckMasterFundCache";
        public const string CONST_METHOD_RTPNL_TRADE_ATTRIBUTES_LABELS_REQUEST = "RtpnlTradeAttributeLabelsRequest";
        #endregion

        #region OpenfinManagementControllerMethods
        public const string CONST_METHOD_GET_DEFAULT_OPENFIN_WORKSPACE = "GetDefaultOpenfinWorkspace";
        public const string CONST_METHOD_SAVE_DEFAULT_OPENFIN_WORKSPACE = "SaveDefaultOpenfinWorkspace";
        public const string CONST_METHOD_DELETE_OPENFIN_WORKSPACE = "DeleteOpenfinWorkspace";
        public const string CONST_METHOD_GET_OPENFIN_PAGE_INFO = "GetOpenfinPageInfo";
        public const string CONST_METHOD_SAVE_OPENFIN_PAGE_INFO = "SaveOpenfinPageInfo";
        #endregion


        #region Power BI 
        public const string CONST_GET_POWERBI_EMBED_INFO = "GetPowerBIReportEmbedInfo/{workspaceId}/{reportId}";
        #endregion
        #region Service Health Status 
        public const string CONST_SERVICE_HEALTH_STATUS = "ServiceStatus";
        #endregion
    }
}
