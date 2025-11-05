CREATE PROCEDURE [dbo].[P_GetRequiredTaxlotsForAU] @allocIds NVARCHAR(MAX)
AS
BEGIN
	DECLARE @AllocID_TV TABLE (AllocId NVARCHAR(100))

	INSERT INTO @AllocID_TV (AllocId)
	SELECT ITEMS
	FROM dbo.Split(@allocIds, ',');

	DECLARE @BlockId_TV TABLE (
		BlockId INT
		,AllocId NVARCHAR(100)
		,TransactTime DATETIME
		)

	INSERT INTO @BlockId_TV
	SELECT BlockId
		,AB.AllocID
		,AB.TransactTime
	FROM T_ThirdPartyAllocationBlocks AB
	JOIN @AllocID_TV TV ON AB.AllocID = TV.AllocId
	WHERE AB.MsgType = 'AK';

	UPDATE @BlockId_TV
	SET TransactTime = AB.TransactTime
	FROM T_ThirdPartyAllocationBlocks AB
	JOIN @BlockId_TV BID ON AB.AllocID = BID.AllocId
	WHERE AB.MsgType = 'P';

	DECLARE @AKMsg_TV TABLE (
		ConfirmId VARCHAR(MAX)
		,ConfirmStatus VARCHAR(1)
		,TradeDate DATETIME
		,AllocId NVARCHAR(100)
		,TransactTime DATETIME
		)

	INSERT INTO @AKMsg_TV
	SELECT AM.ConfirmId
		,AM.ConfirmStatus
		,AM.TradeDate
		,BID.AllocID
		,BID.TransactTime
	FROM T_ThirdPartyAllocationMessages AM
	JOIN @BlockId_TV BID ON AM.BlockId = BID.BlockId
	WHERE AM.MatchStatus NOT IN (
			'5'
			,'6'
			,'15'
			,'16'
			,'20'
			,'21'
			,'22'
			);

	SELECT *
	FROM @AKMsg_TV
END