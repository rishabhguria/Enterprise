    
/***************************************************************************    
    
select dbo.F_GetTotalDaysInDateRange('2014/02/01','2014/02/29','30_360')    
    
***************************************************************************/    
CREATE function F_GetTotalDaysInDateRange    
(    
 @StartDate datetime,    
 @Enddate datetime,    
 @DayCountConvention varchar(20)    
)    
returns float    
as      
Begin    
 Declare @TotalDays int    
 Declare @TotalMonths int    
 SET @TotalDays = 0    
 if(@DayCountConvention ='30_360')    
 Begin    
  Set @TotalMonths = DATEDIFF(m,@StartDate,@Enddate) - 1    
  Set @TotalDays = @TotalDays + @TotalMonths*30    
  Set @TotalDays = @TotalDays + (30-DAY(@StartDate))    
  if(DAY(@Enddate)<>31)    
  Begin  
    if(DAY(@Enddate)=28 and datediff(m,dateadd(d,1,@EndDate),dateadd(m,1,@EndDate)) = 0)  
    begin   
   Set @TotalDays = @TotalDays + DAY(@Enddate)    
   Set @TotalDays = @TotalDays + 2    
    End  
    Else if(DAY(@Enddate)=29 and datediff(m,dateadd(d,1,@EndDate),dateadd(m,1,@EndDate)) = 0)   
    begin   
   Set @TotalDays = @TotalDays + DAY(@Enddate)    
   Set @TotalDays = @TotalDays + 1    
    End  
    else  
   Set @TotalDays = @TotalDays + DAY(@Enddate)     
  END   
   else    
  Set @TotalDays = @TotalDays + 30    
  
  return (@TotalDays+1.0)/datediff(d,getdate(),dateadd(d,360,getdate()))    
 End    
 Else If (@DayCountConvention ='Actual_360')    
 Begin    
  return cast((datediff(d,@StartDate,@Enddate)+1.0)/datediff(d,getdate(),dateadd(d,360,getdate())) as float)    
 End    
 Else If (@DayCountConvention = 'Actual_365')    
 Begin    
  return cast((datediff(d,@StartDate,@Enddate)+1.0)/datediff(d,getdate(),dateadd(d,365,getdate())) as float)    
 End    
 Else If (@DayCountConvention = 'Actual_Actual')    
 Begin    
 return cast(    
 (datediff(d,@StartDate,@Enddate)+1.0) / datediff(d,DATEADD(yy, DATEDIFF(yy,0,@StartDate), 0),DATEADD(yy, DATEDIFF(yy,0,@StartDate) + 1, 0))     
 as float)    
 End    
    return 0.0    
end 