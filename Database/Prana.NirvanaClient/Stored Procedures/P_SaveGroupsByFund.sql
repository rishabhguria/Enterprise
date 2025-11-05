


CREATE  Procedure P_SaveGroupsByFund

(
@fundID varchar(20)
,@groupID varchar(30)
)
as
if((select count(*) from T_FundsGroups where fundid=@fundID and groupID=@groupID )>0)
	begin
	set @fundID=1
	--update T_FundsGroups set   fundID=@fundID , groupID=@groupID
--where fundid=@fundID and groupID=@groupID
--select count(*) from T_FundsGroups where fundid=@fundID and groupID=@groupID
	end
else
	begin
	insert into T_FundsGroups(fundID,GroupID) values(@fundID,@groupID)
	end 



