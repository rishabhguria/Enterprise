CREATE PROCEDURE [dbo].[P_WC_SaveRealtimeData] @Xml NVARCHAR(max)
	,@XmlIndices XML
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
AS
SET @ErrorNumber = 0
SET @ErrorMessage = 'Success'

BEGIN TRY
	DECLARE @handle INT
	DECLARE @CreatedOn DATETIME

	SELECT @CreatedOn = GETUTCDATE()

	EXEC sp_xml_preparedocument @handle OUTPUT
		,@Xml

	INSERT INTO T_PMDataDump (
		[Account]
		,[Symbol]
		,[Security Name]
		,[Asset Class]
		,[NAV (Touch)]
		,[Position]
		,[Beta Adj. Exposure (Base)]
		,[Day P&L (Base)]
		,[Closing Mark]
		,[Px Last]
		,[Underlying Symbol]
		,[Underlying Price]
		,[Cost Basis P&L (Base)]
		,[Market Value (Base)]
		,[Net Exposure (Base)]
		,[Sector]
		,[Sub Sector]
		,[% Change]
		,[CreatedOn]
		,[Cost Basis]
		,[Country]
		,[Trade Currency]
		,[Delta Adj. Position]
		,[Px Selected Feed (Local)]
		,[Px Selected Feed (Base)]
		,[Cash Impact (Base)]
		,[Earned Dividend (Base)]
		--Added the columns as per the jira https://jira.nirvanasolutions.com:8443/browse/TG-1790
		--Modified by Pankaj Sharma
		,[Risk Currency]
		,[Issuer]
		,[Country Of Risk]
		,[Region]
		,[Analyst]
		,[Market Cap]
		,[Custom UDA1]
		,[Custom UDA2]
		,[Custom UDA3]
		,[Custom UDA4]
		,[Custom UDA5]
		,[Custom UDA6]
		,[Custom UDA7]
		,[User Asset]
		,[Security Type]
		,[Pricing Source]
		,[ISIN]
		,[BloombergSymbol]
		,[CUSIP]
		,[SEDOL]
		,Multiplier
		,ContractType
		,FxRate	
		,Delta
		,Beta
		,ItmOtm
		,PercentOfITMOTM
		,IntrinsicValue
		,DaysToExpiry
		,GainLossIfExerciseAssign
		,[Gross Exposure (Local)]
		,[Gross Exposure (Base)]
		,[Net Exposure (Local)]
		,[Exposure (Local)]
		,[Exposure (Base)]
		,[Beta Adj. Exposure (Local)]
		,[Strategy]
		,[Master Strategy]
		,[Trade Attribute 1]
		,[Trade Attribute 2]
		,[Trade Attribute 3]
		,[Trade Attribute 4]
		,[Trade Attribute 5]
		,[Trade Attribute 6]
		,[Expiration Date]
		,[Expiration Month]
		,[Custom UDA8]
		,[Custom UDA9]
		,[Custom UDA10]
		,[Custom UDA11]
		,[Custom UDA12]
		,[BloombergSymbolWithExchangeCode]
		)
	SELECT Level1Name
		,Symbol
		,FullSecurityName
		,Asset
		,NavTouch
		,Quantity
		,BetaAdjExposureInBaseCurrency
		,DayPnLInBaseCurrency
		,YesterdayMarkPriceStr
		,LastPrice
		,UnderlyingSymbol
		,UnderlyingStockPrice
		,CostBasisUnrealizedPnLInBaseCurrency
		,MarketValueInBaseCurrency
		,NetExposureInBaseCurrency
		,UDASector
		,UDASubSector
		,PercentageChange
		,@CreatedOn
		,AvgPrice
		,UDACountry
		,CurrencySymbol
		,DeltaAdjPosition
		,SelectedFeedPrice
		,SelectedFeedPriceInBaseCurrency
		,CashImpactInBaseCurrency
		,EarnedDividendBase
		--Added the columns as per the jira https://jira.nirvanasolutions.com:8443/browse/TG-1790
		,RiskCurrency
		,Issuer
		,CountryOfRisk
		,Region
		,Analyst
		,MarketCap
		,CustomUDA1
		,CustomUDA2
		,CustomUDA3
		,CustomUDA4
		,CustomUDA5
		,CustomUDA6
		,CustomUDA7
		,UDAAsset
		,UDASecurityType
		,PricingSource
		,IsinSymbol
		,BloombergSymbol
		,CusipSymbol
		,SedolSymbol
		,Multiplier
		,ContractType
		,FxRate
		,Delta
		,Beta
		,ItmOtm
		,PercentOfITMOTM
		,IntrinsicValue
		,DaysToExpiry
		,GainLossIfExerciseAssign
		,GrossExposureLocal
		,GrossExposure
		,NetExposure
		,Exposure
		,ExposureInBaseCurrency
		,BetaAdjExposure
		,Level2Name
		,MasterStrategy
		,TradeAttribute1
		,TradeAttribute2
		,TradeAttribute3
		,TradeAttribute4
		,TradeAttribute5
		,TradeAttribute6
		,ExpirationDate
		,ExpirationMonth
		,CustomUDA8
		,CustomUDA9
		,CustomUDA10
		,CustomUDA11
		,CustomUDA12
		,BloombergSymbolWithExchangeCode
	FROM OPENXML(@handle, 'ExposurePnlCacheItemList/System.Collections.IEnumerable/ExposurePnlCacheItem', 2) WITH (
			Level1Name [nvarchar](Max)
			,Symbol [nvarchar](Max)
			,FullSecurityName [nvarchar](Max)
			,Asset [nvarchar](Max)
			,NavTouch [nvarchar](Max)
			,Quantity [nvarchar](Max)
			,BetaAdjExposureInBaseCurrency [nvarchar](Max)
			,DayPnLInBaseCurrency [nvarchar](Max)
			,YesterdayMarkPriceStr [nvarchar](Max)
			,LastPrice [nvarchar](Max)
			,UnderlyingSymbol [nvarchar](Max)
			,UnderlyingStockPrice [nvarchar](Max)
			,CostBasisUnrealizedPnLInBaseCurrency [nvarchar](Max)
			,MarketValueInBaseCurrency [nvarchar](Max)
			,NetExposureInBaseCurrency [nvarchar](Max)
			,UDASector [nvarchar](Max)
			,UDASubSector [nvarchar](Max)
			,PercentageChange [nvarchar](Max)
			,CreatedOn [datetime]
			,AvgPrice [nvarchar](Max)
			,UDACountry [nvarchar](Max)
			,CurrencySymbol [nvarchar](Max)
			,DeltaAdjPosition [nvarchar](Max)
			,SelectedFeedPrice [nvarchar](Max)
			,SelectedFeedPriceInBaseCurrency [nvarchar](Max)
			,CashImpactInBaseCurrency [nvarchar](Max)
			,EarnedDividendBase [nvarchar](Max)
			--Added the columns as per the jira https://jira.nirvanasolutions.com:8443/browse/TG-1790
			,RiskCurrency [nvarchar](MAX)
			,Issuer [nvarchar](MAX)
			,CountryOfRisk [nvarchar](MAX)
			,Region [nvarchar](MAX)
			,Analyst [nvarchar](MAX)
			,MarketCap [nvarchar](MAX)
			,CustomUDA1 [nvarchar](MAX)
			,CustomUDA2 [nvarchar](MAX)
			,CustomUDA3 [nvarchar](MAX)
			,CustomUDA4 [nvarchar](MAX)
			,CustomUDA5 [nvarchar](MAX)
			,CustomUDA6 [nvarchar](MAX)
			,CustomUDA7 [nvarchar](MAX)
			,UDAAsset [nvarchar](MAX)
			,UDASecurityType [nvarchar](MAX)
			,PricingSource [nvarchar](MAX)
			,IsinSymbol [nvarchar](50)
			,BloombergSymbol [nvarchar](200)
			,CusipSymbol [nvarchar](50)
			,SedolSymbol [nvarchar](50)
			,Multiplier [nvarchar](50)
		    ,ContractType [nvarchar](50)
			,FxRate [nvarchar](50)
			,Delta [nvarchar](100)
			,Beta [nvarchar](100)
			,ItmOtm [nvarchar](50)
			,PercentOfITMOTM [nvarchar](50)
			,IntrinsicValue [nvarchar](50)
			,DaysToExpiry [nvarchar](50)
			,GainLossIfExerciseAssign [nvarchar](50)
			,GrossExposureLocal [nvarchar](50)
			,GrossExposure [nvarchar](50)
			,NetExposure [nvarchar](50)
			,Exposure [nvarchar](50)
			,ExposureInBaseCurrency [nvarchar](50)
			,BetaAdjExposure [nvarchar](50)
			,Level2Name [nvarchar](200)
			,MasterStrategy [nvarchar](200)
			,TradeAttribute1 [nvarchar](200)
			,TradeAttribute2 [nvarchar](200)
			,TradeAttribute3 [nvarchar](200)
			,TradeAttribute4 [nvarchar](200)
			,TradeAttribute5 [nvarchar](200)
			,TradeAttribute6 [nvarchar](200)
			,ExpirationDate [datetime]
			,ExpirationMonth [nvarchar](50)
			,CustomUDA8 [nvarchar](MAX)
			,CustomUDA9 [nvarchar](MAX)
			,CustomUDA10 [nvarchar](MAX)
			,CustomUDA11 [nvarchar](MAX)
			,CustomUDA12 [nvarchar](MAX)
			,BloombergSymbolWithExchangeCode [nvarchar](200)
			)

	EXEC sp_xml_removedocument @handle

	INSERT INTO T_PMIndicesDataDump (
		[IndexSymbol]
		,[Caption]
		,[Performance]
		,[CreatedOn]
		)
	SELECT ref.value('IndexSymbol[1]', 'nvarchar(Max)') AS [IndexSymbol]
		,ref.value('Caption[1]', 'nvarchar(Max)') AS [Caption]
		,ref.value('Performance[1]', 'nvarchar(Max)') AS [Performance]
		,@CreatedOn AS [CreatedOn]
	FROM @XmlIndices.nodes('/NewDataSet/IndicesPerformance') xmlData(ref)
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();
END CATCH;
