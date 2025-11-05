CREATE TABLE [dbo].[T_UnAllocatedGroupOrder] (
    [GroupID]      VARCHAR (50) NULL,
    [OrderID]      VARCHAR (500) NOT NULL,
    [GroupOrderID] BIGINT       IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_T_UnAllocatedGroupOrder] PRIMARY KEY CLUSTERED ([GroupOrderID] ASC),
    CONSTRAINT [FK_T_UnAllocatedGroupOrder_T_UnallocatedGroup] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[T_UnallocatedGroup] ([GroupID])
);

