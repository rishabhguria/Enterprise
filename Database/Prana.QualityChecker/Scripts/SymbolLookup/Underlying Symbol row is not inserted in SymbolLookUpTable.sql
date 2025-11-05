
Declare @errormsg varchar(max)

set @errormsg=''

Select Distinct 
UnderLyingSymbol,
TickerSymbol 
into #temp from V_secmasterdata

Select 
Tickersymbol,
UnderLyingSymbol 
into #temp_tickerSymbol from #temp 
WHERE UnderLyingSymbol 
not in (select tickersymbol from V_secmasterdata)


Select TickerSymbol,UnderLyingSymbol into #NullUnderLyingSymbol from #temp where UnderLyingSymbol is null

IF EXISTS (Select * from #NullUnderLyingSymbol)
BEGIN
set @errormsg ='Symbol with Null UnderLying Symbol detected |'
Select * from #NullUnderLyingSymbol
END

IF EXISTS (Select * from #temp_tickerSymbol)
BEGIN
set @errormsg ='Below Symbols are stored as UnderLying Symbol but not as Ticker Symbol'
Select * from #temp_tickerSymbol
END

select @errormsg as ErrorMsg





Drop table #temp,#temp_tickerSymbol,#NullUnderLyingSymbol