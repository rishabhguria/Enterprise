CREATE procedure       
 P_GetOpenPositionFundWise      
as      
      
SELECT      
Symbol,  
Sum(TaxlotOpenQty*  
CASE OrderSideTagValue    
When '1' then 1    
When '3' then 1    
When 'A' then 1    
When 'B' then 1    
else -1    
end )   
AS TotalOpenQty,  
FundID  
From PM_Taxlots where taxlot_PK in (Select max(taxlot_PK) from PM_Taxlots group by TaxlotID)  
GROUP BY  Symbol,FundID  
ORDER BY Symbol  
  