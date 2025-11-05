create PROCEDURE [dbo].[P_GetModuleForMarketData]	
as 
select MarketDataType, ModuleName from 	T_MarketDataType MDT, T_MarketDataComponent MDC, T_Module M 
where MDT.MarketDataTypeID =  MDT.marketDataTypeID and MDC.ModuleID = M.ModuleID
