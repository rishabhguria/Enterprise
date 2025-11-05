              
              
                    
                                                                                           
CREATE PROCEDURE [dbo].[PMUndoSplit]                                     
(                                                          
   @RowTaxlotID bigint                                                                                                                                                       
 , @ErrorMessage varchar(500) output                                                                                                                                                        
 , @ErrorNumber int output                                                                                                                    
)                                                                                                                                     
AS                                                       
BEGIN                                                                                                                                                        
                                                                                                                                                    
SET @ErrorMessage = 'Success'                                                                                                                                 
SET @ErrorNumber = 0                                                                                                                                                        
                                                                                                                                                  
BEGIN TRAN TRAN1                                                                                                                                                         
                                                                                                                                        
BEGIN TRY                                                                                                                                                       
 
declare @parentRowPk bigint           
set @parentRowPk = ( select ParentRow_pk from PM_taxlots where taxlotID= @RowTaxlotID      )
    
declare @TaxlotID  varchar(50)            
set @TaxlotID = (select taxlotID from   PM_taxlots where  taxlot_pk =@parentRowPk)        

          
declare @quantity float                      
set @quantity = (select max(taxlotOpenQty) from   PM_taxlots where   taxlotID= @RowTaxlotID )         
declare @TotalOpenCommission float                      
set @TotalOpenCommission = (select max(OpenTotalCommissionandFees) from   PM_taxlots where   taxlotID= @RowTaxlotID )      

          
Delete From PM_Taxlots where taxlotID= @RowTaxlotID                                         
    
if(@RowTaxlotID<>@TaxlotID)    
begin         
update PM_Taxlots             
SET   
TaxlotOpenQty = TaxlotOpenQty+@quantity ,    
OpenTotalCommissionandFees=OpenTotalCommissionandFees+@TotalOpenCommission    
        
from PM_taxlots           
where Taxlot_pk > @parentRowPk and       taxlotID=@TaxlotID  
end     
            
                                                                           
COMMIT TRANSACTION TRAN1                                                                                          
                                             
 END TRY                                     
 BEGIN CATCH                                            
 SET @ErrorMessage = ERROR_MESSAGE();                                                                                                                                                        
 SET @ErrorNumber = Error_number();                                             
 ROLLBACK TRANSACTION TRAN1                                                                                                                 
                                             
 END CATCH;                                                                                      
                                                                                                                                                        
END 


