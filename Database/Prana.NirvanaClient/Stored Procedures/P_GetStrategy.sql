


/****** Object:  Stored Procedure dbo.P_GetStrategy    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetStrategy
	(
		@strategyID int
	)
AS
	SELECT     CompanyStrategyID, StrategyName, StrategyShortName, CompanyID
	FROM         T_CompanyStrategy
	Where CompanyStrategyID = @strategyID


