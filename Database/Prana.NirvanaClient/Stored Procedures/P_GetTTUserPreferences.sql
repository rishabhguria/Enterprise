CREATE PROCEDURE [dbo].[P_GetTTUserPreferences] @id INT  
AS  
SELECT AssetID  
 ,SideID  
FROM T_UserTTAssetPreferences  
WHERE UserID = @id  
  
SELECT CounterPartyID  
 ,VenueID  
 ,OrderTypeID  
 ,TimeInForceID  
 ,ExecutionInstructionID  
 ,HandlingInstructionID  
 ,TradingAccountID
 ,StrategyID  
 ,AccountID  
 ,IsSettlementCurrencyBase  
 ,Quantity  
 ,IncrementOnQty  
 ,IncrementOnStop  
 ,IncrementOnLimit
 ,QuantityType
 ,IsUseRoundLots
 
FROM T_UserTTGeneralPreferences  
WHERE UserID = @id  