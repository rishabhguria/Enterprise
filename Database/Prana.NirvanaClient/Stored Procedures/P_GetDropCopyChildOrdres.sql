
create procedure [dbo].[P_GetDropCopyChildOrdres] as

select distinct CF.ClOrderID, F.ClOrderID from T_Fills as F join T_ClientFills as CF
on F.ClOrderId=CF.OrderID
