

Declare @errormsg varchar(max)
declare @FundIds varchar(max)
Declare @FromDate datetime
Declare @ToDate datetime

set @errormsg=''
set @FromDate=''
set @ToDate=''
set @FundIds=''

Select 
Distinct Symbol,
FundID
Into #OpenSymbols
From PM_Taxlots  
Where TaxLotOpenQty<>0 and  
Taxlot_PK in                                                                                             
(                                                                                                   
   Select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                               
   where DateDiff(d,PM_Taxlots.AUECModifiedDate,getdate()) >=0                                                                                                                                      
   group by taxlotid                                                   
 )

Create Table #ErrorTable
(
Symbol Varchar(200),
[UnderLying Symbol] Varchar(200)
)

Insert Into #ErrorTable
Select
Distinct 
TickerSymbol,
UnderLyingSymbol 
FROM V_SecMasterData VSM
Inner Join #OpenSymbols on VSM.TickerSymbol=#OpenSymbols.Symbol 
and
(
UnderLyingSymbol='' OR UnderLyingSymbol='Undefined' Or UnderLyingSymbol IS NULL
)

IF  exists( select * from #ErrorTable)
begin
set @errormsg='Symbols with Blank UnderLyingSymbol'
select * from #ErrorTable
END

select @errormsg as ErrorMsg


Drop Table #OpenSymbols,#ErrorTable