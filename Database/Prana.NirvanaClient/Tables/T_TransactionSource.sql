CREATE TABLE [dbo].[T_TransactionSource] (
    [TransactionSourceAcronym] VARCHAR (50)  NOT NULL,
    [TransactionSourceName]    VARCHAR (200) NULL,
    CONSTRAINT [PK_T_TransactionType] PRIMARY KEY CLUSTERED ([TransactionSourceAcronym] ASC)
);

