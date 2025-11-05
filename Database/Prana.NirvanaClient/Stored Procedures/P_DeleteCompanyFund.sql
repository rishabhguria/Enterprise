


/****** Object:  Stored Procedure dbo.P_DeleteCompanyFund    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyFund
	(
		@companyFundID int	
	)
AS
Delete T_CompanyFunds
Where CompanyFundID = @companyFundID


