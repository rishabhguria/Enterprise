CREATE TABLE [dbo].[T_AL_PR] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [CheckListId] INT            NOT NULL,
    [PR]          NVARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_T_AL_PR_CheckListId] FOREIGN KEY ([CheckListId]) REFERENCES [dbo].[T_AL_CheckList] ([CheckListId]),
    CONSTRAINT [AK_T_AL_PR_[CheckListId_PR] UNIQUE NONCLUSTERED ([CheckListId] ASC, [PR] ASC)
);

