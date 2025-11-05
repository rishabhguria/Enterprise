CREATE PROCEDURE [dbo].[P_GetAssetSides] (@assetID INT)
AS
SELECT AssetID
	,SideID
FROM T_AssetSide
WHERE AssetID = @assetID