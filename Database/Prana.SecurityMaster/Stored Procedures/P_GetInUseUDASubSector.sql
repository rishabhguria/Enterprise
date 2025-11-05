CREATE PROC [dbo].[P_GetInUseUDASubSector]  
as  
select distinct smLookupTable.UDASubSectorID, udaSubSector.SubSectorName from T_SMSymbolLookUpTable smLookupTable
inner join T_UDASubSector udaSubSector on smLookupTable.UDASubSectorID = udaSubSector.SubSectorID
order by udaSubSector.SubSectorName