-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 28/02/14
--Purpose: Get the list of feederFunds for the company
-----------------------------------------------------------------

CREATE procedure [dbo].[P_GetAllFeederFunds] @companyID int
as
select feederFundId, FeederFundName, FeederFundShortName, CompanyID, Amount, Currency, AllocatedAmount 
from T_CompanyFeederFunds where CompanyID=@companyID and isActive=1 order BY FeederFundName
