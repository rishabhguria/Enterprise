CREATE procedure [dbo].[P_GetFundsForSchedule]
as
select 
t1.ImportFileSettingID, t1.FundID, t2.fundName  
from T_ImportFileSettings t1 inner JOIN T_CompanyFunds t2
on
t1.FundID=t2.CompanyFundID
