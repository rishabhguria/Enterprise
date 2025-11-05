


/****** Object:  Stored Procedure dbo.P_DeleteCompanyMPID    Script Date: 12/29/2005 5:45:21 PM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyMPID
	(
		@companyID int	
	)
AS
Delete T_CompanyMPID
Where CompanyID = @companyID


