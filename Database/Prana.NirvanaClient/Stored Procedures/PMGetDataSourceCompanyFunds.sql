/****************************************************************************      
Name :   T_CompanyFunds      
Date Created: 24-Nov-2006       
Purpose:  Gets the ID, shortName and FullName for the dataSource and companyID passed.      
Author: Sugandh Jain      
Parameters:       
   @companyID int         
   , @thirdPartyID int        
Execution StateMent:       
   EXEC [PMGetDataSourceCompanyFunds] 1, 0 , 0,''       
Date Modified: 14-Dec-2006      
Description:   Adding ErrorNumber and ErrorMessage Out Parameters      
Modified By:   Sugandh Jain      
****************************************************************************/      
Create PROCEDURE [dbo].[PMGetDataSourceCompanyFunds]      
 (      
     @companyID int        
   , @thirdPartyID int          
   , @ErrorNumber int output      
   , @ErrorMessage varchar(500) output      
 )      
AS       
      
SET @ErrorMessage = 'Success'      
SET @ErrorNumber = 0      
      
BEGIN TRY      
SELECT       
   CompanyFundID as [FundID]      
 , FundShortName AS [FullFundName]       
FROM       
 T_CompanyFunds      
WHERE      
 CompanyID = @companyid      
 And       
 CompanyThirdPartyID = @thirdPartyID      
      
END TRY      
BEGIN CATCH       
 SET @ErrorNumber = Error_Number();      
 SET @ErrorMessage = ERROR_MESSAGE();        
END CATCH;      
      
  select * from T_CompanyFunds    
      
      