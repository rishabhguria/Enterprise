CREATE PROCEDURE [dbo].[P_GetFundOpenPositionQty_symbol] (
	@ToAllAUECDatesString VARCHAR(MAX)
	,@FundIds VARCHAR(MAX)
	,@Symbols VARCHAR(MAX)
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

	CREATE TABLE #Funds (FundID INT)

	IF (
			@FundIds IS NULL
			OR @FundIds = ''
			)
		INSERT INTO #Funds
		SELECT CompanyFundID AS FundID
		FROM T_CompanyFunds Where IsActive=1
	ELSE
		INSERT INTO #Funds
		SELECT Items AS FundID
		FROM dbo.Split(@FundIds, ',')

	CREATE TABLE #Symbols (symbol VARCHAR(100))

	INSERT INTO #Symbols
	SELECT Items AS Symbol
	FROM dbo.Split(@Symbols, ',')

	CREATE TABLE #TempTaxlotPK (Taxlot_PK BIGINT)

	INSERT INTO #TempTaxlotPK
	SELECT max(taxlot_PK)
	FROM PM_Taxlots
	INNER JOIN T_Group G ON G.GroupID = PM_Taxlots.GroupID
	INNER JOIN T_AUEC AUEC ON AUEC.AUECID = G.AUECID
	INNER JOIN @AUECDatesTable AUECDates ON AUEC.AUECID = AUECDates.AUECID
	INNER JOIN #Symbols ON PM_Taxlots.Symbol = #Symbols.Symbol
	INNER JOIN #Funds ON PM_Taxlots.FundID = #Funds.FundID
	WHERE Datediff(d, PM_Taxlots.AUECModifiedDate, AUECDates.CurrentAUECDate) >= 0
	GROUP BY TaxlotID

	-- Get UTC Date. UTC Date is stored corresponding to AUECID 0 in @AUECDatesTable                                                                                                                                                        
	SELECT PT.Symbol AS Symbol
		,CASE 
			WHEN (
					PT.OrderSideTagValue = '2'
					OR PT.OrderSideTagValue = 'B'
					OR PT.OrderSideTagValue = 'D'
					) THEN -PT.TaxLotOpenQty
					ELSE PT.TaxLotOpenQty
					END AS OpenQty
		,CASE 
			WHEN (
					PT.OrderSideTagValue = '1'
					OR PT.OrderSideTagValue = '2'
					OR PT.OrderSideTagValue = '3'
					OR PT.OrderSideTagValue = 'A'
					OR PT.OrderSideTagValue = 'D'
					)
				THEN 0
			ELSE 1
			END AS PositionTag
	FROM PM_Taxlots PT
	INNER JOIN #TempTaxlotPK TempPK ON TempPK.taxlot_PK = PT.taxlot_PK
	WHERE PT.TaxLotOpenQty <> 0
	ORDER BY TaxlotId

	DROP TABLE #Symbols
		,#Funds
		,#TempTaxlotPK
END
