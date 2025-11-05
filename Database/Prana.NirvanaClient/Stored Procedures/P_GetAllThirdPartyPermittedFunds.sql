---------------------------------------------------
--Modified By: Bharat Raturi
--Date: 16/05/2014
--Purpose: Get all the funds for the third party clients

--usage:P_GetAllThirdPartyPermittedFunds 
--------------------------------------------------- 
CREATE procedure [dbo].[P_GetAllThirdPartyPermittedFunds]
as
select
CompanyThirdPartyID, CompanyID, CompanyFundID, FundName, ISNULL(FundReleaseID,0) AS ReleaseID
 from T_CompanyFunds Where IsActive=1 order BY CompanyThirdPartyID, CompanyID
