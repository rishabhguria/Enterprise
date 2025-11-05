-- =============================================        
-- Author:  Ashish Poddar and Sandeep Singh         
-- Create date: 16th October, 2008        
-- Description: This function returns a table with Mark Prices for all symbols for a given date. If mark price is not         
--_____________ available for a particular date then it picks up the mark price for previous available date.        
        
--select * from dbo.GetMarkPriceForGivenDate(Null)        
-- =============================================        
CREATE FUNCTION [dbo].[GetMarkPriceForGivenDate]        
(         
 @MarkPriceDate datetime = Null        
)        
RETURNS @MarkPriceForDate TABLE                           
(                          
 FinalMarkPrice float  ,                    
 MarkPriceIndicator int,              
 Symbol varchar(50) ,              
 Date Datetime                  
)              
                             
AS                          
BEGIN               
--declare @MarkPriceDate datetime        
--set @MarkPriceDate = NULL        
        
INSERT INTO @MarkPriceForDate                          
Select             
 MP.FinalMarkprice ,             
 CASE                    
  WHEN datediff(d,MP.Date,IsNull(@MarkPriceDate, getutcdate())) = 0                    
   THEN 0                    
   ELSE 1                    
  END AS MarkPriceIndicator   ,         
 MP.Symbol,            
 Date           
from PM_DayMarkPrice MP        
 where MP.Date =         
  (        
  select Max(MP2.Date) from PM_DayMarkPrice MP2        
  Where MP.Symbol = MP2.Symbol and         
    datediff(d,MP2.date,IsNull(@MarkPriceDate, getutcdate())) >= 0 and         
    MP2.FinalMarkprice >0        
  )        
Order by  dbo.GetFormattedDatePart(MP.Date), MP.Symbol        
        
RETURN         
END 

