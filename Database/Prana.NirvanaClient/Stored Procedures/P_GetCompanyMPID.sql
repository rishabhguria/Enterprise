

/****** Object:  Stored Procedure dbo.GetCompanyMPID    Script Date: 01/07/2006 4:30:22 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyMPID
	(
		@companyMPID int	
	)
AS
	
	Select CompanyMPID, CompanyID, MPID From T_CompanyMPID
	Where CompanyMPID = @companyMPID



