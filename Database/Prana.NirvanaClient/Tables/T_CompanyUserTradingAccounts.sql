CREATE TABLE [dbo].[T_CompanyUserTradingAccounts] (
    [CompanyUserTradingAccountID] INT IDENTITY (1, 1) NOT NULL,
    [TradingAccountID]            INT NOT NULL,
    [CompanyUserID]               INT NOT NULL,
    CONSTRAINT [PK_T_CompanyUserTradingAccounts] PRIMARY KEY CLUSTERED ([CompanyUserTradingAccountID] ASC),
    CONSTRAINT [FK_T_CompanyUserTradingAccounts_T_CompanyUser] FOREIGN KEY ([CompanyUserID]) REFERENCES [dbo].[T_CompanyUser] ([UserID])
);

