CREATE PROCEDURE dbo.P_GetCommissionRuleClearingFeeByRuleId
(
   @RuleId int=0
)
AS	

SELECT CRCF.ClearingFeeID,
 CRCF.RuleId,
 CRCF.CalculationId, CRCF.CurrencyId, 
 CRCF.CommissionRate,
CC.CalculationType
 FROM T_CommissionRuleClearingFee CRCF
INNER JOIN T_CommissionCalculation CC
ON CRCF.CalculationId=CC.CommissionCalculationID
WHERE CRCF.RuleId=@RuleId 