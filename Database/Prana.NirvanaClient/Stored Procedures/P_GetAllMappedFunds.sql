-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 10-mar-2014
--Purpose: Get fund-master fund mapping 
-----------------------------------------------------------------
Create procedure [dbo].[P_GetAllMappedFunds]
as
select DISTINCT CompanyFundID from T_CompanyMasterFundSubAccountAssociation
