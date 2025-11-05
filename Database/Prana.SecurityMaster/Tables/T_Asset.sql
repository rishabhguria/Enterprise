CREATE TABLE [dbo].[T_Asset] (
    [AssetID]   INT          IDENTITY (1, 1) NOT NULL,
    [AssetName] VARCHAR (50) NOT NULL,
    [Comment]   TEXT         NULL,
    CONSTRAINT [PK__T_Asset__0E6E26BF] PRIMARY KEY CLUSTERED ([AssetID] ASC) WITH (FILLFACTOR = 100)
);

