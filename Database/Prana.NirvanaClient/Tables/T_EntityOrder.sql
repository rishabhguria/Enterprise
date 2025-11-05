CREATE TABLE [dbo].[T_EntityOrder] (
    [EntityID]           VARCHAR (50) NOT NULL,
    [ClOrderID]          BIGINT       NULL,
    [SideTagValue]       VARCHAR (3)  NULL,
    [Symbol]             VARCHAR (10) NULL,
    [CounterPartyID]     INT          NULL,
    [VenueID]            INT          NULL,
    [TradingAccountID]   INT          NULL,
    [AllocatedQty]       BIGINT       NULL,
    [AvgPrice]           FLOAT (53)   NULL,
    [ExeQty]             BIGINT       NULL,
    [OrderTypeTagValue]  VARCHAR (3)  NULL,
    [TotalQty]           BIGINT       NULL,
    [AUECID]             INT          NULL,
    [IsPreAllocated]     INT          NOT NULL,
    [ListID]             VARCHAR (50) NOT NULL,
    [UserID_Allocatedby] INT          NOT NULL,
    CONSTRAINT [PK_T_EntityOrder] PRIMARY KEY CLUSTERED ([EntityID] ASC)
);

