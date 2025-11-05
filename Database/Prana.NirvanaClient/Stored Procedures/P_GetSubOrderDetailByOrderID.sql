    
    

    
    
--select * from T_Sub    
    
    
    
  CREATE PROC [dbo].[P_GetSubOrderDetailByOrderID]    
(    
 @Root varchar(50)  
)    
AS    
BEGIN    
        
Select top 1   
  sub.AUECLocalDate,    
  orders.Symbol ,    
  orders.OrderSideTagValue,     
  sub.Quantity,    
  sub.OrderTypeTagValue,    
  'Stautus',    
  tif.TimeInForce,    
  execs.ExecutionInstructions,    
  hand.HandlingInstructions,    
  orders.ParentClOrderID,    
  orders.TradingAccountID,    
  TA.TradingAccountName,    
  sub.Price,    
orders.AUECID,     
sub.CounterPartyID,     
sub.VenueID,     
sub.DiscrOffset,     
sub.PegDiff,     
sub.PNP,     
sub.StopPrice,     
sub.Text,    
sub.OpenClose,    
orders.AUECID

From T_Order as orders     
    
join T_sub as sub on orders.Parentclorderid = sub.Parentclorderid  --  dbo.T_Sub  as sub    
     
--join T_OrderType as type on sub.OrderType = type.OrderTypeTagValue    
    
left join T_ExecutionInstructions execs on sub.ExecutionInst = execs.ExecutionInstructionsTagValue    
left join T_HandlingInstructions hand on sub.HandlingInst = hand.HandlingInstructionsTagValue    
join  dbo.T_CompanyTradingAccounts as TA on orders.TradingAccountID = TA.CompanyTradingAccountsID    
left join T_TimeInForce TIF on sub.TimeInForce = TIF.TimeInForceTagValue    
    
join T_CompanyUserTradingAccounts as CUA on orders.TradingAccountID=CUA.TradingAccountID    
    
join  dbo.T_CompanyUser as CU on CU.UserID = CUA.CompanyUserID    
    
--join T_AUEC as auec on auec.AUECID = orders.AUECID    
--join T_AUECExchange as exchange on auec.AUECExchangeID = exchange.AUECExchangeID    
--left join T_CountryFlag as flag on exchange.CountryFlagID = flag.CountryFlagID    
    
where 
orders.ParentClOrderID =  @root
ORDER BY sub.AUECLocalDate DESC

   
    
END 
--select Top 1 * from T_Sub  
--where
--T_Sub.UnderlyingSymbol = 'Goog' 
-- ORDER BY AUECLocalDate DESC
--
--
--SELECT Top 1 * FROM eDates WHERE eID=100 ORDER BY 'eDates' DESC
