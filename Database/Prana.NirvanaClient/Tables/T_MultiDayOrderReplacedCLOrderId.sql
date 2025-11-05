CREATE TABLE [dbo].[T_MultiDayOrderReplacedCLOrderId]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1, 1),
	[OriginalCLOrderId] VARCHAR(50) NOT NULL,
	[CLOrderId] VARCHAR(50) NOT NULL
)
