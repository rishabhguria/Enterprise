create proc P_DeleteFile 
  
@FileID int

as  
delete from T_FileData where FileID=@FileID


