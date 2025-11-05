CREATE PROCEDURE [dbo].[P_GetAllUserData_WatchList]
	@UserId int
AS
	SELECT TabName, IsPermanent FROM [T_WatchList_TabNames] WHERE UserID = @UserId order by TabId
	SELECT SymbolT.Symbol, TabT.[TabName]
	FROM [T_WatchList_Symbols] AS SymbolT INNER JOIN [T_WatchList_TabNames] AS TabT ON SymbolT.TabId = TabT.TabId  
	WHERE TabT.UserID = @UserId
	order By TabT.TabId
RETURN 0
