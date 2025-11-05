


/****** Object:  Stored Procedure dbo.P_GetAllAUECAssets    Script Date: 03/23/2005 02:45:21 PM ******/
CREATE PROCEDURE dbo.P_GetAllAUECAssets
(
	@assetID int
)
AS
	declare @Total int
	Set @Total = 0
	
	Select @Total = Count(*) from T_AUEC Where AssetID = @assetID
	
	if(@Total > 0)
	begin
		Select distinct A.AssetID, AssetName, '' --'' for hard coded as no need to pick up comment from here.
		From T_AUEC AUEC left outer join T_Asset A on AUEC.AssetID = A.AssetID
	end
	else
	begin
		
		Select distinct A.AssetID, AssetName, '' --'' for hard coded as no need to pick up comment from here.
		From T_Asset A inner join T_AUEC AUEC on A.AssetID = AUEC.AssetID
			Union (Select distinct AssetID, AssetName, '' From T_Asset Where AssetID = @assetID)
	end