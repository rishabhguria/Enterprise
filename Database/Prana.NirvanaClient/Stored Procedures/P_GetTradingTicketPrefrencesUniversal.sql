
--Atul Dislay (14/2/2015)
--[Analysis] TT side preference
--http://jira.nirvanasolutions.com:8080/browse/PRANA-5330
CREATE PROCEDURE [dbo].[P_GetTradingTicketPrefrencesUniversal] (@companyUserID INT)
AS
SELECT AssetID
	,UnderlyingID
	,TradingAccountID
	,FundID
	,StrategyID
	,BrokerID
	,ISDefaultCV
	,Quantity
	,DisplayQuantity
	,QuantityIncrement
	,PriceLimitIncrement
	,StopPriceIncrement
	,PegOffset
	,DiscrOffset
	,CounterPartyID
	,VenueID
	,OrderTypeID
	,ExecutionInstructionID
	,HandlingInstructionID
	,TIF
	,CVTradingAccountID
	,CVFundID
	,CVStrategyID
	,CVBorrowerFirmID
	,CMTAID
	,GiveUpID
	,OrderSideID
    ,SettlCurrency
	,IsQuantityDefaultValueChecked
FROM T_TradingTicketPrefrencesSettings
WHERE CompanyUserID = @companyUserID
	--select AssetID,UnderlyingID,CounterPartyID,VenueID,CompanyUserID,FundID,CVFundID FROM T_TradingTicketPrefrencesSettings
	--select * FROM T_TradingTicketPrefrencesSettings
