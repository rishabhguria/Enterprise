


/****** Object:  Stored Procedure dbo.P_DeleteAsset    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_DeleteAsset
	(
		@assetID int
	)

AS
		Declare @total int

		Select @total = Count(1) 
		From T_AUEC
		Where assetID = @assetID
	
		if ( @total = 0)
		begin 
			Select @total = Count(1) 
			From T_Underlying
			Where assetID = @assetID

			if ( @total = 0)
			begin 		
				-- If Asset is not referenced anywhere.
				--Delete Asset.
						 
				Delete T_Asset 
				Where AssetID = @assetID
			end	
		end


