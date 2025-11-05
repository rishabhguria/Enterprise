


/****** Object:  Stored Procedure dbo.P_SaveCVAUECCompliance    Script Date: 12/28/2005 8:00:22 PM ******/
CREATE PROCEDURE dbo.P_SaveCVAUECCompliance
(
		@cVAUECID int,
		@followCompliance int,
		@shortSellConfirmation int,
		@identifierID int,
		@foreignID varchar(20),
		@result int 
	)
AS 
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CVAUECCompliance 
Where CVAUECID = @cVAUECID

if(@total > 0)
begin	
	
	--Update CVAUEC
	Update T_CVAUECCompliance 
	Set FollowCompliance = @followCompliance, 
		ShortSellConfirmation = @shortSellConfirmation, 
		IdentifierID = @identifierID,
		ForeignID = @foreignID
			
	Where CVAUECID = @cVAUECID 
				
		
	Set @result = @cVAUECID 
	
end
else
--Insert CVAUEC
begin
				INSERT T_CVAUECCompliance(CVAUECID, FollowCompliance, ShortSellConfirmation, IdentifierID, ForeignID)
				Values(@cVAUECID, @followCompliance, @shortSellConfirmation, @identifierID, @foreignID)  
					
				Set @result = scope_identity()
end
select @result


