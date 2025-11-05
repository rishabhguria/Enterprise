create procedure P_GetMaxClOrderID

as
select  max(ClOrderID) from T_Sub 