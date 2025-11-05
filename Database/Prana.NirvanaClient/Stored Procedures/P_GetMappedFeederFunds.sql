-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 28/02/14
--Purpose: Get the list of feeder funds of the company that are mapped with funds
-----------------------------------------------------------------
CREATE procedure [dbo].[P_GetMappedFeederFunds]
as
select DISTINCT TM.companyfeederfundid from T_CompanyFundFeederFundAssociation TM
inner JOIN T_CompanyFeederFunds TF on TM.CompanyFeederFundID=TF.FeederFundID where TF.IsActive=1;
