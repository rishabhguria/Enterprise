CREATE TABLE [dbo].[T_PTTMasterFundPreference]
(
	[MasterFundId] INT NOT NULL , 
    [UseProrataPreference] bit NULL DEFAULT 1,
	[PreferenceType] INT NOT NULL Default 0
)