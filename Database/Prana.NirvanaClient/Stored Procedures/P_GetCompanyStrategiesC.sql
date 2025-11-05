

CREATE PROCEDURE [dbo].[P_GetCompanyStrategiesC] AS  
 SELECT     CompanyStrategyID, StrategyShortName, StrategyName, CompanyID  
 FROM         T_CompanyStrategy
