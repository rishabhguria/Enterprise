CREATE PROCEDURE [dbo].[P_SaveVenueDetailsFor]
			 (	
				@VenueName varchar(50),
				@VenueTypeId int,		
				@Route varchar(50),	
			    @exchangeID int		       
	          )
AS
	Declare @total int
	set @total = 0 
	
	Select @total = Count(*)
	From T_Venue
	Where ExchangeID = @exchangeID
	
	if(@total > 0)
	      Begin		      
				   --Update Table
					Update T_Venue
					Set VenueName = @VenueName, 
						VenueTypeId = @VenueTypeId, 
						[Route] = @Route 					
					Where ExchangeID = @ExchangeID					
	       End		 
	 
	Else
	      Begin
		        --Insert Data
				INSERT INTO T_Venue(VenueName,VenueTypeId,[Route],ExchangeID)
			               Values(@VenueName, @VenueTypeId,@Route, @ExchangeID)			
				
		  End 




