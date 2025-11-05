CREATE PROCEDURE [dbo].[P_GetMultiDayOrderAllocations]
AS
BEGIN
	SELECT CLOrderId, GroupId
	FROM T_MultiDayOrderAllocation
END
