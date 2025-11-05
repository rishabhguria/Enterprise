CREATE procedure P_SaveUserFundMapping  
(  
  @userID int,  
  @Xml varchar(max)  
    
)  
As  
  
DECLARE @handle int  
exec sp_xml_preparedocument @handle OUTPUT ,@Xml  
 

 
create table #temp  
(  
  fundID varchar(max)  
)  
  
Insert into #temp  
  
select cast(text as Varchar(max))  
FROM  OPENXML(@handle, '//ArrayOfInt//int',2)         
where text is not null  
  
delete from t_userfundmapping
where UserID = @userID

insert into t_userfundmapping  
(  
  UserID,  
  FundID  
)   
select @userID, Convert(int,fundID) from #temp   
  
drop table #temp  
exec sp_xml_removedocument @handle
