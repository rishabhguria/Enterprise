CREATE TABLE [dbo].[T_BTFundAllocation] (
    [AllocationId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [GroupID]      VARCHAR (50) NOT NULL,
    [AllocatedQty] FLOAT (53)   NOT NULL,
    [Percentage]   FLOAT (53)   NOT NULL,
    [FundID]       INT          NOT NULL,
    CONSTRAINT [PK_T_BTFundAllocation] PRIMARY KEY CLUSTERED ([AllocationId] ASC),
    CONSTRAINT [FK__T_BTFundA__Group__5907D0F5] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[BT_BasketGroups] ([GroupID])
);

