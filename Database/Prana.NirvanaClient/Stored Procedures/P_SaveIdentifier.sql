
/****** Object:  Stored Procedure dbo.P_SaveIdentifier    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveIdentifier
(
		@identifierID int,
		@identifierName varchar(50),
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_Identifier
Where IdentifierID = @identifierID

if(@total > 0)
begin	
	select @count = count(*)
		from T_Identifier
		Where IdentifierName = @identifierName AND IdentifierID <> @identifierID
		if(@count = 0)
		begin
			Update T_Identifier 
			Set IdentifierName = @identifierName	

			Where IdentifierID = @identifierID
			Set @result = @identifierID
		end
		else
		begin
			Set @result = -1
		end
end
else
begin
	select @count = count(*)
	from T_Identifier 
	Where IdentifierName = @identifierName
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_Identifier(IdentifierName)
		Values(@identifierName)
	        
			Set @result = scope_identity()
	end
end
select @result
