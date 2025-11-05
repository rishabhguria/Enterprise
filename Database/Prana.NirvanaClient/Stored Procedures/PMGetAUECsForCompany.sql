/****************************************************************************    
Name :   [PMGetAUECsForCompany]    
Date Created: 22-May-2007     
Purpose:  Gets the list of AUECs for specified CompanyID.
Author: Bhupesh Bareja    
Parameters:     
   @CompanyID int    -- This company id is the Nirvana CompanyId and not the PMCompanyID        
   
Execution StateMent:     
   EXEC [PMGetAUECsForCompany] 1
Date Modified: <DateModified>     
Description:     <DescriptionOfChange>     
Modified By:     <ModifiedBy>     
****************************************************************************/    
CREATE PROCEDURE [dbo].[PMGetAUECsForCompany]    
 (    
   @CompanyID int  
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
 T_CURRENCY C
 --PM_Company PMC,    
 WHERE     
 AUEC.AUECID = CA.AUECID AND    
 AUEC.AssetID = A.AssetID AND    
 AUEC.UnderlyingID = U.UnderlyingID AND    
 AUEC.ExchangeID = E.ExchangeID AND    
 AUEC.BaseCurrencyID = C.CurrencyID AND    
 
 CA.CompanyID = @companyID
 --CDS.PMCompanyID = CA.CompanyID AND    


-- PMC.PMCompanyID = CDS.PMCompanyID AND  

-- PMC.NOMSCompanyID = @CompanyID 

    
END TRY    
BEGIN CATCH    
-- SET @ERROR = ERROR_NUMBER();    
-- SET @ErrorMessage = ERROR_MESSAGE();    
END CATCH;    
--RETURN @ERROR 