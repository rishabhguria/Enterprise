
CREATE PROCEDURE [dbo].[P_DeleteCommRule] (		
		@RuleId uniqueIdentifier
	)
AS
Declare @Count int
Set @Count=0
Declare @result int
Set @result=0

BEGIN TRAN TRAN1       
      
BEGIN TRY 
-- check is there any reference in the table T_CommissionRulesForCVAUEC
Select @Count=Count(*) from T_CommissionRulesForCVAUEC
Where SingleRuleId_FK=@RuleId OR BasketRuleId_FK=@RuleId 


If @Count>0
Begin
set @result=-1
End

Else
Begin
-- delete Rule Asstes  from T_CommissionRuleAssets
Delete from T_CommissionRuleAssets where RuleId_FK=@RuleId
-- Delete Criteria from T_CommisionCriteria of the respective CommissionRule
Delete from T_CommissionCriteria Where RuleId_FK=@RuleId
-- delete from Table T_ClearingFee
delete from T_ClearingFee Where RuleId_FK=@RuleId
-- atlast Delete from Master Table T_CommissionRules
Delete from T_CommissionRules Where RuleId=@RuleId
set @result=1
End

Select @result

COMMIT TRANSACTION TRAN1      
      
END TRY      
BEGIN CATCH     
 SET @result = Error_number();     
 ROLLBACK TRANSACTION TRAN1         
END CATCH;
