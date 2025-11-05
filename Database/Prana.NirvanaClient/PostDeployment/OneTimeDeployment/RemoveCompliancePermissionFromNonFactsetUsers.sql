DELETE FROM T_CompanyUserModule WHERE 
CompanyUserID IN (
SELECT T_CompanyUser.UserID FROM T_CompanyUser INNER JOIN T_CompanyMarketDataProvider ON T_CompanyUser.CompanyID = T_CompanyMarketDataProvider.CompanyID
WHERE (T_CompanyUser.FactSetUsernameAndSerialNumber = '' OR T_CompanyUser.FactSetUsernameAndSerialNumber IS NULL) AND T_CompanyMarketDataProvider.MarketDataProvider = 8) 
AND CompanyModuleID IN (
SELECT DISTINCT CompanyModuleID from T_CompanyModule INNER JOIN T_Module ON T_CompanyModule.ModuleID = T_Module.ModuleID
WHERE T_Module.ModuleID IN (4,5,25,52,53,54))