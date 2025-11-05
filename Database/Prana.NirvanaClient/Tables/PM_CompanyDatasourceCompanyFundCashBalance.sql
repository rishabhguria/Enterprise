CREATE TABLE [dbo].[PM_CompanyDatasourceCompanyFundCashBalance] (
    [CashBalanceID] INT        IDENTITY (1, 1) NOT NULL,
    [CompanyFundID] INT        NOT NULL,
    [Date]          DATETIME   NOT NULL,
    [CashBalance]   FLOAT (53) NULL,
    CONSTRAINT [PK_PM_CompanyDatasourceCompanyFundCashBalance] PRIMARY KEY CLUSTERED ([CashBalanceID] ASC),
    CONSTRAINT [FK_PM_CompanyDatasourceCompanyFundCashBalance_T_CompanyFunds] FOREIGN KEY ([CompanyFundID]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID])
);

