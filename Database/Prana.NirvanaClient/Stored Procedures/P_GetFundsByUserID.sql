

--Modified : Rajat
-- Date  : 26 Nov 2007
   CREATE   procedure [dbo].[P_GetFundsByUserID] (  
@UserID int  
)  
as  
--Changed to Fund long name as expnl service needs to fetch full names.
select T_CompanyFunds.CompanyFundID,T_CompanyFunds.FundShortName,T_CompanyFunds.FundName   
from T_CompanyUserFunds join   
T_CompanyFunds   
on  
T_CompanyFunds.CompanyFundID=T_CompanyUserFunds.CompanyFundID  
where   
T_CompanyUserFunds.CompanyUserID=@UserID
and T_CompanyFunds.IsActive=1

order by T_CompanyFunds.UIOrder

