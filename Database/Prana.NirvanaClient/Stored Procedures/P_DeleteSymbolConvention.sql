


/****** Object:  Stored Procedure dbo.P_DeleteUnit    Script Date: 11/30/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteSymbolConvention
	(
		@symbolConventionID int
	)
AS

--Delete Corresponding SymbolConvention from the tables referring it.
	
		Declare @total int

		Select @total = Count(1) 
		From T_CVSymbolConvention AS CVSC
			Where CVSC.SymbolConventionID = @symbolConventionID
			
				if ( @total = 0)
				begin 	
				
				Select @total = Count(1) 
				From T_AUEC AS AUEC
				Where AUEC.SymbolConventionID = @symbolConventionID
			
				if ( @total = 0)
				begin 		
				
					
					-- If SymbolConventionID is not referenced anywhere.
					
								
					Delete T_SymbolConvention
					Where SymbolConventionID = @symbolConventionID
				end
			end
			
	
	




