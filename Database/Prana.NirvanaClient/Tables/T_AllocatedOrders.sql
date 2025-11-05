CREATE TABLE [dbo].[T_AllocatedOrders] (
    [OrderID]        BIGINT        NULL,
    [Side]           VARCHAR (10)  NULL,
    [Symbol]         VARCHAR (100) NULL,
    [Quantity]       FLOAT (53)    NULL,
    [TradingAccount] VARCHAR (20)  NULL,
    [CounterParty]   VARCHAR (20)  NULL,
    [Venue]          VARCHAR (20)  NULL,
    [Price]          FLOAT (53)    NULL,
    [GroupID]        VARCHAR (30)  NULL,
    [FundID]         VARCHAR (30)  NULL,
    [AllocatedOrder] INT           IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_T_AllocatedOrders] PRIMARY KEY CLUSTERED ([AllocatedOrder] ASC)
);

