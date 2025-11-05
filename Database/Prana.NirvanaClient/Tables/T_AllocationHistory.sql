CREATE TABLE [dbo].[T_AllocationHistory] (
    [tradedate]         DATETIME       NULL,
    [ProcessDate]       DATETIME       NULL,
    [SettlementDate]    DATETIME       NULL,
    [Side]              NVARCHAR (50)  NULL,
    [Symbol]            NVARCHAR (200) NULL,
    [TaxLotQty]         FLOAT (53)     NULL,
    [AvgPrice]          FLOAT (53)     NULL,
    [sideMultiplier]    FLOAT (53)     NULL,
    [Commission]        FLOAT (53)     NULL,
    [OtherFees]         FLOAT (53)     NULL,
    [CounterPartyID]    INT            NULL,
    [Broker]            NVARCHAR (200) NULL,
    [shortName]         NVARCHAR (200) NULL,
    [description]       NVARCHAR (200) NULL,
    [GroupID]           NVARCHAR (50)  NULL,
    [TaxLotID]          NVARCHAR (50)  NULL,
    [ParentClOrderID]   NVARCHAR (50)  NULL,
    [FundId]            INT            NULL,
    [OrderSideTagValue] VARCHAR (3)    NULL,
    [AUECLocalDate]     DATETIME       NULL,
    [TradeAttribute1]   VARCHAR (200)  NULL
);

