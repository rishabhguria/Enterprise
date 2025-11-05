    
    
    
CREATE PROC [dbo].[P_SaveImportTradeDetails]    
(    
 @importSourceID int,    
 @importSource varchar(50),    
 @XSLTFileId int,    
 @fileName varchar(50),    
 @binaryData image,    
 @saveTime DateTime,  
 @fileType int   
)    
    
    
AS    
    
Declare @result int    
    
--update if already exists    
if((@importSourceID != '0')  AND (@XSLTFileId != '0') )    
 begin    
  if(@binaryData is not null)    
  begin      
   UPDATE T_FileData     
   Set FileNames = @fileName,    
    FileData = @binaryData,    
    LastSaveTime = @saveTime    
   where FileId = @XSLTFileId    
  end    
    
  UPDATE T_ImportTrade    
  Set ImportSourceName = @importSource    
  where ImportSourceID = @importSourceID     
 end    
    
--Insert if it is new    
    
if((@importSourceID = '0')AND( @XSLTFileId = '0'))    
begin    
  INSERT T_FileData (FileNames, FileData, LastSaveTime, FileType)    
  Values (@fileName, @binaryData, @saveTime, @fileType)    
  Set @XSLTFileId = scope_identity()     
  INSERT T_ImportTrade (ImportSourceName, XSLTFileID)    
  Values (@importSource, @XSLTFileId)    
end    