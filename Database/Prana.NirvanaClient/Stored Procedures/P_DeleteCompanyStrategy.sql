


/****** Object:  Stored Procedure dbo.P_DeleteCompanyStrategy    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyStrategy
	(
		@companyStrategyID int	
	)
AS
Delete T_CompanyStrategy
Where CompanyStrategyID = @companyStrategyID


