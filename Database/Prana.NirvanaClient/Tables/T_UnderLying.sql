CREATE TABLE [dbo].[T_UnderLying] (
    [UnderLyingID]   INT          IDENTITY (1, 1) NOT NULL,
    [UnderLyingName] VARCHAR (50) NOT NULL,
    [AssetID]        INT          NOT NULL,
    [Comment]        TEXT         NULL,
    CONSTRAINT [PK_T_UnderLying] PRIMARY KEY CLUSTERED ([UnderLyingID] ASC),
    CONSTRAINT [FK_T_UnderLying_T_Asset] FOREIGN KEY ([AssetID]) REFERENCES [dbo].[T_Asset] ([AssetID])
);

