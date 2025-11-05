
/*  
Author : Bhupesh Bareja  
Date   : 02-Jun-2008  
Description : It takes the startdate and no of days asked and returns a table having asked no of busines days.  
Does not takes care of exchange holidays.
*/  
CREATE FUNCTION [dbo].[GetNextAskedBusinessDaysWithoutExchangeHolidays] (  
 @startDate datetime,  
 @noOfAskedDays int 
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
  set @adjustedBusinessDate = (Select [dbo].AdjustBusinessDaysWithoutExchangeHolidays(@tempDate, 1))  -- first get the raw previous day      
  INSERT INTO @nextBusinessDaysRange(Items) VALUES(@adjustedBusinessDate)    
      End      
      Else      
      Begin      
  set @tempDate = @adjustedBusinessDate  
  set @adjustedBusinessDate = (Select [dbo].AdjustBusinessDaysWithoutExchangeHolidays(@tempDate, 1))  -- first get the raw next day      
  INSERT INTO @nextBusinessDaysRange(Items) VALUES(@adjustedBusinessDate)  
   End    
   Set @tempDays = @tempDays + 1    
 End   

    
    RETURN   
End
