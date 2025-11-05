/****** Object:  Stored Procedure dbo.P_GetUserStrategies    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetUserStrategies (@userID INT)
AS
SELECT CompanyStrategyID
	,StrategyName
	,StrategyShortName
FROM T_CompanyStrategy
