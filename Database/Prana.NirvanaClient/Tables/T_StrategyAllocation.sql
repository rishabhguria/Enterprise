CREATE TABLE [dbo].[T_StrategyAllocation] (
    [AllocationID] BIGINT       IDENTITY (1, 1) NOT NULL,
    [GroupID]      VARCHAR (50) NOT NULL,
    [AllocatedQty] FLOAT (53)   NOT NULL,
    [Percentage]   FLOAT (53)   NOT NULL,
    [StrategyID]   INT          NOT NULL,
    CONSTRAINT [PK_T_StrategyAllocation] PRIMARY KEY CLUSTERED ([AllocationID] ASC),
    CONSTRAINT [FK__T_Strateg__Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[T_Group] ([GroupID])
);

