CREATE TABLE [dbo].[T_cashpreferences_Backup] (
    [ID]                       INT          NOT NULL,
    [CashMgmtStartDate]        DATETIME     NOT NULL,
    [MarginPercentage]         VARCHAR (50) NOT NULL,
    [IsCalculatePnL]           BIT          NOT NULL,
    [IsCalulateDividend]       BIT          NOT NULL,
    [IsCalculateBondAccural]   BIT          NOT NULL,
    [IsPublishRevaluationData] BIT          NOT NULL,
    [FundID]                   INT          NOT NULL
);

