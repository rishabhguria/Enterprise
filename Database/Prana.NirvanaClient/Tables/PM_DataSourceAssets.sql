CREATE TABLE [dbo].[PM_DataSourceAssets] (
    [DataSourceAssetID]   INT           IDENTITY (1, 1) NOT NULL,
    [ThirdPartyID]        INT           NOT NULL,
    [DataSourceAssetName] NVARCHAR (50) NOT NULL,
    [ApplicationAssetID]  INT           NOT NULL,
    CONSTRAINT [PK_DataSourceAssets] PRIMARY KEY CLUSTERED ([DataSourceAssetID] ASC),
    CONSTRAINT [FK_PM_DataSourceAssets_T_ThirdParty] FOREIGN KEY ([ThirdPartyID]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID]),
    CONSTRAINT [CK_PM_DataSourceAssets_Unique_DataSourceID_ApplicationAssetID] UNIQUE NONCLUSTERED ([ThirdPartyID] ASC, [ApplicationAssetID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_PM_DataSourceAssets]
    ON [dbo].[PM_DataSourceAssets]([ThirdPartyID] ASC, [ApplicationAssetID] ASC);

