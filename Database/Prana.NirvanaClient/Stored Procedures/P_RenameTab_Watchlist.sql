CREATE PROCEDURE [dbo].[P_RenameTab_Watchlist]
	@NewTabName VARCHAR(100),
	@OldTabName VARCHAR(100),
	@UserId INT
AS
	UPDATE T_WatchList_TabNames
	SET TabName = @NewTabName
	WHERE TabName = @OldTabName AND UserID = @UserId
RETURN 0