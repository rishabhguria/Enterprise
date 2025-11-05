CREATE TABLE [dbo].[T_UserTTAssetPreferences] (
	[UserAssetWisePrefID] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY
	,[UserID] INT NOT NULL
	,[AssetID] INT NOT NULL
	,[SideID] INT  NULL
	,FOREIGN KEY (AssetID) REFERENCES T_Asset(AssetID)
	,FOREIGN KEY (UserID) REFERENCES T_CompanyUser(UserID)
	)
