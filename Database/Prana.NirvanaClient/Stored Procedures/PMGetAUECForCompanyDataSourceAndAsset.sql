/****************************************************************************          
Name :   [PMGetAUECForCompanyDataSourceAndAsset]          
Date Created: 6-Dec-2006           
Purpose:  Gets the list of mapped AUEC for specified CompanyID, ThirdPartyID and AssetID.          
Module: Close Trade/PM  
Author: Ram Shankar Yadav          
Parameters:           
   @CompanyID int    -- This company id is the Nirvana CompanyId and not the PMCompanyID              
   , @ThirdPartyID int,          
   , @AssetID int            
Execution StateMent:           
   EXEC [PMGetAUECForCompanyDataSourceAndAsset] 1, 223, 1          
Date Modified: <DateModified>           
Description:     <DescriptionOfChange>           
Modified By:     <ModifiedBy>           
****************************************************************************/          
CREATE PROCEDURE [dbo].[PMGetAUECForCompanyDataSourceAndAsset] (          
   @CompanyID int,                
   @ThirdPartyID int          
   --,@AssetID int          
 )          
AS           
          
BEGIN TRY          
          
SELECT           
 AUEC.AUECID AS [AUECID],          
 A.AssetName + '\' +           
 U.UnderLyingName + '\' +           
 E.DisplayName + '\' +           
 C.CurrencySymbol AS [AUEC]          
           
          
FROM           
 T_AUEC AUEC,          
 T_CompanyAUEC CA,          
 T_ASSET A,          
 T_UNDERLYING U,          
 T_Exchange E,          
 T_CURRENCY C,          
 PM_CompanyDataSources CDS,        
 PM_Company PMC,          
 PM_DataSourceAssets DSA          
WHERE           
 AUEC.AUECID = CA.AUECID AND          
 AUEC.AssetID = A.AssetID AND          
 AUEC.UnderlyingID = U.UnderlyingID AND          
 AUEC.ExchangeID = E.ExchangeID AND          
 AUEC.BaseCurrencyID = C.CurrencyID AND          
 --CDS.PMCompanyID = CA.CompanyID AND       --Commented on 2nd Nov, 2007    
 PMC.NOMSCompanyID = CA.CompanyID AND    
 CDS.ThirdPartyID = DSA.ThirdPartyID AND          
 DSA.ApplicationAssetID = A.AssetID AND          
 PMC.PMCompanyID = CDS.PMCompanyID AND        
 CDS.ThirdPartyID = @ThirdPartyID AND          
 PMC.NOMSCompanyID = @CompanyID       
--AND     A.AssetID = @AssetID          
          
END TRY          
BEGIN CATCH          
-- SET @ERROR = ERROR_NUMBER();          
-- SET @ErrorMessage = ERROR_MESSAGE();          
END CATCH;          
--RETURN @ERROR  