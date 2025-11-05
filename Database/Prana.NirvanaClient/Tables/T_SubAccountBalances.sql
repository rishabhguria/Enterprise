CREATE TABLE [dbo].[T_SubAccountBalances] (
    [BalanceID]        BIGINT     IDENTITY (1, 1) NOT NULL,
    [FundID]           INT        NOT NULL,
    [SubAccountID]     INT        NOT NULL,
    [CurrencyID]       INT        NOT NULL,
    [OpenBalDate]      DATETIME   NOT NULL,
    [TransactionDate]  DATETIME   NOT NULL,
    [OpenDrBal]        FLOAT (53) CONSTRAINT [DF_Table_1_OpenCrBal] DEFAULT ((0)) NOT NULL,
    [OpenCrBal]        FLOAT (53) CONSTRAINT [DF_Table_1_OpenDrBal] DEFAULT ((0)) NOT NULL,
    [DayDr]            FLOAT (53) CONSTRAINT [DF_T_SubAccountBalances_DayDr] DEFAULT ((0)) NOT NULL,
    [DayCr]            FLOAT (53) CONSTRAINT [DF_T_SubAccountBalances_DayCr] DEFAULT ((0)) NOT NULL,
    [CloseDrBal]       FLOAT (53) CONSTRAINT [DF_T_SubAccountBalances_CloseDrBal] DEFAULT ((0)) NOT NULL,
    [CloseCrBal]       FLOAT (53) CONSTRAINT [DF_T_SubAccountBalances_CloseCrBal] DEFAULT ((0)) NOT NULL,
    [IsCarryForwarded] INT        CONSTRAINT [DF_T_SubAccountBalances_IsCarryForwarded] DEFAULT ((0)) NOT NULL,
    [OpenDrBalBase]    FLOAT (53) CONSTRAINT [DF_Table_1_OpenDrBalBase] DEFAULT ((0)) NOT NULL,
    [OpenCrBalBase]    FLOAT (53) CONSTRAINT [DF_Table_1_OpenCrBalBase] DEFAULT ((0)) NOT NULL,
    [DayDrBase]        FLOAT (53) CONSTRAINT [DF_Table_1_DayDrBase] DEFAULT ((0)) NOT NULL,
    [DayCrBase]        FLOAT (53) CONSTRAINT [DF_Table_1_DayCrBase] DEFAULT ((0)) NOT NULL,
    [CloseDrBalBase]   FLOAT (53) CONSTRAINT [DF_Table_1_CloseDrBalBase] DEFAULT ((0)) NOT NULL,
    [CloseCrBalBase]   FLOAT (53) CONSTRAINT [DF_Table_1_CloseCrBalBase] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_T_SubAccountBalances] PRIMARY KEY CLUSTERED ([BalanceID] ASC)
);

GO
CREATE NONCLUSTERED INDEX [_ix_SubAccountBalances_FundACCTCURR] ON [dbo].[T_SubAccountBalances]
(
	[FundID] ASC,
	[SubAccountId] ASC,
	[CurrencyId] ASC,
	[TransactionDate] ASC
)
INCLUDE (
	[CloseDrBal],
	[CloseCrBal],
	[CloseDrBalBase],
	[CloseCrBalBase],
	[OpenBalDate],
	[IsCarryForwarded]);
