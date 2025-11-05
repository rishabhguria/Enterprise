
/****** Object:  Stored Procedure dbo.P_GetCompanyCompliance    Script Date: 01/03/2005 1:05:22 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyCompliance
	(
		@companyID	int	
	)
AS
	
	Select CompanyComplianceID, FixVersionID, FixCapabilityID, CompanyID 
	From T_CompanyCompliance
	Where CompanyID = @companyID
