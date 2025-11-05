-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 29-mar-14
--Purpose: Save the File Setting details for third party

--Modified BY: Bharat Raturi
--Date: 21-may-14
--Purpose: save the batch format as thirdpartytype.ThirdpartyShortname.importtype.importfileformatname

--Modified BY: Bharat Raturi
--Date: 28-may-14
--Purpose: save the new detail batchstartdate
-----------------------------------------------------------------
CREATE procedure [dbo].[P_SaveThirdPartyFileSettingDetails]  
(@xmlDoc ntext,@thirdPartyID int)  
as  
declare @handle int  
exec sp_xml_preparedocument @handle OUTPUT,@xmlDoc  
  
CREATE  TABLE #TempSetting                                                                                   
(                                                                                   
ImportFileSettingID int,    
IsActive bit,    
FormatName varchar(200),    
ImportTypeID int,    
CompanyID int,    
ReleaseID int,    
FundID int,    
XSLTPath varchar(500),    
XSDPath varchar(500),    
ImportSPName varchar(200),    
FTPFolderPath varchar(500),    
LocalFolderPath varchar(500),    
FtpID int,    
EmailID int,     
EmailLogID int,    
DecryptionID int,    
ThirdPartyID int,    
PriceToleranceColumns varchar(200) ,  
FormatType int,  
BatchStartDate datetime,  
ImportFormatID int 
)            
    
insert INTO #TempSetting --T_ImportFileSettings                                                                                   
(ImportFileSettingID,isActive,FormatName,ImportTypeID, CompanyID, ReleaseID, FundID,    
XSLTPath, XSDPath, ImportSPName, FTPFolderPath, LocalFolderPath, FtpID, EmailID,    
EmailLogID, DecryptionID, ThirdPartyID,PriceToleranceColumns,FormatType,BatchStartDate,ImportFormatID)    
SELECT                                                                                   
SettingID,IsActive,FormatName,ImportTypeID, ClientID, ReleaseID, FundID,    
XSLTPath, XSDPath, ImportSPName, FTPFolderPath, LocalFolderPath, FtpID, EmailID,    
EmailLogID, DecryptionID, ThirdPartyID, PriceToleranceColumns ,FormatType, BatchStartDate ,ImportFormatID                                       
FROM OPENXML(@handle, '/dsSetting/dtSetting', 2)                                                                                     
WITH                                                                                   
(                                                             
SettingID int,    
IsActive bit,    
FormatName varchar(200),    
ImportTypeID int,    
ClientID int,     
ReleaseID int,    
FundID int,    
XSLTPath varchar(500),    
XSDPath varchar(500),    
ImportSPName varchar(200),    
FTPFolderPath varchar(500),    
LocalFolderPath varchar(500),    
FtpID int,    
EmailID int,     
EmailLogID int,    
DecryptionID int,    
ThirdPartyID int,    
PriceToleranceColumns VARCHAR(200),  
FormatType int,  
BatchStartDate DATETIME,
ImportFormatID int 
)                              
DELETE FROM T_ImportFileSettings     
WHERE ThirdPartyID=@thirdpartyID    
Insert into T_ImportFileSettings      
(    
ImportFileSettingID,    
isActive,    
FormatName,    
ImportTypeID,     
CompanyID,     
ReleaseID,     
FundID,    
XSLTPath,     
XSDPath,     
ImportSPName,     
FTPFolderPath,     
LocalFolderPath,     
FtpID,     
EmailID,    
EmailLogID,    
DecryptionID,    
ThirdPartyID,    
PriceToleranceColumns,  
FormatType,  
BatchStartDate,
ImportFormatID)    
select     
ImportFileSettingID,    
isActive,    
FormatName,    
ImportTypeID,     
CompanyID,     
ReleaseID,     
FundID,    
XSLTPath,     
XSDPath,     
ImportSPName,     
FTPFolderPath,     
LocalFolderPath,     
FtpID,     
EmailID,    
EmailLogID,    
DecryptionID,    
ThirdPartyID,    
PriceToleranceColumns,  
FormatType,  
BatchStartDate,
ImportFormatID 
from #TempSetting    
  
create table #TempSchedule    
(    
BatchSchedulerID int,    
ThirdPartyID int,    
FormatName varchar(200)    
)    
insert INTO #TempSchedule    
(    
BatchSchedulerID,    
ThirdPartyID,    
FormatName    
)    
select DISTINCT     
t1.ImportFileSettingID,     
t1.ThirdPartyID,     
CASE WHEN (t1.FormatType= 1)  
THEN   
t4.ThirdPartyTypeShortName + '.' + t4.ShortName + '.' + 'Recon' + '.' + t1.FormatName   
ELSE   
t4.ThirdPartyTypeShortName + '.' + t4.ShortName + '.' + t5.Acronym + '.' + t1.FormatName   
END  
from #TempSetting t1    
inner join     
(select t2.ThirdPartyID, t2.ShortName, t3.ThirdPartyTypeShortName     
from T_ThirdParty t2    
inner JOIN T_ThirdPartyType t3 on t2.ThirdPartyTypeID=t3.ThirdPartyTypeID) t4    
on t1.ThirdPartyID=t4.ThirdPartyID    
LEFT JOIN PM_TableTypes t5 on t1.ImportTypeID=t5.TableTypeID    
    
insert INTO T_BatchSchedulers    
(BatchSchedulerID, ThirdPartyID,FormatName)    
SELECT    
#TempSchedule.BatchSchedulerID,    
#TempSchedule.ThirdPartyID,    
#TempSchedule.FormatName    
from #TempSchedule    
where BatchSchedulerID not in (SELECT BatchSchedulerID from T_BatchSchedulers)    
    
    
Update T_BatchSchedulers    
SET T_BatchSchedulers.FormatName = #TempSchedule.FormatName    
from T_BatchSchedulers    
inner join #TempSchedule on #TempSchedule.BatchSchedulerID = T_BatchSchedulers.BatchSchedulerID    
where T_BatchSchedulers.BatchSchedulerID in (SELECT BatchSchedulerID from T_BatchSchedulers)   
delete FROM T_BatchSchedulers where BatchSchedulerID not in(SELECT distinct ImportFileSettingID from T_ImportFileSettings)    
    
exec sp_xml_removedocument @handle     
    
drop TABLE #TempSetting       
drop TABLE #TempSchedule
