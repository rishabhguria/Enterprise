CREATE PROCEDURE [dbo].[P_DeleteSymbol_WatchList]
	@Symbol VARCHAR(100),
	@TabName VARCHAR(100),
	@UserID INT
AS
	DECLARE @TabID INT
	SELECT @TabId=TabId FROM [T_WatchList_TabNames] WHERE [TabName] = @TabName AND UserID = @UserId
	DELETE FROM [T_WatchList_Symbols] WHERE Symbol = @Symbol AND TabId = @TabID
