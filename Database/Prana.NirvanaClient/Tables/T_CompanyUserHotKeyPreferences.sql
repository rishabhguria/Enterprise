CREATE TABLE [dbo].[T_CompanyUserHotKeyPreferences](
	[CompanyUserHotKeyPreferenceID] INT IDENTITY (1, 1) NOT NULL,
	[CompanyUserID] INT NOT NULL,
	[HotKeyPreferenceElements] VARCHAR(MAX),
	[EnableBookMarkIcon] BIT,
	[HotKeyOrderChanged] BIT,
	[TTTogglePreferenceForWeb] BIT,
	[LastSavedTime] DATETIME,
	CONSTRAINT [PK_T_CompanyUserHotKeyPreferences] PRIMARY KEY CLUSTERED ([CompanyUserHotKeyPreferenceID] ASC),
	CONSTRAINT [FK_T_CompanyUserHotKeyPreferences_T_CompanyUser] FOREIGN KEY ([CompanyUserID]) REFERENCES [dbo].[T_CompanyUser] ([UserID])
);