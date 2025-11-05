
/****** Object:  Stored Procedure dbo.P_SaveVenueType    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveVenueType
(
		@venueTypeID int,
		@venueType varchar(50),
		
		@result	int
)
AS 

Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_VenuType
Where VenueTypeID = @venueTypeID

if(@total > 0)
begin	
	select @count = count(*)
		from T_VenuType
		Where VenuType = @venueType AND VenueTypeID <> @venueTypeID
		if(@count = 0)
		begin	
			Update T_VenuType 
			Set VenuType = @venueType
			Where VenueTypeID = @venueTypeID
			Set @result = @venueTypeID
		end
end
else
begin
	select @count = count(*)
	from T_VenuType 
	Where VenuType = @venueType
	
	if(@count > 0)
	begin
		Set @result = -1
	end
	else
	begin
		INSERT T_VenuType(VenuType)
		Values(@venueType)
		Set @result = scope_identity()
	end
end
select @result
