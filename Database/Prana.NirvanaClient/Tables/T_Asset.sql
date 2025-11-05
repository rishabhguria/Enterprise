CREATE TABLE [dbo].[T_Asset] (
    [AssetID]   INT          IDENTITY (1, 1) NOT NULL,
    [AssetName] VARCHAR (50) NOT NULL,
    [Comment]   TEXT         NULL,
    CONSTRAINT [PK_T_Asset] PRIMARY KEY CLUSTERED ([AssetID] ASC)
);

