Description: Print 'Check Journals Discrepancy Till Current Date.'


declare  @errormsg varchar(max)
declare @FromDate DateTime
declare @ToDate DateTime
DECLARE @FundIds varchar(MAX)
Declare @Smdb varchar(max)
set @FromDate='' 
set @ToDate=''
set @FundIds=''
set @Smdb=''
set @errormsg=''

--Here we will work on only temp table and all update, delete opreations will perform on temp table.
--Do not use main tables for Update and Delete operations.
Select * INTO #TJournal FROM T_Journal 

UPDATE #TJournal 
SET
	FXRate = CASE 
			WHEN FXRate = 0 THEN FXRate
			ELSE 1 / FXRate 
		END
WHERE FXConversionMethodOperator='D'

select 
TransactionID,
sum(dr*fxrate) AS DRBASE,
sum(cr*fxrate) AS CRBASE ,
(sum(cr*fxrate) - sum(dr*fxrate)) AS [CR DR Difference] ,
min(transactiondate)as Date,
max(transactiondate) as Date2,
max(transactionsource) as [Transaction Source],

Max(CASE
When TransactionSource = 1
Then 'Trading'
When TransactionSource = 2
Then 'ManualJournalEntry'
When TransactionSource = 3
Then 'DailyCalculation'
When TransactionSource = 4
Then 'CorpAction'
When TransactionSource = 5
Then 'CashTransaction'
When TransactionSource = 6
Then 'ImportedEditableData'
When TransactionSource = 7
Then 'Closing'
When TransactionSource = 8
Then 'OpeningBalance'
When TransactionSource = 9
Then 'Revaluation'
When TransactionSource = 10
Then 'Unwinding'
When TransactionSource = 11
Then 'SettlementTransaction' 
END) AS [Transaction Source Name]

,max(TC.CurrencySymbol) as [Currency],
 max(pbdesc) as Description,
'Journal DR not match with CR . Incorrect Journal.' as [Message]
into #CorruptJournal1
 from #TJournal 
inner JOIN T_Currency TC on TC.CurrencyID = #TJournal.CurrencyID
group by transactionid having(ROUND(sum(dr*fxrate)-sum(cr*fxrate),2,1)<>0 OR sum(dr*fxrate)-sum(cr*fxrate) IS NULL)

if exists (select * From #CorruptJournal1)
begin
set @errormsg=@errormsg+'1) Incorrect Journal. DR not equal to CR for some journals.'
select * From #CorruptJournal1
End

select TransactionDate,TaxlotID,TransactionID,Symbol,PBDesc as [PB Description],'Fx rate shouldnt be 1' as [Message]  into #CorruptJournal2 from #TJournal where currencyid<>1 and fxrate=1 
AND TRANSACTIONSOURCE IN(1,2)

if exists (select * From #CorruptJournal2)
begin
set @errormsg=@errormsg+' 2) Fx rate 1 for non-USD.'
select * From #CorruptJournal2
End

SELECT *  into #CorruptJournal3 FROM #TJournal 
WHERE TransactionSource=1 AND DATEDIFF(DAY,@FromDate,TransactionDate)>=0 AND DATEDIFF(DAY,@ToDate,TransactionDate)<=0
AND ActivityId_FK NOT IN 
(SELECT ActivityID FROM T_AllActivity WHERE TransactionSource=1 AND DATEDIFF(DAY,@FromDate,TradeDate)>=0 AND DATEDIFF(DAY,@ToDate,TradeDate)<=0)
 
if exists (select * From #CorruptJournal3)
begin
set @errormsg=@errormsg+' 3) Corrupt Journal Detected.'
select * From #CorruptJournal3
End

select @errormsg as ErrorMsg

Drop Table #CorruptJournal1,#CorruptJournal2,#CorruptJournal3, #TJournal