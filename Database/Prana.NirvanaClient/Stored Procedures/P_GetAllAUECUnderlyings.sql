


/****** Object:  Stored Procedure dbo.P_GetAllAUECUnderlyings    Script Date: 03/23/2005 02:50:21 PM ******/
CREATE PROCEDURE [dbo].[P_GetAllAUECUnderlyings]
(
	@assetID int,
	@underLyingID int
)
AS
	declare @Total int
	Set @Total = 0
	
	Select @Total = Count(*) from T_AUEC Where UnderLyingID = @underLyingID
	
	if(@Total > 0)
	begin
		Select distinct U.UnderLyingID, UnderLyingName, U.AssetID, '' --'' for hard coded as no need to pick up comment from here.
		From T_AUEC AUEC inner join T_Underlying U on AUEC.UnderlyingID = U.UnderlyingID Where AUEC.AssetID = @assetID Union (Select UnderLyingID, UnderLyingName, AssetID, '' From T_UnderLying Where UnderlyingID = @underLyingID)
	end
	else
	begin
				
		Select distinct U.UnderLyingID, UnderLyingName, U.AssetID, '' --'' for hard coded as no need to pick up comment from here.
		From T_UnderLying U inner join T_AUEC AUEC on U.UnderlyingID = AUEC.UnderlyingID AND AUEC.AssetID = @assetID Union (Select distinct UnderLyingID, UnderLyingName, AssetID, '' From T_UnderLying Where UnderlyingID = @underLyingID)
	end
