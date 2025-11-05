CREATE TABLE [dbo].[T_AssetSide]
(
    [AssetSideID] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [AssetID] INT NOT NULL, 
    [SideID] INT NOT NULL,
    Foreign Key (SideID) REFERENCES T_Side(SideID),
    Foreign Key (AssetID) REFERENCES T_Asset(AssetID)
)
