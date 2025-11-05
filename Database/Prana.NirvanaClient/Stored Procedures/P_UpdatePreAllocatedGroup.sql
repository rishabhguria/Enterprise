

CREATE procedure [dbo].[P_UpdatePreAllocatedGroup] (  
@groupID varchar(50),  
@CumQty float,  
@AvgPrice float,  
@TargetQty float,  
@OrdTypeTagValue varchar(1)  
)  
as
  
Update T_Group   
Set   
AllocatedQty=@CumQty ,  
CumQty=@CumQty ,  
Quantity = @TargetQty ,  
OrderTypeTagValue = @OrdTypeTagValue ,  
AvgPrice = @AvgPrice   
Where GroupID=@groupID  
  
Update T_FundAllocation   
Set AllocatedQty=@CumQty   
Where GroupID=@groupID   
  
Update T_StrategyAllocation  
Set AllocatedQty=@CumQty   
Where GroupID=@groupID
