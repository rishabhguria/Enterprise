CREATE TABLE [dbo].[T_TempTradeAudit]
(
	[AuditID] INT NOT NULL, 
    [Symbol] VARCHAR(MAX) NULL, 
    [Action] INT NULL, 
    [FundId] INT NULL, 
    [OriginalDate] DATETIME NULL 
)
