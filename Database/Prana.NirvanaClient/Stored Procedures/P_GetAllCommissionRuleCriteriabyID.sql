/****** Object:  Stored Procedure dbo.P_GetAllCommissionRuleCriteriabyID    Script Date: 3/2/2006 3:05:23 PM ******/
CREATE PROCEDURE dbo.P_GetAllCommissionRuleCriteriabyID
(
   @commCriteriaID int
)
AS
	/* SELECT  CommissionRuleCriteriaID,CommissionCriteriaID_FK,OperatorID_FK,Value,CommissionRateID_FK,CommisionRate
FROM         T_CommissionRuleCriteria 
where CommissionCriteriaID_FK = @commCriteriaID */

Declare @OperatorID_FK1 int, @Value1 int, @CommissionRateID_FK1 int, @CommisionRate1 float
Declare @OperatorID_FK2 int, @Value2 int, @CommissionRateID_FK2 int, @CommisionRate2 float
Declare @OperatorID_FK3 int, @Value3 int, @CommissionRateID_FK3 int, @CommisionRate3 float
Declare @CommissionRuleCriteriaID int, @CommissionCriteriaID_FK int

SELECT @OperatorID_FK1 = OperatorID_FK, @Value1 = Value, @CommissionRateID_FK1 = CommissionRateID_FK, 
       @CommisionRate1 =  CommisionRate FROM T_CommissionRuleCriteria WHERE RankID = 1 AND CommissionCriteriaID_FK = @commCriteriaID
       
SELECT @OperatorID_FK2 = OperatorID_FK, @Value2 = Value, @CommissionRateID_FK2 = CommissionRateID_FK, 
       @CommisionRate2 =  CommisionRate FROM T_CommissionRuleCriteria WHERE RankID = 2 AND CommissionCriteriaID_FK = @commCriteriaID
       
       
SELECT @CommissionRuleCriteriaID = CommissionRuleCriteriaID, @CommissionCriteriaID_FK = CommissionCriteriaID_FK, @OperatorID_FK3 = OperatorID_FK, @Value3 = Value, @CommissionRateID_FK3 = CommissionRateID_FK, 
       @CommisionRate3 =  CommisionRate FROM T_CommissionRuleCriteria WHERE RankID = 3 AND CommissionCriteriaID_FK = @commCriteriaID


Select @CommissionRuleCriteriaID, @CommissionCriteriaID_FK, @OperatorID_FK1, @Value1, @CommissionRateID_FK1, 
		@CommisionRate1, @OperatorID_FK2, @Value2, @CommissionRateID_FK2, @CommisionRate2, @OperatorID_FK3, 
		@Value3, @CommissionRateID_FK3, @CommisionRate3
