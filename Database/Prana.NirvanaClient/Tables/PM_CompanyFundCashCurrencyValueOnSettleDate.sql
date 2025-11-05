CREATE TABLE [dbo].[PM_CompanyFundCashCurrencyValueOnSettleDate] (
    [CashCurrencyID]  INT        IDENTITY (1, 1) NOT NULL,
    [SettlementDate]  DATETIME   NOT NULL,
    [FundID]          INT        NOT NULL,
    [BaseCurrencyID]  INT        NULL,
    [CashValueBase]   FLOAT (53) NULL,
    [LocalCurrencyID] INT        NULL,
    [CashValueLocal]  FLOAT (53) NOT NULL,
    PRIMARY KEY CLUSTERED ([CashCurrencyID] ASC) WITH (FILLFACTOR = 100)
);

