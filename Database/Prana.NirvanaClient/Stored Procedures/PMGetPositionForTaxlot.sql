-------------------------------------------------------------------------------------------------------------------------
--Modified By: Disha Sharma
--Date: 10/29/2015
--Description: Change the order of accessing dtabase objects while getiing data to remove deadlock condition, PRANA-11339
-------------------------------------------------------------------------------------------------------------------------
CREATE PROC  [dbo].[PMGetPositionForTaxlot]                
(                
@taxlotID varchar(max)                
)                
AS        
BEGIN
          
DECLARE @TaxLotIDtable table          
(          
taxlotID varchar(100)          
)

INSERT INTO @TaxLotIDtable
	SELECT
		CAST(Items AS varchar(100))
	FROM dbo.split(@taxlotID, ',')

SELECT
	PT.TaxLotID AS TaxLotID,
	G.AUECLocalDate AS TradeDate,
	G.ProcessDate,
	G.OriginalPurchaseDate,
	PT.OrderSideTagValue AS SideID, -- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS
	PT.Symbol AS Symbol,
	PT.TaxLotOpenQty AS TaxLotOpenQty,
	PT.AvgPrice AS AvgPrice,
	PT.FundID AS FundID,
	G.AssetID AS AssetID,
	G.UnderLyingID AS UnderLyingID,
	G.ExchangeID AS ExchangeID,
	G.CurrencyID AS CurrencyID,
	G.AUECID AS AUECID,
	PT.OpenTotalCommissionandFees, --this is open commission and closed commission sum is not necessarily equals to total commission        
	ISNULL(V_SecMasterData.Multiplier, 1) AS Multiplier,
	G.SettlementDate AS SettlementDate,
	NULL,
	NULL,
	ISNULL(V_SecMasterData.ExpirationDate, '1/1/1800') AS ExpirationDate,
	G.Description AS Description,
	PT.Level2ID AS Level2ID,
	ISNULL((PT.TaxLotOpenQty * SW.NotionalValue / G.Quantity), 0) AS NotionalValue,
	ISNULL(SW.BenchMarkRate, 0) AS BenchMarkRate,
	ISNULL(SW.Differential, 0) AS Differential,
	ISNULL(SW.OrigCostBasis, 0) AS OrigCostBasis,
	ISNULL(SW.DayCount, 0) AS DayCount,
	ISNULL(SW.SwapDescription, '') AS SwapDescription,
	SW.FirstResetDate AS FirstResetDate,
	SW.OrigTransDate AS OrigTransDate,
	G.IsSwapped AS IsSwapped,
	G.AUECLocalDate AS AUECLocalDate,
	G.GroupID,
	PT.PositionTag,
	G.FXRate,
	G.FXConversionMethodOperator,
	ISNULL(V_SecMasterData.CompanyName, '') AS CompanyName,
	ISNULL(V_SecMasterData.UnderlyingSymbol, '') AS UnderlyingSymbol,
	ISNULL(V_SecMasterData.Delta, 1) AS Delta,
	ISNULL(V_SecMasterData.PutOrCall, '') AS PutOrCall,
	G.IsPreAllocated,
	G.CumQty,
	G.AllocatedQty,
	G.Quantity,
	PT.taxlot_Pk,
	PT.ParentRow_Pk,
	V_SecMasterData.StrikePrice,
	ISNULL(V_SecMasterData.IsNDF, 0) AS IsNDF,
	ISNULL(V_SecMasterData.FixingDate, '1/1/1800') AS FixingDate,
	V_SecMasterData.LeadCurrencyID,
	V_SecMasterData.VsCurrencyID,
	PT.LotId,
	PT.ExternalTransId,
	PT.TradeAttribute1,
	PT.TradeAttribute2,
	PT.TradeAttribute3,
	PT.TradeAttribute4,
	PT.TradeAttribute5,
	PT.TradeAttribute6,
	L2.TaxLotQty AS ExecutedQty,
	G.TransactionType,
	G.InternalComments,
	G.TradingAccountId,
	G.UserID,
	G.NirvanaProcessDate,
	G.SettlCurrency,
	ISNULL(V_SecMasterData.BloombergSymbol,'') AS BloombergSymbol,
	ISNULL(V_SecMasterData.SEDOLSymbol,'') AS SEDOLSymbol,
	ISNULL(V_SecMasterData.CUSIPSymbol,'') AS CUSIPSymbol,
	ISNULL(V_SecMasterData.ISINSymbol,'') AS ISINSymbol,
	ISNULL(V_SecMasterData.SecurityTypeName,'') AS SecurityTypeName,
	ISNULL(V_SecMasterData.CountryName,'') AS CountryName,
	ISNULL(V_SecMasterData.ProxySymbol,'') AS ProxySymbol,
	ISNULL(V_SecMasterData.IDCOSymbol,'') AS IDCOSymbol,
	ISNULL(V_SecMasterData.OSISymbol,'') AS OSISymbol,
	ISNULL(V_SecMasterData.SectorName,'') AS SectorName,
	ISNULL(V_SecMasterData.SubSectorName,'') AS SubSectorName,
	ISNULL(V_SecMasterData.ReutersSymbol,'') AS RIC,
	ISNULL(V_SecMasterData.AssetName,'') AS UserAsset,
	PT.AdditionalTradeAttributes
FROM T_Group G WITH (NOLOCK)
INNER JOIN T_Level2Allocation L2 WITH (NOLOCK)
	ON L2.GroupID = G.GroupID
INNER JOIN PM_Taxlots PT WITH (NOLOCK)
	ON PT.TaxlotID = L2.TaxlotID
LEFT OUTER JOIN T_SwapParameters SW WITH (NOLOCK)
	ON G.GroupID = SW.GroupID
LEFT OUTER JOIN V_SecMasterData WITH (NOLOCK)
	ON PT.Symbol = V_SecMasterData.TickerSymbol
WHERE taxlot_PK IN (SELECT
	MAX(taxlot_pk)
FROM PM_Taxlots WITH (NOLOCK)
INNER JOIN @TaxLotIDtable Tax
	ON Tax.TaxlotID = PM_Taxlots.taxlotID
GROUP BY PM_Taxlots.TaxlotID)
END