
----------------------------------------------------------------------------------------------------------------------------------
CREATE Proc [dbo].[GetSharesOutsandingForDate]                   
(                           
 @AllAUECDatesString VARCHAR(MAX)                          
)                  
                                 
AS                              
BEGIN                   
          
Create table #AUECDatesTable (AUECID int,DayBeforeCurrentAUECDate DateTime)                                                    
                                    
Insert Into #AUECDatesTable Select AUECID, AUECDates.CurrentAUECDate -- dbo.AdjustBusinessDays(AUECDates.CurrentAUECDate,-1,AUECDates.AUECID)          
 From dbo.GetAllAUECDatesFromString(@AllAUECDatesString) As AUECDates          
              
Create Table #SymbolAUECDates (Symbol varchar(100), AUECID int, DayBeforeCurrentAUECDate datetime)          
Insert Into #SymbolAUECDates          
Select V_SymbolAuec.Symbol,           
    V_SymbolAuec.AUECID,           
    AUECDates.DayBeforeCurrentAUECDate           
from V_SymbolAuec          
INNER JOIN #AUECDatesTable AS AUECDates ON AUECDates.AUECID = V_SymbolAuec.AUECID          
          
                                                   
Create Table #OlderOutstandingPrice (Date datetime,Symbol varchar(100), DayBeforeCurrentAUECDate datetime, AUECID int)                                                    
Insert into #OlderOutstandingPrice          
Select MAX(PM_DailyOutstandings.date), PM_DailyOutstandings.Symbol, MAX(DayBeforeCurrentAUECDate) DayBeforeCurrentAUECDate, MAX(SymbolAUECDates.AUECID)  as AUECID        
from  PM_DailyOutstandings           
INNER JOIN #SymbolAUECDates AS SymbolAUECDates ON  PM_DailyOutstandings.Symbol = SymbolAUECDates.Symbol           
Where          
  PM_DailyOutstandings.Date <= SymbolAUECDates.DayBeforeCurrentAUECDate          
Group by PM_DailyOutstandings.Symbol          
                 
Select                 
Outstandings ,                 
 CASE           
 WHEN datediff(d,PM_DailyOutstandings.Date,OlderOutstandingPrice.DayBeforeCurrentAUECDate) = 0             
 THEN 0                        
 ELSE 1                        
 END AS OutstandingIndicator,                
 PM_DailyOutstandings.Symbol,
 OlderOutstandingPrice.AUECID                
from PM_DailyOutstandings                  
INNER JOIN #OlderOutstandingPrice As OlderOutstandingPrice ON  OlderOutstandingPrice.Symbol = PM_DailyOutstandings.Symbol          
Where PM_DailyOutstandings.date = OlderOutstandingPrice.date           
Order by  dbo.GetFormattedDatePart(PM_DailyOutstandings.Date) DESC                  
          
Drop Table #AUECDatesTable  
Drop Table #SymbolAUECDates  
Drop Table #OlderOutstandingPrice  
      
      
                          
END   
  
