/****************************************************************************      
Name :   [PMGetAssetsForCompany]      
Date Created: 6-Dec-2006       
Purpose:  Gets the list of mapped Assets Names for specified CompanyID.      
Author: Sugandh Jain    
Parameters:       
   @CompanyID int               
Execution StateMent:       
   EXEC [PMGetAssetsForCompany] 1     
Date Modified: <16-05-07>       
Description:     <To correct the join as per the PMcompanyID as the companyID given is NOMSCompanyID>       
Modified By:     <Bhupesh Bareja>    
    
Date Modified: <DateModified>       
Description:     <DescriptionOfChange>       
Modified By:     <ModifiedBy>       
****************************************************************************/      
CREATE PROCEDURE [dbo].[PMGetAssetsForCompany] (      
   @CompanyID int            
       
 )      
AS       
      
BEGIN TRY      
      
SELECT         
 distinct DSA.ApplicationAssetID,      
 CA.AssetName       
FROM      
 PM_DataSourceAssets DSA,      
 T_Asset CA,    
 PM_CompanyDataSources CDS inner join     
 PM_Company PMC on CDS.PMCompanyID = PMC.PMCompanyID    
     
WHERE      
 CDS.ThirdpartyID = DSA.ThirdpartyID AND      
 DSA.ApplicationAssetID = CA.AssetID AND      
 --CDS.ThirdPartyID = @ThirdPartyID AND      
 --CDS.PMCompanyID = @CompanyID      
 PMC.NOMSCompanyID = @CompanyID      
ORDER BY CA.AssetName      
         
END TRY      
BEGIN CATCH      
-- SET @ERROR = ERROR_NUMBER();      
-- SET @ErrorMessage = ERROR_MESSAGE();      
END CATCH;      
--RETURN @ERROR  