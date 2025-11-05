


/****** Object:  Stored Procedure dbo.P_GetUnderLyingsByAssetID    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetUnderLyingsByAssetID
	(
		@ID int	
	)
AS
	Select UnderLyingID, UnderLyingName, AssetID, Comment
	From T_UnderLying
	Where AssetID = @ID
	Order By UnderLyingName



