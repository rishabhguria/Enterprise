/****************************************************************************  
Name :   PMGetAppReconciliedColumnsByID  
Date Created: 29-Nov-2006   
Purpose:  Get all the Run Upload Setup Details for specified datasource  
Author: Bhupesh  
Execution Statement : exec PMGetAppReconciliedColumnsByID 1 ,''  
  
Date Modified: 30 Nov 2006  
Description:   Added ApplicationColumnId fetching code  
Modified By:   Ram  
  
****************************************************************************/  
CREATE PROCEDURE [dbo].[PMGetAppReconciliedColumnsByID]  
 (  
  @ThirdPartyID int,   
  @ErrorMessage varchar(200) output   
 )  
AS  
SET @ErrorMessage = 'Success'  
BEGIN TRY  
  
 Select   
    
  DSC.DataSourceColumnID,  
  AC.Name,   
  ISNULL(AC.Description,'') AS Description,  
  ISNULL(CRC.IncludeAsCash,0) AS IncludeAsCash,  
  ISNULL(CRC.Type,0) [Type],   
  ISNULL(CRC.AcceptableDeviation, 0) AcceptableDeviation,  
  ISNULL(CRC.DeviationSign, 0) DeviationSign,  
  ISNULL(CRC.CompanyReconColumnID, 0) CompanyRecoColumnID,  
  ISNULL(CRC.AcceptDataFrom, 0) AcceptDataFrom  
    
 From   
  PM_DataSourceColumns DSC inner join   
  PM_ApplicationColumns AC on   
  DSC.ApplicationColumnId = AC.ApplicationColumnId  
  left join PM_CompanyReconColumns CRC   
  on DSC.DataSourceColumnID = CRC.DataSourceColumnID  
 Where DSC.ThirdPartyID = @ThirdPartyID  
  
END TRY  
BEGIN CATCH   
 SET @ErrorMessage = ERROR_MESSAGE();  
END CATCH;  