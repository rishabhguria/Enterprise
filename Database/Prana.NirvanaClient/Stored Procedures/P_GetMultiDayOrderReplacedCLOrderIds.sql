CREATE PROCEDURE [dbo].[P_GetMultiDayOrderReplacedCLOrderIds]
AS
BEGIN
	SELECT OriginalCLOrderId
		,CLOrderId
	FROM T_MultiDayOrderReplacedCLOrderId
END