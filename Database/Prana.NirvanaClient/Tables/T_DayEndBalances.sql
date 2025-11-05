CREATE TABLE [dbo].[T_DayEndBalances] (
    [CashCurrencyID]  INT        IDENTITY (1, 1) NOT NULL,
    [Date]            DATETIME   NOT NULL,
    [FundID]          INT        NOT NULL,
    [BaseCurrencyID]  INT        NULL,
    [CashValueBase]   FLOAT (53) NULL,
    [LocalCurrencyID] INT        NULL,
    [CashValueLocal]  FLOAT (53) NOT NULL,
    [BalanceType]     INT        DEFAULT ((1)) NULL,
    CONSTRAINT [PK__PM_CompanyFundCa__39251148_1] PRIMARY KEY CLUSTERED ([CashCurrencyID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK_PM_CompanyFundCashCurrencyValue_T_CompanyFunds] FOREIGN KEY ([FundID]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID])
);

