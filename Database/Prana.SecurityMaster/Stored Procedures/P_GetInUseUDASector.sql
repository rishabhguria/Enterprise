CREATE PROC [dbo].[P_GetInUseUDASector]  
as  
select distinct smLookupTable.UDASectorID, udaSector.SectorName from T_SMSymbolLookUpTable smLookupTable
inner join T_UDASector udaSector on smLookupTable.UDASectorID = udaSector.SectorID
order by udaSector.SectorName