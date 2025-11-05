---Description :  To check duplicate mark price on the same same day


Declare @errormsg varchar(max)
set @errormsg=''

--Check for Duplicate Symbol, remove if duplicate symbol          
;WITH MarkPriceCTE(Date, Symbol,FundID, Ranking)          
AS          
(          
SELECT          
 Date, Symbol,FundID,          
Ranking = DENSE_RANK() OVER(PARTITION BY Date, Symbol,FundID ORDER BY NEWID() ASC)          
FROM PM_DayMarkPrice         
)          
Select
Date,
Symbol,
FundID,
Ranking AS [Duplicacy Number] 
Into #DuplicateMarkPrices 
FROM MarkPriceCTE
WHERE Ranking > 1


IF EXISTS(Select * from #DuplicateMarkPrices)
Begin

set @errormsg='Duplicate Markprices.'
Select Date,
Symbol,
FundID,
[Duplicacy Number],
VM.BloombergSymbol as [Bloomberg Symbol] from #DuplicateMarkPrices DM
Left Outer Join V_SecMasterData VM ON VM.TickerSymbol=DM.Symbol
END



select @errormsg as ErrorMsg

Drop Table #DuplicateMarkPrices