Declare @errormsg varchar(max)
declare @FundIds varchar(max)
Declare @FromDate datetime
Declare @ToDate datetime

set @errormsg=''
set @FromDate=''
set @ToDate=''
set @FundIds=''

Select Distinct Symbol
Into #OpenSymbols
From PM_Taxlots  
Where TaxLotOpenQty<>0 and  
Taxlot_PK in                                                                                             
(                                                                                                   
   Select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                               
   where DateDiff(d,PM_Taxlots.AUECModifiedDate,getdate()) >=0                                                                                                                                      
   group by taxlotid                                                   
 )

Select 
TickerSymbol as [Symbol],
BloombergSymbol As [Bloomberg Symbol]
Into #ErrorTable  
FROM V_SecMasterData VSM
Inner Join #OpenSymbols on VSM.TickerSymbol=#OpenSymbols.Symbol and (BloombergSymbol IS NULL OR BloombergSymbol='')

IF  exists( select * from #ErrorTable)
begin
set @errormsg='Symbols with Bloomberg Symbol Null'
select * from #ErrorTable
END

Select @errormsg as Errormsg


Drop Table #OpenSymbols,#ErrorTable


