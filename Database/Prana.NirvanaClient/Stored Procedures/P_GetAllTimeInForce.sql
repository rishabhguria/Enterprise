


/****** Object:  Stored Procedure dbo.P_GetAllTimeInForce    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllTimeInForce

AS
	SELECT     TimeInForceID, TimeInForce, TimeInForceTagValue
FROM         T_TimeInForce Order By TimeInForce


