



/****** Object:  Stored Procedure dbo.P_SaveCompanyCompliance    Script Date: 03/01/2006 12:28:22 PM ******/
CREATE PROCEDURE [dbo].[P_SaveCompanyCompliance]
(
		@companyComplianceID int,
		@fixVersionID int,
		@fixCapabilityID int,
		@companyID int,
		@result int 
	)
AS 
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyCompliance 
Where CompanyID = @companyID

if(@total > 0)
begin	
	
	--Update T_CompanyCompliance
	Update T_CompanyCompliance 
	Set	FixVersionID = @fixVersionID,
		FixCapabilityID = @fixCapabilityID,
		CompanyID = @companyID
			
	Where CompanyID = @companyID 
				
		
	Select @result = CompanyComplianceID from T_CompanyCompliance Where CompanyID = @companyID 
	
end
else
--Insert T_CompanyCompliance
begin
				INSERT T_CompanyCompliance(FixVersionID, FixCapabilityID, CompanyID)
				Values(@fixVersionID, @fixCapabilityID, @companyID)  
					
				Set @result = scope_identity()
end
select @result
