

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
[Sector Name] Varchar(200),
[Sub Sector Name] Varchar(200)
)

Insert Into #ErrorTable
Select 
Distinct
TickerSymbol as Symbol,
SectorName as [Sector Name],SubSectorName as [Sub Sector Name]
FROM V_SecMasterData VSM
Inner Join #OpenSymbols on VSM.TickerSymbol=#OpenSymbols.Symbol 
and 
(
(VSM.SectorName IS NULL OR VSM.SectorName='Undefined' OR VSM.SectorName='')
OR
(VSM.SubSectorName IS NULL OR VSM.SubSectorName='Undefined' OR VSM.SubSectorName='')
)

IF  exists( select * from #ErrorTable)
begin
set @errormsg='Symbols with Sector and Subsector Null or Undefined'
select * from #ErrorTable
END

select @errormsg as ErrorMsg

Drop Table #OpenSymbols,#ErrorTable