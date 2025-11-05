CREATE TABLE [dbo].[T_TransactionType] (
    [TransactionTypeId]      INT          NOT NULL,
    [TransactionType]        VARCHAR (50) NOT NULL,
    [TransactionTypeAcronym] VARCHAR (50) NULL, 
    CONSTRAINT [PK_T_TransactionTypeId] PRIMARY KEY ([TransactionTypeId])
);

