DECLARE @MaxId int
SELECT @MaxId = max(AssetID) + 1 from T_UDAAssetClass

If((select count(*) from T_UDAAssetClass where AssetName = 'Equity Call') = 0)
Begin
Insert into T_UDAAssetClass values ('Equity Call', @MaxId)
End

SELECT @MaxId = max(AssetID) + 1 from T_UDAAssetClass

If((select count(*) from T_UDAAssetClass where AssetName = 'Equity Put') = 0)
Begin
Insert into T_UDAAssetClass values ('Equity Put', @MaxId)
End

SELECT @MaxId = max(AssetID) + 1 from T_UDAAssetClass

If((select count(*) from T_UDAAssetClass where AssetName = 'Future Call') = 0)
Begin
Insert into T_UDAAssetClass values ('Future Call', @MaxId)
End

SELECT @MaxId = max(AssetID) + 1 from T_UDAAssetClass

If((select count(*) from T_UDAAssetClass where AssetName = 'Future Put') = 0)
Begin
Insert into T_UDAAssetClass values ('Future Put', @MaxId)
End

