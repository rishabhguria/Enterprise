CREATE PROCEDURE [dbo].[P_GetAUECOtherFeeRules]    
 (    
  @auecID int    
 )    
AS    
    
 SELECT OtherFeeRuleID, LongFeeRate, ShortFeeRate, LongCalculationBasis, ShortCalculationBasis, RoundOffPrecision,  
   MaxValue, MinValue, AUECID, FeeTypeID, RoundUpPrecision, RoundDownPrecision, FeePrecisionType, IsCriteriaApplied
 FROM T_OtherFeeRules    
 Where AUECID = @auecID    
   
