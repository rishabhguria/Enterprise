create proc P_GetAllClosedGroups 
as
select distinct L2.Groupid from PM_taxlotclosing as Closing,T_Level2Allocation as L2  where 
Closing.positionaltaxlotid=L2.taxlotid or Closing.closingtaxlotid=L2.taxlotid
