CREATE TABLE [dbo].[T_BasketGroups] (
    [BasketGroupID] VARCHAR (50) NOT NULL,
    [BasketID]      VARCHAR (50) NULL,
    CONSTRAINT [PK_T_BasketGroups] PRIMARY KEY CLUSTERED ([BasketGroupID] ASC),
    CONSTRAINT [FK_T_BasketGroups_T_BTUploadedBaskets] FOREIGN KEY ([BasketID]) REFERENCES [dbo].[T_BTUploadedBaskets] ([BasketID])
);

