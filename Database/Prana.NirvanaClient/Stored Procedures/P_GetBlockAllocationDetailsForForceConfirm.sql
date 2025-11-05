CREATE PROCEDURE P_GetBlockAllocationDetailsForForceConfirm @thirdPartyAllocId NVARCHAR(50)
AS
DECLARE @blockId INT

SET @blockId = (
		SELECT TOP 1 BlockId
		FROM T_ThirdPartyAllocationBlocks
		WHERE AllocID = @thirdPartyAllocId
			AND MsgType IN (
				'AK'
				,'AS'
				)
		ORDER BY BlockId DESC
		);

SELECT AM.AllocAccount
	,AB.Symbol
	,AB.SideID
	,AM.AllocQty
	,AM.AllocAvgPx
	,AM.Commission
	,AM.[Misc Fees]
	,AM.NetMoney
	,AB.TradeDate
	,AM.IndividualAllocID
	,AM.MatchStatus
	,AM.ConfirmStatus
	,AM.BlockId
	,AB.MsgType
	,AB.AllocReportId
FROM T_ThirdPartyAllocationBlocks AB
INNER JOIN T_ThirdPartyAllocationMessages AM ON AB.BlockId = AM.BlockId
WHERE AM.BlockId = @blockId
	AND (
		(
			AB.MsgType = 'AK'
			AND AM.ConfirmStatus = '5'
			)
		OR (
			AB.MsgType = 'AS'
			AND AB.SubStatus IN (
				'4'
				,'9'
				,'10'
				,'11'
				,'12'
				,'13'
				,'18'
				,'23'
				)
			)
		)
	AND (
		(
			AB.MsgType = 'AK'
			AND AM.MatchStatus NOT IN (
				'20'
				,'21'
				,'22'
				)
			OR AB.MsgType = 'AS'
			)
		)