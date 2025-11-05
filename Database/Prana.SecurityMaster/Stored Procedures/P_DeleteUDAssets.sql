CREATE Procedure [dbo].[P_DeleteUDAssets] 
(
@AssetID int
)

as

UPDATE T_SMSymbolLookUpTable
SET UDAAssetClassID = -2147483648
WHERE UDAAssetClassID = @AssetID

DELETE FROM T_UDAAssetClass 
WHERE AssetID = @assetID
