/*******************************************************************                                                                                  
Create by: Narendra Kumar Jangir            
Date : 08 Apr 2015           
Description:  Gets forex transactions for which settlement fx rate is missing at the trade level, we will pick day end fx rate as settlement fx rate                    
EXEC P_GetTransactionsToUpdatedSettlementFields '2015-04-08',''
*/
CREATE PROCEDURE [dbo].[P_GetTransactionsToUpdatedSettlementFields] (
	@ToDate DATETIME
	,@FundIds VARCHAR(MAX)
	)
AS
/*------------usage--------------------                      
declare @FromAllAUECDatesString VARCHAR(MAX)                                                                                                                                                                                                                   
  
declare @ToAllAUECDatesString VARCHAR(MAX)                                                                  
declare @AssetIds VARCHAR(MAX)                                                                   
declare @FundIds VARCHAR(MAX)                         
declare @ReconDateType INT          
                   
set @FromAllAUECDatesString=N'0^3/11/2015 12:00:00 AM~1^3/11/2015 12:00:00 AM~59^3/11/2015 12:00:00 AM~'                    
set @ToAllAUECDatesString=N'0^3/11/2015 12:00:00 AM~1^3/11/2015 12:00:00 AM~59^3/11/2015 12:00:00 AM~'                   
set @AssetIds=''                    
set @FundIds=''     
set @ReconDateType=0                   
*/
BEGIN
	CREATE TABLE #Funds (FundID INT)

	IF (
			@FundIds IS NULL
			OR @FundIds = ''
			)
	BEGIN
		INSERT INTO #Funds
		SELECT CompanyFundID AS FundID
		FROM T_CompanyFunds
		Where IsActive=1 
	END
	ELSE
		INSERT INTO #Funds
		SELECT Items AS FundID
		FROM dbo.Split(@FundIds, ',')

	DECLARE @FromDate DATETIME

	SELECT @FromDate = MIN(LastCalcDate)
	FROM T_LastCalcDateRevaluation
	WHERE FundID IN (
			SELECT FundID
			FROM #Funds
			)

	SELECT TaxlotID
		,PositionTag
	INTO #TempPosTag
	FROM PM_Taxlots
	WHERE TaxLot_PK IN (
			SELECT Min(TaxLot_PK)
			FROM PM_Taxlots
			GROUP BY TaxlotID
			)

	CREATE TABLE #SecMasterDataTempTable (
		AUECID INT
		,TickerSymbol VARCHAR(100)
		,CompanyName VARCHAR(500)
		,
		--Added UDA fields by OMshiv, Nov 2013                                                                                                                                                             
		AssetName VARCHAR(100)
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
		,ISINSymbol VARCHAR(50)
		,ProxySymbol VARCHAR(100)
		,ReutersSymbol VARCHAR(100)
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
		,ISINSymbol
		,ProxySymbol
		,ReutersSymbol
	FROM V_SecMasterData

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
		,AUECLocalDate DATETIME
		,OriginalPurchaseDate DATETIME
		,ProcessDate DATETIME
		,AssetID INT
		,UnderLyingID INT
		,ExchangeID INT
		,CurrencyID INT
		,CurrencySymbol VARCHAR(20)
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
		,
		--following field are added to fulfill the ss requirement                          
		LotID VARCHAR(200)
		,ExternalTransID VARCHAR(100)
		,TradeAttribute1 VARCHAR(200)
		,TradeAttribute2 VARCHAR(200)
		,TradeAttribute3 VARCHAR(200)
		,TradeAttribute4 VARCHAR(200)
		,TradeAttribute5 VARCHAR(200)
		,TradeAttribute6 VARCHAR(200)
		,SecFee FLOAT
		,OccFee FLOAT
		,OrfFee FLOAT
		,ClearingBrokerFee FLOAT
		,SoftCommission FLOAT
		,TaxOnCommissions FLOAT
		,TransactionLevy FLOAT
		,TransactionType VARCHAR(200)
		,InternalComments VARCHAR(500)
		,SettlCurrency_Group INT
		,SettlCurrency_Taxlot INT
		,AccruedInterest FLOAT
		,OptionPremiumAdjustment FLOAT
		,TransactionSource INT
		,AdditionalTradeAttributes VARCHAR(MAX)
		)

	CREATE TABLE #FundsInFXMP (FundID INT)

	INSERT INTO #FundsInFXMP
	SELECT DISTINCT FundID
	FROM T_CurrencyConversionRate

	--Declare @baseCurrencyID int                                                                                    
	--Set @baseCurrencyID= (Select TOP 1 BaseCurrencyID from T_Company)                                                         
	CREATE TABLE #FXConversionRatesForTradeDate (
		FromCurrencyID INT
		,ToCurrencyID INT
		,RateValue FLOAT
		,ConversionMethod INT
		,DATE DATETIME
		,eSignalSymbol VARCHAR(max)
		,FundID INT
		)

	INSERT INTO #FXConversionRatesForTradeDate
	EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @FromDate
		,@ToDate

	UPDATE #FXConversionRatesForTradeDate
	SET RateValue = 1.0 / RateValue
	WHERE RateValue <> 0
		AND ConversionMethod = 1

	UPDATE #FXConversionRatesForTradeDate
	SET RateValue = 0
	WHERE RateValue IS NULL

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
		,AUECLocalDate
		,OriginalPurchaseDate
		,ProcessDate
		,AssetID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,CurrencySymbol
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
		,LotID
		,ExternalTransID
		,TradeAttribute1
		,TradeAttribute2
		,TradeAttribute3
		,TradeAttribute4
		,TradeAttribute5
		,TradeAttribute6
		,SecFee
		,OccFee
		,OrfFee
		,ClearingBrokerFee
		,SoftCommission
		,TaxOnCommissions
		,TransactionLevy
		,TransactionType
		,InternalComments
		,SettlCurrency_Group
		,SettlCurrency_Taxlot
		,AccruedInterest
		,OptionPremiumAdjustment
		,TransactionSource
		,AdditionalTradeAttributes
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
		,AUECLocalDate
		,OriginalPurchaseDate
		,ProcessDate
		,VT.AssetID
		,VT.UnderLyingID
		,VT.ExchangeID
		,VT.CurrencyID
		,CUR.CurrencySymbol
		,VT.AUECID
		,SettlementDate
		,Description
		,AllocationDate
		,IsSwapped
		,VT.GroupID
		,IsNull(VT.FXRate_Taxlot, VT.FXRate) AS FXRate
		,IsNull(FXConversionMethodOperator_Taxlot, FXConversionMethodOperator) AS FXConversionMethodOperator
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
		,VT.LotID
		,VT.ExternalTransID
		,VT.TradeAttribute1
		,VT.TradeAttribute2
		,VT.TradeAttribute3
		,VT.TradeAttribute4
		,VT.TradeAttribute5
		,VT.TradeAttribute6
		,SecFee
		,OccFee
		,OrfFee
		,ClearingBrokerFee
		,SoftCommission
		,VT.TaxOnCommissions
		,VT.TransactionLevy
		,TransactionType
		,VT.InternalComments
		,VT.SettlCurrency_Group
		,VT.SettlCurrency_Taxlot
		,AccruedInterest
		,OptionPremiumAdjustment
		,VT.TransactionSource
		,VT.AdditionalTradeAttributes
	FROM V_Taxlots VT                                             
	INNER JOIN #Funds ON VT.FundID = #Funds.FundID
	INNER JOIN T_LastCalcDateRevaluation RVL ON RVL.FundId = VT.FundID
	INNER JOIN T_Side ON T_Side.SideTagValue = VT.OrderSideTagValue
	INNER JOIN T_currency CUR ON CUR.CurrencyID = VT.CurrencyID
	WHERE
		--Modified by: Aman Seth, if @ReconDateType 0 then consider Trade Date , 1 then consider Process Date,2 then consider Prana Process Date            
		(
			Datediff(d, VT.AUECLocalDate, @ToDate) >= 0
			AND Datediff(d, VT.AUECLocalDate, RVL.LastCalcDate) <= 0
			)
		AND VT.TaxLotQty <> 0
                                                                     
	SELECT Distinct VT.TaxLotID AS TaxLotID
		,VT.AUECLocalDate AS TradeDate
		,VT.OriginalPurchaseDate
		,VT.ProcessDate
		,VT.OrderSideTagValue AS SideID
		,
		-- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS                                                                                                                                                                        
		VT.Symbol AS Symbol
		,(VT.TaxLotQty) AS Quantity
		,VT.AvgPrice AS AvgPX
		,VT.FundID AS FundID
		,VT.AssetID AS AssetID
		,VT.UnderLyingID AS UnderLyingID
		,VT.ExchangeID AS ExchangeID
		,VT.CurrencyID AS CurrencyID
		,VT.CurrencySymbol AS CurrencySymbol
		,VT.AUECID AS AUECID
		,VT.TotalExpenses AS TotalCommissionandFees
		,
		--this is open commission and closed commission sum is not necessarily equals to total commission                                                                                                 
		isnull(SM.Multiplier, 1) AS Multiplier
		,VT.SettlementDate AS SettlementDate
		,SM.LeadCurrencyID
		,SM.VsCurrencyID
		,isnull(SM.ExpirationDate, '1/1/1800') AS ExpirationDate
		,VT.Description AS Description
		,VT.Level2ID AS Level2ID
		,isnull((VT.TaxLotQty * SW.NotionalValue / VT.CumQty), 0) AS NotionalValue
		,isnull(SW.BenchMarkRate, 0) AS BenchMarkRate
		,isnull(SW.Differential, 0) AS Differential
		,isnull(SW.OrigCostBasis, 0) AS OrigCostBasis
		,isnull(SW.DayCount, 0) AS DayCount
		,isnull(SW.SwapDescription, '') AS SwapDescription
		,SW.FirstResetDate AS FirstResetDate
		,SW.OrigTransDate AS OrigTransDate
		,VT.IsSwapped AS IsSwapped
		,VT.AllocationDate AS AUECLocalDate
		,VT.GroupID
		,tag.PositionTag
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN VT.FXRate
						ELSE ISNull((coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue)), 0)
						END
			ELSE 1
			END AS FXRate
		,VT.FXConversionMethodOperator
		,isnull(SM.CompanyName, '') AS CompanyName
		,isnull(SM.UnderlyingSymbol, '') AS UnderlyingSymbol
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
		,CASE 
			WHEN T_Asset.Assetid = 8
				THEN VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses * 100
			ELSE VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses
			END AS NetNotionalValue
		,CF.FundShortName AS FundName
		,
		-- Changed by Nishant Jain [2015-02-20]                                                                   
		VT.Commission AS Commission
		,VT.OtherBrokerFees
		,VT.ClearingFee
		,VT.MiscFees
		,VT.StampDuty
		,SM.IDCOSymbol AS IDCO
		,SM.OSISymbol AS OSI
		,SM.SEDOLSymbol AS SEDOL
		,SM.CUSIPSymbol AS CUSIP
		,SM.BloombergSymbol AS Bloomberg
		,VT.Side
		,T_Asset.AssetName AS Asset
		,CP.ShortName AS CounterParty
		,IsNull(MF.MasterFundName, CF.FundShortName) AS MasterFund
		,
		-- Changed by Nishant Jain [2015-02-20]                                                 
		TTP.ThirdPartyName AS PrimeBroker
		,SM.UnderlyingDelta
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN CASE 
									WHEN VT.FXConversionMethodOperator = 'M'
										THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier) * IsNull(VT.FXRate, 0)
									WHEN VT.FXConversionMethodOperator = 'D'
										AND VT.FXRate > 0
										THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier) * 1 / VT.FXRate
									END
						ELSE IsNull((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
						END
			ELSE VT.TaxLotQty * VT.AvgPrice * SM.Multiplier
			END AS GrossNotionalValueBase
		,CASE 
			WHEN T_Asset.Assetid = 8
				THEN CASE 
						WHEN VT.CurrencyID <> CF.LocalCurrency
							THEN CASE 
									WHEN IsNull(VT.FXRate, 0) <> 0
										THEN CASE 
												WHEN VT.FXConversionMethodOperator = 'M'
													THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses * 100) * IsNull(VT.FXRate, 0)
												WHEN VT.FXConversionMethodOperator = 'D'
													AND VT.FXRate > 0
													THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses * 100) * 1 / VT.FXRate
												END
									ELSE IsNull((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses * 100) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
									END
						ELSE (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses * 100)
						END
			ELSE CASE 
					WHEN VT.CurrencyID <> CF.LocalCurrency
						THEN CASE 
								WHEN IsNull(VT.FXRate, 0) <> 0
									THEN CASE 
											WHEN VT.FXConversionMethodOperator = 'M'
												THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * IsNull(VT.FXRate, 0)
											WHEN VT.FXConversionMethodOperator = 'D'
												AND VT.FXRate > 0
												THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * 1 / VT.FXRate
											END
								ELSE IsNull((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
								END
					ELSE (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses)
					END
			END AS NetNotionalValueBase
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN CASE 
									WHEN VT.FXConversionMethodOperator = 'M'
										THEN (VT.TotalExpenses) * IsNull(VT.FXRate, 0)
									WHEN VT.FXConversionMethodOperator = 'D'
										AND VT.FXRate > 0
										THEN (VT.TotalExpenses) * 1 / VT.FXRate
									END
						ELSE IsNull((VT.TotalExpenses) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
						END
			ELSE VT.TotalExpenses
			END AS TotalCommissionandFeesBase
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN CASE 
									WHEN VT.FXConversionMethodOperator = 'M'
										THEN (VT.Commission) * IsNull(VT.FXRate, 0)
									WHEN VT.FXConversionMethodOperator = 'D'
										AND VT.FXRate > 0
										THEN (VT.Commission) * 1 / VT.FXRate
									END
						ELSE IsNull((VT.Commission) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
						END
			ELSE VT.Commission
			END AS CommissionBase
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN CASE 
									WHEN VT.FXConversionMethodOperator = 'M'
										THEN (VT.OtherBrokerFees) * IsNull(VT.FXRate, 0)
									WHEN VT.FXConversionMethodOperator = 'D'
										AND VT.FXRate > 0
										THEN (VT.OtherBrokerFees) * 1 / VT.FXRate
									END
						ELSE IsNull((VT.OtherBrokerFees) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
						END
			ELSE VT.OtherBrokerFees
			END AS FeesBase
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN CASE 
									WHEN VT.FXConversionMethodOperator = 'M'
										THEN (VT.ClearingFee) * IsNull(VT.FXRate, 0)
									WHEN VT.FXConversionMethodOperator = 'D'
										AND VT.FXRate > 0
										THEN (VT.ClearingFee) * 1 / VT.FXRate
									END
						ELSE IsNull((VT.ClearingFee) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
						END
			ELSE VT.ClearingFee
			END AS ClearingFeeBase
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN CASE 
									WHEN VT.FXConversionMethodOperator = 'M'
										THEN (VT.MiscFees) * IsNull(VT.FXRate, 0)
									WHEN VT.FXConversionMethodOperator = 'D'
										AND VT.FXRate > 0
										THEN (VT.MiscFees) * 1 / VT.FXRate
									END
						ELSE IsNull((VT.MiscFees) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
						END
			ELSE VT.MiscFees
			END AS MiscFeesBase
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN CASE 
									WHEN VT.FXConversionMethodOperator = 'M'
										THEN (VT.StampDuty) * IsNull(VT.FXRate, 0)
									WHEN VT.FXConversionMethodOperator = 'D'
										AND VT.FXRate > 0
										THEN (VT.StampDuty) * 1 / VT.FXRate
									END
						ELSE IsNull((VT.StampDuty) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
						END
			ELSE VT.StampDuty
			END AS StampDutyBase
		,VT.LotID
		,VT.ExternalTransID
		,VT.TradeAttribute1
		,VT.TradeAttribute2
		,VT.TradeAttribute3
		,VT.TradeAttribute4
		,VT.TradeAttribute5
		,VT.TradeAttribute6
		,SM.ProxySymbol
		,
		--Added UDA fields by OMshiv, Nov 2013                      
		SM.AssetName
		,SM.SecurityTypeName
		,SM.SectorName
		,SM.SubSectorName
		,SM.CountryName
		,VT.SecFee
		,VT.OccFee
		,VT.OrfFee
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN CASE 
									WHEN VT.FXConversionMethodOperator = 'M'
										THEN (VT.SecFee) * IsNull(VT.FXRate, 0)
									WHEN VT.FXConversionMethodOperator = 'D'
										AND VT.FXRate > 0
										THEN (VT.SecFee) * 1 / VT.FXRate
									END
						ELSE IsNull((VT.SecFee) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
						END
			ELSE VT.SecFee
			END AS SecFeeBase
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN CASE 
									WHEN VT.FXConversionMethodOperator = 'M'
										THEN (VT.OccFee) * IsNull(VT.FXRate, 0)
									WHEN VT.FXConversionMethodOperator = 'D'
										AND VT.FXRate > 0
										THEN (VT.OccFee) * 1 / VT.FXRate
									END
						ELSE IsNull((VT.OccFee) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
						END
			ELSE VT.OccFee
			END AS OccFeeBase
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN CASE 
									WHEN VT.FXConversionMethodOperator = 'M'
										THEN (VT.OrfFee) * IsNull(VT.FXRate, 0)
									WHEN VT.FXConversionMethodOperator = 'D'
										AND VT.FXRate > 0
										THEN (VT.OrfFee) * 1 / VT.FXRate
									END
						ELSE IsNull((VT.OrfFee) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
						END
			ELSE VT.OrfFee
			END AS OrfFeeBase
		,VT.ClearingBrokerFee AS ClearingBrokerFee
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN CASE 
									WHEN VT.FXConversionMethodOperator = 'M'
										THEN (VT.ClearingBrokerFee) * IsNull(VT.FXRate, 0)
									WHEN VT.FXConversionMethodOperator = 'D'
										AND VT.FXRate > 0
										THEN (VT.ClearingBrokerFee) * 1 / VT.FXRate
									END
						ELSE IsNull((VT.ClearingBrokerFee) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
						END
			ELSE VT.ClearingBrokerFee
			END AS ClearingBrokerFeeBase
		,VT.SoftCommission AS SoftCommission
		,CASE 
			WHEN VT.CurrencyID <> CF.LocalCurrency
				THEN CASE 
						WHEN IsNull(VT.FXRate, 0) <> 0
							THEN CASE 
									WHEN VT.FXConversionMethodOperator = 'M'
										THEN (VT.SoftCommission) * IsNull(VT.FXRate, 0)
									WHEN VT.FXConversionMethodOperator = 'D'
										AND VT.FXRate > 0
										THEN (VT.SoftCommission) * 1 / VT.FXRate
									END
						ELSE IsNull((VT.SoftCommission) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)
						END
			ELSE VT.SoftCommission
			END AS SoftCommissionBase
		,VT.TaxOnCommissions
		,VT.TransactionLevy
		,VT.SideMultiplier
		,TransactionType
		,SM.ReutersSymbol
		,CASE 
			WHEN (
					Quantity = 0
					OR Multiplier = 0
					)
				THEN 0
			WHEN (
					OrderSideTagValue = '1'
					OR OrderSideTagValue = '3'
					OR OrderSideTagValue = 'A'
					OR OrderSideTagValue = 'B'
					OR OrderSideTagValue = 'E'
					)
				THEN (IsNull((Quantity * IsNull(AvgPrice, 0) * Multiplier + TotalExpenses), 0)) / (Quantity * Multiplier)
			ELSE (IsNull((Quantity * IsNull(AvgPrice, 0) * Multiplier - TotalExpenses), 0)) / (Quantity * Multiplier)
			END AS UnitCost
		,VT.SecFee AS SecFees
		,VT.InternalComments
		,CUR1.CurrencySymbol AS BaseCurrency
		,COALESCE(VT.SettlCurrency_Taxlot, VT.SettlCurrency_Group, 'None') AS SettlCurrency
		,VT.AccruedInterest
		,VT.OptionPremiumAdjustment
		,VT.TransactionSource
		,VT.AdditionalTradeAttributes
	FROM #V_Taxlots VT
	INNER JOIN T_AllActivity activity ON activity.FKID = VT.TaxlotID
	INNER JOIN #TempPosTag tag ON VT.TaxlotID = tag.TaxlotID
	INNER JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID
	INNER JOIN T_Company AS TC ON CF.CompanyID = TC.CompanyID
	LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = VT.FundID
	LEFT OUTER JOIN T_companyMasterFunds MF ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID
	LEFT OUTER JOIN T_ThirdParty TTP ON TTP.ThirdPartyID = CF.CompanyThirdPartyID
	LEFT OUTER JOIN T_SwapParameters SW ON VT.GroupID = SW.GroupID
	LEFT OUTER JOIN #SecMasterDataTempTable SM ON VT.Symbol = SM.TickerSymbol
	LEFT OUTER JOIN T_CounterParty CP ON CP.CounterPartyID = VT.CounterPartyID
	INNER JOIN T_Asset ON T_Asset.AssetID = VT.AssetID
	LEFT OUTER JOIN T_currency CUR1 ON CUR1.CurrencyID = CF.LocalCurrency
	LEFT OUTER JOIN T_currency GCUR ON GCUR.CurrencyID = VT.SettlCurrency_Group
	LEFT OUTER JOIN T_currency LTCUR ON LTCUR.CurrencyID = VT.SettlCurrency_Taxlot
	LEFT OUTER JOIN #FXConversionRatesForTradeDate FXDayRatesForTradeDate ON (
			FXDayRatesForTradeDate.FromCurrencyID = VT.CurrencyID
			AND FXDayRatesForTradeDate.ToCurrencyID = CF.LocalCurrency
			AND DateDiff(d, VT.AUECLocalDate, FXDayRatesForTradeDate.DATE) = 0
			AND FXDayRatesForTradeDate.FundID = VT.FundID
			)
	LEFT OUTER JOIN #FXConversionRatesForTradeDate FXDayRatesForTradeDate1 ON (
			FXDayRatesForTradeDate1.FromCurrencyID = VT.CurrencyID
			AND FXDayRatesForTradeDate1.ToCurrencyID = CF.LocalCurrency
			AND DateDiff(d, VT.AUECLocalDate, FXDayRatesForTradeDate1.DATE) = 0
			AND FXDayRatesForTradeDate1.FundID = 0
			)
	LEFT OUTER JOIN #FXConversionRatesForTradeDate SettlementFXDayRatesForTradeDate ON (
			SettlementFXDayRatesForTradeDate.FromCurrencyID = VT.CurrencyID
			AND SettlementFXDayRatesForTradeDate.ToCurrencyID = VT.SettlCurrency_Taxlot
			AND DateDiff(d, VT.AUECLocalDate, SettlementFXDayRatesForTradeDate.DATE) = 0
			AND (
				SettlementFXDayRatesForTradeDate.FundID = VT.FundID
				AND SettlementFXDayRatesForTradeDate.FundID IN (
					SELECT FundID
					FROM #FundsInFXMP
					WHERE FundID IS NOT NULL
					)
				)
			)
		OR (
			SettlementFXDayRatesForTradeDate.FromCurrencyID = VT.CurrencyID
			AND SettlementFXDayRatesForTradeDate.ToCurrencyID = VT.SettlCurrency_Taxlot
			AND DateDiff(d, VT.AUECLocalDate, SettlementFXDayRatesForTradeDate.DATE) = 0
			AND (
				SettlementFXDayRatesForTradeDate.FundID = 0
				OR SettlementFXDayRatesForTradeDate.FundID IS NULL
				AND VT.FundID NOT IN (
					SELECT FundID
					FROM #FundsInFXMP
					WHERE FundID IS NOT NULL
					)
				)
			)
	WHERE ISNULL(VT.SettlCurrency_Taxlot, 0) > 0
			AND VT.CurrencyID <> VT.SettlCurrency_Taxlot

	DROP TABLE #V_Taxlots

	DROP TABLE #Funds

	DROP TABLE #SecMasterDataTempTable

	DROP TABLE #TempPosTag
		,#FundsInFXMP
		,#FXConversionRatesForTradeDate

	RETURN;
END