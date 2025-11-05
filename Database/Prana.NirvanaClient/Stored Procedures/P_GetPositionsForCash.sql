--CREATE PROCEDURE [dbo].[P_GetPositionsForCash]
--	@param1 int = 0,
--	@param2 int
--AS
--	SELECT @param1, @param2
--RETURN 0

CREATE PROCEDURE [dbo].[P_GetPositionsForCash] (  
  @YesterDayDATE DATETIME
 ,@AssetIds VARCHAR(MAX)  
 ,@FundIds VARCHAR(MAX)  
 ,@Symbols VARCHAR(MAX) 
 ,@ReconDateType INT = 1
 )  
AS  
BEGIN  
 SET NOCOUNT ON  
  
 DECLARE @Local_AssetIds VARCHAR(MAX)  
 DECLARE @Local_FundIds VARCHAR(MAX)  
 DECLARE @Local_Symbols VARCHAR(MAX)  

 SET @Local_AssetIds = @AssetIds  
 SET @Local_FundIds = @FundIds  
 SET @Local_Symbols = @Symbols  
 
 CREATE TABLE #AssetClass (AssetID INT)  
  
 IF (  
   @Local_AssetIds IS NULL  
   OR @Local_AssetIds = ''  
   )  
  INSERT INTO #AssetClass  
  SELECT AssetID  
  FROM T_Asset  
 ELSE  
  INSERT INTO #AssetClass  
  SELECT Items AS AssetID  
  FROM dbo.Split(@Local_AssetIds, ',')  
  
 CREATE TABLE #Funds (FundID INT)  
  
 IF (  
   @Local_FundIds IS NULL  
   OR @Local_FundIds = ''  
   )  
  INSERT INTO #Funds  
  SELECT CompanyFundID AS FundID  
  FROM T_CompanyFunds  
  Where IsActive=1 
 ELSE  
  INSERT INTO #Funds  
  SELECT Items AS FundID  
  FROM dbo.Split(@Local_FundIds, ',')  
  
 CREATE TABLE #Symbols (Symbol VARCHAR(100))  
  
 IF (  
   @Local_Symbols IS NOT NULL  
   AND @Local_Symbols <> ''  
   )  
  INSERT INTO #Symbols  
  SELECT ITEMS AS Symbol  
		FROM dbo.Split(@Local_Symbols, ',') -- Get UTC Date. UTC Date is stored corresponding to AUECID 0 in @AUECDatesTable      
  
 SELECT 
   DateAdd(Day,1,@YesterDayDATE) as RunDate
  ,PT.TaxLotID AS TaxLotID  
  ,G.AUECLocalDate AS TradeDate  
  ,PT.OrderSideTagValue AS SideID  
  ,PT.Symbol AS Symbol  
  ,PT.TaxLotOpenQty AS TaxLotOpenQty  
  ,PT.AvgPrice AS AvgPrice  
  ,PT.FundID AS FundID  
  ,G.AssetID AS AssetID  
  ,G.UnderLyingID AS UnderLyingID  
  ,G.ExchangeID AS ExchangeID  
  ,G.CurrencyID AS CurrencyID  
  ,G.AUECID AS AUECID  
  ,PT.OpenTotalCommissionandFees  
  ,--this is open commission and closed commission sum is not necessarily equals to total commission        
   isnull(V_SecMasterData.Multiplier, 1) AS Multiplier  
  ,G.SettlementDate AS SettlementDate  
  ,V_SecMasterData.LeadCurrencyID  
  ,V_SecMasterData.VsCurrencyID  
  ,isnull(V_SecMasterData.ExpirationDate, 1 / 1 / 1800) AS ExpirationDate  
  ,G.Description AS Description  
  ,PT.Level2ID AS Level2ID  
  ,isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty), 0) AS NotionalValue  
  ,isnull(SW.BenchMarkRate, 0) AS BenchMarkRate  
  ,isnull(SW.Differential, 0) AS Differential  
  ,isnull(SW.OrigCostBasis, 0) AS OrigCostBasis  
  ,isnull(SW.DayCount, 0) AS DayCount  
  ,isnull(SW.SwapDescription, '') AS SwapDescription  
  ,SW.FirstResetDate AS FirstResetDate  
  ,SW.OrigTransDate AS OrigTransDate  
  ,G.IsSwapped AS IsSwapped  
  ,G.AllocationDate AS AUECLocalDate  
  ,G.GroupID  
  ,PT.PositionTag  
  ,IsNull(PT.FXRate, G.FXRate) AS FXRate  
  ,IsNull(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) AS FXConversionMethodOperator  
  ,isnull(V_SecMasterData.CompanyName, '') AS CompanyName  
  ,isnull(V_SecMasterData.UnderlyingSymbol, '') AS UnderlyingSymbol  
  ,IsNull(V_SecMasterData.Delta, 1) AS Delta  
  ,IsNull(V_SecMasterData.PutOrCall, '') AS PutOrCall  
  ,G.IsPreAllocated  
  ,G.CumQty  
  ,G.AllocatedQty  
  ,G.Quantity  
  ,PT.taxlot_Pk  
  ,PT.ParentRow_Pk  
  ,IsNull(V_SecMasterData.StrikePrice, 0) AS StrikePrice  
  ,G.UserID  
  ,G.CounterPartyID  
  ,CATaxlots.CorpActionID  
  ,V_SecMasterData.Coupon  
  ,V_SecMasterData.IssueDate  
  ,V_SecMasterData.MaturityDate  
  ,V_SecMasterData.FirstCouponDate  
  ,V_SecMasterData.CouponFrequencyID  
  ,V_SecMasterData.AccrualBasisID  
  ,V_SecMasterData.BondTypeID  
  ,V_SecMasterData.IsZero  
  ,G.ProcessDate  
  ,G.OriginalPurchaseDate  
  ,V_SecMasterData.IsNDF  
  ,V_SecMasterData.FixingDate  
  ,V_SecMasterData.IDCOSymbol  
  ,V_SecMasterData.OSISymbol  
  ,V_SecMasterData.SEDOLSymbol  
  ,V_SecMasterData.CUSIPSymbol  
  ,V_SecMasterData.BloombergSymbol  
  ,MF.MasterFundName AS MasterFund  
  ,V_SecMasterData.UnderlyingDelta  
  ,V_SecMasterData.ISINSymbol  
  ,PT.LotId  
  ,PT.ExternalTransId  
  ,PT.TradeAttribute1  
  ,PT.TradeAttribute2  
  ,PT.TradeAttribute3  
  ,PT.TradeAttribute4  
  ,PT.TradeAttribute5  
  ,PT.TradeAttribute6  
  ,V_SecMasterData.ProxySymbol  
  ,--Added UDA fields by OMshiv, Nov 2013        
   V_SecMasterData.AssetName  
  ,V_SecMasterData.SecurityTypeName  
  ,V_SecMasterData.SectorName  
  ,V_SecMasterData.SubSectorName  
  ,V_SecMasterData.CountryName  
  ,V_SecMasterData.BBGID  
  ,G.TransactionType  
  ,L2.TaxLotQty AS ExecutedQty  
  ,CATaxlotsForClosing.ClosingTaxlotId  
  ,V_SecMasterData.ReutersSymbol  
  ,G.InternalComments  
  ,COALESCE(PTCUR.CurrencySymbol, GCUR.CurrencySymbol, 'None') AS SettlCurrency  
  ,V_SecMasterData.IsCurrencyFuture  
  ,V_SecMasterData.Symbol_PK 
  ,PT.AdditionalTradeAttributes
 FROM PM_Taxlots PT  
 INNER JOIN T_Level2Allocation L2  
  ON L2.TaxLotID = PT.TaxLotID  
 LEFT OUTER JOIN #Symbols  
  ON (  
   PT.Symbol = #Symbols.Symbol  
   AND @Local_Symbols <> ''  
   AND @Local_Symbols IS NOT NULL  
   )  
 INNER JOIN #Funds  
  ON PT.FundID = #Funds.FundID  
 INNER JOIN T_Group G  
  ON G.GroupID = PT.GroupID  
 INNER JOIN #AssetClass  
  ON G.AssetID = #AssetClass.AssetID  
 LEFT OUTER JOIN T_SwapParameters SW  
  ON G.GroupID = SW.GroupID  
 LEFT OUTER JOIN V_SecMasterData  
  ON PT.Symbol = V_SecMasterData.TickerSymbol  
 LEFT OUTER JOIN PM_CorpActionTaxlots CATaxlots  
  ON PT.Taxlot_PK = CATaxlots.FKId  
 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF  
  ON CMF.CompanyFundID = #Funds.FundID  
 LEFT OUTER JOIN T_companyMasterFunds MF  
  ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID  
 LEFT OUTER JOIN T_Currency PTCUR  
  ON PT.SettlCurrency = PTCUR.CurrencyID   
 LEFT OUTER JOIN T_Currency GCUR  
  ON G.SettlCurrency = GCUR.CurrencyID  
 LEFT OUTER JOIN PM_CorpActionTaxlots CATaxlotsForClosing  
  ON PT.TaxLotID = CATaxlotsForClosing.TaxlotId  
  AND CATaxlotsForClosing.ClosingTaxlotId IS NOT NULL  
  AND PT.TaxLotOpenQty <> 0  
 WHERE (  
   (  
    @Local_Symbols <> ''  
    AND @Local_Symbols IS NOT NULL  
    AND #Symbols.Symbol IS NOT NULL  
    )  
   OR (@Local_Symbols = '')  
   OR (@Local_Symbols IS NULL)  
   )  
  AND taxlot_PK IN 
(  
   SELECT MAX(taxlot_PK)  
   FROM PM_Taxlots  
   WHERE  Datediff(d, PM_Taxlots.AUECModifiedDate, @YesterDayDATE) >= 0
   GROUP BY taxlotid  
 )  
  AND TaxLotOpenQty <> 0 
 
  ORDER BY taxlotid  
  
 DROP TABLE #Symbols  
  ,#AssetClass  
  ,#Funds  
  
 RETURN;  
END