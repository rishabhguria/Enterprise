
/*
Name :   [P_GetAUECwiseHolidaysBetweenTwoDates]        
Purpose:  Gets all the Holidaydates auecid wise.        
       
Author: Ishant Kathuria
Parameters:                             
  @ErrorMessage varchar(500)                             
  , @ErrorNumber int 
@fromDate DateTime,                        
@todate Datetime                    
@auecid int
   
                           
Execution StateMent:                             
   EXEC [P_GetAUECwiseHolidaysBetweenTwoDates] 'fromdate' ,'todate' ,'auecid'       
                            
Date Modified:  11/14/2011                          
Description:    CAN FIND HOLIDAYS FOR ANY WEEKLY HOLIDAY UNLIKE BEFORE WHICH CALCULATED ONLY FOR SATURDAYS AND SUNDAYS                           
Modified By:    ISHANT KATHURIA   

*/

Create PROCEDURE [dbo].[P_GetAUECwiseHolidaysBetweenTwoDates]                                            
 (                                                                          
  @StartDate DateTime,                                                                                                                 
  @EndDate DateTime,
  @auecid int
  --@ErrorMessage varchar(500) output,                                                                          
  --@ErrorNumber int output                                                                          
 )                                                                          
AS                                                                                                
                                                                          
--SET @ErrorMessage = 'Success'                                   
--SET @ErrorNumber = '0'                         
                                                                   
BEGIN TRY                                                            
DECLARE @weekday varchar(max)
DECLARE @daycode int 
DECLARE @Daycount int
DECLARE @DaysToAdd int
DECLARE @theday varchar(max)
DECLARE @datetostart DATETIME
DECLARE @newdate DATETIME
--
DECLARE @id varchar(20)

set @weekday = DATENAME(dw, @StartDate) --This calculates the corresponding day of start date
--select @StartDate
--select @weekday
 
select  @daycode = weeklyholidayid from T_weeklyHolidays where HolidayName = @weekday   --It gets the corresponding code from T_weeklyHolidays

create table #Holidays(date datetime)
--------------------------------------------------------
--It gets the number of holidays for the given auecid
Declare @AUECWeeklyHolidaysTable Table(WeeklyHolidayID int)                                          
     Insert Into @AUECWeeklyHolidaysTable     
 select weeklyholidayid  from T_AUECWeeklyHolidays where AUECID = @auecid

--select * from @AUECWeeklyHolidaysTable


---------------------------------------------------------
--This region Gets all the dates of holidays for auecid including weekend holiday for that auecid and insert it into a temporary holiday table
while(Select Count(*) From @AUECWeeklyHolidaysTable)> 0
begin
	select top 1 @id = weeklyholidayid from @AUECWeeklyHolidaysTable
	if(@id = 0)
	begin 
		Set @DaysToAdd = 7 - @daycode
		if(@DaysToAdd = 7)
		begin
--print @startdate
			insert into #Holidays select @StartDate
			--select * from #Holidays
		end
		Set @datetostart = @startdate + @DaysToAdd
		set @theday = datename(dw, @datetostart) 
--Check if there is any weekend between two dates
---------------------------------------
		if( @datetostart <= @EndDate)
		begin
			insert into #Holidays select @datetostart
		end
---------------------------------------
		while(@datetostart < =@EndDate)
		Begin
			set @newdate = @datetostart + 7
			set @theday = datename(dw, @newdate)
			if( @newdate < =@EndDate)
			begin
				insert into #Holidays select @newdate
			end
			set @datetostart = @newdate
		End
	end
	else 
	begin
		Set @DaysToAdd = @id-@daycode
		Set @datetostart = @startdate + @DaysToAdd
		set @theday = datename(dw, @datetostart) 
		if( (@datetostart <= @EndDate) and (@datetostart >= @startdate ))
		begin
			insert into #Holidays select @datetostart
		end
		while(@datetostart <= @EndDate)
		Begin
			set @newdate = @datetostart + 7
			set @theday = datename(dw, @newdate)
			if( @newdate <= @EndDate)
			begin
				insert into #Holidays select @newdate
			end
			set @datetostart = @newdate
		End
	end
	Delete @AUECWeeklyHolidaysTable Where weeklyholidayid = @Id
end
insert into #Holidays select Holidaydate from T_AUECHolidays where auecid=@auecid
select date from #Holidays order by date desc
drop table  #Holidays 
END TRY                               
--------------------------------------------------
BEGIN CATCH                                                                           
 --SET @ErrorMessage = ERROR_MESSAGE();                                                     
 --SET @ErrorNumber = Error_number();                                                                           
END CATCH; 






