CREATE TABLE [dbo].[T_AL_OrderSide]
(
	[Id]          INT IDENTITY (1, 1) NOT NULL,
    [CheckListId] INT NOT NULL,
    [OrderSideId]  VARCHAR(50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_T_AL_OrderSide_CheckListId] FOREIGN KEY ([CheckListId]) REFERENCES [dbo].[T_AL_CheckList] ([CheckListId]),
    CONSTRAINT [AK_T_AL_OrderSide_[CheckListId_OrderSideId] UNIQUE NONCLUSTERED ([CheckListId] ASC, [OrderSideId] ASC)
)
