CREATE TABLE [dbo].[BT_BasketGroups] (
    [GroupID]          VARCHAR (50) NOT NULL,
    [AddedDate]        DATETIME     NOT NULL,
    [AllocationType]   INT          NOT NULL,
    [UserID]           INT          NOT NULL,
    [AssetID]          INT          NULL,
    [UnderLyingID]     INT          NULL,
    [TradingAccountID] INT          NULL,
    [AllocatedQty]     FLOAT (53)   NULL,
    [ExeQty]           FLOAT (53)   NULL,
    [TotalQty]         FLOAT (53)   NULL,
    [AUECID]           INT          NULL,
    [ListID]           VARCHAR (50) NULL,
    [AllocationDate]   DATETIME     NULL,
    [StateID]          INT          NOT NULL,
    [AUECLocalDate]    DATETIME     NOT NULL,
    CONSTRAINT [PK_BT_BasketGroups] PRIMARY KEY CLUSTERED ([GroupID] ASC)
);

