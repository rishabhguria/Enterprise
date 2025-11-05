CREATE Proc [dbo].[GetBetaForDateForZeroBetas]                       
(                               
@symbollist VARCHAR(MAX),       
@auecDateList varchar(max)
)                                  
                                     
AS                                  
BEGIN   
                                
Declare @Symbols Table                                                
(      
id int not null IDENTITY(1,1),                                         
 symbol varchar(max) 
)
Insert into @Symbols                      
Select Cast(Items as varchar(max)) from dbo.Split(@symbollist,',')

Declare @AUECDates Table                                                
(                                                
id int not null IDENTITY(1,1),
 date datetime 
)
Insert into @AUECDates                      
Select Cast(Items as datetime ) from dbo.Split(@auecDateList,',')    

Declare @OlderBetaPrice Table                                                
(                                                
 date datetime,
DayBeforeCurrentAUECDate datetime,
symbol varchar(max)
)      
                                                           
Insert into @OlderBetaPrice            
Select     
MAX(PM_Dailybeta.date),         
MAX([@AUECDates].date),
PM_Dailybeta.Symbol                  
from PM_DailyBeta inner JOIN @Symbols on PM_DailyBeta.Symbol=[@Symbols].symbol   
inner join @AUECDates ON [@AUECDates].id=[@Symbols].id  
where   
DateDiff(d,PM_Dailybeta.Date,[@AUECDates].date) >= 0 and Beta <> 0             
Group by PM_Dailybeta.Symbol  


Select                   
beta ,                   
 CASE             
 WHEN datediff(d,PM_Dailybeta.Date,OlderBetaPrice.DayBeforeCurrentAUECDate) = 0               
 THEN 0                          
 ELSE 1                          
 END AS BetaIndicator,    
PM_DailyBeta.Date as dateActual ,              
 PM_Dailybeta.Symbol,
OlderBetaPrice.DayBeforeCurrentAUECDate as Daterequired              
from PM_Dailybeta                    
INNER JOIN @OlderBetaPrice As OlderBetaPrice ON  OlderBetaPrice.Symbol = PM_Dailybeta.Symbol            
Where PM_Dailybeta.date = OlderBetaPrice.date    
Order by  dbo.GetFormattedDatePart(PM_Dailybeta.Date) DESC 
                           
END   

