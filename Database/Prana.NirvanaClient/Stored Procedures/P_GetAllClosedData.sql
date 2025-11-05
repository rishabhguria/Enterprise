
CREATE Procedure [dbo].[P_GetAllClosedData]        
(          
 @closingToDate datetime,          
 @closingFromDate datetime,          
 @toTradeDate datetime,          
 @fromTradeDate datetime,      
@isUpdateOpenTaxlots bit          
)          
AS    
-------------------------------          
          
Create Table #PM_Taxlots          
(          
TaxLot_PK  bigint,          
TaxLotID varchar(100),          
TaxLotOpenQty Float,           
AUECModifiedDate DateTime,         
TaxlotClosingId  varchar(100)        
)          
        
Insert Into #PM_Taxlots          
Select           
TaxLot_PK,          
TaxLotID,          
TaxLotOpenQty,            
AUECModifiedDate,         
TaxlotClosingId_FK          
From PM_Taxlots           
Where Taxlot_PK in (Select max(Taxlot_PK) from PM_Taxlots group by TaxlotID)        
and TaxlotClosingID_fk is not null        
----------------------------------------------------------------------------------------      
Create Table #PM_TaxlotClosing      
(      
PositionalTaxlotId varchar(100),      
ClosingTaxlotId varchar(100),      
TaxlotClosingId varchar(100),      
AUECLocalDate DateTime,      
ClosingMode int,
ClosingAlgo int,
IsManualyExerciseAssign bit     
)      
Insert Into #PM_TaxlotClosing        
Select         
 PositionalTaxlotId,ClosingTaxlotId,TaxlotClosingId,AUECLocalDate,ClosingMode,ClosingAlgo,IsManualyExerciseAssign    
from PM_TaxlotClosing       
-----------------------------------------------------------------------------------------      
Create Table #PM_TaxlotIDMaxModifiedDate      
(      
TaxlotId varchar(100),      
AUECMaxModifieddate DateTime      
)      
Insert into #PM_TaxlotIDMaxModifiedDate      
Select       
PositionalTaxlotId as TaxlotId,      
 max(AUECLocalDate) as AUECMaxModifieddate from #PM_TaxlotClosing where ClosingMode <> 7 group by PositionalTaxlotId      
      
Insert into #PM_TaxlotIDMaxModifiedDate      
Select       
ClosingTaxlotId as TaxlotId,       
max(AUECLocalDate) as AUECMaxModifiedDate from #PM_TaxlotClosing where ClosingMode <> 7 group by ClosingTaxlotId       
      
---------------------------------------      
Create Table #V_Taxlots      
(      
TaxlotId varchar(100),      
TradeDate DateTime,      
TaxlotClosingId varchar(100)      
)      
Insert Into #V_Taxlots        
Select         
TaxlotId,AUECLocalDate,TaxlotClosingId_FK      
from V_Taxlots       
      
-----------------------------------------------------------------------------------------      
--- temp table to get positional taxlots with details from pm_taxlots           
select PM.TaxlotId as TaxlotId,              
PM.TaxLotOpenQty,             
PTMD.AUECMaxModifiedDate as ClosingDate ,       
V.TradeDate,    
CA.CorpActionId        
into #Temp1          
from           
#PM_Taxlots PM        
inner join #PM_TaxlotIDMaxModifiedDate PTMD on  PM.TaxlotId =  PTMD.TaxLotId       
inner join #V_Taxlots V on PM.TaxlotId = V.TaxlotId     
left outer join  PM_CorpActionTaxlots CA on  PM.TaxlotId = CA.TaxlotId          
--select * from #Temp1      
    
  
---------------------------------------------------------------------------          
-- table schema : taxlot, counterclosingtaxlot,ExerciseId(if closing mode is exercise or physical),           
--Closingmode, TaxLotOpenQty(to determine closing status) and last closing date          
          
--retrieves partially open positions           
Select          
#Temp1.TaxlotId,          
PMC.ClosingTaxlotId,          
V.TaxlotId as ExerciseId,              
#Temp1.TaxLotOpenQty,          
#Temp1.ClosingDate,
PMC.ClosingAlgo,
PMC.IsManualyExerciseAssign         
into          
#temp2          
from #Temp1           
inner join #PM_TaxlotClosing PMC on #Temp1.TaxlotId = PMC.PositionalTaxlotId         
left outer join #V_Taxlots V on V.TaxlotClosingId = PMC.TaxlotClosingId          
where        
(#Temp1.TaxLotOpenQty > 0 and @isUpdateOpenTaxlots = 1)      
or      
(#Temp1.TaxLotOpenQty = 0  and        
(datediff(dd,#Temp1.ClosingDate  ,@closingToDate) >= 0 and datediff(dd,#Temp1.ClosingDate,@closingFromDate) <= 0)          
or           
(datediff(dd,#Temp1.TradeDate ,@toTradeDate) >= 0 and datediff(dd,#Temp1.TradeDate ,@fromTradeDate) <= 0) )      
or  
#Temp1.CorpActionId is NOT NULL    
    
Insert           
into          
#temp2(TaxlotId,closingTaxlotId,ExerciseId,TaxLotOpenQty,ClosingDate,ClosingAlgo,IsManualyExerciseAssign)          
select           
#Temp1.TaxlotId,          
PMC.PositionalTaxlotId,          
null as ExerciseId,            
#Temp1.TaxLotOpenQty,          
#Temp1.ClosingDate,
PMC.ClosingAlgo,
PMC.IsManualyExerciseAssign           
from #Temp1           
inner join #PM_TaxlotClosing PMC on #Temp1.TaxlotId = PMC.ClosingTaxlotid        
where         
(#Temp1.TaxLotOpenQty > 0 and @isUpdateOpenTaxlots = 1)      
      
or      
(#Temp1.TaxLotOpenQty = 0        
and        
(datediff(dd,#Temp1.ClosingDate ,@closingToDate) >= 0 and datediff(dd,#Temp1.ClosingDate ,@closingFromDate) <= 0)          
or           
(datediff(dd,#Temp1.TradeDate ,@toTradeDate) >= 0 and datediff(dd,#Temp1.TradeDate ,@fromTradeDate) <= 0) )    
or  
#Temp1.CorpActionId is NOT NULL      
---------------------------------------------------------------------------------------          
-- If the underlying taxlots generated in case of physical or exercise closing mode are fully closed and does not lie in the above date range,           
-- they are inserted so that while unwinding the parent taxlot, it can be checked whether the underlying is closed or not..           
          
Insert into #temp2(TaxlotId,          
ClosingTaxlotId,          
ExerciseId,              
TaxLotOpenQty,          
ClosingDate,ClosingAlgo,IsManualyExerciseAssign)           
Select #Temp1.TaxlotId,          
PMC.ClosingTaxlotId,          
null as ExerciseId,            
#Temp1.TaxLotOpenQty,          
#Temp1.ClosingDate,
PMC.ClosingAlgo,
PMC.IsManualyExerciseAssign          
from #Temp1  inner join #temp2 on #Temp1.TaxlotId = #temp2.ExerciseId          
inner join #PM_TaxlotClosing PMC on #Temp1.TaxlotId = PMC.PositionalTaxlotId  and #Temp1.TaxLotOpenQty = 0          
where          
(datediff(dd,#Temp1.ClosingDate ,@closingToDate) <= 0 or datediff(dd,#Temp1.ClosingDate,@closingFromDate) >= 0)          
          
Insert into #temp2(TaxlotId,          
ClosingTaxlotId,          
ExerciseId,              
TaxLotOpenQty,          
ClosingDate,ClosingAlgo,IsManualyExerciseAssign)           
Select #Temp1.TaxlotId,          
null as ClosingTaxlotId,          
null as ExerciseId,             
#Temp1.TaxLotOpenQty,          
#Temp1.ClosingDate,
0 as ClosingAlgo,
#temp2.IsManualyExerciseAssign         
from #Temp1 inner join #temp2 on #Temp1.TaxlotId = #temp2.ExerciseId  and  #Temp1.TaxLotOpenQty = 0          
where          
(datediff(dd,#Temp1.ClosingDate ,@closingToDate) <= 0 or datediff(dd,#Temp1.ClosingDate,@closingFromDate) >= 0)          
     
   
select * from #temp2          
          
          
drop table #Temp1,#temp2,#PM_Taxlots, #PM_TaxlotClosing,#V_Taxlots,#PM_TaxlotIDMaxModifiedDate   

