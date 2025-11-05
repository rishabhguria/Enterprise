Create PROCEDURE [dbo].[P_GetAuecOtherFeesRules] 
as
Select 
OtherFeeRuleID,
LongFeeRate,
ShortFeeRate,
LongCalculationBasis,
ShortCalculationBasis,
RoundOffPrecision,
MaxValue,
MinValue,
AUECID,
FeeTypeID,
RoundUpPrecision, 
RoundDownPrecision, 
FeePrecisionType,
IsCriteriaApplied

From T_OtherFeeRules Order By AUECID
