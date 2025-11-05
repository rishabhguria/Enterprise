CREATE PROCEDURE [dbo].[P_CA_GetPrePostModulesPermission]
 @companyID int    
  
AS  
 SELECT
	TM.ModuleId as ModuleId,
	TM.ModuleName as ModuleName,
	CM.CompanyModuleId as CompanyModuleId,
	CM.CompanyID as CompanyId,
	CUM.CompanyUserId as CompanyUserId,
	CUM.Read_WriteID as ReadWriteId
 FROM 
T_CompanyUserModule as CUM left outer join  
T_CompanyModule as CM on CUM.companyModuleId = Cm.companymoduleId   left outer join  
T_Module as TM on TM.moduleId = CM.moduleId 
 
 
where CM.CompanyID = @companyID and TM.ModuleName in ('Compliance Pre Trade','Compliance Post Trade')

