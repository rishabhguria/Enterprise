


/****** Object:  Stored Procedure dbo.P_GetCompanyModulesC    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetCompanyModulesC
	
AS
	SELECT     CM.ModuleID, M.ModuleName
	FROM         T_CompanyModule CM INNER JOIN
                      T_Module M ON CM.ModuleID = M.ModuleID
	

	


