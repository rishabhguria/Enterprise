

/****** Object:  Stored Procedure dbo.P_GetCompanyBorrower    Script Date: 01/03/2005 1:08:22 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyBorrower
	(
		@companyID	int	
	)
AS
	
	Select CompanyBorrowerID, BorrowerName, BorrowerShortName, BorrowerFirmID, CompanyID 
	From T_CompanyBorrower
	Where CompanyID = @companyID

