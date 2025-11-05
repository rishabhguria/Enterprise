


/****** Object:  Stored Procedure dbo.P_GetCompanyClearingFirmsPrimeBrokers    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetCompanyClearingFirmsPrimeBrokers
	(
		@companyID int
	)
AS
	SELECT     CompanyClearingFirmsPrimeBrokersID, ClearingFirmsPrimeBrokersName, ClearingFirmsPrimeBrokersShortName, CompanyID
	FROM         T_CompanyClearingFirmsPrimeBrokers
	Where CompanyID = @companyID



