CREATE PROC [P_GetGroupMinimalForDate]      
(      
 @inputDate datetime      
)        
AS      
      
SELECT      
PT.Symbol,      
PT.TaxlotOpenQty*    
  CASE WHEN (PT.ORderSideTagValue IN ('A', 'B', '1'))       
  THEN 1       
  WHEN (PT.ORderSideTagValue IN ('2', '5', '6', 'C', 'D'))       
  THEN - 1       
  ELSE 1 END       
AS Quantity,    
AUECLocalDate    
FROM T_Group Inner Join PM_Taxlots PT  ON  T_Group.GroupID = PT.GroupID                                                                        
and taxlot_PK in                                                             
(                                        
 select max(taxlot_PK) from PM_Taxlots                                                             
 where datediff(d, PM_Taxlots.AUECModifiedDate,@inputDate) > 0            
 group by taxlotid                           
)                                                            
WHERE TaxLotOpenQty<>0   
order by GroupRefID 
  
SELECT      
Symbol,      
CumQty*    
  CASE WHEN (ORderSideTagValue IN ('A', 'B', '1'))       
  THEN 1       
  WHEN (ORderSideTagValue IN ('2', '5', '6', 'C', 'D'))       
  THEN - 1       
  ELSE 1 END       
AS Quantity,    
AUECLocalDate    
FROM      
T_Group    
WHERE      
datediff(d,AUECLocalDate,@inputDate) = 0       
order  by GroupRefID 