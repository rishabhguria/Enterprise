

/****** Object:  Stored Procedure dbo.P_GetCompanyUserAUEC    Script Date: 01/11/2006 10:45:22 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyUserAUEC
	(
		@companyUserID	int	
	)
AS
	
	Select CompanyUserAUECID, CompanyUserID, CompanyAUECID
	From T_CompanyUserAUEC Where CompanyUserID = @companyUserID
	

