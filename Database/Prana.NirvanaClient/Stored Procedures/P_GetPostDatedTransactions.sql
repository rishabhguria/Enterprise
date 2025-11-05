CREATE PROCEDURE [dbo].[P_GetPostDatedTransactions] (
  @ToAllAUECDatesString VARCHAR(MAX)
  ,@ReconDateType INT = 1
)
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
	SELECT PT.TaxLotID AS TaxLotID
		,G.AUECLocalDate AS TradeDate
		,PT.OrderSideTagValue AS SideID
		,-- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS
		PT.Symbol AS Symbol
		,PT.TaxLotOpenQty AS OpenQuantity
		,PT.AvgPrice AS AvgPX
		,PT.FundID AS FundID
		,G.AssetID AS AssetID
		,G.UnderLyingID AS UnderLyingID
		,G.ExchangeID AS ExchangeID
		,G.CurrencyID AS CurrencyID
		,G.AUECID AS AUECID
		,PT.OpenTotalCommissionandFees AS TotalCommissionandFees
		,--this is open commission and closed commission sum is not necessarily equals to total commission                                                     
		isnull(V_SecMasterData.Multiplier, 1) AS Multiplier
		,G.SettlementDate AS SettlementDate
		,V_SecMasterData.LeadCurrencyID
		,V_SecMasterData.VsCurrencyID
		,isnull(V_SecMasterData.ExpirationDate, '1/1/1800') AS ExpirationDate
		,G.Description AS Description
		,PT.Level2ID AS Level2ID
		,isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.Quantity), 0) AS NotionalValue
		,isnull(SW.BenchMarkRate, 0) AS BenchMarkRate
		,isnull(SW.Differential, 0) AS Differential
		,isnull(SW.OrigCostBasis, 0) AS OrigCostBasis
		,isnull(SW.DayCount, 0) AS DayCount
		,isnull(SW.SwapDescription, '') AS SwapDescription
		,SW.FirstResetDate AS FirstResetDate
		,SW.OrigTransDate AS OrigTransDate
		,G.IsSwapped AS IsSwapped
		,PT.AUECModifiedDate AS AUECLocalDate
		,G.GroupID
		,PT.PositionTag
		,G.FXRate
		,G.FXConversionMethodOperator
		,isnull(V_SecMasterData.CompanyName, '') AS CompanyName
		,isnull(V_SecMasterData.UnderlyingSymbol, '') AS UnderlyingSymbol
		,IsNull(V_SecMasterData.Delta, 1) AS Delta
		,IsNull(V_SecMasterData.PutOrCall, '') AS PutOrCall
		,G.IsPreAllocated
		,G.CumQty
		,G.AllocatedQty
		,PT.TaxLotOpenQty
		,PT.taxlot_Pk
		,PT.ParentRow_Pk
		,IsNull(V_SecMasterData.StrikePrice, 0) AS StrikePrice
		,G.UserID
		,G.CounterPartyID
		,CATaxlots.CorpActionID
		,V_SecMasterData.Coupon
		,V_SecMasterData.IssueDate
		,V_SecMasterData.MaturityDate
		,V_SecMasterData.FirstCouponDate
		,V_SecMasterData.CouponFrequencyID
		,V_SecMasterData.AccrualBasisID
		,V_SecMasterData.BondTypeID
		,V_SecMasterData.IsZero
		,V_SecMasterData.IsNDF
		,V_SecMasterData.FixingDate
		,V_SecMasterData.UnderlyingDelta
		,V_SecMasterData.IDCOSymbol
		,V_SecMasterData.OSISymbol
		,V_SecMasterData.SEDOLSymbol
		,V_SecMasterData.CUSIPSymbol
		,V_SecMasterData.BloombergSymbol
		,V_SecMasterData.ISINSymbol
		,G.OriginalPurchaseDate
		,G.ProcessDate
		,PT.TradeAttribute1
		,PT.TradeAttribute2
		,PT.TradeAttribute3
		,PT.TradeAttribute4
		,PT.TradeAttribute5
		,PT.TradeAttribute6
		,isnull(V_SecMasterData.ProxySymbol, '') AS ProxySymbol
		,G.TransactionType
		,V_SecMasterData.ReutersSymbol
		,G.InternalComments
		,V_SecMasterData.IsCurrencyFuture
		,V_UDA_DynamicUDA.*
		,MF.MasterFundName AS MasterFund
		,PT.LotId  
		,PT.ExternalTransId 		
		,V_SecMasterData.AssetName  
		,V_SecMasterData.SecurityTypeName  
		,V_SecMasterData.SectorName  
		,V_SecMasterData.SubSectorName  
		,V_SecMasterData.CountryName  
		,V_SecMasterData.BBGID
		,L2.TaxLotQty AS ExecutedQty
		,COALESCE(PTCUR.CurrencySymbol, GCUR.CurrencySymbol, 'None') AS SettlCurrency  
		,V_SecMasterData.Symbol_PK
		,G.VenueID
		,G.OrderTypeTagValue
		,V_SecMasterData.FactSetSymbol
		,V_SecMasterData.ActivSymbol
		,V_SecMasterData.BloombergSymbolWithExchangeCode
		,PT.AdditionalTradeAttributes
	FROM PM_Taxlots PT
	
	INNER JOIN T_Group G ON G.GroupID = PT.GroupID
	INNER JOIN T_Level2Allocation L2 ON L2.TaxLotID = PT.TaxLotID
	LEFT JOIN T_SwapParameters SW ON G.GroupID = SW.GroupID
	LEFT JOIN V_SecMasterData ON PT.Symbol = V_SecMasterData.TickerSymbol
	LEFT JOIN PM_CorpActionTaxlots CATaxlots ON PT.Taxlot_PK = CATaxlots.FKId
	LEFT JOIN V_UDA_DynamicUDA ON V_SecMasterData.Symbol_PK = V_UDA_DynamicUDA.Symbol_PK
	LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = PT.FundID  
	LEFT OUTER JOIN T_companyMasterFunds MF  ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID
	LEFT OUTER JOIN T_Currency PTCUR  ON PT.SettlCurrency = PTCUR.CurrencyID  
	LEFT OUTER JOIN T_Currency GCUR  ON G.SettlCurrency = GCUR.CurrencyID 
	WHERE taxlot_PK IN (
			SELECT max(taxlot_PK)
			FROM PM_Taxlots
			INNER JOIN T_Group G ON G.GroupID = PM_Taxlots.GroupID
			INNER JOIN T_AUEC AUEC ON AUEC.AUECID = G.AUECID
			INNER JOIN @AUECDatesTable AUECDates ON AUEC.AUECID = AUECDates.AUECID
            WHERE 
            --Modified by: Kashish Goyal, if @ReconDateType 0 then consider Trade Date , 1 then consider Process Date,2 then consider Prana Process Date
            (  
             (
	          @ReconDateType = '0'
	          AND Datediff(d, G.AUECLocalDate, AUECDates.CurrentAUECDate) < 0
	         )
	         OR (      
                 @ReconDateType = '1'      
                 AND Datediff(d, PM_Taxlots.AUECModifiedDate, AUECDates.CurrentAUECDate) < 0
	             )
	         OR (      
                 @ReconDateType = '2'      
                 AND Datediff(d, G.NirvanaProcessDate, AUECDates.CurrentAUECDate) < 0 
	            )
	            )
			GROUP BY taxlotid
			)
		AND TaxLotOpenQty <> 0

	RETURN;
END