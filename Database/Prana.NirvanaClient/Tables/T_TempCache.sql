CREATE TABLE [dbo].[T_TempCache] (
    [Key]           VARCHAR (25)  NOT NULL,
    [OrderSides]    VARCHAR (100) NULL,
    [OrderTypes]    VARCHAR (100) NULL,
    [HandlingInst]  VARCHAR (100) NULL,
    [ExecutionInst] VARCHAR (100) NULL,
    [TIFs]          VARCHAR (100) NULL,
    CONSTRAINT [PK_T_TempCache] PRIMARY KEY CLUSTERED ([Key] ASC)
);

