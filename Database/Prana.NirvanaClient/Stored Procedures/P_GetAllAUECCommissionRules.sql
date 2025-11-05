/****** Object:  Stored Procedure dbo.P_GetAllAUECCommissionRules    Script Date: 2/17/2006 12:41:23 AM ******/
CREATE PROCEDURE dbo.P_GetAllAUECCommissionRules
AS
	SELECT   RuleID, AUECID_FK, RuleName, ApplyRuletoID_FK, RuleDescription, CalculationID_FK, CurrencyID_FK,
				CommissionRateID_FK, Commission, ApplyCriteria,ApplyClrFee
FROM         T_AUECCommissionRules order by RuleID