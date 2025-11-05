        
/*        
        
EXEC [P_GetCustomHolding_NZS_Jupiter_ForOpenPosition] 1, '1,5', '03-09-2023',1,1,1,1,1        
        
*/        
        
CREATE Procedure [dbo].[P_GetOpenPosition_NZS_Jupiter_EOD]                              
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
--@TypeID int,                                             
--@DateType int,        
--@FileFormatID int          
        
--set @thirdPartyID=116    
--set @companyFundIDs=N'2,3,1,4,5,6,7,8,16,17,18,19,20,21,'    
--set @inputDate='2025-08-14 06:08:06.963'    
--set @companyID=7    
--set @auecIDs=N'20,65,71,64,76,63,53,44,34,43,59,129,54,21,11,1,15,62,73,32,33,'    
--set @TypeID=0    
--set @dateType=1    
--set @fileFormatID=51         
        
        
        
Declare @Fund Table                                                                             
(                                  
FundID int                                        
)                    
                  
Insert into @Fund                                                                                                                      
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')           
        
        
        
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
        
--Delete FROM #MarkPriceForEndDate WHERE FundID = 0        
        
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
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @InputDate ,@InputDate        
        
        
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
SM.ISINSymbol As ISINSymbol,        
SM.SEDOLSymbol As SEDOLSymbol,        
SM.OSISymbol As OSISymbol,        
        
Case         
 When SM.CurrencyID = CF.LocalCurrency         
 Then 1        
Else IsNull(FXRatesForEndDate.Val,0)         
End As EndDateFXRate,        
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,        
TradeCurr.CurrencySymbol As TradeCurrency,        
        
TC.CurrencySymbol As SettlementCurrency,        
SM.BloombergSymbol As BloombergSymbol,        
SM.UnderLyingSymbol,      
SM.Multiplier AS ContractSize,      
UDA.CustomUDA2 AS CustomUDA2,  
SM.CUSIPSymbol AS CUSIP  
        
Into #TempOpenPositionsTable                 
From PM_Taxlots PT With (NoLock)        
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK         
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID        
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol      
LEFT OUTER JOIN  V_UDA_DynamicUDA UDA With (NoLock) ON UDA.Symbol_PK=SM.Symbol_PK      
Inner Join T_Currency TradeCurr With (NoLock) On TradeCurr.CurrencyID = SM.CurrencyID          
Inner Join T_Currency PortfolioCurr With (NoLock) On PortfolioCurr.CurrencyID = CF.LocalCurrency        
Inner Join T_Currency TC With (NoLock) On TC.CurrencyID = PT.SettlCurrency        
Inner Join T_Asset A With (NoLock) On A.AssetID = SM.AssetID        
INNER JOIN T_Group G With (NoLock) ON G.GroupID = PT.GroupID      
        
LEFT OUTER JOIN #MarkPriceForEndDate MPE ON (        
  MPE.Symbol = PT.Symbol AND MPE.FundID = PT.FundID )        
LEFT OUTER JOIN #ZeroFundMarkPriceEndDate MPZeroEndDate ON (        
  PT.Symbol = MPZeroEndDate.Symbol AND MPZeroEndDate.FundID = 0)        
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
        
-- Forex Price for Input Date                            
 LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (                        
   FXDayRatesForEndDate.FromCurrencyID = G.CurrencyID                        
AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency                                                                                                                     
   AND DateDiff(d, @InputDate, FXDayRatesForEndDate.DATE) = 0                        
   AND FXDayRatesForEndDate.FundID = PT.FundID                      
    )         
  LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (             
   ZeroFundFxRateEndDate.FromCurrencyID = G.CurrencyID                       
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
Where PT.TaxlotOpenQty > 0        
        
        
--- In base currency        
Update #TempOpenPositionsTable        
Set MarketValue_Base = MarketValue_Local * EndDateFXRate        
        
----Select * from #TempOpenPositionsTable        
        
Select          
        
Temp.Account As Account,         
AssetClass As AssetClass,            
Temp.Symbol,               
Max(SecurityName) As SecurityName,        
Sum(Temp.OpenQty * Temp.SideMultiplier) As OpenQty,         
Max(MarkPrice) As MarkPrice,        
Sum(MarketValue_Local) AS MarketValue_Local,        
Sum(MarketValue_Base) AS MarketValue_Base,             
Max(ISINSymbol) As ISINSymbol,        
Max(SEDOLSymbol) As SEDOLSymbol,        
Max(OSISymbol) As OSISymbol,        
Max(EndDateFXRate) As EndDateFXRate,        
Max(TradeCurrency) As TradeCurrency,        
Max(SettlementCurrency) As SettlementCurrency,        
Max(BloombergSymbol) As BloombergSymbol,        
Max(UnderlyingSymbol) As UnderlyingSymbol,      
MAx(ContractSize) AS ContractSize,      
Max(CustomUDA2) AS CustomUDA2,  
Max(CUSIP) AS CUSIP      
Into #TempTable_Grouped                      
From #TempOpenPositionsTable Temp          
Group By  Temp.Account,Temp.AssetClass,Temp.Symbol        
        
        
        
Select         
AssetClass,        
Account,        
Symbol,        
SecurityName,        
Round(OpenQty,0) As Quantity,        
MarkPrice,        
MarketValue_Local,        
MarketValue_Base,        
ISINSymbol,        
SEDOLSymbol,        
OSISymbol,        
EndDateFXRate,        
TradeCurrency,        
SettlementCurrency,        
BloombergSymbol,        
FORMAT(CAST(@InputDate As Date),'yyyyMMdd') AS TradeDate,      
ContractSize,      
CustomUDA2,  
CUSIP  
      
InTo #Temp_FinalTable        
From #TempTable_Grouped T        
        
        
Insert into #Temp_FinalTable                
select        
'Cash' As AssetClass,           
CF.FundName As Account,                                         
'.' + CurrencyLocal.CurrencySymbol As Symbol,        
CurrencyLocal.CurrencySymbol + '-USD X-RATE' As SecurityName,                                          
Cash.CashValueLocal as Quantity,        
0.0 As MarkPrice,        
Cash.CashValueLocal As MarketValue_Local,         
Cash.CashValueBase As MarketValue_Base,         
'' As ISINSymbol,        
'' As SEDOLSymbol,        
'' As OSISymbol,        
Case         
 When Cash.LocalCurrencyID = CF.LocalCurrency      
 Then 1        
Else IsNull(FXRatesForEndDate.Val,0)         
End As EndDateFXRate,        
        
CurrencyLocal.CurrencySymbol As TradeCurrency,        
CurrencyLocal.CurrencySymbol As SettlementCurrency,        
'' As BloombergSymbol,        
FORMAT(CAST(@InputDate As Date),'yyyyMMdd') AS TradeDate,      
'' As ContractSize,      
'' As CustomUDA2,  
'' AS CUSIP  
      
                               
From PM_CompanyFundCashCurrencyValue Cash  With (NoLock)                             
Inner Join @Fund Fund On Fund.FundID = Cash.FundID        
inner join T_CompanyFunds CF on Cash.FundID = CF.CompanyFundID                              
Inner join T_Currency CurrencyLocal  With (NoLock) On CurrencyLocal.CurrencyId = Cash.LocalCurrencyID                                        
Inner join T_Currency CurrencyBase  With (NoLock) On CurrencyBase.CurrencyId = Cash.BaseCurrencyID        
-- Forex Price for Input Date                            
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
Order By Account,Symbol          
         
        
Drop Table #TempOpenPositionsTable, #TempTable_Grouped,#Temp_FinalTable        
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates,#ZeroFundMarkPriceEndDate,#ZeroFundFxRate 