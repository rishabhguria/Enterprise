        
        
CREATE Procedure [dbo].[P_GetBBGOpenPos_Daily_Cash_JPS_EOD]                                
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
        
        
Declare @Fund Table                                                                   
(                        
FundID int                              
)          
        
Insert into @Fund                                                                                                            
Select Cast(Items As Int) from dbo.Split(@companyFundIDs,',')         
        
-- get Mark Price for End Date                                
CREATE TABLE #MarkPriceForEndDate           
(            
  Finalmarkprice Float            
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
FROM PM_DayMarkPrice DMP With (NoLock)        
Inner Join @Fund F On F.FundID = DMP.FundID          
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0          
        
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
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @InputDate ,@InputDate        
        
Update #FXConversionRates                            
Set RateValue = 1.0/RateValue                            
Where RateValue <> 0 and ConversionMethod = 1           
                 
-- For Fund Zero                      
SELECT * INTO #ZeroFundFxRate                      
FROM #FXConversionRates                      
WHERE fundID = 0        
        
        
SELECT PT.Taxlot_PK            
InTo #TempTaxlotPK                 
FROM PM_Taxlots PT With (NoLock)               
Inner Join @Fund Fund on Fund.FundID = PT.FundID                      
Where PT.Taxlot_PK In                                               
 (                                                                                                        
   Select Max(Taxlot_PK) from PM_Taxlots With (NoLock)                                                                                           
   Where DateDiff(DAY, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                              
   group by TaxlotId                                                              
 )                                       
And PT.TaxLotOpenQty > 0               
            
Select                       
CF.FundName As AccountName,                      
PT.Symbol,                      
Case                              
 When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1                              
 Then 'Long'                              
 Else 'Short'                              
End as PositionIndicator,          
PT.TaxlotOpenQty As OpenPositions,        
SM.BloombergSymbol,        
Curr.CurrencySymbol As LocalCurrency,        
SM.Multiplier As AssetMultiplier,        
SM.CompanyName As SecurityDescription,         
Case         
 When G.IsSwapped  = 1        
 Then 'EquitySwap'        
 Else A.AssetName         
End As AssetClass,        
SM.ISINSymbol,        
SM.SEDOLSymbol,        
SM.OSISymbol,        
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) TotalCost_Local,         
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,        
IsNull(MP_FundWise.Finalmarkprice,0) As MarkPrice,        
Case         
 When SM.CurrencyID = CF.LocalCurrency         
 Then 1        
Else IsNull(FXRatesForEndDate.Val,0)         
End As FXRate,        
IsNull((PT.TaxlotOpenQty * MP_FundWise.Finalmarkprice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) AS MarketValue,        
SM.CUSIPSymbol,      
UDA.CustomUDA1 AS CustomUDA1,      
UDA.CustomUDA2 AS CustomUDA2,      
UDA.CustomUDA3 AS CustomUDA3      
Into #TempOpenPositionsTable                 
From PM_Taxlots PT With (NoLock)        
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK         
INNER JOIN T_Group G With (NoLock) ON G.GroupID = PT.GroupID        
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol      
LEFT OUTER JOIN V_UDA_DynamicUDA UDA ON UDA.Symbol_PK = SM.Symbol_PK      
Inner Join T_Currency Curr With (NoLock) On Curr.CurrencyID = SM.CurrencyID          
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = PT.FundID        
Inner Join T_Asset A With (NoLock) On A.AssetID = SM.AssetID        
LEFT OUTER JOIN #MarkPriceForEndDate MP_FundWise ON (PT.Symbol = MP_FundWise.Symbol And MP_FundWise.FundID = PT.FundID)         
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
        
        
Select                       
PT.AccountName As AccountName,                      
PT.Symbol,                      
PT.PositionIndicator As PositionIndicator,           
Sum(PT.OpenPositions * PT.SideMultiplier) As OpenPositions,         
Max(BloombergSymbol) As BloombergSymbol,         
Max(LocalCurrency) As LocalCurrency,         
Max(PT.AssetMultiplier) As AssetMultiplier,        
Max(SideMultiplier) AS SideMultiplier,               
Max(SecurityDescription) As SecurityDescription,        
Sum(PT.TotalCost_Local) As TotalCost_Local,         
AssetClass As AssetClass,        
Max(ISINSymbol) As ISINSymbol,        
Max(SEDOLSymbol) As SEDOLSymbol,        
Max(CUSIPSymbol) As CUSIPSymbol,        
Max(MarkPrice) As MarkPrice,        
Max(FXRate) As FXRate,        
Max(OSISymbol) As OSISymbol,       
Max(CustomUDA1) As CustomUDA1,      
Max(CustomUDA2) As CustomUDA2,      
Max(CustomUDA3) As CustomUDA3,      
Sum(MarketValue) AS MarketValue        
Into #TempGroupedOpenPosTable                      
From #TempOpenPositionsTable PT         
Group By PT.AccountName,PT.AssetClass,PT.Symbol,PT.PositionIndicator        
        
        
Alter Table #TempGroupedOpenPosTable              
Add UnitCost Float Null          
        
        
Alter Table #TempGroupedOpenPosTable              
Add UnitCostLocal Float Null            
              
UPdate #TempGroupedOpenPosTable              
Set UnitCost = 0.0           
        
        
UPdate #TempGroupedOpenPosTable              
Set UnitCostLocal = 0.0             
              
UPdate #TempGroupedOpenPosTable              
Set UnitCost =               
Case                
 When OpenPositions <> 0 And AssetMultiplier <> 0                
 Then (TotalCost_Local/OpenPositions) /AssetMultiplier                
 Else 0               
End          
          
UPdate #TempGroupedOpenPosTable              
Set UnitCostLocal = FXRate * (              
Case                
 When OpenPositions <> 0 And AssetMultiplier <> 0                
 Then (TotalCost_Local/OpenPositions) /AssetMultiplier                
 Else 0                
End         
)         
        
--select * from #TempGroupedOpenPosTable        
        
Select         
Convert(varchar, @InputDate,23) As InputDate,        
AccountName,        
Symbol,        
PositionIndicator,        
OpenPositions As OpenPositions,        
BloombergSymbol,        
LocalCurrency,        
AssetMultiplier,        
SideMultiplier,        
SecurityDescription,        
TotalCost_Local,        
AssetClass,        
ISINSymbol,        
SEDOLSymbol,        
CUSIPSymbol,        
MarkPrice,        
 FXRate,        
OSISymbol,      
CustomUDA1,      
CustomUDA2,      
CustomUDA3,      
MarketValue,        
UnitCost,        
UnitCostLocal,        
2 As CustomOrder        
        
InTo #Temp_FinalTable        
From #TempGroupedOpenPosTable T             
        
Insert into #Temp_FinalTable         
Select         
Convert(varchar, @InputDate,23) As InputDate,                       
CF.FundName As AccountName,                      
CC.CurrencySymbol As Symbol ,         
Case                              
 When EODCash.CashValueLocal >= 0                            
 Then 'Long'                              
 Else 'Short'                              
End as PositionIndicator,           
EODCash.CashValueLocal As OpenPositions,        
'' As BloombergSymbol,         
CC.CurrencySymbol As LocalCurrency,         
'' As AssetMultiplier,        
'' AS SideMultiplier,              
CC.CurrencyName As SecurityDescription,        
'' As TotalCost_Local,         
'Cash' As AssetClass,        
'' As ISINSymbol,        
'' As SEDOLSymbol,        
'' As CUSIPSymbol,        
1 As MarkPrice,        
Case         
 When EODCash.LocalCurrencyID = CF.LocalCurrency         
 Then 1        
Else IsNull(FXDayRatesForEndDate.RateValue,0)         
End As FXRate,        
'' As OSISymbol,      
'' AS CustomUDA1,      
'' AS CustomUDA2,      
'' AS CustomUDA3,      
EODCash.CashValueLocal AS MarketValue,        
1 As UnitCost,        
1 AS UnitCostLocal,        
1 As CustomOrder                     
From PM_CompanyFundCashCurrencyValue EODCash With (NoLock)        
Inner Join @Fund Fund On Fund.FundID = EODCash.FundID        
inner join T_CompanyFunds CF With (NoLock) on EODCash.FundID = CF.CompanyFundID        
inner join T_Currency CC With (NoLock) on EODCash.LocalCurrencyID=CC.CurrencyID        
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
         
        
Select * From #Temp_FinalTable        
Order By AccountName,Symbol          
        
Drop Table #TempOpenPositionsTable, #TempGroupedOpenPosTable,#Temp_FinalTable        
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates,#ZeroFundFxRate