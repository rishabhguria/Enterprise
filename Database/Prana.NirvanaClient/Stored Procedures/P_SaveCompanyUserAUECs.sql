


/****** Object:  Stored Procedure dbo.P_SaveCompanyUserAUECs    Script Date: 01/11/2006 9:00:23 PM ******/
CREATE PROCEDURE dbo.P_SaveCompanyUserAUECs
	(
		@companyUserID int,
		@companyAUECID int
	)
AS

Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyUserAUEC
Where CompanyUserID = @companyUserID AND CompanyAUECID = @companyAUECID

if(@total > 0)
begin	
	--Update T_CompanyUserAUEC
	Update T_CompanyUserAUEC 
	Set CompanyUserID = @companyUserID, 
		CompanyAUECID = @companyAUECID
		
	Where CompanyUserID = @companyUserID AND CompanyAUECID = @companyAUECID
	
	Select @result = CompanyUserAUECID From T_CompanyUserAUEC Where CompanyUserID = @companyUserID AND CompanyAUECID = @companyAUECID
end
else
--Insert T_CompanyUserAUEC
begin
	INSERT T_CompanyUserAUEC(CompanyUserID, CompanyAUECID)
	Values(@companyUserID, @companyAUECID)
	
	Set @result = scope_identity()
		--	end
end
select @result
	
	


