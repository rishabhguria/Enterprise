


/****** Object:  Stored Procedure dbo.P_GetAllOrderTypes    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllOrderTypes

AS
	SELECT     OrderTypesID, OrderTypes, OrderTypeTagValue
FROM         T_OrderType Order By OrderTypes



