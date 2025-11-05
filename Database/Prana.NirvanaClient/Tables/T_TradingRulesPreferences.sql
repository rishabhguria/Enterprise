CREATE TABLE [dbo].[T_TradingRulesPreferences]
(
	[CompanyID] INT NOT NULL  , 
    [IsOversellTradingRule] BIT NULL DEFAULT 0, 
    [IsOverbuyTradingRule] BIT NULL DEFAULT 0, 
    [IsUnallocatedTradeAlert] BIT NULL DEFAULT 0, 
    [IsFatFingerTradingRule] BIT NULL DEFAULT 0, 
    [IsDuplicateTradeAlert] BIT NULL DEFAULT 0, 
    [IsPendingNewTradeAlert] BIT NULL DEFAULT 0, 
    [DefineFatFingerValue] FLOAT NULL DEFAULT 0, 
    [DuplicateTradeAlertTime] INT NULL DEFAULT 0, 
    [PendingNewOrderAlertTime] INT NULL DEFAULT 0, 
    [FatFingerAccountOrMasterFund] INT NULL DEFAULT 0,
	[IsAbsoluteAmountOrDefinePercent] INT NULL DEFAULT 0, 
    [IsInMarketIncluded] BIT NULL DEFAULT 0,
    [IsSharesOutstandingRule] BIT NULL DEFAULT 0, 
    [SharesOutstandingAccountOrMF] INT NULL DEFAULT 0,
    [SharesOutstandingPercent] FLOAT NULL DEFAULT 0
	FOREIGN KEY (CompanyID) REFERENCES T_Company(CompanyID)
)
