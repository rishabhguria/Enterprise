CREATE PROCEDURE [dbo].[P_GetThirdPartyDailyJobStatuses] @runDate DATETIME
AS
BEGIN
	SELECT DJ.JobId
		,B.[Description]
		,AB.AllocStatus
		,AB.AllocID
		,B.ThirdPartyBatchId
		,DJ.BatchRunDate
		,AB.BlockId
		,AB.MsgType
		,TP.CounterPartyID
	FROM T_ThirdPartyDailyJobs DJ
	INNER JOIN T_ThirdPartyBatch B ON DJ.ThirdPartyBatchId = B.ThirdPartyBatchId
	INNER JOIN T_ThirdPartyAllocationBlocks AB ON DJ.JobId = AB.JobId
	INNER JOIN T_ThirdParty TP ON B.ThirdPartyId = TP.ThirdPartyID
	WHERE CAST(DJ.BatchRunDate AS DATE) = CAST(@runDate AS DATE)
		AND AB.MsgType = 'J'
END