CREATE TABLE [dbo].[PM_DailyCreditLimit] (
    [DailyCreditLimitId] INT        IDENTITY (1, 1) NOT NULL,
    [FundID]             INT        NOT NULL,
    [LongDebitLimit]     FLOAT (53) NOT NULL,
    [ShortCreditLimit]   FLOAT (53) NOT NULL,
    [LongDebitBalance]   FLOAT (53) NOT NULL,
    [ShortCreditBalance] FLOAT (53) NOT NULL,
    CONSTRAINT [FK_PM_DailyCreditLimit_FundID] FOREIGN KEY ([FundID]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID])
);

