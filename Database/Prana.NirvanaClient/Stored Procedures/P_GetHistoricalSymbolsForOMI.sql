

CREATE PROCEDURE [dbo].[P_GetHistoricalSymbolsForOMI]                     
                                                                                                                     
AS   

BEGIN 

select distinct Symbol from T_UserOptionModelInput 

END  