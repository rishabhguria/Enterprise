


/****** Object:  Stored Procedure dbo.P_GetAUECOrderTypes    Script Date: 12/21/2005 3:25:22 PM ******/
CREATE PROCEDURE dbo.P_GetAUECOrderTypes
	(
		@auecID int		
	)
AS
	
	SELECT AUECID, OrderTypeID
	FROM	T_AUECOrderTypes
	Where  AUECID = @auecID

