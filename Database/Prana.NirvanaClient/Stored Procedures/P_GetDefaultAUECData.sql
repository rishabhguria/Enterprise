CREATE PROCEDURE [dbo].[P_GetDefaultAUECData]
(
@companyID int
)
as    
select CountryID,CurrencyID,t1.AUECID from T_DefaultAUECMapping t1 inner join T_CompanyAUEC t2 
on t1.AUECID=t2.AUECID where t2.CompanyID=@companyID