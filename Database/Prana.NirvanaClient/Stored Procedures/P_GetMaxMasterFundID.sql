CREATE procedure [dbo].[P_GetMaxMasterFundID]
as
select max(CompanyMasterFundID)+1 from T_CompanyMasterFunds
