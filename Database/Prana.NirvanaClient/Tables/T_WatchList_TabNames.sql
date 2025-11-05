CREATE TABLE [dbo].[T_WatchList_TabNames]
(
	[TabId] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [TabName] VARCHAR(100) NOT NULL,
	[IsPermanent] BIT DEFAULT 0, 
	[UserID] INT NOT NULL, 
)
