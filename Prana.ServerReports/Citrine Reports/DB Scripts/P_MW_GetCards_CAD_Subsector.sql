 /**************************************            
            
    
[P_MW_GetCards_CAD_Testing] '2014-06-05','47887,47888,61542,80884,0VQ-954118,99726,OLC-926715'        
             
***************************************/            
            
            
CREATE Procedure [dbo].[P_MW_GetCards_CAD_Subsector]                                                                                           
(                                                                                                                       
 @Date datetime,                       
 @masterfund varchar(max)                                                                                                        
)                                                               
As      
    
        
            
            
SELECT              
MasterFund as MasterFundName,            
UDASubSector as SubSector,             
PNL.UnderlyingSymbol as Underlying,              
CAST(FLOOR(CAST(SM.ExpirationDate AS FLOAT ) ) AS DATETIME ) as ExpDate ,                  
Asset as AssetClass,             
Exchange as Exchange,            
CASE                
WHEN Asset = 'Future' AND Exchange = 'COMX'            
THEN SUM(BeginningQuantity*SideMultiplier*.453592)            
WHEN Asset = 'Future' AND Exchange <> 'COMX'            
THEN SUM(BeginningQuantity*SideMultiplier)            
ELSE 0              
END as FuturePosition,            
            
CASE                
WHEN Asset = 'FutureOption' AND Exchange = 'COMX'            
THEN SUM(BeginningQuantity*PNL.Delta*SideMultiplier*.453592)            
WHEN Asset = 'FutureOption' AND Exchange <> 'COMX'            
THEN SUM(BeginningQuantity*PNL.Delta*SideMultiplier)            
ELSE 0             
END as DeltaPosition,            
            
REPLACE(SM.BloombergSymbol,'comdty','') as BBGSymbol            
 into #Temp            
FROM T_MW_GenericPNL PNL            
left outer join V_SecMasterData SM on  PNL.UnderlyingSymbol = SM.TickerSymbol              
WHERE             
Datediff(d,RunDate,@Date)=0            
AND             
Open_CloseTag = 'O'                
AND             
Asset <> 'Cash'                
AND             
UDASubSector = 'Copper'            
AND            
MasterFund in (SELECT * FROM dbo.split(@masterfund, ','))                
GROUP BY MasterFund, PNL.UnderlyingSymbol,UDASubSector,Asset,Exchange,SM.ExpirationDate,SM.BloombergSymbol              
having             
Sum(BeginningQuantity*PNL.Delta*SideMultiplier) <>0            
--order by MasterFund,UDASector,BBGSymbol,ExpDate         
    
    
insert into #Temp    
select     
'Total' as  MasterFundName    
,SubSector     
,Underlying    
,ExpDate    
,AssetClass    
,Exchange    
,FuturePosition    
,DeltaPosition    
,BbgSymbol    
from #Temp    
       
Select             
MasterFundName,             
Underlying,           
SubSector,           
ExpDate,            
Max(Exchange) as Exchange,            
Sum(FuturePosition) as FuturePosition,            
Sum(DeltaPosition) as DeltaPosition,            
BbgSymbol             
From #Temp            
GROUP BY MasterFundName, Underlying,SubSector,ExpDate,BbgSymbol--,Exchange            
order by MasterFundName,SubSector,ExpDate,BBGSymbol              
            
            
Drop table #Temp