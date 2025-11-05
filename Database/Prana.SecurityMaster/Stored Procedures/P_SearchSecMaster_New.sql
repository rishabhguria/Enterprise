
CREATE PROCEDURE [dbo].[P_SearchSecMaster_New] (@Xml NVARCHAR(max))
AS
DECLARE @handle INT

EXEC sp_xml_preparedocument @handle OUTPUT
	,@Xml

DECLARE @tickerSymbol VARCHAR(100)
DECLARE @companyName VARCHAR(100)
DECLARE @underlying VARCHAR(100)
DECLARE @bloombergSymbol VARCHAR(100)
DECLARE @factSetSymbol VARCHAR(100)
DECLARE @activSymbol VARCHAR(100)
DECLARE @isinSymbol VARCHAR(100)
DECLARE @SedolSymbol VARCHAR(100)
DECLARE @cusipPSymbol VARCHAR(100)
DECLARE @reutersSymbol VARCHAR(100)
DECLARE @osiOptionSymbol VARCHAR(100)
DECLARE @idcoOptionSymbol VARCHAR(100)
DECLARE @opraOptionSymbol VARCHAR(100)
DECLARE @BBGID VARCHAR(100)
DECLARE @isSecApproved BIT
DECLARE @startIndex INT
DECLARE @endIndex INT

SELECT @tickerSymbol = TickerSymbol
	,@companyName = [Name]
	,@underlying = Underlying
	,@bloombergSymbol = BloombergSymbol
	,@factSetSymbol = FactSetSymbol
	,@activSymbol = ActivSymbol
	,@isinSymbol = ISINSymbol
	,@SedolSymbol = SedolSymbol
	,@cusipPSymbol = CusipSymbol
	,@reutersSymbol = ReutersSymbol
	,@osiOptionSymbol = OSIOptionSymbol
	,@idcoOptionSymbol = IDCOOptionSymbol
	,@opraOptionSymbol = OPRAOptionSymbol
	,@isSecApproved = IsSecApproved
	,@BBGID = BBGID
	,@startIndex = StartIndex
	,@endIndex = EndIndex
FROM OPENXML(@handle, '//SymbolLookupRequestObject', 2) WITH (
		TickerSymbol VARCHAR(100)
		,[Name] VARCHAR(100)
		,Underlying VARCHAR(100)
		,BloombergSymbol VARCHAR(100)
		,FactSetSymbol VARCHAR(100)
		,ActivSymbol VARCHAR(100)
		,ISINSymbol VARCHAR(100)
		,SEDOLSymbol VARCHAR(100)
		,CUSIPSymbol VARCHAR(100)
		,OSIOptionSymbol VARCHAR(100)
		,IDCOOptionSymbol VARCHAR(100)
		,OPRAOptionSymbol VARCHAR(100)
		,ReutersSymbol VARCHAR(100)
		,IsSecApproved BIT
		,BBGID VARCHAR(100)
		,StartIndex INT
		,EndIndex INT
		)

CREATE TABLE #TempSecMasterDatatable (
	AssetID INT
	,UnderLyingID INT
	,ExchangeID INT
	,CurrencyID INT
	,TickerSymbol VARCHAR(100)
	,UnderLyingSymbol VARCHAR(100)
	,ReutersSymbol VARCHAR(100)
	,ISINSymbol VARCHAR(100)
	,SedolSymbol VARCHAR(100)
	,CusipSymbol VARCHAR(100)
	,BloombergSymbol VARCHAR(100)
	,OSISymbol VARCHAR(100)
	,IDCOSymbol VARCHAR(100)
	,OPRASymbol VARCHAR(100)
	,LongName VARCHAR(500)
	,Delta FLOAT
	,Sector VARCHAR(100)
	,Symbol_PK BIGINT
	,OPTMultiplier FLOAT
	,[Type] INT
	,StrikePrice FLOAT
	,OptionName VARCHAR(100)
	,FUTMultiplier FLOAT
	,CutOffTime VARCHAR(100)
	,FutureName VARCHAR(100)
	,AUECID INT
	,OPTExpiration DATETIME
	,FUTExpiration DATETIME
	,LeadCurrencyID INT
	,VsCurrencyID INT
	,FxContractName VARCHAR(100)
	,FXForwardMultiplier FLOAT
	,IndexLongName VARCHAR(200)
	,Multiplier FLOAT
	,IssueDate DATETIME
	,Coupon FLOAT
	,MaturityDate DATETIME
	,BondTypeID INT
	,AccrualBasisID INT
	,FixedIncomeLongName VARCHAR(100)
	,FirstCouponDate DATETIME
	,IsZero BIT
	,CouponFrequencyID INT
	,DaysToSettlement INT
	,FIMultiplier FLOAT
	,CreationDate DATETIME
	,ModifiedDate DATETIME
	,IsNDF BIT
	,FixingDate DATETIME
	,FxMultiplier FLOAT
	,FxExpirationDate DATETIME
	,RoundLot DECIMAL(28,10)
	,ProxySymbol VARCHAR(100)
	,IsSecApproved BIT
	,ApprovalDate DATETIME
	,ApprovedBy VARCHAR(100)
	,Comments VARCHAR(500)
	,UDAAssetClassID INT
	,UDASecurityTypeID INT
	,UDASectorID INT
	,UDASubSectorID INT
	,UDACountryID INT
	,CreatedBy VARCHAR(100)
	,ModifiedBy VARCHAR(100)
	,PrimarySymbology INT
	,BBGID VARCHAR(100)
	,StrikePriceMultiplier FLOAT
	,DataSource INT
	,EsignalOptionRoot VARCHAR(100)
	,BloombergOptionRoot VARCHAR(100)
	,DynamicUDA XML
	,OPTIsCurrencyFuture BIT
	,FUTIsCurrencyFuture BIT
	,CollateralTypeID INT
	,FactSetSymbol VARCHAR(100)
	,ActivSymbol VARCHAR(100)
	,SharesOutstanding FLOAT
	,BloombergSymbolWithExchangeCode VARCHAR(100)
	)

IF (@companyName IS NOT NULL) -- Search By Company Name                                          
BEGIN
	INSERT INTO #TempSecMasterDatatable (
		AssetID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,UnderLyingSymbol
		,ReutersSymbol
		,ISINSymbol
		,SedolSymbol
		,CusipSymbol
		,BloombergSymbol
		,FactSetSymbol
		,ActivSymbol
		,OSISymbol
		,IDCOSymbol
		,OPRASymbol
		,LongName
		,Delta
		,Sector
		,Symbol_PK
		,OPTMultiplier
		,[Type]
		,StrikePrice
		,OptionName
		,FUTMultiplier
		,CutOffTime
		,FutureName
		,AUECID
		,OPTExpiration
		,FUTExpiration
		,LeadCurrencyID
		,VsCurrencyID
		,FxContractName
		,FXForwardMultiplier
		,IndexLongName
		,Multiplier
		,IssueDate
		,Coupon
		,MaturityDate
		,BondTypeID
		,AccrualBasisID
		,FixedIncomeLongName
		,FirstCouponDate
		,IsZero
		,CouponFrequencyID
		,DaysToSettlement
		,FIMultiplier
		,CreationDate
		,ModifiedDate
		,IsNDF
		,FixingDate
		,FxMultiplier
		,FxExpirationDate
		,RoundLot
		,ProxySymbol
		,IsSecApproved
		,ApprovalDate
		,ApprovedBy
		,Comments
		,UDAAssetClassID
		,UDASecurityTypeID
		,UDASectorID
		,UDASubSectorID
		,UDACountryID
		,CreatedBy
		,ModifiedBy
		,PrimarySymbology
		,BBGID
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,DynamicUDA
		,OPTIsCurrencyFuture
		,FUTIsCurrencyFuture
		,CollateralTypeID
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT AssetID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,UnderLyingSymbol
		,ReutersSymbol
		,ISINSymbol
		,SedolSymbol
		,CusipSymbol
		,BloombergSymbol
		,FactSetSymbol
		,ActivSymbol
		,OSISymbol
		,IDCOSymbol
		,OPRASymbol
		,LongName
		,Delta
		,Sector
		,Symbol_PK
		,OPTMultiplier
		,[Type]
		,StrikePrice
		,OptionName
		,FUTMultiplier
		,CutOffTime
		,FutureName
		,AUECID
		,OPTExpiration
		,FUTExpiration
		,LeadCurrencyID
		,VsCurrencyID
		,FxContractName
		,FXForwardMultiplier
		,IndexLongName
		,Multiplier
		,IssueDate
		,Coupon
		,MaturityDate
		,BondTypeID
		,AccrualBasisID
		,FixedIncomeLongName
		,FirstCouponDate
		,IsZero
		,CouponFrequencyID
		,DaysToSettlement
		,FIMultiplier
		,CreationDate
		,ModifiedDate
		,IsNDF
		,FixingDate
		,FxMultiplier
		,FxExpirationDate
		,RoundLot
		,ProxySymbol
		,IsSecApproved
		,ApprovalDate
		,ApprovedBy
		,Comments
		,UDAAssetClassID
		,UDASecurityTypeID
		,UDASectorID
		,UDASubSectorID
		,UDACountryID
		,CreatedBy
		,ModifiedBy
		,PrimarySymbology
		,BBGID
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,DynamicUDA
		,OPTIsCurrencyFuture
		,FUTIsCurrencyFuture
		,CollateralTypeID
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
	FROM (
		SELECT ROW_NUMBER() OVER (
				ORDER BY SM.Symbol_PK
				) AS Row
			,SM.AssetID
			,SM.UnderLyingID
			,T_SMReuters.ExchangeID
			,SM.CurrencyID
			,SM.TickerSymbol
			,SM.UnderLyingSymbol
			,T_SMReuters.ReutersSymbol
			,ISINSymbol
			,SedolSymbol
			,CusipSymbol
			,BloombergSymbol
			,FactSetSymbol
			,ActivSymbol
			,OSISymbol
			,IDCOSymbol
			,OPRASymbol
			,ENHD.CompanyName AS LongName
			,CASE 
				WHEN SM.AssetID = 2
					OR SM.AssetID = 4
					OR SM.AssetID = 10
					THEN OPT.LeveragedFactor
				WHEN SM.AssetID = 3
					THEN FUT.LeveragedFactor
				WHEN SM.AssetID = 5
					THEN FxData.LeveragedFactor
				WHEN SM.AssetID = 7
					THEN IndexData.LeveragedFactor
				WHEN SM.AssetID = 8
					THEN FixedIncomeData.LeveragedFactor
				WHEN SM.AssetID = 11
					THEN FxForwardData.LeveragedFactor
				ELSE ENHD.Delta
				END AS Delta
			,Sector
			,SM.Symbol_PK
			,OPT.Multiplier AS OPTMultiplier
			,OPT.[Type]
			,OPT.Strike AS StrikePrice
			,OPT.ContractName AS OptionName
			,FUT.Multiplier AS FUTMultiplier
			,FUT.CutOffTime
			,FUT.ContractName AS FutureName
			,SM.AUECID
			,OPT.ExpirationDate AS OPTExpiration
			,IsNull(FUT.ExpirationDate, FxForwardData.ExpirationDate) AS FUTExpiration
			,IsNull(FxData.LeadCurrencyID, FxForwardData.LeadCurrencyID) AS LeadCurrencyID
			,IsNull(FxData.VsCurrencyID, FxForwardData.VsCurrencyID) AS VsCurrencyID
			,IsNull(FxData.LongName, FxForwardData.LongName) AS FxContractName
			,FxForwardData.Multiplier AS FXForwardMultiplier
			,IndexData.LongName AS IndexLongName
			,ENHD.Multiplier
			,FixedIncomeData.IssueDate
			,FixedIncomeData.Coupon
			,FixedIncomeData.MaturityDate
			,FixedIncomeData.BondTypeID
			,FixedIncomeData.AccrualBasisID
			,FixedIncomeData.BondDescription AS FixedIncomeLongName
			,FixedIncomeData.FirstCouponDate
			,FixedIncomeData.IsZero
			,FixedIncomeData.CouponFrequencyID
			,FixedIncomeData.DaysToSettlement
			,FixedIncomeData.Multiplier AS FIMultiplier
			,SM.CreationDate
			,SM.ModifiedDate
			,IsNull(FxData.IsNDF, FxForwardData.IsNDF) AS IsNDF
			,IsNull(FxData.FixingDate, FxForwardData.FixingDate) AS FixingDate
			,FxData.Multiplier AS FxMultiplier
			,FxData.ExpirationDate AS FxExpirationDate
			,SM.RoundLot AS RoundLot
			,SM.ProxySymbol AS ProxySymbol
			,SM.IsSecApproved
			,SM.ApprovalDate
			,SM.ApprovedBy
			,SM.Comments
			,SM.UDAAssetClassID
			,SM.UDASecurityTypeID
			,SM.UDASectorID
			,SM.UDASubSectorID
			,SM.UDACountryID
			,SM.CreatedBy
			,SM.ModifiedBy
			,SM.PrimarySymbology
			,SM.BBGID
			,SM.StrikePriceMultiplier
			,SM.DataSource
			,SM.EsignalOptionRoot
			,SM.BloombergOptionRoot
			,SM.SharesOutstanding
			,(SELECT * FROM T_UDA_DynamicUDAData WHERE Symbol_PK = DUDA.Symbol_PK FOR XML PATH ('DynamicUDAs')) AS DynamicUDA
			,OPT.IsCurrencyFuture AS OPTIsCurrencyFuture
			,FUT.IsCurrencyFuture AS FUTIsCurrencyFuture
			,FixedIncomeData.CollateralTypeID
			,SM.BloombergSymbolWithExchangeCode
		FROM T_SMSymbolLookUpTable AS SM with (nolock) 
		LEFT JOIN T_SMReuters  with (nolock) ON SM.Symbol_PK = T_SMReuters.Symbol_PK
			AND T_SMReuters.ExchangeID = SM.ExchangeID AND T_SMReuters.ISPrimaryExchange=1
		LEFT JOIN T_SMEquityNonHistoryData AS ENHD  with (nolock) ON SM.Symbol_PK = ENHD.Symbol_PK
		LEFT JOIN T_SMoptionData AS OPT  with (nolock) ON SM.Symbol_PK = OPT.Symbol_PK
		LEFT JOIN T_SMFutureData AS FUT  with (nolock) ON SM.Symbol_PK = FUT.Symbol_PK
		LEFT JOIN T_SMFxData AS FxData  with (nolock) ON SM.Symbol_PK = FxData.Symbol_PK
		LEFT JOIN T_SMFxForwardData AS FxForwardData  with (nolock) ON SM.Symbol_PK = FxForwardData.Symbol_PK
		LEFT JOIN T_SMIndexData AS IndexData  with (nolock) ON SM.Symbol_PK = IndexData.Symbol_PK
		LEFT JOIN T_SMFixedIncomeData AS FixedIncomeData  with (nolock) ON SM.Symbol_PK = FixedIncomeData.Symbol_PK
		LEFT JOIN T_UDA_DynamicUDAData AS DUDA  with (nolock) ON SM.Symbol_PK = DUDA.Symbol_PK
		WHERE (
				(
					@isSecApproved = 'False'
					AND SM.IsSecApproved = 'false'
					)
				OR (@isSecApproved = 'True')
				)
			AND (
				(
					ENHD.CompanyName LIKE isnull(@companyName, ENHD.CompanyName)
					OR OPT.ContractName LIKE isnull(@companyName, OPT.ContractName)
					OR FUT.ContractName LIKE isnull(@companyName, FUT.ContractName)
					OR FxData.LongName LIKE isnull(@companyName, FxData.LongName)
					OR FxForwardData.LongName LIKE isnull(@companyName, FxForwardData.LongName)
					OR IndexData.LongName LIKE isnull(@companyName, IndexData.LongName)
					)
				OR FixedIncomeData.BondDescription LIKE isnull(@companyName, FixedIncomeData.BondDescription)
				)
		) AS TempSM
	WHERE Row >= @startIndex
		AND Row <= @endIndex
END
ELSE
BEGIN
	INSERT INTO #TempSecMasterDatatable (
		AssetID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,UnderLyingSymbol
		,ReutersSymbol
		,ISINSymbol
		,SedolSymbol
		,CusipSymbol
		,BloombergSymbol
		,FactSetSymbol
		,ActivSymbol
		,OSISymbol
		,IDCOSymbol
		,OPRASymbol
		,LongName
		,Delta
		,Sector
		,Symbol_PK
		,OPTMultiplier
		,[Type]
		,StrikePrice
		,OptionName
		,FUTMultiplier
		,CutOffTime
		,FutureName
		,AUECID
		,OPTExpiration
		,FUTExpiration
		,LeadCurrencyID
		,VsCurrencyID
		,FxContractName
		,FXForwardMultiplier
		,IndexLongName
		,Multiplier
		,IssueDate
		,Coupon
		,MaturityDate
		,BondTypeID
		,AccrualBasisID
		,FixedIncomeLongName
		,FirstCouponDate
		,IsZero
		,CouponFrequencyID
		,DaysToSettlement
		,FIMultiplier
		,CreationDate
		,ModifiedDate
		,IsNDF
		,FixingDate
		,FxMultiplier
		,FxExpirationDate
		,RoundLot
		,ProxySymbol
		,IsSecApproved
		,ApprovalDate
		,ApprovedBy
		,Comments
		,UDAAssetClassID
		,UDASecurityTypeID
		,UDASectorID
		,UDASubSectorID
		,UDACountryID
		,CreatedBy
		,ModifiedBy
		,PrimarySymbology
		,BBGID
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,DynamicUDA
		,OPTIsCurrencyFuture
		,FUTIsCurrencyFuture
		,CollateralTypeID
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT AssetID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,UnderLyingSymbol
		,ReutersSymbol
		,ISINSymbol
		,SedolSymbol
		,CusipSymbol
		,BloombergSymbol
		,FactSetSymbol
		,ActivSymbol
		,OSISymbol
		,IDCOSymbol
		,OPRASymbol
		,LongName
		,Delta
		,Sector
		,Symbol_PK
		,OPTMultiplier
		,[Type]
		,StrikePrice
		,OptionName
		,FUTMultiplier
		,CutOffTime
		,FutureName
		,AUECID
		,OPTExpiration
		,FUTExpiration
		,LeadCurrencyID
		,VsCurrencyID
		,FxContractName
		,FXForwardMultiplier
		,IndexLongName
		,Multiplier
		,IssueDate
		,Coupon
		,MaturityDate
		,BondTypeID
		,AccrualBasisID
		,FixedIncomeLongName
		,FirstCouponDate
		,IsZero
		,CouponFrequencyID
		,DaysToSettlement
		,FIMultiplier
		,CreationDate
		,ModifiedDate
		,IsNDF
		,FixingDate
		,FxMultiplier
		,FxExpirationDate
		,RoundLot
		,ProxySymbol
		,IsSecApproved
		,ApprovalDate
		,ApprovedBy
		,Comments
		,UDAAssetClassID
		,UDASecurityTypeID
		,UDASectorID
		,UDASubSectorID
		,UDACountryID
		,CreatedBy
		,ModifiedBy
		,PrimarySymbology
		,BBGID
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,DynamicUDA
		,OPTIsCurrencyFuture
		,FUTIsCurrencyFuture
		,CollateralTypeID
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
	FROM (
		SELECT ROW_NUMBER() OVER (
				ORDER BY SM.Symbol_PK
				) AS Row
			,SM.AssetID
			,SM.UnderLyingID
			,SM.ExchangeID
			,SM.CurrencyID
			,SM.TickerSymbol
			,SM.UnderLyingSymbol
			,T_SMReuters.ReutersSymbol
			,ISINSymbol
			,SedolSymbol
			,CusipSymbol
			,BloombergSymbol
			,FactSetSymbol
			,ActivSymbol
			,OSISymbol
			,IDCOSymbol
			,OPRASymbol
			,ENHD.CompanyName AS LongName
			,CASE 
				WHEN SM.AssetID = 2
					OR SM.AssetID = 4
					OR SM.AssetID = 10
					THEN OPT.LeveragedFactor
				WHEN SM.AssetID = 3
					THEN FUT.LeveragedFactor
				WHEN SM.AssetID = 5
					THEN FxData.LeveragedFactor
				WHEN SM.AssetID = 7
					THEN IndexData.LeveragedFactor
				WHEN SM.AssetID = 8
					THEN FixedIncomeData.LeveragedFactor
				WHEN SM.AssetID = 11
					THEN FxForwardData.LeveragedFactor
				ELSE ENHD.Delta
				END AS Delta
			,Sector
			,SM.Symbol_PK
			,OPT.Multiplier AS OPTMultiplier
			,OPT.[Type]
			,OPT.Strike AS StrikePrice
			,OPT.ContractName AS OptionName
			,FUT.Multiplier AS FUTMultiplier
			,FUT.CutOffTime
			,FUT.ContractName AS FutureName
			,SM.AUECID
			,OPT.ExpirationDate AS OPTExpiration
			,IsNull(FUT.ExpirationDate, FxForwardData.ExpirationDate) AS FUTExpiration
			,IsNull(FxData.LeadCurrencyID, FxForwardData.LeadCurrencyID) AS LeadCurrencyID
			,IsNull(FxData.VsCurrencyID, FxForwardData.VsCurrencyID) AS VsCurrencyID
			,IsNull(FxData.LongName, FxForwardData.LongName) AS FxContractName
			,FxForwardData.Multiplier AS FXForwardMultiplier
			,IndexData.LongName AS IndexLongName
			,ENHD.Multiplier
			,FixedIncomeData.IssueDate
			,FixedIncomeData.Coupon
			,FixedIncomeData.MaturityDate
			,FixedIncomeData.BondTypeID
			,FixedIncomeData.AccrualBasisID
			,FixedIncomeData.BondDescription AS FixedIncomeLongName
			,FixedIncomeData.FirstCouponDate
			,FixedIncomeData.IsZero
			,FixedIncomeData.CouponFrequencyID
			,FixedIncomeData.DaysToSettlement
			,FixedIncomeData.Multiplier AS FIMultiplier
			,SM.CreationDate
			,SM.ModifiedDate
			,IsNull(FxData.IsNDF, FxForwardData.IsNDF) AS IsNDF
			,IsNull(FxData.FixingDate, FxForwardData.FixingDate) AS FixingDate
			,FxData.Multiplier AS FxMultiplier
			,FxData.ExpirationDate AS FxExpirationDate
			,SM.RoundLot AS RoundLot
			,SM.ProxySymbol AS ProxySymbol
			,SM.IsSecApproved
			,SM.ApprovalDate
			,SM.ApprovedBy
			,SM.Comments
			,SM.UDAAssetClassID
			,SM.UDASecurityTypeID
			,SM.UDASectorID
			,SM.UDASubSectorID
			,SM.UDACountryID
			,SM.CreatedBy
			,SM.ModifiedBy
			,SM.PrimarySymbology
			,SM.BBGID
			,SM.StrikePriceMultiplier
			,SM.DataSource
			,SM.EsignalOptionRoot
			,SM.BloombergOptionRoot
			,SM.SharesOutstanding
			,(SELECT * FROM T_UDA_DynamicUDAData  with (nolock) WHERE Symbol_PK = DUDA.Symbol_PK FOR XML PATH ('DynamicUDAs')) AS DynamicUDA
			,OPT.IsCurrencyFuture AS OPTIsCurrencyFuture
			,FUT.IsCurrencyFuture AS FUTIsCurrencyFuture
			,FixedIncomeData.CollateralTypeID
			,SM.BloombergSymbolWithExchangeCode
		FROM T_SMSymbolLookUpTable AS SM with (nolock) 
		LEFT JOIN T_SMReuters  with (nolock) ON SM.Symbol_PK = T_SMReuters.Symbol_PK
			AND T_SMReuters.ExchangeID = SM.ExchangeID
		LEFT JOIN T_SMEquityNonHistoryData AS ENHD  with (nolock) ON SM.Symbol_PK = ENHD.Symbol_PK
		LEFT JOIN T_SMoptionData AS OPT  with (nolock) ON SM.Symbol_PK = OPT.Symbol_PK
		LEFT JOIN T_SMFutureData AS FUT  with (nolock) ON SM.Symbol_PK = FUT.Symbol_PK
		LEFT JOIN T_SMFxData AS FxData  with (nolock) ON SM.Symbol_PK = FxData.Symbol_PK
		LEFT JOIN T_SMFxForwardData AS FxForwardData  with (nolock) ON SM.Symbol_PK = FxForwardData.Symbol_PK
		LEFT JOIN T_SMIndexData AS IndexData  with (nolock) ON SM.Symbol_PK = IndexData.Symbol_PK
		LEFT JOIN T_SMFixedIncomeData AS FixedIncomeData  with (nolock) ON SM.Symbol_PK = FixedIncomeData.Symbol_PK
		LEFT JOIN T_UDA_DynamicUDAData AS DUDA  with (nolock) ON SM.Symbol_PK = DUDA.Symbol_PK
		WHERE
			-- checking it is primary if security has row in T_SMReuters  
			(
				ISPrimaryExchange = 'true'
				OR ISPrimaryExchange IS NULL
				)
			AND (
				(
					@isSecApproved = 'False'
					AND SM.IsSecApproved = 'false'
					)
				OR (@isSecApproved = 'True')
				)
			-- searching security by symbollogy    
			AND
			-- searching by @tickerSymbol symbol      
			(
				(
					@tickerSymbol IS NOT NULL
     AND SM.TickerSymbol LIKE isnull(@tickerSymbol, SM.TickerSymbol)
	 AND ((('%' + SM.TickerSymbol + '%') <> @tickerSymbol) AND ((SM.TickerSymbol + '%') <> @tickerSymbol))
    OR (@tickerSymbol IS NULL))  
				)
			AND
			-- searching by bloombergSymbol symbol      
			(
    (@bloombergSymbol IS NOT NULL
     AND (
         SM.BloombergSymbol LIKE ISNULL(@bloombergSymbol, SM.BloombergSymbol)
         OR SM.bloombergSymbolWithExchangeCode LIKE ISNULL(@bloombergSymbol, SM.bloombergSymbolWithExchangeCode)
     )
     AND (
         (('%' + SM.BloombergSymbol + '%') <> @bloombergSymbol)
         AND ((SM.BloombergSymbol + '%') <> @bloombergSymbol)
         OR (('%' + SM.bloombergSymbolWithExchangeCode + '%') <> @bloombergSymbol)
         AND ((SM.bloombergSymbolWithExchangeCode + '%') <> @bloombergSymbol)
     )
    )
    OR (@bloombergSymbol IS NULL)
            )
			AND
			-- searching by factsetSymbol symbol      
			(
				(
					@factSetSymbol IS NOT NULL
     AND SM.FactSetSymbol LIKE isnull(@factSetSymbol, SM.FactSetSymbol) 
	AND ((('%' + SM.FactSetSymbol + '%') <> @factSetSymbol) AND ((SM.FactSetSymbol + '%') <> @factSetSymbol)) 
    OR (@factSetSymbol IS NULL))
				)
			AND
			-- searching by activSymbol symbol      
			(
				(
					@activSymbol IS NOT NULL
     AND SM.ActivSymbol LIKE isnull(@activSymbol, SM.ActivSymbol) 
	AND ((('%' + SM.ActivSymbol + '%') <> @activSymbol) AND ((SM.ActivSymbol + '%') <> @activSymbol)) 
    OR (@ActivSymbol IS NULL))
				)
			AND
			-- searching by ISINSymbol symbol      
			(
				(
					@isinSymbol IS NOT NULL
     AND SM.ISINSymbol LIKE isnull(@isinSymbol, SM.ISINSymbol) 
	AND ((('%' + SM.ISINSymbol + '%') <> @isinSymbol)  AND ((SM.ISINSymbol + '%') <> @isinSymbol)) 
    OR (@isinSymbol IS NULL))
				)
			AND
			-- searching by SedolSymbol symbol      
			(
				(
					@SedolSymbol IS NOT NULL
     AND SM.SedolSymbol LIKE isnull(@SedolSymbol, SM.SedolSymbol) 
	AND ((('%' + SM.SedolSymbol + '%') <> @SedolSymbol)   AND ((SM.SedolSymbol + '%') <> @SedolSymbol)) 
    OR (@SedolSymbol IS NULL))
				)
			AND
			-- searching by CusipSymbol symbol      
			(
				(
					@cusipPSymbol IS NOT NULL
     AND SM.CusipSymbol LIKE isnull(@cusipPSymbol, SM.CusipSymbol)
	 AND ((('%' + SM.CusipSymbol + '%') <> @cusipPSymbol) AND ((SM.CusipSymbol + '%') <> @cusipPSymbol)) 
    OR (@cusipPSymbol IS NULL))
				)
			AND
			-- searching by @osiOptionSymbol symbol      
			(
				(
					@osiOptionSymbol IS NOT NULL
     AND SM.OSISymbol LIKE isnull(@osiOptionSymbol, SM.OSISymbol) 
	AND ((('%' + SM.OSISymbol + '%') <> @osiOptionSymbol)  AND ((SM.OSISymbol + '%') <> @osiOptionSymbol)) 
    OR (@osiOptionSymbol IS NULL))
				)
			AND
			-- searching by @idcoOptionSymbol symbol      
			(
				(
					@idcoOptionSymbol IS NOT NULL
     AND SM.IDCOSymbol LIKE isnull(@idcoOptionSymbol, SM.IDCOSymbol) 
	AND ((('%' + SM.IDCOSymbol + '%') <> @idcoOptionSymbol) AND ((SM.IDCOSymbol + '%') <> @idcoOptionSymbol)) 
    OR (@idcoOptionSymbol IS NULL))
				)
			AND
			-- searching by @opraOptionSymbol symbol      
			(
				(
					@opraOptionSymbol IS NOT NULL
     AND SM.OPRASymbol LIKE isnull(@opraOptionSymbol, SM.OPRASymbol)
	 AND((('%' + SM.OPRASymbol + '%') <> @opraOptionSymbol) AND ((SM.OPRASymbol + '%') <> @opraOptionSymbol)) 
    OR (@opraOptionSymbol IS NULL))
				)
			AND
			-- searching by underlying symbol      
			(
				(
					@underlying IS NOT NULL
					AND SM.UnderLyingSymbol LIKE isnull(@underlying, SM.UnderLyingSymbol)
					)
				OR (@underlying IS NULL)
				)
			-- searching by ReutersSymbol        
			AND (
				(
					@reutersSymbol IS NOT NULL
     AND T_SMReuters.ReutersSymbol LIKE isnull(@reutersSymbol, T_SMReuters.ReutersSymbol)  
	AND ((('%' + T_SMReuters.ReutersSymbol + '%') <> @reutersSymbol)  AND ((T_SMReuters.ReutersSymbol + '%') <> @reutersSymbol)) 
    OR (@reutersSymbol IS NULL))
				)
			-- searching by BBGID          
   AND (  
    (  
     @BBGID IS NOT NULL  
     AND SM.BBGID LIKE isnull(@BBGID, SM.BBGID)  
	 AND ((('%%' + SM.BBGID + '%%') <> @BBGID)  AND ((SM.BBGID + '%%') <> @BBGID))   
     OR (@BBGID IS NULL  OR @BBGID = '' )   )  
  )) AS TempSM  
	WHERE Row >= @startIndex
		AND Row <= @endIndex
END

UPDATE	#TempSecMasterDatatable
SET		DynamicUDA.modify('delete /DynamicUDAs/IndexID')

UPDATE	#TempSecMasterDatatable
SET		DynamicUDA.modify('delete /DynamicUDAs/Symbol_PK')

UPDATE	#TempSecMasterDatatable
SET		DynamicUDA.modify('delete /DynamicUDAs/UDAData')

UPDATE	#TempSecMasterDatatable
SET		DynamicUDA.modify('delete /DynamicUDAs/FundID')

SELECT	TSM.Symbol_PK,
		(COALESCE(ENHD.CompanyName, OD.ContractName, FD.ContractName, FXD.LongName, FXFD.LongName, FID.BondDescription, 'Undefined'))
		AS Issuer
INTO #IssuerTable
FROM #TempSecMasterDatatable TSM
LEFT JOIN T_SMSymbolLookUpTable SM  with (nolock) ON SM.TickerSymbol = (
		SELECT UnderLyingSymbol
		FROM T_SMSymbolLookUpTable  with (nolock) 
		WHERE Symbol_PK = TSM.Symbol_PK
		)
LEFT JOIN T_SMEquityNonHistoryData ENHD   with (nolock) ON ENHD.Symbol_PK = SM.Symbol_PK
LEFT JOIN T_SMOptionData OD  with (nolock) ON OD.Symbol_PK = SM.Symbol_PK
LEFT JOIN T_SMFutureData FD  with (nolock) ON FD.Symbol_PK = SM.Symbol_PK
LEFT JOIN T_SMFxData FXD with (nolock)  ON FXD.Symbol_PK = SM.Symbol_PK
LEFT JOIN T_SMFxForwardData FXFD  with (nolock) ON FXFD.Symbol_PK = SM.Symbol_PK
LEFT JOIN T_SMFixedIncomeData FID with (nolock)  ON FID.Symbol_PK = SM.Symbol_PK

DECLARE @Symbol_PK BIGINT
	,@Issuer VARCHAR(100)

WHILE EXISTS (
		SELECT 1
		FROM #IssuerTable
		)
BEGIN
	SET @Symbol_PK = (
			SELECT TOP 1 Symbol_PK
			FROM #IssuerTable
			)

	SELECT @Issuer = Issuer
	FROM #IssuerTable
	WHERE Symbol_PK = @Symbol_PK

	IF (
			(
				SELECT DynamicUDA.exist('/DynamicUDAs/Issuer')
				FROM #TempSecMasterDatatable
				WHERE Symbol_PK = @Symbol_PK
				) = 1
			)
	BEGIN
		UPDATE #TempSecMasterDatatable
		SET DynamicUDA.modify('replace value of (/DynamicUDAs/Issuer/text())[1] with sql:variable("@Issuer")')
		WHERE DynamicUDA.value('(/DynamicUDAs/Issuer/text())[1]', 'VARCHAR(100)') IS NULL
			AND Symbol_PK = @Symbol_PK
			AND DynamicUDA IS NOT NULL
	END
	ELSE
	BEGIN
		UPDATE #TempSecMasterDatatable
		SET DynamicUDA = '<DynamicUDAs />'
		WHERE Symbol_PK = @Symbol_PK
			AND DynamicUDA IS NULL

		UPDATE #TempSecMasterDatatable
		SET DynamicUDA.modify('insert <Issuer>{sql:variable("@Issuer")}</Issuer> into (/DynamicUDAs)[1]')
		WHERE DynamicUDA.value('(/DynamicUDAs/Issuer/text())[1]', 'VARCHAR(100)') IS NULL
			AND Symbol_PK = @Symbol_PK
			AND DynamicUDA IS NOT NULL
	END

	DELETE
	FROM #IssuerTable
	WHERE Symbol_PK = @Symbol_PK
END

SELECT *
FROM #TempSecMasterDatatable

DROP TABLE #TempSecMasterDatatable
	,#IssuerTable
EXEC sp_xml_removedocument @handle