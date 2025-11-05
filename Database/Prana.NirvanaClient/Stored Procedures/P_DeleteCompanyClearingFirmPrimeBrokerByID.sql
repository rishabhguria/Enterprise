
/****** Object:  Stored Procedure dbo.P_DeleteCompanyClearingFirmPrimeBrokerByID    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClearingFirmPrimeBrokerByID
	(
		@companyClearingFirmsPrimeBrokersID int	
	)
AS
	
			Delete T_CompanyClearingFirmsPrimeBrokers
			Where CompanyClearingFirmsPrimeBrokersID = @companyClearingFirmsPrimeBrokersID
			
			Delete T_CompanyCounterPartyVenueDetails
			Where CompanyClearingFirmsPrimeBrokersID = @companyClearingFirmsPrimeBrokersID
		


