CREATE PROCEDURE [dbo].[P_GetThirdPartyDataForATMessage] @allocId NVARCHAR(100)
AS
BEGIN
	DECLARE @BlockId INT = (
			SELECT BlockId
			FROM T_ThirdPartyAllocationBlocks
			WHERE AllocID = @allocId
				AND MsgType = 'J'
			);

	SELECT IndividualAllocID
		,AllocQty
		,AllocAvgPx
		,Commission
		,[Misc Fees]
		,NetMoney
	FROM T_ThirdPartyAllocationMessages
	WHERE BlockId = @BlockId

	DECLARE @BatchId INT = (
			SELECT TOP 1 ThirdPartyBatchId
			FROM T_ThirdPartyDailyJobs DJ
			JOIN T_ThirdPartyAllocationBlocks AB ON DJ.JobId = AB.JobId
			WHERE AB.AllocId = @allocId
			ORDER BY BlockId DESC
			)
	DECLARE @CounterPartyId INT = (
			SELECT TOP 1 TP.CounterPartyID
			FROM T_ThirdPartyBatch TB
			JOIN T_ThirdParty TP ON TB.ThirdPartyId = TP.ThirdPartyId
			WHERE TB.ThirdPartyBatchId = @BatchId
			)

	SELECT MatchingField
		,AvgPrice
		,Commission
		,MiscFees
		,NetMoney
	FROM T_ThirdPartyToleranceProfile
	WHERE ThirdPartyBatchId = @BatchId;

	SELECT @CounterPartyId
END