CREATE TABLE [dbo].[T_PTTDefinition]
(
    [PTTId] INT NOT NULL PRIMARY KEY, 
    [Symbol] NVARCHAR(50) NOT NULL, 
    [Target] DECIMAL (32, 19) NOT NULL, 
    [Type] INT NOT NULL, 
    [Add_Set] INT NOT NULL, 
    [MasterFundOrAccount] NVARCHAR(50) NOT NULL, 
    [CombinedAccountsTotalValue] BIT NOT NULL, 
    [Price] DECIMAL (32, 19) NOT NULL,
    [IsTradeBreak] BIT, 
    [IsRoundLot] BIT,
    [SelectedFundIds] NVARCHAR(MAX) NULL
)
