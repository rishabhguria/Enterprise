                     
CREATE  procedure [dbo].[P_GetOrdersbyDate] (                               
@lowerdate datetime,                
@UserID int,                
@upperdate datetime                
)                 
                
AS                
BEGIN                
            
Select VT.AUECLocalDate,            
VT.Symbol,            
VT.OrderSideTagValue,            
VT.Quantity,            
VT.OrderTypeTagValue,            
'Status',            
VT.TimeInForce,            
VT.ExecutionInst,            
REPLACE(VT.HandlingInst, CHAR(0), '') HandlingInst,            
VT.ParentClOrderID,            
VT.TradingAccountID,            
VT.TradingAccountName,            
VT.AvgPrice,            
VT.AUECID,            
VT.CounterPartyID,            
VT.VenueID,            
--VT.DiscrOffset,                 
--VT.PegDiff,                 
--VT.PNP,                 
--VT.StopPrice,                 
VT.Text,                
VT.AssetID,                
VT.AssetName,                
VT.UnderLyingID,                
VT.OpenClose,                
VT.AUECID,        
VT.AvgFxRateForTrade,      
VT.NirvanaMsgType,    
VT.OriginalUserID,  
VT.ProcessDate,
VT.FundID,  
VT.ModifiedUserID,
VT.CurrentUser,
VT.OrderID,
VT.StagedOrderID
    
from V_TradedOrders  as VT            
inner join  T_CompanyUserTradingAccounts on T_CompanyUserTradingAccounts.TradingAccountID = VT.TradingAccountID          
where           
T_CompanyUserTradingAccounts.CompanyUserID  = @UserID               
--VT.UserID = @UserID          
and VT.NirvanaMsgType != 3             
and datediff(d,VT.AuecLocalDate,@lowerdate) <= 0                
and datediff(d,VT.AuecLocalDate,@upperdate) >= 0
order by VT.ParentClOrderID 
END            
          
          
--select * from T_CompanyUserTradingAccounts join V_TradedOrders on T_CompanyUserTradingAccounts.CompanyUserID = V_TradedOrders.UserID 

