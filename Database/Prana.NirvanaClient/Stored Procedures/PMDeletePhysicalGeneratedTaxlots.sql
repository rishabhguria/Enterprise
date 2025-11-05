          
                  
                  
---Modified By Abhishek                 
--as now Expired Position can be unwind From same Sp                 
CREATE Procedure [dbo].PMDeletePhysicalGeneratedTaxlots             
(                    
@TaxlotId varchar(50)  ,  
@TaxlotClosingID varchar(50)  
  
                 
)                    
as                    
                    
Delete from PM_TaxLotClosing                     
where  ClosingTaxlotId = @TaxlotId   
  
Delete from PM_TaxLots                 
where  TaxlotClosingID_Fk = @TaxlotClosingID                     
            
Declare @GroupID varchar(50)                      
Set @GroupID= (select GroupID from V_Taxlots where V_Taxlots.TaxlotID=@TaxlotId)                      
              
Declare @Level1ID varchar(50)                        
Set @Level1ID= (select Level1AllocationID from V_TaxLots where V_TaxLots.TaxlotID=@TaxlotId)              
   
 Delete From T_Level2Allocation  where TaxlotID =@TaxlotId                
                      
Delete From T_FundAllocation                       
where T_FundAllocation.AllocationID=@Level1ID                      
                     
                      
Delete From T_Group where GroupID =@GroupID          
      
 Delete From T_SwapParameters where GroupID =@GroupID           
          
          
--select * from PM_TaxLotClosing 