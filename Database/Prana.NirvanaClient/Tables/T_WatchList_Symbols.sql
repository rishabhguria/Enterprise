CREATE TABLE [dbo].[T_WatchList_Symbols]
(
	[Symbol] VARCHAR(100) NOT NULL, 
    [TabId] INT NOT NULL, 
    CONSTRAINT [T_WatchList_Symbols_ToTableT_WatchList_TabNames] FOREIGN KEY ([TabId]) REFERENCES [T_WatchList_TabNames]([TabId])
)
