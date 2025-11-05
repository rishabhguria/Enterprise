namespace Prana.CalculationService.Constants
{
    internal class RtpnlConstants
    {
        #region Message constants
        public const string MSG_CalculationServicestarted = "Calculation Service started at:- {0} , (local time) {1}";
        public const string MSG_ShutdownService = "Shutting down service.";
        public const string MSG_CalculationServiceClosed = "Calculation Service successfully closed at:- {0} , (local time) {1}";
        public const string MSG_UserLoggedInInfoReceived = "User logged-in information received for userId ";
        public const string MSG_UserLoggedInInfoProcessed = "User logged-in information processed for userId ";
        public const string MSG_UserLoggedOutInformationReceived = "User logged-out information received for userId ";
        public const string MSG_UserLoggedOutInformationProcessed = "User logged-out information processed for userId ";
        public const string MSG_LoadViewsRequestedBy = "Load views request received for userId ";
        public const string MSG_LoadViewsResponseFor = "Load views request processed for userId ";
        public const string MSG_SaveViewsRequestedBy = "Save views request received for userId ";
        public const string MSG_SaveViewsResponseFor = "Save views request processed for userId ";
        public const string MSG_InitRequestForEsper = "Initialization request sent to Esper engine";
        public const string MSG_InitCompleteFromEsper = "Initialization complete from Esper engine";
        public const string MSG_RtpnlDataRequestReceived = "Rtpnl data request received for userId ";
        public const string MSG_RtpnlDataRequestProcessed = "Rtpnl data request processed for userId ";
        public const string MSG_WAITING_FOR_COMMON_DATA_SERVICE = "Waiting for response from Common data service...";
        public const string MSG_WAITING_FOR_COMPLIANCE_ENGINE = "Waiting for response from Esper engine....";
        public const string MSG_COMPLIANCE_PERMISSION_RECEIVED = "Compliance permissions response received";
        public const string MSG_COMPLIANCE_PERMISSION_PROCESSED = "Compliance permissions response processed";
        public const string MSG_COMPLIANCE_PERMISSION_CHANGED = "Company compliance permission is disabled. Please restart the service";
        public const string MSG_SAVE_UPDATE_CONFIG_DETAILS_REQUEST_RECEIVED = "Save and Update configuration request received for userId ";
        public const string MSG_SAVE_UPDATE_CONFIG_DETAILS_REQUEST_PROCESSED = "Save and Update configuration request processed for userId ";
        public const string MSG_SAVE_CONFIG_FOR_EXTRACT_REQUEST_RECEIVED = "Save configuration for Extract request received for userId ";
        public const string MSG_SAVE_CONFIG_FOR_EXTRACT_REQUEST_PROCESSED = "Save configuration for Extract request processed for userId ";
        public static readonly string MSG_CONST_COMPANY_HAS_NO_COMPLIANCE_PERMISSION = "Company has no permission for Compliance Engine and RTPNL services are unavailable.\nPlease contact Administrator.";
        public const string MSG_USER_PERMITTED_ACCOUNTS_REQUEST_RECEIVED = "User permitted accounts request received for userId ";
        public const string MSG_USER_PERMITTED_ACCOUNTS_REQUEST_PROCESSED = "User permitted accounts request processed for userId ";
        public const string MSG_UNALLOCATED_ACCOUNT_PERMISSION_NOT_AVAILABLE_ = "Unallocated account permission is not available for userId ";
        public const string MSG_CheckCalculationServiceRunningProcessed = "Calculation service running request processed with response ";
        public const string MSG_CheckCalculationServiceRunningReceived = "Calculation service running request received ";
        public const string MSG_RtpnlWidgetCountRequestReceived = "Rtpnl widget count request received for userId ";
        public const string MSG_RtpnlWidgetCountRequestProcessed = "Rtpnl widget count request processed for userId ";
        public const string MSG_CheckCalculationServiceRunningRequestReceived = "Api call to check calculation service is running request received for userId ";
        public const string MSG_CheckCalculationServiceRunningRequestProcessed = "Api call to check calculation service is running request processed for userId ";
        public const string MSG_RtpnlWidgetDataRequestReceived = "Rtpnl widget data request received for userId ";
        public const string MSG_RtpnlWidgetDataRequestProcessed = "Rtpnl widget data request processed for userId ";
        public const string MSG_RtpnlConfigWidgetDataRequestReceived = "Rtpnl config widget data request received for userId ";
        public const string MSG_RtpnlConfigWidgetDataRequestProcessed = "Rtpnl config widget data request processed for userId ";
        public const string MSG_LoadRtpnlViewsForLoggedInUserExecutionStarted = "Load rtpnl views for logged in user method execution started for userId: ";
        public const string MSG_DeleteOpenfinPageInformationReceived = "Delete the Openfin Page information for userId ";
        public const string MSG_DeleteOpenfinPageInformationProcessed = "Delete Openfin Page Information request processed for userId: ";
        public const string MSG_ExceptionThrown = "An exception occur while executing the process: ";
        public const string MSG_CacheUpdatedWithData = "Cache updated with data: ";
        public const string MSG_DashLines = "----------------------------------------------------------------------------------------------------------------------";
        public const string MSG_UnsavedWidgetHandling = "Handling of unsaved widgets:- ";
        public const string MSG_UserId = " UserId:";
        public const string MSG_LayoutItem = " layoutItem: ";
        public const string MSG_ViewName = " viewName: ";
        public const string MSG_SpecificWidgetDataDeleted = "Deleted Widget data in View:- ";
        public const string MSG_WidgetDataDeleted = "Widget data deleted:- ";
        public const string MSG_DeleteUnsavedWidgetMES = "DeleteUnsavedWidgetsfromViews method execution started";
        public const string MSG_UserInformationNotAvailable = "User Information not available for User:";
        public const string MSG_NoLayoutAvailableForView = "No layout in the cache for view:";
        public const string MSG_ProblemInLayoutCache = "Problem in user wise cache for user ID:";
        public const string MSG_SAVE_CONFIG_DETAILS_REQUEST_FOR_SAVEPAGEAS_PROCESSED = "Save configuration request for save page as operation processed for userId ";
        public const string MSG_SAVE_CONFIG_DETAILS_REQUEST_FOR_SAVEPAGEAS_RECEIVED = "Save configuration request for save page as operation received for userId ";
        public const string MSG_DynamicUDAInformationReceived = "All Dynamic UDA information has been received from the Esper engine.";
        public const string MSG_DynamicUDADataIncorrect = "Dynamic UDA DataSet does not contain the required columns: Tag, HeaderCaption, DefaultValue.";
        public const string MSG_DynamicUDAInformationNotReceived = "No data received for Dynamic UDA from the Esper engine.";

        #endregion

        #region Error messages
        public const string MSG_ErrorForSaveLayout = "Error saving RTPNL view";
        public const string MSG_ErrorForSaveUpdateWidgetConfig = "Error saving widget configuration details";
        public const string MSG_Error_Widget_Name_Cannot_Empty = "Widget name connot empty";
        public const string MSG_Error_Widget_Name_Too_Large = "Widget name too large";
        public const string MSG_Error_Widget_Name_Already_Exists = "Title already exists";
        public const string Msg_Error_DB_Null_InternalPageInfoDTO = "Found Page Layout as null in InternalPageInfoDTO";
        public const string Msg_Error_DB_Null_OpenFinPageInfoDTO = "Found Page Layout as null in OpenFinPageInfoDTO";
        #endregion

        #region Communication constants
        public const string CONST_Underscore = "_";
        public const string CONST_OtherDataSender = "OtherDataSender";
        public const string CONST_ResponseType = "ResponseType";
        public const string CONST_InitRequest = "InitRequestCalculationService";
        public const string CONST_InitResponse = "InitResponseCalculationService";
        public const string CONST_CommunicationRequestFromEsper = "CommunicationRequestFromEsper";
        public const string CONST_CommunicationResponseForEsper = "CommunicationResponseForEsper";
        public const string CONST_TypeOfRequest = "TypeOfRequest";
        public const string CONST_AccountNav = "AccountNav";
        public const string CONST_SymbolNav = "SymbolNav";
        public const string CONST_MasterFundNav = "MasterFundNav";
        public const string CONST_AccountSymbolNav = "AccountSymbolNav";
        public const string CONST_MasterFundSymbolNav = "MasterFundSymbolNav";
        public const string CONST_DynamicUDAInformation = "DynamicUDAInformation";
        public const string CONST_ExtendedAccountSymbolWithNav = "ExtendedAccountSymbolWithNav";
        public const string CONST_RowCalculationBaseWithNavStartupData = "RowCalculationBaseWithNavStartupData";
        public const string CONST_GlobalNav = "GlobalNav";
        public const string CONST_Account = "Account";
        public const string CONST_Symbol = "Symbol";
        public const string CONST_MasterFund = "MasterFund";
        public const string CONST_AccountSymbol = "Account-Symbol";
        public const string CONST_MasterFundSymbol = "MasterFund-Symbol";
        public const string CONST_Global = "Global";
        public const string CONST_RTPNL_GRID_DATA = "RtpnlGridData";
        public const string CONST_RTPNL_CONFIG_DATA = "RtpnlConfigData";
        public const string CONST_FUND = "Fund";
        public const string CONST_ACCOUNT_SYMBOL = "Symbol Account";
        public const string CONST_FUND_SYMBOL = "Symbol Fund";
        public const string CONST_WIDGET_CONFIG_DATA = "WidgetConfigData";
        public const string CONST_START_UP_GRID_DATA = "StartUpGridData";
        public const string CONST_SUMMARY = "Summary";
        public const string CONST_UNALLOCATED_ACCOUNT_PERMISSION = "UnallocatedAccountPermission";
        public const string CONST_ERROR = "errorMessage";
        public const string CONST_ERROR_MSG = "The RT P&L Calculation Service is not started completely";
        public const string CONST_DYNAMIC_UDA_DATA = "DynamicUDAData";
        public const string CONST_ROW_CALCULATION = "RowCalculationNav";
        public const string CONST_ROW_BASE_NAV_CALCULATION = "RowCalculationBaseNav";
        public const string CONST_ROW_BASE_NAV_CALCULATION_QUANTITY_ZERO = "RowCalculationBaseNavQuantityZero";
        public const string CONST_Dash = "-";
        public const string CONST_NO_POSITION = "NO-Position";

        #endregion

        #region Configuration table columns
        public const string CONST_WidgetName = "WidgetName";
        public const string CONST_WidgetType = "WidgetType";
        public const string CONST_DefaultColumns = "DefaultColumns";
        public const string CONST_ColoredColumns = "ColoredColumns";
        public const string CONST_GraphType = "GraphType";
        public const string CONST_IsFlashColorEnabled = "IsFlashColorEnabled";
        public const string CONST_ChannelDetail = "ChannelDetail";
        public const string CONST_LinkedWidget = "LinkedWidget";
        public const string CONST_ViewName = "ViewName";
        public const string CONST_WidgetKey = "WidgetId";
        public const string CONST_PrimaryMetric = "PrimaryMetric";
        #endregion

        #region Stored procedure
        public const string CONST_UserID = "@userID";
        public const string CONST_PageID = "@pageID";
        public const string CONST_AT_OldViewName = "@oldViewName";
        public const string CONST_AT_NewViewName = "@newViewName";
        public const string CONST_AT_ViewName = "@viewName";
        public const string CONST_AT_Removed_Widgets = "@removedWidgets";
        public const string CONST_AT_WidgetName = "@widgetName";
        public const string CONST_AT_WidgetType = "@widgetType";
        public const string CONST_AT_DefaultColumns = "@defaultColumns";
        public const string CONST_AT_ColoredColumns = "@coloredColumns";
        public const string CONST_AT_GraphType = "@graphType";
        public const string CONST_AT_IsFlashColorEnabled = "@isFlashColorEnabled";
        public const string CONST_AT_ChannelDetail = "@channelDetail";
        public const string CONST_AT_LinkedWidget = "@linkedWidget";
        public const string CONST_AT_WidgetKey = "@widgetId";
        public const string CONST_AT_PrimaryMetric = "@primaryMetric";
        public const string CONST_P_RTPNL_GetUserWidgetConfigDetails = "P_RTPNL_GetUserWidgetConfigDetails";
        public const string CONST_P_RTPNL_SaveUserWidgetConfigDetails = "P_RTPNL_SaveUserWidgetConfigDetails";
        public const string CONST_P_UpdateViewNameWidgetConfigDetails = "P_UpdateViewNameWidgetConfigDetails";
        public const string CONST_P_DeleteUnsavedWidgetsFromView = "P_DeleteUnsavedWidgetsFromView";
        public const string CONST_P_Samsara_SaveOpenfinPageInfo = "P_Samsara_SaveOpenfinPageInfo";
        public const string CONST_P_Samsara_GetOpenfinPageInfo = "P_Samsara_GetOpenfinPageInfo";
        public const string CONST_P_Samsara_GetCompanyUserLayouts = "P_Samsara_GetCompanyUserLayouts";
        public const string CONST_AT_PageName = "@pageName";
        public const string CONST_AT_OldPageName = "@oldPageName";
        public const string CONST_AT_PageTag = "@pageTag";
        public const string CONST_AT_PageId = "@pageId";
        public const string CONST_AT_PageLayout = "@pageLayout";
        public const string CONST_AT_WidgetKeys = "@widgetKeys";
        public const string CONST_AT_ChannelDetails = "@channelDetails";
        public const string CONST_P_SavedcolourDetailsFromHeaderForWidgets = "P_SavedcolourDetailsFromHeaderForWidgets";
        public const string CONST_P_Samsara_DeleteOpenfinPage = "P_Samsara_DeleteOpenfinPages";
        #endregion

        #region EOM constants
        public const string CONST_MasterFundNavEOM = "MasterFundNavEOM";
        public const string CONST_AccountNavEOM = "AccountNavEOM";
        public const string CONST_AccountSymbolNavEOM = "AccountSymbolNavEOM";
        public const string CONST_MasterFundSymbolNavEOM = "MasterFundSymbolNavEOM";
        public const string CONST_SymbolNavEOM = "SymbolNavEOM";
        public const string CONST_GlobalNavEOM = "GlobalNavEOM";
        public const string CONST_RowCalculationEOM = "RowCalculationEOM";
        #endregion

        public const string CONFIG_APPSETTING_RtpnlUpdateTimeInterval = "RtpnlUpdateTimeInterval";
        public const string CONFIG_APPSETTING_RtpnlEventsCountLogInterval = "RtpnlEventsCountLogInterval";
        public const string CONFIG_APPSETTING_IsCustomGroupingEnabled = "IsCustomGroupingEnabled";
        public const string CONFIG_APPSETTING_SendDataToRtpnlWidgetsInterval = "SendDataToRtpnlWidgetsInterval";
        public const string CONFIG_APPSETTING_IsUpdatesLogsRequired = "IsUpdatesLogsRequired";
        public const string CONFIG_APPSETTING_SymbolsRequireLogging = "SymbolsRequireLogging";
        public const string CONST_Multiply = "M";
        public const string CONST_Divide = "D";
        public const int CONST_GLOBAL_ID = 0;
        public const string CONST_EMPTY_STRING = "";
        public const char CONST_TILDE = '~';
        public const string CONST_TAB_NAME = "Tab 1";
        public const string CONST_RTPNL_VIEW_LAYOUT = "RtpnlViewLayout";
        public const string CONST_TXT = ".txt";
        public const string CONST_CUSTOM_GROUPING_PERMISSION = "CustomGroupingPermission";
        public const string CONST_CALCULATION_SERVICE_STARTED = "CalculationServiceStarted";

        #region OpenFinPageInfo table columns
        public const string CONST_PageName = "PageName";
        public const string CONST_PageId = "PageId";
        public const string CONST_PageLayout = "PageLayout";
        public const string CONST_PageTag = "PageTag";
        public const string CONST_ViewId = "ViewId";
        #endregion

        #region CompanyUserLayouts table columns
        public const string CONST_FileName = "FileName";
        public const string CONST_FileData = "FileData";
        public const string CONST_ModuleName = "ModuleName";
        public const string CONST_Description = "Description";
        #endregion
    }
}
