CREATE TABLE [dbo].[T_CompanyClientTradingAccount] (
    [CompanyClientTradingAccountID] INT          IDENTITY (1, 1) NOT NULL,
    [CompanyClientTradingAccount]   VARCHAR (50) NULL,
    [CompanyTradingAccountID]       INT          NOT NULL,
    [CompanyClientID]               INT          NOT NULL,
    [ClientTraderID]                INT          NULL,
    CONSTRAINT [PK_T_CompanyClientTradingAccount] PRIMARY KEY CLUSTERED ([CompanyClientTradingAccountID] ASC),
    CONSTRAINT [FK_T_CompanyClientTradingAccount_T_CompanyClient] FOREIGN KEY ([CompanyClientID]) REFERENCES [dbo].[T_CompanyClient] ([CompanyClientID]),
    CONSTRAINT [FK_T_CompanyClientTradingAccount_T_CompanyClientTrader] FOREIGN KEY ([ClientTraderID]) REFERENCES [dbo].[T_CompanyClientTrader] ([TraderID]),
    CONSTRAINT [FK_T_CompanyClientTradingAccount_T_CompanyTradingAccounts] FOREIGN KEY ([CompanyTradingAccountID]) REFERENCES [dbo].[T_CompanyTradingAccounts] ([CompanyTradingAccountsID])
);

