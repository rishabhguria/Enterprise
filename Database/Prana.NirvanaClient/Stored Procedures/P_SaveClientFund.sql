


/****** Object:  Stored Procedure dbo.P_SaveClientFund    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_SaveClientFund
(
		@companyClientID int,
		@fundName varchar(50),		
		@fundShortName varchar(50),
		@ID int
)
AS
Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyClientFund
Where CompanyClientFundID = @ID
	
	if(@total > 0)
	begin	
		Update T_CompanyClientFund 
		Set CompanyClientFundName = @fundName, 
			CompanyClientFundShortName = @fundShortName, 
			CompanyClientID = @companyClientID
		Where CompanyClientFundID = @ID
		
		Set @result = @ID
	end
	else
	begin
		INSERT INTO T_CompanyClientFund( CompanyClientFundName, CompanyClientFundShortName, CompanyClientID)
		VALUES( @fundName, @fundShortName, @companyClientID)
		
		SET @result = scope_identity()
	End
	
	Select @result


