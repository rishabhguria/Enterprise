/* =============================================              
 Author:  Gaurav Dhariwal         
 Create date: 8 Sep 2011              
 Description: Check For AUEC wise Holidays, Also takes care of exchange holidays.      
 Usage      
 If Given Date Is Holiday Will Return 1 Else 0
-- =============================================              
*/      
      
CREATE function [dbo].[CheckForHoliday] (@dateToCheck datetime,@AUECID int)      
returns int as      
--declare @Date as datetime
--set     @Date='9/6/2011'
--declare @AUECID int
--set @AUECID=1 


Begin      
     
Declare @isHoliday int

  declare @flagNoWeeklyHoliday int  
  set @flagNoWeeklyHoliday = 0  

Declare @weekDay tinyInt

Declare @AUECWeeklyHolidaysTable Table(WeeklyHolidayID int)                                        
     Insert Into @AUECWeeklyHolidaysTable   
  Select WeeklyHolidayID From T_AUECWeeklyHolidays WHERE AUECID = @AUECID  

Declare @tempCount int --temp varriable used to store more than 0 value depending upon the weekly off for given  
       --auec. If not there is not any given weekly holiday then 0 would be stored other wise more than 0.  
  Set @tempCount = 0  

SET @weekDay =((@@dateFirst+datePart(dw,@dateToCheck)-2) % 7) + 1       

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
     Day(HolidayDate) = Day(@dateToCheck)       
     And Month(HolidayDate) = Month(@dateToCheck)      
     And Year(HolidayDate) = Year(@dateToCheck)      
     And AUECID = @AUECID) AND @flagNoWeeklyHoliday = 1  
	 --The combined condition returns true when there is no holiday
	 Begin
		set @isHoliday=0      
		--select  'Not Holiday'
     End   
	else
     Begin      
		set @isHoliday=1   -- Holiday
		--select 'Holiday'
     End
  return @isHoliday      
 End

