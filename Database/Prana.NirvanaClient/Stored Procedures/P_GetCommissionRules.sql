/******************************************************  
  
Date Modified : 2012-04-09  
Description: Insertion  
Modified By : Rahul Gupta      Details : http://jira.nirvanasolutions.com:8080/browse/PRANA-1437  
  
******************************************************/     
CREATE PROCEDURE [dbo].[P_GetCommissionRules] As        
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
CF.MinFee as MinimumCommissionClearing,        
CMR.MaxCommission,    
CMR.IsRoundOff,    
CMR.RoundOffValue,    
CMR.CalculationBasedOnForSoft,
CMR.CommissionRateForSoft,
CMR.MinCommissionForSoft,
CMR.MaxCommissionForSoft,
CMR.IsCriteriaAppliedForSoft,
CMR.IsRoundOffForSoft,
CMR.RoundOffValueForSoft,        
CMR.IsClearingBrokerFeeApplied,        
CF2.CalculationBasedOn as CalculationBasedOnClearingBrokerFee,        
CF2.FeeRate as CommissionRateClearingBrokerFee,        
CF2.MinFee as MinimumCommissionClearingBrokerFee
from T_CommissionRules CMR        
Left Outer Join T_ClearingFee CF on CMR.RuleId=CF.RuleId_FK and CF.BrokerLevelFeeType = 0
Left OUTER Join T_ClearingFee CF2 on CMR.RuleId=CF2.RuleId_FK and CF2.BrokerLevelFeeType = 1

