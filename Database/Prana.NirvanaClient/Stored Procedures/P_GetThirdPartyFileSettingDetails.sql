
/**  
Author: Aman Seth  
Date: 04/09/2014 11:04:00  
**/  
CREATE PROCEDURE [dbo].[P_GetThirdPartyFileSettingDetails]  
(            
@ThirdPartyTypeShortName VARCHAR(100)           
,@ThirdPartyName  VARCHAR(100)         
,@ImportType  VARCHAR(100)  
,@FormatName  VARCHAR(100)   
)  
  
AS  
BEGIN  

DECLARE @ThirdPartyID varchar(30); 
DECLARE @ThirdPartyTypeID varchar(30); 
SELECT @ThirdPartyTypeID = (SELECT distinct ThirdPartyTypeID FROM T_ThirdPartyType where ThirdPartyTypeShortName= @ThirdPartyTypeShortName )
SELECT @ThirdPartyID = (SELECT ThirdPartyID FROM T_ThirdParty where ShortName=@ThirdPartyName and ThirdPartyTypeID= @ThirdPartyTypeID )

SELECT  
@ThirdPartyTypeShortName as ThirdPartyTypeShortName  
,@ThirdPartyName as ThirdPartyName  
,@ImportType as ImportType       
,@FormatName as FormatName   
,FTPFolderPath      
,LocalFolderPath       
,XSLTPath         
,XSDPath          
,ISNULL(setting.ThirdPartyID,0)   as ThirdPartyID     
,ISNULL(FtpID,0)   as FtpID     
,ISNULL(DecryptionID,0)as  DecryptionID        
,ISNULL(EmailID,0)   as EmailID       
,ISNULL(EnablePriceTolerance,0)as EnablePriceTolerance  
,ISNULL(PriceTolerance,0)   as PriceTolerance   
,ISNULL(PriceToleranceColumns,'') as PriceToleranceColumns   
,FormatType 
,company.CompanyID
,company.ShortName
,company.Name 
,BatchStartDate
,setting.ImportSPName 
from T_ImportFileSettings setting 
INNER JOIN T_BatchSchedulers batch    
on batch.BatchSchedulerID = setting.ImportFileSettingID
INNER JOIN T_Company company 
On setting.CompanyID = company.CompanyID   
where setting.FormatName=@FormatName    
and batch.ThirdPartyID=@ThirdPartyID     

END


