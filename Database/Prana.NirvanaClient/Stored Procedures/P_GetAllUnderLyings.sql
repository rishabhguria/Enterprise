


/****** Object:  Stored Procedure dbo.P_GetAllUnderLyings    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_GetAllUnderLyings	
AS
	Select UnderLyingID, UnderLyingName, AssetID, Comment
	From T_UnderLying
	Order By AssetID, UnderLyingName
	


