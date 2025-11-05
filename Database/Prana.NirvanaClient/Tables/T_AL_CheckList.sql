CREATE TABLE [dbo].[T_AL_CheckList] (
    [CheckListId]            INT IDENTITY (1, 1) NOT NULL,
    [PresetDefId]            INT NOT NULL,
    [ExchangeOperator]       INT NOT NULL,
    [AssetOperator]          INT NOT NULL,
    [PROperator]             INT NOT NULL,
    [AllocationBase]         INT NOT NULL,
    [MatchingRule]           INT NOT NULL,
    [MatchPortfolioPosition] INT NOT NULL DEFAULT 0,
    [PreferencedFundId]      INT NULL,
    [ProrataDaysBack]        INT NULL,
	[OrderSideOperator]		 INT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_T_AL_CheckList_CheckListId] PRIMARY KEY CLUSTERED ([CheckListId] ASC),
    CONSTRAINT [FK_T_AL_CheckList_AllocationBase] FOREIGN KEY ([AllocationBase]) REFERENCES [dbo].[T_AL_AllocationBase] ([Id]),
	CONSTRAINT [FK_T_AL_CheckList_OrderSideOperator] FOREIGN KEY ([OrderSideOperator]) REFERENCES [dbo].[T_AL_Operator] ([Id]),
    CONSTRAINT [FK_T_AL_CheckList_AssetOperator] FOREIGN KEY ([AssetOperator]) REFERENCES [dbo].[T_AL_Operator] ([Id]),
    CONSTRAINT [FK_T_AL_CheckList_ExchangeOperator] FOREIGN KEY ([ExchangeOperator]) REFERENCES [dbo].[T_AL_Operator] ([Id]),
    CONSTRAINT [FK_T_AL_CheckList_MatchingRule] FOREIGN KEY ([MatchingRule]) REFERENCES [dbo].[T_AL_MatchingRule] ([Id]),
    CONSTRAINT [FK_T_AL_CheckList_PreferencedFundId] FOREIGN KEY ([PreferencedFundId]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID]),
    CONSTRAINT [FK_T_AL_CheckList_PROperator] FOREIGN KEY ([PROperator]) REFERENCES [dbo].[T_AL_Operator] ([Id]),
    CONSTRAINT [FK_T_AL_CheckList_TargetPercentageId] FOREIGN KEY ([PresetDefId]) REFERENCES [dbo].[T_AL_AllocationPreferenceDef] ([Id])
);

