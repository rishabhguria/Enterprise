
  --use nirvanaclient  
/****************************************************************************    
Name :   PMGetApplicationFundsForCompanyWithDataSource    
Date Created: 24-Nov-2006     
Purpose:  Gets the list of mapped DataSourceFundNames and Application Fund Names for the     
   CompanyID and ThirdPartyID given.    
Author: Sugandh Jain    
Parameters:     
   @companyID int       
    
select * from sys.messages where language_id = 1033 and severity = 16 and message_id between 2000 and 2999    
order by message_id asc    
     
select * from sys.messages where message_id = 2601 and language_id = 1033 order by severity asc    
select * from sys.messages where language_id = 1033 and text like '%object cannot %'    
Execution StateMent:     
   EXEC [PMGetApplicationFundsDataForCompanyWithDataSource] 2, 221, '' , 0    
Date Modified: <DateModified>     
Description:     <DescriptionOfChange>     
Modified By:     <ModifiedBy>     
****************************************************************************/    
Create PROCEDURE [dbo].[PMGetApplicationFundsDataForCompanyWithDataSource]    
 (    
     @CompanyID int          
   , @ThirdPartyID int     
   , @ErrorMessage varchar(500) output    
   , @ErrorNumber int output     
 )    
AS     
    
    
SET @ErrorMessage = 'Success'    
SET @ErrorNumber = 0    
    
DECLARE @NirvanaCompanyID int;  
SET @NirvanaCompanyID = (Select NOMSCompanyID from PM_Company where PmcompanyID = @CompanyID)  
  
BEGIN TRY    
SELECT        
   TCF.CompanyID as [SourceItemID]    
 , TCF.FundShortName as [SourceItemName]    
 , TCF.CompanyFundID as [ApplicationItemId]      
FROM    
 T_CompanyFunds TCF       
WHERE    
 TCF.CompanyID = @NirvanaCompanyID   
 AND    
 TCF.companyThirdPartyID = @ThirdPartyID    
    
END TRY    
BEGIN CATCH    
 SET @ERRORNumber = ERROR_NUMBER();    
 SET @ErrorMessage = ERROR_MESSAGE();    
END CATCH;    
    
    
    
    
    
    
    
    