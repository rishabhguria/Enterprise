/****************************************************************************      
Name :   PMGetApplicationFundsForCompany      
Date Created: 24-Nov-2006       
Purpose:  Gets the ID, shortName and FullName for the companyId passed.      
Author: Sugandh Jain      
Parameters:       
   @companyID int         
Execution StateMent:       
   EXEC [PMGetApplicationFundsForCompany] 5,7,   '', 0      
Date Modified: <DateModified>       
Description:     <DescriptionOfChange>       
Modified By:     <ModifiedBy>       
****************************************************************************/      
Create PROCEDURE [dbo].[PMGetApplicationFundsForCompany]      
 (      
   @companyID int       
   , @thirdPartyID int    
   , @ErrorMessage varchar(500) output      
   , @ErrorNumber int output            
 )      
AS       
      
      
SET @ErrorMessage = 'Success'      
SET @ErrorNumber = 0      
      
BEGIN TRY      
  
DECLARE @NirvanaCompanyID int;  
SET @NirvanaCompanyID = (SELECT NOMSCompanyID FROM PM_Company Where PMCompanyID = @companyID)  
  
  
SELECT       
   A.CompanyFundID      
 , A.FundName      
 , A.FundShortName      
FROM       
 T_CompanyFunds  A      
WHERE      
 A.CompanyID = @NirvanaCompanyID      
 AND    
 (  A.CompanyFundID is NULL  OR   A.CompanyThirdPartyID = @thirdPartyID    )    
      
END TRY      
BEGIN CATCH      
SET @ERRORNumber = ERROR_NUMBER();      
 SET @ErrorMessage = ERROR_MESSAGE();      
END CATCH;      
      
      
  /*    
select * from T_CompanyFunds        
*/