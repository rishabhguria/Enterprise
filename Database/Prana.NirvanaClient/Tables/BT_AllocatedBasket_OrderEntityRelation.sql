CREATE TABLE [dbo].[BT_AllocatedBasket_OrderEntityRelation] (
    [BasketGroupID] VARCHAR (50) NOT NULL,
    [EntityID]      NCHAR (50)   NOT NULL,
    [PK]            INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_BT_AllocatedBasket_OrderEntityRelation] PRIMARY KEY CLUSTERED ([PK] ASC)
);

