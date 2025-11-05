
/*  
  
Author : Rajat  
Created: 02 Nov 2007  
Usage   
declare @date datetime  
set @date = getutcdate()  

select * from dbo.GetPositionDateAndAUECLocalDateRange('2008-01-10 00:05:00 AM',getutcdate(),'2008-01-11 09:05:00 AM') 
*/  
CREATE FUNCTION [dbo].[GetPositionDateAndAUECLocalDateRange] (  
 @startDate datetime,   
 @endDate datetime,
 @auecLocalStartDate datetime  
)  
RETURNS @dateRange TABLE (DateItem datetime, AUECLocalDateItem datetime)  
AS  
  
 BEGIN  
  
	 DECLARE @tempDate datetime  
	 DECLARE @tempAUECLocalStartDate datetime 

	 set @tempDate = @startDate 
	 set @tempAUECLocalStartDate = @auecLocalStartDate

	 If @endDate is NULL   
	 BEGIN  
	  Set @endDate = @startDate  
	 END  

	 WHILE datediff(d,@tempDate,@endDate) >= 0  
	  BEGIN   
		  INSERT INTO @dateRange(DateItem,AUECLocalDateItem) VALUES(@tempDate,@tempAUECLocalStartDate)  
		  SET @tempDate = dateadd(dd,1,@tempDate) 
		  SET @tempAUECLocalStartDate = dateadd(dd,1,@tempAUECLocalStartDate) 
	  END  
    RETURN 
  
 END


