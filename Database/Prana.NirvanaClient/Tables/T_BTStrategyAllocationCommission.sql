CREATE TABLE [dbo].[T_BTStrategyAllocationCommission] (
    [StrategyAllocationId] BIGINT     IDENTITY (1, 1) NOT NULL,
    [AllocationId]         BIGINT     NOT NULL,
    [Commission]           FLOAT (53) NOT NULL,
    [Fees]                 NCHAR (10) NOT NULL,
    PRIMARY KEY CLUSTERED ([StrategyAllocationId] ASC) WITH (FILLFACTOR = 100)
);

