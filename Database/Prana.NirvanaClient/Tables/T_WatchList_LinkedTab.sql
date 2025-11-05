CREATE TABLE [dbo].[T_WatchList_LinkedTab]
(
    [TabID] INT NOT NULL,
	[UserID] INT NOT NULL, 
    CONSTRAINT [T_WatchList_LinkedTab_ToTableT_WatchList_TabNames] FOREIGN KEY ([TabId]) REFERENCES [T_WatchList_TabNames]([TabId])
)
