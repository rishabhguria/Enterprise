


-- =============================================        
-- Author:  Ashish Poddar        
-- Create date: 18th May, 2007        
-- Description: To get summary of orders by date in one single row.         
-- =============================================        
CREATE PROCEDURE [dbo].[P_GetOrderSummarybyDate] 
 @lowerdate datetime ,        
 @companyUserID int,        
 @upperDate datetime        
AS        
BEGIN        
        
select         
 VTO.ParentClOrderID,        
 VTO.OrderSideName,        
 VTO.OrderSideTagValue,        
 VTO.OrderTypeName,        
 VTO.OrderTypeTagValue,        
 VTO.CounterPartyID,        
 VTO.CounterPartyName,        
 VTO.VenueID,        
 VTO.VenueName,        
 VTO.Symbol,        
 VTO.TradingAccountID,        
 VTO.TradingAccountName,        
 VTO.AvgPrice,        
 VTO.Quantity,        
 VTO.CumQty,        
 VTO.Price,        
 VTO.OrderStatus,        
 VTO.TransactTime,        
 VTO.AssetID,        
 VTO.AssetName,        
 VTO.UnderLyingID,        
 VTO.UnderLyingName,        
 VTO.OpenClose,        
 VTO.AUECID,  
 VTO.OriginalUserID,
 VTO.ProcessDate,
 VTO.FundID,
 VTO.ModifiedUserID,  
 VTO.CurrentUser,
 VTO.OrderID,
 VTO.TimeInForce,
 VTO.StagedOrderID
        
from V_TradedOrders as VTO      
    
inner join  T_CompanyUserTradingAccounts on T_CompanyUserTradingAccounts.TradingAccountID = VTO.TradingAccountID    
where     
T_CompanyUserTradingAccounts.CompanyUserID  = @companyUserID         
      
and VTO.NirvanaMsgType != 3 -- Exclude Staged Orders        
and datediff(d,VTO.AuecLocalDate,@lowerdate) <= 0     
and datediff(d,VTO.AuecLocalDate,@upperDate) >= 0 

order by ParentClOrderID 
        
END        

