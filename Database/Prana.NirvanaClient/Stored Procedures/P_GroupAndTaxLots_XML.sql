CREATE procedure [dbo].[P_GroupAndTaxLots_XML]   
--(                                                                                
--@ToAllAUECDatesString varchar(max),                                                      
--@FromAllAUECDatesString varchar(max)                                                                             
--)                                                                                  
as                                                                                      
                                                                                                        
                                                                                                                  
BEGIN TRAN TRAN1                                     
BEGIN TRY                                                                                      
--Declare @ToAUECDatesTable Table(AUECID int,CurrentAUECDate DateTime)                                                                                                                
--                                                                                                    
--Insert Into @ToAUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@ToAllAUECDatesString)                                                    
--                                                  
--Declare @FromAUECDatesTable Table(AUECID int,CurrentAUECDate DateTime)                                                                                                                
--                                                                                                    
--Insert Into @FromAUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@FromAllAUECDatesString)                                                     
                                                                                 
select                                                                                      
1 as Tag ,                                                                                      
null as parent,                                                                                                                                                                       
G.GroupID as "Group!1!GroupID",                                                                                      
G.Symbol as "Group!1!Symbol",                                                                                      
                                                
      
      
                                      
null as "Taxlot!2!TaxlotID",                                                                                        
null as "Taxlot!2!TaxlotQty",                                      
null as "Taxlot!2!Percentage" ,                                                                    
null as "Taxlot!2!Commission"  ,                                                       
null as "Taxlot!2!OtherBrokerFees" ,                                                           
null as "Taxlot!2!StampDuty" ,                                                             
null as "Taxlot!2!TransactionLevy" ,                                                        
null as "Taxlot!2!ClearingFee" ,                                                       
null as "Taxlot!2!TaxOnCommissions" ,                                                        
null as "Taxlot!2!MiscFees" ,                                                        
null as "Taxlot!2!Level1ID",                                                                       
null as "Taxlot!2!StrategyID" ,                                                                            
null as "Taxlot!2!TaxLotState" ,                                                           
null as "Taxlot!2!SecFee" ,                                                           
null as "Taxlot!2!OccFee" ,                                                           
null as "Taxlot!2!OrfFee",                                                       
null as "Taxlot!2!ClearingBrokerFee",                                                                    
null as "Taxlot!2!SoftCommission"                         
                                                                                    
                                
from T_Group as G                                                                                 
  --inner join @ToAUECDatesTable  As ToAUECDates on ToAUECDates.AUECID = G.AUECID                                                                        
 --inner join @FromAUECDatesTable  As FromAUECDates on FromAUECDates.AUECID = G.AUECID                                                    
                
left outer join V_SecMasterData on    V_SecMasterData.TickerSymbol=G.Symbol                                         
--where (G.StateID=1 and DATEDIFF(d,G.AUECLocalDate,ToAUECDates.CurrentAUECDate)>=0 )  or (G.StateID=2 and DATEDIFF(d,G.AllocationDate,ToAUECDates.CurrentAUECDate)>=0 and DATEDIFF(d,G.AllocationDate,FromAUECDates.CurrentAUECDate)<=0 )                     
   
     
                                                              
 union                                                                                        
select                                                                    
2,                                                    
1,                                                                                                                                                                          
G.GroupID,     
null,                                                                                                                                                                     
                                                                     
                                                                              
                                                                             
                                                                                  
L2.TaxlotID,                                                                                           
L2.TaxlotQty ,                                                             
L2.Level2Percentage ,                                                                              
isnull(L2.Commission,0),                                                                                  
isnull(L2.OtherBrokerFees,0),                                                           
isnull(L2.StampDuty,0),                                                        
isnull(L2.TransactionLevy,0),                                                                                
isnull(L2.ClearingFee,0),                                  
isnull(L2.TaxOnCommissions,0),                                                        
isnull(L2.MiscFees,0),                                                        
FA.FundID,                                                                                  
L2.Level2ID,                                                    
L2.TaxLotState,                                                           
isnull(L2.SecFee,0),                                                           
isnull(L2.OccFee,0),                                                           
isnull(L2.OrfFee,0),                                                                                  
isnull(L2.ClearingBrokerFee,0),                                                                              
isnull(L2.SoftCommission,0)                                                               
                                                                                      
from T_Group as G                                                                         
 join T_FundAllocation as FA on FA.GroupID=G.GroupID                                                                                      
 join T_Level2Allocation as L2 on L2.Level1AllocationID=FA.AllocationID                                                                                        
  --inner join @ToAUECDatesTable  As ToAUECDates on ToAUECDates.AUECID = G.AUECID                                                                        
 --inner join @FromAUECDatesTable  As FromAUECDates on FromAUECDates.AUECID = G.AUECID                                                     
                                                                      
--where (G.StateID=1 and DATEDIFF(d,G.AUECLocalDate,ToAUECDates.CurrentAUECDate)>=0 )  or (G.StateID=2 and DATEDIFF(d,G.AllocationDate,ToAUECDates.CurrentAUECDate)>=0 and DATEDIFF(d,G.AllocationDate,FromAUECDates.CurrentAUECDate)<=0)                      
   
                                                            
order by "Group!1!GroupID","Taxlot!2!TaxlotID"                                                                                      
FOR XML EXPLICIT                                                                                                        
                                                                                      
                                                                                      
COMMIT TRANSACTION TRAN1                                                                                                                    
                                                                                  
                                                                                         
END TRY                                                                                                                    
BEGIN CATCH                                                       
                                                                            
ROLLBACK TRANSACTION TRAN1                                                               
END CATCH;

