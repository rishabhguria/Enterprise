
CREATE procedure [dbo].[P_BTGetAuditTrail]
(
@clientOrderID varchar(50)
)
as
select
F.Symbol,
F.SideID,
S.OrderTypeTagValue,
F.AveragePrice,
F.CumQty,
F.LastShares,
F.LastPx,
F.LeavesQty,
F.OrderStatus,
F.Text

from T_Fills 
as F join T_Sub  as S 
on F.ClOrderID=S.ParentClOrderID
where S.ClientOrderID=@clientOrderID
order by F.NirvanaSeqNumber

--select * from T_sub