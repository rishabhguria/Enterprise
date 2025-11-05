

/****** Object:  Stored Procedure dbo.P_GetRules   Script Date: 2/17/2006 7:25:21 PM ******/
CREATE PROCEDURE dbo.P_GetRules
AS
	Select ApplyRuletoId, TypeofTrade
	From T_ApplyRule
	Order By TypeofTrade Asc


