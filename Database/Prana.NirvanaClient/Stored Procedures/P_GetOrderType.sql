


/****** Object:  Stored Procedure dbo.P_GetOrderType    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetOrderType
(
		@orderTypesID int
)
AS
SELECT     OrderTypesID, OrderTypes, OrderTypeTagValue
FROM         T_OrderType
Where OrderTypesID = @orderTypesID



