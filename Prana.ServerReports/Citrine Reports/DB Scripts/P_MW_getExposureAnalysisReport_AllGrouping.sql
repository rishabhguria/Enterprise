          
              
/***************************                
Author : Praveen Bora          
          
Modified Date: April 22, 2014                
          
Description:          
Fund ,Strategy And Symbol Grouping for Getting Net Postion of a Symbol                      
if Exposure Choice is 1 then Long and Short will be calculated on the basis of Position                
and if its is 2 then based on exposure net                
          
Execution Method:                
[P_MW_getExposureAnalysisReport_AllGrouping_BAK20140606] '2014-04-22',2,'Strategy','ExpirationMonthYear','Select','Select','99726','Equity,EquityOption,FutureOption','Aluminium,Copper,Gold,Lead,Nickel,Palladium',1,1             
***************************/                
                
                
CREATE Proc [dbo].P_MW_getExposureAnalysisReport_AllGrouping                
(                
 @Date datetime,                
 @LongShortExposureChoice int,                
 @GroupBy1 varchar(100),                
 @GroupBy2 varchar(100),                
 @GroupBy3 varchar(100),                
 @GroupBy4 varchar(100),              
 @Funds Varchar(max),              
 @Assets Varchar(max),              
 @Sectors Varchar(max),          
 @GroupByUnderlyingExpiration bit,          
 @paramNAVbyMWorPM int          
)     --with recompile           
As                
                
----Declare @Date datetime                
----Declare @LongShortExposureChoice int                
----Declare @GroupBy1 varchar(100)                
----Declare @GroupBy2 varchar(100)                
----Declare @GroupBy3 varchar(100)                
----Declare @GroupBy4 varchar(100)                
----Declare @Funds Varchar(max)              
----Declare @Assets Varchar(max)              
----Declare @Sectors Varchar(max)           
----Declare @GroupByUnderlyingExpiration bit          
----Declare @paramNAVbyMWorPM int             
----                
----Set @Date  = '2014-3-13'                
----Set @LongShortExposureChoice = 2                
----Set @GroupBy1 = 'ExpirationMonthYear'                
----Set @GroupBy2 = 'Sector'                
----Set @GroupBy3 = 'Select'                
----Set @GroupBy4 = 'Select'                
----Set @Funds = '99726'                
----Set @Assets = 'FutureOption'                
----Set @Sectors = 'Aluminium,Copper,Gold,Lead,Nickel,Palladium'          
----Set @GroupByUnderlyingExpiration = 1          
----Set @paramNAVbyMWorPM =1                  
----              
Select * Into #Funds                                                
from dbo.Split(@Funds, ',')                  
                      
Select * Into #Assets                                               
from dbo.Split(@Assets, ',')                
              
Select * Into #Sectors                                               
from dbo.Split(@Sectors, ',')                
          
Create table #NoGroupedPNLData                
(                
Fund varchar(100),                  
Symbol varchar(100),                  
Strategy varchar(100),                  
UDASector varchar(100),           
UDASubSector varchar(100),                 
Asset varchar(100),               
BeginningQuantity float,              
SideMultiplier int,              
PutOrCall varchar(100),                  
BloombergSymbol varchar(100),                  
ExpirationDate datetime,          
UnderlyingSymbol varchar(100),              
Position float,                  
UnderlyingSymbolPrice float,                  
Multiplier float,                  
Delta Varchar(100),                  
DeltaExposureBase float    ,            
Exchange Varchar(100) ,                  
NAV float,  
OpenTradeAttribute1 varchar(100),  
OpenTradeAttribute2 varchar(100),  
OpenTradeAttribute3 varchar(100),  
OpenTradeAttribute4 varchar(100),  
OpenTradeAttribute5 varchar(100),  
OpenTradeAttribute6 varchar(100),  
ClosedTradeAttribute1 varchar(100),  
ClosedTradeAttribute2 varchar(100),  
ClosedTradeAttribute3 varchar(100),  
ClosedTradeAttribute4 varchar(100),  
ClosedTradeAttribute5 varchar(100),  
ClosedTradeAttribute6 varchar(100)  
)           
          
          
Insert Into #NoGroupedPNLData            
              
  SELECT             
    PNL.Fund,                  
    PNL.Symbol,                  
 PNL.Strategy,                  
 PNL.UDASector as UDASector,           
 PNL.UDASubSector as UDASubSector,                  
 PNL.Asset as Asset,             
 PNL.BeginningQuantity,            
 PNL.SideMultiplier,            
 PNL.PutOrCall as PutOrCall ,                  
 PNL.BloombergSymbol as BloombergSymbol,                  
 CASE            
  WHEN (@GroupByUnderlyingExpiration=1 and PNL.Asset = 'FutureOption')            
  THEN SM.ExpirationDate            
  ELSE PNL.ExpirationDate            
    END AS ExpirationDate,           
 PNL.UnderlyingSymbol as UnderlyingSymbol,           
 PNL.BeginningQuantity*PNL.SideMultiplier as Position,                  
 PNL.UnderlyingSymbolPrice as UnderlyingSymbolPrice,               
 PNL.Multiplier as Multiplier,                  
 PNL.Delta as Delta,                
 PNL.DeltaExposureBase as DeltaExposureBase,            
 PNL.Exchange,                  
 isnull(P.NAVValue,0) as NAV,  
 PNL.OpenTradeAttribute1 ,  
 PNL.OpenTradeAttribute2 ,  
 PNL.OpenTradeAttribute3 ,  
 PNL.OpenTradeAttribute4 ,  
 PNL.OpenTradeAttribute5 ,  
 PNL.OpenTradeAttribute6 ,  
 PNL.ClosedTradeAttribute1 ,  
 PNL.ClosedTradeAttribute2 ,  
 PNL.ClosedTradeAttribute3 ,  
 PNL.ClosedTradeAttribute4 ,  
 PNL.ClosedTradeAttribute5 ,  
 PNL.ClosedTradeAttribute6   
 from                   
T_MW_genericPNL PNL                  
  LEFT OUTER JOIN T_CompanyFunds on Fund = FundName                  
  LEFT OUTER JOIN PM_NAVValue P on CompanyFundID=FundID and datediff(d,Date,@Date)=0                
  LEFT OUTER JOIN V_SecMasterData_WithUnderlying SM ON PNL.UnderlyingSymbol = SM.TickerSymbol and PNL.Asset = 'FutureOption'            
  where                  
  datediff(d,Rundate,@Date)=0                    
  and                    
  Open_CloseTag='O'                    
  and                     
  Asset <> 'Cash'                  
  and                 
  Fund in (Select * from #Funds)                 
  and                 
  Asset in (Select * from #Assets)                
  and                
  UDASector in (Select * from #Sectors)               
              
             
Create table #TempPNL                
(                
Fund varchar(100),                  
Symbol varchar(100),                  
Strategy varchar(100),                  
UDASector varchar(100),           
UDASubSector varchar(100),                 
Asset varchar(100),                 
PutOrCall varchar(100),                  
BloombergSymbol varchar(100),                  
ExpirationDate datetime,          
UnderlyingSymbol varchar(100),                         
Position float,                  
UnderlyingSymbolPrice float,                  
Multiplier float,                  
Delta Varchar(100),                  
DeltaAdjustedPosition float,                  
DeltaExposureBase float,                  
LongExposure float,                  
ShortExposure float,                  
GrossExposureBase float,                  
NAV float,  
OpenTradeAttribute1 varchar(100),  
OpenTradeAttribute2 varchar(100),  
OpenTradeAttribute3 varchar(100),  
OpenTradeAttribute4 varchar(100),  
OpenTradeAttribute5 varchar(100),  
OpenTradeAttribute6 varchar(100),  
ClosedTradeAttribute1 varchar(100),  
ClosedTradeAttribute2 varchar(100),  
ClosedTradeAttribute3 varchar(100),  
ClosedTradeAttribute4 varchar(100),  
ClosedTradeAttribute5 varchar(100),  
ClosedTradeAttribute6 varchar(100)                    
                
)                
                     
if(  
@GroupBy3 = 'UnderlyingSymbol' or  @GroupBy3 = 'Strategy' or @GroupBy2 = 'UnderlyingSymbol' or  @GroupBy2 = 'Strategy'       
or @GroupBy1 = 'UnderlyingSymbol' or  @GroupBy1 = 'Strategy' or @GroupBy4 = 'UnderlyingSymbol' or  @GroupBy4 = 'Strategy'  
or @GroupBy1 = 'OpenTradeAttribute1' or @GroupBy1 = 'OpenTradeAttribute2' or @GroupBy1 = 'OpenTradeAttribute3'or @GroupBy1 = 'OpenTradeAttribute4'or @GroupBy1 = 'OpenTradeAttribute5'or @GroupBy1 = 'OpenTradeAttribute6'    
or @GroupBy2 = 'OpenTradeAttribute1' or @GroupBy2 = 'OpenTradeAttribute2' or @GroupBy2 = 'OpenTradeAttribute3'or @GroupBy2 = 'OpenTradeAttribute4'or @GroupBy2 = 'OpenTradeAttribute5'or @GroupBy2 = 'OpenTradeAttribute6'    
or @GroupBy3 = 'OpenTradeAttribute1' or @GroupBy3 = 'OpenTradeAttribute2' or @GroupBy3 = 'OpenTradeAttribute3'or @GroupBy3 = 'OpenTradeAttribute4'or @GroupBy3 = 'OpenTradeAttribute5'or @GroupBy3 = 'OpenTradeAttribute6'    
or @GroupBy4 = 'OpenTradeAttribute1' or @GroupBy4 = 'OpenTradeAttribute2' or @GroupBy4 = 'OpenTradeAttribute3'or @GroupBy4 = 'OpenTradeAttribute4'or @GroupBy4 = 'OpenTradeAttribute5'or @GroupBy4 = 'OpenTradeAttribute6'    
or @GroupBy1 = 'ClosedTradeAttribute1' or @GroupBy1 = 'ClosedTradeAttribute2' or @GroupBy1 = 'ClosedTradeAttribute3'or @GroupBy1 = 'ClosedTradeAttribute4'or @GroupBy1 = 'ClosedTradeAttribute5'or @GroupBy1 = 'ClosedTradeAttribute6'    
or @GroupBy2 = 'ClosedTradeAttribute1' or @GroupBy2 = 'ClosedTradeAttribute2' or @GroupBy2 = 'ClosedTradeAttribute3'or @GroupBy2 = 'ClosedTradeAttribute4'or @GroupBy2 = 'ClosedTradeAttribute5'or @GroupBy2 = 'ClosedTradeAttribute6'    
or @GroupBy3 = 'ClosedTradeAttribute1' or @GroupBy3 = 'ClosedTradeAttribute2' or @GroupBy3 = 'ClosedTradeAttribute3'or @GroupBy3 = 'ClosedTradeAttribute4'or @GroupBy3 = 'ClosedTradeAttribute5'or @GroupBy3 = 'ClosedTradeAttribute6'    
or @GroupBy4 = 'ClosedTradeAttribute1' or @GroupBy4 = 'ClosedTradeAttribute2' or @GroupBy4 = 'ClosedTradeAttribute3'or @GroupBy4 = 'ClosedTradeAttribute4'or @GroupBy4 = 'ClosedTradeAttribute5'or @GroupBy4 = 'ClosedTradeAttribute6'    
  
  
)      
BEGIN      
 Insert Into #TempPNL      
      Select                   
   (Fund),                  
   (Symbol) ,                  
   (Strategy),                  
   (UDASector) as UDASector,           
   (UDASubSector) as UDASubSector,                  
   (Asset) as Asset,             
                    
   (PutOrCall) as PutOrCall ,                  
   (BloombergSymbol) as BloombergSymbol,                  
   (ExpirationDate) as ExpirationDate,          
   (UnderlyingSymbol) as UnderlyingSymbol,                  
   (BeginningQuantity*SideMultiplier) as Position,                  
   (UnderlyingSymbolPrice) as UnderlyingSymbolPrice,               
                
   (Multiplier) as Multiplier,                  
   (Delta) as Delta,                  
                     
   (                  
   Case                  
   when (UDASector = 'Copper'and Exchange = 'COMX')                  
   Then (BeginningQuantity*SideMultiplier*Delta*0.453592)                  
   Else (BeginningQuantity*SideMultiplier*Delta)                   
   End                  
   )  as DeltaAdjustedPosition,                    
   (DeltaExposureBase) as DeltaExposureBase,                  
   Case                   
   When                   
   (                  
   (@LongShortExposureChoice = 1 and  (BeginningQuantity*SideMultiplier) >0 and (Asset) in ('EquityOption','FutureOption')) or                   
   (@LongShortExposureChoice = 2 and (BeginningQuantity*SideMultiplier*Delta)>0 and (Asset) in ('EquityOption','FutureOption'))  or                  
   ((Asset) not in ('EquityOption','FutureOption') and (BeginningQuantity*SideMultiplier) >0)                  
   )           
   Then                   
   (DeltaExposureBase)                  
   Else 0                  
   End as  LongExposure,              
                 
   Case                   
   When                   
   (                  
   (@LongShortExposureChoice = 1 and  (BeginningQuantity*SideMultiplier)<0 and (Asset) in ('EquityOption','FutureOption')) or                   
   (@LongShortExposureChoice = 2 and (BeginningQuantity*SideMultiplier*Delta)<0 and (Asset) in ('EquityOption','FutureOption'))  or                  
   ((Asset) not in ('EquityOption','FutureOption') and (BeginningQuantity*SideMultiplier) <0)                  
   )                  
   Then                   
   (DeltaExposureBase)                  
   Else 0                  
   End as  ShortExposure,              
                 
    abs((DeltaExposureBase)) as GrossExposureBase,                
    (NAV) as NAV,  
    OpenTradeAttribute1 ,  
    OpenTradeAttribute2 ,  
  OpenTradeAttribute3 ,  
  OpenTradeAttribute4 ,  
  OpenTradeAttribute5 ,  
  OpenTradeAttribute6 ,  
  ClosedTradeAttribute1 ,  
  ClosedTradeAttribute2 ,  
  ClosedTradeAttribute3 ,  
  ClosedTradeAttribute4 ,  
  ClosedTradeAttribute5 ,  
  ClosedTradeAttribute6                
   from                   
   #NoGroupedPNLData      
      
 order by Symbol       
END      
ELSE      
BEGIN      
 Insert Into #TempPNL                
   Select                   
   MAX(Fund),                  
   Max(Symbol) ,                  
   MAX(Strategy),                  
   Max(UDASector) as UDASector,           
   Max(UDASubSector) as UDASubSector,                  
   Max(Asset) as Asset,             
                    
   Max(PutOrCall) as PutOrCall ,                  
   Max(BloombergSymbol) as BloombergSymbol,                  
   Max(ExpirationDate) as ExpirationDate,          
   MAX(UnderlyingSymbol) as UnderlyingSymbol,                  
   Sum(BeginningQuantity*SideMultiplier) as Position,                  
   Max(UnderlyingSymbolPrice) as UnderlyingSymbolPrice,               
                
   Max(Multiplier) as Multiplier,                  
   Max(Delta) as Delta,                  
   Sum                  
   (                  
   Case                  
   when (UDASector = 'Copper'and Exchange = 'COMX')                  
   Then (BeginningQuantity*SideMultiplier*Delta*0.453592)                  
   Else (BeginningQuantity*SideMultiplier*Delta)                   
   End                  
   )  as DeltaAdjustedPosition,                    
   Sum(DeltaExposureBase) as DeltaExposureBase,                  
   Case                   
   When                   
   (                  
   (@LongShortExposureChoice = 1 and  Sum(BeginningQuantity*SideMultiplier) >0 and Max(Asset) in ('EquityOption','FutureOption')) or                   
   (@LongShortExposureChoice = 2 and Sum(BeginningQuantity*SideMultiplier*Delta)>0 and Max(Asset) in ('EquityOption','FutureOption'))  or                  
   (Max(Asset) not in ('EquityOption','FutureOption') and Sum(BeginningQuantity*SideMultiplier) >0)                  
   )                  
   Then                   
   SUm(DeltaExposureBase)                  
   Else 0                  
   End as  LongExposure,              
                 
   Case                   
   When                   
   (                  
   (@LongShortExposureChoice = 1 and  Sum(BeginningQuantity*SideMultiplier)<0 and Max(Asset) in ('EquityOption','FutureOption')) or                   
   (@LongShortExposureChoice = 2 and Sum(BeginningQuantity*SideMultiplier*Delta)<0 and Max(Asset) in ('EquityOption','FutureOption'))  or                  
   (Max(Asset) not in ('EquityOption','FutureOption') and Sum(BeginningQuantity*SideMultiplier) <0)                  
   )                  
   Then                   
   SUm(DeltaExposureBase)                  
   Else 0                  
   End as  ShortExposure,              
                 
   abs(Sum(DeltaExposureBase)) as GrossExposureBase,                
   MAX(NAV) as NAV,  
     MAX(OpenTradeAttribute1) ,  
  MAX(OpenTradeAttribute2) ,  
  MAX(OpenTradeAttribute3) ,  
  MAX(OpenTradeAttribute4) ,  
  MAX(OpenTradeAttribute5) ,  
  MAX(OpenTradeAttribute6) ,  
  MAX(ClosedTradeAttribute1) ,  
  MAX(ClosedTradeAttribute2) ,  
  MAX(ClosedTradeAttribute3) ,  
  MAX(ClosedTradeAttribute4) ,  
  MAX(ClosedTradeAttribute5) ,  
  MAX(ClosedTradeAttribute6)                  
   from                   
   #NoGroupedPNLData             
                
   Group by  --Symbol--,datename(Month,ExpirationDate)        
           
 /***************************/            
         
   CASE          
    WHEN                 
 (           
    (@GroupBy1 = 'Fund')             
    )            
    Then Fund            
    WHEN                 
    (                
    (@GroupBy1 = 'Sector')             
    )            
    Then UDASector          
    WHEN                 
    (                
    (@GroupBy1 = 'ExpirationMonthYear')             
    )            
    Then datename(Year,ExpirationDate)          
                 
    WHEN                 
    (                
    (@GroupBy1 = 'Subsector')             
    )            
    Then UDASubSector          
--    WHEN                 
--    (                
--    (@GroupBy1 = 'Strategy')             
--    )            
--    Then Strategy          
    WHEN                 
    (                
    (@GroupBy1 = 'Symbol')             
    )            
    Then Symbol          
    WHEN                 
    (                
    (@GroupBy1 = 'ExpirationDate')             
    )            
    Then Convert(varchar, ExpirationDate ,111)          
--    WHEN           
--    (                
--    (@GroupBy1 = 'UnderlyingSymbol')             
--    )            
--    Then UnderlyingSymbol           
    WHEN                 
    (                
    (@GroupBy1 = 'Bloomberg')             
    )            
    Then BloombergSymbol        
    WHEN                 
    (                
    (@GroupBy1 = 'Asset')             
    )            
    Then Asset        
    Else Symbol         
    End            
    ,        
    CASE          
    WHEN                 
    (                
    (@GroupBy1 = 'ExpirationMonthYear')             
    )            
    Then datename(Month,ExpirationDate)          
    Else ''        
    End       
  ,         
    CASE          
    WHEN                 
    (                
    (@GroupBy2 = 'Sector')             
    )            
    Then UDASector          
    WHEN                 
    (                
    (@GroupBy2 = 'ExpirationMonthYear')             
    )            
    Then datename(Year,ExpirationDate)          
    WHEN                 
    (                
    (@GroupBy2 = 'Subsector')             
    )            
    Then UDASubSector          
--    WHEN                 
--    (                
--    (@GroupBy2 = 'Strategy')             
--    )            
--    Then Strategy          
    WHEN                 
    (                
    (@GroupBy2 = 'ExpirationDate')             
    )            
    Then Convert(varchar, ExpirationDate ,111)         
--    WHEN                 
--    (                
--    (@GroupBy2 = 'UnderlyingSymbol')             
--    )            
--    Then UnderlyingSymbol        
    WHEN                 
    (                
    (@GroupBy2 = 'Bloomberg')             
    )            
    Then BloombergSymbol        
    WHEN                 
    (                
    (@GroupBy2 = 'Asset')             
    )            
    Then Asset         
    WHEN                 
    (                
    (@GroupBy2 = 'Symbol')             
    )            
    Then Symbol        
    Else ''        
    End            
    ,      
  Case      
  WHEN                 
    (                
    (@GroupBy2 = 'ExpirationMonthYear')             
    )            
    Then datename(Month,ExpirationDate)          
   Else      
   ''      
  END       
    ,        
    CASE          
    WHEN                 
    (                
    (@GroupBy3 = 'ExpirationMonthYear')             
    )            
    Then datename(Year,ExpirationDate)          
    WHEN                 
    (                
    (@GroupBy3 = 'Subsector')             
    )            
    Then UDASubSector          
--    WHEN                 
--    (                
--    (@GroupBy3 = 'Strategy')             
--    )            
--    Then Strategy        
    WHEN                 
    (                
    (@GroupBy3 = 'ExpirationDate')             
    )            
    Then Convert(varchar, ExpirationDate ,111)          
--    WHEN                 
--    (                
--    (@GroupBy3 = 'UnderlyingSymbol')             
--    )            
--    Then UnderlyingSymbol        
    WHEN                 
    (                
    (@GroupBy3 = 'Bloomberg')             
    )            
    Then BloombergSymbol        
    WHEN                 
    (                
    (@GroupBy3 = 'Symbol')             
    )            
    Then Symbol         
    Else ''        
    End            
    ,       
  Case      
  WHEN                 
    (                
    (@GroupBy3 = 'ExpirationMonthYear')             
    )            
    Then datename(Month,ExpirationDate)          
   Else      
   ''      
  END          
    ,          
    CASE          
    WHEN                 
    (                
    (@GroupBy4 = 'Subsector')             
    )            
    Then UDASubSector          
--    WHEN                 
--    (                
--    (@GroupBy4 = 'strategy')             
--    )            
--    Then Strategy         
    WHEN                 
    (                
    (@GroupBy4 = 'ExpirationDate')             
    )            
    Then Convert(varchar, ExpirationDate ,111)          
--    WHEN                 
--    (                
--    (@GroupBy4 = 'UnderlyingSymbol')             
--    )            
--    Then UnderlyingSymbol        
    WHEN                 
    (                
    (@GroupBy4 = 'Bloomberg')             
    )            
    Then BloombergSymbol        
    WHEN                 
    (                
    (@GroupBy4 = 'Symbol')             
    )            
    Then Symbol         
    Else ''        
    End             
           
 /***************************/           
           
   having Sum(BeginningQuantity*SideMultiplier*Delta) <>0                
   order by MAX(Symbol)          
END      
           
           
          
            
          
          
                   
                
Declare @NAV float          
          
If (@paramNAVbyMWorPM=1)          
BEGIN          
 Select @NAV=ISNULL(SUM(EndingMarketValueBase),0)          
 From T_MW_GenericPNL          
 Where           
 DATEDIFF(d,Rundate,@date)=0          
 and           
 Fund in(select * from #Funds)          
END          
Else          
Begin           
 Select @NAV=ISNULL(NAVValue,0)          
 from PM_NAVValue NAV            
 inner JOIN T_CompanyFunds CF on CF.CompanyFundID=NAV.FundID          
 where           
 datediff(d,@Date,Date)=0                
 and               
 CF.FundName in (select * from #Funds)           
End                     
                
Alter table #TempPNL add  DeltaExposure_perc float                  
Alter table #TempPNL add  LongExposure_perc float                  
Alter table #TempPNL add  ShortExposure_perc float                  
Alter table #TempPNL add  GrossExposure_perc float                 
Alter table #TempPNL add  ExpirationMonth varchar(20)               
Alter table #TempPNL add  ExpirationMonthID varchar(20)                  
Alter table #TempPNL add  ExpirationYear varchar(10)                
                
Update #TempPNL            
set            
DeltaExposure_perc=             
case               
when @NAV=0              
then 0              
else isnull(((DeltaExposureBase/@NAV)*100),0)              
End  ,            
            
LongExposure_perc =              
case               
when @NAV=0              
then 0              
else isnull(((LongExposure/@NAV)*100),0)              
End ,            
            
ShortExposure_perc =              
case               
when @NAV=0              
then 0              
else isnull(((ShortExposure/@NAV)*100),0)              
End,             
            
GrossExposure_perc=              
case               
when @NAV=0              
then 0              
else isnull(((GrossExposureBase/@NAV)*100),0)              
End ,              
                
ExpirationMonth= Datename(month,(ExpirationDate)) ,                
ExpirationMonthID =Cast (month(ExpirationDate) as Varchar(20)) ,                
ExpirationYear= Year(ExpirationDate)                
                
                
Update #TempPNL                
Set ExpirationMonthID =                 
Case                 
When ExpirationMonthID not in (10,11,12)                
Then '0'+ExpirationMonthID                
Else                 
ExpirationMonthID                
End                 
                
                
Select *,               
@NAV as TotalNAV,           
(ExpirationMonth +' '+ ExpirationYear) as ExpirationMonthYear ,                
(ExpirationYear + Cast (ExpirationMonthID as Varchar(10))) as ExpirationYearMonthID                 
 from #TempPNL                
                
------Select * from #TempPNL                
                
drop table  #TempPNL,#Funds,#Assets,#Sectors,#NoGroupedPNLData 