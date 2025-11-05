--Description: Find if Revaluation Journal entry is duplicate for any Trade

declare	@FromDate Datetime 
declare @ToDate Datetime
declare @ErrorMsg Varchar(Max)

set @FromDate='' 
set @ToDate=''
set @ErrorMsg=''


Select Symbol,FundID,SubAccountID,PBDesc,TransactionDate,DR,CR,COUNT(*) as NumberOfEntries into #TempDuplicateJournal from T_Journal 
Where TransactionSource = 9 and Symbol <> ''
And DateDiff(D, TransactionDate, @FromDate)<=0 AND DATEDIFF(D, TransactionDate, @ToDate)>=0
Group By Symbol,FundID,SubAccountID,PBDesc,TransactionDate,DR,CR
Having count(*) > 1
Order By Symbol,TransactionDate


IF EXISTS (SELECT * FROM #TempDuplicateJournal)
BEGIN
SET @errormsg=@errormsg+'Duplicate Revaluation Journal Entries present in T_Journal Table'
	SELECT * FROM #TempDuplicateJournal
END

SELECT @errormsg AS ErrorMsg

DROP TABLE #TempDuplicateJournal