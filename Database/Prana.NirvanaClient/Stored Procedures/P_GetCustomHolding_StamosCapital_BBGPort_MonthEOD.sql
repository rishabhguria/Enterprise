        
CREATE Procedure [dbo].[P_GetCustomHolding_StamosCapital_BBGPort_MonthEOD]                                
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
--@CompanyFundIDs varchar(200),                                                                                                                                                                                  
--@InputDate datetime,                                                                                                                                                                              
--@CompanyID int,                                                                                                                                              
--@AUECIDs varchar(500),                                                                                    
--@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                    
--@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                    
--@FileFormatID int          
        
--Set @ThirdPartyID = 1                                                    
--Set @CompanyFundIDs = '1,5'                                                                                                                                                                           
--Set @InputDate =  '06-23-2025'                                                                                                                                                                           
--Set @CompanyID = 1                                                                                                                                             
--set @AUECIDs = '1'                                                                                    
--Set @TypeID = 0        
--Set @DateType = 0                                                                                                                                                                                 
--Set @FileFormatID = 0       
      
Declare @PreviousBusinessDate DateTime        
        
Declare @Fund Table                                                                             
(                                  
FundID int                                        
)                    
                  
Insert into @Fund     
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')           
        
Create Table #Fund                                                                   
(                       
FundID int,        
FundName Varchar(100),        
LocalCurrency Int,        
MasterFundName Varchar(100)                              
)          
        
Insert into #Fund         
Select         
CF.CompanyFundID,        
CF.FundName,        
CF.LocalCurrency,         
MF.MasterFundName        
From T_CompanyFunds CF WITH(NOLOCK)        
Inner Join @Fund F On F.FundID = CF.CompanyFundID        
Inner Join T_CompanyMasterFundSubAccountAssociation MFA WITH(NOLOCK) On MFA.CompanyFundID = CF.CompanyFundID         
Inner Join T_CompanyMasterFunds MF WITH(NOLOCK) On MF.CompanyMasterFundID = MFA.CompanyMasterFundID        
      
      
Create Table #Temp_FinalTable      
(      
HoldingDate Varchar(20),      
AssetClass Varchar(20),      
MasterFund Varchar(100),      
Account Varchar(100),      
Symbol Varchar(100),        
SecurityName Varchar(200),        
Quantity Float,        
MarkPrice Float,        
MarketValue_Local Float,        
MarketValue_Base Float,        
UnitCost Float,        
CostBasis_Local Float,        
CostBasis_Base Float,        
UnrealizedGainLoss Float,        
ContractSize Float,        
ISINSymbol Varchar(50),         
CUSIPSymbol Varchar(20),        
SEDOLSymbol Varchar(50),        
OSISymbol Varchar(50),         
TradeCurrency Varchar(10),      
PortfolioCurrency Varchar(10),       
BloombergSymbol Varchar(50),        
CustomOrder Int      
)      
      
CREATE TABLE #MarkPriceForEndDate           
(            
 FinalMarkPrice Float,            
 Symbol VARCHAR(200),           
 FundID INT            
)       
CREATE TABLE #ZeroFundMarkPriceEndDate           
(            
 FinalMarkPrice Float,            
 Symbol VARCHAR(200),           
 FundID INT            
)         
-- get forex rates for 2 date ranges                  
CREATE TABLE #FXConversionRates         
(        
 FromCurrencyID INT        
 ,ToCurrencyID INT        
 ,RateValue FLOAT        
 ,ConversionMethod INT        
 ,DATE DATETIME        
 ,eSignalSymbol VARCHAR(100)        
 ,FundID INT        
)       
-- get forex rates for 2 date ranges                  
Create Table #ZeroFundFxRate         
(        
 FromCurrencyID INT        
 ,ToCurrencyID INT        
 ,RateValue FLOAT        
 ,ConversionMethod INT        
 ,DATE DATETIME        
 ,eSignalSymbol VARCHAR(100)        
 ,FundID INT        
)       
      
Create Table #TempTaxlotPK        
(         
 Taxlot_PK BigInt        
)       
      
Create Table #TempOpenPositionsTable      
(      
MasterFund Varchar(100),      
Account Varchar(100),      
AssetClass Varchar(20),      
Symbol Varchar(100),        
PositionSide Varchar(10),      
SecurityName Varchar(200),        
OpenQty Float,        
MarkPrice Float,        
MarketValue_Local Float,        
MarketValue_Base Float,         
CostBasis_Local Float,        
CostBasis_Base Float,      
ContractSize Float,       
ISINSymbol Varchar(50),         
CUSIPSymbol Varchar(50),        
SEDOLSymbol Varchar(50),        
OSISymbol Varchar(50),         
TradeFXRate Float,      
EndDateFXRate Float,      
SideMultiplier Int,      
TradeCurrency Varchar(10),      
PortfolioCurrency Varchar(10),       
UnderLyingSymbol Varchar(100),      
BloombergSymbol Varchar(50)      
)      
      
Create Table #TempEODCashAndAccruals          
(           
HoldingDate Date        
,MasterFund varchar(200)        
,Account Varchar(200)          
,Quantity Float        
,MarketValue_Local Float          
,MarketValue_Base Float        
)          
      
Declare @MonthStartDate Date      
Set @MonthStartDate = DATETRUNC(MONTH, @InputDate)  
  
Set @MonthStartDate =   
 CASE   
  WHEN DATEPART(WEEKDAY, @MonthStartDate) = 7 THEN DATEADD(DAY, 2, @MonthStartDate) -- Saturday → Monday  
  WHEN DATEPART(WEEKDAY, @MonthStartDate) = 1 THEN DATEADD(DAY, 1, @MonthStartDate) -- Sunday → Monday  
 ELSE @MonthStartDate -- Weekday → return as-is  
 END   
      
While DateDiff(Day, @MonthStartDate, @InputDate) >= 0      
      
Begin          
          
 --Select @MonthStartDate, @InputDate      
      
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
 FROM PM_DayMarkPrice DMP With (NoLock)        
 WHERE DateDiff(Day,DMP.DATE,@MonthStartDate) = 0        
        
 -- For Fund Zero        
 Insert INTO #ZeroFundMarkPriceEndDate       
 SELECT *         
 FROM #MarkPriceForEndDate        
 WHERE FundID = 0        
        
        
 Insert InTo #TempTaxlotPK        
 SELECT Distinct PT.Taxlot_PK            
        
 FROM PM_Taxlots PT With (NoLock)                  
 Inner Join @Fund Fund on Fund.FundID = PT.FundID          
 Where PT.Taxlot_PK in                                               
 (                                                                                                        
  Select Max(Taxlot_PK) from PM_Taxlots With (NoLock)                              
  Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@MonthStartDate) >= 0                                                                              
  Group by TaxlotId                                                                              
 )                                                                                                  
 And Round(PT.TaxLotOpenQty,2) > 0       
      
       
 -- get Mark Price for End Date        
 Set @PreviousBusinessDate = (Select Min(G.AUECLocalDate) As MinTradeDate        
 FROM PM_Taxlots PT WITH(NOLOCK)        
 Inner Join #TempTaxlotPK T On T.Taxlot_PK = PT.Taxlot_PK        
 INNER JOIN T_Group G WITH(NOLOCK) ON PT.GroupID = G.GroupID)        
        
 Set @PreviousBusinessDate = dbo.AdjustBusinessDays(@PreviousBusinessDate,-1,1)        
        
 --Select @PreviousBusinessDate        
      
 INSERT INTO #FXConversionRates        
 EXEC P_GetAllFXConversionRatesForGivenDateRange @PreviousBusinessDate ,@MonthStartDate        
        
 UPDATE #FXConversionRates        
 SET RateValue = 1.0 / RateValue        
 WHERE RateValue <> 0        
  AND ConversionMethod = 1        
        
 -- For FundID = 0 (Zero)       
 Insert InTo #ZeroFundFxRate       
 SELECT *          
 FROM #FXConversionRates        
 WHERE FundID = 0       
      
 Insert Into #TempEODCashAndAccruals          
 SELECT        
 Convert(varchar, @MonthStartDate,23) As HoldingDate        
 ,Max(Fund.MasterFundName) As MasterFund        
 ,Max(Fund.FundName) As AccountName            
 ,Round(SUM(IsNull(SubAccountBalances.CloseDRBalBase - SubAccountBalances.CloseCRBalBase, 0)),4) AS Quantity              
 ,Round(SUM(IsNull(SubAccountBalances.CloseDRBalBase - SubAccountBalances.CloseCRBalBase, 0)),4) As MarketValue_Local         
 ,Round(SUM(IsNull(SubAccountBalances.CloseDRBalBase - SubAccountBalances.CloseCRBalBase, 0)),4) AS MarketValue_Base             
 FROM T_SubAccountBalances SubAccountBalances With (NoLock)           
 INNER JOIN T_SubAccounts SubAccounts With (NoLock) ON SubAccounts.SubAccountID = SubAccountBalances.SubAccountID            
 INNER JOIN T_TransactionType TransType With (NoLock) ON SubAccounts.TransactionTypeID = TransType.TransactionTypeID            
 INNER JOIN #Fund Fund ON Fund.FundID = SubAccountBalances.FundID          
 Inner Join T_Currency CURR With (NoLock) On CURR.CurrencyID =  SubAccountBalances.CurrencyID            
 WHERE (DateDiff(Day, SubAccountBalances.TransactionDate, @MonthStartDate) = 0)            
  AND TransType.TransactionType = 'Accrued Balance'                        
 GROUP BY Fund.FundName           
 HAVING Round(SUM(IsNull(SubAccountBalances.CloseDRBalBase - SubAccountBalances.CloseCRBalBase, 0)),4) <> 0       
      
 Insert Into #TempOpenPositionsTable        
 Select          
 CF.MasterFundName As MasterFund,         
 CF.FundName As Account,          
 A.AssetName As AssetClass,        
 SM.TickerSymbol As Symbol,          
 Case  dbo.GetSideMultiplier(PT.OrderSideTagValue)                            
  When 1                              
  Then 'LONG'                              
 Else 'SHORT'                              
 End as PositionSide,                
 SM.CompanyName As SecurityName,          
 PT.TaxlotOpenQty As OpenQty,        
 IsNull(MPEndDate.Val, 0) As MarkPrice,        
 IsNull((PT.TaxlotOpenQty * IsNull(MPEndDate.Val, 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) As MarketValue_Local,        
 Cast(0.00 As Float) As MarketValue_Base,        
 (PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) As CostBasis_Local,         
 Cast(0.00 As Float) As CostBasis_Base,        
 SM.Multiplier As ContractSize,        
        
 SM.ISINSymbol As ISINSymbol,        
 SM.CUSIPSymbol As CUSIPSymbol,        
 SM.SEDOLSymbol As SEDOLSymbol,        
 SM.OSISymbol As OSISymbol,        
 CASE         
  WHEN G.CurrencyID = CF.LocalCurrency         
  THEN 1        
 Else         
  CASE         
   WHEN ISNULL(PT.FXRate, 0) > 0        
    THEN CASE ISNULL(PT.FXConversionMethodOperator, 'M')        
   WHEN 'M'        
    THEN PT.FXRate        
   WHEN 'D'        
    THEN 1 / PT.FXRate        
   END        
   WHEN ISNULL(G.FXrate, 0) > 0        
    THEN CASE ISNULL(G.FXConversionMethodOperator, 'M')        
   WHEN 'M'        
    THEN G.FXRate       
   WHEN 'D'        
    THEN 1 / G.FXRate        
   END        
   ELSE ISNULL(FXRatesForTradeDate.Val, 0)        
   END         
 End AS TradeFXRate,        
 Case         
  When SM.CurrencyID = CF.LocalCurrency         
  Then 1        
 Else IsNull(FXRatesForEndDate.Val,0)         
 End As EndDateFXRate,        
 dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,        
 TradeCurr.CurrencySymbol As TradeCurrency,        
 PortfolioCurr.CurrencySymbol As PortfolioCurrency,        
 SM.UnderLyingSymbol,        
 SM.BloombergSymbol        
                 
 From PM_Taxlots PT With (NoLock)        
 Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK         
 Inner Join #Fund CF With (NoLock) On CF.FundID = PT.FundID        
 Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol        
 Inner Join T_Currency TradeCurr With (NoLock) On TradeCurr.CurrencyID = SM.CurrencyID          
 Inner Join T_Currency PortfolioCurr With (NoLock) On PortfolioCurr.CurrencyID = CF.LocalCurrency        
 Inner Join T_Asset A With (NoLock) On A.AssetID = SM.AssetID        
 INNER JOIN T_Group G With (NoLock) ON G.GroupID = PT.GroupID        
        
 LEFT OUTER JOIN #MarkPriceForEndDate MPE ON (        
   MPE.Symbol = PT.Symbol AND MPE.FundID = PT.FundID )        
 LEFT OUTER JOIN #ZeroFundMarkPriceEndDate MPZeroEndDate ON (        
   PT.Symbol = MPZeroEndDate.Symbol AND MPZeroEndDate.FundID = 0)        
 /* Forex Price for Trade Date */        
 LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON         
   (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID        
   AND FXDayRatesForTradeDate.ToCurrencyID = CF.LocalCurrency        
   AND DateDiff(d, G.ProcessDate, FXDayRatesForTradeDate.DATE) = 0        
   AND FXDayRatesForTradeDate.FundID = PT.FundID)        
 LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON         
   (ZeroFundFxRateTradeDate.FromCurrencyID = G.CurrencyID        
   AND ZeroFundFxRateTradeDate.ToCurrencyID = CF.LocalCurrency        
   AND DateDiff(d, G.ProcessDate, ZeroFundFxRateTradeDate.DATE) = 0        
   AND ZeroFundFxRateTradeDate.FundID = 0)        
 /* Forex Price for End Date */        
 LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON         
   (FXDayRatesForEndDate.FromCurrencyID = G.CurrencyID        
   AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency        
   AND DateDiff(d, FXDayRatesForEndDate.DATE,@MonthStartDate) = 0)        
   AND FXDayRatesForEndDate.FundID = PT.FundID           
 LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON         
   (ZeroFundFxRateEndDate.FromCurrencyID = G.CurrencyID        
   AND ZeroFundFxRateEndDate.ToCurrencyID = CF.LocalCurrency        
   AND DateDiff(d, @MonthStartDate, ZeroFundFxRateEndDate.DATE) = 0        
   AND ZeroFundFxRateEndDate.FundID = 0)        
 CROSS APPLY (        
  SELECT CASE         
    WHEN MPE.FinalMarkPrice IS NULL        
  THEN CASE         
    WHEN MPZeroEndDate.FinalMarkPrice IS NULL        
    THEN 0        
    ELSE MPZeroEndDate.FinalMarkPrice        
   END        
    ELSE MPE.Finalmarkprice        
    END        
  ) AS MPEndDate(Val)        
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
  SELECT CASE         
    WHEN FXDayRatesForEndDate.RateValue IS NULL        
  THEN CASE         
    WHEN ZeroFundFxRateEndDate.RateValue IS NULL        
     THEN 0        
    ELSE ZeroFundFxRateEndDate.RateValue        
    END        
    ELSE FXDayRatesForEndDate.RateValue        
    END        
  ) AS FXRatesForEndDate(Val)        
 Where PT.TaxlotOpenQty > 0        
        
 Update #TempOpenPositionsTable        
 Set CostBasis_Base = CostBasis_Local * TradeFXRate,        
 MarketValue_Base = MarketValue_Local * EndDateFXRate        
      
 ----Select * from #TempOpenPositionsTable        
        
 Select          
 Temp.MasterFund,        
 Temp.Account As Account,         
 AssetClass As AssetClass,            
 Temp.Symbol,               
 Max(SecurityName) As SecurityName,        
 Sum(Temp.OpenQty * Temp.SideMultiplier) As OpenQty,         
 Max(MarkPrice) As MarkPrice,        
 Sum(MarketValue_Local) AS MarketValue_Local,        
 Sum(MarketValue_Base) AS MarketValue_Base,        
 Sum(Temp.CostBasis_Local) As CostBasis_Local,         
 Sum(Temp.CostBasis_Base) As CostBasis_Base,         
 Max(Temp.ContractSize) As ContractSize,              
 Max(ISINSymbol) As ISINSymbol,        
 Max(CUSIPSymbol) As CUSIPSymbol,        
 Max(SEDOLSymbol) As SEDOLSymbol,        
 Max(OSISymbol) As OSISymbol,        
 Max(TradeFXRate) As TradeFXRate,        
 Max(EndDateFXRate) As EndDateFXRate,        
 Max(TradeCurrency) As TradeCurrency,        
 Max(PortfolioCurrency) As PortfolioCurrency,         
 Max(UnderlyingSymbol) As UnderlyingSymbol,        
 Max(BloombergSymbol) As BloombergSymbol       
 Into #TempTable                      
 From #TempOpenPositionsTable Temp          
 Group By Temp.MasterFund, Temp.Account,Temp.AssetClass,Temp.Symbol        
        
 Alter Table #TempTable              
 Add UnitCost Float Null,        
 UnrealizedGainLoss Float Null        
              
 UPdate #TempTable              
 Set UnitCost = 0.0,UnrealizedGainLoss = 0.0           
              
 UPdate #TempTable              
 Set UnitCost =               
 Case                
  When OpenQty <> 0 And ContractSize <> 0                
  Then (CostBasis_Local/OpenQty) /ContractSize                
  Else 0                
 End,        
 UnrealizedGainLoss = MarketValue_Base - CostBasis_Base            
        
 Insert InTo #Temp_FinalTable       
 Select         
 Convert(varchar, @MonthStartDate,23) As HoldingDate,          
 AssetClass,        
 T.MasterFund,        
 Account,        
 Symbol,        
 SecurityName,        
 Round(OpenQty,0) As Quantity,        
 MarkPrice,        
 MarketValue_Local,        
 MarketValue_Base,        
 UnitCost,        
 CostBasis_Local As CostBasis_Local,        
 CostBasis_Base As CostBasis_Base,        
 UnrealizedGainLoss,        
 ContractSize,        
 ISINSymbol,        
 CUSIPSymbol,        
 SEDOLSymbol,        
 OSISymbol,        
 TradeCurrency,        
 PortfolioCurrency,        
 BloombergSymbol,      
 2 As CustomOrder      
 From #TempTable T        
        
 -- EOD Cash        
 Insert into #Temp_FinalTable                
 select            
 Convert(varchar, @MonthStartDate,23) As HoldingDate,        
 'Cash' As AssetClass,          
 CF.MasterFundName As MasterFund,          
 CF.FundName As Account,                                         
 'Cash-' + CurrencyLocal.CurrencySymbol As Symbol,        
 'Cash-' + CurrencyLocal.CurrencySymbol As SecurityName,                                          
Cast(Cash.CashValueLocal As Decimal(18,2)) as Quantity,      
0.0 As MarkPrice,      
Cast(Cash.CashValueLocal As Decimal(18,2)) As MarketValue_Local,       
Cast(Cash.CashValueBase As Decimal(18,2)) As MarketValue_Base,              
 0.0 As UnitCost,        
 0.0 As CostBasis_Local,        
 0.0 As CostBasis_Base,        
 0.0 As UnrealizedGainLoss,        
 1 As ContractSize,        
 '' As ISINSymbol,        
 '' As CUSIPSymbol,        
 '' As SEDOLSymbol,        
 '' As OSISymbol,        
        
 CurrencyLocal.CurrencySymbol As TradeCurrency,        
 CurrencyBase.CurrencySymbol As PortfolioCurrency ,        
 '' As BloombergSymbol,        
 1 As CustomOrder                                   
 From PM_CompanyFundCashCurrencyValue Cash  With (NoLock)                             
 Inner join #Fund CF On CF.FundId = Cash.FundID                              
 Inner join T_Currency CurrencyLocal  With (NoLock) On CurrencyLocal.CurrencyId = Cash.LocalCurrencyID                                        
 Inner join T_Currency CurrencyBase  With (NoLock) On CurrencyBase.CurrencyId = Cash.BaseCurrencyID                                        
 Where DateDiff(Day, Cash.Date, @MonthStartDate) = 0        
        
 ----Accruals        
        
 Insert InTo #Temp_FinalTable                
 Select            
 HoldingDate,        
 'Cash' As AssetClass,          
 MasterFund,          
 Account,                                         
 'Cash-USD' As Symbol,        
 'Cash-USD' As SecurityName,                                          
 Quantity,        
 0.0 As MarkPrice,        
 MarketValue_Local,         
 MarketValue_Base,            
 0.0 As UnitCost,        
 0.0 As CostBasis_Local,        
 0.0 As CostBasis_Base,        
 0.0 As UnrealizedGainLoss,        
 1 As ContractSize,        
 '' As ISINSymbol,        
 '' As CUSIPSymbol,        
 '' As SEDOLSymbol,        
 '' As OSISymbol,        
 'USD' As TradeCurrency,        
 'USD' As PortfolioCurrency ,        
 '' As BloombergSymbol,        
 3 As CustomOrder                              
 From #TempEODCashAndAccruals        
      
 Truncate Table #MarkPriceForEndDate      
 Truncate Table #TempTaxlotPK      
 Truncate Table #ZeroFundMarkPriceEndDate      
 Truncate Table #FXConversionRates      
 Truncate Table #ZeroFundFxRate      
 Truncate Table #TempEODCashAndAccruals      
 Truncate Table #TempOpenPositionsTable      
 Drop Table #TempTable      
      
 ----Set @MonthStartDate = dbo.AdjustBusinessDays(@MonthStartDate,1,1)      
 Set @MonthStartDate = dbo.AdjustBusinessDaysWithoutExchangeHolidays(@MonthStartDate,1)      
      
End      
      
        
Select *        
From #Temp_FinalTable        
--Where AssetClass = 'Accrual'        
Order By HoldingDate, CustomOrder, MasterFund,Account,Symbol          
        
Drop Table #TempOpenPositionsTable,#Temp_FinalTable, #Fund,#TempEODCashAndAccruals --,#TempTable       
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates,#ZeroFundMarkPriceEndDate,#ZeroFundFxRate