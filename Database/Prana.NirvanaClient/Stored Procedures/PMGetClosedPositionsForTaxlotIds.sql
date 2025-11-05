
/*    
  
Authosr: Narendra Kumar Jangir  
Date: 2013-10-10  
Description: Get positions for taxlotids 
    
*/
CREATE PROCEDURE [dbo].[PMGetClosedPositionsForTaxlotIds] (@TaxlotIds VARCHAR(MAX))
AS
BEGIN
	CREATE TABLE #TaxlotIds (TaxlotID VARCHAR(50))

	INSERT INTO #TaxlotIds
	SELECT Items AS TaxlotID
	FROM dbo.Split(@TaxlotIds, ',')

	CREATE TABLE #SecurityMasterTemp (
		TickerSymbol VARCHAR(50)
		,Multiplier FLOAT
		,LeadCurrencyID INT
		,VsCurrencyID INT
		)

	INSERT INTO #SecurityMasterTemp
	SELECT TickerSymbol
		,Multiplier
		,LeadCurrencyID
		,VsCurrencyID
	FROM V_SecMasterData

	SELECT *
	INTO #PM_TaxLotClosing
	FROM PM_TaxlotClosing
	WHERE PositionalTaxlotId IN (
			SELECT TaxlotID
			FROM #TaxlotIds
			)
		OR ClosingTaxlotId IN (
			SELECT TaxlotID
			FROM #TaxlotIds
			)

	CREATE TABLE #PM_Taxlots (
		Symbol VARCHAR(50)
		,OrderSideTagValue CHAR(1)
		,AvgPrice FLOAT
		,FundID INT
		,Level2ID INT
		,ClosedTotalCommissionandFees FLOAT
		,PositionTag INT
		,TaxLotClosingId_Fk UNIQUEIDENTIFIER
		,TaxlotID VARCHAR(50)
		,GroupID VARCHAR(50)
		)

	INSERT INTO #PM_Taxlots
	SELECT Symbol
		,OrderSideTagValue
		,AvgPrice
		,FundID
		,Level2ID
		,ClosedTotalCommissionandFees
		,PositionTag
		,TaxLotClosingId_Fk
		,TaxlotID
		,GroupID
	FROM PM_Taxlots

	SELECT DISTINCT PTC.PositionalTaxlotID
		,PTC.ClosingTaxlotID
		,PT.Symbol AS Symbol
		,PT.OrderSideTagValue AS PositionSideID
		,PT1.OrderSideTagValue AS ClosingSideID
		,G.ProcessDate AS PositionTradeDate
		,PTC.AUECLocalDate AS ClosingTradeDate
		,--now closing taxlot Trade date is cloisng date                                                    
		PTC.OpenPrice AS OpenPrice
		,PTC.ClosePrice AS ClosingPrice
		,PT.FundID AS FundID
		,PT.Level2ID AS Level2ID
		,G.AssetID
		,G.UnderLyingID
		,G.ExchangeID
		,G.CurrencyID
		,PT.ClosedTotalCommissionandFees AS PositionalTaxlotCommission
		,PT1.ClosedTotalCommissionandFees AS ClosingTaxlotCommission
		,PTC.ClosingMode AS ClosingMode
		,SM.Multiplier AS Multiplier
		,PT.PositionTag AS OpeiningPositionTag
		,PT1.PositionTag AS ClosingPositionTag
		,PTC.ClosedQty
		,isnull(SW.NotionalValue * ((PTC.ClosedQty) / G.CumQty), 0) AS PositionNotionalValue
		,isnull(SW.BenchMarkRate, 0) AS PositionBenchMarkRate
		,isnull(SW.Differential, 0) AS PositionDifferential
		,isnull(SW.OrigCostBasis, 0) AS PositionOrigCostBasis
		,isnull(SW.DayCount, 0) AS PositionDayCount
		,SW.SwapDescription AS PositionalSwapDescription
		,SW.FirstResetDate AS PositionFirstResetDate
		,SW.OrigTransDate AS PositionOrigTransDate
		,isnull(SW1.NotionalValue * ((PTC.ClosedQty) / G1.CumQty), 0) AS NotionalValue
		,isnull(SW1.BenchMarkRate, 0) AS BenchMarkRate
		,isnull(SW1.Differential, 0) AS Differential
		,isnull(SW1.OrigCostBasis, 0) AS OrigCostBasis
		,isnull(SW1.DayCount, 0) AS DayCount
		,SW1.SwapDescription AS ClosingSwapDescription
		,SW1.FirstResetDate AS FirstResetDate
		,SW1.OrigTransDate AS OrigTransDate
		,G.IsSwapped
		,G1.IsSwapped
		,PT.TaxLotClosingId_Fk
		,PTC.PositionSide
		,PTC.ClosingAlgo
		,SM.LeadCurrencyID
		,SM.VsCurrencyID
	FROM #PM_TaxlotClosing PTC
	INNER JOIN #PM_Taxlots PT ON (
			PTC.PositionalTaxlotID = PT.TaxlotID
			AND PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk
			)
	INNER JOIN #PM_Taxlots PT1 ON (
			PTC.ClosingTaxlotID = PT1.TaxlotID
			AND PTC.TaxLotClosingId = PT1.TaxLotClosingId_Fk
			)
	INNER JOIN T_Group G ON G.GroupID = PT.GroupID
	INNER JOIN T_Group G1 ON G1.GroupID = PT1.GroupID
	INNER JOIN T_AUEC AUEC ON AUEC.AUECID = G.AUECID
	LEFT OUTER JOIN #SecurityMasterTemp SM ON PT.Symbol = SM.TickerSymbol
	LEFT OUTER JOIN T_SwapParameters SW ON SW.GroupID = G.GroupID
	LEFT OUTER JOIN T_SwapParameters SW1 ON SW1.GroupID = G1.GroupID

	DROP TABLE #SecurityMasterTemp
		,#PM_TaxlotClosing
		,#PM_Taxlots
END
