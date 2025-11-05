CREATE PROCEDURE [dbo].[P_GetThirdPartyJMsgAndToleranceData] @individualAllocId NVARCHAR(100)
	,@allocId NVARCHAR(100)
AS
BEGIN
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

	SELECT TOP 1 AM.AllocAvgPx
		,AM.Commission
		,AM.[Misc Fees]
		,AM.NetMoney
		,AM.AllocQty
	FROM T_ThirdPartyAllocationMessages AM
	JOIN T_ThirdPartyAllocationBlocks AB ON AM.BlockId = AB.BlockId
	WHERE AM.IndividualAllocID = @individualAllocId
		AND AB.MsgType = 'J'
	ORDER BY AM.MsgId DESC;

	SELECT MatchingField
		,AvgPrice
		,Commission
		,MiscFees
		,NetMoney
	FROM T_ThirdPartyToleranceProfile
	WHERE ThirdPartyBatchId = @BatchId;

	DECLARE @TransactTimePMsg DATETIME = (
			SELECT TOP 1 TransactTime
			FROM T_ThirdPartyAllocationBlocks
			WHERE AllocID = @allocId
				AND MsgType = 'P'
			ORDER BY BlockId DESC
			);

	SELECT @CounterPartyId
		,@TransactTimePMsg;
END