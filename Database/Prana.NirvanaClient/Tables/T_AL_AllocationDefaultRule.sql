CREATE TABLE [dbo].[T_AL_AllocationDefaultRule] (
    [CompanyId]							INT				NOT NULL,
    [AllocationBase]					INT				NOT NULL,
    [MatchingRule]						INT				NOT NULL,
    [MatchPortfolioPosition]			INT				DEFAULT 0 NOT NULL,
    [PreferencedFundId]					INT				NULL,
    [DoCheckSide]						BIT				NULL,
    [ProrataFundList]					NVARCHAR (MAX)	NULL,
    [ProrataDaysBack]					INT				NULL,
    [AllowEditPreferences]				BIT				DEFAULT ((1)) NULL,
    [CheckSidePreference]		        NVARCHAR (MAX)	NULL,
    [PrecisionDigit]					INT				NULL,
    [AssetsWithCommissionInNetAmount]	NVARCHAR(MAX)	NULL, 
	[MsgOnBrokerChange]					BIT				DEFAULT ((1)) NOT NULL, 
	[RecalculateOnBrokerChange]			BIT				DEFAULT ((1)) NOT NULL, 
	[MsgOnAllocation]					BIT				DEFAULT ((1)) NOT NULL, 
	[RecalculateOnAllocation]			BIT				DEFAULT ((1)) NOT NULL,
	[EnableMasterFundAllocation]        BIT			    DEFAULT 0 NULL,
	[IsOneSymbolOneMasterFundAllocation]BIT			    DEFAULT 0 NULL,
	[ProrataSchemeName]					NVARCHAR(MAX)   DEFAULT '' NOT NULL,
	[AllocationSchemeKey]               INT				DEFAULT 1 NOT NULL,
	[SetSchemeFromUI]					BIT				default 0 NOT NULL
    CONSTRAINT [PK_T_AL_AllocationDefaultRule_Id] PRIMARY KEY CLUSTERED ([CompanyId] ASC),
    CONSTRAINT [FK_T_AL_AllocationDefaultRule_AllocationBase] FOREIGN KEY ([AllocationBase]) REFERENCES [dbo].[T_AL_AllocationBase] ([Id]),
    CONSTRAINT [FK_T_AL_AllocationDefaultRule_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[T_Company] ([CompanyID]),
    CONSTRAINT [FK_T_AL_AllocationDefaultRule_MatchingRule] FOREIGN KEY ([MatchingRule]) REFERENCES [dbo].[T_AL_MatchingRule] ([Id]),
    CONSTRAINT [FK_T_AL_AllocationDefaultRule_PreferencedFundId] FOREIGN KEY ([PreferencedFundId]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID])
);

