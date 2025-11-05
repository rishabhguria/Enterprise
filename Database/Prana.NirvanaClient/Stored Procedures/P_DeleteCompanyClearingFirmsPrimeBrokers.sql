


/****** Object:  Stored Procedure dbo.P_DeleteCompanyClearingFirmsPrimeBrokers    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClearingFirmsPrimeBrokers
	(
		@companyID int
	)
AS
	Delete T_CompanyClearingFirmsPrimeBrokers
	Where CompanyID = @companyID


