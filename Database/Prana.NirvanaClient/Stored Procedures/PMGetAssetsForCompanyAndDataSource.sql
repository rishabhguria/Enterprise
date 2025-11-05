/****************************************************************************      
Name :   [PMGetAssetsForCompanyAndDataSource]      
Date Created: 6-Dec-2006       
Purpose:  Gets the list of mapped Assets Names for specified CompanyID and ThirdPartyID.      
Author: Ram Shankar Yadav      
Parameters:       
   @CompanyID int            
   , @ThirdPartyID int         
Execution StateMent:       
   EXEC [PMGetAssetsForCompanyAndDataSource] 1, 223      
Date Modified: <DateModified>       
Description:     <DescriptionOfChange>       
Modified By:     <ModifiedBy>       
****************************************************************************/      
CREATE PROCEDURE [dbo].[PMGetAssetsForCompanyAndDataSource]      
 (      
   @NOMSCompanyID int            
   , @ThirdPartyID int       
 )      
AS       
      
BEGIN TRY      
      
SELECT         
 DSA.ApplicationAssetID,      
 CA.AssetName       
FROM      
 PM_DataSourceAssets DSA,      
 T_Asset CA,      
 PM_CompanyDataSources CDS  ,    
 PM_Company PMC    
WHERE      
 CDS.ThirdPartyID = DSA.ThirdPartyID AND      
 DSA.ApplicationAssetID = CA.AssetID AND      
 PMC.PMCompanyID = CDS.PMCompanyID AND    
  DSA.ThirdPartyID = @ThirdPartyID AND      
 PMC.NOMSCompanyID = @NOMSCompanyID      
ORDER BY CA.AssetName      
    
         
END TRY      
BEGIN CATCH      
-- SET @ERROR = ERROR_NUMBER();      
-- SET @ErrorMessage = ERROR_MESSAGE();      
END CATCH;      
--RETURN @ERROR      
    