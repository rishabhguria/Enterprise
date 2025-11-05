CREATE  PROCEDURE [dbo].[P_UDAGetAllAssets] AS          
 Select  AssetName ,AssetID          
 From T_UDAAssetClass   
order by AssetName
