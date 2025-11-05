
/****** Object:  Stored Procedure dbo.P_SaveSide    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveSide
(
		@sideID int,
		@side varchar(50),
		@sideTagValue varchar(50),
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_Side
Where SideID = @sideID

if(@total > 0)
begin	
	select @count = count(*)
		from T_Side
		Where (Side = @side OR SideTagValue = @sideTagValue) AND SideID <> @sideID
		if(@count = 0)
		begin
			Update T_Side 
			Set Side = @side,
				SideTagValue = @sideTagValue	
			Where SideID = @sideID
			Set @result = @sideID
		end
		else
		begin
			Set @result = -1
		end
end
else
begin
	select @count = count(*)
	from T_Side 
	Where (Side = @side OR SideTagValue = @sideTagValue)
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_Side(Side, SideTagValue)
		Values(@side, @sideTagValue)
		Set @result = scope_identity()
	end
end
select @result
