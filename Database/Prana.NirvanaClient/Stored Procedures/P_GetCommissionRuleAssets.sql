
CREATE PROCEDURE [dbo].[P_GetCommissionRuleAssets] (
@RuleId uniqueIdentifier 
)
As
Select 
AssetId_FK as AssetId

FROM T_CommissionRuleAssets
Where RuleId_FK=@RuleId
