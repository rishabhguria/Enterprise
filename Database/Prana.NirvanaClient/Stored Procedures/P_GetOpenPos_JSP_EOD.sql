  
  
create Procedure [dbo].[P_GetOpenPos_JSP_EOD]                          
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
--    Declare   
--@ThirdPartyID int,                                                
--@CompanyFundIDs varchar(max),                                                                                                                                                                              
--@InputDate datetime,                                                                                                                                                                          
--@CompanyID int,                                                                                                                                          
--@AUECIDs varchar(max),                                                                                
--@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                
--@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                
--@FileFormatID int     
  
  
--Set @ThirdPartyID  = 56                                              
--Set @CompanyFundIDs   = N'1,2,3,4,5'                                                                                                                                                                         
--Set @InputDate  = '2024-11-06'                                                                                                                                                                         
--Set @CompanyID      =7                                                                                                                                    
--Set @AUECIDs=N'20,43,21,18,61,74,1,15,11,62,73,12,80,32,81'                                                                               
--Set @TypeID    =0                                             
--Set @DateType    =0                                                                                                                                                                    
--Set @FileFormatID  =120   
  
         
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
Case     
 When G.CurrencyID = CF.LocalCurrency     
 Then 1    
Else IsNull(FXRatesForEndDate.Val,0)     
End As FXRate_EndDate,    
IsNull((PT.TaxlotOpenQty * MP_FundWise.FinalMarkPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) AS MarketValue,    
Cast(0 As float) AS MarketValueBase  
  
Into #TempOpenPositionsTable             
From PM_Taxlots PT    
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK     
INNER JOIN T_Group G ON G.GroupID = PT.GroupID    
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol     
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID     
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
Set MarketValueBase = MarketValue * FXRate_EndDate    
    
Select                   
PT.AccountName As AccountName,                  
Sum(MarketValue) AS MarketValue,    
Sum(MarketValueBase) AS MarketValueBase,    
Convert(varchar(10),@InputDate,112) As InputDate    
Into #TempGroupedOpenPosTable                  
From #TempOpenPositionsTable PT     
Group By PT.AccountName     
    
    
Select                   
CF.FundName As AccountName,          
EODCash.CashValueLocal As MarketValue,          
EODCash.CashValueBase As MarketValueBase,    
Convert(varchar(10),@InputDate,112) As InputDate   
Into #TempEODCash                 
From PM_CompanyFundCashCurrencyValue EODCash    
Inner Join @Fund Fund On Fund.FundID = EODCash.FundID    
inner join T_CompanyFunds CF on EODCash.FundID = CF.CompanyFundID    
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
Sum(MarketValue) AS MarketValue,                   
Sum(MarketValueBase) AS MarketValueBase,    
Max(InputDate) AS InputDate   
From #TempEODCash EODCash      
Group By EODCash.AccountName    
    
Select AccountName, sum(MarketValueBase) as MarketValueBase From #TempGroupedOpenPosTable    
--Where AssetClass = 'Cash'   
group by AccountName
Order By AccountName  
    
Drop Table #TempOpenPositionsTable, #TempGroupedOpenPosTable,#TempEODCash    
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates,#ZeroFundFxRate