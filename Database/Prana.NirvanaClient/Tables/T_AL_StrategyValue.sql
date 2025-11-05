CREATE TABLE [dbo].[T_AL_StrategyValue] (
    [Id]                   INT              IDENTITY (1, 1) NOT NULL,
    [AllocationPrefDataId] INT              NOT NULL,
    [StrategyId]           INT              NULL,
    [Value]                DECIMAL (32, 19) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_T_AL_StrategyValue_AllocationPrefDataId] FOREIGN KEY ([AllocationPrefDataId]) REFERENCES [dbo].[T_AL_AllocationPreferenceData] ([Id]),
    CONSTRAINT [FK_T_AL_StrategyValue_StrategyId] FOREIGN KEY ([StrategyId]) REFERENCES [dbo].[T_CompanyStrategy] ([CompanyStrategyID]),
    CONSTRAINT [AK_T_AL_StrategyValue_[AllocationPrefDataId_StrategyId] UNIQUE NONCLUSTERED ([AllocationPrefDataId] ASC, [StrategyId] ASC)
);

