


CREATE procedure [dbo].[DC_GetDropCopyOrderFills]
as
--select * from T_Fills as F join T_Sub as S on
--F.ClOrderID=S.ClOrderID where NirvanaMsgType='10'

select 
ExecutionID ,F.MsgType,MsgSeqNumber,F.TargetCompID,F.SenderCompID,F.TargetSubID,ExecTransType,
F.ClOrderID,F.OrderID,F.OrigClOrderID,F.Symbol,SideID,F.Quantity,OrderTypeID ,F.Price,isnull(F.StopPrice,0) as StopPrice,
TimeInForceID, LastShares, LastPx ,AveragePrice,CumQty ,LeavesQty ,LastMkt,OrderStatus, OrderRejReason ,
CxlRejectReason, ExecType,TransactTime , SendingTime,F.InsertionTime , FillRecieveServerTime ,
F.Text, F.NirvanaSeqNumber,SenderSubID ,ParentClOrderID,OrderTypeTagValue
from T_Fills as F join T_Sub as S on
F.ClOrderID=S.ClOrderID where NirvanaMsgType='10'
and DATEDIFF ( dd , cast(CONVERT(varchar(8),SendingTime) as DateTime) , getutcdate())<=1





