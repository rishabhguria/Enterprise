CREATE PROCEDURE [dbo].[P_GetCommissionRuleCriterias] (  
   @RuleId  uniqueidentifier  
)  
AS   
  
SELECT CommissionCriteriaId,  
 ValueGreaterThan,  
 ValueLessThanOrEqualTo, CommissionRate, CommissionType FROM T_CommissionCriteria   
WHERE RuleId_FK=@RuleId order by ValueGreaterThan  

