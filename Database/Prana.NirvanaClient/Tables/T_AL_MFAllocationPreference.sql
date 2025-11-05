CREATE TABLE [dbo].[T_AL_MFAllocationPreference]
(
	[MFPreferenceId]			INT				NOT NULL IDENTITY (50000, 1), 
    [MFPreferenceName]			NVARCHAR(200)	NOT NULL, 
    [CompanyId]					INT				NOT NULL, 
    [UpdateDateTime]			DATETIME		NOT NULL, 
    [AllocationBase]			INT				NOT NULL DEFAULT 1, 
    [MatchingRule]				INT				NOT NULL DEFAULT 1, 
    [MatchPortfolioPosition]	INT				NOT NULL DEFAULT 0, 
    [PreferencedFundId]			INT				NULL, 
    [ProrataDaysBack]			INT				NULL,
    CONSTRAINT [PK_T_AL_MFAllocationPreference_Id] PRIMARY KEY CLUSTERED ([MFPreferenceId] ASC),
    CONSTRAINT [FK_T_AL_MFAllocationPreference_AllocationBase] FOREIGN KEY ([AllocationBase]) REFERENCES [dbo].[T_AL_AllocationBase] ([Id]),
    CONSTRAINT [FK_T_AL_MFAllocationPreference_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[T_Company] ([CompanyID]),
    CONSTRAINT [FK_T_AL_MFAllocationPreference_MatchingRule] FOREIGN KEY ([MatchingRule]) REFERENCES [dbo].[T_AL_MatchingRule] ([Id]),
    CONSTRAINT [AK_T_AL_MFAllocationPreference_Name_CompanyId] UNIQUE NONCLUSTERED ([MFPreferenceName] ASC, [CompanyId] ASC)
)
