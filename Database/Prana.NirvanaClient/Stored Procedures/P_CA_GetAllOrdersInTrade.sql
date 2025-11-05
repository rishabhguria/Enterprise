CREATE PROC [dbo].[P_CA_GetAllOrdersInTrade] (        
 @AllAUECDatesString VARCHAR(max)        
 --,@userID INT        
 ,@isAllowMultidayStagedOrders BIT     
 )        
AS        
DECLARE @AUECDatesTable TABLE (        
 AUECID INT        
 ,CurrentAUECDate DATETIME        
 )        
        
INSERT INTO @AUECDatesTable        
SELECT *        
FROM dbo.GetAllAUECDatesFromString(@AllAUECDatesString)        
        
--DECLARE @UserAllowedTradingAccounts TABLE (        
-- TradingAccountID INT        
-- ,CompanyUserID INT        
-- )        
        
--INSERT INTO @UserAllowedTradingAccounts        
--SELECT TradingAccountID        
-- ,CompanyUserID        
--FROM T_CompanyUserTradingAccounts        
--WHERE CompanyUserID = @userID       
   
CREATE TABLE #TradingDataFinal (  
 [datedifference] [int] NULL  
 ,[ClOrderID] [varchar](50) NOT NULL  
 ,[ParentClOrderID] [varchar](50) NOT NULL  
 ,[Price] [float] NULL  
 ,[Symbol] [varchar](100) NULL  
 ,[OrderID] [varchar](500) NULL  
 ,[ExecutionInst] [varchar](50) NULL  
 ,[OrderType] [varchar](3) NULL  
 ,[TimeInForce] [varchar](3) NULL  
 ,[CounterPartyID] [int] NULL  
 ,[VenueID] [int] NULL  
 ,[DiscrOffset] [float] NULL  
 ,[PegDiff] [varchar](50) NULL  
 ,[PNP] [int] NULL  
 ,[StopPrice] [float] NULL  
 ,[TargetCompID] [varchar](50) NULL  
 ,[TargetSubID] [varchar](50) NULL  
 ,[StagedOrderID] [varchar](50) NULL  
 ,[NirvanaMsgType] [int] NULL  
 ,[SecurityType] [varchar](20) NULL  
 ,[OpenClose] [varchar](3) NULL  
 ,[Quantity] [float] NULL  
 ,[OrigClOrderID] [varchar](50) NULL  
 ,[Lastpx] [float] NOT NULL  
 ,[AveragePrice] [float] NOT NULL  
 ,[LeavesQty] [float] NOT NULL  
 ,[CumQty] [float] NOT NULL  
 ,[OrderStatus] [varchar](3) NOT NULL  
 ,[LastShares] [float] NOT NULL  
 ,[Expr3] [varchar](50) NOT NULL  
 ,[AUECID] [int] NULL  
 ,[CompanyAUECID] [int] NOT NULL  
 ,[CompanyUserID] [int] NOT NULL  
 ,[OriginalCompanyUserID] [int] NULL  
 ,[Expr1] [int] NOT NULL  
 ,[TradingAccountID] [int] NULL  
 ,[InsertionTime] [datetime] NULL  
 ,[CurrencyID] [int] NULL  
 ,[ExchangeID] [int] NOT NULL  
 ,[AssetID] [int] NOT NULL  
 ,[UnderLyingID] [int] NOT NULL  
 ,[CountryFlagImage] [image] NULL  
 ,[OrderSeqNumber] [bigint] NULL  
 ,[ServerTime] [varchar](50) NULL  
 ,[CompanyUserAUECID] [int] NOT NULL  
 ,[Expr2] [varchar](50) NOT NULL  
 ,[FillMsgType] [varchar](10) NULL  
 ,[HandlingInst] [varchar](3) NULL  
 ,[ListID] [varchar](50) NULL  
 ,[Side] [varchar](3) NULL  
 ,[FundID] [int] NULL  
 ,[StrategyID] [int] NULL  
 ,[MsgType] [varchar](3) NULL  
 ,[CMTA] [varchar](20) NULL  
 ,[GiveUpID] [int] NULL  
 ,[UnderlyingSymbol] [varchar](100) NULL  
 ,[GiveUp] [varchar](20) NULL  
 ,[AlgoStrategyID] [varchar](50) NULL  
 ,[AlgoParameters] [varchar](500) NULL  
 ,[OriginatorTypeID] [int] NULL  
 ,[ClientOrderID] [varchar](50) NOT NULL  
 ,[AUECLocalDate] [datetime] NULL  
 ,[SettlementDate] [datetime] NULL  
 ,[SenderSubID] [varchar](50) NULL  
 ,[AvgFxRateForTrade] [float] NULL  
 ,[ProcessDate] [datetime] NULL  
 ,[CalcBasis] [varchar](50) NOT NULL  
 ,[CommissionRate] [float] NOT NULL  
 ,[CommissionAmt] [float] NULL  
 ,[ImportFileName] [nvarchar](500) NULL  
 ,[ImportFileID] [int] NULL  
 ,[SoftCommissionCalcBasis] [varchar](1) NOT NULL  
 ,[SoftCommissionRate] [float] NOT NULL  
 ,[SoftCommissionAmt] [float] NULL  
 ,[TradeAttribute6] [varchar](50) NULL  
 ,[TradeAttribute5] [varchar](50) NULL  
 ,[TradeAttribute4] [varchar](50) NULL  
 ,[TradeAttribute3] [varchar](50) NULL  
 ,[TradeAttribute2] [varchar](50) NULL  
 ,[TradeAttribute1] [varchar](50) NULL  
 ,[InternalComments] [varchar](200) NULL  
 ,[SettlCurrency] [int] NULL  
 ,[FxRate] [float] NULL  
 ,[FxRateCalc] [varchar](1) NULL  
 ,[BorrowerID] [VARCHAR] (50) NULL  
    ,[BorrowBroker] [VARCHAR](50) NULL  
    ,[ShortRebate] [FLOAT] NULL  
 ,[NirvanaLocateID] [int] NULL  
 ,[IsSwapped] [bit] NULL  
 ,[NotionalValue] [float] NULL  
 ,[BenchMarkRate] [float] NULL  
 ,[Differential] [float] NULL  
 ,[OrigCostBasis] [float] NULL  
 ,[DayCount] [int] NULL  
 ,[SwapDescription] [varchar](500) NULL  
 ,[FirstResetDate] [datetime] NULL  
 ,[OrigTransDate] [datetime] NULL  
 ,[ResetFrequency] [varchar](20) NULL  
 ,[ClosingPrice] [float] NULL  
 ,[ClosingDate] [datetime] NULL  
 ,[TransDate] [datetime] NULL  
 ,[ShouldOverrideNotional] [bit] NULL  
 ,[ShouldOverrideCostBasis] [bit] NULL  
 ,[ChangeType] [int] NULL  
 ,[Text] [varchar](200) NULL  
 ,[OriginalAllocationPreferenceID] [int] NULL  
 ,[TransactionSource] [int] NULL  
 ,[StateID] [int] NULL  
 ,[IsHidden] [bit] NOT NULL  
 ,[AccountIds] [nvarchar](max) NULL  
 ,[StrategyIds] [nvarchar](max) NULL  
 ,[AllocationSchemeID] [int] NULL  
 ,[IsManualOrder] [BIT] NULL  
 ,[ExecutionTimeLastFill] [varchar](50) NULL  
 ,[ExpireTime] [varchar](50) NULL  
 ,[DayCumQty] [float] NULL  
    ,[DayAvgPx] [float] NULL  
 ,[IsUseCustodianBroker] [BIT] NULL  
,[OriginalPurchaseDate] [datetime] NULL
,[AdditionalTradeAttributes] [nvarchar](max) NULL  
)  
  
INSERT INTO #TradingDataFinal  
EXEC P_TradingDataFinal @AllAUECDatesString  
 ,@isAllowMultidayStagedOrders  
 ,null  
       
CREATE TABLE #FinalOrders (        
 clorderid VARCHAR(50)        
 ,ParentClorderiD VARCHAR(50)        
 ,lastpx FLOAT        
 ,averagePrice FLOAT        
 ,LeavesQty FLOAT        
 ,CumQty FLOAT        
 ,OrderStatus VARCHAR(3)        
 ,LastShares FLOAT        
 ,Quantity FLOAT        
 ,Symbol VARCHAR(100)        
 ,Side VARCHAR(3)        
 ,OrderType VARCHAR(3)        
 ,Price FLOAT        
 ,OrigClorderiD VARCHAR(50)        
 ,Expr2 VARCHAR(50)        
 ,--refers to ExecutionID                                                                            
 ServerTime VARCHAR(50)        
 ,CounterPartyID INT        
 ,VenueID INT        
 ,AUECID INT        
 ,AssetID INT        
 ,UnderLyingID INT        
 --,CountryFlagImage IMAGE        
 ,StagedOrderID VARCHAR(50)        
 ,TradingAccountID INT        
 ,CompanyUserID INT        
 ,NirvanaMsgType INT        
 ,DiscrOffset FLOAT        
 ,PegDiff VARCHAR(50)        
 ,StopPrice FLOAT        
 ,ClearanceTime DATETIME        
 ,ExpirationDate DATETIME        
 ,--MaturityYearMonth,                                                                            
 StrikePrice FLOAT        
 ,PutOrCall VARCHAR(4)        
 ,SecurityType NVARCHAR(40)        
 ,OpenClose VARCHAR(3)        
 ,ExecutionInst VARCHAR(50)        
 ,TimeInForce VARCHAR(3)        
 ,HandlingInst VARCHAR(3)        
 ,FillMsgType VARCHAR(3)        
 ,CMTA VARCHAR(20)        
 ,GiveUpID INT        
 ,UnderlyingSymbol VARCHAR(100)        
 ,OrderID VARCHAR(500)        
 ,AlgoStrategyID INT        
 ,AlgoParameters VARCHAR(500)        
 ,OriginatorTypeID VARCHAR(50)        
 ,ClientOrderID VARCHAR(50)        
 ,AUECLocalDate DATETIME        
 ,SettlementDate DATETIME        
 ,SenderSubID VARCHAR(50)        
 ,CurrencyID INT        
 ,AvgFxRateForTrade FLOAT        
 ,Multiplier FLOAT        
 ,ProcessDate DATETIME        
 ,FundId INT        
 ,StrategyId INT        
 ,OrderSeqNumber BIGINT        
 ,CalcBasis VARCHAR(50)        
 ,CommissionRate FLOAT        
 ,CommissionAmt FLOAT        
 ,ImportFileName NVARCHAR(500)        
 ,ImportFileID INT        
 ,BloombergSymbol NVARCHAR(500)        
 ,SoftCommissionCalcBasis VARCHAR(50)        
 ,SoftCommissionRate FLOAT        
 ,SoftCommissionAmt FLOAT        
 ,TradeAttribute1 VARCHAR(50)        
 ,TradeAttribute2 VARCHAR(50)        
 ,TradeAttribute3 VARCHAR(50)        
 ,TradeAttribute4 VARCHAR(50)        
 ,TradeAttribute5 VARCHAR(50)        
 ,TradeAttribute6 VARCHAR(50)        
 ,InternalComments VARCHAR(200)        
 ,SettlCurrency INT        
 ,FxRate FLOAT        
 ,FxRateCalc VARCHAR(1)   
 ,[OriginalAllocationPreferenceID] int  
 ,ExpireTime VARCHAR(50)
 ,ExecutionTimeLastFill VARCHAR(50)  
 ,AdditionalTradeAttributes VARCHAR(max)
 )        
        
INSERT INTO #FinalOrders (        
 clorderid        
 ,ParentClorderiD        
 ,lastpx        
 ,averagePrice        
 ,LeavesQty        
 ,CumQty        
 ,OrderStatus        
 ,LastShares        
 ,Quantity        
 ,Symbol        
 ,Side        
 ,OrderType        
 ,Price        
 ,OrigClorderiD        
 ,Expr2        
 ,--refers to ExecutionID                      
 ServerTime        
 ,CounterPartyID        
 ,VenueID        
 ,AUECID        
 ,AssetID        
 ,UnderLyingID        
 --,CountryFlagImage     
 ,StagedOrderID        
 ,TradingAccountID        
 ,CompanyUserID        
 ,NirvanaMsgType        
 ,DiscrOffset        
 ,PegDiff        
 ,StopPrice        
 ,ClearanceTime        
 ,ExpirationDate        
 ,--MaturityYearMonth,                                                                            
 StrikePrice        
 ,PutOrCall        
 ,SecurityType        
 ,OpenClose        
 ,ExecutionInst        
 ,TimeInForce        
 ,HandlingInst        
 ,FillMsgType        
 ,CMTA        
 ,GiveUpID        
 ,UnderlyingSymbol        
 ,OrderID        
 ,AlgoStrategyID        
 ,AlgoParameters        
 ,OriginatorTypeID        
 ,ClientOrderID        
 ,AUECLocalDate        
 ,SettlementDate        
 ,SenderSubID        
 ,CurrencyID        
 ,AvgFxRateForTrade        
 ,Multiplier        
 ,ProcessDate        
 ,FundId        
 ,StrategyId        
 ,OrderSeqNumber        
 ,CalcBasis        
 ,CommissionRate        
 ,CommissionAmt        
 ,ImportFileName        
 ,ImportFileID        
 ,BloombergSymbol        
 ,SoftCommissionCalcBasis        
 ,SoftCommissionRate        
 ,SoftCommissionAmt        
 ,TradeAttribute1        
 ,TradeAttribute2        
 ,TradeAttribute3        
 ,TradeAttribute4        
 ,TradeAttribute5        
 ,TradeAttribute6        
 ,InternalComments        
 ,SettlCurrency        
 ,FxRate        
 ,FxRateCalc  
 ,[OriginalAllocationPreferenceID]   
 ,ExpireTime  
 ,ExecutionTimeLastFill
 ,AdditionalTradeAttributes
 )        
SELECT clorderid        
 ,ParentClorderiD        
 ,lastpx        
 ,averagePrice        
 ,LeavesQty        
 ,CumQty        
 ,OrderStatus        
 ,LastShares        
 ,Quantity        
 ,Symbol        
 ,Side        
 ,OrderType        
 ,Price        
 ,OrigClorderiD        
 ,Expr2        
 ,--refers to ExecutionID                                                                            
 ServerTime        
 ,CounterPartyID        
 ,VenueID        
 ,#TradingDataFinal.AUECID        
 ,#TradingDataFinal.AssetID        
 ,#TradingDataFinal.UnderLyingID        
 --,CountryFlagImage        
 ,StagedOrderID        
 ,#TradingDataFinal.TradingAccountID        
 ,#TradingDataFinal.CompanyUserID        
 ,NirvanaMsgType        
 ,DiscrOffset        
 ,PegDiff        
 ,StopPrice        
 ,NULL    
 ,V_SecmasterData.ExpirationDate        
 ,--MaturityYearMonth,                                                                            
 V_SecmasterData.StrikePrice        
 ,V_SecmasterData.PutOrCall        
 ,SecurityType        
 ,OpenClose        
 ,ExecutionInst        
 ,TimeInForce        
 ,HandlingInst        
 ,FillMsgType        
 ,CMTA        
 ,GiveUpID        
 ,V_SecmasterData.UnderlyingSymbol        
 ,OrderID        
 ,AlgoStrategyID        
 ,AlgoParameters        
 ,OriginatorTypeID        
 ,ClientOrderID        
 ,AUECLocalDate        
 ,SettlementDate        
 ,SenderSubID        
 ,#TradingDataFinal.CurrencyID      
 ,AvgFxRateForTrade        
 ,V_SecmasterData.Multiplier        
 ,ProcessDate        
 ,#TradingDataFinal.FundId        
 ,#TradingDataFinal.StrategyId        
 ,#TradingDataFinal.OrderSeqNumber        
 ,#TradingDataFinal.CalcBasis        
 ,#TradingDataFinal.CommissionRate        
 ,#TradingDataFinal.CommissionAmt        
 ,#TradingDataFinal.ImportFileName        
 ,#TradingDataFinal.ImportFileID        
 ,V_SecMasterData.BloombergSymbol        
 ,#TradingDataFinal.SoftCommissionCalcBasis        
 ,#TradingDataFinal.SoftCommissionRate        
 ,#TradingDataFinal.SoftCommissionAmt        
 ,#TradingDataFinal.TradeAttribute1        
 ,#TradingDataFinal.TradeAttribute2        
 ,#TradingDataFinal.TradeAttribute3        
 ,#TradingDataFinal.TradeAttribute4        
 ,#TradingDataFinal.TradeAttribute5        
 ,#TradingDataFinal.TradeAttribute6        
 ,#TradingDataFinal.InternalComments  
 ,ISNULL(#TradingDataFinal.settlCurrency, 0)        
 ,#TradingDataFinal.FxRate        
 ,CASE         
  WHEN (        
    #TradingDataFinal.FxRateCalc = 'M'        
    OR #TradingDataFinal.FxRateCalc = 'D'        
    )        
   THEN #TradingDataFinal.FxRateCalc        
  ELSE 'M'        
  END AS FxRateCalc       
 ,ISNULL(#TradingDataFinal.[OriginalAllocationPreferenceID] ,0)  
 ,#TradingDataFinal.ExpireTime  
 ,#TradingDataFinal.ExecutionTimeLastFill
 ,#TradingDataFinal.AdditionalTradeAttributes
FROM #TradingDataFinal  
INNER JOIN T_AUEC AUEC ON AUEC.AUECID = #TradingDataFinal.AUECID  
INNER JOIN V_SecmasterData ON V_SecmasterData.TickerSymbol = #TradingDataFinal.Symbol            
 -- and clorderid in     
 --(    
 -- select distinct max(clorderId)     
 -- from #TradingDataFinal     
  --where leavesQty <> 0 and cumQty <>0     
 -- group by parentclorderId    
-- )        
      
--or (((NirvanaMsgType =3 and OrderStatus not in ('4'))or StagedOrderID <>'')  And @isAllowMultidayStagedOrders=1)                      
IF (@isAllowMultidayStagedOrders = 1)        
BEGIN        
 SELECT ParentClorderID        
  ,OrderStatus        
 INTO #Temp_StagedOrders        
 FROM #FinalOrders        
 WHERE OrderSeqNumber IN (        
   SELECT Max(OrderSeqNumber)        
   FROM #FinalOrders        
   INNER JOIN @AUECDatesTable AS AUECTable ON AUECTable.AUECID = #FinalOrders.AUECID        
   WHERE NirvanaMsgType = 3        
    AND OrderStatus = '4'        
    AND NOT (        
     (        
      datediff(d, AUECTable.CurrentAUECDate, #FinalOrders.AUECLocalDate) = 0        
      AND (dbo.TimeOnlyDiff(ClearanceTime, AUECLocalDate) = - 1)        
      )        
     OR (        
      datediff(d, AUECTable.CurrentAUECDate, #FinalOrders.AUECLocalDate) = 0        
      AND (dbo.TimeOnlyDiff(ClearanceTime, CurrentAUECDate) = 1)        
      )        
     OR (        
      datediff(d, AUECTable.CurrentAUECDate, #FinalOrders.AUECLocalDate) = - 1        
      AND dbo.TimeOnlyDiff(ClearanceTime, AUECLocalDate) = - 1        
      AND dbo.timeonlyDiff(CurrentAUECDate, ClearanceTime) = - 1        
      )        
     )        
   GROUP BY ParentClorderID        
   )        
        
 DELETE #FinalOrders        
 FROM #FinalOrders        
 INNER JOIN #Temp_StagedOrders ON #FinalOrders.ParentClorderID = #Temp_StagedOrders.ParentClorderID        
        
 DELETE #FinalOrders        
 FROM #FinalOrders        
 INNER JOIN #Temp_StagedOrders ON (        
   IsNull(StagedOrderID, '') <> ''        
   AND #FinalOrders.StagedOrderID = #Temp_StagedOrders.ParentClorderID        
   )        
        
 --Where (IsNull(StagedOrderID,'') <> ''                     
 DROP TABLE #Temp_StagedOrders        
END        
     
DELETE from #FinalOrders  
where getDate() >= (select expirationdate from  V_SecmasterData as S where S.tickersymbol = #FinalOrders.symbol and S.AssetID in(2,4,8,11,3,13))  
     
SELECT parentClorderID, MAX(OrderSeqNumber) As OrderSeqNumber  
INTO #RolloverData  
FROM #FinalOrders  
Group By parentClorderID  
  
DELETE FO      
FROM #FinalOrders FO       
INNER JOIN #RolloverData RD ON FO.parentClorderID = RD.parentClorderID AND FO.OrderSeqNumber < RD.OrderSeqNumber AND FO.OrderStatus = 'R'     
  
SELECT parentClorderID, MAX(OrderSeqNumber) As OrderSeqNumber  
INTO #RejectData  
FROM #FinalOrders  
Group By parentClorderID  
  
DELETE FO      
FROM #FinalOrders FO       
INNER JOIN #RejectData RD ON FO.parentClorderID = RD.parentClorderID AND FO.OrderSeqNumber < RD.OrderSeqNumber AND FO.FillMsgType = '9'   
   
DROP TABLE #RolloverData, #RejectData  
  
SELECT *        
FROM #FinalOrders        
ORDER BY ClorderID    
 ,parentClorderID        
        
DROP TABLE #FinalOrders        
        
DROP TABLE #TradingDataFinal