namespace Prana.BusinessObjects.BlotterDataService
{
    public class BlotterDataConstants
    {
        public const string CONST_OrderTab = "OrderTab";
        public const string CONST_WorkingTab = "WorkingTab";
        public const string CONST_RemovedOrders = "RemovedOrders";
        public const string CONST_Cancelled = "Cancelled";
        public const string CONST_PendingNew = "PendingNew";
        public const string CONST_PendingReplace = "PendingReplace";
        public const string CONST_New = "New";
        public const string CONST_BlockedByCompliance = "Blocked by Compliance";
        public const string CONST_BRACKET_OPEN = "[";
        public const string CONST_BRACKET_CLOSE = "] ";
        public const string CONST_PARENTCLORDERID = "parentClOrderId";
        public const string CONST_ORDER_QTY = "OrderQuantity";
        public const string CONST_FUND_ID = "FundID";
        public const string CONST_ACCOUNT = "account";
        public const string CONST_ALLOCATION_PREFERENCE = "AllocationPreference";
        public const string CONST_IS_GROUPED_ORDER = "IsGroupedOrder";
        public const string CONST_TARGET_ALLOCATION_PERCENTAGE = "TargetPercent";
        public const string CONST_PERCENTAGE_ON_TOTAL_QTY = "% Executed Quantity";
        public const string CONST_FILL = "fill";
        public const string CONST_PRICE = "price";
        public const string CONST_MANUALFILLS = "ManualFills";
        public const string CONST_TRADINGTICKETUIPREF = "TradingTicketUIPreference";
        public const string CONST_ACCOUNTLIST = "AccountList";
        public const string CONST_ALLOCATED_DATA = "AllocatedData";
        public const string CONST_LIMIT = "Limit";
        public const string CONST_AT = "@";
        public const string CONST_SPACE = " ";
        public const string CONST_COMMA_SPACE = ", ";
        public const string CONST_SPACE_AT = " at ";
        public const string CONST_TITLE = "Title";
        public const string CONST_MESSAGE = "Message";
        public const string CONST_COMPANY_USER_ID = "CompanyUserId";
        public const string CONST_ZERO = "0";
        public const string CONST_ACK = "Ack";
        public const string CONST_CANCEL_ACK = "CancelAck";
        public const string CONST_ASSET_ID = "AssetID";
        public const string CONST_ALLOCATION_ENTERED = "Allocation Entered";
        public const string CONST_REMAINING = "Remaining";
        public const string CONST_EXCESS = "Excess";
        public const string CONST_ALLOCATION_PERCENTAGE_ENTERED = "! Allocation % Entered:";
        public const string CONST_PERCENTAGE = "%";
        public const string CONST_DEFAULT_BROKERS = "Default Broker(s)";
        #region Blotter Message section

        public const string MSG_StrategyUnallocated = "Strategy Unallocated";
        public const string MSG_ShutdownService = "Shutting down service.";
        public const string MSG_GetDataForBlotterOrderGridCountAndWorkingGridCount = "GetData for Blotter has been completed to User Id: {0}, Order Grid Count: {1}, Working Tab Grid Count: {2}";
        public const string MSG_GetDataForBlotterCompleted = "GetData for Blotter has been completed to User Id: ";
        public const string MSG_GetDataForBlotterNotCompleted = "--> GetData for Blotter can't be completed for User Id: {0}, Error {1}";
        public const string MSG_PublishDataForBlotterOrderGridCountAndWorkingGridCount = "Publish Data on Blotter for all Users, Order Grid Count: {0}, Working Tab Grid Count: {1}";
        public const string MSG_BlotterServicestarted = "Blotter Data Service started at:- {0} , (local time) {1}";
        public const string MSG_RemoveDataOnBlotterForAllUserAndCount = "Remove Data on Blotter, for all Users, Count: {0}, BlotterRequestType: {1}";
        public const string MSG_InternalServerError = ", Error: Internal Server Error";
        public const string MSG_BlotterServiceClosed = "BlotterDataService successfully closed at:- {0} , (local time) {1}";
        public const string MSG_FixConnectionDown = "Fix Connection for Broker ";
        public const string MSG_ResendOrder = " is down. Please resend your order.";
        public const string MSG_GetDataRequestedBy = "GetData request received by userId ";
        public const string MSG_GetDataResponseFor = "GetData request processed for userId ";
        public const string MSG_CacheDataRequested = "User cache data request for ";
        public const string MSG_CacheDataReceived = "User cache data received for ";
        public const string MSG_BlotterPreferenceDataSetupStarted = "Blotter admin preference data setup started";
        public const string MSG_BlotterPreferenceDataSetupProcessed = "Blotter admin preference data setup processed";
        public const string MSG_CompanyTransferTradeRulesReceived = "Company transfer trade rules received for userId ";
        public const string MSG_CompanyTransferTradeRulesProcessed = "Company transfer trade rules processed for userId ";
        public const string MSG_UserLoggedInInformationReceived = "User logged-in information received for userId ";
        public const string MSG_UserLoggedInInformationProcessed = "User logged-in information processed for userId ";
        public const string MSG_UserLoggedOutInformationReceived = "User logged-out information received for userId ";
        public const string MSG_UserLoggedOutInformationProcessed = "User logged-out information processed for userId ";
        public const string MSG_CompanyUserInformationReceived = "Company user information received for userId ";
        public const string MSG_CompanyUserInformationProcessed = "Company user information processed for userId ";
        public const string MSG_CounterPartyConnectionDetailsReceived = "Counter party connection details received for userId ";
        public const string MSG_CounterPartyConnectionDetailsProcessed = "Counter party connection details processed for userId ";
        public const string MSG_UserDataForBlotterReceived = "User data for blotter received for userId ";
        public const string MSG_UserDataForBlotterProcessed = "User data for blotter processed for userId ";
        public const string MSG_CacheUserDataRequest = "Cache User Data request received";
        public const string MSG_CacheUserDatarResponse = "Cache User Data request processed";
        public const string MSG_BlotterCancelAllSubsRequestedBy = "Blotter Cancel all(subs) request received by userId ";
        public const string MSG_BlotterCancelAllSubsResponseFor = "Blotter Cancel all(subs) request processed for userId ";
        public const string MSG_BlotterRemoveOrdersRequestedBy= "Blotter Remove orders request received by userId ";
        public const string MSG_BlotterRemoveOrdersResponseFor = "Blotter Remove orders request processed for userId ";
        public const string MSG_FreezePendingComplianceRowsRequestedBy = "Freeze pending compliance rows request received by userId ";
        public const string MSG_FreezePendingComplianceRowsResponseFor = "Freeze pending compliance rows request processed for userId ";
        public const string MSG_UnfreezePendingComplianceRowsRequestedBy = "Unfreeze pending compliance rows request received by userId ";
        public const string MSG_UnfreezePendingComplianceRowsResponseFor = "Unfreeze Pending Compliance Rows request processed for userId ";
        public const string MSG_BlotterRemoveManualExecutionRequestedBy = "Blotter remove manual execution request received by userId ";
        public const string MSG_BlotterRemoveManualExecutionResponseFor = "Blotter remove manual execution request processed for userId ";
        public const string MSG_BlotterGetManualFillsRequestedBy = "Blotter get manual fills request received by userId ";
        public const string MSG_BlotterGetManualFillsResponseFor = "Blotter Get Manual Fills request processed for userId ";
        public const string MSG_BlotterSaveManualFillsRequestedBy = "Blotter save manual fills request received by userId ";
        public const string MSG_BlotterSaveManualFillsResponseFor = "Blotter Save Manual Fills request processed for userId ";
        public const string MSG_GetAllocationDetailsRequestedBy = "Get allocation details request received bu userId ";
        public const string MSG_GetAllocationDetailsResponseFor = "Get allocation details request processed for userId ";
        public const string MSG_SaveAllocationDetailsRequestedBy = "Save allocation details request received by userId ";
        public const string MSG_SaveAllocationDetailsResponseFor = "Save allocation details request processed for userId ";
        public const string MSG_LoadLayoutRequestedBy = "Load layout request received for userId ";
        public const string MSG_LoadLayoutResponseFor = "Load layout request processed for userId ";
        public const string MSG_SaveLayoutRequestedBy = "Save layout request received for userId ";
        public const string MSG_SaveLayoutResponseFor = "Save layout request processed for userId ";
        public const string MSG_RenameCustomTabRequestedBy = "Rename custom tab request received for userId ";
        public const string MSG_RenameCustomTabResponseFor = "Rename custom tab request processed for userId ";
        public const string MSG_RemoveCustomTabRequestedBy = "Remove custom tab request received for userId ";
        public const string MSG_RemoveCustomTabResponseFor = "Remove custom tab processed for userId ";
        public const string MSG_GetTradeServerConnectionDetailsRequest = "Get Trade Server Connection Details request received";
        public const string MSG_GetTradeServerConnectionDetailsResponse = "Get Trade Server Connection Details request processed";
        public const string MSG_OrderUpdatePublishedFor = "Order update published for orderId ";
        public const string MSG_ConnectToAllSocketsRequest = "Connect To All Sockets request received";
        public const string MSG_ConnectToAllSocketsResponse = "Connect To All Sockets request processed";
        public const string MSG_ExecQtyGreaterThanTargetQty = "The Executed Qty is greater than Target Qty!!! Please Check.";
        public const string MSG_ExecQtyGreaterThanWorkingQty = "Executed quantity cannot be greater than the combined remaining and working quantity.";
        public const string MSG_ErrorSavingModifiedFills = "Error occurred while Saving modified fills!";
        public const string MSG_ErrorRemovingManualExecution = "Error occurred while Removing manual execution!";
        public const string MSG_ErrorCancelOrders = "Error occurred while Cancelling order!";
        public const string MSG_ErrorRolloverOrders = "Error occurred in order Rollover!";
        public const string MSG_ErrorRemovingOrders = "Error occurred while Removing order(s)!";
        public const string MSG_ErrorAllocationDeleted = "The order has been deleted from Allocation, and thus this operation cannot be performed";
        public const string MSG_ErrorNoFillsAvailable = "No fills available for the order to allocate";
        public const string MSG_ErrorViewNotPermitted = "View of this allocation is not permitted. If you believe this message is displayed erroneously. Please contact your Account Representative";
        public const string MSG_ErrorOrderGrouped = "This order is grouped and thus allocation can be changed from Allocation Module only";
        public const string MSG_ErrorFecthingAllocationDetails = "Error occurred while fetching allocation details!";
        public const string MSG_ErrorSavingAllocationDetails = "Error occurred while saving allocation details!";
        public const string MSG_ErrorAllocationPercentExceed = "The total allocation(%) across accounts should be 100%";
        public const string MSG_ErrorForSaveLayout = "Either SavedLayoutText or GridName is Blank";
        public const string MSG_ErrorFailedToRenameTab = "Error occurred while completing Rename tab operation!";
        public const string MSG_ErrorFailedToRemoveTab = "Error occurred  while completing Remove tab operation!";
        public const string MSG_To = " to ";
        public const string MSG_MARKET = "Market";
        public const string MSG_WITH_STOP = " with Stop ";
        public const string MSG_IS_REJECTED = " is rejected";
        public const string MSG_IS_ACKNOWLEDGED = " is acknowledged";
        public const string MSG_IS_COMPLETED = " is completed";
        public const string MSG_IS_EXPIRED = " is expired";
        public const string MSG_IS_DONE_FOR_DAY = " is done for day";
        public const string MSG_IS_ROLLOVER = " is rolled over";
        public const string MSG_IS_STOPPED = " is stopped";
        public const string MSG_IS_SUSPENDED = " is suspended";
        public const string MSG_IS_CALCULATED = " is calculated";
        public const string MSG_IS_SENT_FOR_APPROVAL = " is sent for Approval";
        public const string MSG_IS_APPROVED = " is approved";
        public const string MSG_ERROR_EXPECTED_DELIMITER = "Expected delimiter";
        public const string MSG_ERROR_ILLEGAL_CHARS_IN_PATH = "Illegal characters in path";
        public const string MSG_USER_REQUESTED_REJECT = "User requested reject";
        public const string MSG_PRICE_COLON = " Price: ";
        public const string MSG_PRICE = " Price";
        public const string MSG_ORDER_NOT_ACK_BY_BROKER = "This order is not acknowledged by Broker.";
        public const string MSG_CONTACT_SUPPORT_REPRESENTATIVE= "Please contact your support representative.";
        #endregion

        #region Order updates title constants

        public const string TITLE_ORDER_REJECTED = "Order Rejected by ";
        public const string TITLE_ORDER_ACKNOWLEDGED = "Order Acknowledged by ";
        public const string TITLE_ORDER_COMPLETED = "Order Completed";
        public const string TITLE_ORDER_CANCELLED = "Order Cancelled by ";
        public const string TITLE_ORDER_REPLACED = "Replace of order is acknowledged by ";
        public const string TITLE_ORDER_EXPIRED = "Order Expired";
        public const string TITLE_ORDER_DONE_FOR_DAY = "Done for Day";
        public const string TITLE_ORDER_ROLLOVER = "Rollover";
        public const string TITLE_ORDER_STOPPED = "Stopped";
        public const string TITLE_ORDER_SUSPENDED = "Suspended";
        public const string TITLE_ORDER_CALCULATED = "Calculated";
        public const string TITLE_ORDER_SENT_TO_COMPLIANCE_OFFICER = "Order sent to Compliance Officer for Approval";
        public const string TITLE_ORDER_APPROVED_COMPLIANCE_OFFICER = "Order Approved by Compliance Officer";
        public const string TITLE_ORDER_REJECTED_COMPLIANCE_OFFICER = "Order Rejected by Compliance Officer";
        public const string TITLE_STAGE_ORDER_SENT = "Stage Order sent to blotter";
        public const string TITLE_MANUAL_ORDER_SUCESS = "Successfully created Manual Order";
        public const string TITLE_ORDER_SENT_SUCCESSFULLY = "Order Sent successfully";
        #endregion

        #region DateTime

        public const string CONST_DATET_TIME_FORMAT = "MM/dd/yyyy hh:mm:ss.fff tt";
        public const string NirvanaDateFormat_withTime = "M/d/yyyy hh:mm:ss tt";

        #endregion
    }
}
