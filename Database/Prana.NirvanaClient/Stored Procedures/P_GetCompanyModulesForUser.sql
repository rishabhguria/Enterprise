
Create PROCEDURE [dbo].[P_GetCompanyModulesForUser]
(
		@companyUserID int		
)
AS
	/* SELECT    CM.ModuleID, M.ModuleName
	FROM         T_CompanyUserModule CM, T_Module M
	Where CM.CompanyUserID = @companyUserID AND CM.ModuleID = M.ModuleID */
	
	Select CUM.CompanyUserModuleID, CUM.CompanyModuleID, M.ModuleName, CUM.Read_WriteID,M.ModuleID,CUM.IsShowExport
	From T_CompanyModule CM, T_Module M, T_CompanyUserModule CUM
	Where CM.CompanyModuleID = CUM.CompanyModuleID And M.ModuleID = CM.ModuleID 
	And CUM.CompanyUserID = @companyUserID and M.ModuleID not in (select distinct ModuleID from T_MarketDataComponent except  
	(select ModuleID from T_MarketDataComponent MDC, T_MarketDataType MDT, T_CompanyUserMarketDataTypes CUMDT where 
	MDC.MarketDataTypeID = MDT.MarketDataTypeID and MDT.MarketDataTypeID = CUMDT.MarketDataTypeID and CUMDT.CompanyUserID = @companyUserID)) order by M.ModuleName
