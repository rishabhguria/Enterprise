

CREATE procedure [dbo].[P_BTUpdateBasketGroup] (    
@groupID varchar(50),    
@state int,    
@allocatedQty float ,  
@AUECLocalDate datetime    
)    
as    
update BT_BasketGroups    
set StateID =@state,    
 AllocatedQty = @allocatedQty,  
AllocationDate=@AUECLocalDate,
AUECLocalDate =  @AUECLocalDate  
where GroupID=@groupID
