

CREATE PROCEDURE dbo.P_SaveCompanyOverallLimitDetail
	(
		
		@companyID int,
		@rMBaseCurrencyID int,
		@calculateRiskLimit int,
		@exposureLimit int,
		@positivePNL int,
		@negativePNL int
	
			
	)
AS 
Declare @result int
Declare @total int

	set @total = 0
	select @total = count(*)
	from T_RMCompanyOverallLimit 
	Where CompanyID = @companyID
	
		
		if(@total > 0)

begin	
	Update T_RMCompanyOverallLimit 
	Set  
		CompanyID = @companyID, 
		RMBaseCurrencyID = @rMBaseCurrencyID, 
		CalculateRiskLimit = @calculateRiskLimit, 
		ExposureLimit = @exposureLimit, 
		PositivePNL = @positivePNL, 
		NegativePNL = @negativePNL
			
	Where CompanyID = @companyID 
	
	set @result = @total --from T_RMCompanyOverallLimit where  CompanyID = @companyID 

end
else
begin
		INSERT  into T_RMCompanyOverallLimit ( CompanyID, RMBaseCurrencyID, CalculateRiskLimit, ExposureLimit, PositivePNL, NegativePNL)
		Values( @companyID, @rMBaseCurrencyID, @calculateRiskLimit, @exposureLimit, @positivePNL,	@negativePNL)  
			
		Set @result = scope_identity()
end
select @result

