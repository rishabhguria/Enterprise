
/****** Object:  Stored Procedure dbo.P_SaveCounterPartyVenue    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveCounterPartyVenue
(
		@counterPartyVenueID int,
		@displayName varchar(50),
		@isElectronic int,
		@oatsIdentifier varchar(20),
		@symbolConventionID int,
		@currencyID int,
		@counterPartyID int,
		@venueID int,
		@result int 
	)
AS 
Declare @total int 
Set @total = 0
declare @count int
set @count = 0

Select @total = Count(*)
From T_CounterPartyVenue 
Where CounterPartyVenueID = @counterPartyVenueID

if(@total > 0)
begin	
	select @count = count(*)
		from T_CounterPartyVenue
		Where DisplayName = @displayName AND CounterPartyVenueID <> @counterPartyVenueID
		if(@count = 0)
		begin
			--Update CounterPartyVenue
			Update T_CounterPartyVenue 
			Set DisplayName = @displayName, 
				IsElectronic = @isElectronic, 
				OatsIdentifier = @oatsIdentifier,
				SymbolConventionID = @symbolConventionID,
				CurrencyID = @currencyID
					
			Where CounterPartyVenueID = @counterPartyVenueID 
						
			Set @result = @counterPartyVenueID
		end
		else
		begin
			Set @result = -1
		end
end
else
begin
		select @count = count(*)
		from T_CounterPartyVenue 
		Where DisplayName = @displayName
		
		if(@count > 0)
		begin
			
			Set @result = -1
		end
		else
		begin
			--Insert CounterPartyVenue
			INSERT T_CounterPartyVenue(CounterPartyID, VenueID, DisplayName, IsElectronic, OatsIdentifier, 
					SymbolConventionID, CurrencyID)
			Values(@counterPartyID, @venueID, @displayName, @isElectronic, @oatsIdentifier, @symbolConventionID, @currencyID)  
				
			Set @result = scope_identity()
		end
end 
select @result
