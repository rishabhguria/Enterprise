CREATE TABLE [dbo].[T_BTSharedTradingAccounts] (
    [RelationID] INT          IDENTITY (1, 1) NOT NULL,
    [BasketID]   VARCHAR (50) NOT NULL,
    [AccountID]  INT          NOT NULL,
    CONSTRAINT [PK_T_BTSharedTradingAccounts] PRIMARY KEY CLUSTERED ([RelationID] ASC)
);

