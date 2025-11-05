

CREATE procedure [dbo].[P_SavePranaUserPrefs] (
@userID int,
@fileName varchar(40),
@data image,
@timeOfSave datetime

)
as
declare @noofRows int
select @noofRows=count(*) from T_PranaUserPrefs where UserID=@userID and FileName=@fileName
if(@noofRows=0)
insert into T_PranaUserPrefs (UserID,[FileName],FileData,LastSaveTime)
values (@UserID,@fileName,@data,@timeOfSave)
else
update T_PranaUserPrefs
set FileData=@data,LastSaveTime=@timeOfSave

where UserID=@userID and FileName=@fileName
