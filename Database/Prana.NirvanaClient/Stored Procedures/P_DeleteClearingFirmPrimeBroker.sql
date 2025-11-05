


/****** Object:  Stored Procedure dbo.P_DeleteClearingFirmPrimeBroker    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_DeleteClearingFirmPrimeBroker
	(
		@clearingFirmsPrimeBrokersID int	
	)
AS
Delete T_ClearingFirmsPrimeBrokers
Where ClearingFirmsPrimeBrokersID = @clearingFirmsPrimeBrokersID


