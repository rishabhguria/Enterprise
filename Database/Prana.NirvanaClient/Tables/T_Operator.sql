CREATE TABLE [dbo].[T_Operator] (
    [OperatorID] INT         IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (4) NOT NULL,
    CONSTRAINT [PK_T_Operator] PRIMARY KEY CLUSTERED ([OperatorID] ASC)
);

