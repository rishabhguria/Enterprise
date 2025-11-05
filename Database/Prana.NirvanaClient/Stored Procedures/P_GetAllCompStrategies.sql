-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 28/02/14
--Purpose: Get the list of strategies for the company

--Modified BY: Bharat Raturi
--Date: 01/04/14
--Purpose: Get the list of strategies for the speciofied company
--Usage: P_GetAllCompStrategies 5
-----------------------------------------------------------------
CREATE procedure [dbo].[P_GetAllCompStrategies]
@companyID int
as
select CompanyStrategyID,StrategyName,StrategyShortName,CompanyID
from T_CompanyStrategy where CompanyID=@companyID and IsActive=1
