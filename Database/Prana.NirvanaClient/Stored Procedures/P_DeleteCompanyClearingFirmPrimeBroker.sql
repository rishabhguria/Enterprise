


/****** Object:  Stored Procedure dbo.P_DeleteCompanyClearingFirmPrimeBroker    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClearingFirmPrimeBroker
	(
		@companyClearingFirmsPrimeBrokersID int	
	)
AS
Delete T_CompanyClearingFirmsPrimeBrokers
Where CompanyClearingFirmsPrimeBrokersID = @companyClearingFirmsPrimeBrokersID


