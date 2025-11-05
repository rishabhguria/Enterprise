CREATE TABLE [dbo].[T_SwapParameters_DeletedAudit] (
    [GroupID]                 VARCHAR (50)     NOT NULL,
    [NotionalValue]           FLOAT (53)       NULL,
    [BenchMarkRate]           FLOAT (53)       NULL,
    [Differential]            FLOAT (53)       NULL,
    [OrigCostBasis]           FLOAT (53)       NULL,
    [DayCount]                INT              NULL,
    [SwapDescription]         VARCHAR (500)    NULL,
    [FirstResetDate]          DATETIME         NULL,
    [OrigTransDate]           DATETIME         NULL,
    [ResetFrequency]          VARCHAR (20)     NULL,
    [ClosingPrice]            FLOAT (53)       NULL,
    [ClosingDate]             DATETIME         NULL,
    [TransDate]               DATETIME         NULL,
    [ShouldOverrideNotional]  BIT              NULL,
    [ShouldOverrideCostBasis] BIT              NULL,
);

