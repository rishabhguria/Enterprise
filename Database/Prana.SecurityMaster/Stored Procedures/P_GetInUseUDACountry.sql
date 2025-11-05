CREATE PROC [dbo].[P_GetInUseUDACountry]  
as  
select distinct smLookupTable.UDACountryID, udaCountry.CountryName from T_SMSymbolLookUpTable smLookupTable
inner join T_UDACountry udaCountry on smLookupTable.UDACountryID = udaCountry.CountryID
order by udaCountry.CountryName