CREATE TABLE [dbo].[T_SwapParameters] (
    [SwapPK]                  INT           IDENTITY (1, 1) NOT NULL,
    [GroupID]                 VARCHAR (50)  NOT NULL,
    [NotionalValue]           FLOAT (53)    NULL,
    [BenchMarkRate]           FLOAT (53)    NULL,
    [Differential]            FLOAT (53)    NULL,
    [OrigCostBasis]           FLOAT (53)    NULL,
    [DayCount]                INT           NULL,
    [SwapDescription]         VARCHAR (500) NULL,
    [FirstResetDate]          DATETIME      NULL,
    [OrigTransDate]           DATETIME      NULL,
    [ResetFrequency]          VARCHAR (20)  NULL,
    [ClosingPrice]            FLOAT (53)    NULL,
    [ClosingDate]             DATETIME      NULL,
    [TransDate]               DATETIME      NULL,
    [ShouldOverrideNotional]  BIT           NULL,
    [ShouldOverrideCostBasis] BIT           NULL,
    CONSTRAINT [PK_T_SwapDescription] PRIMARY KEY CLUSTERED ([SwapPK] ASC)
);
go


CREATE NONCLUSTERED INDEX [<ix_T_SwapParameters_GroupID, sysname,>] ON [dbo].[T_SwapParameters]
(
[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


