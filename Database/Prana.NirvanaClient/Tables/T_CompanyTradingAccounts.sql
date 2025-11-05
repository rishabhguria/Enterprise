CREATE TABLE [dbo].[T_CompanyTradingAccounts] (
    [CompanyTradingAccountsID] INT          IDENTITY (1, 1) NOT NULL,
    [TradingAccountName]       VARCHAR (50) NOT NULL,
    [TradingShortName]         VARCHAR (50) NOT NULL,
    [CompanyID]                INT          NOT NULL,
    [IsActive]                 BIT          DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_T_CompanyTradingAccounts] PRIMARY KEY CLUSTERED ([CompanyTradingAccountsID] ASC),
    CONSTRAINT [FK_T_CompanyTradingAccounts_T_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[T_Company] ([CompanyID])
);

