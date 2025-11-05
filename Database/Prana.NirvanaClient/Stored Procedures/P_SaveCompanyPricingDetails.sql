
/*********************************************************
Author: Sachin Mishra
Creation Date: 27/04/15
Purpose: http://jira.nirvanasolutions.com:8080/browse/CHMW-3483

**********************************************************/ 
 
CREATE procedure [dbo].[P_SaveCompanyPricingDetails]
(@xmlDoc ntext,@compID int,@deletedIds varchar(max))
as
declare @handle int
exec sp_xml_preparedocument @handle OUTPUT,@xmlDoc
IF len(@deletedIds) > 0
BEGIN
	CREATE TABLE #TEMPIDSFORDELETION( pricingid int)

INSERT INTO #TEMPIDSFORDELETION (pricingid)
    SELECT Items
    FROM [dbo].[Split] (@deletedIds, ',') 
END
CREATE TABLE #TempPricing                                                                               
(                                                                               
    PricingRuleID int,      
	CompanyFundID int,
	AssetClassID int,
	ExchangeID int,
	PricingDataType int,
	SourceID int,
	SecondarySourceID int,
 	CompanyID int,
    RuleType int,
    RuleTypeByTime int,
    IsPricingPolicy bit,
    PricingPolicyID int
) 
       
insert INTO #TempPricing                                                                               
(PricingRuleID,
CompanyFundID,
AssetClassID,
ExchangeID,
PricingDataType,
SourceID,
SecondarySourceID,
CompanyID,
RuleType,
RuleTypeByTime,
IsPricingPolicy,
PricingPolicyID
)
SELECT                                                                               
PricingRuleID,
CompanyFundID,
AssetClassID,
ExchangeID,
PricingDataType,
SourceID,
SecondarySourceID,
CompanyID,
RuleType,
RuleTypeByTime ,
IsPricingPolicy,
PricingPolicyID                                    
FROM OPENXML(@handle, '/dsPricing/dtPricing', 2)                                                                                 
WITH                                                                               
(                                                         
	PricingRuleID int,      
	CompanyFundID int,
	AssetClassID int,
	ExchangeID int,
	PricingDataType int,
	SourceID int,
	SecondarySourceID int,
	CompanyID int,
    RuleType int,
    RuleTypeByTime int,
IsPricingPolicy bit,
PricingPolicyID int
)
         
--select * from #TempPricing
                 
IF len(@deletedIds) > 0
begin
DELETE FROM T_CompanyPricingMaster
WHERE CompanyID=@compID and PricingRuleID in (select PricingRuleID from #TempPricing Union Select pricingid from #TEMPIDSFORDELETION)
end
else
begin
DELETE FROM T_CompanyPricingMaster
WHERE CompanyID=@compID and PricingRuleID in (select PricingRuleID from #TempPricing)
enD

update  T_PricingPolicy
set IsActive = 0


Insert into T_CompanyPricingMaster  
(
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
PricingPolicyID  )
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
RuleTypeByTime ,
IsPricingPolicy,
PricingPolicyID  
from #TempPricing 

update  T_PricingPolicy
set     IsActive = 1
where   Id in (SELECT PricingPolicyID from #TempPricing)
exec sp_xml_removedocument @handle

