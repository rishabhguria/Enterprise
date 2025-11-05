                            
 --  '8eb797fe-3bff-4e29-8e17-a964fcf5a8ce'                    
                             
--Description : This procedure is used to unwind the Fund closing for the supplied @PositionalTaxlotId and @ClosingTaxlotId                                
---Modified By Abhishek                             
--date : 20 May 2008                            
--as now Expired Position can be unwind From same Sp                   
                          
CREATE Procedure [dbo].[PMDeleteFundClosingForTaxlotstest]                         
  (                              
 @TaxLotClosingID  varchar(50)                
  )                
                  
                                
as                                
  
BEGIN  
BEGIN TRAN TRAN1                                                                                                                                                 
                                                                                                                                
BEGIN TRY     
           
declare @ClosedQty float       
set @ClosedQty = (select ClosedQty from PM_taxlotClosing Where  TaxlotClosingID= @TaxLotClosingID)      
select @ClosedQty as ClosedQty
      
declare @ClosingMode int        
set @ClosingMode = (select ClosingMode from PM_taxlotClosing Where  TaxlotClosingID= @TaxLotClosingID)      
select @ClosingMode as ClosingMode
      
declare @Opentaxlotid varchar(50)              
set @Opentaxlotid = (select PositionalTaxlotID from PM_TaxLotClosing               
where TaxLotClosingID= @TaxLotClosingID)    
select @Opentaxlotid as Opentaxlotid

    
      
declare  @taxlot_pkOpen bigint       
set @taxlot_pkOpen = (select taxlot_pk from PM_TaxLots where  TaxlotClosingID_FK=@TaxLotClosingID and taxlotID=@Opentaxlotid)      
select @taxlot_pkOpen as taxlot_pkOpen

      
declare @Closetaxlotid varchar(50)              
set @Closetaxlotid = (select ClosingTaxlotID from PM_TaxLotClosing               
where TaxLotClosingID= @TaxLotClosingID)
select @Closetaxlotid as Closetaxlotid        
      
declare  @taxlot_pkClose bigint       
set @taxlot_pkClose = (select taxlot_pk from PM_TaxLots where  TaxlotClosingID_FK=@TaxLotClosingID and taxlotID=@Closetaxlotid)      
select @taxlot_pkClose as taxlot_pkClose 
      
      
      
Delete From PM_TaxLotClosing where TaxlotClosingID=@TaxLotClosingID      
      
Delete From PM_TaxLots where TaxlotClosingID_FK=@TaxLotClosingID      
      
Update PM_taxlots       
  
set   
ClosedTotalCommissionandFees =  
 ( select  case TaxlotOpenQty  
 when 0 then  ClosedTotalCommissionandFees - OpenTotalCommissionandFees  
else   
    
ClosedTotalCommissionandFees - ((OpenTotalCommissionandFees)*(TaxlotOpenQty+@ClosedQty)/TaxlotOpenQty)      
end )  
  
, OpenTotalCommissionandFees =   
  
 ( select  case TaxlotOpenQty  
 when 0 then  OpenTotalCommissionandFees  
else   
    
OpenTotalCommissionandFees*(TaxlotOpenQty+@ClosedQty)/TaxlotOpenQty    
end )  
  
, TaxlotOpenQty = TaxlotOpenQty+@ClosedQty      
  
  
where  TaxlotID=@Opentaxlotid   and Taxlot_pk > @taxlot_pkOpen      
    
                        
    
      
if(@ClosingMode=0  )      
begin      
Update PM_taxlots       
set ClosedTotalCommissionandFees = ClosedTotalCommissionandFees - ((OpenTotalCommissionandFees)*(TaxlotOpenQty+@ClosedQty)/  case  TaxlotOpenQty
          when 0 then @ClosedQty
             else  TaxlotOpenQty
        end
 )      
, OpenTotalCommissionandFees = OpenTotalCommissionandFees*(TaxlotOpenQty+@ClosedQty)/case  TaxlotOpenQty
          when 0 then @ClosedQty
             else  TaxlotOpenQty
        end     
, TaxlotOpenQty = TaxlotOpenQty+@ClosedQty      
where  TaxlotID=@Closetaxlotid   and Taxlot_pk > @taxlot_pkClose      
end      
     
--if(@ClosingMode=2 or @ClosingMode=4)    
--begin     
--Declare @GroupID varchar(50)                            
--Set @GroupID= (select GroupID from V_Taxlots where V_Taxlots.TaxlotID=@Closetaxlotid)                            
--                    
--Declare @Level1ID varchar(50)                              
--Set @Level1ID= (select Level1AllocationID from V_TaxLots where V_TaxLots.TaxlotID=@Closetaxlotid)                    
--     
--Delete From T_Level2Allocation  where TaxlotID =@Closetaxlotid                      
--                            
--Delete From T_FundAllocation                             
--where T_FundAllocation.AllocationID=@Level1ID                            
--        
--                            
--Delete From T_Group where GroupID =@GroupID                
--            
--Delete From T_SwapParameters where GroupID =@GroupID           
--end     
            
  
COMMIT TRANSACTION TRAN1                                                                                  
                                     
 END TRY                                                                                                                                                
 BEGIN CATCH      
                                       
 ROLLBACK TRANSACTION TRAN1                                                                                                         
                                     
 END CATCH;                                                                              
                                                                                                                                                
END 