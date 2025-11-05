

CREATE PROCEDURE dbo.P_GetCompanyUserBrokers
	(
		@userID int
		
	)
AS
	SELECT CompanyClearingFirmsPrimeBrokersID,  ClearingFirmsPrimeBrokersShortName

	FROM     T_CompanyClearingFirmsPrimeBrokers
	
	Where CompanyID = (SELECT CompanyID FROM T_Company)

