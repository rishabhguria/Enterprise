
CREATE PROCEDURE [dbo].[GetAllCommissionRules] As
Select 
CMR.RuleId ,
CMR.RuleName,
CMR.RuleDescription,
CMR.ApplyRuleForTrade,
CMR.CalculationBasedOn,
CMR.CommissionRate,
CMR.MinCommission,
CMR.IsCriteriaApplied,
CMR.IsClearingFeeApplied,
CF.CalculationBasedOn as CalculationBasedOnClearing,
CF.FeeRate as CommissionRateClearing,
CF.MinFee as MinimunCommissionClearing
from T_CommissionRules CMR
Left Outer Join T_ClearingFee CF on CMR.RuleId=CF.RuleId_FK
