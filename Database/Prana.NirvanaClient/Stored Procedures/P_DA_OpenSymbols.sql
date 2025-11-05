/*

EXEC [P_DA_OpenSymbols] '08-01-2016',1,'',0
https://jira.nirvanasolutions.com:8443/browse/PRANA-30596
Modified By: Anjesh Aggarwal
Date: 12 December 2018
Desc: Added filters using filterId
*/

CREATE PROCEDURE [dbo].[P_DA_OpenSymbols] 
(
	@date DATETIME
	,@filterId INT = 0
	,@errorMessage VARCHAR(max) = NULL OUTPUT
	,@errorNumber INT = 0 OUTPUT
)

AS

--DECLARE @date DateTime  
--DECLARE @filterId int
--DECLARE @errorMessage varchar(max) 
--DECLARE @errorNumber int 

--Set @Date = '2018-12-12'
--SET @filterId = 21
--SET @ErrorMessage = 'Success'                                      
--SET @ErrorNumber = 0  

BEGIN TRY
	-- To ensure no locking, it allows dirty reads, so check for blank symbols and Qty>0
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	DECLARE @Funds VARCHAR(max)

	CREATE TABLE #Funds (FundId INT)

	DECLARE @assets VARCHAR(max)

	CREATE TABLE #Assets (AssetId INT)

	DECLARE @exchanges VARCHAR(max)

	CREATE TABLE #Exchanges (ExchangeId INT)

	DECLARE @auec VARCHAR(max)

	CREATE TABLE #Auecs (AuecId INT)

	IF EXISTS (
			SELECT *
			FROM T_DA_Filters
			WHERE FilterId = @filterId
			)
	BEGIN
		SELECT @Funds = Funds
		FROM T_DA_Filters
		WHERE FilterId = @filterId

		SELECT @assets = Assets
		FROM T_DA_Filters
		WHERE FilterId = @filterId

		SELECT @exchanges = Exchanges
		FROM T_DA_Filters
		WHERE FilterId = @filterId

		SELECT @auec = AUECs
		FROM T_DA_Filters
		WHERE FilterId = @filterId
	END
	ELSE IF (@filterId <> 0)
		RAISERROR (
				'filterId does not exist in T_DA_Filters'
				,16
				,1
				);

	IF (
			@Funds IS NULL
			OR @Funds = '0'
			OR @Funds = ''
			)
	BEGIN
		INSERT INTO #Funds
		SELECT CompanyFundID
		FROM T_CompanyFunds
		Where IsActive=1
	END
	ELSE
	BEGIN
		INSERT INTO #Funds
		SELECT Items
		FROM Split(@Funds, ',')
	END

	IF (
			@assets IS NULL
			OR @assets = '0'
			OR @assets = ''
			)
	BEGIN
		INSERT INTO #Assets
		SELECT AssetID
		FROM T_Asset
	END
	ELSE
	BEGIN
		INSERT INTO #Assets
		SELECT Items
		FROM Split(@assets, ',')
	END

	IF (
			@exchanges IS NULL
			OR @exchanges = '0'
			OR @exchanges = ''
			)
	BEGIN
		INSERT INTO #Exchanges
		SELECT ExchangeID
		FROM T_Exchange
	END
	ELSE
	BEGIN
		INSERT INTO #Exchanges
		SELECT Items
		FROM Split(@exchanges, ',')
	END

	IF (
			@auec IS NULL
			OR @auec = '0'
			OR @auec = ''
			)
	BEGIN
		INSERT INTO #Auecs
		SELECT AUECID
		FROM T_AUEC
	END
	ELSE
	BEGIN
		INSERT INTO #Auecs
		SELECT Items
		FROM Split(@auec, ',')
	END

	CREATE TABLE #Groups (GroupId VARCHAR(50))

	INSERT INTO #Groups
	SELECT GroupID
	FROM T_Group grp
	INNER JOIN #Exchanges exchng ON exchng.ExchangeId = grp.ExchangeID
	INNER JOIN #Auecs auec ON auec.AuecId = grp.AUECID
	INNER JOIN #Assets asset ON asset.AssetId = grp.AssetID

	CREATE TABLE #Taxlotpks (
		Taxlot_PK INT
		,
		)

	INSERT INTO #Taxlotpks
	SELECT Max(Taxlot_PK)
	FROM PM_Taxlots PT
	INNER JOIN #Funds Funds ON PT.FundID = Funds.FundId
	INNER JOIN #Groups grp ON PT.GroupID = grp.GroupId
	WHERE Datediff(Day, PT.AUECModifiedDate, @Date) >= 0
	GROUP BY TaxlotId

	SELECT BloombergSymbol
		,CUSIPSymbol
		,SEDOLSymbol
		,TickerSymbol
		,ISINSymbol
		,ReutersSymbol
		,UnderlyingSymbol
		,AssetID
		,LeadCurrency
		,VsCurrency
		,AUECID
	INTO #TempSMTABLE
	FROM V_SecmasterData

	SELECT DISTINCT SM.BloombergSymbol
		,SM.CUSIPSymbol
		,SM.SEDOLSymbol
		,SM.TickerSymbol
		,SM.ISINSymbol
		,SM.ReutersSymbol
	FROM #TempSMTABLE SM
	INNER JOIN PM_Taxlots ON PM_Taxlots.Symbol = SM.TickerSymbol
	INNER JOIN #Taxlotpks TaxlotPKs ON PM_Taxlots.Taxlot_PK = TaxlotPKs.Taxlot_PK
	WHERE (
			PM_Taxlots.TaxlotOpenQty <> 0
			OR Datediff(d, PM_Taxlots.AUECModifiedDate, @date) <= 0
			)
	
	UNION
	
	-- get underlying symbol 
	SELECT DISTINCT SMUnder.BloombergSymbol
		,SMUnder.CUSIPSymbol
		,SMUnder.SEDOLSymbol
		,SMUnder.TickerSymbol
		,SMUnder.ISINSymbol
		,SMUnder.ReutersSymbol
	FROM PM_Taxlots
	INNER JOIN #taxlotpks TaxlotPKs ON PM_Taxlots.Taxlot_PK = TaxlotPKs.Taxlot_PK
	INNER JOIN #TempSMTABLE SM ON PM_Taxlots.Symbol = SM.TickerSymbol
	INNER JOIN V_SecMasterData_WithUnderlying SMUnder ON SM.UnderlyingSymbol = SMUnder.TickerSymbol
	WHERE (
			PM_Taxlots.TaxlotOpenQty <> 0
			OR Datediff(d, PM_Taxlots.AUECModifiedDate, @date) <= 0
			)
	
	UNION
	
	SELECT DISTINCT LeadCurrency + ' CURNCY' AS BloombergSymbol
		,'' AS CUSIPSymbol
		,'' AS SEDOLSymbol
		,LeadCurrency AS TickerSymbol
		,'' AS ISINSymbol
		,'' AS ReutersSymbol
	FROM #TempSMTABLE TempSM
	INNER JOIN T_AUEC auec ON TempSM.AUECID = auec.AUECID
	INNER JOIN #Auecs auecs ON auecs.AuecId = TempSM.AUECID
	INNER JOIN #Exchanges exchanges ON exchanges.ExchangeId = auec.ExchangeID
	INNER JOIN #Assets assets ON assets.AssetId = TempSM.AssetId
	WHERE TempSM.AssetID IN (
			5
			,11
			)
	
	UNION
	
	SELECT DISTINCT VsCurrency + ' CURNCY' AS BloombergSymbol
		,'' AS CUSIPSymbol
		,'' AS SEDOLSymbol
		,VsCurrency AS TickerSymbol
		,'' AS ISINSymbol
		,'' AS ReutersSymbol
	FROM #TempSMTABLE TempSM
	INNER JOIN T_AUEC auec ON TempSM.AUECID = auec.AUECID
	INNER JOIN #Auecs auecs ON auecs.AuecId = TempSM.AUECID
	INNER JOIN #Exchanges exchanges ON exchanges.ExchangeId = auec.ExchangeID
	INNER JOIN #Assets assets ON assets.AssetId = TempSM.AssetId
	WHERE TempSM.AssetID IN (
			5
			,11
			)
	
	UNION
	
	SELECT DISTINCT CLocal.CurrencySymbol + ' CURNCY' AS BloombergSymbol
		,'' AS CUSIPSymbol
		,'' AS SEDOLSymbol
		,CLocal.CurrencySymbol AS TickerSymbol
		,'' AS ISINSymbol
		,'' AS ReutersSymbol
	FROM PM_CompanyFundCashCurrencyValue CFCC
	INNER JOIN T_Currency CLocal ON CFCC.LocalCurrencyID = CLocal.CurrencyID
	INNER JOIN #Funds Funds ON Funds.FundId = CFCC.FundID

	----Order By TickerSymbol 
	DROP TABLE #Taxlotpks
		,#TempSMTABLE
		,#Funds
		,#Assets
		,#Auecs
		,#Exchanges
		,#Groups
END TRY

BEGIN CATCH
	SET @errorMessage = ERROR_MESSAGE()
	SET @errorNumber = ERROR_NUMBER()

	RAISERROR (
			@errorMessage
			,16
			,1
			)
END CATCH