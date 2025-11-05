CREATE PROC [dbo].[PMSaveReconXSLTSetupDetails]      
(      
@ReconThirdPartyID int,      
@ThirdPartyID int,      
@reconTypeID int,      
@formatType  varchar(50),      
@xsltID int,      
@xsltName varchar(100),      
@xsltData image,      
@saveTime datetime,    
@fileType int    
)      
AS      
BEGIN TRAN Tran1      
BEGIN TRY      
      
--update if already exists      
if((@ReconThirdPartyID > 0)  AND (@xsltID > 0))      
 begin      
  if( @xsltData is not null)      
  begin      
   UPDATE T_FileData       
   Set FileNames = @xsltName,      
    FileData = @xsltData,      
    LastSaveTime = @saveTime      
   where FileId = @xsltID      
  end      
      
  UPDATE PM_ReconDataSourceXSLT      
  Set ThirdPartyID = @ThirdPartyID,      
   ReconType = @reconTypeID,      
   FormatType = @formatType      
  where ReconThirdPartyID  = @ReconThirdPartyID       
 end      
      
--Insert if it is new      
if((@ReconThirdPartyID < 1 )AND( @xsltID < 1))      
begin      
  INSERT T_FileData (FileNames, FileData, LastSaveTime, FileType)      
  Values (@xsltName, @xsltData, @saveTime, @fileType)      
  Set @xsltID = scope_identity()       
  INSERT PM_ReconDataSourceXSLT (ThirdPartyID, ReconType, FormatType, XSLTID)      
  Values (@ThirdPartyID, @reconTypeID, @formatType, @xsltID)      
end      
      
      
COMMIT TRANSACTION Tran1      
END TRY      
BEGIN CATCH      
ROLLBACK TRANSACTION Tran1      
END CATCH      
      
/*      
select * from PM_ReconDataSourceXSLT      
select * from T_FileData      
*/ 