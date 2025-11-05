CREATE PROCEDURE [dbo].[P_SaveMultiDayOrderAllocation]
	@clOrderID VARCHAR (50),
	@groupId VARCHAR (50)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM T_MultiDayOrderAllocation WHERE ClOrderID = @clOrderID)
		BEGIN
			INSERT INTO T_MultiDayOrderAllocation
			SELECT @clOrderID, @groupId
		END
	ELSE
		BEGIN
			UPDATE T_MultiDayOrderAllocation SET GroupId = @groupId WHERE ClOrderID = @clOrderID
		END
END
