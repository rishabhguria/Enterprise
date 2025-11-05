CREATE TABLE [dbo].[T_AL_MasterFundProrataList]
(
    [MFPreferenceId]	INT NOT NULL,
    [MasterFundId]		INT NOT NULL,
    CONSTRAINT [PK_T_AL_MasterFundProrataList_MFPreferenceId_MasterFundId] PRIMARY KEY CLUSTERED ([MFPreferenceId] ASC, [MasterFundId] ASC),
    CONSTRAINT [FK_T_AL_MasterFundProrataList_MasterFundId] FOREIGN KEY ([MasterFundId]) REFERENCES [dbo].[T_CompanyMasterFunds] ([CompanyMasterFundID]),
    CONSTRAINT [FK_T_AL_MasterFundProrataList_MFPreferenceId] FOREIGN KEY ([MFPreferenceId]) REFERENCES [dbo].[T_AL_MFAllocationPreference] ([MFPreferenceId])
)
