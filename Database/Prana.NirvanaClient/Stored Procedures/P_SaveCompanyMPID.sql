


/****** Object:  Stored Procedure dbo.P_SaveCompanyMPID    Script Date: 12/29/2005 5:50:22 PM ******/
CREATE PROCEDURE dbo.P_SaveCompanyMPID
(
		@companyID int,
		@MPID varchar(15),
		@companyMPID int
)
AS 
Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyMPID 
Where CompanyMPID = @companyMPID

if(@total > 0)
begin	
	
	--Update CompanyMPID
	Update T_CompanyMPID
	Set CompanyID = @companyID,
		MPID = @MPID
			
	Where CompanyMPID = @companyMPID 
				
	Select @result = CompanyMPID From T_CompanyMPID Where CompanyMPID = @companyMPID
	
end
else
--Insert CompanyMPID
begin
				INSERT T_CompanyMPID(CompanyID, MPID)
				Values(@companyID, @MPID)  
				
				Set @result = scope_identity()
end
select @result
 


