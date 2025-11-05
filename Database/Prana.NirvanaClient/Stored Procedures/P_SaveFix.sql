
/****** Object:  Stored Procedure dbo.P_SaveFix    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveFix
(
		@fixID int,
		@fixVersion varchar(50),
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_Fix
Where FixID = @fixID

if(@total > 0)
begin	
	select @count = count(*)
		from T_Fix
		Where FixVersion = @fixVersion AND FixID <> @fixID
		if(@count = 0)
		begin
			Update T_Fix 
			Set FixVersion = @fixVersion	
			Where FixID = @fixID
			Set @result = @fixID
		end
		else
		begin
			Set @result = -1
		end
end
else
begin
	select @count = count(*)
	from T_Fix 
	Where FixVersion = @fixVersion
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_Fix(FixVersion)
		Values(@fixVersion)
		Set @result = scope_identity()
	end
end
select @result
