CREATE PROCEDURE [dbo].[P_SMGetSecurityMasterData_XML] (@xml NTEXT)
AS
DECLARE @handle INT
DECLARE @assetID INT
DECLARE @Symbol_PK VARCHAR(100)

EXEC sp_xml_preparedocument @handle OUTPUT
	,@xml

CREATE TABLE #RequestTable (
	[ReutersSymbol] [varchar](100)
	,[ISINSymbol] [varchar](20)
	,[TickerSymbol] [varchar](100)
	,[CusipSymbol] [varchar](20)
	,[SEDOLSymbol] [varchar](20)
	,[BloombergSymbol] [varchar](200)
	,[OSIOptionSymbol] [varchar](25)
	,[IDCOOptionSymbol] [varchar](25)
	,[OPRAOptionSymbol] [varchar](20)
	,[BBGID] [varchar](100)
	,[FactSetSymbol] [varchar](100)
	,[ActivSymbol] [varchar](100)
	,[BloombergSymbolWithExchangeCode] [varchar](200)
	)

INSERT INTO #RequestTable (
	CusipSymbol
	,SEDOLSymbol
	,ISINSymbol
	,ReutersSymbol
	,TickerSymbol
	,BloombergSymbol
	,OSIOptionSymbol
	,IDCOOptionSymbol
	,OPRAOptionSymbol
	,BBGID
	,FactSetSymbol
	,ActivSymbol
	,BloombergSymbolWithExchangeCode
	)
SELECT DISTINCT CUSIPSymbol
	,SEDOLSymbol
	,ISINSymbol
	,ReutersSymbol
	,TickerSymbol
	,BloombergSymbol
	,OSIOptionSymbol
	,IDCOOptionSymbol
	,OPRAOptionSymbol
	,BBGID
	,FactSetSymbol
	,ActivSymbol
	,BloombergSymbolWithExchangeCode
FROM OPENXML(@handle, '//SecMasterRequest', 2) WITH (
		CUSIPSymbol VARCHAR(20)
		,SEDOLSymbol VARCHAR(20)
		,ISINSymbol VARCHAR(20)
		,ReutersSymbol VARCHAR(100)
		,TickerSymbol VARCHAR(100)
		,BloombergSymbol VARCHAR(200)
		,OSIOptionSymbol VARCHAR(25)
		,IDCOOptionSymbol VARCHAR(25)
		,OPRAOptionSymbol VARCHAR(20)
		,BBGID VARCHAR(100)
		,[FactSetSymbol] [varchar](100)
		,[ActivSymbol] [varchar](100)
	    ,[BloombergSymbolWithExchangeCode] [varchar](200)
	)

CREATE TABLE #ReturnTable (
	AssetID INT
	,UnderLyingSymbol VARCHAR(100)
	,AUECID INT
	,UnderLyingID INT
	,ExchangeID INT
	,CurrencyID INT
	,TickerSymbol VARCHAR(100)
	,ReutersSymbol VARCHAR(100)
	,ISINSymbol VARCHAR(20)
	,SEDOLSymbol VARCHAR(20)
	,CUSIPSymbol VARCHAR(20)
	,BloombergSymbol VARCHAR(200)
	,OSIOptionSymbol VARCHAR(25)
	,IDCOOptionSymbol VARCHAR(25)
	,OPRAOptionSymbol VARCHAR(20)
	,FactSetSymbol VARCHAR(200)
	,ActivSymbol VARCHAR(200)
	,ContractName VARCHAR(50)
	,Sector VARCHAR(20)
	,Symbol_PK VARCHAR(100)
	,Multiplier FLOAT
	,Strike FLOAT
	,[Type] CHAR(1)
	,SettlementDate DATETIME
	,RoundLot DECIMAL(28, 10)
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
	,BBGID VARCHAR(20)
	,CreationDate DATETIME
	,ModifiedDate DATETIME
	,StrikePriceMultiplier FLOAT
	,DataSource INT
	,EsignalOptionRoot VARCHAR(100)
	,BloombergOptionRoot VARCHAR(100)
	,DynamicUDA XML
	,IsCurrencyFuture BIT
	,SharesOutstanding FLOAT
	,BloombergSymbolWithExchangeCode VARCHAR(200)
	)

IF (
		(
			SELECT count(*)
			FROM #RequestTable
			WHERE #RequestTable.CusipSymbol != ''
				AND #RequestTable.CusipSymbol IS NOT NULL
			) > 0
		)
BEGIN
	INSERT INTO #ReturnTable (
		AssetID
		,UnderLyingSymbol
		,AUECID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,ReutersSymbol
		,ISINSymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,OSIOptionSymbol
		,IDCOOptionSymbol
		,OPRAOptionSymbol
		,FactSetSymbol
		,ActivSymbol
		,ContractName
		,Sector
		,Symbol_PK
		,Multiplier
		,Strike
		,[Type]
		,SettlementDate
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
		,CreationDate
		,ModifiedDate
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,IsCurrencyFuture
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT T_SM.AssetID
		,UnderLyingSymbol
		,T_SM.AUECID
		,UnderLyingID
		,T_SM.ExchangeID
		,CurrencyID
		,T_SM.TickerSymbol
		,T_SMReuters.ReutersSymbol
		,T_SM.ISINSymbol
		,T_SM.SEDOLSymbol
		,T_SM.CUSIPSymbol
		,T_SM.BloombergSymbol
		,T_SM.OSISymbol
		,T_SM.IDCOSymbol
		,T_SM.OpraSymbol
		,T_SM.FactSetSymbol
		,T_SM.ActivSymbol
		,''
		,Sector
		,T_SM.Symbol_PK
		,NULL
		,NULL
		,NULL
		,NULL
		,T_SM.RoundLot
		,T_SM.ProxySymbol
		,T_SM.IsSecApproved
		,T_SM.ApprovalDate
		,T_SM.ApprovedBy
		,T_SM.Comments
		,T_SM.UDAAssetClassID
		,T_SM.UDASecurityTypeID
		,T_SM.UDASectorID
		,T_SM.UDASubSectorID
		,T_SM.UDACountryID
		,T_SM.CreatedBy
		,T_SM.ModifiedBy
		,T_SM.PrimarySymbology
		,T_SM.BBGID
		,T_SM.CreationDate
		,T_SM.ModifiedDate
		,T_SM.StrikePriceMultiplier
		,T_SM.DataSource
		,T_SM.EsignalOptionRoot
		,T_SM.BloombergOptionRoot
		,COALESCE(T_Fut.IsCurrencyFuture, T_Opt.IsCurrencyFuture, 0) AS IsCurrencyFuture
		,T_SM.SharesOutstanding
		,T_SM.BloombergSymbolWithExchangeCode
	FROM T_SMSymbolLookUpTable AS T_SM with (nolock)
	 JOIN #RequestTable ON T_SM.CusipSymbol = #RequestTable.CusipSymbol
	 JOIN T_SMReuters ON T_SM.Symbol_PK = T_SMReuters.Symbol_PK
        LEFT JOIN T_SMOptionData AS T_Opt  with (nolock) ON T_SM.Symbol_PK = T_Opt.Symbol_PK
	LEFT JOIN T_SMFutureData AS T_Fut  with (nolock) ON T_SM.Symbol_PK = T_Fut.Symbol_PK	
	LEFT JOIN T_UDA_DynamicUDAData AS D  with (nolock) ON T_SM.Symbol_PK = D.Symbol_PK
	WHERE #RequestTable.CusipSymbol != ''
		AND ISPrimaryExchange = 'true'
END

IF (
		(
			SELECT count(*)
			FROM #RequestTable
			WHERE #RequestTable.ReutersSymbol != ''
				AND #RequestTable.ReutersSymbol IS NOT NULL
			) > 0
		)
BEGIN
	INSERT INTO #ReturnTable (
		AssetID
		,UnderLyingSymbol
		,AUECID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,ReutersSymbol
		,ISINSymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,OSIOptionSymbol
		,IDCOOptionSymbol
		,OPRAOptionSymbol
		,FactSetSymbol
		,ActivSymbol
		,ContractName
		,Sector
		,Symbol_PK
		,Multiplier
		,Strike
		,[Type]
		,SettlementDate
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
		,CreationDate
		,ModifiedDate
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,IsCurrencyFuture
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT T_SM.AssetID
		,UnderLyingSymbol
		,T_SMReuters.AUECID
		,UnderLyingID
		,T_SMReuters.ExchangeID
		,CurrencyID
		,T_SM.TickerSymbol
		,T_SMReuters.ReutersSymbol
		,T_SM.ISINSymbol
		,T_SM.SEDOLSymbol
		,T_SM.CUSIPSymbol
		,T_SM.BloombergSymbol
		,T_SM.OSISymbol
		,T_SM.IDCOSymbol
		,T_SM.OpraSymbol
		,T_SM.FactSetSymbol
		,T_SM.ActivSymbol
		,''
		,Sector
		,T_SM.Symbol_PK
		,NULL
		,NULL
		,NULL
		,NULL
		,T_SM.RoundLot
		,T_SM.ProxySymbol
		,T_SM.IsSecApproved
		,T_SM.ApprovalDate
		,T_SM.ApprovedBy
		,T_SM.Comments
		,T_SM.UDAAssetClassID
		,T_SM.UDASecurityTypeID
		,T_SM.UDASectorID
		,T_SM.UDASubSectorID
		,T_SM.UDACountryID
		,T_SM.CreatedBy
		,T_SM.ModifiedBy
		,T_SM.PrimarySymbology
		,T_SM.BBGID
		,T_SM.CreationDate
		,T_SM.ModifiedDate
		,T_SM.StrikePriceMultiplier
		,T_SM.DataSource
		,T_SM.EsignalOptionRoot
		,T_SM.BloombergOptionRoot
		,COALESCE(T_Fut.IsCurrencyFuture, T_Opt.IsCurrencyFuture, 0) AS IsCurrencyFuture
		,T_SM.SharesOutstanding
		,T_SM.BloombergSymbolWithExchangeCode
	FROM T_SMSymbolLookUpTable AS T_SM  with (nolock)
	INNER JOIN T_SMReuters  with (nolock) ON T_SM.Symbol_PK = T_SMReuters.Symbol_PK
	INNER JOIN #RequestTable ON T_SMReuters.ReutersSymbol = #RequestTable.ReutersSymbol
	LEFT JOIN T_SMOptionData AS T_Opt  with (nolock)ON T_SM.Symbol_PK = T_Opt.Symbol_PK
	LEFT JOIN T_SMFutureData AS T_Fut  with (nolock) ON T_SM.Symbol_PK = T_Fut.Symbol_PK
	LEFT JOIN T_UDA_DynamicUDAData AS D  with (nolock) ON T_SM.Symbol_PK = D.Symbol_PK
	WHERE #RequestTable.ReutersSymbol != ''
		AND ISPrimaryExchange = 'true'
END

IF (
		(
			SELECT count(*)
			FROM #RequestTable
			WHERE (
					#RequestTable.TickerSymbol != ''
					AND #RequestTable.TickerSymbol IS NOT NULL
					)
			) > 0
		)
BEGIN
	INSERT INTO #ReturnTable (
		AssetID
		,UnderLyingSymbol
		,AUECID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,ReutersSymbol
		,ISINSymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,OSIOptionSymbol
		,IDCOOptionSymbol
		,OPRAOptionSymbol
		,FactSetSymbol
		,ActivSymbol
		,ContractName
		,Sector
		,Symbol_PK
		,Multiplier
		,Strike
		,[Type]
		,SettlementDate
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
		,CreationDate
		,ModifiedDate
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,IsCurrencyFuture
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT T_SM.AssetID
		,UnderLyingSymbol
		,T_SM.AUECID
		,UnderLyingID
		,T_SM.ExchangeID
		,CurrencyID
		,T_SM.TickerSymbol
		,T_SMReuters.ReutersSymbol
		,T_SM.ISINSymbol
		,T_SM.SEDOLSymbol
		,T_SM.CUSIPSymbol
		,T_SM.BloombergSymbol
		,T_SM.OSISymbol
		,T_SM.IDCOSymbol
		,T_SM.OpraSymbol
		,T_SM.FactSetSymbol
		,T_SM.ActivSymbol
		,''
		,Sector
		,T_SM.Symbol_PK
		,NULL
		,NULL
		,NULL
		,NULL
		,T_SM.RoundLot
		,T_SM.ProxySymbol
		,T_SM.IsSecApproved
		,T_SM.ApprovalDate
		,T_SM.ApprovedBy
		,T_SM.Comments
		,T_SM.UDAAssetClassID
		,T_SM.UDASecurityTypeID
		,T_SM.UDASectorID
		,T_SM.UDASubSectorID
		,T_SM.UDACountryID
		,T_SM.CreatedBy
		,T_SM.ModifiedBy
		,T_SM.PrimarySymbology
		,T_SM.BBGID
		,T_SM.CreationDate
		,T_SM.ModifiedDate
		,T_SM.StrikePriceMultiplier
		,T_SM.DataSource
		,T_SM.EsignalOptionRoot
		,T_SM.BloombergOptionRoot
		,COALESCE(T_Fut.IsCurrencyFuture, T_Opt.IsCurrencyFuture, 0) AS IsCurrencyFuture
		,T_SM.SharesOutstanding
		,T_SM.BloombergSymbolWithExchangeCode
	FROM T_SMSymbolLookUpTable AS T_SM  with (nolock)
	 JOIN #RequestTable ON T_SM.TickerSymbol = #RequestTable.TickerSymbol
	 JOIN T_SMReuters  with (nolock) ON T_SM.Symbol_PK = T_SMReuters.Symbol_PK
	LEFT JOIN T_SMOptionData AS T_Opt  with (nolock)ON T_SM.Symbol_PK = T_Opt.Symbol_PK
	LEFT JOIN T_SMFutureData AS T_Fut with (nolock) ON T_SM.Symbol_PK = T_Fut.Symbol_PK
	LEFT JOIN T_UDA_DynamicUDAData AS D  with (nolock) ON T_SM.Symbol_PK = D.Symbol_PK
	WHERE #RequestTable.TickerSymbol != ''
		AND ISPrimaryExchange = 'true'
END

IF (
		(
			SELECT count(*)
			FROM #RequestTable
			WHERE #RequestTable.ISINSymbol != ''
				AND #RequestTable.ISINSymbol IS NOT NULL
			) > 0
		)
BEGIN
	INSERT INTO #ReturnTable (
		AssetID
		,UnderLyingSymbol
		,AUECID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,ReutersSymbol
		,ISINSymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,OSIOptionSymbol
		,IDCOOptionSymbol
		,OPRAOptionSymbol
		,FactSetSymbol
		,ActivSymbol
		,ContractName
		,Sector
		,Symbol_PK
		,Multiplier
		,Strike
		,[Type]
		,SettlementDate
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
		,CreationDate
		,ModifiedDate
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,IsCurrencyFuture
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT T_SM.AssetID
		,UnderLyingSymbol
		,T_SM.AUECID
		,UnderLyingID
		,T_SM.ExchangeID
		,CurrencyID
		,T_SM.TickerSymbol
		,T_SMReuters.ReutersSymbol
		,T_SM.ISINSymbol
		,T_SM.SEDOLSymbol
		,T_SM.CUSIPSymbol
		,T_SM.BloombergSymbol
		,T_SM.OSISymbol
		,T_SM.IDCOSymbol
		,T_SM.OpraSymbol
		,T_SM.FactSetSymbol
		,T_SM.ActivSymbol
		,''
		,Sector
		,T_SM.Symbol_PK
		,NULL
		,NULL
		,NULL
		,NULL
		,T_SM.RoundLot
		,T_SM.ProxySymbol
		,T_SM.IsSecApproved
		,T_SM.ApprovalDate
		,T_SM.ApprovedBy
		,T_SM.Comments
		,T_SM.UDAAssetClassID
		,T_SM.UDASecurityTypeID
		,T_SM.UDASectorID
		,T_SM.UDASubSectorID
		,T_SM.UDACountryID
		,T_SM.CreatedBy
		,T_SM.ModifiedBy
		,T_SM.PrimarySymbology
		,T_SM.BBGID
		,T_SM.CreationDate
		,T_SM.ModifiedDate
		,T_SM.StrikePriceMultiplier
		,T_SM.DataSource
		,T_SM.EsignalOptionRoot
		,T_SM.BloombergOptionRoot
		,COALESCE(T_Fut.IsCurrencyFuture, T_Opt.IsCurrencyFuture, 0) AS IsCurrencyFuture
		,T_SM.SharesOutstanding
		,T_SM.BloombergSymbolWithExchangeCode
	FROM T_SMSymbolLookUpTable AS T_SM  with (nolock)
	 JOIN #RequestTable ON T_SM.ISINSymbol = #RequestTable.ISINSymbol
	 JOIN T_SMReuters  with (nolock) ON T_SM.Symbol_PK = T_SMReuters.Symbol_PK
	LEFT JOIN T_SMOptionData AS T_Opt  with (nolock)ON T_SM.Symbol_PK = T_Opt.Symbol_PK
	LEFT JOIN T_SMFutureData AS T_Fut  with (nolock) ON T_SM.Symbol_PK = T_Fut.Symbol_PK
	LEFT JOIN T_UDA_DynamicUDAData AS D  with (nolock) ON T_SM.Symbol_PK = D.Symbol_PK
	WHERE #RequestTable.ISINSymbol != ''
		AND ISPrimaryExchange = 'true'
END

IF (
		(
			SELECT count(*)
			FROM #RequestTable
			WHERE #RequestTable.SEDOLSymbol != ''
				AND #RequestTable.SEDOLSymbol IS NOT NULL
			) > 0
		)
BEGIN
	INSERT INTO #ReturnTable (
		AssetID
		,UnderLyingSymbol
		,AUECID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,ReutersSymbol
		,ISINSymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,OSIOptionSymbol
		,IDCOOptionSymbol
		,OPRAOptionSymbol
		,FactSetSymbol
		,ActivSymbol
		,ContractName
		,Sector
		,Symbol_PK
		,Multiplier
		,Strike
		,[Type]
		,SettlementDate
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
		,CreationDate
		,ModifiedDate
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,IsCurrencyFuture
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT T_SM.AssetID
		,UnderLyingSymbol
		,T_SM.AUECID
		,UnderLyingID
		,T_SM.ExchangeID
		,CurrencyID
		,T_SM.TickerSymbol
		,T_SMReuters.ReutersSymbol
		,T_SM.ISINSymbol
		,T_SM.SEDOLSymbol
		,T_SM.CUSIPSymbol
		,T_SM.BloombergSymbol
		,T_SM.OSISymbol
		,T_SM.IDCOSymbol
		,T_SM.OpraSymbol
		,T_SM.FactSetSymbol
		,T_SM.ActivSymbol
		,''
		,Sector
		,T_SM.Symbol_PK
		,NULL
		,NULL
		,NULL
		,NULL
		,T_SM.RoundLot
		,T_SM.ProxySymbol
		,T_SM.IsSecApproved
		,T_SM.ApprovalDate
		,T_SM.ApprovedBy
		,T_SM.Comments
		,T_SM.UDAAssetClassID
		,T_SM.UDASecurityTypeID
		,T_SM.UDASectorID
		,T_SM.UDASubSectorID
		,T_SM.UDACountryID
		,T_SM.CreatedBy
		,T_SM.ModifiedBy
		,T_SM.PrimarySymbology
		,T_SM.BBGID
		,T_SM.CreationDate
		,T_SM.ModifiedDate
		,T_SM.StrikePriceMultiplier
		,T_SM.DataSource
		,T_SM.EsignalOptionRoot
		,T_SM.BloombergOptionRoot
		,COALESCE(T_Fut.IsCurrencyFuture, T_Opt.IsCurrencyFuture, 0) AS IsCurrencyFuture
		,T_SM.SharesOutstanding
		,T_SM.BloombergSymbolWithExchangeCode
	FROM T_SMSymbolLookUpTable AS T_SM with (nolock)
	 JOIN #RequestTable ON T_SM.SEDOLSymbol = #RequestTable.SEDOLSymbol
	 JOIN T_SMReuters  with (nolock)ON T_SM.Symbol_PK = T_SMReuters.Symbol_PK
	LEFT JOIN T_SMOptionData AS T_Opt  with (nolock)ON T_SM.Symbol_PK = T_Opt.Symbol_PK
	LEFT JOIN T_SMFutureData AS T_Fut  with (nolock)ON T_SM.Symbol_PK = T_Fut.Symbol_PK
	LEFT JOIN T_UDA_DynamicUDAData AS D  with (nolock) ON T_SM.Symbol_PK = D.Symbol_PK
	WHERE #RequestTable.SEDOLSymbol != ''
		AND ISPrimaryExchange = 'true'
END

IF (
		(
			SELECT count(*)
			FROM #RequestTable
			WHERE #RequestTable.BloombergSymbol != ''
				AND #RequestTable.BloombergSymbol IS NOT NULL
			) > 0
		)
BEGIN
	INSERT INTO #ReturnTable (
		AssetID
		,UnderLyingSymbol
		,AUECID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,ReutersSymbol
		,ISINSymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,OSIOptionSymbol
		,IDCOOptionSymbol
		,OPRAOptionSymbol
		,FactSetSymbol
		,ActivSymbol
		,ContractName
		,Sector
		,Symbol_PK
		,Multiplier
		,Strike
		,[Type]
		,SettlementDate
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
		,CreationDate
		,ModifiedDate
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,IsCurrencyFuture
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT T_SM.AssetID
		,UnderLyingSymbol
		,T_SM.AUECID
		,UnderLyingID
		,T_SM.ExchangeID
		,CurrencyID
		,T_SM.TickerSymbol
		,T_SMReuters.ReutersSymbol
		,T_SM.ISINSymbol
		,T_SM.SEDOLSymbol
		,T_SM.CUSIPSymbol
		,T_SM.BloombergSymbol
		,T_SM.OSISymbol
		,T_SM.IDCOSymbol
		,T_SM.OpraSymbol
		,T_SM.FactSetSymbol
		,T_SM.ActivSymbol
		,''
		,Sector
		,T_SM.Symbol_PK
		,NULL
		,NULL
		,NULL
		,NULL
		,T_SM.RoundLot
		,T_SM.ProxySymbol
		,T_SM.IsSecApproved
		,T_SM.ApprovalDate
		,T_SM.ApprovedBy
		,T_SM.Comments
		,T_SM.UDAAssetClassID
		,T_SM.UDASecurityTypeID
		,T_SM.UDASectorID
		,T_SM.UDASubSectorID
		,T_SM.UDACountryID
		,T_SM.CreatedBy
		,T_SM.ModifiedBy
		,T_SM.PrimarySymbology
		,T_SM.BBGID
		,T_SM.CreationDate
		,T_SM.ModifiedDate
		,T_SM.StrikePriceMultiplier
		,T_SM.DataSource
		,T_SM.EsignalOptionRoot
		,T_SM.BloombergOptionRoot
		,COALESCE(T_Fut.IsCurrencyFuture, T_Opt.IsCurrencyFuture, 0) AS IsCurrencyFuture
		,T_SM.SharesOutstanding
		,T_SM.BloombergSymbolWithExchangeCode
	FROM T_SMSymbolLookUpTable AS T_SM  with (nolock)
	 JOIN #RequestTable ON T_SM.BloombergSymbol = #RequestTable.BloombergSymbol
	 JOIN T_SMReuters  with (nolock) ON T_SM.Symbol_PK = T_SMReuters.Symbol_PK
	LEFT JOIN T_SMOptionData AS T_Opt  with (nolock) ON T_SM.Symbol_PK = T_Opt.Symbol_PK
	LEFT JOIN T_SMFutureData AS T_Fut  with (nolock) ON T_SM.Symbol_PK = T_Fut.Symbol_PK
	LEFT JOIN T_UDA_DynamicUDAData AS D  with (nolock) ON T_SM.Symbol_PK = D.Symbol_PK
	WHERE #RequestTable.BloombergSymbol != ''
		AND ISPrimaryExchange = 'true'
END

IF (
		(
			SELECT count(*)
			FROM #RequestTable
			WHERE #RequestTable.OSIOptionSymbol != ''
				AND #RequestTable.OSIOptionSymbol IS NOT NULL
			) > 0
		)
BEGIN
	INSERT INTO #ReturnTable (
		AssetID
		,UnderLyingSymbol
		,AUECID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,ReutersSymbol
		,ISINSymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,OSIOptionSymbol
		,IDCOOptionSymbol
		,OPRAOptionSymbol
		,FactSetSymbol
		,ActivSymbol
		,ContractName
		,Sector
		,Symbol_PK
		,Multiplier
		,Strike
		,[Type]
		,SettlementDate
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
		,CreationDate
		,ModifiedDate
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,IsCurrencyFuture
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT T_SM.AssetID
		,UnderLyingSymbol
		,T_SM.AUECID
		,UnderLyingID
		,T_SM.ExchangeID
		,CurrencyID
		,T_SM.TickerSymbol
		,T_SMReuters.ReutersSymbol
		,T_SM.ISINSymbol
		,T_SM.SEDOLSymbol
		,T_SM.CUSIPSymbol
		,T_SM.BloombergSymbol
		,T_SM.OSISymbol
		,T_SM.IDCOSymbol
		,T_SM.OpraSymbol
		,T_SM.FactSetSymbol
		,T_SM.ActivSymbol
		,''
		,Sector
		,T_SM.Symbol_PK
		,NULL
		,NULL
		,NULL
		,NULL
		,T_SM.RoundLot
		,T_SM.ProxySymbol
		,T_SM.IsSecApproved
		,T_SM.ApprovalDate
		,T_SM.ApprovedBy
		,T_SM.Comments
		,T_SM.UDAAssetClassID
		,T_SM.UDASecurityTypeID
		,T_SM.UDASectorID
		,T_SM.UDASubSectorID
		,T_SM.UDACountryID
		,T_SM.CreatedBy
		,T_SM.ModifiedBy
		,T_SM.PrimarySymbology
		,T_SM.BBGID
		,T_SM.CreationDate
		,T_SM.ModifiedDate
		,T_SM.StrikePriceMultiplier
		,T_SM.DataSource
		,T_SM.EsignalOptionRoot
		,T_SM.BloombergOptionRoot
	,COALESCE(T_Fut.IsCurrencyFuture, T_Opt.IsCurrencyFuture, 0) AS IsCurrencyFuture
	,T_SM.SharesOutstanding
    ,T_SM.BloombergSymbolWithExchangeCode
	FROM T_SMSymbolLookUpTable AS T_SM  with (nolock)
	 JOIN #RequestTable ON T_SM.OSISymbol = #RequestTable.OSIOptionSymbol
	 JOIN T_SMReuters  with (nolock) ON T_SM.Symbol_PK = T_SMReuters.Symbol_PK
	LEFT JOIN T_SMOptionData AS T_Opt  with (nolock)ON T_SM.Symbol_PK = T_Opt.Symbol_PK
	LEFT JOIN T_SMFutureData AS T_Fut  with (nolock)ON T_SM.Symbol_PK = T_Fut.Symbol_PK
	LEFT JOIN T_UDA_DynamicUDAData AS D  with (nolock) ON T_SM.Symbol_PK = D.Symbol_PK
	WHERE #RequestTable.OSIOptionSymbol != ''
		AND ISPrimaryExchange = 'true'
END

IF (
		(
			SELECT count(*)
			FROM #RequestTable
			WHERE #RequestTable.IDCOOptionSymbol != ''
				AND #RequestTable.IDCOOptionSymbol IS NOT NULL
			) > 0
		)
BEGIN
	INSERT INTO #ReturnTable (
		AssetID
		,UnderLyingSymbol
		,AUECID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,ReutersSymbol
		,ISINSymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,OSIOptionSymbol
		,IDCOOptionSymbol
		,OPRAOptionSymbol
		,FactSetSymbol
		,ActivSymbol
		,ContractName
		,Sector
		,Symbol_PK
		,Multiplier
		,Strike
		,[Type]
		,SettlementDate
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
		,CreationDate
		,ModifiedDate
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
        ,IsCurrencyFuture
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT T_SM.AssetID
		,UnderLyingSymbol
		,T_SM.AUECID
		,UnderLyingID
		,T_SM.ExchangeID
		,CurrencyID
		,T_SM.TickerSymbol
		,T_SMReuters.ReutersSymbol
		,T_SM.ISINSymbol
		,T_SM.SEDOLSymbol
		,T_SM.CUSIPSymbol
		,T_SM.BloombergSymbol
		,T_SM.OSISymbol
		,T_SM.IDCOSymbol
		,T_SM.OpraSymbol
		,T_SM.FactSetSymbol
		,T_SM.ActivSymbol
		,''
		,Sector
		,T_SM.Symbol_PK
		,NULL
		,NULL
		,NULL
		,NULL
		,T_SM.RoundLot
		,T_SM.ProxySymbol
		,T_SM.IsSecApproved
		,T_SM.ApprovalDate
		,T_SM.ApprovedBy
		,T_SM.Comments
		,T_SM.UDAAssetClassID
		,T_SM.UDASecurityTypeID
		,T_SM.UDASectorID
		,T_SM.UDASubSectorID
		,T_SM.UDACountryID
		,T_SM.CreatedBy
		,T_SM.ModifiedBy
		,T_SM.PrimarySymbology
		,T_SM.BBGID
		,T_SM.CreationDate
		,T_SM.ModifiedDate
		,T_SM.StrikePriceMultiplier
		,T_SM.DataSource
		,T_SM.EsignalOptionRoot
		,T_SM.BloombergOptionRoot
	,COALESCE(T_Fut.IsCurrencyFuture, T_Opt.IsCurrencyFuture, 0) AS IsCurrencyFuture
	,T_SM.SharesOutstanding
	,T_SM.BloombergSymbolWithExchangeCode
	FROM T_SMSymbolLookUpTable AS T_SM  with (nolock)
	INNER JOIN #RequestTable ON T_SM.IDCOSymbol = #RequestTable.IDCOOptionSymbol
	INNER JOIN T_SMReuters  with (nolock)ON T_SM.Symbol_PK = T_SMReuters.Symbol_PK
	LEFT JOIN T_SMOptionData AS T_Opt  with (nolock)ON T_SM.Symbol_PK = T_Opt.Symbol_PK
	LEFT JOIN T_SMFutureData AS T_Fut  with (nolock)ON T_SM.Symbol_PK = T_Fut.Symbol_PK
	LEFT JOIN T_UDA_DynamicUDAData AS D  with (nolock) ON T_SM.Symbol_PK = D.Symbol_PK
	WHERE #RequestTable.IDCOOptionSymbol != ''
		AND ISPrimaryExchange = 'true'
END

IF (
		(
			SELECT count(*)
			FROM #RequestTable
			WHERE #RequestTable.OPRAOptionSymbol != ''
				AND #RequestTable.OPRAOptionSymbol IS NOT NULL
			) > 0
		)
BEGIN
	INSERT INTO #ReturnTable (
		AssetID
		,UnderLyingSymbol
		,AUECID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,ReutersSymbol
		,ISINSymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,OSIOptionSymbol
		,IDCOOptionSymbol
		,OPRAOptionSymbol
		,FactSetSymbol
		,ActivSymbol
		,ContractName
		,Sector
		,Symbol_PK
		,Multiplier
		,Strike
		,[Type]
		,SettlementDate
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
		,CreationDate
		,ModifiedDate
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,IsCurrencyFuture
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT T_SM.AssetID
		,UnderLyingSymbol
		,T_SM.AUECID
		,UnderLyingID
		,T_SM.ExchangeID
		,CurrencyID
		,T_SM.TickerSymbol
		,T_SMReuters.ReutersSymbol
		,T_SM.ISINSymbol
		,T_SM.SEDOLSymbol
		,T_SM.CUSIPSymbol
		,T_SM.BloombergSymbol
		,T_SM.OSISymbol
		,T_SM.IDCOSymbol
		,T_SM.OpraSymbol
		,T_SM.FactSetSymbol
		,T_SM.ActivSymbol
		,''
		,Sector
		,T_SM.Symbol_PK
		,NULL
		,NULL
		,NULL
		,NULL
		,T_SM.RoundLot
		,T_SM.ProxySymbol
		,T_SM.IsSecApproved
		,T_SM.ApprovalDate
		,T_SM.ApprovedBy
		,T_SM.Comments
		,T_SM.UDAAssetClassID
		,T_SM.UDASecurityTypeID
		,T_SM.UDASectorID
		,T_SM.UDASubSectorID
		,T_SM.UDACountryID
		,T_SM.CreatedBy
		,T_SM.ModifiedBy
		,T_SM.PrimarySymbology
		,T_SM.BBGID
		,T_SM.CreationDate
		,T_SM.ModifiedDate
		,T_SM.StrikePriceMultiplier
		,T_SM.DataSource
		,T_SM.EsignalOptionRoot
		,T_SM.BloombergOptionRoot
	,COALESCE(T_Fut.IsCurrencyFuture, T_Opt.IsCurrencyFuture, 0) AS IsCurrencyFuture
	,T_SM.SharesOutstanding
	,T_SM.BloombergSymbolWithExchangeCode
	FROM T_SMSymbolLookUpTable AS T_SM with (nolock) 
	 JOIN #RequestTable ON T_SM.OPRASymbol = #RequestTable.OPRAOptionSymbol
	 JOIN T_SMReuters  with (nolock) ON T_SM.Symbol_PK = T_SMReuters.Symbol_PK
	LEFT JOIN T_SMOptionData AS T_Opt  with (nolock) ON T_SM.Symbol_PK = T_Opt.Symbol_PK
	LEFT JOIN T_SMFutureData AS T_Fut  with (nolock) ON T_SM.Symbol_PK = T_Fut.Symbol_PK
	LEFT JOIN T_UDA_DynamicUDAData AS D  with (nolock) ON T_SM.Symbol_PK = D.Symbol_PK
	WHERE #RequestTable.OPRAOptionSymbol != ''
		AND ISPrimaryExchange = 'true'
END

IF (
		(
			SELECT count(*)
			FROM #RequestTable
			WHERE #RequestTable.BBGID != ''
				AND #RequestTable.BBGID IS NOT NULL
			) > 0
		)
BEGIN
	INSERT INTO #ReturnTable (
		AssetID
		,UnderLyingSymbol
		,AUECID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,ReutersSymbol
		,ISINSymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,OSIOptionSymbol
		,IDCOOptionSymbol
		,OPRAOptionSymbol
		,FactSetSymbol
		,ActivSymbol
		,ContractName
		,Sector
		,Symbol_PK
		,Multiplier
		,Strike
		,[Type]
		,SettlementDate
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
		,CreationDate
		,ModifiedDate
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
	,IsCurrencyFuture
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT T_SM.AssetID
		,UnderLyingSymbol
		,T_SM.AUECID
		,UnderLyingID
		,T_SM.ExchangeID
		,CurrencyID
		,T_SM.TickerSymbol
		,T_SMReuters.ReutersSymbol
		,T_SM.ISINSymbol
		,T_SM.SEDOLSymbol
		,T_SM.CUSIPSymbol
		,T_SM.BloombergSymbol
		,T_SM.OSISymbol
		,T_SM.IDCOSymbol
		,T_SM.OpraSymbol
		,T_SM.FactSetSymbol
		,T_SM.ActivSymbol
		,''
		,Sector
		,T_SM.Symbol_PK
		,NULL
		,NULL
		,NULL
		,NULL
		,T_SM.RoundLot
		,T_SM.ProxySymbol
		,T_SM.IsSecApproved
		,T_SM.ApprovalDate
		,T_SM.ApprovedBy
		,T_SM.Comments
		,T_SM.UDAAssetClassID
		,T_SM.UDASecurityTypeID
		,T_SM.UDASectorID
		,T_SM.UDASubSectorID
		,T_SM.UDACountryID
		,T_SM.CreatedBy
		,T_SM.ModifiedBy
		,T_SM.PrimarySymbology
		,T_SM.BBGID
		,T_SM.CreationDate
		,T_SM.ModifiedDate
		,T_SM.StrikePriceMultiplier
		,T_SM.DataSource
		,T_SM.EsignalOptionRoot
		,T_SM.BloombergOptionRoot
		,COALESCE(T_Fut.IsCurrencyFuture, T_Opt.IsCurrencyFuture, 0) AS IsCurrencyFuture
		,T_SM.SharesOutstanding
	    ,T_SM.BloombergSymbolWithExchangeCode
	FROM T_SMSymbolLookUpTable AS T_SM  with (nolock) 
	 JOIN #RequestTable ON T_SM.BBGID = #RequestTable.BBGID
	 JOIN T_SMReuters  with (nolock) ON T_SM.Symbol_PK = T_SMReuters.Symbol_PK
	LEFT JOIN T_SMOptionData AS T_Opt  with (nolock) ON T_SM.Symbol_PK = T_Opt.Symbol_PK
	LEFT JOIN T_SMFutureData AS T_Fut  with (nolock) ON T_SM.Symbol_PK = T_Fut.Symbol_PK
	INNER JOIN T_UDA_DynamicUDAData AS D  with (nolock) ON T_SM.Symbol_PK = D.Symbol_PK
	WHERE #RequestTable.BBGID != ''
		AND ISPrimaryExchange = 'true'
END

IF (
		(
			SELECT count(*)
			FROM #RequestTable
			WHERE #RequestTable.FactSetSymbol != ''
				AND #RequestTable.FactSetSymbol IS NOT NULL
			) > 0
		)
BEGIN
	INSERT INTO #ReturnTable (
		AssetID
		,UnderLyingSymbol
		,AUECID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,ReutersSymbol
		,ISINSymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,OSIOptionSymbol
		,IDCOOptionSymbol
		,OPRAOptionSymbol
		,FactSetSymbol
		,ActivSymbol
		,ContractName
		,Sector
		,Symbol_PK
		,Multiplier
		,Strike
		,[Type]
		,SettlementDate
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
		,CreationDate
		,ModifiedDate
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,IsCurrencyFuture
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT T_SM.AssetID
		,UnderLyingSymbol
		,T_SM.AUECID
		,UnderLyingID
		,T_SM.ExchangeID
		,CurrencyID
		,T_SM.TickerSymbol
		,T_SMReuters.ReutersSymbol
		,T_SM.ISINSymbol
		,T_SM.SEDOLSymbol
		,T_SM.CUSIPSymbol
		,T_SM.BloombergSymbol
		,T_SM.OSISymbol
		,T_SM.IDCOSymbol
		,T_SM.OpraSymbol
		,T_SM.FactSetSymbol
		,T_SM.ActivSymbol
		,''
		,Sector
		,T_SM.Symbol_PK
		,NULL
		,NULL
		,NULL
		,NULL
		,T_SM.RoundLot
		,T_SM.ProxySymbol
		,T_SM.IsSecApproved
		,T_SM.ApprovalDate
		,T_SM.ApprovedBy
		,T_SM.Comments
		,T_SM.UDAAssetClassID
		,T_SM.UDASecurityTypeID
		,T_SM.UDASectorID
		,T_SM.UDASubSectorID
		,T_SM.UDACountryID
		,T_SM.CreatedBy
		,T_SM.ModifiedBy
		,T_SM.PrimarySymbology
		,T_SM.BBGID
		,T_SM.CreationDate
		,T_SM.ModifiedDate
		,T_SM.StrikePriceMultiplier
		,T_SM.DataSource
		,T_SM.EsignalOptionRoot
		,T_SM.BloombergOptionRoot
		,COALESCE(T_Fut.IsCurrencyFuture, T_Opt.IsCurrencyFuture, 0) AS IsCurrencyFuture
		,T_SM.SharesOutstanding
	    ,T_SM.BloombergSymbolWithExchangeCode
	FROM T_SMSymbolLookUpTable AS T_SM with (nolock) 
	 JOIN #RequestTable ON T_SM.FactSetSymbol = #RequestTable.FactSetSymbol
	 JOIN T_SMReuters  with (nolock) ON T_SM.Symbol_PK = T_SMReuters.Symbol_PK
	LEFT JOIN T_SMOptionData AS T_Opt  with (nolock) ON T_SM.Symbol_PK = T_Opt.Symbol_PK
	LEFT JOIN T_SMFutureData AS T_Fut  with (nolock) ON T_SM.Symbol_PK = T_Fut.Symbol_PK
	INNER JOIN T_UDA_DynamicUDAData AS D  with (nolock) ON T_SM.Symbol_PK = D.Symbol_PK
	WHERE #RequestTable.FactSetSymbol != ''
		AND ISPrimaryExchange = 'true'
END

IF (
		(
			SELECT count(*)
			FROM #RequestTable
			WHERE #RequestTable.ActivSymbol != ''
				AND #RequestTable.ActivSymbol IS NOT NULL
			) > 0
		)
BEGIN
	INSERT INTO #ReturnTable (
		AssetID
		,UnderLyingSymbol
		,AUECID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,TickerSymbol
		,ReutersSymbol
		,ISINSymbol
		,SEDOLSymbol
		,CUSIPSymbol
		,BloombergSymbol
		,OSIOptionSymbol
		,IDCOOptionSymbol
		,OPRAOptionSymbol
		,FactSetSymbol
		,ActivSymbol
		,ContractName
		,Sector
		,Symbol_PK
		,Multiplier
		,Strike
		,[Type]
		,SettlementDate
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
		,CreationDate
		,ModifiedDate
		,StrikePriceMultiplier
		,DataSource
		,EsignalOptionRoot
		,BloombergOptionRoot
		,IsCurrencyFuture
		,SharesOutstanding
		,BloombergSymbolWithExchangeCode
		)
	SELECT T_SM.AssetID
		,UnderLyingSymbol
		,T_SM.AUECID
		,UnderLyingID
		,T_SM.ExchangeID
		,CurrencyID
		,T_SM.TickerSymbol
		,T_SMReuters.ReutersSymbol
		,T_SM.ISINSymbol
		,T_SM.SEDOLSymbol
		,T_SM.CUSIPSymbol
		,T_SM.BloombergSymbol
		,T_SM.OSISymbol
		,T_SM.IDCOSymbol
		,T_SM.OpraSymbol
		,T_SM.FactSetSymbol
		,T_SM.ActivSymbol
		,''
		,Sector
		,T_SM.Symbol_PK
		,NULL
		,NULL
		,NULL
		,NULL
		,T_SM.RoundLot
		,T_SM.ProxySymbol
		,T_SM.IsSecApproved
		,T_SM.ApprovalDate
		,T_SM.ApprovedBy
		,T_SM.Comments
		,T_SM.UDAAssetClassID
		,T_SM.UDASecurityTypeID
		,T_SM.UDASectorID
		,T_SM.UDASubSectorID
		,T_SM.UDACountryID
		,T_SM.CreatedBy
		,T_SM.ModifiedBy
		,T_SM.PrimarySymbology
		,T_SM.BBGID
		,T_SM.CreationDate
		,T_SM.ModifiedDate
		,T_SM.StrikePriceMultiplier
		,T_SM.DataSource
		,T_SM.EsignalOptionRoot
		,T_SM.BloombergOptionRoot
		,COALESCE(T_Fut.IsCurrencyFuture, T_Opt.IsCurrencyFuture, 0) AS IsCurrencyFuture
		,T_SM.SharesOutstanding
	    ,T_SM.BloombergSymbolWithExchangeCode
	FROM T_SMSymbolLookUpTable AS T_SM with (nolock) 
	 JOIN #RequestTable ON T_SM.ActivSymbol = #RequestTable.ActivSymbol
	 JOIN T_SMReuters  with (nolock) ON T_SM.Symbol_PK = T_SMReuters.Symbol_PK
	LEFT JOIN T_SMOptionData AS T_Opt  with (nolock) ON T_SM.Symbol_PK = T_Opt.Symbol_PK
	LEFT JOIN T_SMFutureData AS T_Fut  with (nolock) ON T_SM.Symbol_PK = T_Fut.Symbol_PK
	INNER JOIN T_UDA_DynamicUDAData AS D  with (nolock) ON T_SM.Symbol_PK = D.Symbol_PK
	WHERE #RequestTable.ActivSymbol != ''
		AND ISPrimaryExchange = 'true'
END

-------------Update Issuer Field ------------

SELECT (
COALESCE(DUDA.[Issuer], 
NULLIF(ENHD.CompanyName,''),
NULLIF(OD.ContractName,''), 
NULLIF(FD.ContractName,''), 
NULLIF(FXD.LongName,''), 
NULLIF(FXFD.LongName,''), 
NULLIF(FID.BondDescription,''),
'Undefined')
  ) as Issuer,
DUDA.Symbol_PK as Symbol_PK
,DUDA.Analyst
,DUDA.CountryOfRisk
,DUDA.CustomUDA1
,DUDA.CustomUDA2
,DUDA.CustomUDA3
,DUDA.CustomUDA4
,DUDA.CustomUDA5
,DUDA.CustomUDA6
,DUDA.CustomUDA7
,DUDA.LiquidTag
,DUDA.MarketCap
,DUDA.Region
,DUDA.RiskCurrency
,DUDA.UCITSEligibleTag
,DUDA.CustomUDA8
,DUDA.CustomUDA9
,DUDA.CustomUDA10
,DUDA.CustomUDA11
,DUDA.CustomUDA12
into #IssuerSymbolPK
FROM T_UDA_DynamicUDAData DUDA  with (nolock)
Inner join #ReturnTable on DUDA.Symbol_PK= #ReturnTable.Symbol_PK
LEFT JOIN  T_SMSymbolLookUpTable SM		 with (nolock) ON SM.Symbol_PK = DUDA.Symbol_PK 
LEFT JOIN  T_SMFxData FXDRC				 with (nolock) ON FXDRC.Symbol_PK = SM.Symbol_PK 
LEFT JOIN  T_SMFxForwardData FXFDRC		 with (nolock) ON FXFDRC.Symbol_PK = SM.Symbol_PK 
LEFT JOIN  T_SMSymbolLookUpTable SMU		 with (nolock) ON SMU.TickerSymbol = SM.UnderLyingSymbol 
LEFT JOIN  T_SMEquityNonHistoryData ENHD with (nolock) 	ON ENHD.Symbol_PK = SMU.Symbol_PK 
LEFT JOIN  T_SMOptionData OD			 with (nolock) 	ON OD.Symbol_PK = SMU.Symbol_PK 
LEFT JOIN  T_SMFutureData FD				 with (nolock) ON FD.Symbol_PK = SMU.Symbol_PK 
LEFT JOIN  T_SMFxData FXD					 with (nolock) ON FXD.Symbol_PK = SMU.Symbol_PK 
LEFT JOIN  T_SMFxForwardData FXFD			 with (nolock) ON FXFD.Symbol_PK = SMU.Symbol_PK 
LEFT JOIN  T_SMFixedIncomeData FID		 with (nolock) ON FID.Symbol_PK = SMU.Symbol_PK

Create table #FinalIssuerSymbolPK(symbol_pk varchar(100) ,DynamicUDADataXML xml )

Insert into #FinalIssuerSymbolPK(symbol_pk  ,DynamicUDADataXML  )
 select symbol_pk ,
(
SELECT *
FROM #IssuerSymbolPK
where #IssuerSymbolPK.Symbol_PK=temp.Symbol_PK
FOR XML PATH('DynamicUDAs')) as DynamicUDADataXML
from #IssuerSymbolPK temp
 
 
UPDATE #FinalIssuerSymbolPK
SET DynamicUDADataXML.modify('delete /DynamicUDAs/Symbol_PK')

           
UPDATE RT
SET DynamicUDA=ISPK.DynamicUDADataXML
from #ReturnTable RT
inner join #FinalIssuerSymbolPK  ISPK on RT.Symbol_PK= ISPK.Symbol_PK

--------update Issuer done-------------

SELECT #ReturnTable.AssetID
	,#ReturnTable.UnderLyingSymbol
	,#ReturnTable.AUECID
	,#ReturnTable.UnderLyingID
	,#ReturnTable.ExchangeID
	,#ReturnTable.CurrencyID
	,#ReturnTable.TickerSymbol
	,isnull(#ReturnTable.ReutersSymbol, '') AS ReutersSymbol
	,isnull(#ReturnTable.ISINSymbol, '') AS ISINSymbol
	,isnull(#ReturnTable.SEDOLSymbol, '') AS SEDOLSymbol
	,isnull(#ReturnTable.CUSIPSymbol, '') AS CUSIPSymbol
	,isnull(#ReturnTable.BloombergSymbol, '') AS BloombergSymbol
	,#ReturnTable.OSIOptionSymbol
	,#ReturnTable.IDCOOptionSymbol
	,#ReturnTable.OPRAOptionSymbol
	,isnull(#ReturnTable.FactSetSymbol, '') AS FactSetSymbol
	,isnull(#ReturnTable.ActivSymbol, '') AS ACTIVSymbol
	,T_SMEquityNonHistoryData.CompanyName
	,#ReturnTable.Sector
	,#ReturnTable.Symbol_PK
	,CASE 
		WHEN #ReturnTable.AssetID = 2
			THEN COALESCE(UnderlyingEquityData.Delta, UnderlyingIndexData.LeveragedFactor, 1)
		WHEN #ReturnTable.AssetID = 4
			THEN COALESCE(UnderlyingFutureData.LeveragedFactor, 1)
		WHEN #ReturnTable.AssetID = 10
			THEN COALESCE(UnderlyingFXData.LeveragedFactor, 1)
		WHEN #ReturnTable.AssetID = 3
			THEN COALESCE(FUT.leveragedfactor, 1)
		WHEN #ReturnTable.AssetID = 5
			THEN COALESCE(FxData.leveragedfactor, 1)
		WHEN #ReturnTable.AssetID = 7
			THEN COALESCE(IndexData.leveragedfactor, 1)
		WHEN #ReturnTable.AssetID = 8
			THEN COALESCE(FixedIncomeData.leveragedfactor, 1)
		WHEN #ReturnTable.AssetID = 11
			THEN COALESCE(FxForwardData.leveragedfactor, 1)
		WHEN #ReturnTable.AssetID = 14
			THEN COALESCE(UnderlyingEquityData.Delta, 1)
		ELSE COALESCE(T_SMEquityNonHistoryData.Delta, 1)
		END AS delta
	,OPT.Multiplier
	,OPT.[Type]
	,OPT.Strike
	,OPT.ExpirationDate
	,OPT.ContractName
	,FUT.Multiplier
	,FUT.ExpirationDate
	,FUT.ContractName
	,FUT.CutOffTime
	,FxData.LeadCurrencyID
	,FxData.VsCurrencyID
	,FxData.LongName
	,FxForwardData.LeadCurrencyID
	,FxForwardData.VsCurrencyID
	,FxForwardData.LongName
	,FxForwardData.ExpirationDate
	,FxForwardData.Multiplier
	,T_SMEquityNonHistoryData.Multiplier
	,FixedIncomeData.IssueDate
	,FixedIncomeData.Coupon
	,FixedIncomeData.MaturityDate
	,FixedIncomeData.BondTypeID
	,FixedIncomeData.AccrualBasisID
	,FixedIncomeData.BondDescription
	,FixedIncomeData.FirstCouponDate
	,FixedIncomeData.IsZero
	,FixedIncomeData.CouponFrequencyID
	,FixedIncomeData.DaysToSettlement
	,FixedIncomeData.Multiplier
	,FxData.IsNDF
	,FxData.FixingDate
	,FxForwardData.IsNDF
	,FxForwardData.FixingDate
	,FxData.Multiplier
	,FxData.ExpirationDate
	,#ReturnTable.RoundLot
	,#ReturnTable.ProxySymbol
	,#ReturnTable.IsSecApproved
	,#ReturnTable.ApprovalDate
	,#ReturnTable.ApprovedBy
	,#ReturnTable.Comments
	,#ReturnTable.UDAAssetClassID
	,#ReturnTable.UDASecurityTypeID
	,#ReturnTable.UDASectorID
	,#ReturnTable.UDASubSectorID
	,#ReturnTable.UDACountryID
	,T_UDAAssetClass.AssetName
	,T_UDASecurityType.SecurityTypeName
	,T_UDASector.SectorName
	,T_UDASubSector.SubSectorName
	,T_UDACountry.CountryName
	,#ReturnTable.CreatedBy
	,#ReturnTable.ModifiedBy
	,#ReturnTable.PrimarySymbology
	,#ReturnTable.BBGID
	,#ReturnTable.CreationDate
	,#ReturnTable.ModifiedDate
	,#ReturnTable.StrikePriceMultiplier
	,#ReturnTable.DataSource
	,#ReturnTable.EsignalOptionRoot
	,#ReturnTable.BloombergOptionRoot
	,#ReturnTable.DynamicUDA
	,#ReturnTable.IsCurrencyFuture
	,FixedIncomeData.CollateralTypeID
	,#ReturnTable.SharesOutstanding
	,#ReturnTable.BloombergSymbolWithExchangeCode
FROM #ReturnTable
LEFT JOIN T_SMEquityNonHistoryData  with (nolock)  ON #ReturnTable.Symbol_PK = T_SMEquityNonHistoryData.Symbol_PK
LEFT JOIN T_SMFutureData FUT  with (nolock) ON #ReturnTable.Symbol_PK = FUT.Symbol_PK
LEFT JOIN T_SMOptionData OPT  with (nolock) ON #ReturnTable.Symbol_PK = OPT.Symbol_PK
LEFT JOIN T_SMFxData FxData  with (nolock) ON #ReturnTable.Symbol_PK = FxData.Symbol_PK
LEFT JOIN T_SMFxForwardData FxForwardData  with (nolock) ON #ReturnTable.Symbol_PK = FxForwardData.Symbol_PK
LEFT JOIN T_SMFixedIncomeData FixedIncomeData  with (nolock) ON #ReturnTable.Symbol_PK = FixedIncomeData.Symbol_PK
LEFT JOIN T_SMIndexData IndexData  with (nolock) ON #ReturnTable.Symbol_PK = IndexData.Symbol_PK
LEFT JOIN T_SMSymbolLookUpTable UnderlyingSM  with (nolock) ON #ReturnTable.UnderLyingSymbol = UnderlyingSM.TickerSymbol
LEFT JOIN T_SMEquityNonHistoryData UnderlyingEquityData  with (nolock) ON UnderlyingSM.Symbol_PK = UnderlyingEquityData.Symbol_PK
LEFT JOIN T_SMFutureData UnderlyingFutureData  with (nolock) ON UnderlyingSM.Symbol_PK = UnderlyingFutureData.Symbol_PK
LEFT JOIN T_SMFxData UnderlyingFXData  with (nolock) ON UnderlyingSM.Symbol_PK = UnderlyingFXData.Symbol_PK
LEFT JOIN T_SMIndexData UnderlyingIndexData  with (nolock) ON UnderlyingSM.Symbol_PK = UnderlyingIndexData.Symbol_PK
LEFT JOIN T_UDAAssetClass  with (nolock) ON #ReturnTable.UDAAssetClassID = T_UDAAssetClass.AssetID
LEFT JOIN T_UDASecurityType  with (nolock) ON #ReturnTable.UDASecurityTypeID = T_UDASecurityType.SecurityTypeID
LEFT JOIN T_UDASector  with (nolock)  ON #ReturnTable.UDASectorID = T_UDASector.SectorID
LEFT JOIN T_UDASubSector with (nolock)  ON #ReturnTable.UDASubSectorID = T_UDASubSector.SubSectorID
LEFT JOIN T_UDACountry  with (nolock) ON #ReturnTable.UDACountryID = T_UDACountry.CountryID

EXEC sp_xml_removedocument @handle

DROP TABLE #RequestTable
	,#ReturnTable ,#FinalIssuerSymbolPK ,#IssuerSymbolPK 
	--,#IssuerTable