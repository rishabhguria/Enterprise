CREATE Proc [dbo].P_MW_getExposureAnalysisReport_MonthNetting_DateRange                                   
(                                    
 @StartDate datetime,              
 @EndDate datetime,                                    
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
                                    
----Declare @StartDate datetime                                    
----Declare @ENDDate datetime                                    
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
----Set @StartDate  = '2014-7-9'        
----Set @EndDAte = '2014-7-9'                                    
----Set @LongShortExposureChoice = 2                                    
----Set @GroupBy1 = 'Fund'                                    
----Set @GroupBy2 = 'Strategy'                                    
----Set @GroupBy3 = 'OpentradeAttribute1'                                    
----Set @GroupBy4 = 'BloombergSymbol'                                    
----Set @Funds = '99726'                                    
----Set @Assets = 'Equity,EquityOption,FutureOption'                                    
----Set @Sectors = 'Aluminium,Copper,Gold,Lead,Nickel,Palladium'                              
----Set @GroupByUnderlyingExpiration = 1                              
----Set @paramNAVbyMWorPM =1                                      
----                                  
CREATE TABLE #TempDate        
(        
 Date datetime         
)        
        
While (@StartDate <= @EndDate)        
BEGIN         
IF(datepart(dw,@StartDate) NOT IN (1,7))        
BEGIN        
INSERT INTO #TempDate Values(@StartDate)        
END        
SET @StartDAte=DateAdd(day, 1, @Startdate)        
END                  
                  
declare  @FundWiseNav table                  
(                  
 FundName varchar(max),                  
 FundId varchar(max),                  
 NavValue float,              
 Date datetime                  
)                  
                  
If (@paramNAVbyMWorPM=1)                              
BEGIN                   
                  
 insert into @FundWiseNav                  
 select                   
  PNL.fund,                  
  CompanyFundId,                  
  ISNULL(SUM(EndingMarketValueBase),0) ,              
  PNL.rundate               
 from T_MW_GenericPNL PNL                  
 inner join T_companyfunds                   
 on T_companyfunds.fundName = PNL.Fund                  
 where rundate in (Select * from #TempDate)                
 group by rundate,PNL.fund,CompanyFundId                  
END                  
ELSE                   
BEGIN                  
 insert into @FundWiseNav                  
 select                   
  fundName,                  
  FundId,                  
  NavValue,              
  Date                   
 from PM_NAVValue                   
 inner join T_companyfunds                   
 on T_companyfunds.CompanyfundId = PM_NAVValue.FundId                  
 where Date in (Select * from #TempDate)               
              
END           
      
--  select * from @FundWiseNav          
                  
Select * Into #Funds                                                                    
from dbo.Split(@Funds, ',')                                      
                                          
Select * Into #Assets                               
from dbo.Split(@Assets, ',')                                    
                                  
Select * Into #Sectors                                                                   
from dbo.Split(@Sectors, ',')                                    
                              
Create table #NoGroupedPNLData                                    
(              
RunDate datetime,                                  
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
FundWiseNav float,    
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
    PNL.RunDate ,                              
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
 isnull(P.NAVValue,0) as FundWiseNav,  
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
  INNER join  @FundWiseNav P on CompanyFundID=FundID and P.Date = PNL.rundate                   
  LEFT OUTER JOIN V_SecMasterData_WithUnderlying SM ON PNL.UnderlyingSymbol = SM.TickerSymbol and PNL.Asset = 'FutureOption'                                
  Inner join #TempDate on   datediff(d,#TempDate.Date,PNL.Rundate)=0                                      
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
RunDate datetime,                                  
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
FundWiseNav float,  
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
ClosedTradeAttribute6 varchar(100),  
FundCount int  
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
 Rundate,          
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
 isnull(NAV,0) as FundWiseNav,    
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
  ClosedTradeAttribute6 ,  
  -1 as FundCount                         
   from                             
   #NoGroupedPNLData                
 order by Symbol                           
END                          
ELSE                          
BEGIN                          
 Insert Into #TempPNL                                    
   Select             
   Rundate,                                       
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
   SUM(DeltaExposureBase)                                      
   Else 0                                      
   End as  ShortExposure,                                  
                                     
   abs(Sum(DeltaExposureBase)) as GrossExposureBase,                                   
   MAX(NAV) as NAV,                      
   Sum(Distinct NAV) as FundWiseNav,  
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
   MAX(ClosedTradeAttribute6) ,  
   Count(distinct Fund) as FundCount                                     
   from                                       
   #NoGroupedPNLData                                 
                                    
   Group by Rundate,   --Symbol--,datename(Month,ExpirationDate)                            
                               
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
                               
                               
                              
                                
                       
                              
                                       
                                    
             
Create table #FundDateWiseNav                
(                
 Date datetime,                  
 NavValue float            
 )                            
If (@paramNAVbyMWorPM=1)                              
BEGIN                  
insert into #FundDateWiseNav                              
 Select             
 RunDate ,            
 ISNULL(SUM(EndingMarketValueBase),0)                             
 From T_MW_GenericPNL                              
 Where                               
rundate in (Select * from #TempDate)                            
 and                               
 Fund in(select * from #Funds)                 
 Group By rundate                         
END                              
Else                              
Begin              
 insert into #FundDateWiseNav             
Select            
 Date,                             
 ISNULL(NAVValue,0)                              
 from PM_NAVValue NAV                                
 inner JOIN T_CompanyFunds CF on CF.CompanyFundID=NAV.FundID                              
 where                               
 DAte in (Select * from #TempDate)                                   
 and                                   
 CF.FundName in (select * from #Funds)             
Group By Date                               
End                                         
                                    
Alter table #TempPNL add  DeltaExposure_perc float                                      
Alter table #TempPNL add  LongExposure_perc float                                      
Alter table #TempPNL add  ShortExposure_perc float                                      
Alter table #TempPNL add  GrossExposure_perc float                                     
Alter table #TempPNL add  ExpirationMonth varchar(20)                                   
Alter table #TempPNL add  ExpirationMonthID varchar(20)                                      
Alter table #TempPNL add  ExpirationYear varchar(10)                                    
Alter table #TempPNL add  TotalNAV float      
                  
--select @NAV                  
                                 
Update #TempPNL                                
set                                
DeltaExposure_perc=                
case                                   
 when #FundDateWiseNav.NavValue =0                                  
 then 0                                  
 else isnull(((DeltaExposureBase)*100/#FundDateWiseNav.NavValue),0)                                  
End             
,            
LongExposure_perc =                                  
case                                   
 when #FundDateWiseNav.NavValue=0                                  
then 0                                  
 else isnull(((LongExposure)*100/#FundDateWiseNav.NavValue),0)                                  
End ,             
ShortExposure_perc =                                  
case                                   
 when #FundDateWiseNav.NavValue=0                                  
 then 0                                  
 else isnull(((ShortExposure)*100/#FundDateWiseNav.NavValue),0)                                  
End,             
GrossExposure_perc=                                  
case                                   
 when #FundDateWiseNav.NavValue=0                                  
 then 0                                  
 else isnull(((GrossExposureBase)*100/#FundDateWiseNav.NavValue),0)                                  
End ,                                 
ExpirationMonth= Datename(month,(ExpirationDate)) ,                                    
ExpirationMonthID = Cast (month(ExpirationDate) as Varchar(20)),                                    
ExpirationYear= Year(ExpirationDate),      
TotalNAV = #FundDateWiseNav.NavValue       
            
from #TempPNL            
inner join  #FundDateWiseNav on datediff(d,#FundDateWiseNav.Date,#TempPNL.Rundate)=0                                                                                                                                                                
                                   
Update #TempPNL                                    
Set ExpirationMonthID =                                     
Case                                     
When ExpirationMonthID not in (10,11,12)                                    
Then '0'+ExpirationMonthID                                    
Else                                     
ExpirationMonthID                                    
End                                     
                                    
                                    
Select *,            
(ExpirationMonth +' '+ ExpirationYear) as ExpirationMonthYear ,                                    
(ExpirationYear + Cast (ExpirationMonthID as Varchar(10))) as ExpirationYearMonthID                                     
from #TempPNL              
      
                                  
                                    
drop table  #TempPNL,#Funds,#Assets,#Sectors,#NoGroupedPNLData,#FundDateWiseNav,#TempDate