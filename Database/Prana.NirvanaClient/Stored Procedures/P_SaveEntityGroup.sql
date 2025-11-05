

CREATE  procedure P_SaveEntityGroup
(
@entityID varchar(50) ,
@groupID varchar(50)


)
as
if((select count(*) from T_EntityGroup where entityid=@entityID and groupID=@groupID)=0)
insert into T_EntityGroup (entityID,groupID)
 values (@entityID,@groupID)
else
update T_EntityGroup
set


entityID=@entityID,
groupID=@groupID
where 
entityid=@entityID
and groupID=@groupID


