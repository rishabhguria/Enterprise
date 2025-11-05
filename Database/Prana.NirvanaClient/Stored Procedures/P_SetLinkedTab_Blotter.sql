CREATE PROCEDURE [dbo].[P_SetLinkedTab_Blotter]
	@TabName VARCHAR(50),
	@UserId INT
AS
	IF EXISTS (SELECT 1 FROM T_Blotter_LinkedTab WHERE UserID = @UserId)
	BEGIN
		UPDATE [T_Blotter_LinkedTab]
		SET TabName = @TabName
		WHERE UserID = @UserId 
	END
	ELSE
		INSERT INTO [T_Blotter_LinkedTab] VALUES(@TabName, @UserId)
RETURN 0
