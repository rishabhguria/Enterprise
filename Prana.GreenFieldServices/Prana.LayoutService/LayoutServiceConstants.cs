using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.LayoutService
{
    /// <summary>
    /// This Class is responsible for holding all the Constants to be used for the Layout Service.    
    /// </summary>    
    /// <remarks>
    /// The constants defined in this class should be used wherever the specific values are needed in the Layout Service.
    /// This helps in centralizing the maintenance of these values and ensures that any changes are reflected across the service.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// string message = LayoutServiceConstants.MSG_CacheClearRequestReceived + userId;
    /// </code>
    /// </example>
    /// <author> Karan Joshi </author> 
    public class LayoutServiceConstants
    {
        public const string MSG_CacheClearRequestReceived = "Cache Cleared for UserId:";
        public const string MSG_ApplicationLayoutCacheUpdated = "Application Cache updated after user Login: ";
        public const string MSG_GetApplicationLayoutReceived = "Get application layout information request received by UserId: ";
        public const string MSG_GetApplicationLayoutProcessed = "Get application layout information request processed for UserId: ";
        public const string MSG_SaveApplicationLayoutRequestReceived = "Save application layout information request received by UserId: ";
        public const string MSG_SaveApplicationLayoutRequestProcessed = "Save application layout information request processed with data: ";
        public const string MSG_ExceptionThrown = "An exception occur while executing the process: ";
        public const string MSG_ApplicationLayoutCacheUpdatedWithData = "Application layout cache updated with data: ";
        public const string MSG_DeleteApplicationLayoutRequestProcessed = "Delete application layout information request processed with updated data: ";
        public const string MSG_DeleteApplicationLayoutRequestReceived = "Delete application layout information request received by UserId: ";
        public const string MSG_DeleteOpenfinPageInformationReceived = "Delete the Openfin Page information for userId ";
        public const string MSG_DeleteOpenfinPageInformationProcessed = "Delete Openfin Page Information request processed for userId: ";
        public const string MSG_LayoutServiceStart = "LayoutService started at:-";
        public const string MSG_LayoutServiceClose = "LayoutService successfully closed at:- ";
        public const string MSG_LayoutServiceShutDown = "Shutting down service.";
        public const string MSG_LocalTime = " (local time)";
        public const string MSG_WaitingForAuthService = "Waiting for AuthService...";
        public const string MSG_KafkaPath = "KafkaConfigPath";
        public const string MSG_LoadViewsRequestedBy = "Load views request received for userId ";
        public const string MSG_LoadViewsResponseFor = "Load views request processed for userId ";
        public const string MSG_SaveViewsRequestedBy = "Save views request received for userId ";
        public const string MSG_SaveViewsResponseFor = "Save views request processed for userId ";
        public const string MSG_ErrorForSaveLayout = "Error saving RTPNL view";

        public const string CONST_UserID = "@userID";

        public const string CONST_P_Samsara_GetWorkspaceInfo = "P_Samsara_GetOpenfinWorkspaceInfo";
        public const string CONST_P_Samsara_SaveWorkspaceInfo = "P_Samsara_SaveOpenfinWorkspaceInfo";
        public const string CONST_P_Samsara_DeleteWorkspaceInfo = "P_Samsara_DeleteOpenfinWorkspaceInfo";
        public const string CONST_P_Samsara_SavePageInfo = "P_Samsara_SaveOpenfinPageInfo";
        public const string CONST_P_Samsara_GetPageInfo = "P_Samsara_GetOpenfinPageInfo";
        public const string CONST_P_Samsara_GetCompanyUserLayouts = "P_Samsara_GetCompanyUserLayouts";
        public const string CONST_P_Samsara_SaveCompanyUserLayouts = "P_Samsara_SaveCompanyUserLayouts";
        public const string CONST_P_Samsara_DeleteOpenfinPage = "P_Samsara_DeleteOpenfinPages";
        public const string CONST_P_SavedcolourDetailsFromHeaderForWidgets = "P_SavedcolourDetailsFromHeaderForWidgets";
        public const string CONST_P_Samsara_RemovePagesForAnUser = "P_Samsara_RemovePagesForAnUser";
        public const string CONST_P_Samsara_LogDeletedPageForUser = "P_Samsara_LogDeletedPageForUser";

        public const string CONST_AT_PageName = "@pageName";
        public const string CONST_AT_OldPageName = "@oldPageName";
        public const string CONST_AT_PageTag = "@pageTag";
        public const string CONST_AT_PageId = "@pageId";
        public const string CONST_AT_PageLayout = "@pageLayout";
        public const string CONST_AT_WidgetKeys = "@widgetKeys";
        public const string CONST_AT_ChannelDetails = "@channelDetails";
        public const string CONST_AT_ViewId = "@viewId";
        public const string CONST_AT_ModuleName = "@moduleName";
        public const string CONST_AT_Description = "@description";
        public const string CONST_AT_ViewName = "@viewName";
        public const string CONST_AT_ViewLayout = "@viewLayout";
        public const string CONST_TAB_OLD_NAME = "TabOldName";
        public const string CONST_TAB_NEW_NAME = "TabNewName";
        public const string CONST_CUSTOM_TAB_CREATED = "Custom Tab Created";
        public const string CONST_PAGES = "pages";
        public const string CONST_PAGE_ID = "pageId";
        public const string CONST_WINDOWS = "windows";
        public const string CONST_SNAPSHOT = "snapshot";
        public const string CONST_WORKSPACE_INFO = "workspaceInfo";
        public const string CONST_WORKSPACE_PLATFORM = "workspacePlatform";


        public const string Msg_Error_DB_Null_InternalPageInfoDTO = "Found Page Layout as null in InternalPageInfoDTO";
        public const string Msg_Error_DB_Null_PageInfoDTO = "Found Page Layout as null in OpenFinPageInfoDTO";
        public const string MSG_RenameCustomTabRequestedBy = "Rename custom tab request received for userId ";
        public const string MSG_RenameCustomTabResponseFor = "Rename custom tab request processed for userId ";
        public const string MSG_RemoveCustomTabRequestedBy = "Remove custom tab request received for userId ";
        public const string MSG_RemoveCustomTabResponseFor = "Remove custom tab processed for userId ";
        public const string MSG_ErrorFailedToRenameTab = "Error occurred while completing Rename tab operation!";
        public const string MSG_ErrorFailedToRemoveTab = "Error occurred  while completing Remove tab operation!";


        #region OpenFinPageInfo table columns
        public const string CONST_PageName = "PageName";
        public const string CONST_PageId = "PageId";
        public const string CONST_PageLayout = "PageLayout";
        public const string CONST_PageTag = "PageTag";
        public const string CONST_ViewId = "ViewId";
        public const string CONST_ViewName = "ViewName";
        public const string CONST_ViewLayout = "ViewLayout";
        public const string CONST_Description = "Description";
        public const string CONST_ModuleName = "ModuleName";
        public const string CONST_TAB_NAME_DYNAMIC_ORDER = "Dynamic_Order_";
        public const string CONST_ORDER_SEPARATOR = "_Order_";
        public const string CONST_SUBORDER_SEPARATOR = "_SubOrder_";
        #endregion

        #region ModuleNames
        public const string CONST_MODULE_Blotter = "Blotter";
        public const string CONST_MODULE_RTPNL = "Rtpnl";
        #endregion
    }
}
