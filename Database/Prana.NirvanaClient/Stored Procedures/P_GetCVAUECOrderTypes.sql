


/****** Object:  Stored Procedure dbo.P_GetCVAUECOrderTypes    Script Date: 12/28/2005 4:27:22 PM ******/
CREATE PROCEDURE dbo.P_GetCVAUECOrderTypes
	(
		@cvAuecID int		
	)
AS
	
	SELECT CVAUECID, OrderTypesID
	FROM	T_CVAUECOrderTypes	
	Where  CVAUECID = @cvAuecID

