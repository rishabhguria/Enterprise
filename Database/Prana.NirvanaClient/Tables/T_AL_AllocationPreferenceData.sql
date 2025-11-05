CREATE TABLE [dbo].[T_AL_AllocationPreferenceData] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [PresetdefId] INT              NOT NULL,
    [FundId]      INT              NOT NULL,
    [Value]       DECIMAL (32, 19) NOT NULL,
    CONSTRAINT [PK_T_AL_AllocationPreferenceData_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_T_AL_AllocationPreferenceData_FundId] FOREIGN KEY ([FundId]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID]),
    CONSTRAINT [FK_T_AL_AllocationPreferenceData_PresetDefId] FOREIGN KEY ([PresetdefId]) REFERENCES [dbo].[T_AL_AllocationPreferenceDef] ([Id]),
    CONSTRAINT [AK_T_AL_AllocationPreferenceData_PresetDataId_FundId] UNIQUE NONCLUSTERED ([PresetdefId] ASC, [FundId] ASC)
);

