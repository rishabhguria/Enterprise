


/****** Object:  Stored Procedure dbo.P_GetCVFIX    Script Date: 12/29/2005 6:05:22 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyMPIDs
	(
		@companyID int	
	)
AS
	
	Select CompanyMPID, CompanyID, MPID From T_CompanyMPID
	Where CompanyID = @companyID


