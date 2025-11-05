CREATE TABLE [dbo].[T_TradeAudit](
    [ActionDate]        DATETIME         CONSTRAINT [DF_T_TradeAudit_ActionDate] DEFAULT (getdate()) NOT NULL,
    [GroupId]           VARCHAR (50)     NULL,
    [TaxlotId]          VARCHAR (50)     NULL,
    [OrderId]           VARCHAR (50)     NULL,
    [ParentOrderId]     VARCHAR (50)     NULL,
    [TaxlotClosingId]   UNIQUEIDENTIFIER NULL,
    [Action]            INT              NOT NULL,
    [OriginalValue]     VARCHAR (500)     NULL,
	[NewValue]          VARCHAR (500)     NULL,
    [Comment]           VARCHAR (500)    NULL,
    [CompanyUserId]     INT              NULL,
    [Symbol]            VARCHAR (100)    NULL,
    [FundID]            INT              NULL,
    [OrderSideTagValue] NCHAR (10)       NULL,
    [OriginalDate] DATETIME NOT NULL DEFAULT (getdate()), 
    [IsProcessed] INT NOT NULL DEFAULT 0, 
    [AuditID] INT NOT NULL IDENTITY, 
	[IsProcessedMW] INT NOT NULL DEFAULT 0,
    [Source] 			VARCHAR (50)    NULL,
    CONSTRAINT [PK_T_TradeAudit] PRIMARY KEY ([AuditID]), 
);

GO
CREATE NONCLUSTERED INDEX [IX_nonclustered_ORIGINAL_FundID] ON [dbo].[T_TradeAudit]
(
	[OriginalDate] ASC,
	[FundID] ASC
)
INCLUDE ( 	[IsProcessed]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
