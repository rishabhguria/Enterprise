

CREATE procedure [dbo].[P_SaveGroupOrders] (    
@groupID varchar(50),    
@clOrderID varchar(50)    
)    
as    
insert into T_GroupOrder(GroupID,ClOrderID) values(@groupID,@clOrderID)
