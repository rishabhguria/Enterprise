
/****** Object:  Stored Procedure dbo.P_SaveUnit    Script Date: 11/17/2005 9:50:20 AM ******/
CREATE PROCEDURE dbo.P_SaveUnit
(
		@unitID int,
		@unitName varchar(50),
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_Units
Where UnitID = @unitID

if(@total > 0)
begin	
	select @count = count(*)
		from T_Units
		Where UnitName = @unitName AND UnitID <> @unitID
		if(@count = 0)
		begin	
			Update T_Units 
			Set UnitName = @unitName	
			Where UnitID = @unitID
			Set @result = @unitID
		end
		else
		begin
			Set @result = -1
		end
end
else
begin
	select @count = count(*)
	from T_Units 
	Where UnitName = @unitName
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_Units(UnitName)
		Values(@unitName)
			Set @result = scope_identity()
	end
end
select @result
