

/****** Object:  Stored Procedure dbo.P_SaveCommissionRuleCriteria"   Script Date: 2/21/2006 6:30:24 PM ******/
CREATE PROCEDURE dbo.P_SaveCommissionRuleCriteria
	(
		@commRulecriteriaID int,
		@commCriteriaID int,
		@operatorID1 int, 
		@value1 int,
		@commissionRateID1 int,
		@commissionRate1 float,
		@operatorID2 int, 
		@value2 int,
		@commissionRateID2 int,
		@commissionRate2 float,
		@operatorID3 int, 
		@value3 int,
		@commissionRateID3 int,
		@commissionRate3 float
				
	)
AS
	Declare @result int
	set @result = 1
	
	exec P_SaveCommissionRuleCriteriaRankWise @commRulecriteriaID, @commCriteriaID, @operatorID1, @value1, @commissionRateID1, @commissionRate1, 1
	
	exec P_SaveCommissionRuleCriteriaRankWise @commRulecriteriaID, @commCriteriaID, @operatorID2, @value2, @commissionRateID2, @commissionRate2, 2
	
	exec P_SaveCommissionRuleCriteriaRankWise @commRulecriteriaID, @commCriteriaID, @operatorID3, @value3, @commissionRateID3, @commissionRate3, 3
	
	/*Declare @total int
	set @total = 0
	
	Select @total = Count(*)
	From T_CommissionRuleCriteria
	Where CommissionCriteriaID_FK = @commCriteriaID 
	
	if(@total = 0)
	Begin
		--Insert Data
		INSERT INTO  T_CommissionRuleCriteria(CommissionCriteriaID_FK,OperatorID_FK,Value,CommissionRateID_FK,CommisionRate)
		Values(@commCriteriaID,@operatorID,@value,@commissionRateID,@commissionRate )
			
			Set @result = scope_identity()
	end
	else 
	Begin
		--Update Table
		Update T_CommissionRuleCriteria
		Set CommissionCriteriaID_FK=@commCriteriaID,
			OperatorID_FK=@operatorID,
			Value=@value,
			CommissionRateID_FK=@commissionRateID,
			CommisionRate=@commissionRate
			Where CommissionCriteriaID_FK = @commCriteriaID
		 Select @result = CommissionRuleCriteriaID FROM T_CommissionRuleCriteria WHERE CommissionCriteriaID_FK = @commCriteriaID
		 
	end*/
select @result	


