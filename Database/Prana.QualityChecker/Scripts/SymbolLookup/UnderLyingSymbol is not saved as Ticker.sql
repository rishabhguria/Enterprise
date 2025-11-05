--Work on SMDB
Declare @errormsg varchar(max)
declare @FundIds varchar(max)
Declare @FromDate datetime
Declare @ToDate datetime


set @errormsg=''
set @FromDate=''
set @ToDate=''
set @FundIds=''


select UnderLyingSymbol into #temptickettable from T_SMSymbolLookUpTable 
where UnderLyingSymbol<>'' AND UnderLyingSymbol<>'Undefined' AND UnderLyingSymbol IS NOT NULL 

Create Table #tempTicker
(
[UnderLying Symbol] Varchar(200)
)
Insert Into #tempTicker
Select
UnderLyingSymbol 
FROM #temptickettable  
WHERE UnderLyingSymbol NOT IN(select TickerSymbol from T_SMSymbolLookUpTable)

IF  exists( select * from #tempTicker)
begin
set @errormsg='UnderLyingSymbol is not saved as Ticker'
select DISTINCT [UnderLying Symbol] from #tempTicker
END

select @errormsg as ErrorMsg


Drop Table #temptickettable,#tempTicker