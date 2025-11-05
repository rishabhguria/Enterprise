CREATE PROCEDURE [dbo].[P_TradingDataFinal]  
(   
  @AllAUECDatesString VARCHAR(max)    
 ,@isAllowMultidayStagedOrders BIT   
 ,@userID INT   
 )    
AS    
  
DECLARE @AUECDatesTable TABLE (  
 AUECID INT  
 ,CurrentAUECDate DATETIME  
 )  
  
DECLARE @ShowMasterFundOnTT INT  
 SELECT @ShowMasterFundOnTT=CONVERT(INT, PreferenceValue)          
 FROM T_PranaKeyValuePreferences          
 WHERE PreferenceKey = 'IsShowMasterFundonTT'  
  
INSERT INTO @AUECDatesTable  
SELECT *  
FROM dbo.GetAllAUECDatesFromString(@AllAUECDatesString)  
  
Select * into #V_LatestFillByOrderSeqNumber  
from V_LatestFillByOrderSeqNumber  
  
Select * into #V_OrderAUECDetails  
from V_OrderAUECDetails  
  
select distinct StagedOrderID as ParentCLorderId into #tempParentSubIds from T_Sub where NirvanaMsgType= '14'  
  
  
SELECT DATEDIFF(d, GETDATE(), sub.AUECLocalDate) AS datedifference    
  
 ,sub.ClOrderID    
  
 ,sub.ParentClOrderID    
  
 ,sub.Price    
  
 ,dbo.T_Order.Symbol    
  
 ,fill.OrderID    
  
 ,sub.ExecutionInst    
  
 ,sub.OrderTypeTagValue AS OrderType    
  
 ,sub.TimeInForce    
  
 ,ISNULL(fill.CounterPartyID, sub.CounterPartyID) AS CounterPartyID  
  
 ,sub.VenueID    
  
 ,sub.DiscrOffset    
  
 ,sub.PegDiff    
  
 ,sub.PNP    
  
 ,sub.StopPrice    
  
 ,sub.TargetCompID    
  
 ,sub.TargetSubID    
  
 ,sub.StagedOrderID    
  
 ,sub.NirvanaMsgType    
  
 ,sub.SecurityType    
  
 ,sub.OpenClose    
  
 ,CASE sub.MsgType  
  
   WHEN 'G'      
  
    THEN ISNULL(fill.Quantity, sub.Quantity)  
  
   ELSE sub.Quantity      
  
   END AS Quantity       
  
 ,sub.OrigClOrderID    
  
 ,ISNULL(fill.LastPx, 0) AS Lastpx    
  
 ,ISNULL(fill.AveragePrice, 0) AS AveragePrice  
  
 ,ISNULL(fill.LeavesQty, 0) AS LeavesQty    
  
 ,ISNULL(fill.CumQty, 0) AS CumQty    
  
 ,ISNULL(fill.OrderStatus, CASE sub.MsgType    
  
   WHEN 'F'    
  
    THEN '6'    
  
   WHEN 'G'    
  
    THEN 'E'    
  
   ELSE 'A'    
  
   END) AS OrderStatus    
  
 ,ISNULL(fill.LastShares, 0) AS LastShares    
  
 ,dbo.T_Order.ParentClOrderID AS Expr3    
  
 ,dbo.T_Order.AUECID    
  
 ,dbo.T_CompanyAUEC.CompanyAUECID    
  
 ,sub.UserID AS CompanyUserID  
  
 ,sub.OriginalUserID AS OriginalCompanyUserID  
  
 ,dbo.T_CompanyAUEC.AUECID AS Expr1    
  
 ,dbo.T_Order.TradingAccountID    
  
 ,sub.InsertionTime    
  
 ,#V_OrderAUECDetails.CurrencyID    
  
 ,#V_OrderAUECDetails.ExchangeID    
  
 ,#V_OrderAUECDetails.AssetID    
  
 ,#V_OrderAUECDetails.UnderLyingID    
  
 ,dbo.T_CountryFlag.CountryFlagImage    
  
 ,fill.NirvanaSeqNumber AS OrderSeqNumber    
  
 ,sub.ServerTime    
  
 ,dbo.T_CompanyUserAUEC.CompanyUserAUECID    
  
 ,ISNULL(fill.ExecutionID, '') AS Expr2    
  
 ,ISNULL(fill.MsgType, sub.MsgType) AS FillMsgType    
  
 ,sub.HandlingInst    
  
 ,dbo.T_Order.ListID    
  
 ,dbo.T_Order.OrderSidetagValue AS Side    
  
 ,sub.FundID    
  
 ,sub.StrategyID    
  
 ,sub.MsgType    
  
 ,sub.CMTA    
  
 ,sub.GiveUpID    
  
 ,sub.UnderlyingSymbol    
  
 ,sub.GiveUp    
  
 ,sub.AlgoStrategyID    
  
 ,sub.AlgoParameters    
  
 ,dbo.T_Order.OriginatorTypeID    
  
 ,sub.ClientOrderID    
  
 ,sub.AUECLocalDate    
  
 ,sub.SettlementDate    
  
 ,fill.SenderSubID    
  
 ,fill.AvgFxRateForTrade    
  
 ,sub.ProcessDate    
  
 ,dbo.T_Order.CalcBasis    
  
 ,dbo.T_Order.CommissionRate    
  
 ,fill.CommissionAmt    
  
 ,dbo.T_ImportFileLog.ImportFileName    
  
 ,dbo.T_ImportFileLog.ImportFileID    
  
 ,dbo.T_Order.SoftCommissionCalcBasis    
  
 ,dbo.T_Order.SoftCommissionRate    
  
 ,fill.SoftCommissionAmt    
  
 ,CASE WHEN @ShowMasterFundOnTT=1    THEN G.TradeAttribute6  
       WHEN  @ShowMasterFundOnTT=0   THEN sub.TradeAttribute6  
  END  
 ,sub.TradeAttribute5    
  
 ,sub.TradeAttribute4    
  
 ,sub.TradeAttribute3    
  
 ,sub.TradeAttribute2    
  
 ,sub.TradeAttribute1    
  
 ,sub.InternalComments    
  
 ,sub.SettlCurrency    
  
 ,sub.FxRate  
  
 ,sub.FxRateCalc    
  
  ,sub.BorrowerID  
  
  ,sub.BorrowBroker  
  
 ,sub.ShortRebate  
  
 ,sub.NirvanaLocateID  
  
 ,G.IsSwapped      
  
 ,Swap.NotionalValue    
  
 ,Swap.BenchMarkRate    
  
 ,Swap.[Differential]    
  
 ,Swap.OrigCostBasis    
  
 ,Swap.DayCount    
  
 ,Swap.SwapDescription    
  
 ,Swap.FirstResetDate    
  
 ,Swap.OrigTransDate    
  
 ,Swap.ResetFrequency    
  
 ,Swap.ClosingPrice    
  
 ,Swap.ClosingDate    
  
 ,Swap.TransDate    
  
 ,Swap.ShouldOverrideNotional    
  
 ,Swap.ShouldOverrideCostBasis    
  
 ,G.ChangeType    
  
 ,TradedOrders.Text    
  
 ,ISNULL(TradedOrders.[OriginalAllocationPreferenceID],0)  
  
 ,TradedOrders.TransactionSource  
  
 ,G.StateID  
  
 ,sub.IsHidden   
  
 ,case   
  WHEN @isAllowMultidayStagedOrders = 1 AND ((SELECT count(1) FROM T_MultiDayOrderAllocation WHERE CLOrderID = sub.ClOrderID) > 0)  
  THEN  
   SUBSTRING((  
     SELECT ',' + CAST(FA.FundID AS VARCHAR)  
     FROM T_FundAllocation FA  
     INNER JOIN T_MultiDayOrderAllocation MOA  
      ON FA.GroupID = MOA.GroupId  
     WHERE MOA.ClOrderID = sub.CLOrderID  
     FOR XML PATH('')  
     ), 2, 200000)  
  ELSE  
   SUBSTRING((  
     SELECT ',' + CAST(FA.FundID AS VARCHAR)  
     FROM T_FundAllocation FA  
     WHERE FA.GroupID = TradedOrders.GroupID  
     FOR XML PATH('')  
     ), 2, 200000)  
  END AS AccountIds  
  
,SUBSTRING((SELECT ',' + CAST(L2A.Level2ID AS VARCHAR)      
  
     FROM T_Level2Allocation L2A        
  
     WHERE L2A.GroupID = TradedOrders.GroupID        
  
     FOR XML PATH('')),2,200000) AS StrategyIds  
,G.AllocationSchemeID AS AllocationSchemeID   
,sub.IsManualOrder  
,fill.TransactTime  
,sub.ExpireTime  
,fill.DayCumQty  
,fill.DayAvgPx  
,sub.IsUseCustodianBroker  
,G.OriginalPurchaseDate
,sub.AdditionalTradeAttributes
FROM #V_LatestFillByOrderSeqNumber    
  
INNER JOIN dbo.T_Fills AS fill ON #V_LatestFillByOrderSeqNumber.OrderSeqNumber = fill.NirvanaSeqNumber    
  
RIGHT OUTER JOIN #V_OrderAUECDetails    
  
INNER JOIN dbo.T_Order    
  
INNER JOIN dbo.T_Sub AS sub     
  
 ON dbo.T_Order.ParentClOrderID = sub.ParentClOrderID     
  
 ON #V_OrderAUECDetails.ClOrderID = dbo.T_Order.ParentClOrderID     
  
INNER JOIN dbo.T_CompanyUserAUEC    
  
INNER JOIN dbo.T_CompanyAUEC     
  
 ON dbo.T_CompanyUserAUEC.CompanyAUECID = dbo.T_CompanyAUEC.CompanyAUECID     
  
 ON dbo.T_Order.AUECID = dbo.T_CompanyAUEC.AUECID    
  
 AND sub.UserID = dbo.T_CompanyUserAUEC.CompanyUserID     
  
LEFT OUTER JOIN dbo.T_CountryFlag     
  
 ON #V_OrderAUECDetails.CountryFlagID = dbo.T_CountryFlag.CountryFlagID     
  
 ON fill.ClOrderID = sub.ClOrderID     
  
LEFT OUTER JOIN dbo.T_ImportFileLog ON dbo.T_Order.ImportFileID = dbo.T_ImportFileLog.ImportFileID     
  
LEFT OUTER JOIN dbo.T_TradedOrders AS TradedOrders WITH (READPAST) ON dbo.T_Order.ParentClOrderID = TradedOrders.ParentClOrderID   

LEFT OUTER JOIN dbo.T_Group AS G WITH (READPAST) ON G.GroupID = TradedOrders.GroupID  
  
LEFT OUTER JOIN dbo.T_SwapParameters AS Swap WITH (READPAST) ON Swap.GroupID = G.GroupID  
  
INNER JOIN @AUECDatesTable AS AUECTable ON AUECTable.AUECID = T_Order.AUECID  
  
LEFT JOIN [T_CompanyAUECClearanceTimeBlotter] AS clearance WITH (NOLOCK) ON T_CompanyAUEC.CompanyAUECID = clearance.[CompanyAUECID]  
  
WHERE (  
  (  
   datediff(d, AUECTable.CurrentAUECDate, sub.AUECLocalDate) = 0  
   AND (dbo.TimeOnlyDiff(ClearanceTime, sub.AUECLocalDate) = - 1)  
   )  
  OR (  
   datediff(d, AUECTable.CurrentAUECDate, sub.AUECLocalDate) = 0  
   AND (dbo.TimeOnlyDiff(ClearanceTime, CurrentAUECDate) = 1)  
   )  
  OR (  
   datediff(d, AUECTable.CurrentAUECDate, sub.AUECLocalDate) = - 1  
   AND dbo.TimeOnlyDiff(ClearanceTime, sub.AUECLocalDate) = - 1  
   AND dbo.timeonlyDiff(CurrentAUECDate, ClearanceTime) = - 1  
   )  
  OR (  
   @isAllowMultidayStagedOrders = 1  
   AND (  
    sub.NirvanaMsgType = 3  
    OR IsNull(StagedOrderID, '') <> ''  
    OR sub.TimeInForce in (1,6)  
    or sub.ParentClOrderID in (select ParentClOrderID from #tempParentSubIds)  
    or sub.StagedOrderID in (select ParentClOrderID from #tempParentSubIds)  
    )  
   )  
  )  
 AND IsHidden = 0  
 AND T_Order.ListID = ''  
 AND T_Order.tradingaccountid IN (  
  SELECT TradingAccountID  
  FROM T_CompanyUserTradingAccounts WITH (NOLOCK)  
  WHERE (CompanyUserID = @userID or @userID is null)  
  )  
  
  
drop table #V_LatestFillByOrderSeqNumber  
drop table #V_OrderAUECDetails,#tempParentSubIds