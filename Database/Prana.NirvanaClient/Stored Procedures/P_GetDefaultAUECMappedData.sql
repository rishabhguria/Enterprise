CREATE PROCEDURE [dbo].[P_GetDefaultAUECMappedData]
(
@companyID int
)
as   

select t1.CountryID,t2.CurrencySymbol,t1.AUECID 
from T_DefaultAUECMapping t1 inner join T_Currency t2
on t1.CurrencyID=t2.CurrencyID inner join T_CompanyAUEC t3
on t1.AUECID=t3.AUECID where t3.CompanyID=@companyID