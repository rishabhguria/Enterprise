/**************************************    
    
[P_MW_GetCards_merge] '2013-10-2','47887,47888,61542,80884,0VQ-954118'    
***************************************/    
    
    
CREATE Procedure [dbo].[P_MW_GetCards_merge_SubSector]                                                                                   
(                                                                                                               
 @Date datetime,               
 @masterfund varchar(max)                                                                                                
)                                                       
As     
    
SELECT      
Max(MasterFund) as MasterFundName,     
UDASubSector as SubSector,   
PNL.UnderlyingSymbol as Underlying,      
CAST(FLOOR(CAST(SM.ExpirationDate AS FLOAT ) ) AS DATETIME ) as ExpDate ,          
    
MAX(Exchange) as Exchange,    
        
CASE    
WHEN Asset = 'Future'     
THEN SUM(BeginningQuantity*SideMultiplier)    
ELSE 0       
END as FuturePosition,    
    
CASE        
WHEN Asset = 'FutureOption'    
THEN Sum(BeginningQuantity*PNL.Delta*SideMultiplier)    
ELSE 0        
END as DeltaPosition,    
    
CASE    
WHEN  UDASubSector = 'Copper' AND Asset = 'Future' AND MAX(Exchange) = 'COMX'    
THEN SUM(BeginningQuantity*SideMultiplier*.453592)    
WHEN  UDASubSector = 'Copper' AND Asset = 'Future' AND MAX(Exchange) <> 'COMX'    
THEN SUM(BeginningQuantity*SideMultiplier)    
ELSE 0    
END as FuturePosition_Adjusted,    
    
CASE    
WHEN  UDASubSector = 'Copper' AND Asset = 'FutureOption' AND MAX(Exchange) = 'COMX'    
THEN SUM(BeginningQuantity*PNL.Delta*SideMultiplier*.453592)    
WHEN  UDASubSector = 'Copper' AND Asset = 'FutureOption' AND MAX(Exchange) <> 'COMX'    
THEN SUM(BeginningQuantity*PNL.Delta*SideMultiplier)    
ELSE 0    
END as DeltaPosition_Adjusted,    
    
REPLACE(SM.BloombergSymbol,'comdty','') as BBGSymbol    
    
into #temp1     
FROM T_MW_GenericPNL  PNL    
left outer join V_SecMasterData SM on PNL.UnderlyingSymbol = SM.TickerSymbol        
WHERE     
Datediff(d,RunDate,@Date)=0    
AND     
Open_CloseTag = 'O'        
AND     
Asset <> 'Cash'        
AND     
MasterFund in (SELECT * FROM dbo.split(@masterfund, ','))        
--GROUP BY MasterFund, PNL.UnderlyingSymbol,UDASector, Asset,Exchange,SM.ExpirationDate,BloombergSymbol       
GROUP BY  PNL.UnderlyingSymbol,UDASubSector,SM.ExpirationDate,SM.BloombergSymbol ,Asset      
    
having     
Sum(BeginningQuantity*PNL.Delta*SideMultiplier) <>0    
--order by ExpDate,UDASector,BBGSymbol    
    
select    
SubSector,    
Underlying,    
ExpDate,    
MAX(Exchange) as Exchange,    
--Exchange,    
BBGSymbol,    
Sum(FuturePosition) as FuturePosition,    
Sum(DeltaPosition) as DeltaPosition,    
Sum(FuturePosition_Adjusted) as FuturePosition_Adjusted,    
Sum(DeltaPosition_Adjusted) as DeltaPosition_Adjusted    
from #temp1    
GROUP BY  Underlying,SubSector,ExpDate,BbgSymbol--,Exchange     
order by SubSector,ExpDate,BBGSymbol    
Drop table #temp1    
    