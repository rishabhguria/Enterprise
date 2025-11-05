Declare @FromDate datetime
Declare @ToDate datetime
Declare @errormsg varchar(max)

set @FromDate=''
set @ToDate=''
set @errormsg=''


SELECT T_Journal.* into #ErrorTable FROM T_Journal 
INNER JOIN T_CashPreferences ON T_Journal.FundID = T_CashPreferences.FundID
WHERE (CurrencyID IS NULL OR CurrencyID = '') 
AND DATEDIFF(D, @FromDate, TransactionDate)>=0 
AND DATEDIFF(D, TransactionDate, @ToDate)>=0
AND DATEDIFF(D, T_CashPreferences.CashMgmtStartDate, TransactionDate)>=0

SELECT T_Allactivity.* into #ErrorTable1 FROM T_Allactivity
INNER JOIN T_CashPreferences ON T_Allactivity.FundID = T_CashPreferences.FundID
WHERE (CurrencyID IS NULL OR CurrencyID = '') 
AND DATEDIFF(D, @FromDate, TradeDate)>=0 
AND DATEDIFF(D, TradeDate, @ToDate)>=0
AND DATEDIFF(D, T_CashPreferences.CashMgmtStartDate, TradeDate)>=0


If Exists(Select * from #ErrorTable)
Begin
Set @errormsg=' There are Blank Currency issues ||'
Select * from #ErrorTable
End

If Exists(Select * from #ErrorTable1)
Begin
Set @errormsg=@errormsg+' There are Blank Currency issues'
Select * from #ErrorTable1
End

Select @errormsg as ErrorMsg

Drop Table #ErrorTable,#ErrorTable1