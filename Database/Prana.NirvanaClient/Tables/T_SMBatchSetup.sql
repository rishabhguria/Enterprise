CREATE TABLE [dbo].[T_SMBatchSetup] (
    [SMBatchID]              INT           IDENTITY (1, 1) NOT NULL,
    [SystemLevelName]        VARCHAR (500) NOT NULL,
    [CronExpression]         VARCHAR (200) NOT NULL,
    [Fields]                 VARCHAR (MAX) NOT NULL,
    [RunTimeTypeID]          INT           DEFAULT ((2)) NOT NULL,
    [IsHistoricDataRequired] BIT           DEFAULT ((0)) NOT NULL,
    [FundID]                 VARCHAR (MAX) NULL,
    [Indices]                VARCHAR (MAX) NULL,
    [UserDefinedName]        VARCHAR (100) DEFAULT ('') NOT NULL,
    [DaysOfHistoricData]     INT           NULL,
    [FilterClause]           VARCHAR (MAX) NULL,
    [BatchType]              INT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_T_SMBatchSetup] PRIMARY KEY CLUSTERED ([SMBatchID] ASC),
    CONSTRAINT [IX_T_SMBatchSetup] UNIQUE NONCLUSTERED ([SystemLevelName] ASC)
);

