CREATE TABLE [dbo].[T_CompanyTTAssetPreferences] (
	[CompanyAssetWisePrefID] INT IDENTITY(1, 1) NOT NULL PRIMARY KEY
	,[CompanyID] INT NOT NULL
	,[AssetID] INT NOT NULL
	,[SideID] INT 
	,FOREIGN KEY (AssetID) REFERENCES T_Asset(AssetID)
	,FOREIGN KEY (CompanyID) REFERENCES T_Company(CompanyID)
	,FOREIGN KEY (SideID) REFERENCES T_Side(SideID)
	)
