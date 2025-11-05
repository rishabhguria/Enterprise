create procedure P_DeleteGroupOrders
(
@groupID varchar(50)
)
as

delete from T_GroupOrder where GroupID=@groupID