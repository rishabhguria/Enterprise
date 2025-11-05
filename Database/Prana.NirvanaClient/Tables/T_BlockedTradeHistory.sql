CREATE TABLE [dbo].[T_BlockedTradeHistory]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [InsertionDate] DATETIME NULL, 
    [PranaMessage] VARCHAR(MAX) NULL
)
