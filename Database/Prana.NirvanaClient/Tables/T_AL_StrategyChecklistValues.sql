CREATE TABLE [dbo].[T_AL_StrategyChecklistValues]
(
	[Id]                   INT              IDENTITY (1, 1) NOT NULL,
	[AccountCheckListId]			   INT              NOT NULL,
    [StrategyId]           INT              NOT NULL,
    [Value]                DECIMAL (32, 19) NOT NULL,
	CONSTRAINT [PK_T_AL_StrategyChecklistValues_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_T_AL_StrategyChecklistValues_AccountId] FOREIGN KEY ([AccountCheckListId]) REFERENCES [dbo].[T_AL_AccountCheckListValue] ([Id]),
	CONSTRAINT [FK_T_AL_StrategyChecklistValues_StrategyId] FOREIGN KEY ([StrategyId]) REFERENCES [dbo].[T_CompanyStrategy] ([CompanyStrategyID]),
	CONSTRAINT [AK_T_AL_StrategyChecklistValues_[AccountId_StrategyId] UNIQUE NONCLUSTERED ([AccountCheckListId] ASC, [StrategyId] ASC)
);
