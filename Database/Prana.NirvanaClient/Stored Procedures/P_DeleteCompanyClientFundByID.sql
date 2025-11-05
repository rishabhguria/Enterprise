


/****** Object:  Stored Procedure dbo.P_DeleteCompanyClientFundByID    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClientFundByID
(
	@companyClientFundID int	
)
AS

Delete T_CompanyClientFund
Where CompanyClientFundID = @companyClientFundID



