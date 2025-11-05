-----------------------------------------------------------------------------------
--Updated BY: Bhavana Rao
--Date: 31/03/14
--Purpose: added columns IsMarketOff and IsSettlementOff in table T_AUECHolidays
-----------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[P_GetAUECHolidays]    
 (    
  @aUECID int,    
  @exchangeID int,  
  @year int,
  @choice int  -- 0 for year specific and 1 for all holidays
 )    
AS       
     
if(@choice = 0)   
Begin    
	 Select HolidayID, AUECID, Description, HolidayDate, IsMarketoff, IsSettlementOff    
	 From T_AUECHolidays    
	 Where AUECID = @aUECID  and datepart(yy,holidaydate) = @year  
end  
  
else    
Begin
	 select HolidayID, AUECID, Description, HolidayDate, IsMarketoff, IsSettlementOff 
	 From T_AUECHolidays    
	 Where AUECID = @aUECID
end    
