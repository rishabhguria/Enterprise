    
CREATE Procedure [dbo].[P_GetOpenPos_EternalCapital]                              
(                                       
@ThirdPartyID int,                                                  
@CompanyFundIDs varchar(max),                                                                                                                                                                                
@InputDate datetime,                                                                                                                                                                            
@CompanyID int,                                                                                                                                            
@AUECIDs varchar(max),                                                                                  
@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                  
@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                  
@FileFormatID int                                        
)                                        
AS            
          
--Declare @ThirdPartyID int,                                                  
-- @CompanyFundIDs varchar(max),                                                                                                                                                                                
-- @InputDate datetime,                                                                                                                                                                            
-- @CompanyID int,                                                                                                                                            
-- @AUECIDs varchar(max),                                                                                  
-- @TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                  
-- @DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                  
-- @FileFormatID int      
       
--set @thirdPartyID=86      
--set @companyFundIDs=N'1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16'      
--set @inputDate='07-18-2024'      
--set @companyID=7      
--set @auecIDs=N'20,43,21,18,61,74,1,15,11,62,73,12,80,32,81'      
--set @TypeID=0      
--set @dateType=0      
--set @fileFormatID=67      
      
Declare @Fund Table                                                                 
(                      
FundID int                            
)        
      
Insert into @Fund                                                                                                          
Select Cast(Items As Int) from dbo.Split(@companyFundIDs,',')       
      
-- get Mark Price for End Date                              
CREATE TABLE #MarkPriceForEndDate         
(          
  FinalMarkPrice Float          
 ,Symbol Varchar(100)          
 ,FundID INT          
 )         
        
INSERT INTO #MarkPriceForEndDate         
(          
 FinalMarkPrice          
 ,Symbol          
 ,FundID          
 )          
SELECT         
  DMP.FinalMarkPrice          
 ,DMP.Symbol          
 ,DMP.FundID          
FROM PM_DayMarkPrice DMP      
Inner Join @Fund F On F.FundID = DMP.FundID        
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0        
      
      
Create Table #TempTaxlotPK      
(       
Taxlot_PK BigInt      
)      
      
Insert InTo #TempTaxlotPK      
SELECT Distinct PT.Taxlot_PK          
      
FROM PM_Taxlots PT With (NoLock)                
Inner Join @Fund Fund on Fund.FundID = PT.FundID        
Where PT.Taxlot_PK in                                             
(                                                  
 Select Max(Taxlot_PK) from PM_Taxlots With (NoLock)                                                                                       
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                            
 Group by TaxlotId                                                                            
)                                                                                                
And PT.TaxLotOpenQty > 0       
      
Declare @MinTradeDate DateTime      
      
Set @MinTradeDate = (Select Min(G.AUECLocalDate) As MinTradeDate      
FROM PM_Taxlots PT WITH(NOLOCK)      
Inner Join #TempTaxlotPK T On T.Taxlot_PK = PT.Taxlot_PK      
INNER JOIN T_Group G WITH(NOLOCK) ON PT.GroupID = G.GroupID)      
      
Set @MinTradeDate = dbo.AdjustBusinessDays(@MinTradeDate,-1,1)      
      
-- get forex rates for 2 date ranges                
CREATE TABLE #FXConversionRates       
(      
 FromCurrencyID INT      
 ,ToCurrencyID INT      
 ,RateValue FLOAT      
 ,ConversionMethod INT      
 ,DATE DATETIME      
 ,eSignalSymbol VARCHAR(200)      
 ,FundID INT      
)      
      
INSERT INTO #FXConversionRates      
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @MinTradeDate ,@InputDate      
      
Update #FXConversionRates                          
Set RateValue = 1.0/RateValue                          
Where RateValue <> 0 and ConversionMethod = 1         
               
-- For Fund Zero                    
SELECT * INTO #ZeroFundFxRate                    
FROM #FXConversionRates                    
WHERE fundID = 0      
      
Delete From #FXConversionRates Where FundID = 0      
      
    
Select                     
CF.FundName As AccountName,                    
PT.Symbol,                    
Case                            
 When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1                            
 Then 'Long'                            
 Else 'Short'                            
End as PositionIndicator,        
(PT.TaxlotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue)) As OpenPositions,      
SM.BloombergSymbol,      
Curr.CurrencySymbol As LocalCurrency,    
TC.CurrencySymbol As SettlementCurrency,  
SM.Multiplier As AssetMultiplier,      
CONVERT(VARCHAR(10), G.AUECLocalDate, 101) AS TradeDate,        
SM.CompanyName As SecurityDescription,       
Case       
 When G.IsSwapped  = 1       
 Then 'EquitySwap'      
 Else A.AssetName       
End As AssetClass,      
SM.ISINSymbol,      
SM.SEDOLSymbol,      
SM.OSISymbol,      
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) As TotalCost_Local,       
--(PT.TaxlotOpenQty * PT.AvgPrice ) As TotalCost_Local,      
      
Cast(0.0 As Float) As TotalCost_Base,      
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,      
IsNull(MP_FundWise.Finalmarkprice,0) As MarkPrice,      
Case       
 When G.CurrencyID = CF.LocalCurrency       
 Then 1      
Else IsNull(FXRatesForEndDate.Val,0)       
End As FXRate_EndDate,      
--IsNull((PT.TaxlotOpenQty * MP_FundWise.FinalMarkPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) AS MarketValue,    
CASE   
    WHEN G.AssetID = '3'   
        THEN ISNULL((MP_FundWise.FinalMarkPrice - PT.AvgPrice)*(PT.TaxlotOpenQty * SM.Multiplier), 0)  
    ELSE   
        ISNULL(PT.TaxlotOpenQty * MP_FundWise.FinalMarkPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue), 0)  
END AS MarketValue,  
Cast(0 As float) AS MarketValueBase,       
SM.LeadCurrency,      
SM.VsCurrency,      
SM.CUSIPSymbol,      
Case       
 When G.AssetID = '5'       
 Then G.SettlementDate      
Else SM.ExpirationDate      
End As ExpirationDate,      
CASE       
 WHEN G.CurrencyID <> CF.LocalCurrency      
  THEN       
   CASE       
    WHEN IsNull(PT.FXRate,0) > 0      
     THEN      
      CASE       
       WHEN PT.FXConversionMethodOperator = 'M'      
       THEN PT.FXRate      
       WHEN PT.FXConversionMethodOperator= 'D'      
       THEN 1 / PT.FXRate      
      END      
   ELSE IsNull(FXRatesForTradeDate.Val, 0)      
   END      
 ELSE 1      
END As FXRate_TradeDate,      
SM.CountryName      
Into #TempOpenPositionsTable               
From PM_Taxlots PT      
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK       
INNER JOIN T_Group G ON G.GroupID = PT.GroupID      
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol      
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID   
Inner Join T_Currency TC With (NoLock) On TC.CurrencyID = PT.SettlCurrency  
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID      
Inner Join T_Asset A On A.AssetID = SM.AssetID      
LEFT OUTER JOIN #MarkPriceForEndDate MP_FundWise ON (PT.Symbol = MP_FundWise.Symbol And MP_FundWise.FundID = PT.FundID)       
-- FX Rate for Input Date        
LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON (      
  FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID AND FXDayRatesForTradeDate.ToCurrencyID = CF.LocalCurrency      
  AND DateDiff(DAY, G.AUECLocalDate, FXDayRatesForTradeDate.DATE) = 0 AND FXDayRatesForTradeDate.FundID = PT.FundID      
  )      
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON (      
  ZeroFundFxRateTradeDate.FromCurrencyID = G.CurrencyID AND ZeroFundFxRateTradeDate.ToCurrencyID = CF.LocalCurrency      
  AND DateDiff(d, G.AUECLocalDate, ZeroFundFxRateTradeDate.DATE) = 0 AND ZeroFundFxRateTradeDate.FundID = 0      
  )      
--  FX Rate for Input Date                          
 LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (      
  FXDayRatesForEndDate.FromCurrencyID = G.CurrencyID AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency                                                                                                                   
  AND DateDiff(d, @InputDate, FXDayRatesForEndDate.DATE) = 0 AND FXDayRatesForEndDate.FundID = PT.FundID       
  )       
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (      
  ZeroFundFxRateEndDate.FromCurrencyID = G.CurrencyID AND ZeroFundFxRateEndDate.ToCurrencyID = CF.LocalCurrency                    
  AND DateDiff(d, @InputDate, ZeroFundFxRateEndDate.DATE) = 0 AND ZeroFundFxRateEndDate.FundID = 0       
  )      
CROSS APPLY (      
 SELECT CASE       
   WHEN FXDayRatesForTradeDate.RateValue IS NULL      
    THEN CASE       
      WHEN ZeroFundFxRateTradeDate.RateValue IS NULL      
       THEN 0      
      ELSE ZeroFundFxRateTradeDate.RateValue      
      END      
   ELSE FXDayRatesForTradeDate.RateValue      
   END      
 ) AS FXRatesForTradeDate(Val)        
CROSS APPLY (                    
  SELECT       
 CASE                     
 WHEN FXDayRatesForEndDate.RateValue IS NULL        
 THEN       
  CASE            
   WHEN ZeroFundFxRateEndDate.RateValue IS NULL                    
   THEN 0              
  ELSE ZeroFundFxRateEndDate.RateValue        
  END                    
 ELSE FXDayRatesForEndDate.RateValue                    
 END                    
  ) AS FXRatesForEndDate(Val)       
Where PT.TaxlotOpenQty > 0      
      
Update #TempOpenPositionsTable      
Set TotalCost_Base = TotalCost_Local * FXRate_TradeDate,      
MarketValueBase = MarketValue * FXRate_EndDate      
      
Select                     
PT.AccountName As AccountName,                    
PT.Symbol,                    
PT.PositionIndicator As PositionIndicator,         
Sum(PT.OpenPositions) As OpenPositions,       
Max(BloombergSymbol) As BloombergSymbol,       
Max(LocalCurrency) As LocalCurrency,      
Max(SettlementCurrency) As SettlementCurrency,  
Max(PT.AssetMultiplier) As AssetMultiplier,      
Max(SideMultiplier) AS SideMultiplier,             
Max(SecurityDescription) As SecurityDescription,      
Sum(PT.TotalCost_Local) As TotalCost_Local,       
Sum(PT.TotalCost_Base) As TotalCost_Base,       
AssetClass As AssetClass,      
Max(ISINSymbol) As ISINSymbol,      
Max(SEDOLSymbol) As SEDOLSymbol,      
Max(CUSIPSymbol) As CUSIPSymbol,      
Max(MarkPrice) As MarkPrice,      
Max(FXRate_EndDate) As FXRate_EndDate,      
Max(FXRate_TradeDate) As FXRate_TradeDate,      
Max(OSISymbol) As OSISymbol,      
Sum(MarketValue) AS MarketValue,      
Sum(MarketValueBase) AS MarketValueBase,      
Convert(varchar(10),@InputDate,112) As InputDate,      
Max(TradeDate) As TradeDate,      
PT.ExpirationDate,      
Max(CountryName) As CountryName,      
1 As CustomOrderby        
Into #TempGroupedOpenPosTable                    
From #TempOpenPositionsTable PT       
Group By PT.AccountName,PT.AssetClass,PT.Symbol,PT.PositionIndicator,PT.ExpirationDate         
      
      
Alter Table #TempGroupedOpenPosTable            
Add UnrealizedPNL Float Null       
      
Update #TempGroupedOpenPosTable            
Set UnrealizedPNL = 0.0      
      
Update #TempGroupedOpenPosTable            
Set UnrealizedPNL = (MarketValueBase - TotalCost_Base)      
      
Select           
CF.FundName As AccountName,                    
CC.CurrencySymbol As Symbol ,       
Case                            
 When EODCash.CashValueLocal >= 0                          
 Then 'Long'        
 Else 'Short'                          
End as PositionIndicator,         
EODCash.CashValueLocal As OpenPositions,       
CC.CurrencySymbol As BloombergSymbol,       
CC.CurrencySymbol As LocalCurrency,     
CurrencyLocal.CurrencySymbol As SettlementCurrency,  
'' As AssetMultiplier,      
'' AS SideMultiplier,            
CC.CurrencyName As SecurityDescription,      
'' As TotalCost_Local,       
'' As TotalCost_Base,       
'Cash & Equivalents' As AssetClass,      
'' As ISINSymbol,      
'' As SEDOLSymbol,      
'' As CUSIPSymbol,      
1 As MarkPrice,      
Case       
 When EODCash.LocalCurrencyID = CF.LocalCurrency       
 Then 1      
Else Cast(IsNull(FXDayRatesForEndDate.RateValue,0) As decimal(18,8))      
End As FXRate_EndDate,      
1 AS FXRate_TradeDate,      
'' As OSISymbol,      
EODCash.CashValueLocal AS MarketValue,      
EODCash.CashValueBase As MarketValueBase,      
Convert(varchar(10),@InputDate,112) As InputDate,      
CONVERT(VARCHAR(10), EODCash.Date, 101) AS TradeDate,       
'' As ExpirationDate,      
'' As CountryName,      
1 AS UnrealizedPNL      
      
Into #TempEODCash                   
From PM_CompanyFundCashCurrencyValue EODCash      
Inner Join @Fund Fund On Fund.FundID = EODCash.FundID      
inner join T_CompanyFunds CF on EODCash.FundID = CF.CompanyFundID    
Inner join T_Currency CurrencyLocal  With (NoLock) On CurrencyLocal.CurrencyId = EODCash.LocalCurrencyID   
inner join T_Currency CC on EODCash.LocalCurrencyID=CC.CurrencyID      
-- Forex Price for Input Date                          
 LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (                      
   FXDayRatesForEndDate.FromCurrencyID = EODCash.LocalCurrencyID               
AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency                                                                                                                   
   AND DateDiff(d, @InputDate, FXDayRatesForEndDate.DATE) = 0                      
   AND FXDayRatesForEndDate.FundID = EODCash.FundID                    
    )       
  LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (           
   ZeroFundFxRateEndDate.FromCurrencyID = EODCash.LocalCurrencyID                     
   AND ZeroFundFxRateEndDate.ToCurrencyID = CF.LocalCurrency                    
   AND DateDiff(d, @InputDate, ZeroFundFxRateEndDate.DATE) = 0          
   AND ZeroFundFxRateEndDate.FundID = 0                    
   )        
CROSS APPLY (                    
  SELECT       
 CASE                     
 WHEN FXDayRatesForEndDate.RateValue IS NULL                    
 THEN       
  CASE                     
   WHEN ZeroFundFxRateEndDate.RateValue IS NULL                    
   THEN 0              
  ELSE ZeroFundFxRateEndDate.RateValue                    
  END                    
 ELSE FXDayRatesForEndDate.RateValue                    
 END                    
  ) AS FXRatesForEndDate(Val)       
  Where datediff(d,EODCash.Date,@Inputdate)=0      
--  ------------------------------------------------      
-- -- Select * from #TempEODCash      
Insert InTo #TempGroupedOpenPosTable          
Select                     
EODCash.AccountName As AccountName,                    
EODCash.Symbol,                    
(EODCash.PositionIndicator) As PositionIndicator,         
Sum(EODCash.OpenPositions) As OpenPositions,      
Max(BloombergSymbol) As BloombergSymbol,       
Max(LocalCurrency) As LocalCurrency,       
Max(SettlementCurrency) As SettlementCurrency,  
Max(EODCash.AssetMultiplier) As AssetMultiplier,      
Max(EODCash.SideMultiplier) As SideMultiplier,      
Max(SecurityDescription) As SecurityDescription,      
Min(EODCash.TotalCost_Local) As TotalCost_Local,       
Min(EODCash.TotalCost_Base) As TotalCost_Base,       
Min(EODCash.AssetClass) As AssetClass,      
Max(ISINSymbol) As ISINSymbol,      
Max(SEDOLSymbol) As SEDOLSymbol,      
Max(CUSIPSymbol) As CUSIPSymbol,      
Max(MarkPrice) As MarkPrice,      
Max(FXRate_Enddate) As FXRate,      
Max(FXRate_TradeDate) As FXRate_TradeDate,      
Max(OSISymbol) As OSISymbol,      
Sum(MarketValue) AS MarketValue,      
Sum(MarketValueBase) AS MarketValueBase,      
Max(InputDate) AS InputDate,      
Max(EODCash.TradeDate) AS TradeDate,      
Min(EODCash.ExpirationDate) As ExpirationDate,
'' As CountryName,      
2 As CustomOrderby,       
1 AS UnrealizedPNL      
From #TempEODCash EODCash        
Group By EODCash.AccountName,EODCash.Symbol,EODCash.PositionIndicator      
      
Select * From #TempGroupedOpenPosTable      
--Where AssetClass = 'Cash'      
Order By CustomOrderby,AccountName,Symbol,PositionIndicator        
      
Drop Table #TempOpenPositionsTable, #TempGroupedOpenPosTable,#TempEODCash      
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates,#ZeroFundFxRate