CREATE TABLE [dbo].[T_PTTDetails]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1), 
    [PTTId] INT NOT NULL, 
    [AccountId] INT NOT NULL, 
    [StartingPosition] DECIMAL (38, 19) NOT NULL, 
    [StartingValue] DECIMAL (38, 19) NOT NULL, 
    [AccountNAV] FLOAT NOT NULL, 
    [StartingPercentage] DECIMAL (38, 19) NOT NULL, 
    [PercentageType]  DECIMAL (38, 19) NOT NULL, 
    [TradeQuantity]  DECIMAL (38, 19) NOT NULL, 
    [EndingPercentage]  DECIMAL (38, 19) NOT NULL, 
    [EndingPosition]  DECIMAL (38, 18) NOT NULL, 
    [EndingValue]  DECIMAL (38, 18) NOT NULL, 
    [PercentageAllocation]  DECIMAL (38, 19) NOT NULL, 
    [OrderSideID] VARCHAR(10) NULL 
    CONSTRAINT [FK_T_PTTDetails_T_PTTDefinition] FOREIGN KEY ([PTTId]) REFERENCES [T_PTTDefinition]([PTTId])
)
          