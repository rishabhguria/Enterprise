


/****** Object:  Stored Procedure dbo.P_GetUnderLyingsByID    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetUnderLyingsByID
	(
		@ID int	
	)
AS
	Select UnderLyingID, UnderLyingName, AssetID, Comment
	From T_UnderLying
	Where UnderLyingID = @ID
	Order By AssetID, UnderLyingName



