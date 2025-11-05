CREATE TABLE [dbo].[T_BTOperators] (
    [OperatorID]   INT          IDENTITY (1, 1) NOT NULL,
    [Symbol]       VARCHAR (5)  NULL,
    [OperatorName] VARCHAR (50) NULL,
    CONSTRAINT [PK_T_BTOperators] PRIMARY KEY CLUSTERED ([OperatorID] ASC)
);

