CREATE TABLE [dbo].[T_CompanyMasterFundTradingAccountAssociation] (
    [CompanyMaster-TradingAccountID]	INT IDENTITY (1, 1) NOT NULL,
    [CompanyMasterFundID]				INT NOT NULL,
    [CompanyTradingAccountID]			INT NOT NULL,
    CONSTRAINT [PK_T_CompanyMasterFundTradingAccountAssociation] PRIMARY KEY CLUSTERED ([CompanyMaster-TradingAccountID] ASC),
    CONSTRAINT [FK_T_CompanyMasterFundTradingAccountAssociation_T_CompanyTradingAccounts] FOREIGN KEY ([CompanyTradingAccountID]) REFERENCES [dbo].[T_CompanyTradingAccounts] ([CompanyTradingAccountsID]),
    CONSTRAINT [FK_T_CompanyMasterFundTradingAccountAssociation_T_CompanyMasterFund] FOREIGN KEY ([CompanyMasterFundID]) REFERENCES [dbo].[T_CompanyMasterFunds] ([CompanyMasterFundID])
);

