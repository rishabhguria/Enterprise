CREATE PROCEDURE [dbo].[P_RemoveTab_WatchList]
	@tabName varchar(50),
	@userId int
AS
	DECLARE @tabID int
	SELECT @tabID =TabId FROM [T_WatchList_TabNames] WHERE TabName = @tabName AND UserID = @userId
	DELETE FROM T_WatchList_Symbols WHERE TabId = @tabID
	DELETE FROM T_WatchList_LinkedTab WHERE TabId = @tabID
	DELETE FROM T_WatchList_TabNames WHERE TabId = @tabID
RETURN 0
