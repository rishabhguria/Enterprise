
Create PROCEDURE [dbo].[P_GetMarketDataColumnsNotForUser]
(
		@companyUserID int		
)
AS
	select M.ModuleName, MDC.ComponentColumn from T_Module M, T_MarketDataColumns MDC where M.ModuleID = MDC.ModuleID-- order by  M.ModuleName
 Except
	select M.ModuleName, MDC.ComponentColumn from T_Module M, T_MarketDataColumns MDC, T_MarketDataType MDT, T_CompanyUserMarketDataTypes CUMDT where 
	M.ModuleID = MDC.ModuleID and MDC.MarketDataTypeID = MDT.MarketDataTypeID and MDT.MarketDataTypeID = CUMDT.MarketDataTypeID and CUMDT.CompanyUserID = @companyUserID --order by  M.ModuleName

