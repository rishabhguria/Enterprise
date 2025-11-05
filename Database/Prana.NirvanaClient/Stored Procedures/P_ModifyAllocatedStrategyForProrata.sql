

CREATE Procedure [dbo].[P_ModifyAllocatedStrategyForProrata] (  
@GroupID varchar(50),  
  
@StrategyID int,  
@AllocatedQty float  
)  
As  
  
Update T_StrategyAllocation   
Set AllocatedQty=@AllocatedQty  
Where GroupID=@GroupID  
And StrategyID=@StrategyID
