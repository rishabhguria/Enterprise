CREATE TABLE [dbo].[T_StrategyAllocationCommission] (
    [StrategyCommissionId] BIGINT     IDENTITY (1, 1) NOT NULL,
    [AllocationId_Fk]      BIGINT     NOT NULL,
    [Commission]           FLOAT (53) NOT NULL,
    [Fees]                 FLOAT (53) NOT NULL,
    PRIMARY KEY CLUSTERED ([StrategyCommissionId] ASC) WITH (FILLFACTOR = 100)
);

