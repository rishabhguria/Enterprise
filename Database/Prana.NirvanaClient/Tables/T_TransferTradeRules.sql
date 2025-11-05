CREATE TABLE [dbo].[T_TransferTradeRules] (
    [IsTIFChange]                               BIT NOT NULL,
    [IsTradingAccChange]                        BIT NOT NULL,
    [IsFundChange]                              BIT NOT NULL,
    [IsStrategyChange]                          BIT NOT NULL,
    [IsHandlingInstrChange]                     BIT NOT NULL,
    [IsVenueCPChange]                           BIT NOT NULL,
    [IsAllowAllUserToCancelReplaceRemove]             BIT NOT NULL,
    [IsAllowAllUserToChangeOrderType]           BIT NOT NULL,
    [IsExecutionInstrChange]                    BIT NOT NULL,
    [CompanyId]                                 INT NOT NULL,
    [IsAllowAllUserToTransferTrade]             BIT DEFAULT ('0') NOT NULL,
    [IsAllowAllUserToGenerateSub]               BIT DEFAULT ('0') NOT NULL,
    [IsApplyLimitRulesForReplacingStagedOrders] BIT DEFAULT ('0') NOT NULL,
    [IsApplyLimitRulesForReplacingOtherOrders]  BIT DEFAULT ('1') NOT NULL,
    [IsApplyLimitRulesForReplacingSubOrders]    BIT DEFAULT ('0') NOT NULL,
    [IsAllowRestrictedSecuritiesList]			BIT DEFAULT ('0') NOT NULL,
    [IsAllowAllowedSecuritiesList]				BIT DEFAULT ('0') NOT NULL, 
    [MasterUsersIDs]							VARCHAR(MAX) NULL, 
	[IsDefaultOrderTypeLimitForMultiDay]  BIT DEFAULT ('1') NOT NULL,
    CONSTRAINT [PK_T_TransferTradeRules] PRIMARY KEY CLUSTERED ([CompanyId] ASC)
);

