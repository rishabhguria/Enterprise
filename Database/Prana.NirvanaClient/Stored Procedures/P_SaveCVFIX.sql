


/****** Object:  Stored Procedure dbo.P_SaveCVFIX    Script Date: 12/29/2005 12:00:22 PM ******/
CREATE PROCEDURE dbo.P_SaveCVFIX
(
		@counterPartyVenueID int,
		@acronymn varchar(50),
		@fixVersionID int,
		@targetCompID varchar(50),
		@deliverToCompID varchar(50),
		@deliverToSubID varchar(50),
		@result int 
	)
AS 
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CVFIX 
Where CounterPartyVenueID = @counterPartyVenueID

if(@total > 0)
begin	
	
	--Update CVAUEC
	Update T_CVFIX 
	Set CounterPartyVenueID = @counterPartyVenueID, 
		Acronymn = @acronymn, 
		FixVersionID = @fixVersionID,
		TargetCompID = @targetCompID,
		DeliverToCompID = @deliverToCompID,
		DeliverToSubID = @deliverToSubID
			
	Where CounterPartyVenueID = @counterPartyVenueID 
				
		
	Set @result = @counterPartyVenueID 
	
end
else
--Insert CVAUEC
begin
				INSERT T_CVFIX(CounterPartyVenueID, Acronymn, FixVersionID, TargetCompID,
						DeliverToCompID, DeliverToSubID)
				Values(@counterPartyVenueID, @acronymn, @fixVersionID, @targetCompID, 
						@deliverToCompID, @deliverToSubID)  
					
				Set @result = scope_identity()
end
select @result
 


