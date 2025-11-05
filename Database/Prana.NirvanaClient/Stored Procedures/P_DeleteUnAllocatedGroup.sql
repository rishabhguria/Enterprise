

CREATE   procedure P_DeleteUnAllocatedGroup
(
@groupID varchar(50)
)
as


delete from T_UnAllocatedGroupOrder where groupID=@groupID

delete from T_UnAllocatedGroup where  groupID=@groupID
