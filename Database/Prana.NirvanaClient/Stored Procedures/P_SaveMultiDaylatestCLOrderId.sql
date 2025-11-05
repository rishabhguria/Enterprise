CREATE PROCEDURE [dbo].[P_SaveMultiDaylatestCLOrderId]
     @latestClOrderId VARCHAR(50)
	,@parentClOrderId VARCHAR(50)
AS
BEGIN
	IF EXISTS (
			SELECT TOP 1 1
			FROM T_MultiDayClOrderIDMapping
			WHERE latestClOrderID = @latestClOrderId
			)
	BEGIN
		UPDATE T_MultiDayClOrderIDMapping
		SET parentClOrderID = @parentClOrderId
		WHERE latestClOrderID = @latestClOrderId
	END
	ELSE
	BEGIN
		INSERT INTO T_MultiDayClOrderIDMapping
		SELECT @latestClOrderId
			,@parentClOrderId
	END
END