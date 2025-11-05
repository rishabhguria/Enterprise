Declare @errormsg varchar(max)
declare @FundIds varchar(max)
Declare @FromDate datetime
Declare @ToDate datetime

set @errormsg=''
set @FromDate=''
set @ToDate=''
set @FundIds=''


Select 
Distinct Symbol
Into #OpenSymbols
From PM_Taxlots  
Where TaxLotOpenQty<>0 and  
Taxlot_PK in                                                                                             
(                                                                                                   
   Select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                               
   where DateDiff(d,PM_Taxlots.AUECModifiedDate,@ToDate) >=0                                                                                                                                      
   group by taxlotid                                                   
 )

Select TickerSymbol,
ExpirationDate
into #OpenOptionSymbols
From V_SecMasterData V INNER JOIN #OpenSymbols S ON V.TickerSymbol = S.Symbol
Where datediff(d,@ToDate,Expirationdate)<=0 and V.AssetId IN (2,4,10)

IF exists( select * from #OpenOptionSymbols)
begin
set @errormsg='Open symbols with Expiration date'
select * from #OpenOptionSymbols
END

select @errormsg as ErrorMsg

drop table #OpenSymbols,#OpenOptionSymbols
