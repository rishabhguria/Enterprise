
--Atul Dislay (14/2/2015)  
--[Analysis] TT side preference  
--http://jira.nirvanasolutions.com:8080/browse/PRANA-5330  
CREATE PROCEDURE [dbo].[P_SaveTradingTicketPrefrencesUniversal] (
	@assetID VARCHAR(50)
	,@underlyingID VARCHAR(50)
	,@tradingAccountID VARCHAR(50)
	,@fundID VARCHAR(50)
	,@strategyID VARCHAR(50)
	,@brokerID VARCHAR(50)
	,@isDefaultCV VARCHAR(6)
	,@quantity VARCHAR(50)
	,@displayQuantity VARCHAR(50)
	,@quantityIncrement VARCHAR(50)
	,@priceLimitIncrement VARCHAR(50)
	,@stopPriceIncrement VARCHAR(50)
	,@pegOffset VARCHAR(50)
	,@discrOffset VARCHAR(50)
	,@counterPartyID VARCHAR(50)
	,@venueID VARCHAR(50)
	,@orderTypeID VARCHAR(50)
	,@executionInstructionID VARCHAR(50)
	,@handlingInstructionID VARCHAR(50)
	,@tIF VARCHAR(50)
	,@cVTradingAccountID VARCHAR(50)
	,@cVFundID VARCHAR(50)
	,@cVStrategyID VARCHAR(50)
	,@CVBorrowerFirmID VARCHAR(50)
	,@CompanyUserID INT
	,@CMTAID INT
	,@GiveUpID INT
	,@OrderSideID VARCHAR(50)
    ,@SettlCurrency INT
	,@IsQuantityDefaultValueChecked VARCHAR(6)
	)
AS
--Delete From T_TradingTicketPrefrencesSettings  
DECLARE @result INT

--Insert  
INSERT INTO T_TradingTicketPrefrencesSettings (
	AssetID
	,UnderlyingID
	,TradingAccountID
	,FundID
	,StrategyID
	,BrokerID
	,IsDefaultCV
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
	,CompanyUserID
	,CMTAID
	,GiveUpID
	,OrderSideID
	,SettlCurrency
	,IsQuantityDefaultValueChecked
	)
VALUES (
	@assetID
	,@underlyingID
	,@tradingAccountID
	,@fundID
	,@strategyID
	,@brokerID
	,@isDefaultCV
	,@quantity
	,@displayQuantity
	,@quantityIncrement
	,@priceLimitIncrement
	,@stopPriceIncrement
	,@pegOffset
	,@discrOffset
	,@counterPartyID
	,@venueID
	,@orderTypeID
	,@executionInstructionID
	,@handlingInstructionID
	,@tIF
	,@cVTradingAccountID
	,@cVFundID
	,@cVStrategyID
	,@CVBorrowerFirmID
	,@CompanyUserID
	,@CMTAID
	,@GiveUpID
	,@OrderSideID
	,ISnull(@SettlCurrency,0)
	,@IsQuantityDefaultValueChecked
	)

SET @result = scope_identity()

SELECT @result

SELECT *
FROM T_TradingTicketPrefrencesSettings
