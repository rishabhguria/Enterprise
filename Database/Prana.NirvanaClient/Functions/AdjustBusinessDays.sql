/* =============================================              
 Author:  Sumit kakra              
 Create date: 13 Dec 2007              
 Description: Add Subtract businessdays. Also takes care of exchange holidays.      
 Usage      
 Select dbo.AdjustBusinessDays(getutcdate(),1,11)      
 Select dbo.AdjustBusinessDays(getutcdate(),-1,11)      
-- =============================================              
*/      
      
CREATE function [dbo].[AdjustBusinessDays] (@startDate datetime,@numDays int,@AUECID int)      
returns datetime as      
      
Begin      
     Declare @nextBusDay datetime      
     Declare @weekDay tinyInt      
      
     set @nextBusDay = @startDate      
      
     Declare @dayLoop int      
     set @dayLoop = 0      
  
  declare @flagNoWeeklyHoliday int  
  set @flagNoWeeklyHoliday = 0  
  
 --Table to store weekly holidays for the given auec id. 
 Declare @AUECWeeklyHolidaysTable Table(WeeklyHolidayID int)                                        
     Insert Into @AUECWeeklyHolidaysTable   
  Select WeeklyHolidayID From T_AUECWeeklyHolidays WHERE AUECID = @AUECID  
        
  Declare @tempCount int --temp varriable used to store more than 0 value depending upon the weekly off for given  
       --auec. If not there is not any given weekly holiday then 0 would be stored other wise more than 0.  
  Set @tempCount = 0  
      
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
   if @weekday = 7  
    set @weekday = 0 -- As for weekdays stored in table, 0 is for sunday.  
  
   set @tempCount = (Select count(*) from @AUECWeeklyHolidaysTable WHERE WeeklyHolidayID = @weekday)  
   if @tempCount = 0 --When no weekly off is found for auec.  
    begin    
     Set @flagNoWeeklyHoliday = 1  -- Here no weekly holiday is found.
    End  
    --Check if the day is in the list of holidays if yes then skip else increase dayloop      
    if NOT EXISTS(Select HolidayDate from T_AUECHolidays Where       
     Day(HolidayDate) = Day(@nextBusDay)       
     And Month(HolidayDate) = Month(@nextBusDay)      
     And Year(HolidayDate) = Year(@nextBusDay)      
     And AUECID = @AUECID) AND @flagNoWeeklyHoliday = 1  
	 --The combined condition returns true when there is no holiday found for the current day so the day conunter 
	 --is increased.
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
    Set @flagNoWeeklyHoliday = 0    --reset flag holiday so that it can again be checked for holiday condition.
   End   
  return @nextBusDay      
 End    