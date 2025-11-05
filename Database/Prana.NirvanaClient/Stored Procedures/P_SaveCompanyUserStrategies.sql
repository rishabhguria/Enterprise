


/****** Object:  Stored Procedure dbo.P_SaveCompanyUserStrategies    Script Date: 04/12/2006 2:25:23 PM ******/
CREATE PROCEDURE dbo.P_SaveCompanyUserStrategies
	(
		@companyUserID int,
		@companyStrategyID int
	)
AS

Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyUserStrategies
Where CompanyUserID = @companyUserID AND CompanyStrategyID = @companyStrategyID

if(@total > 0)
begin	
	--Update T_CompanyUserStrategies
	Update T_CompanyUserStrategies 
	Set CompanyUserID = @companyUserID, 
		CompanyStrategyID = @companyStrategyID
		
	Where CompanyUserID = @companyUserID AND CompanyStrategyID = @companyStrategyID
	
	Select @result = CompanyUserStrategyID From T_CompanyUserStrategies Where CompanyUserID = @companyUserID AND CompanyStrategyID = @companyStrategyID
end
else
--Insert T_CompanyUserStrategies
begin
	INSERT T_CompanyUserStrategies(CompanyUserID, CompanyStrategyID)
	Values(@companyUserID, @companyStrategyID)
	
	Set @result = scope_identity()
		--	end
end
select @result
	
	


