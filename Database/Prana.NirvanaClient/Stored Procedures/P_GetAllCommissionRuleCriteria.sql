

/****** Object:  Stored Procedure dbo.P_GetAllCommissionRuleCriteria    Script Date: 2/28/2006 3:45:23 AM ******/
CREATE PROCEDURE dbo.P_GetAllCommissionRuleCriteria
AS
	SELECT  CommissionRuleCriteriaID,CommissionCriteriaID_FK,OperatorID_FK,Value,CommissionRateID_FK,CommisionRate
FROM         T_CommissionRuleCriteria order by CommissionRuleCriteriaID


