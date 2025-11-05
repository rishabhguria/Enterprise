CREATE PROCEDURE [dbo].[P_GetAUECOtherFeeCriteria]
 (    
   @RuleId  uniqueidentifier   
 )    
AS    
    
SELECT
OtherFeesCriteriaId,
LongValueGreaterThan,      
LongValueLessThanOrEqualTo,  
LongFeeRate, 
LongCalculationBasis,
OtherFeeRuleId_FK,
ShortValueGreaterThan,
ShortValueLessThanOrEqualTo,
ShortFeeRate,
ShortCalculationBasis	
FROM T_OtherFeesCriteria    
Where OtherFeeRuleId_FK = @RuleId  
order by OtherFeesCriteriaId
   