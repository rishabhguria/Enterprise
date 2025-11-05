CREATE TABLE [dbo].[T_TradingTransactionType] (
    [TransactionTypeAcronym] VARCHAR (50)  NOT NULL,
    [TransactionTypeName]    VARCHAR (200) NULL,
    CONSTRAINT [PK_T_TradingTransactionType] PRIMARY KEY CLUSTERED ([TransactionTypeAcronym] ASC)
);

