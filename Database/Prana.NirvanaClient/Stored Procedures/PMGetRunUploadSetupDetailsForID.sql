/****************************************************************************                  
Name :   [PMGetRunUploadSetupDetailsForID]                  
Date Created: 22-Nov-2006                   
Purpose:  Get all the Run Upload Setup Details for specified Company                  
Author: Ram Shankar Yadav                  
Execution Statement : exec [PMGetRunUploadSetupDetailsForID] 5,'',0                 
                  
Date Modified: 25 Nov 2006                  
Description:     a.Recurrence column removed from table and SP                  
Modified By:     Rajat                  
Date Modified: 29 Nov 2006                  
Description:     removing the columns not required for the set up                   
     run upload screen. We will get those from a separate SP.                   
    Added error handling code.                   
Modified By:     Sugandh                  
                  
Date Modified: 27 Dec 2006                  
Description:     Added one field i.e. TableTypeID in retrieving list.                   
Modified By:     Bhupesh               
              
Date Modified: 04 Jan 2008                  
Description:     Added one field i.e. DataSourceXSLT to get XSLT.                   
Modified By:     Sandeep             
        
Date Modified: 23 Feb 2009                  
Description:   PM_CompanyUploadSetup table PMCompanyID join was with T_Company's CompanyID, but actaully        
      in between there is one more table PM_Company, which is PM company table and the join should be through        
      this table                  
Modified By:     Sandeep              
****************************************************************************/                  
                  
Create PROC PMGetRunUploadSetupDetailsForID           
(                  
 @PMCompanyID int,                  
 @ErrorMessage varchar(500) output,                  
 @ErrorNumber int output                  
)                  
AS                  
SET @ErrorMessage = 'Success'                  
SET @ErrorNumber = 0                  
BEGIN TRY                  
                  
                  
                  
SELECT                   
 a.CompanyUploadSetupID,                   
 a.PMCompanyID,                   
 c.ShortName CompanyShortName,                  
 a.ThirdPartyID,                  
 b.ShortName DataSourceShortName,                  
 a.FTPServer,                  
 a.FTPPort,                   
 a.FTPUserName,                   
 a.FTPPassword,                   
 ISNULL(a.AutoTime,'1900-01-01') AS AutoTime,                  
 a.DirectoryPath,                  
 a.FileName,                  
 a.LastRunUploadDate,                  
 a.AutoImport,                  
 ISNULL(a.TableTypeID, -1) AS TableTypeID,                
 a.FileLayoutType AS FileLayoutType,              
ISNULL(d.FileNames,'') as DataSourceXSLT,            
ISNULL(a.TableFormatName,'') as TableFormatName,             
a.XSLTFileID as DataSourceXSLTFileID,
a.FTPFilePath                   
FROM                   
 PM_CompanyUploadSetup a,                  
 T_ThirdParty b,                    
 T_Company c,              
 T_FileData d ,        
 PM_Company e              
WHERE                  
 a.ThirdPartyID = b.ThirdPartyID AND         
 a.PMCompanyID  = e.PMCompanyID And        
 e.NOMSCompanyID = c.CompanyID AND                  
 e.PMCompanyID = @PMCompanyID AND            
 a.XSLTFileId  = d.FileId              
                  
END TRY                  
BEGIN CATCH                   
 SET @ErrorMessage = ERROR_MESSAGE();                  
 SET @ErrorNumber = Error_number();                   
END CATCH;             
            
           
/*          
select * from  PM_CompanyUploadSetup                  
select * from  T_ThirdParty                     
select * from  PM_Company         
select * from  T_Company 
select * from  T_FileData           
*/ 