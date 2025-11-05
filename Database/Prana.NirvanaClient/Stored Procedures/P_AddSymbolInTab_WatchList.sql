CREATE PROCEDURE [dbo].[P_AddSymbolInTab_WatchList]
	@Symbol VARCHAR(100),
	@TabName VARCHAR(100),
	@UserId INT
AS
	DECLARE @TabId INT 
	SELECT @TabId=TabId FROM [T_WatchList_TabNames] WHERE [TabName] = @TabName AND UserID = @UserId
	INSERT INTO [T_WatchList_Symbols](Symbol, TabId) VALUES(@Symbol, @TabId);
RETURN 0
