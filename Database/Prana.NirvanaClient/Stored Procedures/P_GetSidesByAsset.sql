CREATE PROCEDURE [dbo].[P_GetSidesByAsset] (@assetID INT)
AS
SELECT T_AssetSide.SideID
	,T_Side.Side
	,T_Side.SideTagValue
FROM T_AssetSide
INNER JOIN T_Side
	ON T_AssetSide.SideID = T_Side.SideID
WHERE T_AssetSide.AssetID = @assetID
