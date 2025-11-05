


CREATE PROCEDURE [dbo].[P_ValidateCompanyUserLogin]
	(
		@login		varchar(20),
		@password	varchar(20)		
	)
AS	
	Declare @Total int
	
	Set @Total = 0;
	
	Select @Total = Count(1)
	From T_CompanyUser
	Where
CAST(Login AS varbinary(20)) = CAST(@login AS varbinary(20))
		AND CAST(Password AS varbinary(20)) = CAST(@password AS varbinary(20))
		AND Login = @login
		AND Password = @password

-- above code on adding 
--
-- Login = @login
--		And Password = @password
		
	if(@Total > 0)
	begin
		Select UserID
		From T_CompanyUser
		Where Login = @login
			And Password = @password
	end 
	else
	begin
		Select @Total		
	end
