CREATE  procedure [dbo].[P_UDAGetAllCountry] as        
select CountryName , CountryID         
from T_UDACountry   
order by CountryName
