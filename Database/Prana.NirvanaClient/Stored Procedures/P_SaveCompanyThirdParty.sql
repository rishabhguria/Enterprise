


/****** Object:  Stored Procedure dbo.P_SaveCompanyThirdParty    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_SaveCompanyThirdParty
	(	
		@companyID int,
		@thirdPartyID int
	)
AS

Declare @result int
Declare @companyThirdPartyID int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyThirdParty
Where CompanyID = @companyID AND ThirdPartyID = @thirdPartyID

if(@total > 0)
begin	
	--Update CompanyThirdParty
	Update T_CompanyThirdParty 
	Set CompanyID = @companyID, 
		ThirdPartyID = @thirdPartyID
		
	Where CompanyID = @companyID AND ThirdPartyID = @thirdPartyID
	Select @companyThirdPartyID = CompanyThirdPartyID  From T_CompanyThirdParty Where CompanyID = @companyID AND ThirdPartyID = @thirdPartyID
	Set @result = @companyThirdPartyID
end
else
--Insert CompanyThirdParty
begin
	INSERT T_CompanyThirdParty(CompanyID, ThirdPartyID)
	Values(@companyID, @thirdPartyID)
	
	Set @result = scope_identity()
		--	end
end
select @result
	
	
	
	
	
	
	
	/*Insert T_CompanyThirdParty(companyID, ThirdPartyID)
	Values(@companyID, @thirdPartyID)*/



