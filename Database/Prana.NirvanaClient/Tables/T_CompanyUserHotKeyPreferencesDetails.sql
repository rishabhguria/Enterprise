CREATE TABLE [dbo].[T_CompanyUserHotKeyPreferencesDetails](
	[CompanyUserHotKeyID] INT IDENTITY (1, 1) NOT NULL,
	[CompanyUserID] INT NOT NULL,
	[CompanyUserHotKeyName] VARCHAR(MAX) NOT NULL,
	[HotKeyPreferenceNameValue] VARCHAR(MAX),
	[IsFavourites] BIT,
	[HotKeySequence] INT,
	[Module] VARCHAR(MAX),    
	[HotButtonType] VARCHAR(MAX),
	[LastSavedTime] DATETIME,
	CONSTRAINT [PK_T_CompanyUserHotKeyPreferencesDetails] PRIMARY KEY CLUSTERED ([CompanyUserHotKeyID] ASC),
	CONSTRAINT [FK_T_CompanyUserHotKeyPreferencesDetails_T_CompanyUser] FOREIGN KEY ([CompanyUserID]) REFERENCES [dbo].[T_CompanyUser] ([UserID])
);