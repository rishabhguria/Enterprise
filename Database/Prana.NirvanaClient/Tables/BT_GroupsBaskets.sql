CREATE TABLE [dbo].[BT_GroupsBaskets] (
    [GroupBasketID] INT          IDENTITY (1, 1) NOT NULL,
    [GroupID]       VARCHAR (50) NOT NULL,
    [BasketID]      VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_BT_GroupsBaskets] PRIMARY KEY CLUSTERED ([GroupBasketID] ASC),
    CONSTRAINT [FK__BT_Groups__Group__5813ACBC] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[BT_BasketGroups] ([GroupID])
);

