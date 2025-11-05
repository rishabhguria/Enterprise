

/**
	Give permission to company for all modules if the company module permission table is emplt (eg. on a fresh install)
*/
IF (NOT EXISTS(SELECT 1 FROM T_CompanyModule)) 
BEGIN
	insert into T_CompanyModule(CompanyID, ModuleID, Read_WriteID)
	select CompanyID, ModuleID, 1 as Read_WriteID
	from T_company cross JOIN T_Module
END