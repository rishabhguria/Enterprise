/*******************************************************************************
created by : omshiv
Date - 11 Jun 2014
Desc- increase the column length of UDA  

********************************************************************************/
create Procedure [dbo].[P_InsertUDAAsset] 
(
@AssetName varchar(200),
@AssetID int 
)
as


DELETE FROM T_UDAAssetClass where AssetID=@assetID

INSERT INTO T_UDAAssetClass(AssetName , AssetID)
    VALUES(@AssetName,@AssetID)
