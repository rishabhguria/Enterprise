declare @FromDate datetime
declare @ToDate datetime
Declare @errormsg varchar(max)
Declare @Smdb Varchar(max)
Declare @Date Datetime

set @FromDate=''
set @ToDate=''
set @errormsg=''
set @Smdb=''


Set @Date=@FromDate

Declare @IndexInputQuery Varchar(MAX)

CREATE TABLE #IndiceSymbols (Symbol VARCHAR(MAX))
CREATE TABLE #opensymbols (Symbol VARCHAR(MAX))

--To Insert indexes PRANA-16163

Set @IndexInputQuery = 'Insert into #IndiceSymbols
Select
TickerSymbol
From ['+@Smdb+'].dbo.T_SMSymbolLookupTable
Where AssetID=7'



Exec(@IndexInputQuery)

--Select * from #opensymbols

Create Table #CompanyFunds
( 
CompanyFundID int,
FundName Varchar(50)
)

Insert Into #CompanyFunds
Select
CompanyFundID,
FundName
From T_CompanyFunds

Create Table #OpenSymbolsWithMarkPrice
(
Date Datetime,
Symbol varchar(200),
[Mark Price] float,
BloombergSymbol Varchar(200),
FundName Varchar(200),
[No Entry For Symbol] Varchar(10)
)

WHILE(@Date<=@ToDate)
BEGIN

DELETE #opensymbols

INSERT INTO #opensymbols
Select distinct Symbol 
From PM_Taxlots
Where TaxLotOpenQty<>0 and
Taxlot_PK in
(
  Select max(Taxlot_PK) from PM_Taxlots
  where DateDiff(d,PM_Taxlots.AUECModifiedDate,@Date) >=0
  group by taxlotid
)

INSERT INTO #opensymbols
Select Symbol From #IndiceSymbols

Insert into #OpenSymbolsWithMarkPrice
Select
@Date,
Symbol,
0,
'',
'',
'False' from #opensymbols 

Set @Date=DateAdd(d,1,@Date)

END

UPDATE #OpenSymbolsWithMarkPrice 
Set [Mark Price]= CASE When FinalMarkPrice IS NOT NULL THEN FinalMarkPrice ELSE NULL END,
BloombergSymbol= VM.BloombergSymbol,
FundName = CASE WHEN CF.FundName IS NULL THEN NULL ELSE CF.FundName END,
[No Entry For Symbol]=Case When FinalMarkPrice IS NULL THEN 'Ture' Else 'False' End
FROM #OpenSymbolsWithMarkPrice OMP
Left Outer Join PM_DayMarkPrice DMP On OMP.Symbol = DMP.Symbol and OMP.Date=DMP.Date
Left Outer Join #CompanyFunds CF On DMP.FundID = CF.CompanyFundID
Left Outer Join V_SecMasterDAta VM ON OMP.Symbol = VM.TickerSymbol




IF EXISTS (Select * from #OpenSymbolsWithMarkPrice Where [Mark Price]=0 OR [Mark Price] IS NULL)
BEGIN
set @errormsg ='Mark price is Zero for some symbols'
Select * from #OpenSymbolsWithMarkPrice 
Where [Mark Price]=0 OR [Mark Price] IS NULL
Order By Date Desc,Symbol
END


select @errormsg as ErrorMsg

Drop table #opensymbols,#CompanyFunds,#IndiceSymbols
Drop table #OpenSymbolsWithMarkPrice