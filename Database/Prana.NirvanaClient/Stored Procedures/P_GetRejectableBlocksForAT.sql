CREATE PROCEDURE [dbo].[P_GetRejectableBlocksForAT] @allocIds NVARCHAR(MAX)
AS
BEGIN
	DECLARE @AllocID_TV TABLE (AllocId NVARCHAR(100))

	INSERT INTO @AllocID_TV (AllocId)
	SELECT ITEMS
	FROM dbo.Split(@allocIds, ',');

	SELECT AB.AllocId
		,AB.AllocReportId
	FROM T_ThirdPartyAllocationBlocks AB
	JOIN @AllocID_TV TV ON AB.AllocID = TV.AllocId
	WHERE AB.MsgType = 'AS'
		AND AB.Substatus IN (
			'4'
			,'9'
			,'10'
			,'11'
			,'12'
			,'13'
			,'18'
			,'23'
			);
END
