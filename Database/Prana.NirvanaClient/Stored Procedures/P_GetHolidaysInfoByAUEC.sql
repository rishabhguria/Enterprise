CREATE PROCEDURE [dbo].[P_GetHolidaysInfoByAUEC]AS

begin      
       
Select AUECID, HolidayDate,IsSettlementOff from T_AUECHolidays      
order by AUECID,HolidayDate desc  
      
end
