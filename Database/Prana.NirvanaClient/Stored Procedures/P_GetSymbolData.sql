  
CREATE procedure [dbo].[P_GetSymbolData] as  
  
select TickerSymbol,AssetName,SecurityTypeName,sectorName,SubSectorName,CountryName from V_GetSymbolUDAData  