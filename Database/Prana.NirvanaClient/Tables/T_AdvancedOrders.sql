CREATE TABLE [dbo].[T_AdvancedOrders] (
    [AdvancedOrdersID] INT          IDENTITY (1, 1) NOT NULL,
    [AdvancedOrders]   VARCHAR (50) NULL,
    CONSTRAINT [PK_T_AdvancedOrders] PRIMARY KEY CLUSTERED ([AdvancedOrdersID] ASC)
);

