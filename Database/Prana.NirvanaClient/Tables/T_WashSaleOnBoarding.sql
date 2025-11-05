-- [T_WashSaleOnBoarding] table for storing wash sale trades info.

CREATE TABLE [dbo].[T_WashSaleOnBoarding]
(
	[TaxlotID] VARCHAR(50) NOT NULL PRIMARY KEY,
	[TypeofTransaction] VARCHAR(50) NULL,
	[TradeDate] DateTime NOT NULL,
	[OriginalPurchaseDate] DATETIME NOT NULL,
	[Account] VARCHAR(100) NOT NULL,
	[Side] VARCHAR(50) NOT NULL,
	[Asset] VARCHAR(50) NOT NULL,
	[Currency] VARCHAR(50) NOT NULL,
	[Broker] VARCHAR(50) NOT NULL,
	[Symbol] VARCHAR(100) NOT NULL,
	[BloombergSymbol] VARCHAR(100) NOT NULL,
	[CUSIP] VARCHAR(50) NOT NULL,
	[Issuer] VARCHAR(100) NOT NULL,
	[UnderlyingSymbol] VARCHAR(100) NOT NULL,
	[Quantity] FLOAT(53) NOT NULL,
	[UnitCostLocal] FLOAT(53) NOT NULL,
	[TotalCostLocal] FLOAT(53) NOT NULL,
	[TotalCost] FLOAT(53) NOT NULL,
	[WashSaleAdjustedRealizedLoss] VARCHAR(200),
	[WashSaleAdjustedHoldingsPeriod] INT ,
	[WashSaleAdjustedCostBasis] VARCHAR(200),
	[WashSaleAdjustedHoldingsStartDate] DateTime

)
