
CREATE PROCEDURE [dbo].[P_GetCompanyModule]
	(
		@companyModuleID int		
	)
AS
	SELECT     CompanyModuleID, M.ModuleName, Read_WriteID, M.ModuleID
	FROM         T_CompanyModule CM inner join 
                      T_Module M ON CM.ModuleID = M.ModuleID
	Where CompanyModuleID = @companyModuleID


