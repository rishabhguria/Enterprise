-- =============================================    
-- Author:  Abhishek    
-- Create date: 24 Sept 2007    
-- Description: diff in mark price    
-- =============================================    
CREATE FUNCTION  DIFFINMARKPRICE     
(    
 @Symbol varchar(50)    
)    
RETURNS  Float    
AS    
BEGIN    
 declare @TodayDate varchar(10),@YesterdayDate varchar(10) ,@diff float    
 set @TodayDate = CONVERT(VARCHAR(10), getutcdate(), 120)     
 set  @YesterdayDate = CONVERT(VARCHAR(10), getutcdate()-1, 120)     
  
 set @diff =(SELECT         
  FinalMarkPrice         
 FROM         
  PM_DayMarkPrice        
 WHERE        
  Symbol = @Symbol     
  AND        
  DATEADD(day, DATEDIFF(day, 0, Date ), 0) = DATEADD(day, DATEDIFF(day, 0, @TodayDate ), 0)          
  AND ISActive = 1 )-(    
 SELECT         
  FinalMarkPrice         
 FROM         
  PM_DayMarkPrice        
 WHERE        
  Symbol = @Symbol     
  AND        
  DATEADD(day, DATEDIFF(day, 0, Date ), 0) = DATEADD(day, DATEDIFF(day, 0, @YesterdayDate ), 0)          
  AND ISActive = 1 )    
    
 RETURN (@diff)    
    
END 