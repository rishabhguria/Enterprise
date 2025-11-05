CREATE PROCEDURE dbo.P_GetAllCommissionRuleCriteriabyIDUpdated
(
   @commCriteriaID int=0
)
AS	

SELECT CommissionRuleCriteriaID,
 CommissionCriteriaID_FK,
 ValueFrom, ValueTo, 
CommissionRateID_FK, 
CommisionRate FROM T_CommissionRuleCriteriaUpdated 
WHERE CommissionCriteriaID_FK=@commCriteriaID order by ValueFrom



