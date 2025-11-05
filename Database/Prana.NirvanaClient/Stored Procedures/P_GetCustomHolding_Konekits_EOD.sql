        
        
        
CREATE Procedure [dbo].[P_GetCustomHolding_Konekits_EOD]                                
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
--@CompanyFundIDs varchar(max),                                                                                                                                                                                  
--@InputDate datetime,                                                                                                                                                                              
--@CompanyID int,                                                                                                                                              
--@AUECIDs varchar(max),                                                                                    
--@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                    
--@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                    
--@FileFormatID int          
        
--set @thirdPartyID=56      
--set @companyFundIDs=N'1342,1345,'      
--set @inputDate='2025-08-29 06:01:07'      
--set @companyID=7      
--set @auecIDs=N'20,65,76,63,53,44,34,43,56,59,31,54,21,18,61,1,15,11,62,73,12,32,'      
--set @TypeID=0      
--set @dateType=1      
--set @fileFormatID=129      
      
Declare @PreviousBusinessDate DateTime        
        
Declare @Fund Table                                                                             
(                                  
FundID int                                        
)                    
                  
Insert into @Fund                                                                                                                      
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')           
        
--Declare @varMasterFundIds Varchar(1000)        
        
--Set @varMasterFundIds = '21'        
        
--Declare @MasterFunds Table        
--(        
--MasterFundId Int,        
--MasterFundName Varchar(200)        
--)        
        
--Insert into @MasterFunds          
--Select         
--CompanyMasterFundID,        
--MasterFundName         
--From T_CompanyMasterFunds        
--Where CompanyMasterFundID IN        
--(                                                                                                          
--Select Cast(Items as int) from dbo.Split(@varMasterFundIds,',')         
--)         
    
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
        
Create Table #TempAccountNAV      
(      
MasterFund Varchar(100),      
NAV Float      
)      
      
Insert InTo #TempAccountNAV      
      
SELECT       
CMF.MasterFundName As MasterFund,      
Sum( SubBal.CloseDrBalBase - SubBal.CloseCrBalBase) As NAV      
      
FROM T_SubAccountBalances SubBal WITH(NOLOCK)      
Inner Join @Fund TempF On TempF.FundID = SubBal.FundID      
INNER JOIN T_SubAccounts SubAcc WITH(NOLOCK) ON SubAcc.SubAccountID = SubBal.SubAccountID      
INNER JOIN T_CompanyFunds Funds WITH(NOLOCK) ON Funds.CompanyFundID = SubBal.FundID      
Inner Join T_CompanyMasterFundSubAccountAssociation MFA On MFA.CompanyFundID = Funds.CompanyFundID       
Inner Join T_CompanyMasterFunds CMF On CMF.CompanyMasterFundID = MFA.CompanyMasterFundID      
INNER JOIN T_SubCategory SubCat WITH(NOLOCK) ON SubCat.SubCategoryID = SubAcc.SubCategoryID      
INNER JOIN T_MasterCategory MastCat WITH(NOLOCK) ON MastCat.MasterCategoryID = SubCat.MasterCategoryID      
INNER JOIN T_TransactionType AccType WITH(NOLOCK) ON AccType.TransactionTypeId = SubAcc.TransactionTypeId      
INNER JOIN T_CashPreferences CashPref WITH(NOLOCK) ON CashPref.FundID = SubBal.FundID      
WHERE DateDiff(d, TransactionDate, @InputDate) = 0      
 AND DATEDIFF(d, SubBal.TransactionDate, CashPref.CashMgmtStartDate) <= 0      
 And MastCat.MasterCategoryID In (1,2,6)      
Group By CMF.MasterFundName      
Order By CMF.MasterFundName      
      
-- get Mark Price for End Date                      
CREATE TABLE #MarkPriceForEndDate           
(            
  Finalmarkprice FLOAT            
 ,Symbol VARCHAR(200)            
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
FROM PM_DayMarkPrice DMP With (NoLock)        
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0        
        
-- For Fund Zero        
SELECT *        
INTO #ZeroFundMarkPriceEndDate        
FROM #MarkPriceForEndDate        
WHERE FundID = 0        
        
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
And Round(PT.TaxLotOpenQty,2) > 0         
        
Set @PreviousBusinessDate = (Select Min(G.AUECLocalDate) As MinTradeDate        
FROM PM_Taxlots PT WITH(NOLOCK)        
Inner Join #TempTaxlotPK T On T.Taxlot_PK = PT.Taxlot_PK        
INNER JOIN T_Group G WITH(NOLOCK) ON PT.GroupID = G.GroupID)        
        
Set @PreviousBusinessDate = dbo.AdjustBusinessDays(@PreviousBusinessDate,-1,1)        
        
--Select @PreviousBusinessDate        
        
-- get forex rates for 2 date ranges                  
CREATE TABLE #FXConversionRates         
(        
 FromCurrencyID INT        
 ,ToCurrencyID INT        
 ,RateValue FLOAT        
 ,ConversionMethod INT        
 ,DATE DATETIME        
 ,eSignalSymbol VARCHAR(max)        
 ,FundID INT        
)        
        
INSERT INTO #FXConversionRates        
EXEC P_GetAllFXConversionRatesForGivenDateRange @PreviousBusinessDate ,@InputDate        
        
UPDATE #FXConversionRates        
SET RateValue = 1.0 / RateValue        
WHERE RateValue <> 0        
 AND ConversionMethod = 1        
        
-- For FundID = 0 (Zero)        
SELECT *        
INTO #ZeroFundFxRate        
FROM #FXConversionRates        
WHERE FundID = 0          
      
Select          
CF.MasterFundName As MasterFund,         
CF.FundName As Account,          
A.AssetName As AssetClass,        
SM.TickerSymbol As Symbol,          
Case  dbo.GetSideMultiplier(PT.OrderSideTagValue)                            
 When 1                              
 Then 'Long'                              
Else 'Short'                              
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
        
Into #TempOpenPositionsTable                 
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
  AND DateDiff(d, FXDayRatesForEndDate.DATE,@InputDate) = 0)        
  AND FXDayRatesForEndDate.FundID = PT.FundID           
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON         
  (ZeroFundFxRateEndDate.FromCurrencyID = G.CurrencyID        
  AND ZeroFundFxRateEndDate.ToCurrencyID = CF.LocalCurrency        
  AND DateDiff(d, @InputDate, ZeroFundFxRateEndDate.DATE) = 0        
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
Max(PositionSide) AS PositionSide,      
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
Add      
PercentEquity Decimal(18,10) Null,      
Nav Float Null      
              
UPdate #TempTable              
Set PercentEquity=0.0, Nav=0.0    
       
      
UPdate OpenPos            
Set PercentEquity =             
Case              
 When IsNull(MasterFundNAV.NAV,0) <> 0              
 Then ((OpenPos.MarketValue_Local) / MasterFundNAV.NAV ) * 100            
 Else 0              
End ,      
Nav= MasterFundNAV.NAV      
From #TempTable OpenPos      
Inner Join #TempAccountNAV MasterFundNAV On MasterFundNAV.MasterFund = OpenPos.MasterFund      
        
Select         
Convert(varchar, @InputDate,23) As HoldingDate,          
AssetClass,        
T.MasterFund,        
Account,        
Symbol,       
PositionSide,      
SecurityName,        
Round(OpenQty,0) As Quantity,        
MarkPrice,        
MarketValue_Local,        
MarketValue_Base,         
CostBasis_Local As CostBasis_Local,        
CostBasis_Base As CostBasis_Base,        
ContractSize,        
ISINSymbol,        
CUSIPSymbol,        
SEDOLSymbol,        
OSISymbol,        
TradeCurrency,        
PortfolioCurrency,      
BloombergSymbol,      
EndDateFXRate,      
PercentEquity,      
Nav,      
2 As CustomOrder      
        
InTo #Temp_FinalTable        
From #TempTable T        
        
-- EOD Cash        
Insert into #Temp_FinalTable                
select            
Convert(varchar, @InputDate,23) As HoldingDate,        
'Cash' As AssetClass,          
CF.MasterFundName As MasterFund,          
CF.FundName As Account,                                         
'Cash-' + CurrencyLocal.CurrencySymbol As Symbol,      
'Cash' AS PositionSide,      
'Cash-' + CurrencyLocal.CurrencySymbol As SecurityName,                                          
Cash.CashValueLocal as Quantity,        
0.0 As MarkPrice,        
Cash.CashValueLocal As MarketValue_Local,         
Cash.CashValueBase As MarketValue_Base,       
0.0 As CostBasis_Local,        
0.0 As CostBasis_Base,        
1 As ContractSize,        
'' As ISINSymbol,        
'' As CUSIPSymbol,        
'' As SEDOLSymbol,        
'' As OSISymbol,       
CurrencyLocal.CurrencySymbol As TradeCurrency,        
CurrencyBase.CurrencySymbol As PortfolioCurrency ,        
'' As BloombergSymbol,      
Case       
 When Cash.LocalCurrencyID = CF.LocalCurrency       
 Then 1      
Else IsNull(FXDayRatesForEndDate.RateValue,0)       
End As EndDateFXRate,      
0.0 AS PercentEquity,      
0.0 AS Nav,      
1 As CustomOrder                                   
From PM_CompanyFundCashCurrencyValue Cash  With (NoLock)                             
Inner join #Fund CF On CF.FundId = Cash.FundID                              
Inner join T_Currency CurrencyLocal  With (NoLock) On CurrencyLocal.CurrencyId = Cash.LocalCurrencyID                                        
Inner join T_Currency CurrencyBase  With (NoLock) On CurrencyBase.CurrencyId = Cash.BaseCurrencyID       
LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (                      
   FXDayRatesForEndDate.FromCurrencyID = Cash.LocalCurrencyID               
AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency                                                                                                                   
   AND DateDiff(d, @InputDate, FXDayRatesForEndDate.DATE) = 0                      
   AND FXDayRatesForEndDate.FundID = Cash.FundID                    
    )       
  LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (           
   ZeroFundFxRateEndDate.FromCurrencyID = Cash.LocalCurrencyID                     
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
Where DateDiff(Day, Cash.Date, @inputDate) = 0        
    
Select *        
From #Temp_FinalTable        
--Where AssetClass = 'Accrual'        
Order By CustomOrder, MasterFund,Account,Symbol          
         
        
Drop Table #TempOpenPositionsTable, #TempTable,#Temp_FinalTable, #Fund       
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates,#ZeroFundMarkPriceEndDate,#ZeroFundFxRate,#TempAccountNAV