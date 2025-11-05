/****** Object:  Stored Procedure dbo.[P_GetWeeklyHolidayIdsForAllAUECs]    Script Date: 07/10/2008 ******/          
-- Author  : Bhupesh Bareja        
-- Date   : 07-10-2008        
-- Usage  : P_GetWeeklyHolidayIdsForAllAUECs
-- Description : This sp returns the AUECId wise weekly holiday ids.        
        
CREATE PROCEDURE [dbo].[P_GetWeeklyHolidayIdsForAllAUECs] AS          
          
begin        
         
Select AUECID, WeeklyHolidayID from T_AUECWeeklyHolidays        
order by AUECID    
        
end