/****************************************************************************                                
Name :   PMAddUpdateRunUploadSetup                                
Date Created: 22-nov-2006                                 
Purpose:  Add Update Run Upload Setup items                                
Author: Ram Shankar Yadav                                
Parameters:                                 
 @Xml nText,                              
 @ErrorNumber int output,                              
 @ErrorMessage varchar(200) output                                 
                                
Execution Statement :                                 
 exec [PMAddUpdateRunUploadSetup]  '<xml><element>value</element></xml>'                                
                                
Date Modified: 29 nov 2006                                
Description:   Insertion code updated                                 
Modified By:   Ram Shankar Yadav                               
                              
Date Modified: 30 nov 2006                                
Description:   Renamed Xpath node from //RunUpload to //SetUploadClient                                 
Modified By:   Ram Shankar Yadav                              
                              
Date Modified: 27 dec 2006                                
Description:   Added one insertion value i.e. TableTypeID                                 
Modified By:   Bhupesh Bareja                     
                  
Date Modified: 26 aug 2008                               
Description:   Added two column for XSLTFileName and Its data; and data is placed in T_FileData                                 
Modified By:   Abhilash Katiyar                              
****************************************************************************/                               
Create Proc [dbo].[PMAddUpdateRunUploadSetup_New]                              
(                              
 @Xml nText,                              
 @ErrorNumber int output,                               
 @ErrorMessage varchar(200) output                              
)                           
AS                               
                              
SET @ErrorNumber = 0                              
SET @ErrorMessage = 'Success'                              
                              
BEGIN TRY                              
                              
BEGIN TRAN                              
                              
DECLARE @hDoc int                                 
exec sp_xml_preparedocument @hDoc OUTPUT,@Xml                                 
                              
--This code updates old data.                              
CREATE TABLE     #xmlitem                                                                     
  (                                                                       
  CompanyUploadSetupID Integer,                         
  CompanyID Integer,                         
  ThirdPartyID Integer ,                        
  AutoImport bit,                        
  AutoTime DateTime,                        
  FTPServer varchar(300),                         
  Port Integer,                         
  UserName varchar(50),                         
  Password varchar(30),                        
  DirPath varchar(300),                        
  [FileName] varchar(100),                        
  TableTypeID Integer,                        
  FileLayoutTypeID Integer ,                      
  DataSourceXSLT Varchar(500),                    
  TableFormatName varchar(100),                  
  XSLTBinary varchar(max),                  
  SaveTime DateTime,                  
  DataSourceXSLTFileID int,        
  MappingFileType int,
  FTPFilePath varchar(max)                   
 )                    
                                           
INSERT INTO #xmlitem                                   
 (                                     
  CompanyUploadSetupID ,                         
  CompanyID ,                   
  ThirdPartyID  ,                        
  AutoImport ,                         
  AutoTime ,                        
  FTPServer ,                         
  Port ,                         
  UserName ,                         
  Password ,                        
  DirPath ,                        
  [FileName] ,                        
  TableTypeID ,                        
  FileLayoutTypeID ,                      
  DataSourceXSLT,                    
  TableFormatName,                  
  XSLTBinary,                  
  SaveTime,                  
  DataSourceXSLTFileID,        
  MappingFileType,
  FTPFilePath                                    
 )                                     
SELECT                                   
  CompanyUploadSetupID ,                         
  CompanyID ,                         
  ThirdPartyID  ,                        
  AutoImport ,                         
  AutoTime ,                        
  FTPServer ,                         
  Port ,                         
  UserName ,                         
  Password ,                        
  DirPath ,                        
  [FileName] ,                        
  TableTypeID ,                        
  FileLayoutTypeID,                      
  DataSourceXSLT,                    
  TableFormatName,             
  DataSourceXSLTBinaryData,               
  LastSaveTime,                  
  DataSourceXSLTFileID,        
  MappingFileType,
  FTPFilePath                       
FROM                               
 OPENXML(@hDoc, '//SetUploadClient', 2)                                 
 WITH                              
 (                        
 CompanyUploadSetupID Integer,                         
 CompanyID Integer,                   
 ThirdPartyID Integer ,                         
 AutoImport bit,                        
 AutoTime DateTime,                         
 FTPServer varchar(300),                         
 Port Integer,                   
 UserName varchar(50),                         
 Password varchar(30),                         
 DirPath varchar(300),                        
 FileName varchar(100),                        
 TableTypeID Integer,                        
 FileLayoutTypeID Integer,                      
 DataSourceXSLT varchar(500),                    
 TableFormatName varchar(100),                        
 DataSourceXSLTBinaryData varchar(max),                  
 LastSaveTime DateTime,                  
 DataSourceXSLTFileID int,        
 MappingFileType int,
 FTPFilePath varchar(max)                  
)          
                          
              
            
                  
                
-------------UPDATION------------                  
                  
--UPDATE T_FileData for updated files: Binary Data and ID is Recieved for them                  
UPDATE T_FileData                  
SET                  
 T_FileData.FileNames = #xmlitem.DataSourceXSLT,                  
 T_FileData.FileData = #xmlitem.XSLTBinary,                  
 T_FileData.LastSaveTime = GETUTCDATE()                   
FROM                  
 #xmlitem                  
WHERE                  
 (#xmlitem.DataSourceXSLTFileID = T_FileData.FileId) AND                  
 (#xmlitem.XSLTBinary is not null)                   
                  
-- UPDATE PM_CompanyUploadSetup for already existing Information, XSLTFileID need not to changed                  
UPDATE PM_CompanyUploadSetup                        
SET                               
 PM_CompanyUploadSetup.PMCompanyID = #xmlitem.CompanyID,                              
 PM_CompanyUploadSetup.ThirdPartyID = #xmlitem.ThirdPartyID,                              
 PM_CompanyUploadSetup.FTPServer = #xmlitem.FTPServer,                              
 PM_CompanyUploadSetup.FTPPort = #xmlitem.Port,                              
 PM_CompanyUploadSetup.FTPUserName = #xmlitem.UserName,                              
 PM_CompanyUploadSetup.FTPPassword = #xmlitem.Password,                              
 PM_CompanyUploadSetup.AutoTime = #xmlitem.AutoTime,                              
 PM_CompanyUploadSetup.DirectoryPath = #xmlitem.DirPath,                              
 PM_CompanyUploadSetup.[FileName]= #xmlitem.[FileName],                              
 PM_CompanyUploadSetup.AutoImport = #xmlitem.AutoImport,                              
 PM_CompanyUploadSetup.TableTypeID = #xmlitem.TableTypeID  ,                    
 PM_CompanyUploadSetup.FileLayoutType = #xmlitem.FileLayoutTypeID  ,                            
 PM_CompanyUploadSetup.TableFormatName= #xmlitem.TableFormatName,
 PM_CompanyUploadSetup.FTPFilePath=#xmlitem.FTPFilePath                       
FROM                               
 #xmlitem                              
WHERE                               
  PM_CompanyUploadSetup.CompanyUploadSetupID = #xmlitem.CompanyUploadSetupID                              
 AND #xmlitem.CompanyID <> -1                              
 AND #xmlitem.ThirdPartyID <> -1                              
             
                             
--------------INSERTION-------------              
DECLARE             
@companyUploadSetupID Integer,                          
@companyID Integer,                         
@ThirdPartyID Integer ,                        
@autoImport bit,                        
@autoTime DateTime,                        
@ftpServer varchar(300),                         
@port Integer,                         
@userName varchar(50),                         
@password varchar(30),                        
@dirPath varchar(300),                        
@fileName varchar(100),                        
@tableTypeID Integer,                        
@fileLayoutTypeID Integer ,                      
@dataSourceXSLT Varchar(500),                    
@tableFormatName varchar(100),                  
@xsltBinary varchar(max),                  
@saveTime DateTime,                  
@dataSourceXSLTFileID int,        
@mappingFileType int,             
@ftpfilepath varchar(max)
            
DECLARE PMUploadSetup_Cursor CURSOR FAST_FORWARD FOR                                              
SELECT                                 
  CompanyID ,                         
  ThirdPartyID  ,                        
  AutoImport ,                         
  AutoTime ,                        
  FTPServer ,                         
  Port ,                         
  UserName ,                         
  Password ,                        
  DirPath ,                        
  [FileName] ,                        
  TableTypeID ,                        
  FileLayoutTypeID ,                      
  DataSourceXSLT,                    
  TableFormatName,                  
  XSLTBinary,                  
  SaveTime,                  
  DataSourceXSLTFileID,        
  MappingFileType,
  FTPFilePath                   
FROM  #xmlitem  WHERE #xmlitem.CompanyUploadSetupID Not IN (Select CompanyUploadSetupID from PM_CompanyUploadSetup)                               
 AND #xmlitem.CompanyID <> -1                              
 AND #xmlitem.ThirdPartyID <> -1                
                  
OPEN PMUploadSetup_Cursor;                                            
                                            
FETCH NEXT FROM PMUploadSetup_Cursor INTO                               
@companyID ,                         
@ThirdPartyID  ,                        
@autoImport ,                        
@autoTime ,                        
@ftpServer ,                         
@port ,                         
@userName ,                         
@password ,                        
@dirPath ,                        
@fileName ,                        
@tableTypeID ,                        
@fileLayoutTypeID  ,                      
@dataSourceXSLT ,                    
@tableFormatName ,                  
@xsltBinary ,                  
@saveTime ,                  
@dataSourceXSLTFileID,        
@mappingFileType,
@ftpfilepath;                  
              
                
WHILE @@fetch_status = 0                                              
BEGIN                   
 INSERT INTO  T_FileData                     
 (                  
  FileNames,                  
  FileData,                  
  LastSaveTime,        
  FileType     
 )                  
 SELECT                  
  @dataSourceXSLT,                  
  @xsltBinary,                  
  GETUTCDATE(),        
  @mappingFileType           
                  
 SET @dataSourceXSLTFileID = scope_identity()            
            
 INSERT INTO PM_CompanyUploadSetup                              
 (                              
 PMCompanyID,                               
 ThirdPartyID,             
 AutoImport,            
 AutoTime,                              
 FTPServer,                               
 FTPPort,                               
 FTPUserName,                               
 FTPPassword,                                               
 DirectoryPath,                              
 [FileName],                                                
 TableTypeID,                            
 FileLayoutType ,                          
 TableFormatName,                  
 XSLTFileID,
 FTPFilePath                            
 )                              
 SELECT                                           
 @companyID ,                         
 @ThirdPartyID  ,                        
 @autoImport ,                        
 @autoTime ,                        
 @ftpServer ,                         
 @port ,                         
 @userName ,                         
 @password ,                        
 @dirPath ,                        
 @fileName ,                        
 @tableTypeID ,                        
 @fileLayoutTypeID  ,                             
 @tableFormatName ,                             
 @dataSourceXSLTFileID,
 @ftpfilepath                 
            
 SET @companyUploadSetupID = scope_identity()            
            
 INSERT INTO #xmlitem            
 (                                     
 CompanyUploadSetupID ,                         
 CompanyID ,                     
 ThirdPartyID  ,                        
 AutoImport ,                         
 AutoTime ,                        
 FTPServer ,                         
 Port ,                         
 UserName ,                         
 Password ,                        
 DirPath ,                        
 [FileName] ,                        
 TableTypeID ,                        
 FileLayoutTypeID ,                              
 TableFormatName,                             
 DataSourceXSLTFileID,
 FTPFilePath                                       
 )                                     
 SELECT            
 @companyUploadSetupID ,            
 @companyID ,                         
 @ThirdPartyID  ,                        
 @autoImport ,                        
 @autoTime ,                        
 @ftpServer ,                         
 @port ,                         
 @userName ,                         
 @password ,                        
 @dirPath ,                        
 @fileName ,                        
 @tableTypeID ,                        
 @fileLayoutTypeID  ,                             
 @tableFormatName ,                             
 @dataSourceXSLTFileID ,
 @ftpfilepath                   
                     
 FETCH NEXT FROM PMUploadSetup_Cursor INTO                                 
 @companyID ,                         
 @ThirdPartyID  ,                        
 @autoImport ,                        
 @autoTime ,                        
 @ftpServer ,                         
 @port ,                         
 @userName ,                         
 @password ,                    
 @dirPath ,                        
 @fileName ,                        
 @tableTypeID ,                        
 @fileLayoutTypeID  ,                      
 @dataSourceXSLT ,                    
 @tableFormatName ,                  
 @xsltBinary ,                  
 @saveTime ,                  
 @dataSourceXSLTFileID,        
 @mappingFileType ,
 @ftpfilepath;                
                
END                       
            
                       
                   
-----------DELETION-------------                  
                      
--DELETE Files from the T_FileData which are no longer in use                     
            
DELETE FROM #xmlitem Where CompanyUploadSetupID = 0            
            
DELETE FROM T_FileData            
WHERE (T_FileData.FileId IN ( Select XSLTFileID From  PM_CompanyUploadSetup)) AND            
   (T_FileData.FileId NOT IN ( Select DataSourceXSLTFileID From  #xmlitem )  )            
            
DELETE FROM PM_CompanyUploadSetup            
WHERE CompanyUploadSetupID NOT IN ( Select #xmlitem.CompanyUploadSetupID From  #xmlitem  )            
            
            
CLOSE PMUploadSetup_Cursor;                                             
DEALLOCATE PMUploadSetup_Cursor;                            
EXEC sp_xml_removedocument @hDoc             
DROP TABLE #xmlitem                            
                            COMMIT TRAN                              
                              
END TRY                              
BEGIN CATCH                              
                               
 SET @ErrorNumber = ERROR_NUMBER();                              
 SET @ErrorMessage = ERROR_MESSAGE();                              
                               
 ROLLBACK TRAN                              
                           
END CATCH;             
           
/*            
select * from PM_CompanyUploadSetup            
select * from T_FileData      
select GETUTCDATE()            
delete from PM_CompanyUploadSetup where CompanyUploadSetUpID=63            
delete from T_FileData where FileId = 142        
       
*/ 