
/****** Object:  Stored Procedure dbo.P_GetAUECCommissionRulesByCVAUECID    Script Date: 04/21/2006 12:20:23 PM ******/
CREATE PROCEDURE dbo.P_GetAUECCommissionRulesByCVAUECID
(
	@cvAUECID int
)
AS
	SELECT distinct RuleID, AUECID_FK, RuleName, ApplyRuletoID_FK, RuleDescription, CalculationID_FK, CurrencyID_FK,
				CommissionRateID_FK, Commission, ApplyCriteria,ApplyClrFee
FROM         T_AUECCommissionRules ACR inner join T_CVAUEC CVA on
				ACR.AUECID_FK = CVA.AUECID 
				where CVA.CVAUECID = @cvAUECID order by RuleID




