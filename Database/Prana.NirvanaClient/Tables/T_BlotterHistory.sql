CREATE TABLE [dbo].[T_BlotterHistory] (
    [TradeDate]         DATETIME       NULL,
    [Side]              NVARCHAR (50)  NULL,
    [Symbol]            NVARCHAR (200) NULL,
    [LastShares]        FLOAT (53)     NULL,
    [LastPx]            FLOAT (53)     NULL,
    [Broker]            NVARCHAR (200) NULL,
    [users]             NVARCHAR (200) NULL,
    [Description]       NVARCHAR (200) NULL,
    [SideMultiplier]    FLOAT (53)     NULL,
    [ParentCLOrderID]   VARCHAR (50)   NULL,
    [CLOrderID]         VARCHAR (50)   NULL,
    [Quantity]          FLOAT (53)     NULL,
    [CumQty]            FLOAT (53)     NULL,
    [AveragePrice]      FLOAT (53)     NULL,
    [OrderSidetagValue] VARCHAR (3)    NULL,
    [Fills_PK]          INT            NULL,
    [IsSummary]         BIT            NULL
);

