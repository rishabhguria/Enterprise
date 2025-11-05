CREATE TABLE [dbo].[T_OrderSide] (
    [OrderSideID] INT          IDENTITY (1, 1) NOT NULL,
    [OrderSide]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_OrderSide] PRIMARY KEY CLUSTERED ([OrderSideID] ASC)
);

