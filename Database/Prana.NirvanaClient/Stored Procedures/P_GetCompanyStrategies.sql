-----------------------------------------------------------------
--Modified BY: Bhavana
--Date: 3-july-14
--Purpose: Get the sorted record by StrategyName
-----------------------------------------------------------------
/****** Object:  Stored Procedure dbo.P_GetCompanyStrategies    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_GetCompanyStrategies] (@companyID INT)
AS
SELECT CompanyStrategyID
	,StrategyName
	,StrategyShortName
	,CompanyID
FROM T_CompanyStrategy
WHERE CompanyID = @companyID
	AND IsActive = 1
ORDER BY StrategyName
