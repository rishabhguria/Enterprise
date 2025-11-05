
/****** Object:  Stored Procedure dbo.P_SaveFixCapability    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveFixCapability
(
		@fixCapabilityID int,
		@description varchar(50),
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_FixCapability
Where FixCapabilityID = @fixCapabilityID

if(@total > 0)
begin	
	select @count = count(*)
		from T_FixCapability
		Where Description = @description AND FixCapabilityID <> @fixCapabilityID
		if(@count = 0)
		begin
			Update T_FixCapability 
			Set Description = @description	
			Where FixCapabilityID = @fixCapabilityID
			Set @result = @fixCapabilityID
		end
		else
		begin
			Set @result = -1
		end
end
else
begin
	select @count = count(*)
	from T_FixCapability 
	Where Description = @description
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_FixCapability(Description)
		Values(@description)
	        
			Set @result = scope_identity()
	end
end
select @result
