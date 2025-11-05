CREATE TABLE [dbo].[T_UDAAssetClass] (
    [AssetName] VARCHAR (200) NULL,
    [AssetID]   INT           NULL,
    CONSTRAINT [T_UDAAssetClass_Unique_AssetName] UNIQUE NONCLUSTERED ([AssetName] ASC)
);

