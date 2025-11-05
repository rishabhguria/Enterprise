--Description: Find if Jounrnal entry is not created for any trade

DECLARE	@FromDate DATETIME 
DECLARE @ToDate DATETIME
DECLARE @FundIds VARCHAR(MAX)
DECLARE @Symbols VARCHAR(MAX)
--Declare @Smdb

SET @FromDate='' 
SET @ToDate='' 
SET @FundIds='' 
SET @Symbols=''
--set @Smdb='' 


DECLARE  @errormsg VARCHAR(max)
SET @errormsg=''

SELECT * INTO #Funds                                      
FROM dbo.Split(@FundIds, ',')

SELECT * INTO #TempSymbol                 
FROM dbo.split(@Symbols , ',')

SELECT DISTINCT * INTO #Symbol                 
FROM #TempSymbol


SELECT * INTO #TempActivity FROM T_AllActivity
WHERE ActivityID NOT IN
(
	SELECT ActivityId_FK FROM T_Journal
)
AND TransactionSource = 1 --AND Amount <> 0

SELECT PMT.* INTO #TempMissingJournal FROM PM_Taxlots PMT
INNER JOIN #TempActivity AC ON AC.FKID = PMT.TaxLotID
INNER JOIN T_CashPreferences TC on TC.FundID=PMT.FundID
WHERE DATEDIFF(D,  PMT.AUECModifiedDate, @FromDate)>=0 AND DATEDIFF(D, PMT.AUECModifiedDate, @ToDate)>=0
AND DATEDIFF(D, TC.CashMgmtStartDate , PMT.AUECModifiedDate)>=0
AND PMT.TaxLotOpenQty<>0 AND PMT.AvgPrice<>0

IF EXISTS (SELECT * FROM #TempMissingJournal)
BEGIN
SET @errormsg=@errormsg+'Missing Journal But Activity Exists.'
	SELECT * FROM #TempMissingJournal
END

SELECT @errormsg AS ErrorMsg

DROP TABLE #TempMissingJournal,#TempActivity
DROP TABLE #Funds,#TempSymbol,#Symbol
