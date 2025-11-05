CREATE TABLE [dbo].[T_EntityGroup] (
    [EntityGroupID] INT          IDENTITY (1, 1) NOT NULL,
    [EntityID]      VARCHAR (50) NULL,
    [GroupID]       VARCHAR (50) NULL,
    CONSTRAINT [PK_T_EntityGroup] PRIMARY KEY CLUSTERED ([EntityGroupID] ASC),
    CONSTRAINT [FK_T_EntityGroup_T_EntityOrder] FOREIGN KEY ([EntityID]) REFERENCES [dbo].[T_EntityOrder] ([EntityID])
);

