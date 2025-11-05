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
        
     
          
 CREATE FUNCTION [dbo].[GetMarkPriceForDate1]   
(           
 @AllAUECDatesString VARCHAR(MAX)          
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
 Declare @AUECDatesTable Table(AUECID int,CurrentAUECDate DateTime)                                    
                    
Insert Into @AUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@AllAUECDatesString)  
   
INSERT INTO @MarkPriceForDate              
 


Select 
FinalMarkprice , 
 CASE dbo.GetFormattedDatePart(t1.Date)        
 WHEN dbo.GetFormattedDatePart(AUECDates.CurrentAUECDate)        
 THEN 0        
 ELSE 1        
 END AS MarkPriceIndicator,
 t1.Symbol,
 date 
from PM_DayMarkPrice t1 
inner join V_SymbolAuec VSA on VSA.Symbol = t1.Symbol 
 INNER JOIN @AUECDatesTable As AUECDates on AUECDates.AUECID = VSA.AUECID 
where t1.date = (select max(t2.date) from  PM_DayMarkPrice t2 inner join V_SymbolAuec VSA on VSA.Symbol = t1.Symbol 
 INNER JOIN @AUECDatesTable As AUECDates on AUECDates.AUECID = VSA.AUECID 
where t1.symbol = t2.symbol 
and t2.date <= dbo.GetFormattedDatePart( dbo.adjustbusinessdays(AUECDates.CurrentAUECDate,0,VSA.AUECID)))

 Order by  dbo.GetFormattedDatePart(t1.Date) DESC  
RETURN          
END  
  
 --select * from dbo.GetMarkPriceForDate1('0^6/26/2008 12:00:00 AM~1^6/26/2008 12:00:00 AM~11^6/26/2008 12:00:00 AM~12^6/26/2008 12:00:00 AM~15^6/26/2008 12:00:00 AM~16^6/26/2008 12:00:00 AM~17^6/26/2008 12:00:00 AM~18^6/26/2008 12:00:00 AM~19^6/26/2008 12:00:00 AM~20^6/26/2008 12:00:00 AM~21^6/26/2008 12:00:00 AM~22^6/26/2008 12:00:00 AM~23^6/26/2008 12:00:00 AM~24^6/26/2008 12:00:00 AM~26^6/26/2008 12:00:00 AM~27^6/26/2008 12:00:00 AM~28^6/26/2008 12:00:00 AM~29^6/26/2008 12:00:00 AM~30^6/26/2008 12:00:00 AM~31^6/26/2008 12:00:00 AM~32^6/26/2008 12:00:00 AM~33^6/26/2008 12:00:00 AM~34^6/26/2008 12:00:00 AM~36^6/26/2008 12:00:00 AM~37^6/26/2008 12:00:00 AM~38^6/26/2008 12:00:00 AM~39^6/26/2008 12:00:00 AM~43^6/26/2008 12:00:00 AM~44^6/26/2008 12:00:00 AM~45^6/26/2008 12:00:00 AM~47^6/26/2008 12:00:00 AM~48^6/26/2008 12:00:00 AM~49^6/26/2008 12:00:00 AM~53^6/26/2008 12:00:00 AM~54^6/26/2008 12:00:00 AM~55^6/26/2008 12:00:00 AM~56^6/26/2008 12:00:00 AM~57^6/26/2008 12:00:00 AM~58^6/26/2008 12:00:00 AM~59^6/26/2008 12:00:00 AM~60^6/26/2008 12:00:00 AM~61^6/26/2008 12:00:00 AM~62^6/26/2008 12:00:00 AM~64^6/26/2008 12:00:00 AM~65^6/26/2008 12:00:00 AM~66^6/26/2008 12:00:00 AM~67^6/26/2008 12:00:00 AM~68^6/26/2008 12:00:00 AM~69^6/26/2008 12:00:00 AM~70^6/26/2008 12:00:00 AM~')  
  
 --select * from  V_SymbolAuec
  
  --select * from V_taxlots  20080626195845598711820, 20080626195845551511820

  --Insert into Pm_Taxlotclosing values(6, 20080626195845598711820,20080626195845551511820,500,getutcdate(),getutcdate())
  

  