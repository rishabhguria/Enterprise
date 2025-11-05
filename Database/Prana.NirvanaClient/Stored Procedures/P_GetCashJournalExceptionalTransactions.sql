/* Description :                                 
Author: Sandeep Singh      
Date: July 27,2012      
Description: It returns allocated transactions for the supplied taxlotids.         
exec P_GetTransactionsById '120620135141011820,120620135141011830,120626112534011820,120629124251011820,120726161240011820'      
      
Updated-- Gaurav Dhariwal      
Description: It returns allocated transactions which does not exists in T_Journal for the supplied date range.    Updated-- Narendra Kumar Jangir Date: Sept 06,2013   Description: It returns allocated trading transactions which does not exists in T_Journa
l for the supplied date range.   Issue fixed when a taxlot have corporate action applied and does not have trading journal then it was skipped. http://jira.nirvanasolutions.com:8080/browse/PRANA-2409     

updated: Bharat raturi, 1 aug 2014
add the fundIDs to get the data for selected funds only

*/
CREATE PROCEDURE [P_GetCashJournalExceptionalTransactions] (
	@startDate DATETIME
	,@endDate DATETIME
	,@fundIDs VARCHAR(max)
	)
AS
BEGIN
	--added by: Bharat raturi      
	CREATE TABLE #funds (fundID INT)

	INSERT INTO #funds
	SELECT Items
	FROM dbo.Split(@fundIDs, ',')

	SELECT TaxlotID
		,PositionTag
	INTO #TempPosTag
	FROM PM_Taxlots
	WHERE TaxLot_PK IN (
			SELECT Min(TaxLot_PK)
			FROM PM_Taxlots
			GROUP BY TaxlotID
			)

	CREATE TABLE #V_Taxlots (
		TaxLotID VARCHAR(50)
		,OrderSideTagValue CHAR(1)
		,TotalExpenses FLOAT
		,Level2ID INT
		,TaxLotQty FLOAT
		,AvgPrice FLOAT
		,Commission FLOAT
		,OtherBrokerFees FLOAT
		,ClearingFee FLOAT
		,MiscFees FLOAT
		,StampDuty FLOAT
		,TransactionLevy FLOAT
		,TaxOnCommissions FLOAT
		,AUECLocalDate DATETIME
		,OriginalPurchaseDate DATETIME
		,ProcessDate DATETIME
		,AssetID INT
		,UnderLyingID INT
		,ExchangeID INT
		,CurrencyID INT
		,AUECID INT
		,SettlementDate DATETIME
		,Description VARCHAR(max)
		,AllocationDate DATETIME
		,IsSwapped BIT
		,GroupID VARCHAR(50)
		,FXRate FLOAT
		,FXConversionMethodOperator VARCHAR(3)
		,IsPreAllocated BIT
		,CumQty FLOAT
		,AllocatedQty FLOAT
		,Quantity FLOAT
		,UserID INT
		,CounterPartyID INT
		,FundID INT
		,symbol VARCHAR(50)
		,SideMultiplier INT
		,Side VARCHAR(50)
		,UnderlyingDelta FLOAT
		,SecFee FLOAT
		,OccFee FLOAT
		,OrfFee FLOAT
		,ClearingBrokerFee FLOAT
		,SoftCommission FLOAT
		,OptionPremiumAdjustment FLOAT
		,TransactionSource INT
		,AccruedInterest FLOAT
		)

	INSERT INTO #V_Taxlots (
		TaxLotID
		,OrderSideTagValue
		,TotalExpenses
		,Level2ID
		,TaxLotQty
		,AvgPrice
		,Commission
		,OtherBrokerFees
		,ClearingFee
		,MiscFees
		,StampDuty
		,TransactionLevy
		,TaxOnCommissions
		,AUECLocalDate
		,OriginalPurchaseDate
		,ProcessDate
		,AssetID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,AUECID
		,SettlementDate
		,[Description]
		,AllocationDate
		,IsSwapped
		,GroupID
		,FXRate
		,FXConversionMethodOperator
		,IsPreAllocated
		,CumQty
		,AllocatedQty
		,Quantity
		,UserID
		,CounterPartyID
		,FundID
		,symbol
		,SideMultiplier
		,Side
		,SecFee
		,OccFee
		,OrfFee
		,ClearingBrokerFee
		,SoftCommission
		,OptionPremiumAdjustment 
		,TransactionSource
		,AccruedInterest
		)
	SELECT VT.TaxLotID
		,OrderSideTagValue
		,TotalExpenses
		,Level2ID
		,TaxLotQty
		,AvgPrice
		,Commission
		,OtherBrokerFees
		,ClearingFee
		,MiscFees
		,StampDuty
		,TransactionLevy
		,TaxOnCommissions
		,AUECLocalDate
		,OriginalPurchaseDate
		,ProcessDate
		,VT.AssetID
		,VT.UnderLyingID
		,VT.ExchangeID
		,VT.CurrencyID
		,VT.AUECID
		,SettlementDate
		,Description
		,AllocationDate
		,IsSwapped
		,VT.GroupID
		,FXRate
		,FXConversionMethodOperator
		,IsPreAllocated
		,CumQty
		,AllocatedQty
		,Quantity
		,UserID
		,CounterPartyID
		,VT.FundID
		,Symbol
		,SideMultiplier
		,T_Side.Side
		,SecFee
		,OccFee
		,OrfFee
		,ClearingBrokerFee
		,SoftCommission
		,VT.OptionPremiumAdjustment 
		,VT.TransactionSource
		,VT.AccruedInterest
	FROM V_Taxlots VT
	INNER JOIN T_Side ON T_Side.SideTagValue = VT.OrderSideTagValue
	WHERE TaxlotID NOT IN (
			SELECT FKID
			FROM T_AllActivity
			WHERE datediff(dd, TradeDate, @startDate) <= 0
				AND datediff(dd, TradeDate, @endDate) >= 0
				AND TransactionSource = 1
			)
		AND (
			datediff(dd, allocationdate, @startDate) <= 0
			AND datediff(dd, allocationdate, @endDate) >= 0
			)

	-- get security master data in to a temp table        
	CREATE TABLE #SecMasterDataTempTable (
		AUECID INT
		,TickerSymbol VARCHAR(100)
		,CompanyName VARCHAR(500)
		,AssetName VARCHAR(100)
		,SecurityTypeName VARCHAR(200)
		,SectorName VARCHAR(100)
		,SubSectorName VARCHAR(100)
		,CountryName VARCHAR(100)
		,PutOrCall VARCHAR(5)
		,Multiplier FLOAT
		,LeadCurrencyID INT
		,VsCurrencyID INT
		,CurrencyID INT
		,UnderlyingSymbol VARCHAR(100)
		,ExpirationDate DATETIME
		,Coupon FLOAT
		,IssueDate DATETIME
		,MaturityDate DATETIME
		,FirstCouponDate DATETIME
		,CouponFrequencyID INT
		,AccrualBasisID INT
		,BondTypeID INT
		,IsZero INT
		,IsNDF BIT
		,FixingDate DATETIME
		,IDCOSymbol VARCHAR(50)
		,OSISymbol VARCHAR(50)
		,SEDOLSymbol VARCHAR(50)
		,CUSIPSymbol VARCHAR(50)
		,BloombergSymbol VARCHAR(200)
		,Delta FLOAT
		,StrikePrice FLOAT
		,UnderlyingDelta FLOAT
		)

	INSERT INTO #SecMasterDataTempTable
	SELECT AUECID
		,TickerSymbol
		,CompanyName
		,AssetName
		,SecurityTypeName
		,SectorName
		,SubSectorName
		,CountryName
		,PutOrCall
		,Multiplier
		,LeadCurrencyID
		,VsCurrencyID
		,CurrencyID
		,UnderlyingSymbol
		,ExpirationDate
		,Coupon
		,IssueDate
		,MaturityDate
		,FirstCouponDate
		,CouponFrequencyID
		,AccrualBasisID
		,BondTypeID
		,IsZero
		,IsNDF
		,FixingDate
		,IDCOSymbol
		,OSISymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,Delta
		,StrikePrice
		,UnderlyingDelta
	FROM V_SecMasterData
	WHERE Tickersymbol IN (
			SELECT DISTINCT symbol
			FROM #V_Taxlots
			)

	SELECT VT.TaxLotID AS TaxLotID
		,VT.AUECLocalDate AS TradeDate
		,VT.OriginalPurchaseDate
		,VT.ProcessDate
		,VT.OrderSideTagValue AS SideID
		,-- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS                                                                                                                                                          
		VT.Symbol AS Symbol
		,VT.TaxLotQty AS Quantity
		,VT.AvgPrice AS AvgPX
		,VT.FundID AS FundID
		,VT.AssetID AS AssetID
		,VT.UnderLyingID AS UnderLyingID
		,VT.ExchangeID AS ExchangeID
		,VT.CurrencyID AS CurrencyID
		,VT.AUECID AS AUECID
		,VT.TotalExpenses AS TotalCommissionandFees
		,--this is open commission and closed commission sum is not necessarily equals to total commission                                                         
		IsNull(SM.Multiplier, 1) AS Multiplier
		,VT.SettlementDate AS SettlementDate
		,SM.LeadCurrencyID
		,SM.VsCurrencyID
		,IsNull(SM.ExpirationDate, '1/1/1800') AS ExpirationDate
		,VT.Description AS Description
		,VT.Level2ID AS Level2ID
		,IsNull((VT.TaxLotQty * SW.NotionalValue / VT.CumQty), 0) AS NotionalValue
		,IsNull(SW.BenchMarkRate, 0) AS BenchMarkRate
		,IsNull(SW.Differential, 0) AS Differential
		,IsNull(SW.OrigCostBasis, 0) AS OrigCostBasis
		,IsNull(SW.DayCount, 0) AS DayCount
		,IsNull(SW.SwapDescription, '') AS SwapDescription
		,SW.FirstResetDate AS FirstResetDate
		,SW.OrigTransDate AS OrigTransDate
		,VT.IsSwapped AS IsSwapped
		,VT.AllocationDate AS AUECLocalDate
		,VT.GroupID
		,Tag.PositionTag
		,VT.FXRate
		,VT.FXConversionMethodOperator
		,IsNull(SM.CompanyName, '') AS CompanyName
		,IsNull(SM.UnderlyingSymbol, '') AS UnderlyingSymbol
		,IsNull(SM.Delta, 1) AS Delta
		,IsNull(SM.PutOrCall, '') AS PutOrCall
		,VT.IsPreAllocated
		,VT.CumQty
		,VT.AllocatedQty
		,VT.Quantity
		,IsNull(SM.StrikePrice, 0) AS StrikePrice
		,VT.UserID
		,VT.CounterPartyID
		,SM.Coupon
		,SM.IssueDate
		,SM.MaturityDate
		,SM.FirstCouponDate
		,SM.CouponFrequencyID
		,SM.AccrualBasisID
		,SM.BondTypeID
		,SM.IsZero
		,SM.IsNDF
		,SM.FixingDate
		,VT.TaxLotQty * VT.AvgPrice * SM.Multiplier AS GrossNotionalValue
		,VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses AS NetNotionalValue
		,T_CompanyFunds.FundName
		,VT.Commission AS Commission
		,VT.OtherBrokerFees
		,VT.ClearingFee
		,VT.MiscFees
		,VT.StampDuty
		,VT.TransactionLevy
		,VT.TaxOnCommissions
		,SM.IDCOSymbol AS IDCO
		,SM.OSISymbol AS OSI
		,SM.SEDOLSymbol AS SEDOL
		,SM.CUSIPSymbol AS CUSIP
		,SM.BloombergSymbol AS Bloomberg
		,VT.Side
		,T_Asset.AssetName AS Asset
		,CP.ShortName AS CounterParty
		,CASE 
			WHEN VT.FXConversionMethodOperator = 'M'
				THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier) * IsNull(VT.FXRate, 0)
			WHEN VT.FXConversionMethodOperator = 'D'
				AND VT.FXRate > 0
				THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier) * 1 / VT.FXRate
			ELSE 0
			END AS GrossNotionalValueBase
		,CASE 
			WHEN VT.FXConversionMethodOperator = 'M'
				THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * IsNull(VT.FXRate, 0)
			WHEN VT.FXConversionMethodOperator = 'D'
				AND VT.FXRate > 0
				THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * 1 / VT.FXRate
			ELSE 0
			END AS NetNotionalValueBase
		,CASE 
			WHEN VT.FXConversionMethodOperator = 'M'
				THEN (VT.TotalExpenses) * IsNull(VT.FXRate, 0)
			WHEN VT.FXConversionMethodOperator = 'D'
				AND VT.FXRate > 0
				THEN (VT.TotalExpenses) * 1 / VT.FXRate
			ELSE 0
			END AS TotalCommissionandFeesBase
		,CASE 
			WHEN VT.FXConversionMethodOperator = 'M'
				THEN (VT.Commission) * IsNull(VT.FXRate, 0)
			WHEN VT.FXConversionMethodOperator = 'D'
				AND VT.FXRate > 0
				THEN (VT.Commission) * 1 / VT.FXRate
			ELSE 0
			END AS CommissionBase
		,CASE 
			WHEN VT.FXConversionMethodOperator = 'M'
				THEN (VT.OtherBrokerFees) * IsNull(VT.FXRate, 0)
			WHEN VT.FXConversionMethodOperator = 'D'
				AND VT.FXRate > 0
				THEN (VT.OtherBrokerFees) * 1 / VT.FXRate
			ELSE 0
			END AS FeesBase
		,CASE 
			WHEN VT.FXConversionMethodOperator = 'M'
				THEN (VT.ClearingFee) * IsNull(VT.FXRate, 0)
			WHEN VT.FXConversionMethodOperator = 'D'
				AND VT.FXRate > 0
				THEN (VT.ClearingFee) * 1 / VT.FXRate
			ELSE 0
			END AS ClearingFeeBase
		,CASE 
			WHEN VT.FXConversionMethodOperator = 'M'
				THEN (VT.MiscFees) * IsNull(VT.FXRate, 0)
			WHEN VT.FXConversionMethodOperator = 'D'
				AND VT.FXRate > 0
				THEN (VT.MiscFees) * 1 / VT.FXRate
			ELSE 0
			END AS MiscFeesBase
		,CASE 
			WHEN VT.FXConversionMethodOperator = 'M'
				THEN (VT.StampDuty) * IsNull(VT.FXRate, 0)
			WHEN VT.FXConversionMethodOperator = 'D'
				AND VT.FXRate > 0
				THEN (VT.StampDuty) * 1 / VT.FXRate
			ELSE 0
			END AS StampDutyBase
		,SM.UnderlyingDelta
		,VT.SecFee
		,VT.OccFee
		,VT.OrfFee
		,VT.ClearingBrokerFee
		,VT.SoftCommission AS SoftCommission
		,VT.OptionPremiumAdjustment  
		,VT.TransactionSource
		,VT.AccruedInterest
	FROM #V_Taxlots VT
	INNER JOIN #TempPosTag tag ON VT.TaxlotID = tag.TaxlotID
	INNER JOIN T_CompanyFunds ON T_CompanyFunds.CompanyFundID = VT.FundID
	LEFT OUTER JOIN T_SwapParameters SW ON VT.GroupID = SW.GroupID
	LEFT OUTER JOIN #SecMasterDataTempTable SM ON VT.Symbol = SM.TickerSymbol
	LEFT OUTER JOIN T_CounterParty CP ON CP.CounterPartyID = VT.CounterPartyID
	INNER JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
	INNER JOIN T_CashPreferences tcpref ON vt.FundID = tcpref.FundID
	WHERE DATEDIFF(d, tcpref.CashMgmtStartDate, VT.AUECLocalDate) >= 0
		AND VT.FundID IN (
			SELECT #funds.fundID
			FROM #funds
			)

	DROP TABLE #V_Taxlots

	DROP TABLE #TempPosTag

	DROP TABLE #SecMasterDataTempTable
END
