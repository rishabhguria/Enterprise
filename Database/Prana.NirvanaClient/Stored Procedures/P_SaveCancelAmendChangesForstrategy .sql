

CREATE PROCEDURE [dbo].[P_SaveCancelAmendChangesForstrategy ] (        
  @OrderXml Xml ,       
 @ErrorMessage varchar(500) output                                           
   ,@ErrorNumber int output              
)        
AS        
                                                                                   
SET @ErrorMessage = 'Success'                                                                                      
SET @ErrorNumber = 0                                                                                      
BEGIN TRAN TRAN1                      
BEGIN TRY   
DECLARE @handle int  
exec sp_xml_preparedocument @handle OUTPUT,@OrderXml
 CREATE Table #OrdersandFills    
(    
ExecutionID varchar(50),                        
ClorderID bigint,                        
LastPx float ,      
AveragePrice float ,                        
ParentClOrderID bigint ,      
OrderCommission float ,      
OrderFees float  ,      
CounterPartyID int ,    
VenueID int      
)    
Insert Into #OrdersandFills    
(    
ExecutionID ,                    
ClorderID,                        
LastPx,     
AveragePrice,    
ParentClOrderID,    
OrderCommission ,    
OrderFees,     
CounterPartyID ,    
VenueID     
)    
Select     
ExecutionID ,                    
ClorderID,                        
LastPx,     
AveragePrice,    
ParentClOrderID,    
OrderCommission ,    
OrderFees,     
CounterPartyID ,    
VenueID     
    
FROM  OPENXML(@handle, '//OrdersAndFills',2)    
 WITH      
(    
ExecutionID varchar(50),                        
ClorderID bigint,                        
LastPx float ,      
AveragePrice float ,                        
ParentClOrderID bigint ,      
OrderCommission float ,      
OrderFees float  ,      
CounterPartyID int ,    
VenueID int     
)
CREATE TABLE #Orders    
(    
ParentClOrderID bigint ,      
OrderCommission float ,      
OrderFees float     
    
)    
INSERT INTO #Orders    
(    
ParentClOrderID ,      
OrderCommission ,    
OrderFees    
)    
SELECT     
DISTINCT     
ParentClOrderID ,      
OrderCommission ,    
OrderFees    
FROM #OrdersandFills    

UPdate T_Group     
SET T_Group.AvgPrice = G.AP   
FROM     
(Select T_Group.GroupID, (Sum( V_TradedOrders.AvgPrice*V_TradedOrders.Quantity)/Sum (V_TradedOrders.Quantity)) as AP  from V_TradedOrders      
left outer Join  #Orders on #Orders.ParentClOrderID=V_TradedOrders.ParentClorderID left outer Join  T_GroupOrder on T_groupOrder.ClorderID = #Orders.ParentClOrderID     
left outer join  T_Group on T_Group.GroupID = T_GroupOrder.GroupID     
where T_Group.AllocationTypeID = 1 Group by T_Group.GroupID )As G  where T_Group.GroupID = G.GroupID  

Update T_Group
SET T_Group.AvgPrice = G.AP
FROM 
(Select T_RelationShip.StrategyGroupID As GroupID ,T_Group.AvgPrice as AP ,T_Group.IsManualGroup from 
T_RelationShip left outer join T_Group on T_Group.GroupID=T_RelationShip.StrategyGroupID) AS G 
Where G.IsManualGroup = 1 and T_Group.GroupID=G.GroupID

   
UPDATE T_GroupCommission
SET T_GroupCommission.Commission = G.Commission ,
    T_GroupCommission.Fees = G.Fees
FROM     
(Select T_Group.GroupID, (Sum(#Orders.OrderCommission)) as Commission,(Sum(#Orders.OrderFees)) as Fees from 
 #Orders left outer Join  T_GroupOrder on T_groupOrder.ClorderID = #Orders.ParentClOrderID     
left outer join  T_Group on T_Group.GroupID = T_GroupOrder.GroupID  left outer join T_GroupCommission 
on T_GroupCommission.GroupID_Fk = T_Group.GroupID    
where T_Group.AllocationTypeID = 1 Group by T_Group.GroupID )As G  where T_GroupCommission.GroupID_Fk = G.GroupID   

UPDATE T_StrategyAllocationCommission        
SET T_StrategyAllocationCommission.Commission = CC.Commission,        
 T_StrategyAllocationCommission.Fees = CC.Fees   
FROM (Select T_StrategyAllocation.AllocationID,(T_GroupCommission.Commission*T_StrategyAllocation.Percentage/100) As Commission,
 (T_GroupCommission.Fees *T_StrategyAllocation.Percentage/100) As Fees from T_StrategyAllocation
 left outer join T_Group on  T_Group.GroupID = T_StrategyAllocation.GroupID 
left outer join T_GroupCommission  on T_GroupCommission.GroupID_Fk= T_Group.GroupID 
left outer join T_GroupOrder on T_GroupOrder.GroupID= T_GroupCommission.GroupID_Fk 
left outer join #Orders on #Orders.ParentClorderID = T_GroupOrder.ClorderID) as CC 
where CC.AllocationID=T_StrategyAllocationCommission.AllocationID_Fk

Drop Table #Orders , #OrdersandFills         
         
EXEC sp_xml_removedocument @handle        

COMMIT TRANSACTION TRAN1                  
                            
                           
 END TRY                            
 BEGIN CATCH                             
 SET @ErrorMessage = ERROR_MESSAGE();                            
 SET @ErrorNumber = Error_number();                             
 ROLLBACK TRANSACTION TRAN1                               
 END CATCH   ;
