CREATE PROCEDURE [dbo].[P_GetPendingReplacedClOrderId]
AS
begin
select  distinct 
T_Sub.ParentClOrderID, 
T_Fills.ClOrderID from T_Sub inner join T_Fills 
on T_sub.ClOrderID=T_Fills.ClOrderID 
where (T_Fills.TimeInForceID=1 or T_Fills.TimeInForceID=6) 
and T_Fills.OrderStatus='E' and T_Fills.ClOrderID 
not in (select ClOrderID from T_fills where OrderStatus='5');
end
