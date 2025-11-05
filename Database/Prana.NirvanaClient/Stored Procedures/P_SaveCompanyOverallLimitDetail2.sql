

CREATE PROCEDURE dbo.P_SaveCompanyOverallLimitDetail2
	(
		--@rMCompanyOverallLimitID int,
		@companyID int,
		@rMBaseCurrencyID int,
		@calculateRiskLimit int,
		@exposureLimit int,
		@positivePNL int,
		@negativePNL int
		--@result int
			
	)
AS 
Declare @result int
Declare @total int

	set @total = 0
	select @total = count(*)
	from T_RMCompanyOverallLimit 
	Where CompanyID = @companyID
	--RMCompanyOverallLimitID = @rMCompanyOverallLimitID
		--And CompanyID = @companyID 
		
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
			
	Where CompanyID = @companyID --and RMCompanyOverallLimitID = @rMCompanyOverallLimitID
	
	select @result = @total from T_RMCompanyOverallLimit where  CompanyID = @companyID --and RMCompanyOverallLimitID = @rMCompanyOverallLimitID

end
else
begin
		INSERT  into T_RMCompanyOverallLimit ( CompanyID, RMBaseCurrencyID, CalculateRiskLimit, ExposureLimit, PositivePNL, NegativePNL)
		Values( @companyID, @rMBaseCurrencyID, @calculateRiskLimit, @exposureLimit, @positivePNL,	@negativePNL)  
			
		Set @result = scope_identity()
end
select @result

