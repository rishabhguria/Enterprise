CREATE TABLE [dbo].[T_CashAudit]
(
	[AuditId] INT IDENTITY (1,1) NOT NULL PRIMARY KEY, 
    [ActionDate] DATETIME NOT NULL, 
    [Action] VARCHAR(150) NOT NULL, 
    [FromDate] DATETIME NULL, 
    [ToDate] DATETIME NULL, 
    [AccountIds] NVARCHAR(MAX) NOT NULL, 
    [UserId] INT NOT NULL
)
