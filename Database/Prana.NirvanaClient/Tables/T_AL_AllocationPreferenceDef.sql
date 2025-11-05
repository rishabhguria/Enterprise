CREATE TABLE [dbo].[T_AL_AllocationPreferenceDef] (
    [Id]                     INT            IDENTITY (10000, 1) NOT NULL,
    [Name]                   NVARCHAR (200) NOT NULL,
    [CompanyId]              INT            NOT NULL,
    [AllocationBase]         INT            NOT NULL,
    [MatchingRule]           INT            NOT NULL,
    [MatchPortfolioPosition] INT		    NOT NULL DEFAULT 0,
    [PreferencedFundId]      INT            NULL,
    [UpdateDateTime]         DATETIME       NOT NULL,
    [ProrataDaysBack]        INT            NULL,
    [PositionPriority]       INT            NULL,
    [IsPrefVisible]			 BIT            NOT NULL DEFAULT 1,
	[RebalancerFileName]     NVARCHAR (200) NULL,
    CONSTRAINT [PK_T_AL_AllocationPreferenceDef_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_T_AL_AllocationPreferenceDef_AllocationBase] FOREIGN KEY ([AllocationBase]) REFERENCES [dbo].[T_AL_AllocationBase] ([Id]),
    CONSTRAINT [FK_T_AL_AllocationPreferenceDef_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[T_Company] ([CompanyID]),
    CONSTRAINT [FK_T_AL_AllocationPreferenceDef_MatchingRule] FOREIGN KEY ([MatchingRule]) REFERENCES [dbo].[T_AL_MatchingRule] ([Id]),
    CONSTRAINT [FK_T_AL_AllocationPreferenceDef_PreferencedFundId] FOREIGN KEY ([PreferencedFundId]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID]),
    CONSTRAINT [AK_T_AL_AllocationPreferenceDef_Name_CompanyId] UNIQUE NONCLUSTERED ([Name] ASC, [CompanyId] ASC)
);

