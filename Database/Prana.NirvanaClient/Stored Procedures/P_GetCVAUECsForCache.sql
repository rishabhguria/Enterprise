create procedure P_GetCVAUECsForCache
(
@companyUserID int 
)
as
select distinct CVAUECID,AUECID,CounterPartyID ,VenueID
from V_GetAllCVAUEC
where 
CompanyUserID=@companyUserID