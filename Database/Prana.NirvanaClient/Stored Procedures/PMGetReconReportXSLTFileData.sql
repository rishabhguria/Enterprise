CREATE procedure [dbo].[PMGetReconReportXSLTFileData]  
(  
@fileNameTimeStampPair varchar(8000),    
@seperator1 char(1),    
@seperator2 char(1)    
)  
  
as  
  
--Temporary table to hold the files info from the DB  
CREATE TABLE #FileData(  
FileNames varchar(50),  
FileData image,  
LastSaveTime datetime )  
  
INSERT INTO #FileData (FileNames, FileData, LastSaveTime)  
  
Select FileNames, FileData, LastSaveTime  
From T_FileData  
Where T_FileData.FileId IN   
 (   
  Select XSLTID From PM_ReconDataSourceXSLT Where XSLTID is not null  
 )  
 
--Temporary table to hold the information about the files from the client   
CREATE TABLE #T_FileInfoFromClient  
(  
Files varchar(50),  
FileDate datetime  
)  
INSERT INTO #T_FileInfoFromClient (Files, FileDate)  
select Column1 as Files ,Convert(datetime,Column2) as FileDate    
from  GetTableFromString(@fileNameTimeStampPair,@seperator1,@seperator2)    
  
Select   
  #FileData.FileNames, #FileData.FileData   
from #FileData  
left join #T_FileInfoFromClient   
on #FileData.FileNames = #T_FileInfoFromClient.Files  
where #FileData.LastSaveTime >= #T_FileInfoFromClient.FileDate or #T_FileInfoFromClient.FileDate is null  
  
drop table #FileData  
drop table #T_FileInfoFromClient  
  
  

/*
select * from  PM_ReconDataSourceXSLT 
select * from T_FileData
*/
