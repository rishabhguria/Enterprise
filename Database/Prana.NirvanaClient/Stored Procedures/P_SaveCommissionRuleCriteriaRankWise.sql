

/****** Object:  Stored Procedure dbo.P_SaveCommissionRuleCriteriaRankWise"   Script Date: 3/16/2006 3:45:24 PM ******/
CREATE PROCEDURE dbo.P_SaveCommissionRuleCriteriaRankWise
	(
		@commRulecriteriaID int,
		@commCriteriaID int,
		@operatorID int, 
		@value int,
		@commissionRateID int,
		@commissionRate float,
		
		@rankID	int	
	)
AS
	Declare @result int
	Declare @total int
set @total = 0
	
	Select @total = Count(*)
	From T_CommissionRuleCriteria
	where CommissionCriteriaID_FK = @commCriteriaID and RankID = @rankID 
	
	if(@total = 0)
	Begin
		--Insert Data
		INSERT INTO  T_CommissionRuleCriteria(CommissionCriteriaID_FK, OperatorID_FK, Value, CommissionRateID_FK, CommisionRate, RankID)
		Values(@commCriteriaID, @operatorID, @value, @commissionRateID, @commissionRate, @rankID )
			
			Set @result = scope_identity()
	end

	else 
	Begin
		--Update Table
		Update T_CommissionRuleCriteria
		Set CommissionCriteriaID_FK = @commCriteriaID,
			OperatorID_FK = @operatorID,
			Value = @value,
			CommissionRateID_FK = @commissionRateID,
			CommisionRate = @commissionRate
			Where CommissionCriteriaID_FK = @commCriteriaID AND RankID = @rankID
			
			Select @result = CommissionRuleCriteriaID FROM T_CommissionRuleCriteria WHERE CommissionCriteriaID_FK = @commCriteriaID AND RankID = @rankID
	end

		 select @result	
		 

