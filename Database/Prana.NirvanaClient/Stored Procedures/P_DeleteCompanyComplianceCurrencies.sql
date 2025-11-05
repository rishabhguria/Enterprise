
/****** Object:  Stored Procedure dbo.P_DeleteCompanyComplianceCurrencies    Script Date: 05/18/2006 12:33:23 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyComplianceCurrencies
	(
		@companyID int	
	)
AS
Delete T_CompanyAllCurrencies
Where CompanyID = @companyID
