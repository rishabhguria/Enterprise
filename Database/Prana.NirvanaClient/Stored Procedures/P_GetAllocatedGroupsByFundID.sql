

CREATE procedure P_GetAllocatedGroupsByFundID
(
@FundID varchar(30)
)
as
select GroupID,OrderID,Side,Symbol,TradingAccount,CounterParty,Venue,Price  from T_AllocatedOrders



