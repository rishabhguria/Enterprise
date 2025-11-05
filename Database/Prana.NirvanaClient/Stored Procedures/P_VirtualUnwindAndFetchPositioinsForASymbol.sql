
/*                                                          
Author: Narendra Kumar Jangir  
Description: Virtually unwind closing and fetch open positions for a symbol
                                                        
Usage:  
EXEC P_VirtualUnwindAndFetchPositioinsForASymbol '8B601g60-1399-411D-997C-9D04C27BFD08,'                                                          
*/
CREATE PROCEDURE [dbo].[P_VirtualUnwindAndFetchPositioinsForASymbol] (@TaxLotClosingIDString VARCHAR(MAX))
AS
BEGIN

	SELECT *
	INTO #Temp_TaxlotClosingIDs
	FROM dbo.split(@TaxLotClosingIDString, ',')

	CREATE TABLE #Temp_GroupIDs (GroupID VARCHAR(50))

	CREATE TABLE #PM_Taxlots (
		Taxlot_pk BIGINT
		,TaxlotClosingID_fk UNIQUEIDENTIFIER
		,TaxlotID VARCHAR(MAX)
		,GroupID VARCHAR(100)
		,Symbol VARCHAR(50)
		,FundId INT
		,AuecModifiedDate DATETIME
		)

	--insert taxlot details for which ClosingTaxlotId is passed to SP               
	INSERT INTO #PM_Taxlots (
		Taxlot_pk
		,TaxlotClosingID_fk
		,TaxlotID
		,GroupID
		,Symbol
		,FundId
		,AuecModifiedDate
		)
	SELECT Taxlot_pk
		,TaxlotClosingID_fk
		,TaxlotID
		,GroupID
		,Symbol
		,FundId
		,AuecModifiedDate
	FROM PM_Taxlots PM
	INNER JOIN #Temp_TaxlotClosingIDs TEMP ON TEMP.Items = PM.TaxlotClosingId_fk

	--group data by fund.symbol,groupid,taxlotid
	SELECT MIN(Taxlot_pk) AS Taxlot_pk
		,TaxlotID
		,GroupID
		,Symbol
		,FundId
		,MIN(AuecModifiedDate) AS AuecModifiedDate
	INTO #PM_Taxlots_Grouped
	FROM #PM_Taxlots
	GROUP BY FundId
		,Symbol
		,GroupID
		,TaxlotID

	CREATE TABLE #PM_TaxlotClosing (
		PositionalTaxlotID VARCHAR(MAX)
		,ClosingTaxlotID VARCHAR(MAX)
		,TaxlotClosingID UNIQUEIDENTIFIER
		,Closingmode INT
		,ClosedQty FLOAT
		)

	--insert taxlotclosingid with same symbol and fundid which have closing in future date.            
	INSERT INTO #PM_TaxlotClosing (
		PositionalTaxlotID
		,ClosingTaxlotID
		,TaxlotClosingID
		,Closingmode
		,ClosedQty
		)
	SELECT positionalTaxlotID
		,ClosingTaxlotID
		,TaxlotClosingID
		,Closingmode
		,ClosedQty
	FROM PM_TaxLotClosing PTC
	INNER JOIN #PM_Taxlots PM ON PTC.TaxlotClosingID = PM.TaxlotClosingID_FK

	IF EXISTS (
			SELECT TaxlotClosingID
			FROM #PM_TaxlotClosing PMC
			WHERE (
					PMC.Closingmode <> 0
					AND PMC.Closingmode <> 7
					)
			)
	BEGIN
		---Mukul// fetching all the Exercised underlying ..Need a better way as right now the only way we can       
		--identify this is thorugh T_group where TaxlotclosingID for a particular Exercise/Assignment is set in Pm_TaxlotclosingID_fk field (in T_group) with underlying generated..      
		INSERT INTO #Temp_GroupIDs (GroupID)
		SELECT DISTINCT (GroupID)
		FROM T_group
		INNER JOIN #PM_TaxlotClosing PMC ON T_group.TaxlotClosingID_fk = PMC.taxlotClosingID
		WHERE PMC.Closingmode <> 0
			AND PMC.Closingmode <> 7
		--Mukul// fetching all the expired transactions..This can be handled using transaction type (Expiry)..      
		
		UNION
		
		SELECT DISTINCT (#PM_Taxlots.GroupID)
		FROM #PM_Taxlots
		INNER JOIN #PM_TaxlotClosing PMC ON PMC.ClosingTaxlotID = #PM_Taxlots.TaxlotID
		WHERE PMC.Closingmode <> 0
			AND PMC.Closingmode <> 7
	END

	--taxlot with second max Taxlot_pk will be picked in order to fetch positions after unwinding       
	SELECT PM.TaxLotID AS TaxLotID
		,G.AUECLocalDate AS TradeDate
		,G.ProcessDate
		,G.OriginalPurchaseDate
		,PM.OrderSideTagValue AS SideID
		,-- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS
		 PM.Symbol AS Symbol
		,PM.TaxLotOpenQty AS TaxLotOpenQty
		,PM.AvgPrice AS AvgPrice
		,PM.FundID AS FundID
		,G.AssetID AS AssetID
		,G.UnderLyingID AS UnderLyingID
		,G.ExchangeID AS ExchangeID
		,G.CurrencyID AS CurrencyID
		,G.AUECID AS AUECID
		,PM.OpenTotalCommissionandFees
		,--this is open commission and closed commission sum is not necessarily equals to total commission                                                        
		isnull(V_SecMasterData.Multiplier, 1) AS Multiplier
		,G.SettlementDate AS SettlementDate
		,NULL
		,NULL
		,isnull(V_SecMasterData.ExpirationDate, '1/1/1800') AS ExpirationDate
		,G.Description AS Description
		,PM.Level2ID AS Level2ID
		,isnull((PM.TaxLotOpenQty * SW.NotionalValue / G.Quantity), 0) AS NotionalValue
		,isnull(SW.BenchMarkRate, 0) AS BenchMarkRate
		,isnull(SW.Differential, 0) AS Differential
		,isnull(SW.OrigCostBasis, 0) AS OrigCostBasis
		,isnull(SW.DayCount, 0) AS DayCount
		,isnull(SW.SwapDescription, '') AS SwapDescription
		,SW.FirstResetDate AS FirstResetDate
		,SW.OrigTransDate AS OrigTransDate
		,G.IsSwapped AS IsSwapped
		,G.AUECLocalDate AS AUECLocalDate
		,G.GroupID
		,PM.PositionTag
		,G.FXRate
		,G.FXConversionMethodOperator
		,isnull(V_SecMasterData.CompanyName, '') AS CompanyName
		,isnull(V_SecMasterData.UnderlyingSymbol, '') AS UnderlyingSymbol
		,IsNull(V_SecMasterData.Delta, 1) AS Delta
		,IsNull(V_SecMasterData.PutOrCall, '') AS PutOrCall
		,G.IsPreAllocated
		,G.CumQty
		,G.AllocatedQty
		,G.Quantity
		,PM.taxlot_Pk
		,PM.ParentRow_Pk
		,V_SecMasterData.StrikePrice
		,isnull(V_SecMasterData.IsNDF, 0) AS IsNDF
		,isnull(V_SecMasterData.FixingDate, '1/1/1800') AS FixingDate
		,V_SecMasterData.LeadCurrencyID
		,V_SecMasterData.VsCurrencyID
		,PM.LotId
		,PM.ExternalTransId
		,PM.TradeAttribute1
		,PM.TradeAttribute2
		,PM.TradeAttribute3
		,PM.TradeAttribute4
		,PM.TradeAttribute5
		,PM.TradeAttribute6
		,L2.TaxLotQty AS ExecutedQty
		,G.TransactionType
		,G.InternalComments
		,G.TradingAccountId
		,G.UserID
		,G.NirvanaProcessDate
		,G.SettlCurrency
        ,ISNULL(V_SecMasterData.BloombergSymbol,'') AS BloombergSymbol
        ,ISNULL(V_SecMasterData.SEDOLSymbol,'') AS SEDOLSymbol
		,ISNULL(V_SecMasterData.CUSIPSymbol,'') AS CUSIPSymbol
        ,ISNULL(V_SecMasterData.ISINSymbol,'') AS ISINSymbol
		,ISNULL(V_SecMasterData.SecurityTypeName,'') AS SecurityTypeName
	    ,ISNULL(V_SecMasterData.CountryName,'') AS CountryName
	    ,ISNULL(V_SecMasterData.ProxySymbol,'') AS ProxySymbol
	    ,ISNULL(V_SecMasterData.IDCOSymbol,'') AS IDCOSymbol
	    ,ISNULL(V_SecMasterData.OSISymbol,'') AS OSISymbol
	    ,ISNULL(V_SecMasterData.SectorName,'') AS SectorName
	    ,ISNULL(V_SecMasterData.SubSectorName,'') AS SubSectorName
	    ,ISNULL(V_SecMasterData.ReutersSymbol,'') AS RIC
	    ,ISNULL(V_SecMasterData.AssetName,'') AS UserAsset    
		,PM.AdditionalTradeAttributes
	FROM PM_Taxlots PM
	INNER JOIN T_Level2Allocation L2 ON L2.TaxLotID = PM.TaxLotID
	INNER JOIN T_Group G ON G.GroupID = PM.GroupID
	LEFT OUTER JOIN T_SwapParameters SW ON G.GroupID = SW.GroupID
	LEFT OUTER JOIN V_SecMasterData ON PM.Symbol = V_SecMasterData.TickerSymbol
	WHERE PM.TaxLot_PK IN (
			SELECT max(PT.taxlot_PK)
			FROM PM_Taxlots PT
			INNER JOIN #PM_Taxlots_Grouped PTG ON PT.TaxLotID = PTG.TaxLotID
				AND PT.TaxLot_PK < PTG.Taxlot_pk
			GROUP BY PT.TaxLotID
			)
		AND PM.GroupID NOT IN (
			SELECT GroupID
			FROM #Temp_GroupIDs
			)

	--drop tables  
	DROP TABLE #Temp_TaxlotClosingIDs
		,#PM_Taxlots
		,#PM_Taxlots_Grouped
		,#PM_TaxlotClosing
		,#Temp_GroupIDs
END
