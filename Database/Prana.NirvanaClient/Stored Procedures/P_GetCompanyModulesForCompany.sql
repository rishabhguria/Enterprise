



/****** Object:  Stored Procedure dbo.P_GetCompanyModulesForCompany    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_GetCompanyModulesForCompany]
	(
		@companyID int		
	)
AS
	SELECT   CM.CompanyModuleID, CM.CompanyID, M.ModuleName, M.ModuleID, CM.Read_WriteID
	FROM         T_CompanyModule CM, T_Module M
	Where CM.CompanyID = @companyID AND CM.ModuleID = M.ModuleID order by M.ModuleName



