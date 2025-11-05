-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 28/02/14
--Purpose: Get the names of feeder funds for the company
-----------------------------------------------------------------
CREATE procedure [dbo].[P_GetAllFeederNames]
as
select FeederFundName from T_CompanyFeederFunds where IsActive=1
