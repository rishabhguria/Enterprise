CREATE PROCEDURE P_GetBlockAllocationDetails @thirdPartyBatchId INT
	,@runDate DATETIME
AS
BEGIN
	WITH RankedAllocations
	AS (
		SELECT AB.BlockId
			,AB.TransactTime
			,C.CurrencySymbol
			,AB.Symbol
			,AB.ISIN
			,AB.Sedol
			,AB.Cusip
			,AB.GrossTradeAmt
			,AB.SideID
			,AB.Quantity
			,AB.TradeDate
			,AB.SettlDate
			,AB.AvgPx
			,AB.Commission AS BlockCommission
			,AB.NetMoney AS BlockNetMoney
			,AB.AllocStatus
			,AB.AllocID
			,AB.SubStatus
			,AB.AllocReportId
			,AB.MsgType
			,ROW_NUMBER() OVER (
				PARTITION BY AB.AllocID
				,AB.BlockId ORDER BY AB.BlockId DESC
				) AS RowNum
		FROM T_ThirdPartyDailyJobs DJ
		INNER JOIN T_ThirdPartyAllocationBlocks AB ON DJ.JobId = AB.JobId
		LEFT JOIN T_Currency C ON C.CurrencyId = AB.Currency
		WHERE DJ.ThirdPartyBatchId = @thirdPartyBatchId
			AND CAST(DJ.BatchRunDate AS DATE) = CAST(@runDate AS DATE)
			AND AB.MsgType = 'J'
		)
	SELECT *
	INTO #JBlocks
	FROM RankedAllocations
	WHERE RowNum = 1;

	SELECT AB.BlockId
		,AB.TransactTime
		,J.CurrencySymbol
		,AB.Symbol
		,AB.ISIN
		,AB.Sedol
		,AB.Cusip
		,AB.GrossTradeAmt
		,AB.SideID
		,AB.Quantity
		,AB.TradeDate
		,AB.SettlDate
		,AB.AvgPx
		,AB.Commission AS BlockCommission
		,AB.NetMoney AS BlockNetMoney
		,AB.AllocStatus
		,AB.AllocID
		,AB.SubStatus
		,AB.AllocReportId
		,AB.MsgType
		,J.BlockId AS JBlockId
	INTO #FilteredBlocks
	FROM T_ThirdPartyAllocationBlocks AB
	JOIN #JBlocks J ON AB.JBlockId = J.BlockId
	WHERE AB.MsgType IN (
			'J'
			,'AK'
			,'AS'
			)

	SELECT *
	FROM #FilteredBlocks
	WHERE MsgType = 'J'
	ORDER BY AllocID
		,BlockId

	SELECT AM.MsgId
		,AM.AllocAccount
		,AM.Commission AS AllocationCommission
		,AM.AllocQty
		,AM.[Misc Fees]
		,AM.AllocAvgPx
		,AM.NetMoney AS AllocationNetMoney
		,AM.MatchStatus
		,AM.IndividualAllocID
		,FB.JBlockId AS BlockId
		,FB.MsgType
	FROM T_ThirdPartyAllocationMessages AM
	INNER JOIN #FilteredBlocks FB ON AM.BlockId = FB.BlockId
	ORDER BY FB.BlockId
		,AM.IndividualAllocID

	DROP TABLE #FilteredBlocks;

	DROP TABLE #JBlocks;
END