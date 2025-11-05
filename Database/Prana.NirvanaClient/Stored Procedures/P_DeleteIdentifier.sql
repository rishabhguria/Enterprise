
/****** Object:  Stored Procedure dbo.P_DeleteIdentifier    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_DeleteIdentifier
	(
		@identifierID int
	)
AS

--Delete Corresponding Identifier from the tables referring it.
		Declare @total int

		Select @total = Count(1) 
		From T_AUEC Where IdentifierID = @identifierID
			
		if ( @total = 0)
		begin
		
			Select @total = Count(1) 
			FROM T_CVAUECCompliance Where IdentifierID = @identifierID

			if ( @total = 0)
			begin 
			
			Select @total = Count(1) 
			FROM T_CompanyClientIdentifier Where IdentifierID = @identifierID

				if ( @total = 0)
				begin 
					-- If IdentifierID is not referenced anywhere.
					--Delete Identifier.
			
										
				Delete T_Identifier
				Where IdentifierID = @identifierID
				end
			end
		end
	


