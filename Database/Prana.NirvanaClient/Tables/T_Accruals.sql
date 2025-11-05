CREATE TABLE [dbo].[T_Accruals] (
    [AccrualID]    BIGINT     IDENTITY (1, 1) NOT NULL,
    [FundID]       INT        NOT NULL,
    [SubAccountID] INT        NULL,
    [CashValue]    FLOAT (53) CONSTRAINT [DF_T_Accurals_CashValue] DEFAULT ((0)) NOT NULL,
    [Date]         DATETIME   NOT NULL,
    [CurrencyID]   INT        NOT NULL,
    CONSTRAINT [PK_T_Accurals] PRIMARY KEY CLUSTERED ([AccrualID] ASC)
);

