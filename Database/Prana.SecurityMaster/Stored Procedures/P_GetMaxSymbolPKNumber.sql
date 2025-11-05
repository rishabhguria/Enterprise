-- Author : Puneet
-- Date  : 1 May 14  
-- Description: Picks up the max id from T_SMsymbolLookupTable. This id is further used to generate the new distinct ids greater than this.  
Create procedure [dbo].[P_GetMaxSymbolPKNumber]    
as    
select max(Symbol_PK) from T_SMSymbolLookUpTable
