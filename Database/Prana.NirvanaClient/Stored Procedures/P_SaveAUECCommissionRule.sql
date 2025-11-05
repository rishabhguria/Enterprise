CREATE PROCEDURE dbo.P_SaveAUECCommissionRule
	(
		@ruleID int,
		@auecID int,
		@rulename varchar(50),
		@applyruleID int, 
		@ruledescription varchar(50),
		@calculationID int, 
		@currencyID int, 
		@commissionrateID int,
		@commission float,
		@applycriteria int,
        @ApplyClrFee int
		
	)
AS
	Declare @result int
	
	Declare @total int
	set @total = 0
	declare @count int
	set @count = 0
	
	Select @total = Count(*)
	From T_AUECCommissionRules
	Where RuleID =@ruleID
	
	if(@total = 0)
	Begin
		select @count = count(*)
		from T_AUECCommissionRules
		Where RuleName = @rulename
		
		if(@count > 0)
		begin
			
			Set @result = -1
		end
		else
		begin
			--Insert Data
			INSERT INTO  T_AUECCommissionRules(AUECID_FK,RuleName,ApplyRuletoID_FK,RuleDescription,CalculationID_FK,CurrencyID_FK,
			CommissionRateID_FK,Commission,ApplyCriteria,ApplyClrFee)
			Values(@auecID,@rulename,@applyruleID,@ruledescription,@calculationID,@currencyID,@commissionrateID,
			@commission,@applycriteria,@ApplyClrFee)
				
				Set @result = scope_identity()
		end
	end
	else 
	Begin
		select @count = count(*)
		from T_AUECCommissionRules
		Where RuleName = @rulename AND RuleID <> @ruleID
		if(@count = 0)
		begin
			--Update Table
			Update T_AUECCommissionRules
			Set AUECID_FK = @auecID,
				RuleName = @rulename,
				ApplyRuletoID_FK = @applyruleID,
				RuleDescription = @ruledescription,
				CalculationID_FK = @calculationID,
				CurrencyID_FK = @currencyID,
				CommissionRateID_FK = @commissionrateID,
				Commission = @commission,
				ApplyCriteria = @applycriteria,
				ApplyClrFee = @ApplyClrFee
			Where RuleID = @ruleID
			 Select @result =@ruleID
		 end
			else
			begin
				Set @result = -1
			end
	end
select @result	

