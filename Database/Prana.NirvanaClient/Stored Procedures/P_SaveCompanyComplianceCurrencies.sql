/****** Object:  Stored Procedure dbo.P_SaveCompanyComplianceCurrencies    Script Date: 05/15/2006 5:25:23 PM ******/
CREATE PROCEDURE dbo.P_SaveCompanyComplianceCurrencies
	(
		@currencyID int,
		@companyID int,
		@result int
	)
AS

	INSERT T_CompanyAllCurrencies(CurrencyID, CompanyID)
	Values(@currencyID, @companyID)
	
	Set @result = scope_identity()
select @result