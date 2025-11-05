/**  
Author: Aman Seth  
Date: 04/09/2014 11:04:00  
**/  
CREATE PROCEDURE [dbo].[P_GetThirdPartyReconFileSettingDetails]      
AS      
BEGIN    
SELECT   
DISTINCT batch.FormatName    
,FTPFolderPath    
,LocalFolderPath     
,XSLTPath      
,XSDPath      
,ISNULL(setting.ThirdPartyID,0) as ThirdPartyID  
,ISNULL(FtpID,0) as FtpID  
,ISNULL(DecryptionID,0)as  DecryptionID    
,ISNULL(EmailID,0) as EmailID  
,ISNULL(EnablePriceTolerance,0)as EnablePriceTolerance    
,ISNULL(PriceTolerance,0) as PriceTolerance  
,ISNULL(setting.PriceToleranceColumns,'') as PriceToleranceColumns  
,setting.FormatType  
,setting.ImportSPName  
,setting.FundID 
from T_ImportFileSettings setting   
INNER JOIN T_BatchSchedulers batch    
on batch.BatchSchedulerID = setting.ImportFileSettingID    
where setting.FormatType=1 and setting.IsActive=1
end
