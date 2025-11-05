

CREATE PROCEDURE dbo.P_SaveCommissionRuleClearingFee
	(
		@ruleID int,
		@CalculationId int,	
		@currencyID int, 	
		@commissionRate decimal(18,4)		
	)
AS
	Declare @result int
	
	Declare @total int
	set @total = 0
	
	Select @total = Count(*)
	From T_CommissionRuleClearingFee
	Where RuleID =@ruleID
	
	if(@total = 0)
	Begin
	
			--Insert Data
			INSERT INTO  T_CommissionRuleClearingFee
               (RuleId,
				CalculationId,
				CurrencyId,
				CommissionRate)
			Values
				(@ruleId,
				@calculationId,
				@currencyId,
				@commissionrate)
				
				Set @result = scope_identity()
	end

	else 

	Begin
		
		
			--Update Table
			Update T_CommissionRuleClearingFee
			Set 
				CalculationId= @calculationId,
				CurrencyId = @currencyId,
				CommissionRate = @commissionRate				
			Where RuleID = @ruleID
			 Select @result =@ruleID
  end			

select @result	

