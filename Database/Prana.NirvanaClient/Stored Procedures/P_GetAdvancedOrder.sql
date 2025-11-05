


/****** Object:  Stored Procedure dbo.P_GetAdvancedOrder    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAdvancedOrder
(
		@advancedOrdersID int
)
AS
	SELECT     AdvancedOrdersID, AdvancedOrders
FROM         T_AdvancedOrders
Where AdvancedOrdersID = @advancedOrdersID
 



