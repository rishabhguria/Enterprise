CREATE PROCEDURE [dbo].[GetClOrdIDFromParentClOrderID]
(
@parentclOrderID varchar(50)    
)
AS
SELECT CLOrderID FROM T_TradedOrders WHERE ParentclOrderID = @parentclOrderID 
