CREATE TABLE [dbo].[T_SubAccountBalances_Backup] (
    [BalanceID]        BIGINT   IDENTITY (1, 1) NOT NULL,
    [FundID]           INT      NOT NULL,
    [SubAccountID]     INT      NOT NULL,
    [CurrencyID]       INT      NOT NULL,
    [OpenBalDate]      DATETIME NOT NULL,
    [TransactionDate]  DATETIME NOT NULL,
    [OpenDrBal]        MONEY    NOT NULL,
    [OpenCrBal]        MONEY    NOT NULL,
    [DayDr]            MONEY    NOT NULL,
    [DayCr]            MONEY    NOT NULL,
    [CloseDrBal]       MONEY    NOT NULL,
    [CloseCrBal]       MONEY    NOT NULL,
    [IsCarryForwarded] INT      NOT NULL,
    [OpenDrBalBase]    MONEY    NOT NULL,
    [OpenCrBalBase]    MONEY    NOT NULL,
    [DayDrBase]        MONEY    NOT NULL,
    [DayCrBase]        MONEY    NOT NULL,
    [CloseDrBalBase]   MONEY    NOT NULL,
    [CloseCrBalBase]   MONEY    NOT NULL
);

