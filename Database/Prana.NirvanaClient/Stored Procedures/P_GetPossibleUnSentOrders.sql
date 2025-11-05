


CREATE procedure [dbo].[P_GetPossibleUnSentOrders]
as
select S.ParentClOrderID,S.ClOrderID,
O.OrderSidetagValue,O.Symbol,S.OrderTypeTagvalue,
S.UserID,O.TradingAccountID,O.AUECID,S.CounterPartyID,S.VenueID,
S.FundID,S.StrategyID,S.MsgType,S.NirvanaMsgType,
S.Quantity,S.Price,S.ExecutionInst,
S.TimeInForce,S.HandlingInst,
S.InsertionTime
from T_Sub as S
join T_Order as O on O.ParentClOrderID=S.ParentClOrderID
left join T_Fills as F on S.ClOrderID=F.ClOrderID
where F.ClOrderID is null


