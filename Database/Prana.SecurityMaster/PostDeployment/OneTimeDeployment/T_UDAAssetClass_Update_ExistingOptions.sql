/*
One time datascript, to update the UDA Asset class of already existing options in SM database.
*/

select * into #UDAAssetClass from T_UDAAssetClass 
where AssetName in ('Equity Call','Equity Put','Future Call','Future Put')

ALTER TABLE #UDAAssetClass
ADD AssetClassID int

ALTER TABLE #UDAAssetClass
ADD Type int

update #UDAAssetClass
set AssetClassID = 2
where AssetName like 'Equity%'

update #UDAAssetClass
set AssetClassID = 4
where AssetName like 'Future%'

update #UDAAssetClass
set Type = 0
where AssetName like '%Put'

update #UDAAssetClass
set Type = 1
where AssetName like '%Call'

update sm
set sm.UDAAssetClassID = temp.AssetID
from T_SMSymbolLookUpTable sm inner join #UDAAssetClass temp
on sm.AssetID = temp.AssetClassID
inner join T_SMOptionData opt
on sm.Symbol_PK = opt.Symbol_PK
where sm.AssetID = temp.AssetClassID and opt.Type = temp.Type

drop table #UDAAssetClass