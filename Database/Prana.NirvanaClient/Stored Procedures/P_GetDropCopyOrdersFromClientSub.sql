
CREATE proc [dbo].[P_GetDropCopyOrdersFromClientSub]
AS
Select ClOrderID,Symbol,Broker,OrderSideTagValue,OrdertypeTagValue,Quantity,Price,TimeInForce,
MsgType,CounterPartyID,VenueID,O.AUECID,[Text]
from T_ClientSub as S join T_ClientOrder as O on S.ParentClOrderID=O.ParentClOrderID
