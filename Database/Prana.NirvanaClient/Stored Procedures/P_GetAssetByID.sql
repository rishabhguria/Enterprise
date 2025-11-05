


/****** Object:  Stored Procedure dbo.P_GetAssetByID    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAssetByID
	(
		@assetID int
	)
AS
	Select AssetID, AssetName, Comment
	From T_Asset
	Where AssetID = @assetID



