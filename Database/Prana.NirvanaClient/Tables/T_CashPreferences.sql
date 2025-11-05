CREATE TABLE [dbo].[T_CashPreferences] (
    [ID]                             INT          NOT NULL,
    [CashMgmtStartDate]              DATETIME     NOT NULL,
    [MarginPercentage]               VARCHAR (50) NOT NULL,
	[CollateralFrequencyInterest]	 VARCHAR (50) DEFAULT (('-1')) NOT NULL,
    [IsCalculatePnL]                 BIT          NOT NULL,
    [IsCalulateDividend]             BIT          NOT NULL,
    [IsCalculateBondAccural]         BIT          NOT NULL,
	[IsCalculateCollateral]			 BIT		  DEFAULT ((0)) NOT NULL, 
	[IsBreakRealizedPnlSubaccount]	 BIT		  DEFAULT ((0)) NOT NULL,
	[IsBreakTotalIntoTradingAndFxPnl] BIT		  DEFAULT ((0)) NOT NULL,
    [IsPublishRevaluationData]       BIT          DEFAULT ((0)) NOT NULL,
    [FundID]                         INT          DEFAULT ((0)) NOT NULL,
    [IsCashSettlementEntriesVisible] BIT          DEFAULT ((1)) NOT NULL,
	[IsAccruedTillSettlement]        BIT          DEFAULT ((1)) NOT NULL,
	[SymbolWiseRevaluationDate]		 DATETIME,
    [IsCreateManualJournals]         BIT          DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_T_CashPreferences] PRIMARY KEY CLUSTERED ([FundID] ASC)
);

