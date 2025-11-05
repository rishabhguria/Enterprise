CREATE PROCEDURE [dbo].[P_DeleteAssetSides] (@assetID INT)
AS
DELETE
FROM T_AssetSide
WHERE AssetID = @assetID