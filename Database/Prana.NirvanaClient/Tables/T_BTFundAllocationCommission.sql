CREATE TABLE [dbo].[T_BTFundAllocationCommission] (
    [FundCommissionId] BIGINT     IDENTITY (1, 1) NOT NULL,
    [AllocationID]     BIGINT     NOT NULL,
    [Commission]       FLOAT (53) NOT NULL,
    [Fees]             FLOAT (53) NOT NULL,
    PRIMARY KEY CLUSTERED ([FundCommissionId] ASC) WITH (FILLFACTOR = 100)
);

