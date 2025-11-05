CREATE procedure [dbo].[P_GetAllPricingRulesForMarkPrice]
as
select
PricingRuleID, 
CompanyFundID,
AssetClassID,
ExchangeID,
PricingDataType,
CP.SourceID,
SecondarySourceID,
CompanyID,
SS.SourceName,
CP.IsPricingPolicy,
CP.PricingPolicyID
from T_CompanyPricingMaster CP
left JOIN T_PricingSecondarySource  SS on CP.SecondarySourceID = SS.SourceID
order BY
PricingRuleID
