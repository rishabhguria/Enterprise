/****************************************************************************      
Name :   PMGetApplicationFundsForCompany      
Date Created: 24-Nov-2006       
Purpose:  Gets the ID, shortName and FullName for the companyId passed.      
Author: Sugandh Jain      
Parameters:       
   @companyID int         
Execution StateMent:       
   EXEC [PMGetApplicationFundsForCompany] 1, '', 0      
Date Modified: <DateModified>       
Description:     <DescriptionOfChange>       
Modified By:     <ModifiedBy>       
****************************************************************************/      
Create PROCEDURE [dbo].[PMGetMappedApplicationFundsForCompany]      
 (      
   @companyID int       
--   , @ThirdPartyID int    
   , @ErrorMessage varchar(500) output      
   , @ErrorNumber int output            
 )      
AS       
      
      
SET @ErrorMessage = 'Success'      
SET @ErrorNumber = 0      
      
BEGIN TRY      
SELECT       
   A.CompanyFundID      
 , A.FundName      
 , A.FundShortName      
FROM       
 T_CompanyFunds  A     
WHERE      
 A.CompanyID = @companyid      
 AND    
 (     
 A.CompanyFundID is NOT NULL     
 --OR   B.ThirdPartyID = @ThirdPartyID    
)    
      
END TRY      
BEGIN CATCH      
SET @ERRORNumber = ERROR_NUMBER();      
 SET @ErrorMessage = ERROR_MESSAGE();      
END CATCH;      
      
      
  /*    
select * from T_CompanyFunds      
*/