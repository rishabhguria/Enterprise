CREATE TABLE [dbo].[T_SymbolLevelAccrualsSubAccountBalances]
(
	[BalanceID]        BIGINT     IDENTITY (1, 1) NOT NULL,
    [FundID]           INT        NOT NULL,
    [SubAccountID]     INT        NOT NULL,
    [CurrencyID]       INT        NOT NULL,
    [Symbol]           VARCHAR (100),
    [OpenBalDate]      DATETIME   NOT NULL,
    [TransactionDate]  DATETIME   NOT NULL,
    [OpenDrBal]        FLOAT (53) CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_OpenCrBal] DEFAULT ((0)) NOT NULL,
    [OpenCrBal]        FLOAT (53) CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_OpenDrBal] DEFAULT ((0)) NOT NULL,
    [DayDr]            FLOAT (53) CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_DayDr] DEFAULT ((0)) NOT NULL,
    [DayCr]            FLOAT (53) CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_DayCr] DEFAULT ((0)) NOT NULL,
    [CloseDrBal]       FLOAT (53) CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_CloseDrBal] DEFAULT ((0)) NOT NULL,
    [CloseCrBal]       FLOAT (53) CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_CloseCrBal] DEFAULT ((0)) NOT NULL,
    [IsCarryForwarded] INT        CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_IsCarryForwarded] DEFAULT ((0)) NOT NULL,
    [OpenDrBalBase]    FLOAT (53) CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_OpenDrBalBase] DEFAULT ((0)) NOT NULL,
    [OpenCrBalBase]    FLOAT (53) CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_OpenCrBalBase] DEFAULT ((0)) NOT NULL,
    [DayDrBase]        FLOAT (53) CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_DayDrBase] DEFAULT ((0)) NOT NULL,
    [DayCrBase]        FLOAT (53) CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_DayCrBase] DEFAULT ((0)) NOT NULL,
    [CloseDrBalBase]   FLOAT (53) CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_CloseDrBalBase] DEFAULT ((0)) NOT NULL,
    [CloseCrBalBase]   FLOAT (53) CONSTRAINT [DF_T_SymbolLevelAccrualsSubAccountBalances_CloseCrBalBase] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_T_SymbolLevelAccrualsSubAccountBalances] PRIMARY KEY CLUSTERED ([BalanceID] ASC)
	);

    GO
    CREATE NONCLUSTERED INDEX [_ix_SymbolLevelAccrualsSubAccountBalances_FundACCTCURR] ON [dbo].[T_SymbolLevelAccrualsSubAccountBalances]
    (
	[FundID] ASC,
	[SubAccountId] ASC,
	[CurrencyId] ASC,
	[Symbol] ASC,
	[TransactionDate] ASC
    )
    INCLUDE ( 	
	[CloseDrBal],
	[CloseCrBal],
	[CloseDrBalBase],
	[CloseCrBalBase],
	[OpenBalDate],
	[IsCarryForwarded]
	);