CREATE TABLE [dbo].[T_ThirdPartyFileAuditLogs]
(
	[LogId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[DateTime] DateTime NOT NULL,
	[Type] VARCHAR(15) NOT NULL,
	[Action] VARCHAR(MAX) NOT NULL,
	[Details] VARCHAR(MAX) NOT NULL
)
