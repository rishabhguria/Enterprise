CREATE PROCEDURE [dbo].[P_NewTab_WatchList]
	@TabName VARCHAR(50),
	@UserId INT
AS
	INSERT INTO [T_WatchList_TabNames] (TabName, UserID) VALUES(@TabName, @UserId)
RETURN 0
