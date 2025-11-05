
/****** Object:  Stored Procedure dbo.P_DeleteCompanyBorrowerByBorrowerID    Script Date: 05/18/2006 9:00:22 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyBorrowerByBorrowerID
	(
		@companyBorrowerID int	
	)
AS
	
			Delete T_CompanyBorrower
			Where CompanyBorrowerID = @companyBorrowerID

