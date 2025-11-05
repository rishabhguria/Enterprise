  
Create PROC [dbo].[P_GetFillsByOrderID]    
(    
 @Root varchar(50)    
)    
AS  
Declare @orderstatus char
Select @orderstatus = orderstatus from T_fills where ClOrderID=@root

if (@orderstatus = '4')
BEGIN    
Select  max(fills.LastShares) ,max(fills.Text) ,max(fills.LastPx) ,max(fills.TransactTime) ,fills.ClOrderID 
,max(fills.ExecTransType) ,fills.OrderStatus ,max(fills.CumQty) ,max(fills.AveragePrice)
From dbo.T_Fills as fills     
where ClOrderID = @Root 
group by fills.ClOrderID, fills.OrderStatus  
order by max(fills.NirvanaSeqNumber)  
END 

ELSE
BEGIN
Select  fills.LastShares,fills.Text ,fills.LastPx ,fills.TransactTime ,fills.ClOrderID   
,fills.ExecTransType ,fills.OrderStatus ,fills.CumQty ,fills.AveragePrice  
From dbo.T_Fills as fills 
where ClOrderID = @Root
END

