
/****************************************************************************                                                                                                                                                                                  
  
    
Name :   PMGetUnrealizedPNLReport                                                                                                  
Date Created: 20-Sep-2007                                                                                                                                                                                                                                     
Purpose:  Get the unrealized pnl report i.e. open position's PNL till the date passed.                                                                                                                                                                         
  
              
Module Name: PortfolioReports/Unrealized PNL Report                                                                                                                                                                                            
Author: Bhupesh Bareja                                                                                                                                                                                                                                    
Parameters:                                                                                                                                                                                                                                     
 @companyID int,                                                                                                                                                                                                                                    
 @date datetime,                                                                                                                                                                                                                                    
 @reportMode int                                                                                                                                                          
 Date Modified: 13-05-2008                                                                                                                                                                                                                                    
 Description:                                                                                                                                                                                                                                       
 Modified By:  Sandeep SIngh                                                                                                                
                                                                                                                
Date Modified: 24-10-2008                                                                                                                                                                                                                                    
Description: Execution Optimizations & Formatting                                                                                                              
Modified By:  Sumit Kakra                                                                                                              
                                                                                                                                                                                                                                 
Execution StateMent:                              
   EXEC [PMGetUnrealizedPNLReport] 2,'10-12-2012',0,'1182,1183,1184','1,2,3,4,5'              
select * from T_Group                  
                      
Modified Date: 24-April-2012                                                            
Modified By : Sandeep Singh                                                                                       
<Description: FX Spot, FX Forward and Fixed Income related changes>               
              
Modified Date: 27-Sep-2012              
Modified By: Rahul Gupta              
Description: FX level implementation at taxlot level        
  
Modified Date: 12-DEc-2012              
Modified By: om shiv              
Description: Added UDA columns    
  
Modified Date: 30-OCT-2014              
Modified By: Sandeep Singh              
Description: Transaction Type column added   
                                                                                                                 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMGetUnrealizedPNLReport] (
	@companyID INT
	,@date DATETIME
	,@reportMode INT
	,@Fund VARCHAR(max)
	,@Asset VARCHAR(max)
	)
AS
BEGIN
	SELECT PM_Taxlots.*
	INTO #PM_Taxlots
	FROM PM_Taxlots
	WHERE taxlot_PK IN (
			SELECT max(taxlot_PK)
			FROM PM_Taxlots
			WHERE Datediff(d, PM_Taxlots.AUECModifiedDate, @date) >= 0
			GROUP BY taxlotid
			)
		AND TaxLotOpenQty <> 0

	DECLARE @startingDate DATETIME

	SET @startingDate = (
			SELECT min(AuecLocalDate)
			FROM PM_Taxlots T
			INNER JOIN T_group G
				ON G.groupID = T.GroupID
			WHERE T.taxlotopenqty <> 0
				AND T.Taxlot_pk IN (
					SELECT max(Taxlot_pk)
					FROM PM_taxlots
					WHERE datediff(d, Auecmodifieddate, @date) >= 0
					GROUP BY Taxlotid
					)
				AND DateDiff(D, AuecLocalDate, '1-1-1800') <> 0
			)

	--Select min(AUECModifiedDate) as TradeDate from #PM_Taxlots      
	--Select @startingDate      
	IF @startingDate IS NULL
		SET @startingDate = @date

	CREATE TABLE #TEMPGetConversionFactorForGivenDateRange (
		FCID INT
		,TCID INT
		,ConversionFactor FLOAT
		,ConversionMethod INT
		,DateCC DATETIME
		)

	INSERT INTO #TEMPGetConversionFactorForGivenDateRange
	SELECT FromCurrencyID
		,ToCurrencyID
		,RateValue
		,ConversionMethod
		,DATE
	FROM dbo.GetAllFXConversionRatesForGivenDateRange(@startingDate, @date)

	DECLARE @AllAUECDatesString VARCHAR(MAX)

	SET @AllAUECDatesString = dbo.GetAUECDateString(@date)

	DECLARE @T_FundIDs TABLE (FundId INT)

	INSERT INTO @T_FundIDs
	SELECT *
	FROM dbo.Split(@Fund, ',')

	DECLARE @T_AssetIDs TABLE (AssetId INT)

	INSERT INTO @T_AssetIDs
	SELECT *
	FROM dbo.Split(@Asset, ',')

	SELECT T_Asset.*
	INTO #T_Asset
	FROM T_Asset
	INNER JOIN @T_AssetIDs AssetIDs
		ON T_Asset.AssetID = AssetIDs.AssetId

	CREATE TABLE #T_CompanyFunds (
		CompanyFundID INT
		,FundName VARCHAR(50)
		,FundShortName VARCHAR(50)
		,CompanyID INT
		,FundTypeID INT
		,UIOrder INT NULL
		)

	INSERT INTO #T_CompanyFunds
	SELECT CompanyFundID
		,FundName
		,FundShortName
		,CompanyID
		,FundTypeID
		,UIOrder
	FROM T_CompanyFunds
	INNER JOIN @T_FundIDs FundIDs
		ON T_CompanyFunds.CompanyFundID = FundIDs.FundID
		Where T_CompanyFunds.IsActive=1

	CREATE TABLE #TEMPFundPositionsForDate_ValReport (
		TaxLotID VARCHAR(50)
		,CreationDate DATETIME
		,OrderSideTagValue CHAR(1)
		,Symbol VARCHAR(200)
		,OpenQuantity FLOAT
		,-- quantity is the net quantity of the position fetched i.e. the current quantity.                                                         
		AveragePrice FLOAT
		,FundID INT
		,AssetID INT
		,UnderLyingID INT
		,ExchangeID INT
		,CurrencyID INT
		,AUECID INT
		,OpenTotalCommissionandFees FLOAT
		,Multiplier FLOAT
		,SettlementDate DATETIME
		,VsCurrencyID INT
		,TradedCurrencyID INT
		,ExpirationDate DATETIME
		,Description VARCHAR(max)
		,Level2ID INT
		,NotionalValue FLOAT
		,BenchMarkRate FLOAT
		,Differential FLOAT
		,OrigCostBasis FLOAT
		,DC INT
		,SwapDescription VARCHAR(max)
		,FirstResetDate DATETIME
		,OrigTransDate DATETIME
		,IsSwapped BIT
		,AUECLocalDate DATETIME
		,GroupID VARCHAR(50)
		,PositionTag INT
		,FXRate FLOAT
		,FXConversionMethodOperator VARCHAR(5)
		,CompanyName VARCHAR(500)
		,UnderlyingSymbol VARCHAR(50)
		)

	CREATE TABLE #PositionTable (
		TaxLotID VARCHAR(50)
		,AUECLocalDate DATETIME
		,SideID CHAR(1)
		,Symbol VARCHAR(200)
		,OpenQuantity FLOAT
		,AveragePrice FLOAT
		,FundID INT
		,AssetID INT
		,UnderLyingID INT
		,ExchangeID INT
		,CurrencyID INT
		,AUECID INT
		,TotalCommissionandFees FLOAT
		,Multiplier FLOAT
		,SettlementDate DATETIME
		,LeadCurrencyID INT
		,VsCurrencyID INT
		,ExpirationDate DATETIME
		,Description VARCHAR(max)
		,Level2ID INT
		,NotionalValue FLOAT
		,BenchMarkRate FLOAT
		,Differential FLOAT
		,OrigCostBasis FLOAT
		,DayCount INT
		,SwapDescription VARCHAR(max)
		,FirstResetDate DATETIME
		,OrigTransDate DATETIME
		,IsSwapped BIT
		,AllocationDate DATETIME
		,GroupID VARCHAR(50)
		,PositionTag INT
		,FXRate FLOAT
		,FXConversionMethodOperator VARCHAR(5)
		,CompanyName VARCHAR(500)
		,UnderlyingSymbol VARCHAR(50)
		,Delta FLOAT
		,PutOrCall VARCHAR(5)
		,IsGrPreAllocated BIT
		,GrCumQty FLOAT
		,GrAllocatedQty FLOAT
		,GrQuantity FLOAT
		,Taxlot_Pk BIGINT
		,ParentRow_Pk BIGINT
		,StrikePrice FLOAT
		,UserID INT
		,CounterPartyID INT
		,CorpActionID UNIQUEIDENTIFIER
		,Coupon FLOAT
		,IssueDate DATETIME
		,MaturityDate DATETIME
		,FirstCouponDate DATETIME
		,CouponFrequencyID INT
		,AccrualBasisID INT
		,BondTypeID INT
		,IsZero BIT
		,ProcessDate DATETIME
		,OriginalPurchaseDate DATETIME
		,IsNDF BIT
		,ReRateDate DATETIME
		,IDCOSymbol VARCHAR(50)
		,OSISymbol VARCHAR(50)
		,SEDOLSymbol VARCHAR(50)
		,CUSIPSymbol VARCHAR(50)
		,BloombergSymbol VARCHAR(200)
		,MasterFund VARCHAR(50)
		,UnderlyingDelta FLOAT
		,ISINSymbol VARCHAR(50)
		,LotId VARCHAR(200)
		,ExternalTransId VARCHAR(100)
		,TradeAttribute1 VARCHAR(200)
		,TradeAttribute2 VARCHAR(200)
		,TradeAttribute3 VARCHAR(200)
		,TradeAttribute4 VARCHAR(200)
		,TradeAttribute5 VARCHAR(200)
		,TradeAttribute6 VARCHAR(200)
		,ProxySymbol VARCHAR(100)
		,
		--Added UDA columns ,by Omshiv, Nov, 2013    
		AssetName VARCHAR(100)
		,SecurityTypeName VARCHAR(200)
		,SectorName VARCHAR(100)
		,SubSectorName VARCHAR(100)
		,CountryName VARCHAR(100)
		,BBGID VARCHAR(20)
		,TransactionType VARCHAR(200)
		,ExecutedQty FLOAT
		,ClosingTaxlotId VARCHAR(50)
		,ReutersSymbol VARCHAR(50)
		,InternalComments VARCHAR(500)
		,SettlCurrency VARCHAR(4)
		,IsCurrencyFuture BIT
	    ,Symbol_PK BIGINT 
		,VenueID INT
		,OrderTypeTagValue VARCHAR(50)
		,BloombergSymbolWithExchangeCode VARCHAR(200)
		,AdditionalTradeAttributes VARCHAR(MAX)
		)

	INSERT INTO #PositionTable
	EXEC P_GetPositions @AllAUECDatesString
		,NULL
		,NULL
		,NULL

	INSERT INTO #TEMPFundPositionsForDate_ValReport (
		TaxLotID
		,CreationDate
		,OrderSideTagValue
		,Symbol
		,OpenQuantity
		,AveragePrice
		,FundID
		,AssetID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,AUECID
		,OpenTotalCommissionandFees
		,Multiplier
		,SettlementDate
		,VsCurrencyID
		,TradedCurrencyID
		,ExpirationDate
		,Description
		,Level2ID
		,NotionalValue
		,BenchMarkRate
		,Differential
		,OrigCostBasis
		,DC
		,SwapDescription
		,FirstResetDate
		,OrigTransDate
		,IsSwapped
		,AUECLocalDate
		,GroupID
		,PositionTag
		,FXRate
		,FXConversionMethodOperator
		,CompanyName
		,UnderlyingSymbol
		)
	SELECT TaxLotID
		,ProcessDate
		,--AUECLocalDate,                                                                                                            
		SideID
		,Symbol
		,OpenQuantity
		,AveragePrice
		,FundID
		,AssetID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,AUECID
		,TotalCommissionandFees
		,--this is open commission and closed commission sum is not necessarily equals to total commission                                              
		Multiplier
		,SettlementDate
		,VsCurrencyID
		,LeadCurrencyID
		,ExpirationDate
		,Description
		,Level2ID
		,NotionalValue
		,BenchMarkRate
		,Differential
		,OrigCostBasis
		,DayCount
		,SwapDescription
		,FirstResetDate
		,OrigTransDate
		,IsSwapped
		,AllocationDate
		,GroupID
		,PositionTag
		,FXRate
		,FXConversionMethodOperator
		,CompanyName
		,UnderlyingSymbol
	FROM #PositionTable

	CREATE TABLE #DayMarkPrices (
		Symbol VARCHAR(200)
		,YesterDayMarkPrice FLOAT
		,TodayMarkPrice FLOAT
		)

	CREATE TABLE #AUECYesterDates (
		AUECID INT
		,YESTERDAYBIZDATE DATETIME
		)

	CREATE TABLE #CompanyNamesAndUDAData (
		Symbol VARCHAR(200)
		,CompanyName VARCHAR(500)
		,UDAAssetName VARCHAR(100)
		,UDASecurityTypeName VARCHAR(100)
		,UDASectorName VARCHAR(100)
		,UDASubSectorName VARCHAR(100)
		,UDACountryName VARCHAR(100)
		,PutOrCall VARCHAR(10)
		,FutureMultiplier FLOAT
		,BloombergSymbol VARCHAR(100)
		,SedolSymbol VARCHAR(100)
		,ISINSymbol VARCHAR(100)
		,CusipSymbol VARCHAR(100)
		,OSISymbol VARCHAR(100)
		,IDCOSymbol VARCHAR(100)
		,ReutersSymbol VARCHAR(100)
		,BloombergSymbolWithExchangeCode VARCHAR(200)
		)

	INSERT INTO #CompanyNamesAndUDAData
	SELECT TickerSymbol
		,CompanyName
		,AssetName
		,SecurityTypeName
		,SectorName
		,SubSectorName
		,CountryName
		,PutOrCall
		,Multiplier
		,BloombergSymbol
		,SedolSymbol
		,ISINSymbol
		,CusipSymbol
		,OSISymbol
		,IDCOSymbol
		,ReutersSymbol
		,BloombergSymbolWithExchangeCode
	FROM V_SecMasterData

	CREATE TABLE #TempSplitFactorForOpen (
		TaxlotID VARCHAR(50)
		,Symbol VARCHAR(100)
		,SplitFactor FLOAT
		)

	INSERT INTO #TempSplitFactorForOpen
	SELECT PT.TaxlotID
		,PT.Symbol
		,SplitFactor
	FROM #PM_Taxlots PT
	INNER JOIN V_CorpActionData VCA
		ON PT.Symbol = VCA.Symbol
			AND DateDiff(day, VCA.EffectiveDate, @Date) = 0
			AND VCA.IsApplied = 1

	DECLARE @RecentDateForNonZeroCash DATETIME

	SET @RecentDateForNonZeroCash = dbo.[GetRecentDateForNonZeroCash](@date)

	-----------------------------------------------------------------------------------------------------                                                                                                                                                
	IF (@reportMode = 0) -- Cost basis mode                                                                                                                                                   
	BEGIN
		INSERT INTO #AUECYesterDates
		SELECT DISTINCT V_SymbolAUEC.AUECID
			,dbo.AdjustBusinessDays(DateAdd(d, 1, @date), - 1, V_SymbolAUEC.AUECID)
		FROM V_SymbolAUEC

		INSERT INTO #DayMarkPrices
		SELECT DayMarkPrice.Symbol
			,0
			,DayMarkPrice.FinalMarkPrice
		FROM PM_DayMarkPrice DayMarkPrice
		INNER JOIN V_SymbolAUEC
			ON DayMarkPrice.Symbol = V_SymbolAUEC.Symbol
		INNER JOIN #AUECYesterDates AUECDates
			ON AUECDates.AUECID = V_SymbolAUEC.AUECID
		WHERE Datediff(d, DayMarkPrice.DATE, AUECDates.YESTERDAYBIZDATE) = 0

		------------------------------------------------------------------------------------------------                                                                                                           
		SELECT Convert(VARCHAR(200), TFPVR.Symbol) AS Symbol
			,OpenQuantity AS Quantity
			,AveragePrice AS CostPrice
			,ISNULL(TFPVR.OpenTotalCommissionandFees, 0) AS TotalCommissionandFees
			,1 AS TotalCostPerShare
			,CASE AUEC.AssetID
				WHEN 5
					THEN 0.0
				ELSE 0.0
				END AS TotalCost
			,0.0 AS MarketValue
			,((1 * OpenQuantity) - (OpenQuantity * AveragePrice)) AS Gain
			,FundName
			,CASE TFPVR.OrderSideTagValue
				WHEN '1'
					THEN 'Long'
				WHEN '2'
					THEN 'Short'
				WHEN '5'
					THEN 'Short' --Sell Short                                                
				WHEN 'A'
					THEN 'Long' --Buy To Open                                                                                                  
				WHEN 'B'
					THEN 'Long' --Buy To Close                                                                                                                                           
				WHEN 'C'
					THEN 'Short' --Sell To Open                                                                                                                   
				WHEN 'D'
					THEN 'Short' --Sell To Close                                   
				END AS [Position Type]
			,C.CurrencySymbol
			,' ' AS LanguageName
			,0.0 AS TotalCostInBaseCurrency
			,Comp.BaseCurrencyID AS BaseCurrencyID
			,'' AS BaseCurrencyLanguageName
			,'' AS GroupParamaeter1
			,'' AS GroupParamaeter2
			,'' AS GroupParamaeter3
			,0.0 AS MarketValueInBaseCurrency
			,0.00 AS GainInBaseCurrency
			,1 AS AggregateOption
			,-- Selected as 1 by default so as not to use aggregate option for the first time load.                                                                         
			0.0 AS YesterdayMarketValue
			,0.0 AS YesterdayMarketValueInBaseCurrency
			,CONVERT(VARCHAR(10), TFPVR.CreationDate, 101) AS TradeDate
			,ISNULL(CompanyNames.FutureMultiplier, 0) AS Multiplier
			,AUEC.AUECID AS AUECID
			,AUEC.AssetID AS AssetID
			,AUEC.BaseCurrencyID AS CurrencyID
			,ISNULL(TFPVR.TradedCurrencyID, 0) AS TradedCurrencyID
			,ISNULL(TFPVR.VsCurrencyID, 0) AS VsCurrencyID
			,IsNull(CMF.MasterFundName, 'Unassigned') AS MasterFundName
			,'' AS GroupParamaeter4
			,isnull(BenchMarkRate + Differential, 0) AS I1
			,DC
			,isnull(DayMark.TodayMarkPrice, 0.0) AS RRMarkPriceCost
			--ISNULL(FXRRConvFactor.ConversionFactor, 0)AS FXRRMarkPriceCost  --this is mark price for forex                         
			-- Previously  FX Spot and Forward Mark Price was picked from FX Rate Table, now it has been updated and Mark Price is picked from the PM_DayMarkPrice table                        
			,isnull(DayMark.TodayMarkPrice, 0.0) AS FXRRMarkPriceCost
			,0.0 AS RRYMarkPriceCost --Not required for cost.                                              
			,0.0 AS FXRRYMarkPriceCost
			,--Not required for cost.                                                                                
			CASE Comp.BaseCurrencyID
				WHEN TFPVR.CurrencyID
					THEN 1
				ELSE CASE ISNULL(TFPVR.FXRate, 0)
						WHEN 0
							THEN IsNull(TDConvFactor.ConversionFactor, 0)
						ELSE TFPVR.FXRate
						END
				END AS TDConversionFactorCost
			,CASE ISNULL(TFPVR.FXRate, 0)
				WHEN 0
					THEN ISNULL(TDConvFactor.ConversionMethod, 0)
				ELSE CASE ISNULL(FXConversionMethodOperator, 'M')
						WHEN 'M'
							THEN 0
						ELSE 1
						END
				END AS TDConversionMethodCost
			,CASE Comp.BaseCurrencyID
				WHEN TFPVR.VsCurrencyID
					THEN 1
				ELSE CASE 
						WHEN TFPVR.FXRate > 0
							THEN TFPVR.FXRate
						ELSE ISNULL(TDConvFactor.ConversionFactor, 0)
						END
				END AS FXTDConversionFactorCost
			,CASE ISNULL(TFPVR.FXRate, 0)
				WHEN 0
					THEN ISNULL(TDConvFactor.ConversionMethod, 0)
				ELSE CASE 
						WHEN ISNULL(FXConversionMethodOperator, 'M') = 'M'
							THEN 0
						WHEN ISNULL(FXConversionMethodOperator, 'D') = 'D'
							THEN 1
						ELSE ISNULL(TDConvFactor.ConversionMethod, 0)
						END
				END AS FXTDConversionMethodCost
			,CASE Comp.BaseCurrencyID
				WHEN TFPVR.CurrencyID
					THEN 1
				ELSE ISNULL(RRConvFactor.ConversionFactor, 0)
				END AS RRConversionFactorCost
			,ISNULL(RRConvFactor.ConversionMethod, 0) AS RRConversionMethodCost
			,CASE Comp.BaseCurrencyID
				WHEN TFPVR.VsCurrencyID
					THEN 1
				ELSE ISNULL(RRConvFactor.ConversionFactor, 0)
				END AS FXRRConversionFactorCost
			,ISNULL(RRConvFactor.ConversionMethod, 0) AS FXRRConversionMethodCost
			,0.0 AS RRMarkPriceDay
			,0.0 AS FXRRMarkPriceDay
			,0.0 AS RRYMarkPriceDay
			,0.0 AS FXRRYMarkPriceDay
			,0.0 AS RRConversionFactorDay
			,0 AS RRConversionMethodDay
			,0.0 AS FXRRConversionFactorDay
			,0 AS FXRRConversionMethodDay
			,0.0 AS RRYConversionFactorDay
			,0 AS RRYConversionMethodDay
			,0.0 AS FXRRYConversionFactorDay
			,0 AS FXRRYConversionMethodDay
			,dbo.GetSideMultiplier(TFPVR.OrderSideTagValue) AS SideMultiplier
			,isnull(NotionalValue, 0) AS NotionalValue
			,CompanyNames.CompanyName AS SymbolCompanyName
			,TFPVR.TaxlotID AS TaxlotID
			,'01-01-1900' AS YesterdayBusinessDate
			,'01-01-1900' AS FXYesterdayBusinessDate
			,CF.CompanyFundID AS CompanyFundID
			,A.AssetName AS AssetName
			,TFPVR.Level2ID AS StrategyID
			,ISNULL(CS.StrategyName, 'Strategy Unallocated') AS StrategyName
			,ISNULL(CompanyNames.Symbol, 'Undefined') AS UDATickerSymbol
			,ISNULL(CompanyNames.UDAAssetName, 'Undefined') AS UDAAssetName
			,ISNULL(CompanyNames.UDASecurityTypeName, 'Undefined') AS UDASecurityTypeName
			,ISNULL(CompanyNames.UDASectorName, 'Undefined') AS UDASectorName
			,ISNULL(CompanyNames.UDASubSectorName, 'Undefined') AS UDASubSectorName
			,ISNULL(CompanyNames.UDACountryName, 'Undefined') AS UDACountryName
			,ISNULL(CompanyNames.PutOrCall, '-1') AS PutOrCall
			,0 FXRRConversionFactorDay2
			,0 FXRRConversionMethodDay2
			,IsNull(CMS.MasterStrategyName, 'Unassigned') AS MasterStrategyName
			,ISNull(TFPVR.UnderlyingSymbol, '') AS UnderlyingSymbol
			,TFPVR.IsSwapped
			,Isnull(CompanyNames.BloombergSymbol, '') AS BloombergSymbol
			,Isnull(CompanyNames.SedolSymbol, '') AS SedolSymbol
			,Isnull(CompanyNames.ISINSymbol, '') AS ISINSymbol
			,Isnull(CompanyNames.CusipSymbol, '') AS CusipSymbol
			,Isnull(CompanyNames.OSISymbol, '') AS OSISymbol
			,Isnull(CompanyNames.IDCOSymbol, '') AS IDCOSymbol
			,ISNULL(CompanyNames.BloombergSymbolWithExchangeCode, '') AS BloombergSymbolWithExchangeCode
		FROM #TEMPFundPositionsForDate_ValReport TFPVR
		INNER JOIN #T_CompanyFunds CF
			ON TFPVR.FundID = CF.CompanyFundID
		INNER JOIN T_Company Comp
			ON Comp.CompanyID = @companyID
		INNER JOIN T_AUEC AUEC
			ON TFPVR.AUECID = AUEC.AUECID
		INNER JOIN #T_Asset A
			ON AUEC.AssetID = A.AssetID
		INNER JOIN T_Currency C
			ON TFPVR.CurrencyID = C.CurrencyID
		LEFT JOIN T_CompanyStrategy CS
			ON TFPVR.Level2ID = CS.CompanyStrategyID
		LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA
			ON CF.CompanyFundID = CMFSSAA.CompanyFundID
		LEFT OUTER JOIN T_CompanyMasterFunds CMF
			ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID
		LEFT OUTER JOIN #DayMarkPrices DayMark
			ON DayMark.Symbol = TFPVR.Symbol
		LEFT OUTER JOIN #AUECYesterDates AUECYesterDates
			ON TFPVR.AUECID = AUECYesterDates.AUECID
		LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange TDConvFactor
			ON TFPVR.CurrencyID = TDConvFactor.FCID
				AND Comp.BaseCurrencyID = TDConvFactor.TCID
				AND DATEDIFF(d, TDConvFactor.DateCC, TFPVR.CreationDate) = 0
		LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange RRConvFactor
			ON TFPVR.CurrencyID = RRConvFactor.FCID
				AND Comp.BaseCurrencyID = RRConvFactor.TCID
				AND DATEDIFF(d, RRConvFactor.DateCC, AUECYesterDates.YESTERDAYBIZDATE) = 0
		--LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXTDConvFactor                                              
		--ON (FXTDConvFactor.FCID = TFPVR.VsCurrencyID                                                                                                                       
		--And FXTDConvFactor.TCID = Comp.BaseCurrencyID)                                
		--AND DATEDIFF(d,FXTDConvFactor.DateCC,TFPVR.CreationDate) = 0                                                         
		--LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXRRConvFactor                                                                        
		--ON (FXRRConvFactor.FCID = TFPVR.TradedCurrencyID                                             
		--And FXRRConvFactor.TCID = TFPVR.VsCurrencyID)                                                                                                                       
		--AND DATEDIFF(d,FXRRConvFactor.DateCC,AUECYesterDates.YESTERDAYBIZDATE) = 0                                             
		--LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXRRConvFactorVsToBase                                                                                                           
		--ON (FXRRConvFactorVsToBase.FCID = TFPVR.VsCurrencyID                                                                    
		--And FXRRConvFactorVsToBase.TCID = Comp.BaseCurrencyID)                                                                                                                       
		--AND DATEDIFF(d,FXRRConvFactorVsToBase.DateCC,AUECYesterDates.YESTERDAYBIZDATE) = 0                                                                                 
		LEFT OUTER JOIN #CompanyNamesAndUDAData CompanyNames
			ON CompanyNames.Symbol = TFPVR.Symbol
		LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA
			ON CS.CompanyStrategyID = CMSSSAA.CompanyStrategyID
		LEFT OUTER JOIN T_CompanyMasterStrategy CMS
			ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID
		WHERE TFPVR.OpenQuantity > 0
		
		UNION ALL
		
		(
			SELECT MIN(C.CurrencySymbol) AS Symbol
				,SUM(CashValueLocal) AS Quantity
				,0.0 AS CostPrice
				,0.0 AS TotalCommissionandFees
				,1 AS TotalCostPerShare
				,0.0 AS TotalCost
				,SUM(CashValueLocal) AS MarketValue
				,0.0 AS Gain
				,MIN(CF.FundName) AS FundName
				,'' AS [Position Type]
				,MIN(C.CurrencySymbol) AS CurrencySymbol
				,' ' AS LanguageName
				,0.0 AS TotalCostInBaseCurrency
				,MIN(BaseCurrencyID) AS BaseCurrencyID
				,'' AS BaseCurrencyLanguageName
				,'' AS GroupParamaeter1
				,'' AS GroupParamaeter2
				,'' AS GroupParamaeter3
				,
				--   CASE ISNULL(MIN(RRConvFactor.ConversionMethod), 0)                                                               
				--    WHEN 0 THEN --MULTILPY                                                                                     
				--     ISNULL(MIN(RRConvFactor.ConversionFactor), 0) * SUM(ISNULL(CashValueLocal, 0))                                                                                                            
				--    WHEN 1 THEN --DIVIDE                                                                                                       
				--  CASE SUM(ISNULL(CashValueLocal, 0))                                                                                                          
				--   WHEN 0 THEN                                                                                                
				--    0                      
				--   ELSE                                                                                      
				--       CASE ISNULL(MIN(RRConvFactor.ConversionFactor), 1)                                                                                                          
				--     WHEN 0 THEN                                                                                                          
				--      0                                                                                                          
				--     ELSE                                                                                                          
				--      (1 / ISNULL(MIN(RRConvFactor.ConversionFactor), 1)) * SUM(ISNULL(CashValueLocal, 0))                                                                                                            
				--   END                                                                                                          
				--  END                                                                     
				--   END                                                    
				SUM(CashValueBase) AS MarketValueInBaseCurrency
				,--As the base value is directly picked from the DB so no need to convert into base market value.                                                                         
				0.00 AS GainInBaseCurrency
				,1 AS AggregateOption
				,-- Selected as 1 by default so as not to use aggregate option for the first time load.                                                                                                              
				0.0 AS YesterdayMarketValue
				,0.0 AS YesterdayMarketValueInBaseCurrency
				,MIN(CFCC.DATE) AS TradeDate
				,1 AS Multiplier
				,0 AS AUECID
				,6 AS AssetID
				,MIN(LocalCurrencyID) AS CurrencyID
				,0 AS TradedCurrencyID
				,0 AS VsCurrencyID
				,MIN(IsNull(CMF.MasterFundName, 'Unassigned')) AS MasterFundName
				,'' AS GroupParamaeter4
				,0.0 AS I1
				,0 AS DC
				,0.0 AS RRMarkPriceCost
				,0.0 AS FXRRMarkPriceCost
				,0.0 AS RRYMarkPriceCost
				,0.0 AS FXRRYMarkPriceCost
				,0.0 AS TDConversionFactorCost
				,0 AS TDConversionMethodCost
				,0.0 AS FXTDConversionFactorCost
				,0 AS FXTDConversionMethodCost
				,0 AS RRConversionFactorCost
				,--As the base value is directly picked from the DB so no need to get coversion rate.                                  
				MIN(RRConvFactor.ConversionMethod) AS RRConversionMethodCost
				,0.0 AS FXRRConversionFactorCost
				,0 AS FXRRConversionMethodCost
				,0.0 AS RRMarkPriceDay
				,0.0 AS FXRRMarkPriceDay
				,0.0 AS RRYMarkPriceDay
				,0.0 AS FXRRYMarkPriceDay
				,0.0 AS RRConversionFactorDay
				,0 AS RRConversionMethodDay
				,0.0 AS FXRRConversionFactorDay
				,0 AS FXRRConversionMethodDay
				,0.0 AS RRYConversionFactorDay
				,0 AS RRYConversionMethodDay
				,0.0 AS FXRRYConversionFactorDay
				,0 AS FXRRYConversionMethodDay
				,0 AS SideMultiplier
				,0.0 AS NotionalValue
				,'' AS SymbolCompanyName
				,'' AS TaxlotID
				,'01-01-1900' AS YesterdayBusinessDate
				,'01-01-1900' AS FXYesterdayBusinessDate
				,MIN(CF.CompanyFundID) AS CompanyFundID
				,'Cash' AS AssetName
				,0 AS StrategyID
				,'Strategy Unallocated' AS StrategyName
				,'Undefined' AS UDATickerSymbol
				,'Undefined' AS UDAAssetName
				,'Undefined' AS UDASecurityTypeName
				,'Undefined' AS UDASectorName
				,'Undefined' AS UDASubSectorName
				,'Undefined' AS UDACountryName
				,'-1' AS PutOrCall
				,0 FXRRConversionFactorDay2
				,0 FXRRConversionMethodDay2
				,'Unassigned' AS MasterStrategyName
				,'' AS UnderlyingSymbol
				,'' AS IsSwapped
				,'' AS BloombergSymbol
				,'' AS SedolSymbol
				,'' AS ISINSymbol
				,'' AS CusipSymbol
				,'' AS OSISymbol
				,'' AS IDCOSymbol
				,'' AS ReutersSymbol
				,'' AS BloombergSymbolWithExchangeCode
			FROM T_DayEndBalances CFCC
			INNER JOIN #T_CompanyFunds CF
				ON CFCC.FundID = CF.CompanyFundID
			INNER JOIN T_Currency C
				ON CFCC.LocalCurrencyID = C.CurrencyID
					AND CFCC.BalanceType = 1
			INNER JOIN T_Currency CBase
				ON BaseCurrencyID = CBase.CurrencyID
			LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA
				ON CF.CompanyFundID = CMFSSAA.CompanyFundID
			LEFT OUTER JOIN T_CompanyMasterFunds CMF
				ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID
			LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange RRConvFactor
				ON CFCC.LocalCurrencyID = RRConvFactor.FCID
					AND CFCC.BaseCurrencyID = RRConvFactor.TCID
					AND DATEDIFF(d, RRConvFactor.DateCC, CFCC.DATE) = 0
			WHERE DateDiff(Day, DATE, @RecentDateForNonZeroCash) = 0
			GROUP BY CFCC.FundID
				,LocalCurrencyID
			)
	END
			--------------------------------------------------------------------------------------------------------                                                     
	ELSE
	BEGIN
		INSERT INTO #AUECYesterDates
		SELECT DISTINCT V_SymbolAUEC.AUECID
			,dbo.AdjustBusinessDays(@date, - 1, V_SymbolAUEC.AUECID)
		FROM V_SymbolAUEC

		INSERT INTO #DayMarkPrices
		SELECT Symbol
			,[YesterDayMarkPrice]
			,[TodayMarkPrice]
		FROM (
			SELECT Symbol
				,[Day]
				,FinalMarkPrice
			FROM (
				SELECT PM_DayMarkPrice.Symbol Symbol
					,CASE 
						WHEN Datediff(d, DATE, AUECYesterDates.YESTERDAYBIZDATE) = 0
							THEN 'YesterDayMarkPrice'
						ELSE 'TodayMarkPrice'
						END AS [Day]
					,FinalMarkPrice
				FROM PM_DayMarkPrice
				INNER JOIN V_SymbolAuec
					ON PM_DayMarkPrice.Symbol = V_SymbolAuec.Symbol
				INNER JOIN #AUECYesterDates AUECYesterDates
					ON AUECYesterDates.AUECID = V_SymbolAuec.AUECID
				WHERE (
						DATEDIFF(d, PM_DayMarkPrice.DATE, @date) = 0
						OR DATEDIFF(d, PM_DayMarkPrice.DATE, AUECYesterDates.YESTERDAYBIZDATE) = 0
						)
				) AS TempDayMarkPrices
			) AS MarkPrices
		PIVOT(MAX(FinalMarkPrice) FOR [Day] IN (
					[YesterDayMarkPrice]
					,[TodayMarkPrice]
					)) AS Pvt;

		SELECT TFPVR.Symbol AS Symbol
			,TFPVR.OpenQuantity AS Quantity
			,TFPVR.AveragePrice AS CostPrice
			,ISNULL(TFPVR.OpenTotalCommissionandFees, 0) AS TotalCommissionandFees
			,1 AS TotalCostPerShare
			,CASE AUEC.AssetID
				WHEN 5
					THEN 0.0
				ELSE 0.0
				END AS TotalCost
			,0.0 AS MarketValue
			,((1 * TFPVR.OpenQuantity) - (TFPVR.OpenQuantity * TFPVR.AveragePrice)) AS Gain
			,FundName
			,CASE TFPVR.OrderSideTagValue
				WHEN '1'
					THEN 'Long'
				WHEN '2'
					THEN 'Short'
				WHEN '5'
					THEN 'Short' --Sell Short                                                                             
				WHEN 'A'
					THEN 'Long' --Buy To Open                                                                                         
				WHEN 'B'
					THEN 'Long' --Buy To Close                                                                                                                                                   
				WHEN 'C'
					THEN 'Short' --Sell To Open                                                                   
				WHEN 'D'
					THEN 'Short' --Sell To Close                                                                                                                                
				END AS [Position Type]
			,C.CurrencySymbol
			,' ' AS LanguageName
			,0.0 AS TotalCostInBaseCurrency
			,Comp.BaseCurrencyID AS BaseCurrencyID
			,'' AS BaseCurrencyLanguageName
			,'' AS GroupParamaeter1
			,'' AS GroupParamaeter2
			,'' AS GroupParamaeter3
			,0.0 AS MarketValueInBaseCurrency
			,0.00 AS GainInBaseCurrency
			,1 AS AggregateOption
			,-- Selected as 1 by default so as not to use aggregate option for the first time load.                                                                                                                                      
			0.0 AS YesterdayMarketValue
			,0.0 AS YesterdayMarketValueInBaseCurrency
			,CONVERT(VARCHAR(10), TFPVR.CreationDate, 101) AS TradeDate
			,ISNULL(CompanyNames.FutureMultiplier, 0) AS Multiplier
			,AUEC.AUECID AS AUECID
			,AUEC.AssetID AS AssetID
			,AUEC.BaseCurrencyID AS CurrencyID
			,ISNULL(TFPVR.TradedCurrencyID, 0) AS TradedCurrencyID
			,ISNULL(TFPVR.VsCurrencyID, 0) AS VsCurrencyID
			,IsNull(CMF.MasterFundName, 'Unassigned') AS MasterFundName
			,'' AS GroupParamaeter4
			,isnull(BenchMarkRate + Differential, 0) AS I1
			,DC
			,0.0 AS RRMarkPriceCost
			,0.0 AS FXRRMarkPriceCost
			,0.0 AS RRYMarkPriceCost
			,0.0 AS FXRRYMarkPriceCost
			,0.0 AS TDConversionFactorCost
			,0 AS TDConversionMethodCost
			,0.0 AS FXTDConversionFactorCost
			,0 AS FXTDConversionMethodCost
			,0.0 AS RRConversionFactorCost
			,0 AS RRConversionMethodCost
			,0.0 AS FXRRConversionFactorCost
			,0 AS FXRRConversionMethodCost
			,ISNULL(PMDMP.TodayMarkPrice, 0.0) AS RRMarkPriceDay
			,ISNULL(FXRRConvFactor.ConversionFactor, 0) AS FXRRMarkPriceDay
			,ISNULL(PMDMP.YesterdayMarkPrice, 0.0) / IsNull(SplitTab.SplitFactor, 1) AS RRYMarkPriceDay
			,ISNULL(FXDayMarkYest.ConversionFactor, 0) AS FXRRYMarkPriceDay
			,CASE Comp.BaseCurrencyID
				WHEN TFPVR.CurrencyID
					THEN 1
				ELSE ISNULL(RRConvFactor.ConversionFactor, 0)
				END AS RRConversionFactorDay
			,isnull(RRConvFactor.ConversionMethod, 0) AS RRConversionMethodDay
			,CASE Comp.BaseCurrencyID
				WHEN TFPVR.VsCurrencyID
					THEN 1
				ELSE ISNULL(RRConvFactor.ConversionFactor, 0)
				END AS FXRRConversionFactorDay
			,isnull(RRConvFactor.ConversionMethod, 0) AS FXRRConversionMethodDay
			,CASE Comp.BaseCurrencyID
				WHEN TFPVR.CurrencyID
					THEN 1
				ELSE ISNULL(RRYConvFactor.ConversionFactor, 0)
				END AS RRYConversionFactorDay
			,ISNULL(RRYConvFactor.ConversionMethod, 0) AS RRYConversionMethodDay
			,CASE Comp.BaseCurrencyID
				WHEN TFPVR.VsCurrencyID
					THEN 1
				ELSE ISNULL(RRYConvFactor.ConversionFactor, 0)
				END AS FXRRYConversionFactorDay
			,isnull(RRYConvFactor.ConversionMethod, 0) AS FXRRYConversionMethodDay
			,dbo.GetSideMultiplier(TFPVR.OrderSideTagValue) AS SideMultiplier
			,isnull(NotionalValue, 0) AS NotionalValue
			,CompanyNames.CompanyName AS SymbolCompanyName
			,TFPVR.TaxlotID AS TaxlotID
			,dbo.GetFormattedDatePart(dbo.AdjustBusinessDays(@date, - 1, TFPVR.AUECID)) AS YesterdayBusinessDate
			,dbo.GetFormattedDatePart(dateadd(d, - 1, @date)) AS FXYesterdayBusinessDate
			,CF.CompanyFundID AS CompanyFundID
			,A.AssetName AS AssetName
			,TFPVR.Level2ID AS StrategyID
			,ISNULL(CS.StrategyName, 'Strategy Unallocated') AS StrategyName
			,ISNULL(CompanyNames.Symbol, 'Undefined') AS UDATickerSymbol
			,ISNULL(CompanyNames.UDAAssetName, 'Undefined') AS UDAAssetName
			,ISNULL(CompanyNames.UDASecurityTypeName, 'Undefined') AS UDASecurityTypeName
			,ISNULL(CompanyNames.UDASectorName, 'Undefined') AS UDASectorName
			,ISNULL(CompanyNames.UDASubSectorName, 'Undefined') AS UDASubSectorName
			,ISNULL(CompanyNames.UDACountryName, 'Undefined') AS UDACountryName
			,ISNULL(CompanyNames.PutOrCall, '-1') AS PutOrCall
			,CASE Comp.BaseCurrencyID
				WHEN TFPVR.VsCurrencyID
					THEN 1
				ELSE CASE 
						WHEN DateDiff(d, TFPVR.CreationDate, @date) = 0
							THEN CASE 
									WHEN TFPVR.FXRate > 0
										THEN TFPVR.FXRate
									ELSE ISNULL(RRConvFactor.ConversionFactor, 0)
									END
						ELSE ISNULL(RRConvFactor.ConversionFactor, 0)
						END
				END AS FXRRConversionFactorDay2
			,CASE 
				WHEN DateDiff(d, TFPVR.CreationDate, @date) = 0
					THEN CASE ISNULL(TFPVR.FXRate, 0)
							WHEN 0
								THEN ISNULL(RRConvFactor.ConversionMethod, 0)
							ELSE CASE 
									WHEN ISNULL(FXConversionMethodOperator, 'M') = 'M'
										AND TFPVR.FXRate > 0
										THEN 0
									WHEN ISNULL(FXConversionMethodOperator, 'D') = 'D'
										AND TFPVR.FXRate > 0
										THEN 1
									ELSE ISNULL(RRConvFactor.ConversionMethod, 0)
									END
							END
				ELSE ISNULL(RRConvFactor.ConversionMethod, 0)
				END AS FXRRConversionMethodDay2
			,IsNull(CMS.MasterStrategyName, 'Unassigned') AS MasterStrategyName
			,ISNull(TFPVR.UnderlyingSymbol, '') AS UnderlyingSymbol
			,TFPVR.IsSwapped
			,Isnull(CompanyNames.BloombergSymbol, '') AS BloombergSymbol
			,Isnull(CompanyNames.SedolSymbol, '') AS SedolSymbol
			,Isnull(CompanyNames.ISINSymbol, '') AS ISINSymbol
			,Isnull(CompanyNames.CusipSymbol, '') AS CusipSymbol
			,Isnull(CompanyNames.OSISymbol, '') AS OSISymbol
			,Isnull(CompanyNames.IDCOSymbol, '') AS IDCOSymbol
			,ISNULL(CompanyNames.BloombergSymbolWithExchangeCode, '') AS BloombergSymbolWithExchangeCode
		FROM #TEMPFundPositionsForDate_ValReport TFPVR
		INNER JOIN T_Company Comp
			ON Comp.CompanyID = @companyID
		INNER JOIN T_AUEC AUEC
			ON TFPVR.AUECID = AUEC.AUECID
		INNER JOIN #T_Asset A
			ON AUEC.AssetID = A.AssetID
		INNER JOIN T_Currency C
			ON TFPVR.CurrencyID = C.CurrencyID
		INNER JOIN #T_CompanyFunds CF
			ON TFPVR.FundID = CF.CompanyFundID
		LEFT JOIN T_CompanyStrategy CS
			ON TFPVR.Level2ID = CS.CompanyStrategyID
		LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA
			ON CF.CompanyFundID = CMFSSAA.CompanyFundID
		LEFT OUTER JOIN T_CompanyMasterFunds CMF
			ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID
		LEFT OUTER JOIN #DayMarkPrices PMDMP
			ON TFPVR.Symbol = PMDMP.Symbol
		LEFT OUTER JOIN #AUECYesterDates AUECYesterDates
			ON TFPVR.AUECID = AUECYesterDates.AUECID
		LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXDayMarkYest
			ON (
					FXDayMarkYest.FCID = TFPVR.TradedCurrencyID
					AND FXDayMarkYest.TCID = TFPVR.VsCurrencyID
					)
				AND DATEDIFF(d, FXDayMarkYest.DateCC, AUECYesterDates.YESTERDAYBIZDATE) = 0
		LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange RRYConvFactor
			ON TFPVR.CurrencyID = RRYConvFactor.FCID
				AND Comp.BaseCurrencyID = RRYConvFactor.TCID
				AND DATEDIFF(d, RRYConvFactor.DateCC, AUECYesterDates.YESTERDAYBIZDATE) = 0
		LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange RRConvFactor
			ON TFPVR.CurrencyID = RRConvFactor.FCID
				AND Comp.BaseCurrencyID = RRConvFactor.TCID
				AND DATEDIFF(d, RRConvFactor.DateCC, @date) = 0
		LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXRRConvFactor
			ON (
					FXRRConvFactor.FCID = TFPVR.TradedCurrencyID
					AND FXRRConvFactor.TCID = TFPVR.VsCurrencyID
					)
				AND DATEDIFF(d, FXRRConvFactor.DateCC, @date) = 0
		--new code 4 lines                                                                      
		--LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXRRConvFactorVsToBase                                                                         
		--ON (FXRRConvFactorVsToBase.FCID = TFPVR.VsCurrencyID                     
		--And FXRRConvFactorVsToBase.TCID = Comp.BaseCurrencyID)                                                                                                                       
		--AND DATEDIFF(d,FXRRConvFactorVsToBase.DateCC,@date) = 0                                                                         
		--LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXRRYConvFactor                                                                                                                       
		--ON (FXRRYConvFactor.FCID = TFPVR.VsCurrencyID                                                                                                                       
		--And FXRRYConvFactor.TCID = Comp.BaseCurrencyID)                                                                       
		--AND DATEDIFF(d,FXRRYConvFactor.DateCC, AUECYesterDates.YESTERDAYBIZDATE) = 0                                                       
		LEFT OUTER JOIN #CompanyNamesAndUDAData CompanyNames
			ON CompanyNames.Symbol = TFPVR.Symbol
		LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA
			ON CS.CompanyStrategyID = CMSSSAA.CompanyStrategyID
		LEFT OUTER JOIN T_CompanyMasterStrategy CMS
			ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID
		LEFT OUTER JOIN #TempSplitFactorForOpen SplitTab
			ON SplitTab.TaxlotID = TFPVR.TaxlotID
		WHERE TFPVR.OpenQuantity > 0
		
		UNION ALL
		
		(
			SELECT MIN(C.CurrencySymbol) AS Symbol
				,SUM(CashValueLocal) AS Quantity
				,0.0 AS CostPrice
				,0.0 AS TotalCommissionandFees
				,1 AS TotalCostPerShare
				,0.0 AS TotalCost
				,SUM(CashValueLocal) AS MarketValue
				,0.0 AS Gain
				,MIN(CF.FundName) AS FundName
				,'' AS [Position Type]
				,MIN(C.CurrencySymbol) AS CurrencySymbol
				,' ' AS LanguageName
				,0.0 AS TotalCostInBaseCurrency
				,MIN(BaseCurrencyID) AS BaseCurrencyID
				,'' AS BaseCurrencyLanguageName
				,'' AS GroupParamaeter1
				,'' AS GroupParamaeter2
				,'' AS GroupParamaeter3
				,SUM(CashValueBase) AS MarketValueInBaseCurrency
				,--As the base value is directly picked from the DB so no need to convert into base market value.                       
				0.00 AS GainInBaseCurrency
				,1 AS AggregateOption
				,-- Selected as 1 by default so as not to use aggregate option for the first time load.                            
				0.0 AS YesterdayMarketValue
				,0.0 AS YesterdayMarketValueInBaseCurrency
				,MIN(CFCC.DATE) AS TradeDate
				,1 AS Multiplier
				,0 AS AUECID
				,6 AS AssetID
				,MIN(LocalCurrencyID) AS CurrencyID
				,0 AS TradedCurrencyID
				,0 AS VsCurrencyID
				,MIN(IsNull(CMF.MasterFundName, 'Unassigned')) AS MasterFundName
				,'' AS GroupParamaeter4
				,0.0 AS I1
				,0 AS DC
				,0.0 AS RRMarkPriceCost
				,0.0 AS FXRRMarkPriceCost
				,0.0 AS RRYMarkPriceCost
				,0.0 AS FXRRYMarkPriceCost
				,0.0 AS TDConversionFactorCost
				,0 AS TDConversionMethodCost
				,0.0 AS FXTDConversionFactorCost
				,0 AS FXTDConversionMethodCost
				,0.0 AS RRConversionFactorCost
				,0 AS RRConversionMethodCost
				,0.0 AS FXRRConversionFactorCost
				,0 AS FXRRConversionMethodCost
				,0.0 AS RRMarkPriceDay
				,0.0 AS FXRRMarkPriceDay
				,0.0 AS RRYMarkPriceDay
				,0.0 AS FXRRYMarkPriceDay
				,0 AS RRConversionFactorDay
				,--As the base value is directly picked from the DB so no need to get coversion rate.                                                                                                      
				MIN(RRConvFactor.ConversionMethod) AS RRConversionMethodDay
				,0.0 AS FXRRConversionFactorDay
				,0 AS FXRRConversionMethodDay
				,0.0 AS RRYConversionFactorDay
				,0 AS RRYConversionMethodDay
				,0.0 AS FXRRYConversionFactorDay
				,0 AS FXRRYConversionMethodDay
				,0 AS SideMultiplier
				,0.0 AS NotionalValue
				,'' AS SymbolCompanyName
				,'' AS TaxlotID
				,'01-01-1900' AS YesterdayBusinessDate
				,'01-01-1900' AS FXYesterdayBusinessDate
				,MIN(CF.CompanyFundID) AS CompanyFundID
				,'Cash' AS AssetName
				,0 AS StrategyID
				,'Strategy Unallocated' AS StrategyName
				,'Undefined' AS UDATickerSymbol
				,'Undefined' AS UDAAssetName
				,'Undefined' AS UDASecurityTypeName
				,'Undefined' AS UDASectorName
				,'Undefined' AS UDASubSectorName
				,'Undefined' AS UDACountryName
				,'-1' AS PutOrCall
				,0 FXRRConversionFactorDay2
				,0 FXRRConversionMethodDay2
				,'Unassigned' AS MasterStrategyName
				,'' AS UnderlyingSymbol
				,'' AS IsSwapped
				,'' AS BloombergSymbol
				,'' AS SedolSymbol
				,'' AS ISINSymbol
				,'' AS CusipSymbol
				,'' AS OSISymbol
				,'' AS IDCOSymbol
				,'' AS ReutersSymbol
				,'' AS BloombergSymbolWithExchangeCode
			FROM T_DayEndBalances CFCC
			INNER JOIN T_Currency C
				ON CFCC.LocalCurrencyID = C.CurrencyID
					AND CFCC.BalanceType = 1
			INNER JOIN T_Currency CBase
				ON BaseCurrencyID = CBase.CurrencyID
			INNER JOIN #T_CompanyFunds CF
				ON CFCC.FundID = CF.CompanyFundID
			LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA
				ON CF.CompanyFundID = CMFSSAA.CompanyFundID
			LEFT OUTER JOIN T_CompanyMasterFunds CMF
				ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID
			LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange RRConvFactor
				ON CFCC.LocalCurrencyID = RRConvFactor.FCID
					AND CFCC.BaseCurrencyID = RRConvFactor.TCID
					AND DATEDIFF(d, RRConvFactor.DateCC, CFCC.DATE) = 0
			WHERE DateDiff(Day, DATE, @RecentDateForNonZeroCash) = 0
			GROUP BY CFCC.FundID
				,LocalCurrencyID
			)
	END

	------------------------------------------------------------------------------------------------------          
	--Dropping temporary tables                                                                                                                                                                  
	DROP TABLE #TEMPGetConversionFactorForGivenDateRange

	DROP TABLE #TEMPFundPositionsForDate_ValReport
		,#PositionTable
		,#DayMarkPrices

	DROP TABLE #AUECYesterDates
		,#CompanyNamesAndUDAData
		,#TempSplitFactorForOpen

	DROP TABLE #PM_Taxlots
		,#T_CompanyFunds
		,#T_Asset
END
