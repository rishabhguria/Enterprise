


/****** Object:  Stored Procedure dbo.P_SaveCounterPartyVenueDialog    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveCounterPartyVenueDialog
(
		@counterPartyVenueID int,
		@counterPartyID int,
		@venueID int,
		@result int 
)
AS 


if(@counterPartyVenueID > 0)
begin	
	Update T_CounterPartyVenue 
	Set CounterPartyID = @counterPartyID, 
		VenueID = @venueID
		
			
		Where CounterPartyVenueID = @counterPartyVenueID
				
		
		Set @result = @counterPartyVenueID
	end
	else
	begin
		declare @total int
		set @total = 0
		select @total = count(*)
		from T_CounterPartyVenue
		Where CounterPartyID = @counterPartyID And VenueID = @venueID	
		
		if(@total > 0)
		begin
			Set @result	= -2
		end
		else
		begin
		INSERT T_CounterPartyVenue(CounterPartyID, VenueID)
		Values(@counterPartyID, @venueID)  
			
		Set @result = scope_identity()
	end
end
select @result


