---------------------------------------------------
--Modified By: Bharat Raturi
--Modified Date: 16/05/2014
--Purpose: Get the client for the release
--usage:P_GetClientsForReleases 
---------------------------------------------------
CREATE procedure [dbo].[P_GetClientsForReleases]
as
SELECT DISTINCT
t1.CompanyReleaseID,t3.CompanyID,t3.Name 
from
T_CompanyReleaseDetails t1 inner join
T_CompanyReleaseMapping t2 on t1.CompanyReleaseID=t2.ReleaseID INNER JOIN
T_Company t3 on t3.CompanyID=t2.CompanyID  
where t3.IsActive = 1
