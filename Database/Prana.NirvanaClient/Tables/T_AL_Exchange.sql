CREATE TABLE [dbo].[T_AL_Exchange] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [CheckListId] INT NOT NULL,
    [ExchangeId]  INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_T_AL_Exchange_CheckListId] FOREIGN KEY ([CheckListId]) REFERENCES [dbo].[T_AL_CheckList] ([CheckListId]),
    CONSTRAINT [FK_T_AL_Exchange_ExchangeId] FOREIGN KEY ([ExchangeId]) REFERENCES [dbo].[T_Exchange] ([ExchangeID]),
    CONSTRAINT [AK_T_AL_Exchange_[CheckListId_ExchangeId] UNIQUE NONCLUSTERED ([CheckListId] ASC, [ExchangeId] ASC)
);

