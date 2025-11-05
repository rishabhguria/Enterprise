
/* =============================================            
 Author:  Bhupesh Bareja
 Create date: 02 Jul 2008            
 Description: Add Subtract businessdays. Does not takes care of exchange holidays.    
 Usage    
 Select dbo.AdjustBusinessDaysWithoutExchangeHolidays(getutcdate(),1)    
 Select dbo.AdjustBusinessDaysWithoutExchangeHolidays(getutcdate(),-1)    
-- =============================================            
*/    
    
CREATE function [dbo].[AdjustBusinessDaysWithoutExchangeHolidays] (@startDate datetime, @numDays int)    
returns datetime as    
    
Begin    
     Declare @nextBusDay datetime    
     Declare @weekDay tinyInt    
    
     set @nextBusDay = @startDate    
    
     Declare @dayLoop int    
     set @dayLoop = 0    
    
     while @dayLoop != @numDays    
          Begin    
      if @numDays < 0    
      Begin    
     set @nextBusDay = dateAdd(d,-1,@nextBusDay)  -- first get the raw previous day    
      End    
      Else    
      Begin    
     set @nextBusDay = dateAdd(d,1,@nextBusDay)  -- first get the raw next day    
      End    
    
               SET @weekDay =((@@dateFirst+datePart(dw,@nextBusDay)-2) % 7) + 1     
      -- Statement above will always return 1 for Monday    
      -- and 6 and 7 for Sat and Sun respectively    
    
      if @numDays < 0    
      Begin    
       if @weekDay = 6 set @nextBusDay = @nextBusDay - 1  -- 6 is Saturday so jump to Monday    
       if @weekDay = 7 set @nextBusDay = @nextBusDay - 2  -- 7 is Sunday so jump to Friday    
      End    
      Else    
               Begin    
       if @weekDay = 6 set @nextBusDay = @nextBusDay + 2  -- 6 is Saturday so jump to Monday    
       if @weekDay = 7 set @nextBusDay = @nextBusDay + 1  -- 7 is Sunday so jump to Monday    
      End    
          
      --Check if the day is in the list of holidays if yes then skip else increase dayloop    
--      if NOT EXISTS(Select HolidayDate from T_AUECHolidays Where     
--                 Day(HolidayDate) = Day(@nextBusDay)     
--                 And Month(HolidayDate) = Month(@nextBusDay)    
--                 And Year(HolidayDate) = Year(@nextBusDay)    
--                 And AUECID = 1)    
      Begin    
        -- next day    
       if @numDays < 0    
       Begin    
        set @dayLoop = @dayLoop - 1     
       End    
       Else    
       Begin    
        set @dayLoop = @dayLoop + 1     
       End    
      End    
          End     
     return @nextBusDay    
End
