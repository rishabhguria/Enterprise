
Create PROCEDURE [dbo].[P_CA_CheckPreComplianceEnabled]
@CompanyId int
AS
BEGIN	
	Select C.CompanyId, M.ModuleName 
	from T_CompanyModule as C join T_Module as M on C.ModuleId = M.ModuleId 
	where C.CompanyId = @CompanyId and M.ModuleName in ('Compliance Pre Trade')
END

