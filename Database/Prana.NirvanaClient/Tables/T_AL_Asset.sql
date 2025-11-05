CREATE TABLE [dbo].[T_AL_Asset] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [CheckListId] INT NOT NULL,
    [AssetId]     INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_T_AL_Asset_AssetId] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[T_Asset] ([AssetID]),
    CONSTRAINT [FK_T_AL_Asset_CheckListId] FOREIGN KEY ([CheckListId]) REFERENCES [dbo].[T_AL_CheckList] ([CheckListId]),
    CONSTRAINT [AK_T_AL_Asset_[CheckListId_AssetId] UNIQUE NONCLUSTERED ([CheckListId] ASC, [AssetId] ASC)
);

