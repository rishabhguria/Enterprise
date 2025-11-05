CREATE TABLE [dbo].[T_ThirdPartyTimeBatches]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FileFormatId] INT NOT NULL,
	[BatchRunTime] Time NOT NULL,
	[LastRunDate] DateTime,
	[BatchSuccess] BIT DEFAULT(0) NOT NULL,
	[IsPaused] BIT DEFAULT(0) NOT NULL,
)

