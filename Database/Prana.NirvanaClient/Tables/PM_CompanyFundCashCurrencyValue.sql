CREATE TABLE [dbo].[PM_CompanyFundCashCurrencyValue] (
    [CashCurrencyID]  INT        IDENTITY (1, 1) NOT NULL,
    [Date]            DATETIME   NOT NULL,
    [FundID]          INT        NOT NULL,
    [BaseCurrencyID]  INT        NULL,
    [CashValueBase]   FLOAT (53) NULL,
    [LocalCurrencyID] INT        NULL,
    [CashValueLocal]  FLOAT (53) NOT NULL,
    [BalanceType]     INT        DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([CashCurrencyID] ASC),
    CONSTRAINT [FK_PM_CompanyFundCashCurrencyValue_T_CompanyFunds1] FOREIGN KEY ([FundID]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID])
);

