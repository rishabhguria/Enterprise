CREATE PROCEDURE [dbo].[P_SaveLinkedTab_Watchlist]
	@TabName VARCHAR(50),
	@UserId INT
AS

Declare @TabID INT = -1

select @TabID = TabID from T_WatchList_TabNames where TabName = @TabName and UserID = @UserId

IF EXISTS (SELECT 1 FROM T_WatchList_LinkedTab WHERE UserID = @UserId)
	BEGIN
		UPDATE [T_WatchList_LinkedTab]
		SET TabID = @TabID
		WHERE UserID = @UserId 
	END
	ELSE
		INSERT INTO [T_WatchList_LinkedTab] VALUES(@TabID, @UserId)
RETURN 0
