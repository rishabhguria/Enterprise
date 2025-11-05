  
-- =============================================              
-- Author:  Rajat              
-- Create date: 01 Nov 2007              
-- Description: Returns the day pnl based on various inputs.            
-- =============================================              
CREATE FUNCTION [dbo].[GetDayPNL] (              
 @positionStartDate datetime,          
 @todayMarkDate datetime,          
 @AvgPrice float,          
 @todayMarkPrice float,          
 @yesterdayMarkPrice float,          
 @positionOpenQuantity bigint,        
 @sideID varchar(1),    
 @contractMultiplier float,         
 @Operator bit -- 0 means multiply, 1 means divide  
)              
RETURNS  Float              
AS              
BEGIN              
           
 Declare @todayFinalMarkPrice float          
 Declare @yesterdayFinalMarkPrice float          
 Declare @sideMultiplier int        
 Declare @dayPNL float          
        
 --declare @positionStartDate datetime          
 --declare @todayMarkDate datetime          
 --declare @AvgPrice float          
 --declare @todayMarkPrice float          
 --declare @yesterdayMarkPrice float         
 --declare @positionOpenQuantity bigint       
 --declare @sideID varchar(1)         
 --      
 --set @positionStartDate = '2007-11-05 14:38:40.693'      
 --set @todayMarkDate = '2007-11-05 00:00:00.000'      
 --set @AvgPrice = 33.3      
 --set @todayMarkPrice = 20.0000      
 --set @yesterdayMarkPrice = 20.0000      
 --set @positionOpenQuantity = 500      
 --set @sideID = 1      
       
 If dbo.GetFormattedDatePart(@positionStartDate) = dbo.GetFormattedDatePart(@todayMarkDate)           
  Begin  
   -- This position is of today's date, hence mark price will be the avg price          
   Set @yesterdayFinalMarkPrice  = IsNull(@AvgPrice,0)          
  End  
 Else          
  Begin  
   -- Else it would be today's mark price           
   Set @yesterdayFinalMarkPrice = IsNull(@yesterdayMarkPrice,0)   
   If @Operator = 1 And @yesterdayFinalMarkPrice > 0   
   Begin  
    Set @yesterdayFinalMarkPrice = 1.0 / @yesterdayFinalMarkPrice  
   End  
  End  
  
 Set @todayFinalMarkPrice  = IsNull(@todayMarkPrice ,0)          
 If @Operator = 1 And @todayFinalMarkPrice > 0   
 Begin  
  Set @todayFinalMarkPrice = 1.0 / @todayFinalMarkPrice  
 End  
  
 Set @sideMultiplier = dbo.GetSideMultiplier(@sideID)        
           
 -- If either of TodayMark or YesterdayMark is 0, it means that mark prices not present and hence DayPNL would be 0          
 If @todayFinalMarkPrice = 0 or @yesterdayFinalMarkPrice = 0          
  Set @dayPNL = 0          
 -- TodayMark and YesterdayMark prices, both are not 0 then DayPNL = (Today's mark price - yesterday's mark price) * Position Open Quantity          
 Else           
  Set @dayPNL = (@todayFinalMarkPrice - @yesterdayFinalMarkPrice) * @positionOpenQuantity  * @sideMultiplier * @contractMultiplier       
       
 RETURN (@dayPNL)              
END  