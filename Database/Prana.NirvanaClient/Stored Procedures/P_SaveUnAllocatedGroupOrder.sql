


CREATE    procedure P_SaveUnAllocatedGroupOrder
(

@groupID varchar(50),
@allocationType varchar(10),
@OrderID bigint,
@currentdate datetime	,
@userID int
)
as
begin
if((select count(*) from T_UnallocatedGroup where

groupID=@groupID)=0)

insert into T_UnallocatedGroup values(@groupID,@allocationType,@currentdate,@userID)
end
begin
if((select count(*) from T_UnAllocatedGroupOrder where
groupID =@groupID and OrderID=@OrderID)=0)

insert into T_UnAllocatedGroupOrder (groupID,OrderID)
 values (@groupID,@OrderID)

else
update T_UnAllocatedGroupOrder
set
groupID=@groupID,
OrderID=@OrderID
where 
groupID =@groupID  and OrderID=@OrderID
end



