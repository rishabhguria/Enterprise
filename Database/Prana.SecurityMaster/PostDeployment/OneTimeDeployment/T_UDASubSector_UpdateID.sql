select * into #UDASubSector from T_UDASubSector where SubSectorName <> 'Undefined'

ALTER TABLE #UDASubSector
ADD NewSubSectorID INT IDENTITY
CONSTRAINT PK_SubSectorID PRIMARY KEY CLUSTERED

--select * from #UDASubSector

update uda
set uda.SubSectorID = temp.NewSubSectorID
from T_UDASubSector uda
inner join #UDASubSector temp
on uda.SubSectorID = temp.SubSectorID

update sm
set sm.UDASubSectorID = temp.NewSubSectorID
from T_SMSymbolLookUpTable sm
inner join #UDASubSector temp
on sm.UDASubSectorID = temp.SubSectorID

drop table #UDASubSector