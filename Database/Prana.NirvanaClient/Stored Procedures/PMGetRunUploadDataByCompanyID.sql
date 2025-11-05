/****************************************************************************                  
Name :   [PMGetRunUploadSetupDetailsForID]                  
Date Created: 22-Nov-2006                   
Purpose:  Get all the Run Upload Setup Details for specified Company                  
Author: Ram Shankar Yadav                  
Execution Statement :                   
                  
DECLARE @StartTime DateTIME                  
DECLARE @ENDTIME Datetime                  
SET @StartTime = GETDATE()                  
exec [PMGetRunUploadDataByCompanyID] 1 ,''                  
SET @ENDTIME = GETDATE()                  
SELECT DATEDIFF(ms, @StartTime, @ENDTIME)                  
select * from PM_UploadRuns                  
                  
Date Modified: 25 Nov 2006                  
Description:     a.Recurrence column removed from table and SP                  
Modified By:     Rajat                  
Date Modified: 29 Nov 2006                  
Description:     removing the columns not required for the set up                   
     run upload screen. We will get those from a separate SP.                   
    Added error handling code.                   
Modified By:     Sugandh                  
Date Modified: 30 Nov 2006                  
Description:     After trying several different things, temp table comes to my rescue                  
     to fetch the right records, one row and the latest from upload runs, from                  
     the multiple records for running of getimport thing.                  
Modified By:     Sugandh                  
                  
                
Date Modified: 27 Dec 2006                  
Description:     Added one field i.e. TableTypeID in retrieving list.                   
Modified By:     Bhupesh                  
                
exec [PMGetRunUploadDataByCompanyID] 5, '', 0                
select * from pm_uploadruns          
 select * from PM_CompanyUploadSetup              
****************************************************************************/                  
				   
Create PROC [dbo].[PMGetRunUploadDataByCompanyID]                  
(                  
 @PMCompanyID int,                  
 @ErrorMessage varchar(500) output,                  
 @ErrorNumber int output                  
)                  
AS                  
                  
SET @ErrorMessage = 'Success'                  
SET @ErrorNumber = 0                  
BEGIN TRY                  
                  
CREATE TABLE #TEMP                  
(                   
   uploadid int                  
 , companyuploadsetupid int                  
 , status int                  
 , uploadstart datetime                  
 , uploadend datetime                  
 , totalrecords int                   
 , statusdescription varchar(4000)                  
 , headerrowIndex int                  
 , FirstRecordIndex int                  
)                  
                  
insert into #Temp                   
  select                   
    *                   
  from                   
    PM_UploadRuns                  
  where                   
    UploadID IN                   
    (                   
     select                   
       max(uploadid)                   
     from                   
       PM_UploadRuns                   
     group by                   
       CompanyUploadSetupID                  
    )         
        
         
                  
SELECT                     
 a.PMCompanyID,                   
 d.ShortName CompanyShortName,                  
 a.ThirdPartyID,                  
 TTP.ShortName ThirdPartyShortName,                 
 a.FTPServer,                  
 a.FTPPort,                   
 a.FTPUserName,                   
 a.FTPPassword,                   
 ISNULL(a.AutoTime,'') AS [AutoTime],                  
 a.DirectoryPath,                  
 a.FileName,                  
 a.AutoImport,                    
 UploadEnd  AS [LastRunUploadDate],                   
 ISNULL(b.UploadID, 0) AS [UploadID],                   
 ISNULL(b.HeaderRowIndex, 0) AS [HeaderIndex],                  
 ISNULL(b.FirstRecordIndex, 0) AS [FirstRecordIndex],                  
 ISNULL(b.Status, 0) AS [FileStatus],                  
 a.CompanyUploadSetUpID ,             
 ISNULL(b.TotalRecords, 0) AS TotalRecords,                  
 ISNULL(a.TableTypeID, -1) AS TableTypeID  ,               
 a.FileLayoutType as FileLayoutType,             
 ISNULL(e.FileNames,'') as DataSourceXSLT,            
 ISNULL(a.TableFormatName,'') as TableFormatName ,   
 f.Acronym as ImportTypeAcronym,
 a.FTPFilePath,
 a.LastImportedFile
          
                   
FROM                    
 PM_CompanyUploadSetup a                   
 INNER JOIN T_ThirdParty  TTP ON a.ThirdPartyID = TTP.ThirdPartyID      
Inner Join PM_Company on PM_Company.PMCompanyID=a.PMCompanyID               
 INNER JOIN T_Company d ON  d.CompanyID = PM_Company.NOMSCompanyID                 
 LEFT OUTER JOIN #Temp b ON b.CompanyUploadSetupID  = a.CompanyUploadSetupID        
 LEFT OUTER JOIN T_FileData e ON a.XSLTFileId = e.FileId    
 LEFT OUTER JOIN PM_TableTYpes f ON a.TableTypeID = f.TableTypeID    
                 
                   
WHERE                  
 a.PMCompanyID = @PMCompanyID                  
END TRY                
BEGIN CATCH                   
 SET @ErrorMessage = ERROR_MESSAGE();                  
 SET @ErrorNumber = Error_number();                   
END CATCH; 