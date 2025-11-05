-- =============================================            
-- Author:  <Sandeep>            
-- Create date: <09-May-2008>            
-- Description: <Get Mark Price for passed date>          
--Select * from T_Group  Select * from PM_DayMarkPrice where Symbol='0330-HKG'      
--Select * from GetYesterdayMarkPrice('0330-HKG',20,'2008-05-15')      
--Modified By: Sumit Kakra      
-- Modified date: 2008-05-19     
   
--Modified By: Rajat Tandon       
-- Modified date: 2008-05-22      
-- Description : [GetYesterdayMarkPrice] changed to GetMarkPriceForDate.  
--Select * from GetMarkPriceForDate('0330-HKG',20,'2008-05-15')   
-- Correct Date (yesterday date for yesterday mark price) will be supplied from the consumer end.  
-- Description: Performace was very bad. Modified for better performance      
-- =============================================     
        
CREATE FUNCTION [dbo].[GetMarkPriceForDateOld] (         
@symbol varchar(100),        
@auecID int,           
@date datetime            
)            
RETURNS @MarkPriceForDate TABLE             
 (            
  FinalMarkPrice float  ,      
  MarkPriceIndicator int --return 0 if get the yesterday mark price , if previous date then return 1        
--  YesterdayMarkPriceIndicator int --return 0 if get the yesterday mark price , if previous date then return 1        
 )            
AS            
BEGIN      
--Declare @YesterDayDate Datetime      
--Set @YesterDayDate = dbo.GetFormattedDatePart(dbo.AdjustBusinessDays(@date,-1, @auecID))       
         
INSERT INTO @MarkPriceForDate            
Select   TOP 1      
 PM_DayMarkPrice.FinalMarkPrice,      
 CASE dbo.GetFormattedDatePart(PM_DayMarkPrice.Date)      
 WHEN dbo.GetFormattedDatePart(@date)      
 THEN 0      
 ELSE 1      
 END       
From PM_DayMarkPrice              
Where       
 dbo.GetFormattedDatePart(PM_DayMarkPrice.Date) <= dbo.GetFormattedDatePart(@date)      
 and  PM_DayMarkPrice.Symbol= @symbol and PM_DayMarkPrice.FinalMarkPrice >0      
Order by  dbo.GetFormattedDatePart(PM_DayMarkPrice.Date) DESC      
RETURN        
END  