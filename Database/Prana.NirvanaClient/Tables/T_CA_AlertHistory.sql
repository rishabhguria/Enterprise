CREATE TABLE [dbo].[T_CA_AlertHistory] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [UserId]           VARCHAR (100) NULL,
    [RuleType]         VARCHAR (100) NULL,
    [Summary]          VARCHAR (MAX) NULL,
    [CompressionLevel] VARCHAR (100) NULL,
    [Parameters]       VARCHAR (MAX) NULL,
    [OrderId]          VARCHAR (500)  NULL,
    [Status]           VARCHAR (100) NULL,
    [ValidationTime]   DATETIME      NULL,
    [RuleId]           VARCHAR (50)  NULL,
    [Description]      VARCHAR (MAX) NULL,
    [Dimension]        VARCHAR (50)  NULL,
    [PreTradeType]     VARCHAR (50)  NULL,
    [ActionUser] VARCHAR(50) NULL, 
    [PreTradeActionType] VARCHAR(50) NULL,
	[ComplianceOfficerNotes] VARCHAR (MAX) NULL,
    [UserNotes] VARCHAR (MAX) NULL,
    [TradeDetails] VARCHAR (MAX) NULL,
    [AlertPopUpResponse] INT NULL,
    CONSTRAINT [PK_T_CA_AlertHistory] PRIMARY KEY CLUSTERED ([ID] ASC)
);

