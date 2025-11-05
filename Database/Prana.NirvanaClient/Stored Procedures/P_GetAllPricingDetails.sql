
Create  procedure [dbo].[P_GetAllPricingDetails]
@companyID int
as
select
PricingRuleID, 
CompanyFundID,
AssetClassID,
ExchangeID,
PricingDataType,
SourceID,
SecondarySourceID,
CompanyID,
RuleType,
TimeDuration,
IsPricingPolicy,
PricingPolicyID 
from T_CompanyPricingMaster
where CompanyID=@companyID
order BY
PricingRuleID
