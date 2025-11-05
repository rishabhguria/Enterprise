


/****** Object:  Stored Procedure dbo.P_SaveCompanyAUECs    Script Date: 01/05/2006 3:15:23 PM ******/
CREATE PROCEDURE dbo.P_SaveCompanyAUECs
	(
		@companyAUECID int,
		@companyID int,
		@auecID int
	)
AS

	/*Insert T_CompanyAUEC(CompanyID, AUECID)
	Values(@companyID, @auecID)*/
	
	
Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyAUEC
Where CompanyID = @companyID AND AUECID = @auecID

if(@total > 0)
begin	
	--Update T_CompanyAUEC
	Update T_CompanyAUEC 
	Set CompanyID = @companyID, 
		AUECID = @auecID
		
	Where CompanyAUECID = @companyAUECID
	
	Select @result = CompanyAUECID From T_CompanyAUEC Where CompanyID = @companyID AND AUECID = @auecID
end
else
--Insert T_CompanyAUEC
begin
	INSERT T_CompanyAUEC(CompanyID, AUECID)
	Values(@companyID, @auecID)
	
	Set @result = scope_identity()
		--	end
end
select @result
	
	


