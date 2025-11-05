CREATE PROCEDURE [dbo].[P_GetLinkedTab_Blotter]
	@UserId INT
AS
	IF EXISTS(SELECT TabName FROM T_Blotter_LinkedTab where UserID = @UserId)
		SELECT TabName FROM T_Blotter_LinkedTab where UserID = @UserId 
	ELSE
		SELECT ''
RETURN 0