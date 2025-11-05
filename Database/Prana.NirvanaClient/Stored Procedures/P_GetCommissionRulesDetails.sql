CREATE PROCEDURE [dbo].[P_GetCommissionRulesDetails] As   
         
SELECT           
CMR.RuleId ,          
CMR.RuleName,          
CMR.RuleDescription,          
CMR.ApplyRuleForTrade,          
CMR.CalculationBasedOn,          
CMR.CommissionRate,          
CMR.MinCommission,          
CMR.IsCriteriaApplied,          
CMR.IsClearingFeeApplied,          
CF.CalculationBasedOn AS CalculationBasedOnClearing,          
CF.FeeRate AS CommissionRateClearing,          
CF.MinFee AS MinimumCommissionClearing,         
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
CF2.MinFee as MinimumCommissionClearingBrokerFee,
CASE WHEN CF2.IsCriteriaApplied IS NOT NULL THEN CF2.IsCriteriaApplied ELSE 0 END AS IsCriteriaAppliedForClearingBrokerFee, 
CASE WHEN CF.IsCriteriaApplied IS NOT NULL THEN  CF.IsCriteriaApplied ELSE 0 END AS IsCriteriaAppliedForClearingFee
FROM T_CommissionRules CMR          
Left Outer Join T_ClearingFee CF ON CMR.RuleId=CF.RuleId_FK and CF.BrokerLevelFeeType = 0
Left OUTER Join T_ClearingFee CF2 on CMR.RuleId=CF2.RuleId_FK and CF2.BrokerLevelFeeType = 1 
  
SELECT  
CRA.RuleAssetId,  
CRA.AssetId_FK AS AssetID,  
ASSET.AssetName,  
CRA.RuleId_FK AS RuleID  
FROM T_CommissionRuleAssets CRA  
INNER JOIN T_CommissionRules CR  
ON CRA.RuleId_FK=CR.RuleId  
INNER JOIN T_Asset ASSET  
ON ASSET.AssetID=CRA.AssetId_FK  
  
SELECT   
CC.CommissionCriteriaId,  
CC.ValueGreaterThan,  
CC.ValueLessThanOrEqualTo,  
CC.RuleId_FK AS RuleID,  
CC.CommissionRate,
CC.CommissionType  
FROM T_CommissionCriteria CC  
INNER JOIN T_CommissionRules CR  
ON CC.RuleId_FK=CR.RuleId  
ORDER BY CC.ValueGreaterThan 

SELECT   
CC.ClearingFeeCriteriaID,  
CC.ValueGreaterThan,  
CC.ValueLessThanOrEqualTo,  
CC.RuleId_FK AS RuleID,  
CC.ClearingFeeRate,
CC.ClearingFeeType  
FROM T_ClearingFeeCriteria CC  
INNER JOIN T_CommissionRules CR  
ON CC.RuleId_FK=CR.RuleId  
ORDER BY CC.ValueGreaterThan
