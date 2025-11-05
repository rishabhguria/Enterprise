CREATE TABLE [dbo].[T_MultiBrokerTradeDetails]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[ParentCLOrderId] VARCHAR(50) NOT NULL,
	[CounterPartyID] INT NOT NULL,
	[CLOrderID] VARCHAR(50) NOT NULL,
	[InsertionTime] DATETIME NOT NULL,
	CONSTRAINT FK_T_MultiBrokerTradeDetails_T_Sub_CLOrderID
        FOREIGN KEY (ParentCLOrderId) 
        REFERENCES T_Sub (CLOrderID)
)
