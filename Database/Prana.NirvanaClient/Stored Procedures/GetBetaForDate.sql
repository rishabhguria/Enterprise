
CREATE Proc [dbo].[GetBetaForDate]                     
(                             
@AllAUECDatesString VARCHAR(MAX)     
)                                
                                   
AS                                
BEGIN                     
            
Select     
AUECID,     
AUECDates.CurrentAUECDate As DayBeforeCurrentAUECDate --dbo.AdjustBusinessDays(AUECDates.CurrentAUECDate,-1,AUECDates.AUECID)            
 into #AUECDatesTable     
From dbo.GetAllAUECDatesFromString(@AllAUECDatesString) As AUECDates            
 
Create Table #V_SecMasterdata 
(  
UnderlyingSymbol varchar(100),  
AUECID int,  
)  
Insert Into #V_SecMasterdata    
Select     
UnderlyingSymbol,             
AUECID  
from V_SecMasterdata      

          
Select     
SM.UnderlyingSymbol as Symbol,             
SM.AUECID,             
AUECDates.DayBeforeCurrentAUECDate As DayBeforeCurrentAUECDate      
into #SymbolAUECDates      
from #V_SecMasterdata SM             
INNER JOIN #AUECDatesTable AS AUECDates ON AUECDates.AUECID = SM.AUECID            
            
                                                     
Declare @OlderBetaPrice Table    
(    
Date datetime,    
Symbol varchar(100),     
DayBeforeCurrentAUECDate datetime    
)                                                      
Insert into @OlderBetaPrice            
Select     
MAX(PM_Dailybeta.date),     
PM_Dailybeta.Symbol,     
MAX(DayBeforeCurrentAUECDate) DayBeforeCurrentAUECDate            
from  PM_Dailybeta             
INNER JOIN #SymbolAUECDates AS SymbolAUECDates ON  PM_Dailybeta.Symbol = SymbolAUECDates.Symbol             
Where            
  DateDiff(d,PM_Dailybeta.Date,SymbolAUECDates.DayBeforeCurrentAUECDate) >= 0 and Beta <>0            
Group by PM_Dailybeta.Symbol    
                   
Select                   
beta ,                   
 CASE             
 WHEN datediff(d,PM_Dailybeta.Date,OlderBetaPrice.DayBeforeCurrentAUECDate) = 0               
 THEN 0                          
 ELSE 1                          
 END AS BetaIndicator,   
PM_DailyBeta.Date as dateActual,               
 PM_Dailybeta.Symbol,
OlderBetaPrice.DayBeforeCurrentAUECDate as DateRequired                 
from PM_Dailybeta                    
INNER JOIN @OlderBetaPrice As OlderBetaPrice ON  OlderBetaPrice.Symbol = PM_Dailybeta.Symbol            
Where PM_Dailybeta.date = OlderBetaPrice.date    
Order by  dbo.GetFormattedDatePart(PM_Dailybeta.Date) DESC                    
            
drop table #SymbolAUECDates,#AUECDatesTable,#V_SecMasterdata    
                            
END            
               
