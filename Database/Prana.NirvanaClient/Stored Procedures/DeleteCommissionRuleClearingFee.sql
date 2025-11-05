CREATE PROCEDURE dbo.DeleteCommissionRuleClearingFee
	(
		
		@ruleID int
	)

AS
					 
				Delete T_CommissionRuleClearingFee
				Where RuleId = @ruleID 			
			
				
				UPDATE T_AUECCommissionRules SET ApplyClrFee=0 
				WHERE Ruleid=@ruleID
				
				
	return 1

