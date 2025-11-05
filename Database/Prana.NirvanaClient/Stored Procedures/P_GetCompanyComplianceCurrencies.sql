
/****** Object:  Stored Procedure dbo.P_GetCompanyComplianceCurrencies    Script Date: 05/18/2006 12:50:22 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyComplianceCurrencies
	(
		@companyID	int	
	)
AS
	
	Select CompanyAllCurrencyID, CurrencyID, CompanyID 
	From T_CompanyAllCurrencies
	Where CompanyID = @companyID
