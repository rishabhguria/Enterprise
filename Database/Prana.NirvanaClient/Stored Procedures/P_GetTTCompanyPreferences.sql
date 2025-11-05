CREATE PROCEDURE [dbo].[P_GetTTCompanyPreferences] @id INT  
AS  
SELECT AssetID  
 ,SideID  
FROM T_CompanyTTAssetPreferences  
WHERE CompanyID = @id  
  
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
 ,IsShowTargetQTY
 ,FromControlToControlMapping
FROM T_CompanyTTGeneralPreferences  
WHERE CompanyID = @id