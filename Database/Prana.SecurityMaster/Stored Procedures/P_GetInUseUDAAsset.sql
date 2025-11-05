CREATE PROC [dbo].[P_GetInUseUDAAsset]  
as  
select distinct smLookupTable.UDAAssetClassID, udaAsset.AssetName from T_SMSymbolLookUpTable smLookupTable
inner join T_UDAAssetClass udaAsset on smLookupTable.UDAAssetClassID = udaAsset.AssetID
order by udaAsset.AssetName