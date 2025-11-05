
-- =============================================                  
-- Author:  <Sandeep>                  
-- Create date: <09-May-2008>                  
-- Description: <Get Mark Price for passed date>                
--Select * from T_Group  Select * from PM_DayMarkPrice where Symbol='0330-HKG'            
--Select * from GetYesterdayMarkPrice('0330-HKG',20,'2008-05-15')            
--Modified By: Sumit Kakra            
-- Modified date: 2008-05-19   
-- Description: Performace was very bad. Modified for better performance            
         
--Modified By: Rajat Tandon             
-- Modified date: 2008-05-22            
-- Description : [GetYesterdayMarkPrice] changed to GetMarkPriceForDate.  
      
--Modified By: Sumit Kakra
-- Modified date: 2008-10-22            
-- Description : Optimizations brought down execution time from 9 secs to couple of miliseconds
/* 
Select * from GetMarkPriceForDate('0^10/22/2008 12:00:00 AM~1^4/16/2008 12:00:00 AM~11^10/22/2008 12:00:00 AM~12^10/22/2008 12:00:00 AM~15^10/22/2008 12:00:00 AM~16^10/22/2008 12:00:00 AM~17^10/22/2008 12:00:00 AM~18^10/22/2008 12:00:00 AM~19^10/22/2008 12:00:00 AM~20^10/22/2008 12:00:00 AM~21^10/22/2008 12:00:00 AM~22^10/22/2008 12:00:00 AM~23^10/22/2008 12:00:00 AM~24^10/22/2008 12:00:00 AM~25^10/22/2008 12:00:00 AM~26^10/22/2008 12:00:00 AM~27^10/22/2008 12:00:00 AM~28^10/22/2008 12:00:00 AM~29^10/22/2008 12:00:00 AM~30^10/22/2008 12:00:00 AM~31^10/22/2008 12:00:00 AM~32^10/22/2008 12:00:00 AM~33^10/22/2008 12:00:00 AM~34^10/22/2008 12:00:00 AM~36^10/22/2008 12:00:00 AM~37^10/22/2008 12:00:00 AM~38^10/22/2008 12:00:00 AM~39^10/22/2008 12:00:00 AM~')         
*/
-- =============================================           
              
           
                
 CREATE procedure [dbo].[P_GetMarkPriceForDate]         
(                 
 @AllAUECDatesString VARCHAR(MAX)                
)        
                     
AS                    
BEGIN         

Create Table #AUECDatesTable (AUECID int,DayBeforeCurrentAUECDate DateTime)                                          
                          
Insert Into #AUECDatesTable Select AUECID, dbo.AdjustBusinessDays(AUECDates.CurrentAUECDate,-1,AUECDates.AUECID)
 From dbo.GetAllAUECDatesFromString(@AllAUECDatesString) As AUECDates
    
Create Table #SymbolAUECDates (Symbol varchar(100), AUECID int, DayBeforeCurrentAUECDate datetime)
Insert Into #SymbolAUECDates
Select V_SymbolAuec.Symbol, 
	   V_SymbolAuec.AUECID, 
	   AUECDates.DayBeforeCurrentAUECDate 
from V_SymbolAuec
INNER JOIN #AUECDatesTable AS AUECDates ON AUECDates.AUECID = V_SymbolAuec.AUECID

                                         
Create Table #OlderMarkPrice (Date datetime,Symbol varchar(100), DayBeforeCurrentAUECDate datetime)                                          
Insert into #OlderMarkPrice
Select MAX(PM_DayMarkPrice.date), PM_DayMarkPrice.Symbol, MAX(DayBeforeCurrentAUECDate) DayBeforeCurrentAUECDate
from  PM_DayMarkPrice 
INNER JOIN #SymbolAUECDates AS SymbolAUECDates ON  PM_DayMarkPrice.Symbol = SymbolAUECDates.Symbol 
Where PM_DayMarkPrice.FinalMarkprice>0
 AND PM_DayMarkPrice.Date <= SymbolAUECDates.DayBeforeCurrentAUECDate
Group by PM_DayMarkPrice.Symbol

Select       
FinalMarkprice ,       
 CASE 
 WHEN datediff(d,PM_DayMarkPrice.Date,OlderMarkPrice.DayBeforeCurrentAUECDate) = 0   
 THEN 0              
 ELSE 1              
 END AS MarkPriceIndicator,      
 PM_DayMarkPrice.Symbol,      
 PM_DayMarkPrice.date       
from PM_DayMarkPrice        
INNER JOIN #OlderMarkPrice As OlderMarkPrice ON  OlderMarkPrice.Symbol = PM_DayMarkPrice.Symbol
Where PM_DayMarkPrice.date = OlderMarkPrice.date 
Order by  PM_DayMarkPrice.Date DESC        

Drop Table   #AUECDatesTable 
Drop Table   #SymbolAUECDates 
Drop Table   #OlderMarkPrice 
END



