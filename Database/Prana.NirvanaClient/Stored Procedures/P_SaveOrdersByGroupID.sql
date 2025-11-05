





CREATE     Procedure P_SaveOrdersByGroupID

(

@groupID varchar(30),
@orderID bigint,
@FundID varchar(30),
@AvgPrice float ,
@ExeQty bigint,
@side varchar(10),
@counterparty varchar(20),
@symbol varchar(20),
@tradingaccount varchar(20),
@venue varchar(20)

)
as
if((select count(*) from T_AllocatedOrders where OrderID=@orderID and groupID=@groupID and fundID=@FundID)>0)

set @venue='1'


else
begin
--insert into T_GroupsOrders(groupID,OrderID,FundID) values(@groupID,@orderID,@FundID)
insert into T_AllocatedOrders(groupID,FundID,OrderID,Side,Price,Quantity,Symbol,Venue,TradingAccount,CounterParty) 
values(@groupID,@FundID,@orderID,@side,@AvgPrice,@ExeQty,@symbol,@venue,@tradingaccount,@counterparty)
end





