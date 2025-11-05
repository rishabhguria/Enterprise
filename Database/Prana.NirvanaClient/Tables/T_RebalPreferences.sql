CREATE TABLE [dbo].[T_RebalPreferences]
(
    [PreferenceKey] VARCHAR(200) NOT NULL , 
    [PreferenceValue] NVARCHAR(MAX) NOT NULL, 
    [AccountId] INT NOT NULL DEFAULT (0),
	CONSTRAINT [PK_T_RebalKey] PRIMARY KEY(PreferenceKey,AccountId)
)
