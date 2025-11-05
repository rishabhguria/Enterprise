CREATE TABLE [dbo].[T_LastCalcDateRevaluation] (
    [ID]                 INT      IDENTITY (1, 1) NOT NULL,
    [LastCalcDate]       DATETIME NOT NULL,
	[LastRevalRunDate]	 DATETIME NULL,
    [FundID]             INT      NULL,
    [UpdatedRevaluation] BIT      DEFAULT ((0)) NULL,
	[LastCalcDateMW] [datetime] NULL,
	[LastCalcDateCashMW] [datetime] NULL,
    CONSTRAINT [PK_T_LastCalcDateRevaluation] PRIMARY KEY CLUSTERED ([ID] ASC)
);

