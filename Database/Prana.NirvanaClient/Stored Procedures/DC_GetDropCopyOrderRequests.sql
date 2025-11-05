
CREATE procedure [dbo].[DC_GetDropCopyOrderRequests]
as
select S.OrderID,O.ParentClOrderID as ParentClOrderID,S.ClOrderID,O.Symbol,S.OrigClOrderID from T_Order as O join T_Sub as S
on O.ParentClOrderID=S.ParentClOrderID
where OriginatorTypeID=3 and S.NirvanaMsgType <> 14
and DATEDIFF ( dd , cast(CONVERT(varchar(12),O.InsertionTime) as DateTime) , getutcdate())<=1



