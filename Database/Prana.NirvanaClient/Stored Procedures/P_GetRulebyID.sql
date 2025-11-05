


/****** Object:  Stored Procedure dbo.P_GetRulebyID   Script Date: 2/18/2005 1:00:21 PM ******/
CREATE PROCEDURE dbo.P_GetRulebyID
	(
		@applyruletoId int
	)
AS
	
	Select ApplyRuletoId, TypeofTrade
	From T_ApplyRule
	Where ApplyRuletoId = @applyruletoId



