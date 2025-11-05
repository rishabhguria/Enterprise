      
      
CREATE Procedure [dbo].[P_GetDailyvaluationMarkPrice_SS_EOD]                              
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
      
--Declare                                        
--@ThirdPartyID int,                                                  
--@CompanyFundIDs varchar(max),                                                                                                                                                                                
--@InputDate datetime,                                                                                                                                                                            
--@CompanyID int,                                                                                                                                            
--@AUECIDs varchar(max),                                                                                  
--@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                  
--@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                  
--@FileFormatID int                                        
          
          
-- set @thirdPartyID=20      
-- set @companyFundIDs=N'1,3,4,2,5,6,'      
-- set @inputDate='2025-06-04 03:47:15.717'      
-- set @companyID=2      
-- set @auecIDs=N'20,77,63,53,44,34,43,59,31,18,61,74,1,15,11,62,73,80,81,'     
-- set @TypeID=0      
-- set @dateType=1      
-- set @fileFormatID=41    
         
      
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
 ,Symbol VARCHAR(max)          
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
WHERE DateDiff(Day,DMP.[Date],@InputDate) = 0      
      
      
SELECT PT.Taxlot_PK          
InTo #TempTaxlotPK               
FROM PM_Taxlots PT                
Inner Join @Fund Fund on Fund.FundID = PT.FundID    
Where PT.Taxlot_PK in                                             
(                                                      
 Select Max(Taxlot_PK) from PM_Taxlots  With (NoLock)                                                                                       
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                         
 group by TaxlotId                                                                            
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
      
PT.Symbol,      
PT.TaxlotOpenQty As Quantity,      
IsNull(MP_FundWise.Finalmarkprice,0) As MarkPrice,      
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,      
Replace(isnull(SM.CompanyName, ''), ',', ' ') AS CompanyName,      
Isnull(Curr.CurrencySymbol,'') AS TradeCurrency,    
Case     
 When G.CurrencyID = CF.LocalCurrency     
 Then 1    
Else IsNull(FXRatesForEndDate.Val,0)     
End As FXRate_EndDate,    
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
Cast(0.00 As Float)  AS MarkPriceBase    
      
Into #TempOpenPositionsTable               
From PM_Taxlots PT With (NoLock)      
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK      
INNER JOIN T_Group G ON G.GroupID = PT.GroupID    
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol      
Inner Join T_Currency Curr With (NoLock) On Curr.CurrencyID = SM.CurrencyID     
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID    
LEFT OUTER JOIN #MarkPriceForEndDate MP_FundWise ON (PT.Symbol = MP_FundWise.Symbol)     
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
 Set MarkPriceBase =   
 CASE   
  WHEN #TempOpenPositionsTable.Symbol = 'BU.CNW2' THEN MarkPrice  
  ELSE MarkPrice * FXRate_EndDate   
 END   
      
Select                     
              
Temp.Symbol,      
Sum(Temp.Quantity * Temp.SideMultiplier) As OpenPositions,      
Max(MarkPrice) As MarkPrice,      
Temp.TradeCurrency,      
Temp.CompanyName AS CompanyName,    
Max(MarkPriceBase) As MarkPriceBase    
Into #TempTable                    
From #TempOpenPositionsTable Temp        
Group By Temp.Symbol,Temp.TradeCurrency,Temp.CompanyName                 
            
Select * from #TempTable       
Order By Symbol      
      
Drop Table #TempOpenPositionsTable, #TempTable      
Drop Table #TempTaxlotPK,#MarkPriceForEndDate,#FXConversionRates,#ZeroFundFxRate 