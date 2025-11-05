
CREATE PROC [dbo].[P_GetOrderTrailwithFills]    
(    
 @Root varchar(50),   
 @isGtcGtd  bit, 
 @userID int   
)    
AS    
BEGIN  
if @isGtcGtd=1
Select distinct * From(
 (Select     
      fills.TransactTime,     
      fills.LastShares,     
      fills.LastPx,     
      fills.Text,     
      fills.AveragePrice,     
      fills.CumQty,     
      fills.OrderStatus,    
      sub.ClOrderID,    
fills.Quantity,    
fills.Price,    
fills.NirvanaSeqNumber,
fills.CounterPartyID,
fills.ExchangeID,
sub.UserID,
sub.TimeInForce
From      
    T_Sub as sub inner join T_Fills as fills on sub.ClOrderID = fills.clOrderid 
	inner join T_TradedOrders as TTO on sub.ClOrderID = TTO.ParentClOrderID
	inner join T_CompanyUserTradingAccounts as TTA on TTA.TradingAccountID = TTO.TradingAccountID 
	where TTO.orderId=@root and (sub.OrderId=@Root or sub.ClOrderID=@Root ) and TTA.CompanyUserID= @userID 
UNION
Select     
      fills.TransactTime,     
      fills.LastShares,     
      fills.LastPx,     
      fills.Text,     
      fills.AveragePrice,     
      fills.CumQty,     
      fills.OrderStatus,    
      sub.ClOrderID,    
fills.Quantity,    
fills.Price,    
fills.NirvanaSeqNumber,
fills.CounterPartyID,
fills.ExchangeID,
sub.UserID,
sub.TimeInForce
From
   T_Sub as sub join T_Fills as fills on sub.ClOrderID = fills.clOrderid 
	where sub.OrderId=@Root or sub.OrderId like @Root+'_%' or sub.ClOrderID=@Root or sub.ParentClOrderID=@Root))as result;
else 
   (Select     
      fills.TransactTime,     
      fills.LastShares,     
      fills.LastPx,     
      fills.Text,     
      fills.AveragePrice,     
      fills.CumQty,     
      fills.OrderStatus,    
      sub.ClOrderID,    
fills.Quantity,    
fills.Price,    
fills.NirvanaSeqNumber,
fills.CounterPartyID,
fills.ExchangeID,
sub.UserID,
sub.TimeInForce
From     
     T_Sub as sub join T_Fills as fills on sub.ClOrderID = fills.clOrderid  
where sub.ClOrderID = @Root or sub.ParentClOrderID = @Root);   
End 