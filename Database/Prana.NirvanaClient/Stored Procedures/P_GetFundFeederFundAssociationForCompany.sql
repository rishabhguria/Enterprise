-----------------------------------------------------------------
--Modified BY: Bharat Raturi
--Date: 10-mar-2014
--Purpose: Get the mapping of the fund with feeder fund 
-----------------------------------------------------------------
CREATE procedure [dbo].[P_GetFundFeederFundAssociationForCompany]
@companyID int
as
select map.CompanyMasterSubAccountID, 
map.CompanyFundID, 
map.companyfeederfundid, 
map.AllocatedAmount,
feed.FeederFundShortName,
feed.CompanyID,
feed.Amount,
feed.RemainingAmount,
feed.currency
from T_CompanyFundFeederFundAssociation map inner JOIN T_CompanyFeederFunds feed
on map.CompanyFeederFundID=feed.FeederFundID
where companyID=@companyID and feed.IsActive=1
