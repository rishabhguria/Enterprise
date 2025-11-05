

/****** Object:  Stored Procedure dbo.P_GetAUECHolidays    Script Date: 11/17/2005 9:50:24 AM ******/        
-- Author  : Rajat Tandon      
-- Date   : 17 Jan 2008      
-- Description : This sp returns the AUECId wise holiday dates.      
      
CREATE PROCEDURE [dbo].[P_GetHolidaysByAUEC] AS        
        
begin      
       
Select AUECID, HolidayDate from T_AUECHolidays      
order by AUECID,HolidayDate desc  
      
end
