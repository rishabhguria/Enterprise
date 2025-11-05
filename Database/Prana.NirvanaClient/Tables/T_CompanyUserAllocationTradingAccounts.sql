CREATE TABLE [dbo].[T_CompanyUserAllocationTradingAccounts] (
    [CompanyUserAllocationTradingAccountID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyTradingAccountID]               INT NOT NULL,
    [CompanyUserID]                         INT NOT NULL,
    CONSTRAINT [PK_T_CompanyUserAllocationTradingAccounts] PRIMARY KEY CLUSTERED ([CompanyUserAllocationTradingAccountID] ASC)
);

