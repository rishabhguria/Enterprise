


/****** Object:  Stored Procedure dbo.P_SaveCompanyUserFunds    Script Date: 04/12/2006 3:15:23 PM ******/
CREATE PROCEDURE dbo.P_SaveCompanyUserFunds
	(
		@companyUserID int,
		@companyFundID int
	)
AS

Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyUserFunds
Where CompanyUserID = @companyUserID AND CompanyFundID = @companyFundID

if(@total > 0)
begin	
	--Update T_CompanyUserFunds
	Update T_CompanyUserFunds 
	Set CompanyUserID = @companyUserID, 
		CompanyFundID = @companyFundID
		
	Where CompanyUserID = @companyUserID AND CompanyFundID = @companyFundID
	
	Select @result = CompanyUserFundID From T_CompanyUserFunds Where CompanyUserID = @companyUserID AND CompanyFundID = @companyFundID
end
else
--Insert T_CompanyUserFunds
begin
	INSERT T_CompanyUserFunds(CompanyUserID, CompanyFundID)
	Values(@companyUserID, @companyFundID)
	
	Set @result = scope_identity()
		--	end
end
select @result
	
	


