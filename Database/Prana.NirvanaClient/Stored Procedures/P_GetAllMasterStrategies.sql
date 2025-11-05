-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 28/02/14
--Purpose: Get the list of master strategies for the company

--Modified BY: Bharat Raturi
--Date: 01/04/14
--Purpose: Get the list of master strategies for the specified company
--Usage: P_GetAllMasterStrategies 5
-----------------------------------------------------------------

CREATE procedure [dbo].[P_GetAllMasterStrategies]
@companyID int
as
select CompanyMasterStrategyID,MasterStrategyName
from T_CompanyMasterStrategy
where 
CompanyID=@companyID and IsActive=1
