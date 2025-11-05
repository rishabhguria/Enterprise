CREATE proc P_SaveFile 
  
@FileNames varchar(50),
@FileData image,
@FiletypeID int ,
@LastSaveTime datetime

as  
insert into T_FileData(FileNames,FileData,FileType,LastSaveTime)
values (@FileNames,@FileData,@FiletypeID,@LastSaveTime)