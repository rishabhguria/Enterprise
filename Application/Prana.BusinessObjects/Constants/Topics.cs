namespace Prana.BusinessObjects.Constants
{
    public class Topics
    {
        public const string Topic_Closing = "Closing";
        public const string Topic_Allocation = "Allocation";
        public const string Topic_CreateGroup = "CreateGroup";
        public const string Topic_Closing_NetPositions = "Closing_NetPositions";
        public const string Topic_CashData = "CashData";
        public const string Topic_CashActivity = "CashActivity";
        public const string Topic_DayEndCash = "DayEndCash";
        public const string Topic_CI = "CollateralInterest";
        public const string Topic_MarkPrice = "MarkPrice";
        public const string Topic_Beta = "Beta";
        public const string Topic_Split = "Split";
        public const string Topic_OutStandings = "OutStandings";
        public const string Topic_ForexRate = "ForexRate";
        public const string Topic_UnwindPositions = "UnwindPositions";
        public const string Topic_ImportAck = "Import_Ack";
        public const string Topic_SecurityMaster = "SecurityMaster";
        public const string Topic_PricingInputsData = "PIData";
        public const string Topic_UDAData = "UDAData";
        public const string Topic_ClosingStatus = "ClosingStatus";
        public const string Topic_ClosingCompleted = "Closing_Completed";
        public const string Topic_RevaluationJournal = "RevaluationJournal";
        public const string Topic_RevaluationActivity = "RevaluationActivity";
        public const string Topic_ActivityType = "ActivityType";
        public const string Topic_ActivityJournalMapping = "ActivityJournalMapping";
        public const string Topic_DailyCreditLimit = "DailyCreditLimit";
        public const string Topic_AllocationPreferenceUpdated = "AllocationPreferenceUpdated";
        public const string Topic_ManualJournalActivity = "ManualJournalActivity";
        public const string Topic_ReconPreferenceUpdated = "ReconPreferenceUpdated";
        public const string Topic_HistoricalJournalActivity = "PreExistingJournalActivity";
        public const string Topic_OMIData = "OMIData";
        public const string Topic_RevaluationProgress = "RevaluationProgressPercentage";
        public const string Topic_SubAccounts = "SubAccount";
        public const string Topic_RiskSnapshotData = "RiskSnapshotData";
        public const string Topic_RiskStepAnalysisData = "RiskStepAnalysisData";
        public const string Topic_DailyVolatility = "DailyVolatility";
        public const string Topic_DailyVWAP = "DailyVVWAP";
        public const string Topic_DailyCollateral = "DailyCollateral";
        public const string Topic_DailyDividendYield = "DailyDividendYield";
        public const string Topic_PerformanceNumber = "PerformanceNumbers";
        public const string Topic_UpdateInTrade = "UpdateInTrade";
        public const string Topic_StartDayOfAccrual = "StartDayOfAccrual";
        public const string Topic_StageOrderRemovalFromBlotter = "StageOrderRemovalFromBlotter";
        public const string Topic_SubOrderRemovalFromBlotter = "SubOrderRemovalFromBlotter";
        public const string Topic_UpdateBlotterStatusBarMessage = "UpdateBlotterStatusBarMessage";

        public const string Topic_TradeServiceLogsData = "TradeServiceLogsData";
        public const string Topic_TradeServiceConnectedUserData = "TradeServiceConnectedUserData";
        public const string Topic_TradeServicePricingConnectionData = "TradeServicePricingConnectionData";
        public const string Topic_TradeServiceFixConnectionData = "TradeServiceFixConnectionData";
        public const string Topic_TradeServiceFixAutoConnectionStatus = "TradeServiceFixAutoConnectionStatus";

        public const string Topic_PricingService2LogsData = "PricingService2LogsData";
        public const string Topic_PricingService2ConnectedUserData = "PricingService2ConnectedUserData";
        public const string Topic_PricingService2LiveFeedConnectionData = "PricingService2LiveFeedConnectionData";

        public const string Topic_ExpnlServiceLogsData = "ExpnlServiceLogsData";
        public const string Topic_ExpnlServiceConnectedUserData = "ExpnlServiceConnectedUserData";
        public const string Topic_ExpnlServicePricingConnectionData = "ExpnlServicePricingConnectionData";
        public const string Topic_ExpnlServiceTradeConnectionData = "ExpnlServiceTradeConnectionData";
        public const string Topic_ExpnlServiceLiveFeedConnectionData = "ExpnlServiceLiveFeedConnectionData";
        public const string Topic_ExpnlServiceCompressionData = "ExpnlServiceCompressionData";

        public const string Topic_DMSymbolDataResponse = "DMSymbolDataResponse";
        public const string Topic_DMSymbolLimitResponse = "DMSymbolLimitResponse";
        public const string Topic_DMSymbolLimitServicesResponse = "DMSymbolLimitServicesResponse";
        public const string Topic_DMServicesResponse = "DMServicesResponse";
        public const string Topic_DMLiveFeedData = "DMLiveFeedData";

        public const string Topic_TradingTicketServiceLogsData = "TradingTicketServiceLogsData";
        public const string Topic_UpdateTradeAttributePref = "UpdateTradeAttributePref";

        /// <summary>
        /// Topic for refreshing allocation scheme after save
        /// </summary>
        public const string Topic_AllocationSchemeUpdated = "AllocationScheme";
        //public const string Topic_NewBrokerAdded = "NewBrokerAdded";
        /// <summary>
        /// Topic for updating  Revaluation date
        /// </summary>
        public const string Topic_RevaluationDate = "RevaluationDate";
        public const string Topic_ClosingCorrupted = "Closing_Corrupted";
        public const string Topic_Rebalancer_ModelPortfolio = "Rebalancer_ModelPortfolio";
        public const string Topic_Rebalancer_CustomGroup = "Rebalancer_CustomGroup";

        public const string Topic_UpdatesForWebBlotter = "UpdatesForWebBlotter";
        public const string Topic_PendingOrderUpdatesForWeb = "PendingOrderUpdatesForWeb";
        public const string Topic_OverrideOrderUpdatesForWeb = "OverrideOrderUpdatesForWeb";

        public const string Topic_RefreshBlotterAfterImport = "RefreshBlotterAfterImport";
        public const string Topic_ImportStarted = "ImportStarted";
        public const string Topic_ThirdPartyStatusMessage = "ThirdPartyStatusMessage";
        public const string Topic_ThirdPartyMessage = "ThirdPartyMessage";
        public const string Topic_ThirdPartyDuplicateFileConfirmation = "ThirdPartyDuplicateFileConfirmation";
        public const string Topic_ThirdPartyAllocationMatchStatusUpdate = "ThirdPartyAllocationMatchStatusUpdate";
        public const string Topic_ThirdPartyMismatchFileConfirmation = "ThirdPartyMismatchFileConfirmation";
        public const string Topic_ThirdPartyAutomatedBatchStatus = "ThirdPartyAutomatedBatchStatus";
        public const string Topic_CreateGroupForWeb = "CreateGroupForWeb";
    }
}
