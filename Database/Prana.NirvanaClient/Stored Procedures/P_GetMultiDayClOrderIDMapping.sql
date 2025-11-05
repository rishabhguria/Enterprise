CREATE PROCEDURE [dbo].[P_GetMultiDayClOrderIDMapping]
AS
BEGIN
	SELECT latestClOrderID, parentClOrderID
	FROM T_MultiDayClOrderIDMapping
END
