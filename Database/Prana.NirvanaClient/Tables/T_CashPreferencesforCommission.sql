CREATE TABLE [dbo].[T_CashPreferencesforCommission]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [AssetClass] VARCHAR(50) NOT NULL, 
    [IsChecked] BIT NOT NULL
)
