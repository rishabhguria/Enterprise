CREATE PROCEDURE [dbo].[P_SaveMultiDayOrderReplacedCLOrderId] @originalClOrderId VARCHAR(50)
	,@clOrderid VARCHAR(50)
AS
BEGIN
	IF EXISTS (
			SELECT TOP 1 1
			FROM T_MultiDayOrderReplacedCLOrderId
			WHERE OriginalCLOrderId = @originalClOrderId
			)
	BEGIN
		UPDATE T_MultiDayOrderReplacedCLOrderId
		SET CLOrderId = @clOrderid
		WHERE OriginalCLOrderId = @originalClOrderId
	END
	ELSE
	BEGIN
		INSERT INTO T_MultiDayOrderReplacedCLOrderId
		SELECT @originalClOrderId
			,@clOrderid
	END
END