 /**************************************        
        
[P_MW_GetCards_Testing] '2014-06-05','47887,47888,61542,80884,0VQ-954118,99726,OLC-926715'        
***************************************/        
        
        
CREATE Procedure [dbo].[P_MW_GetCards_Subsector]                                                                                       
(                                                                                                                   
 @Date datetime,                   
 @masterfund varchar(max)                                                                                                    
)                                                           
As         
        
--declare @Date datetime        
--declare @masterfund varchar(max)         
--        
--Set @Date='2014-06-05'        
--Set @masterfund='47887,47888,61542,80884,0VQ-954118,99726,OLC-926715'       
    
       
    
SELECT          
MasterFund as MasterFundName,       
UDASubsector as SubSector,         
PNL.UnderlyingSymbol as Underlying,          
CAST(FLOOR(CAST(SM.ExpirationDate AS FLOAT ) ) AS DATETIME ) as ExpDate ,              
Asset as AssetClass,         
Exchange as Exchange,        
CASE            
WHEN Asset = 'Future'            
THEN Sum(BeginningQuantity*SideMultiplier)        
ELSE 0            
END as FuturePosition,        
CASE            
WHEN Asset = 'FutureOption'        
THEN Sum(BeginningQuantity*PNL.Delta*SideMultiplier)        
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
MasterFund in (SELECT * FROM dbo.split(@masterfund, ','))            
GROUP BY MasterFund,UDASubsector, PNL.UnderlyingSymbol,SM.ExpirationDate,Symbol, Asset,Exchange,SM.BloombergSymbol          
having         
Sum(BeginningQuantity*PNL.Delta*SideMultiplier) <>0        
--order by MasterFund,UDASector,SM.BloombergSymbol,ExpDate        
    
    
    
    
Insert into #temp    
select     
'Total' as MasterFundName,        
SubSector,        
Underlying,        
ExpDate,    
AssetClass,        
Exchange,        
FuturePosition,        
DeltaPosition,        
BbgSymbol      
From #temp        
    
    
    
Select         
MasterFundName,        
SubSector,        
Underlying,        
ExpDate,        
Max(Exchange) as Exchange,        
Sum(FuturePosition) as FuturePosition,        
Sum(DeltaPosition) as DeltaPosition,        
BbgSymbol ,        
Case          
when SubSector='Copper'          
then 0          
else 1          
END as SortOrder        
From #temp        
GROUP BY MasterFundName,SubSector,Underlying,ExpDate,BbgSymbol--,Exchange          
order by SortOrder,SubSector,MasterFundName,ExpDate,BbgSymbol        
        
        
Drop table #temp