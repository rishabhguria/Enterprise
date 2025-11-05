CREATE procedure P_DeleteAllocatedGroupOrders
(
@entityID varchar(50) 
)
as

delete from T_GroupOrder where groupID in
(
select GroupID from T_EntityGroup where entityID = @entityID
)
