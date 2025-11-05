  
/*    
Author : Bhupesh Bareja    
Date   : 15-Jul-2008    
Description : It takes the startdate and no of days asked and returns a table having asked no of days.    
Does not takes care of exchange holidays.  
*/    
CREATE FUNCTION [dbo].[GetNextAskedDaysWithoutExchangeANDBusinessHolidays] (    
 @startDate datetime,    
 @noOfAskedDays int   
)    
RETURNS @nextDaysRange TABLE (Items datetime)      
    
Begin    
DECLARE @tempDate datetime    
DECLARE @tempDays int      
SET @tempDays = 0    
SET @tempDate = dateadd(d, -1, @startDate)
    
while @tempDays != @noOfAskedDays        
Begin        
	set @tempDate = dateadd(d, 1, @tempDate)    
	INSERT INTO @nextDaysRange(Items) VALUES(@tempDate)      
    Set @tempDays = @tempDays + 1      
End     
  
      
    RETURN     
End  