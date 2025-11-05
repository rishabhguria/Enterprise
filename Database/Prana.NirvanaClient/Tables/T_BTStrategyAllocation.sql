CREATE TABLE [dbo].[T_BTStrategyAllocation] (
    [AllocationID] BIGINT       IDENTITY (1, 1) NOT NULL,
    [GroupID]      VARCHAR (50) NULL,
    [AllocatedQty] FLOAT (53)   NULL,
    [Percentage]   FLOAT (53)   NULL,
    [StrategyID]   INT          NULL,
    CONSTRAINT [PK_T_BTStrategyAllocation] PRIMARY KEY CLUSTERED ([AllocationID] ASC),
    CONSTRAINT [FK__T_BTStrat__Group__59FBF52E] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[BT_BasketGroups] ([GroupID])
);

