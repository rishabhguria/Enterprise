CREATE TABLE [dbo].[PM_CashReconciledData] (
    [CashReconciledDataID]      INT        NOT NULL,
    [CashReconID]               INT        NOT NULL,
    [ThirdPartyID]              INT        NOT NULL,
    [CurrencyID]                INT        NOT NULL,
    [CompanyFundID]             INT        NOT NULL,
    [ClosingBalanceApplication] FLOAT (53) NOT NULL,
    [ClosingBalanceDataSource]  FLOAT (53) NOT NULL,
    [ClosingBalancemanualEntry] FLOAT (53) NULL,
    [Status]                    INT        CONSTRAINT [DF_PM_CashReconciledData_Status] DEFAULT ((0)) NOT NULL,
    [AcceptedFrom]              INT        NOT NULL,
    CONSTRAINT [PK_PM_CashReconciledData_1] PRIMARY KEY CLUSTERED ([CashReconciledDataID] ASC),
    CONSTRAINT [FK_PM_CashReconciledData_PM_CompanyCashRecon] FOREIGN KEY ([CashReconID]) REFERENCES [dbo].[PM_CompanyCashRecon] ([CashReconID]),
    CONSTRAINT [FK_PM_CashReconciledData_T_CompanyFunds] FOREIGN KEY ([CompanyFundID]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID]),
    CONSTRAINT [FK_PM_CashReconciledData_T_Currency] FOREIGN KEY ([CurrencyID]) REFERENCES [dbo].[T_Currency] ([CurrencyID]),
    CONSTRAINT [FK_PM_CashReconciledData_T_ThirdParty] FOREIGN KEY ([ThirdPartyID]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID])
);

