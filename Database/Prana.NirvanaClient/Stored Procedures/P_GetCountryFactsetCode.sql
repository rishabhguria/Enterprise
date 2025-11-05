CREATE PROCEDURE [dbo].[P_GetCountryFactsetCode]
as    
select CountryID,ISOCode from T_Country
