                
-- [P_GetUnallocatedOrdersCount] '08/27/2008 12:00:00 AM'       
                                
CREATE procedure [dbo].[P_GetUnallocatedOrdersCount]           
(                                     
@date datetime                      
)                                            
as                                        
select count(*)                         
from T_Group as G        
where G.StateID=1 and CumQty>0 and      
datediff(d,G.AUECLocalDate,@date) = 0    
     
                                           
      
