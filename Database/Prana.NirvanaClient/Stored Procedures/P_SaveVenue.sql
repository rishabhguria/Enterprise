
/****** Object:  Stored Procedure dbo.P_SaveVenue    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveVenue
(
		@venueID int,
		@venueName varchar(50),
		@venueTypeID int,
		@route varchar(50),
		@exchangeID int,
		@result	int
)
AS 
declare @total int
set @total = 0
declare @count int
set @count = 0
Select @total = Count(*) From T_Venue Where VenueID = @venueID
if(@total > 0)
begin	
	select @count = count(*)
		from T_Venue
		Where (VenueName = @venueName OR Route = @route) AND VenueID <> @venueID
		if(@count = 0)
		begin
			Update T_Venue 
			Set VenueName = @venueName, 
				VenueTypeID = @venueTypeID, 
				Route = @route,
				ExchangeID = @exchangeID

			Where VenueID = @venueID
			Set @result = @venueID
		end
		else
			begin
				Set @result = -1
			end			
	
end
else
begin
	select @count = count(*)
		from T_Venue 
		Where VenueName = @venueName OR Route = @route
		
		if(@count > 0)
		begin
			
			Set @result = -1
		end
		else
		begin
			INSERT T_Venue(VenueName, VenueTypeID, Route, ExchangeID)
			Values(@venueName, @venueTypeID, @route, @exchangeID)
		        
				Set @result = scope_identity()
        end
end
select @result
