CREATE TABLE [dbo].[T_UnderLying] (
    [UnderLyingID]   INT          NOT NULL,
    [UnderLyingName] VARCHAR (50) NOT NULL,
    [AssetID]        INT          NOT NULL,
    [Comment]        TEXT         NULL,
    CONSTRAINT [PK__T_UnderLying__14270015] PRIMARY KEY CLUSTERED ([UnderLyingID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK__T_UnderLy__Asset__18EBB532] FOREIGN KEY ([AssetID]) REFERENCES [dbo].[T_Asset] ([AssetID])
);

