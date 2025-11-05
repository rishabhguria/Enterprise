CREATE TABLE [dbo].[T_BTUnBundledBasketFundOrders] (
    [PK]                INT          IDENTITY (1, 1) NOT NULL,
    [BasketID]          VARCHAR (50) NULL,
    [ClOrderID]         VARCHAR (50) NULL,
    [AllocationStateID] INT          NOT NULL,
    CONSTRAINT [PK__T_UnBundledBaske__0D269379] PRIMARY KEY CLUSTERED ([PK] ASC)
);

