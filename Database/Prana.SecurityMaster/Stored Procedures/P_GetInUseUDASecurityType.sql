CREATE PROC [dbo].[P_GetInUseUDASecurityType]  
as  
select distinct smLookupTable.UDASecurityTypeID, udaSecurityType.SecurityTypeName from T_SMSymbolLookUpTable smLookupTable 
inner join T_UDASecurityType udaSecurityType on smLookupTable.UDASecurityTypeID = udaSecurityType.SecurityTypeID
order by udaSecurityType.SecurityTypeName