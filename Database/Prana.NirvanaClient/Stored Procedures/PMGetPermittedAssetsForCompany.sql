/****************************************************************************      
Name :   [PMGetPermittedAssetsForCompany]      
Date Created: 14-Mar-2008  
Purpose:  Gets the list of company permitted Assets Names for specified CompanyID.      
Author: Bhupesh Bareja    
Parameters:       
   @CompanyID int               
Execution StateMent:       
   EXEC [PMGetPermittedAssetsForCompany] 1     
Date Modified:   
Description:     
Modified By:     
****************************************************************************/      
Create PROCEDURE [dbo].[PMGetPermittedAssetsForCompany] (      
   @companyID int            
       
 )      
AS       
      
BEGIN TRY      
      
SELECT         
 distinct A.AssetID AS ApplicationAssetID,  
 A.AssetName       
FROM      
 T_CompanyAUEC CA INNER JOIN T_AUEC AUEC ON CA.AUECID = AUEC.AUECID   
 INNER JOIN T_Asset A ON AUEC.AssetID = A.AssetID   
 INNER JOIN PM_Company PMC on PMC.NOMSCompanyID = @companyID    
     
--WHERE      
-- CDS.ThirdPartyID = DSA.ThirdPartyID AND      
-- DSA.ApplicationAssetID = CA.AssetID AND      
-- --CDS.ThirdPartyID = @ThirdPartyID AND      
-- --CDS.PMCompanyID = @CompanyID      
-- PMC.NOMSCompanyID = @CompanyID      
ORDER BY A.AssetName      
         
END TRY      
BEGIN CATCH      
-- SET @ERROR = ERROR_NUMBER();      
-- SET @ErrorMessage = ERROR_MESSAGE();      
END CATCH;      
--RETURN @ERROR  