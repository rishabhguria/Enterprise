
/*--=====================================================================================              
Modified by: Om shiv      
Date : Nov, 2013      
Discription: Added UDA columns with secMaster Info      
      
      
-- Author  : Rajat                      
-- Description  : Used for fetching the unallocated open trades for the given datestring.        
      
-- Modified by: Rahul gupta      
-- Description: http://jira.nirvanasolutions.com:8080/browse/QUAD-48                     
--=====================================================================================*/
CREATE PROCEDURE [dbo].[P_GetOpenUnallocatedTrades] (@ToAllAUECDatesString VARCHAR(MAX))
AS
BEGIN
	DECLARE @AUECDatesTable TABLE (
		AUECID INT
		,CurrentAUECDate DATETIME
		)

	INSERT INTO @AUECDatesTable
	SELECT *
	FROM dbo.GetAllAUECDatesFromString(@ToAllAUECDatesString)

	-- Get UTC Date. UTC Date is stored corresponding to AUECID 0 in @AUECDatesTable                                                                                                                                                                        
	SELECT G.GroupID AS TaxLotID
		,G.AUECLocalDate AS TradeDate
		,G.OrderSideTagValue AS SideID
		,-- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS                                                                                                                                                            
		G.Symbol AS Symbol
		,G.CumQty AS OpenQuantity
		,G.AvgPrice AS AvgPX
		,- 1 AS FundID
		,-- Fundid = -1 for unallocated trade.                      
		G.AssetID AS AssetID
		,G.UnderLyingID AS UnderLyingID
		,G.ExchangeID AS ExchangeID
		,G.CurrencyID AS CurrencyID
		,G.AUECID AS AUECID
		,0 AS TotalCommissionandFees
		,--For Unallocated trade it is 0                      
		isnull(V_SecMasterData.Multiplier, 1) AS Multiplier
		,G.SettlementDate AS SettlementDate
		,V_SecMasterData.LeadCurrencyID
		,V_SecMasterData.VsCurrencyID
		,isnull(V_SecMasterData.ExpirationDate, '1/1/1800') AS ExpirationDate
		,G.Description AS Description
		,- 1 AS Level2ID
		,-- -1 for unallocated trade                      
		isnull((G.CumQty * SW.NotionalValue / G.Quantity), 0) AS NotionalValue
		,-- Check                                                                                                                        
		isnull(SW.BenchMarkRate, 0) AS BenchMarkRate
		,isnull(SW.Differential, 0) AS Differential
		,isnull(SW.OrigCostBasis, 0) AS OrigCostBasis
		,isnull(SW.DayCount, 0) AS DayCount
		,isnull(SW.SwapDescription, '') AS SwapDescription
		,SW.FirstResetDate AS FirstResetDate
		,SW.OrigTransDate AS OrigTransDate
		,G.IsSwapped AS IsSwapped
		,G.AUECLocalDate AS AUECLocalDate
		,G.GroupID
		,2 AS PositionTag
		,-- 2 Represents PositionTag = None                        
		G.FXRate
		,G.FXConversionMethodOperator
		,isnull(V_SecMasterData.CompanyName, '') AS CompanyName
		,isnull(V_SecMasterData.UnderlyingSymbol, '') AS UnderlyingSymbol
		,IsNull(V_SecMasterData.Delta, 1) AS Delta
		,IsNull(V_SecMasterData.PutOrCall, '') AS PutOrCall
		,G.IsPreAllocated
		,
		--G.CumQty,    --Already selecting it as an OpenQuantity above                                                            
		G.AllocatedQty
		,G.Quantity AS TargetQuantity
		,- 1 AS taxlot_Pk
		,-- Since no entry in PM_taxlots                      
		- 1 AS ParentRow_Pk
		,-- Since no entry in PM_taxlots                      
		IsNull(V_SecMasterData.StrikePrice, 0) AS StrikePrice
		,G.UserID
		,G.CounterPartyID
		,dbo.GetEmptyGUID() AS CorpActionID
		,-- As unallocated hence corpaction can not be applied                      
		V_SecMasterData.Coupon
		,V_SecMasterData.IssueDate
		,V_SecMasterData.MaturityDate
		,V_SecMasterData.FirstCouponDate
		,V_SecMasterData.CouponFrequencyID
		,V_SecMasterData.AccrualBasisID
		,V_SecMasterData.BondTypeID
		,V_SecMasterData.IsZero
		,V_SecMasterData.IsNDF
		,V_SecMasterData.FixingDate
		,V_SecmasterData.UnderlyingDelta
		,V_SecMasterData.IDCOSymbol
		,V_SecMasterData.OSISymbol
		,V_SecMasterData.SEDOLSymbol
		,V_SecMasterData.CUSIPSymbol
		,V_SecMasterData.BloombergSymbol
		,V_SecMasterData.ISINSymbol
		,G.OriginalPurchaseDate
		,G.ProcessDate
		,G.TradeAttribute1
		,G.TradeAttribute2
		,G.TradeAttribute3
		,G.TradeAttribute4
		,G.TradeAttribute5
		,G.TradeAttribute6
		,V_SecMasterData.ProxySymbol
		,
		--Added UDA fields by OMshiv, Nov 2013      
		V_SecMasterData.AssetName
		,V_SecMasterData.SecurityTypeName
		,V_SecMasterData.SectorName
		,V_SecMasterData.SubSectorName
		,V_SecMasterData.CountryName
		,
		----Sandeep on NOV 12 2014    
		G.TransactionType
		,V_SecMasterData.ReutersSymbol
		,CASE 
			WHEN (
					G.CumQty = 0
					OR V_SecMasterData.Multiplier = 0
					)
				THEN 0
			ELSE (IsNull((G.CumQty * IsNull(G.AvgPrice, 0) * V_SecMasterData.Multiplier), 0)) / (G.CumQty * V_SecMasterData.Multiplier)
			END AS UnitCost
		,G.SecFee AS SecFees
		,G.InternalComments
		,V_SecMasterData.IsCurrencyFuture
		,V_UDA_DynamicUDA.*
		,G.VenueID
		,G.OrderTypeTagValue
		,V_SecMasterData.FactSetSymbol
		,V_SecMasterData.ActivSymbol
		,V_SecMasterData.BloombergSymbolWithExchangeCode
		,G.AdditionalTradeAttributes
	FROM T_Group G WITH (nolock)
	INNER JOIN @AUECDatesTable AUECDates ON AUECDates.AUECID = G.AUECID
	LEFT OUTER JOIN T_SwapParameters SW ON G.GroupID = SW.GroupID
	LEFT OUTER JOIN V_SecMasterData ON G.Symbol = V_SecMasterData.TickerSymbol
	LEFT OUTER JOIN V_UDA_DynamicUDA ON V_SecMasterData.Symbol_PK = V_UDA_DynamicUDA.Symbol_PK
	WHERE G.StateID = 1
		AND G.CumQty > 0

	RETURN;
END
