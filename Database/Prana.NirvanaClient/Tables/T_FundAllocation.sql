CREATE TABLE [dbo].[T_FundAllocation] (
    [AllocationId] VARCHAR (50) NOT NULL,
    [GroupID]      VARCHAR (50) NOT NULL,
    [AllocatedQty] FLOAT (53)   NOT NULL,
    [Percentage]   FLOAT (53)   NOT NULL,
    [FundID]       INT          NOT NULL,
    CONSTRAINT [PK_T_FundAllocation] PRIMARY KEY CLUSTERED ([AllocationId] ASC),
    CONSTRAINT [FK__T_FundAll__Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[T_Group] ([GroupID])
);


GO
CREATE STATISTICS [_dta_stat_1088827041_2_1]
    ON [dbo].[T_FundAllocation]([GroupID], [AllocationId]);


GO
CREATE STATISTICS [_dta_stat_1088827041_2_2]
    ON [dbo].[T_FundAllocation]([GroupID], [AllocationId]);


GO
CREATE STATISTICS [_dta_stat_1088827041_2_3]
    ON [dbo].[T_FundAllocation]([GroupID], [AllocationId]);

