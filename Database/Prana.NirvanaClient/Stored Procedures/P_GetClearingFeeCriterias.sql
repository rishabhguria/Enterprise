CREATE PROCEDURE [dbo].[P_GetClearingFeeCriterias] (  
   @RuleId  uniqueidentifier  
)  
AS   
  
SELECT ClearingFeeCriteriaID,  
 ValueGreaterThan,  
 ValueLessThanOrEqualTo, ClearingFeeRate, ClearingFeeType FROM T_ClearingFeeCriteria   
WHERE RuleId_FK=@RuleId order by ValueGreaterThan  

