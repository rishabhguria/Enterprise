

-- =============================================        
-- Author:  Abhishek Mehta        
-- Create date: 11-03-2008    
-- Description: Check Allocation of baskets and returning AUEC local Date       
-- =============================================        
CREATE PROCEDURE [dbo].[P_CheckBasketAllocation] (    
@TradedBasketID varchar(50),  
@AllocationType bit   
   
)    
As    
select isnull(StateID,0),AUECLocalDate from BT_BasketGroups     
left outer join BT_GroupsBaskets on     
BT_BasketGroups.GroupID= BT_GroupsBaskets.GroupID    
    
where BT_GroupsBaskets.BasketID = @TradedBasketID and BT_BasketGroups.AllocationType=@AllocationType
