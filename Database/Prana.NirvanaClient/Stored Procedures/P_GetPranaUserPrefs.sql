
CREATE procedure [dbo].[P_GetPranaUserPrefs] (  
@userID int,  
@fileNameTimeStampPair varchar(8000),  
@seperator1 char(1),  
@seperator2 char(1)  
  
)  
as  
select DataBasePrefs.[FileName],FileData , DataBasePrefs.LastSaveTime from T_PranaUserPrefs as DataBasePrefs  
left join   
(  
  select Column1 as FileName ,Convert(datetime,Column2) as FileDate  
 from  GetTableFromString(@fileNameTimeStampPair,@seperator1,@seperator2)  
) as T_FilePrefs  
on  DataBasePrefs.FileName=T_FilePrefs.FileName  
where  (DataBasePrefs.LastSaveTime > T_FilePrefs.FileDate  and UserID = @userID)
or (T_FilePrefs.FileDate is null and UserID = @userID)

