CREATE PROCEDURE [dbo].[P_SaveWashSaleTaxlots]
(@xml NTEXT,
@fundIDs VARCHAR(max),
@assetIDs VARCHAR(max),
@currencyIDs VARCHAR(max))
AS
-------------------------------------------------
--Gets all the fundID and store in temp table--
CREATE TABLE #funds (fundID INT)

INSERT INTO #funds
SELECT Items
FROM dbo.Split(@fundIDs, ',')
--------------------------------------------------
--Gets all the asset and store in temp table--
CREATE TABLE #assetClass (assetID INT)

INSERT INTO #assetClass
SELECT Items
FROM dbo.Split(@assetIDs, ',')
--------------------------------------------------
--Gets all the currency and store in temp table--
CREATE TABLE #currencies (currID INT)

INSERT INTO #currencies
SELECT Items
FROM dbo.Split(@currencyIDs, ',')
-------------------------------------------------
--Delets all the rows which satisfy the condition
DELETE FROM T_WashSaleOnBoarding
WHERE 
T_WashSaleOnBoarding.Account in  (Select T_CompanyFunds.FundName from T_CompanyFunds WHERE T_CompanyFunds.CompanyFundID in (Select fundID from #funds)) AND
(T_WashSaleOnBoarding.Asset in  (Select T_Asset.AssetName from T_Asset WHERE T_Asset.AssetID in (Select assetID from #assetClass)) OR
(T_WashSaleOnBoarding.Asset = 'EquitySwap' AND ((Select COUNT(*) from T_Asset) + 1 in (Select assetID from #assetClass)))) AND
T_WashSaleOnBoarding.Currency in  (Select T_Currency.CurrencySymbol from T_Currency WHERE T_Currency.CurrencyID in (Select currID from #currencies))
-------------------------------------------------

DECLARE @handle INT 
Create TABLE #WashSaleTaxlot(
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
	[WashSaleAdjustedRealizedLoss] VARCHAR(200) ,
	[WashSaleAdjustedHoldingsPeriod] INT ,
	[WashSaleAdjustedCostBasis] VARCHAR(200),
	[WashSaleAdjustedHoldingsStartDate] DateTime
	)
	EXEC sp_xml_preparedocument @handle OUTPUT
	,@xml
	INSERT INTO #WashSaleTaxlot(
	     TaxlotID,
		 TypeofTransaction,
		 TradeDate,
		 OriginalPurchaseDate,
		 Account,
		 Side,
		 Asset,
		 Currency,
		 Broker,
		 Symbol,
		 BloombergSymbol,
		 CUSIP,
		 Issuer,
		 UnderlyingSymbol,
		 Quantity,
		 UnitCostLocal,
		 TotalCostLocal,
		 TotalCost,
		 WashSaleAdjustedRealizedLoss,
		 WashSaleAdjustedHoldingsPeriod,
		 WashSaleAdjustedCostBasis,
		 WashSaleAdjustedHoldingsStartDate
	)
	SELECT * FROM OPENXML (@handle, '//T_WashSaleOnBoarding', 2) WITH(

	TaxlotID VARCHAR(50) ,
	[TypeofTransaction] VARCHAR(50),
	[TradeDate] DateTime ,
	[OriginalPurchaseDate] DATETIME,
	[Account] VARCHAR(100) ,
	[Side] VARCHAR(50) ,
	[Asset] VARCHAR(50) ,
	[Currency] VARCHAR(50) ,
	[Broker] VARCHAR(50) ,
	[Symbol] VARCHAR(100) ,
	[BloombergSymbol] VARCHAR(100) ,
	[CUSIP] VARCHAR(50) ,
	[Issuer] VARCHAR(100),
	[UnderlyingSymbol] VARCHAR(100),
	[Quantity] FLOAT(53),
	[UnitCostLocal] FLOAT(53),
	[TotalCostLocal] FLOAT(53),
	[TotalCost] FLOAT(53),
	[WashSaleAdjustedRealizedLoss] VARCHAR(200),
	[WashSaleAdjustedHoldingsPeriod] INT ,
	[WashSaleAdjustedCostBasis] VARCHAR(200),
	[WashSaleAdjustedHoldingsStartDate] DateTime
	)

	  Update T_WashSaleOnBoarding
      Set WashSaleAdjustedRealizedLoss = T.WashSaleAdjustedRealizedLoss,
      WashSaleAdjustedHoldingsPeriod = T.WashSaleAdjustedHoldingsPeriod,
	  WashSaleAdjustedCostBasis = T.WashSaleAdjustedCostBasis,
	  WashSaleAdjustedHoldingsStartDate= T.WashSaleAdjustedHoldingsStartDate
	  From T_WashSaleOnBoarding inner join #WashSaleTaxlot  AS T
	  ON T.TaxlotID = T_WashSaleOnBoarding.TaxlotID

      INSERT INTO T_WashSaleOnBoarding       
	  SELECT * FROM #WashSaleTaxlot AS T
      WHERE NOT EXISTS(SELECT *
      FROM T_WashSaleOnBoarding
      WHERE TaxlotID = T.TaxlotID)

Drop table #funds
Drop table #assetClass
Drop table #currencies
