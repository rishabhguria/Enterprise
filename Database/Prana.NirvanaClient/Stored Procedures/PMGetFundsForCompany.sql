/****************************************************************************        
Name :   PMGetFundsForCompany        
Date Created: 25-Sep-2007         
Purpose:  Gets the ID, shortName and FullName for the companyId passed.        
Author: Bhupesh Bareja        
Parameters:         
   @companyID int           
Execution StateMent:         
   EXEC [PMGetApplicationFundsForCompany] 2, '', 0        
Date Modified: <DateModified>         
Description:     <DescriptionOfChange>         
Modified By:     <ModifiedBy>         
****************************************************************************/        
Create PROCEDURE [dbo].[PMGetFundsForCompany]        
 (        
     @companyID int         
   , @ErrorMessage varchar(500) output        
   , @ErrorNumber int output              
 )        
AS         
        
        
SET @ErrorMessage = 'Success'        
SET @ErrorNumber = 0        
        
BEGIN TRY        
    
--DECLARE @NirvanaCompanyID int;    
--SET @NirvanaCompanyID = (SELECT NOMSCompanyID FROM PM_Company Where PMCompanyID = @companyID)    
    
    
SELECT         
   CF.CompanyFundID        
 , CF.FundName        
 , CF.FundShortName        
FROM         
 T_CompanyFunds  TCF      
 LEFT OUTER JOIN T_CompanyFunds CF ON TCF.CompanyfundID = CF.CompanyFundID      
WHERE        
 CF.CompanyID = @companyID        
 /*AND      
 (       
 B.CompanyFundID is NULL       
 OR   B.ThirdPartyID = @ThirdPartyID      
)  */    
        
END TRY        
BEGIN CATCH        
SET @ERRORNumber = ERROR_NUMBER();        
 SET @ErrorMessage = ERROR_MESSAGE();        
END CATCH;        