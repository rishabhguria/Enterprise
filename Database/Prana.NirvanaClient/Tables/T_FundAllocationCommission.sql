CREATE TABLE [dbo].[T_FundAllocationCommission] (
    [FundCommissionId] BIGINT     IDENTITY (1, 1) NOT NULL,
    [AllocationId_Fk]  BIGINT     NOT NULL,
    [Commission]       FLOAT (53) NOT NULL,
    [Fees]             FLOAT (53) NOT NULL,
    [IsBasket]         BIT        DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([FundCommissionId] ASC) WITH (FILLFACTOR = 100)
);

