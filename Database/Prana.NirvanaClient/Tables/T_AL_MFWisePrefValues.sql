CREATE TABLE [dbo].[T_AL_MFWisePrefValues]
(
	[MFPreferenceId] INT NOT NULL, 
    [MFId] INT NOT NULL, 
    [Value] DECIMAL(32, 19) NOT NULL, 
    [CalculatedPrefId] INT NOT NULL, 
    PRIMARY KEY ([MFPreferenceId], [MFId]), 
    CONSTRAINT [FK_T_AL_MFWisePrefValues_T_AL_MFAllocationPreference] FOREIGN KEY ([MFPreferenceId]) REFERENCES [T_AL_MFAllocationPreference]([MFPreferenceId]), 
    CONSTRAINT [FK_T_AL_MFWisePrefValues_T_CompanyMasterFunds] FOREIGN KEY ([MFId]) REFERENCES [T_CompanyMasterFunds]([CompanyMasterFundID]) 
)
