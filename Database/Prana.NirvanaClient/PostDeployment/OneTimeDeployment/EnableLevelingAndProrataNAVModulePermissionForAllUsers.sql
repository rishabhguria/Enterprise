--Enable company level permission for Allocation Leveling
INSERT INTO T_CompanyModule (CompanyID, ModuleID, Read_WriteID)
SELECT CompanyID, 43, 1 FROM T_Company

--Enable company level permission for Allocation Pro-rata (NAV)
INSERT INTO T_CompanyModule (CompanyID, ModuleID, Read_WriteID)
SELECT CompanyID, 44, 1 FROM T_Company


--Enable user level permission for Allocation Leveling
INSERT INTO T_CompanyUserModule (CompanyModuleID, CompanyUserID, Read_WriteID)
SELECT CM.CompanyModuleID, CU.UserID, 1 FROM T_CompanyUser CU
INNER JOIN T_CompanyModule CM ON CM.CompanyID = CU.CompanyID AND CM.ModuleID = 43

--Enable user level permission for Allocation Pro-rata (NAV)
INSERT INTO T_CompanyUserModule (CompanyModuleID, CompanyUserID, Read_WriteID)
SELECT CM.CompanyModuleID, CU.UserID, 1 FROM T_CompanyUser CU
INNER JOIN T_CompanyModule CM ON CM.CompanyID = CU.CompanyID AND CM.ModuleID = 44