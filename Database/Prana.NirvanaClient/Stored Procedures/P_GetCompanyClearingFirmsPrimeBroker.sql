


/****** Object:  Stored Procedure dbo.P_GetCompanyClearingFirmsPrimeBroker    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetCompanyClearingFirmsPrimeBroker
	(
		@clearingFirmPrimeBrokerID int
	)
AS
	SELECT     CompanyClearingFirmsPrimeBrokersID, ClearingFirmsPrimeBrokersName, ClearingFirmsPrimeBrokersShortName, CompanyID
	FROM         T_CompanyClearingFirmsPrimeBrokers
	Where CompanyClearingFirmsPrimeBrokersID = @clearingFirmPrimeBrokerID



