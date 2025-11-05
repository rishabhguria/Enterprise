
/****** Object:  Stored Procedure dbo.P_SaveCompanyBorrower    Script Date: 03/01/2006 12:45:22 PM ******/
CREATE PROCEDURE dbo.P_SaveCompanyBorrower
(
		@companyBorrowerID int,
		@borrowerName varchar(50),
		@borrowerShortName varchar(50),
		@borrowerFirmID varchar(50),
		@companyID int,
		@result int 
	)
AS 
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyBorrower 
Where CompanyBorrowerID = @companyBorrowerID

if(@total > 0)
begin	
	
	--Update T_CompanyBorrower
	Update T_CompanyBorrower 
	Set BorrowerName = @borrowerName, 
		BorrowerShortName = @borrowerShortName, 
		BorrowerFirmID = @borrowerFirmID,
		CompanyID = @companyID
			
	Where CompanyBorrowerID = @companyBorrowerID  
				
		
	Set @result = @companyBorrowerID 
	
end
else
--Insert T_CompanyBorrower
begin
				INSERT T_CompanyBorrower(BorrowerName, BorrowerShortName, BorrowerFirmID, CompanyID)
				Values(@borrowerName, @borrowerShortName, @borrowerFirmID, @companyID)  
					
				Set @result = scope_identity()
end
select @result
