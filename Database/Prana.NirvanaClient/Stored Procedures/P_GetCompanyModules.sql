



/****** Object:  Stored Procedure dbo.P_GetCompanyModules    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_GetCompanyModules]
	(
		@companyID int		
	)
AS
	SELECT     CM.CompanyModuleID, CM.CompanyID, M.ModuleName, M.ModuleID, CM.Read_WriteID
	FROM         T_CompanyModule CM INNER JOIN
                      T_Module M ON CM.ModuleID = M.ModuleID
	WHERE     (CM.CompanyID = @companyID)

	



