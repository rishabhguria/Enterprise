/****** Object:  Stored Procedure dbo.P_GetAllAUECCommissionRulesbyRuleID   Script Date: 3/2/2006 10:38:22 AM ******/
CREATE PROCEDURE dbo.P_GetAllAUECCommissionRulesbyRuleID
	(
		@ruleID int		
	)
AS
	

	SELECT   RuleID, AUECID_FK, RuleName, ApplyRuletoID_FK,RuleDescription,CalculationID_FK,CurrencyID_FK,CommissionRateID_FK,Commission,ApplyCriteria,ApplyClrFee
FROM T_AUECCommissionRules 
Where RuleID= @ruleID

