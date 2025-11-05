    
CREATE PROCEDURE [dbo].[P_DeleteOmittedGroups]       
    
(                      
    
 @Xml varchar(max),      
    
 @ErrorMessage varchar(500) output,                          
    
 @ErrorNumber int output                          
    
)                      
    
as               
SET @ErrorNumber = 0                                                                                                                                     
SET @ErrorMessage = 'Success'         
       
BEGIN TRAN TranDelete                                                                                                                                
BEGIN TRY        
    
DECLARE @handle int                     
    
exec sp_xml_preparedocument @handle OUTPUT,@Xml                      
    
create table #Temp_Group                  
(GroupID varchar(50))                  
    
insert into #Temp_Group                  
select                   
GroupID from                   
openxml(@handle,'/Groups/AllocationGroup',1)                  
with                  
(                  
  GroupID varchar(50) '@GroupID'                  
)          
    
delete from PM_Taxlots where GroupID in (select GroupID from #Temp_Group)                                     
    
delete from T_Level2Allocation  where taxlotID in (select taxlotID from V_taxlots inner join #Temp_Group on        
#Temp_Group.GroupID = V_taxlots.GroupID)            
    
delete from T_FundAllocation where GroupID in (select GroupID from #Temp_Group)          
    
delete From T_TradedOrders where ParentCLOrderID in   (Select ClOrderID from T_GroupOrder where GroupID in       
(select GroupID from #Temp_Group)  )               
    
Delete T_Fills where ClorderID              
in (select ClorderID from T_Sub where ParentClOrderID in (Select ParentClOrderID From T_Order where      
ParentClOrderID              
in (select ClorderID from T_GroupOrder inner join #Temp_Group on #Temp_Group.GroupID =T_GroupOrder.GroupID             
)))              
    
Delete T_Sub where ParentClOrderID               
in (Select ParentClOrderID From T_Order where  ParentClOrderID               
in (select ClorderID from T_GroupOrder inner join #Temp_Group on #Temp_Group.GroupID =T_GroupOrder.GroupID              
) )            
    
delete T_Order where ParentClorderID in (select ClorderID from T_GroupOrder inner join #Temp_Group on      
#Temp_Group.GroupID =T_GroupOrder.GroupID)                          
  
Delete From T_OTC_EquitySwapTradeData where GroupID in (Select GroupID from #Temp_Group)  

Delete From T_OTC_CFDTradeData where GroupID in (Select GroupID from #Temp_Group)
Delete From T_OTC_ConvertibleBondTradeData where GroupID in (Select GroupID from #Temp_Group)
   
Delete T_Group where GroupID in (Select GroupID from #Temp_Group )              
    
Delete From T_SwapParameters where GroupID in (Select GroupID from #Temp_Group)  
Delete From T_UngroupedAllocationGroups where GroupID in (Select GroupID from #Temp_Group)  
--delete from T_GroupOrder  where GroupID in (select GroupID from #Temp_Group)         
    
--delete from T_TradedOrders where ParentClorderID in     
    
--(select ClorderID from T_GroupOrder inner join #Temp_Group on #Temp_Group.GroupID =T_GroupOrder.GroupID)                
     
-- Remove the internal representation.                        
    
EXEC sp_xml_removedocument @handle  

/*
Kuldeep A.:
We called this SP here as in case when user mark any price for a symbol in PI and then deletes the trade from
allocation then PI prices was not being cleaned up and whenever this symbol is traded again then it was appearing 
with the user price already checked.
*/
exec P_UpdateOptionModelUserData 0      
    
COMMIT TRANSACTION TranDelete                                                                                         
 
END TRY                                                                                                    
  
BEGIN CATCH            
    
SET @ErrorMessage = ERROR_MESSAGE();                                                                                     
                                              
SET @ErrorNumber = Error_number();      
    
-- Remove the internal representation.                        
    
EXEC sp_xml_removedocument @handle        
  
ROLLBACK TRANSACTION TranDelete                                                                                          
                                            
END CATCH;     

