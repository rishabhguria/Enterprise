-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 28/02/14
--Purpose: Get the list of master strategies for the company
-----------------------------------------------------------------
--Modified BY: Bharat Raturi
--Date: 01/04/14
--Purpose: Get the mapping of Strategy and master strategy for the company
--usage P_CompanyMasterStrategySubAccountAssociation 5
-----------------------------------------------------------------

create procedure [dbo].[P_CompanyMasterStrategySubAccountAssociation]
@CompanyID int
as
select t1.CompanyMasterStrategyID,t1.CompanyStrategyID
from T_CompanyMasterStrategySubAccountAssociation t1 inner JOIN 
T_CompanyMasterStrategy t2
on t1.CompanyMasterStrategyID=t2.CompanyMasterStrategyID
where 
t2.CompanyID=@CompanyID
