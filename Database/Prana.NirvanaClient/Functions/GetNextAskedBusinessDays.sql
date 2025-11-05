
/*
Author : Bhupesh Bareja
Date   : 16-05-2008
Description : It takes the startdate and no of days asked and returns a table having asked no of busines days.
*/
CREATE FUNCTION GetNextAskedBusinessDays
(
	@startDate datetime,
	@noOfAskedDays int,
	@auecID int
)
RETURNS @nextBusinessDaysRange TABLE (Items datetime)  

Begin
DECLARE @tempDate datetime
DECLARE @tempDays int  
SET @tempDays = 0
DECLARE @adjustedBusinessDate datetime

while @tempDays != @noOfAskedDays    
          Begin    
      if @tempDays = 0    
      Begin    
		set @tempDate = dateadd([day], -1, @startDate)
		set @adjustedBusinessDate = (Select [dbo].AdjustBusinessDays(@tempDate, 1, @auecID))  -- first get the raw previous day    
		INSERT INTO @nextBusinessDaysRange(Items) VALUES(@adjustedBusinessDate)  
      End    
      Else    
      Begin    
		set @tempDate = @adjustedBusinessDate
		set @adjustedBusinessDate = (Select [dbo].AdjustBusinessDays(@tempDate, 1, @auecID))  -- first get the raw next day    
		INSERT INTO @nextBusinessDaysRange(Items) VALUES(@adjustedBusinessDate)
	  End  
	  Set @tempDays = @tempDays + 1		
	End 









--set @tempDate = @startDate  
-- If @endDate is NULL   
-- BEGIN  
--  Set @endDate = @startDate  
-- END  
--    WHILE dbo.GetFormattedDatePart(@tempDate) <= dbo.GetFormattedDatePart(@endDate)  
--  BEGIN   
--         INSERT INTO @dateRange(Items) VALUES(@tempDate)  
--   SET @tempDate = dateadd(dd,1,@tempDate)  
--     END  
  
    RETURN 
End