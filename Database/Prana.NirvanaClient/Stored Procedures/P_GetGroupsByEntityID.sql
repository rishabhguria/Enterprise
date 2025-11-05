

create procedure P_GetGroupsByEntityID
(
@EntityID varchar(50)
)
as
select GroupID from T_EntityGroup where
EntityID=@EntityID











