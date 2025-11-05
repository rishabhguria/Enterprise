-- =============================================        
-- Author:  om shiv   
-- Create date: 16 JAn 2014       
-- Description: Save Master fund target ratios      
-- =============================================        
CREATE PROCEDURE [dbo].[P_SaveMasterFundTargetRatio]        
(    
 @Xml nText                      
 , @ErrorMessage varchar(500) output                      
 , @ErrorNumber int output                      
 )             
AS        

   
SET @ErrorMessage = 'Success'                      
SET @ErrorNumber = 0     
                     
                                 
BEGIN TRY     
BEGIN TRAN TRAN1       
    
DECLARE @handle int                         
exec sp_xml_preparedocument @handle OUTPUT,@Xml         
    
 CREATE TABLE #TempMasterFund                                                                             
  (                                                                             
   CompanyMasterFundID int,    
   TargetRatioPct float                 
   )      
    
INSERT INTO #TempMasterFund                  
 (                                                                            
    CompanyMasterFundID                      
   ,TargetRatioPct                                                     
 )                                                                            
SELECT                                                                             
    CompanyMasterFundID                      
   ,TargetRatioPct                                    
    FROM OPENXML(@handle, '//MasterFund', 2)                                                                               
 WITH                                                                             
 (                                                       
   CompanyMasterFundID int,    
   TargetRatioPct float               
 )                        
    
--Delete from T_AllocationMasterfundRatio    

update T_AllocationMasterfundRatio 
SET TargetRatioPct = TR.TargetRatioPct
from T_AllocationMasterfundRatio AR 
inner join #TempMasterFund TR ON TR.CompanyMasterFundID = AR.MasterFundID
where TR.CompanyMasterFundID in (SELECT MasterFundID from T_AllocationMasterfundRatio)
    
Insert into T_AllocationMasterfundRatio(MasterFundID,TargetRatioPct) 
select CompanyMasterFundID,TargetRatioPct from #TempMasterFund 
where #TempMasterFund.CompanyMasterFundID not in (SELECT MasterFundID from T_AllocationMasterfundRatio)
    
drop table #TempMasterFund    
    
EXEC sp_xml_removedocument @handle                      
                      
COMMIT TRANSACTION TRAN1                      
                     
END TRY     
                   
BEGIN CATCH                       
SET @ErrorMessage = ERROR_MESSAGE();                      
--  print @errormessage                      
SET @ErrorNumber = Error_number();              
ROLLBACK TRANSACTION TRAN1                       
END CATCH;

