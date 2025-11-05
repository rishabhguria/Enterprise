/****************************************************************************        
Name :   [PMGetFundsForCompanyAndDataSource]        
Date Created: 6-Dec-2006         
Purpose:  Gets the list of mapped Fund Names for specified CompanyID and ThirdPartyID.        
Module: Close Trade/PM  
Author: Ram Shankar Yadav        
Parameters:         
   @CompanyID int              
   , @ThirdPartyID int           
Execution StateMent:         
   EXEC [PMGetFundsForCompanyAndDataSource] 1, 1        
Date Modified: <DateModified>         
Description:     <DescriptionOfChange>         
Modified By:     <ModifiedBy>         
****************************************************************************/        
Create PROCEDURE [dbo].[PMGetFundsForCompanyAndDataSource] (        
   @NOMSCompanyID int              
   , @thirdPartyID int         
 )        
AS         
        
BEGIN TRY        
SELECT            
  TCF.CompanyFundID as CompanyFundID        
 , TCF.FundShortName         
FROM        
 T_CompanyFunds TCF                 
WHERE        
 TCF.CompanyID = @NOMSCompanyID        
 AND        
 TCF.CompanyThirdPartyID   = @thirdPartyID        
        
END TRY        
BEGIN CATCH        
-- SET @ERROR = ERROR_NUMBER();        
-- SET @ErrorMessage = ERROR_MESSAGE();        
END CATCH;        
--RETURN @ERROR  