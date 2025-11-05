  
      
      
      
CREATE Procedure [dbo].[P_GetCancelAmendOrders] (@StartDate datetime ,@EndDate datetime )                              
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
--OC.Fees  ,            
O.OrderSideTagValue,        
O.AUECID,  
O.Symbol,  
S.CounterPartyID,  
S.VenueID,  
S.Quantity,  
G.GroupID,  
F.TransactTime,  
S.NirvanaMsgType  
--select *                    
from                               
T_Fills as F               
left outer join T_Sub S on S.ClOrderID= F.ClOrderID               
left outer join T_Order O on O.ParentClOrderID = S.ParentClOrderID              
left outer join T_GroupOrder GO on GO.ClorderID = O.ParentClOrderID        
Left outer Join T_Group G on  G.GroupID = GO.GroupID        
--and S.NirvanaMsgType != 3 and F.LastShares>0      
--order by F.CumQty      
Where DATEDIFF(d,S.AUECLocalDate,@StartDate)<=0  and DATEDIFF(d,S.AUECLocalDate,@EndDate)>=0           
and S.NirvanaMsgType != 3    
--and G.AllocationTypeID = 0 or G.AllocationTypeID is null       
        
order by F.CumQty      
      
      
      
--select * from T_Fills where Symbol = 'GOOG'      