insert into T_CompanyModule(CompanyID,ModuleID,Read_WriteID)
select distinct CompanyID,52,1 from T_CompanyModule where ModuleID =25

insert into T_CompanyModule(CompanyID,ModuleID,Read_WriteID)
select distinct CompanyID,53,1 from T_CompanyModule where ModuleID =25

insert into T_CompanyModule(CompanyID,ModuleID,Read_WriteID)
select distinct CompanyID,54,1 from T_CompanyModule where ModuleID =25

insert into T_CompanyUserModule(CompanyModuleID,CompanyUserID,Read_WriteID,IsShowExport)
select (Select CompanyModuleID from T_CompanyModule where ModuleID=52),
CompanyUserID,1,1 from T_CompanyUserModule where CompanyModuleID in (select CompanyModuleID from T_CompanyModule where ModuleID =25)

insert into T_CompanyUserModule(CompanyModuleID,CompanyUserID,Read_WriteID,IsShowExport)
select (Select CompanyModuleID from T_CompanyModule where ModuleID=53),
 CompanyUserID,1,1 from T_CompanyUserModule where CompanyModuleID in(select CompanyModuleID from T_CompanyModule where ModuleID =25)

insert into T_CompanyUserModule(CompanyModuleID,CompanyUserID,Read_WriteID,IsShowExport)
select (Select CompanyModuleID from T_CompanyModule where ModuleID=54),
CompanyUserID,1,1 from T_CompanyUserModule where CompanyModuleID in(select CompanyModuleID from T_CompanyModule where ModuleID =25)

delete T_CompanyUserModule where CompanyModuleID=(Select CompanyModuleID from T_CompanyModule where ModuleID=25)
delete T_CompanyModule where ModuleID=25
delete T_Module where ModuleID=25




