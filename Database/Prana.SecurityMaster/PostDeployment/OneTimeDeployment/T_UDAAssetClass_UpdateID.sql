select * into #UDAAssetClass from T_UDAAssetClass where AssetName <> 'Undefined'

ALTER TABLE #UDAAssetClass
ADD NewAssetID INT IDENTITY
CONSTRAINT PK_AssetID PRIMARY KEY CLUSTERED

--select * from #UDAAssetClass

update uda
set uda.AssetID = temp.NewAssetID
from T_UDAAssetClass uda
inner join #UDAAssetClass temp
on uda.AssetID = temp.AssetID

update sm
set sm.UDAAssetClassID = temp.NewAssetID
from T_SMSymbolLookUpTable sm
inner join #UDAAssetClass temp
on sm.UDAAssetClassID = temp.AssetID

drop table #UDAAssetClass