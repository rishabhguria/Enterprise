
CREATE proc [dbo].[GetAllGroupIDSFromParentClOrderID]           
AS          
select T_tradedorders.GroupID,ClOrderID,T_tradedorders.CumQty from T_tradedorders join
T_Group on T_tradedorders.GroupID = T_Group.GroupID   
where  DATEDIFF(dd,GETDATE(),T_Group.AUECLocalDate) >=0

