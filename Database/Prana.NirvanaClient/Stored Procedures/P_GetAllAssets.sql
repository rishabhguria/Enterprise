


/****** Object:  Stored Procedure dbo.P_GetAllAssets    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_GetAllAssets
AS
	Select AssetID, AssetName, Comment
	From T_Asset



