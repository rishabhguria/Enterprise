create proc [dbo].[GetGroupIDFromParentClOrderID]     
(    
@parentclOrderID varchar(50)    
)    
AS    
SELECT GroupID FROM T_TradedOrders WHERE ParentclOrderID = @parentclOrderID 
