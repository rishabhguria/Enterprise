
  
  
CREATE Procedure [dbo].[P_GetCancelAmendGroupedOrders] (@GroupID varchar(100))                        
as                        
                        
select                   
F.ExecutionID,                        
F.ClorderID,                        
F.LastShares,        
F.LastPx,      
F.CumQty,        
F.AveragePrice,                        
S.ParentClOrderID,      
--OC.Commission,      
--OC.Fees,    
S.Quantity,  
F.TransactTime,  
O.AUECID,
S.NirvanaMsgType        
  
from                         
T_Fills as F         
left outer join T_Sub S on S.ClOrderID= F.ClOrderID         
left outer join T_Order O on O.ParentClOrderID = S.ParentClOrderID        
left outer Join T_GroupOrder GO on GO.ClOrderID= O.ParentClOrderID        
left outer Join T_Group G on G.GroupID = GO.GroupID       
      
Where G.GroupID = @GroupID        
  order by F.CumQty     
  
   
