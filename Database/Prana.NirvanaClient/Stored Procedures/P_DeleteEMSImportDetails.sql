
  
CREATE proc [dbo].[P_DeleteEMSImportDetails]  
(  
@importSourceID int,  
@xsltFileID int  
)  
  
AS  
Declare @result int
Declare @count int
Set @result = 0

Select @count = COUNT(EMSSourceID) from T_CompanyEMSSource Where EMSSourceID = @importSourceID
If( @count < 1 AND @importSourceID > 0 AND @xsltFileID > 0)
begin
	delete from T_ImportTrade  
	where ImportSourceID = @importSourceID  AND XSLTFileID = @xsltFileID
	delete from T_FileData   
	where FileId = @xsltFileID
	Set @result = 1
end

select @result;

-- select * from T_CompanyEMSSource    
-- select * from T_ImportTrade  
-- select * from T_FileData
-- P_DeleteEMSImportDetails '6', '119' 
