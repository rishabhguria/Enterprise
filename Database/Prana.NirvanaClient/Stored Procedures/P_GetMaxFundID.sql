CREATE procedure [dbo].[P_GetMaxFundID]
as
select max(CompanyFundID)+1 from T_CompanyFunds
