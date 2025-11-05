


/****** Object:  Stored Procedure dbo.P_GetTimeInForce    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetTimeInForce
(
		@timeInForceID int
)
AS
SELECT     TimeInForceID, TimeInForce, TimeInForceTagValue
FROM         T_TimeInForce
	Where TimeInForceID = @timeInForceID



