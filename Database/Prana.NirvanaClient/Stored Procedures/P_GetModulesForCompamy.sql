


/****** Object:  Stored Procedure dbo.P_GetModulesForCompamy    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetModulesForCompamy
	(
		@companyID int		
	)
AS
	SELECT     CM.CompanyID, CM.ModuleID, M.ModuleName
	FROM         T_CompanyModule CM, T_Module M
	Where CM.CompanyID = @companyID AND CM.ModuleID = M.ModuleID



