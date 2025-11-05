  
/****************************************************************************      
Name :   [PMGetAUECForCompanyAndAsset]      
Date Created: 19-Mar-2007       
Purpose:  Gets the list of mapped AUEC for specified CompanyID, ThirdpartyID and AssetID.      
Author: Sugandh Jain     
Parameters:       
   @CompanyID int                
   , @AssetID int        
Execution StateMent:       
   EXEC PMGetAUECForCompanyAndAsset 1, 1      
Date Modified: <DateModified>       
Description:     <DescriptionOfChange>       
Modified By:     <ModifiedBy>       
****************************************************************************/      

Create  PROCEDURE [dbo].[PMGetAUECForCompanyAndAsset] (      
   @CompanyID int,                
   @AssetID int      
 )      
AS       
      
BEGIN TRY      
      
SELECT   DISTINCT    
 AUEC.AUECID AS [AUECID],      
 A.AssetName + '\' +       
 U.UnderLyingName + '\' +       
 E.DisplayName + '\' +       
 C.CurrencySymbol AS [AUEC],    
 A.AssetID,    
 U.UnderlyingID,    
 E.ExchangeID,    
 C.CurrencyID    
FROM       
 T_AUEC AUEC,      
 T_CompanyAUEC CA,      
 T_ASSET A,      
 T_UNDERLYING U,      
 T_Exchange E,      
 T_CURRENCY C      
 --PM_CompanyDataSources CDS inner join     
 --PM_Company PMC on CDS.PMCompanyID = PMC.PMCompanyID      
WHERE       
 AUEC.AUECID = CA.AUECID AND      
 AUEC.AssetID = A.AssetID AND      
 AUEC.UnderlyingID = U.UnderlyingID AND      
 AUEC.ExchangeID = E.ExchangeID AND      
 AUEC.BaseCurrencyID = C.CurrencyID AND      
-- CDS.PMCompanyID = CA.CompanyID AND        
 --PMC.NOMSCompanyID = @CompanyID AND     
 A.AssetID = @AssetID      
      
END TRY      
BEGIN CATCH      
-- SET @ERROR = ERROR_NUMBER();      
-- SET @ErrorMessage = ERROR_MESSAGE();      
END CATCH;      
--RETURN @ERROR  