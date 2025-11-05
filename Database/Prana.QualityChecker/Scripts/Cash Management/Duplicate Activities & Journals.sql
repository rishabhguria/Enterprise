Declare @errormsg varchar(max)
declare @FundIds varchar(max)
Declare @FromDate datetime
Declare @ToDate datetime

SET @errormsg = ''
SET @FromDate = ''
SET @ToDate = ''
SET @FundIds = ''


CREATE Table #TempAllActivity(
ActivityID varchar(50),
FundID int,
Symbol varchar(300),
TradeDate datetime,
CurrencyID int,
Amount varchar(50)
)

CREATE Table #WorngEntries(
ActivityID varchar(50),
FundName varchar(300),
Symbol varchar(300),
TradeDate datetime,
CurrencyName varchar(300),
Amount varchar(50)
)

INSERT INTO #TempAllActivity (ActivityID, FundID, Symbol, TradeDate, CurrencyID, Amount)

	SELECT
		ActivityID
		,FundID
		,Symbol
		,TradeDate
		,CurrencyID
		,Amount
	FROM T_AllActivity
	WHERE FKID IN (SELECT
			TA.FKID
		FROM T_AllActivity TA
		INNER JOIN PM_TaxLots PM
			ON PM.TaxLotId = TA.FKID
		INNER JOIN T_Group G
			ON G.GroupId = PM.GroupId
		WHERE TA.TransactionSource = 1
		AND PM.TaxLotClosingId_Fk IS NULL
		AND G.AssetId <> 8
		AND G.AssetId <> 9
		GROUP BY TA.FKID
		HAVING COUNT(TA.FKID) > 1) UNION SELECT
		ActivityID
		,FundID
		,Symbol
		,TradeDate
		,CurrencyID
		,Amount
	FROM T_AllActivity
	WHERE FKID IN (SELECT
			TA.FKID
		FROM T_AllActivity TA
		INNER JOIN PM_TaxLots PM
			ON PM.TaxLotId = TA.FKID
		INNER JOIN T_Group G
			ON G.GroupId = PM.GroupId
		WHERE TA.TransactionSource = 1
		AND PM.TaxLotClosingId_Fk IS NULL
		AND (G.AssetId = 8
		OR G.AssetId = 9)
		GROUP BY TA.FKID
		HAVING COUNT(TA.FKID) > 2)UNION SELECT
		ActivityID
		,FundID
		,Symbol
		,TradeDate
		,CurrencyID
		,Amount
	FROM T_AllActivity
	WHERE FKID NOT IN (SELECT
			TaxLotId
		FROM PM_TaxLots)
	AND TransactionSource = 1

IF  exists (SELECT * FROM #TempAllActivity)
 BEGIN
INSERT INTO #WorngEntries (ActivityID, FundName, Symbol, TradeDate, CurrencyName, Amount)
SELECT
	ActivityID
	,TCF.FundName AS [Fund Name]
	,Symbol
	,TradeDate
	,TC.CurrencyName AS [Currency Name]
	,Amount
FROM #TempAllActivity TAA
INNER JOIN T_CompanyFunds TCF
	ON TAA.FundID = TCF.CompanyFundID
INNER JOIN T_Currency TC
	ON TAA.CurrencyID = TC.CurrencyID
INNER JOIN T_CashPreferences TCP
	ON TCP.FundID = TAA.FundID
WHERE DATEDIFF(D, TCP.CashMgmtStartDate , TAA.TradeDate)>=0

IF  EXISTS (SELECT * FROM #WorngEntries)
BEGIN
SET @errormsg = 'Duplicate Activities & Journals.'
SELECT * FROM #WorngEntries
END
END

SELECT	@errormsg AS ErrorMsg

DROP TABLE #TempAllActivity,#WorngEntries