--EXEC [PMGetWinnerLoserONPnL] '2015/5/1','2015/5/1',1,'C','S',1  
  
CREATE PROCEDURE  [dbo].[PMGetWinnerLoserONPnL]                                                                                                                                                      
(                                                                                                                                                      
 -- Add the parameters for the stored procedure here                                     
 @StartDate DateTime,                                  
 @EndDate DateTime,                                                                                                                                                  
 @TopX int,                                                                                                                                                    
 @TopBy char(50),-- Symbol or Underlying Symbol                                    
 @OrderBy char(20),--Winners or Losers                                  
 @FundID varchar(max)                                   
)                                                                                                                                                      
AS                                                                                                                                                      
BEGIN                                                                                                                                                      
 SET NOCOUNT ON;                                     
                                    
--Declare @StartDate datetime                                                                                                                                                       
--Declare @EndDate datetime                                      
--                                    
--Set @StartDate = GetUTCDate()                                    
--Set @EndDate = GetUTCDate()                                    
                                    
                                                                                                              
Declare @MinTradeDate DateTime                                                                                                               
Declare @BaseCurrencyID int                                                                                                                                      
Set @BaseCurrencyID=(select top 1 BaseCurrencyID from T_Company where companyId <> -1)                                                                                                                  
                                                                                                              
                                       
Declare @Fund Table           
(                    
 FundID int                               
)                                     
                                  
Insert into @Fund                                    
Select Cast(Items as int) from dbo.Split(@FundID,',')                                  
                                  
-- get Mark Price for Start Date                                                                                                                                                     
Declare @MarkPriceForStartDate   Table                        
(                                  
Finalmarkprice float ,                           
Symbol varchar(50)                                                
)                                                     
                                   
-- get Mark Price for End Date                                                         
Declare @MarkPriceForEndDate  Table                                                                                       
(                                           
Finalmarkprice float ,                                                            
Symbol varchar(50)                                                                   
)                                                                      
-- get forex rates for 2 date ranges                                                                                                              
Declare @FXConversionRates Table                                                                                                                                              
(                                                                           
 FromCurrencyID int,                                                                                      
 ToCurrencyID int,                                                                                                                                                    
 RateValue float,                                                                                                             
 ConversionMethod int,                                                                                                                                                     
 Date DateTime,                                                                                                                                                             
 eSignalSymbol varchar(max),  
FundID Int                                                                                                                  
)                                                                                              
                                                                                            
-- get yesterday business day AUEC wise                                                            
Declare @AUECYesterDates TABLE                                                                                                                    
(                                                                                                                      
   AUECID INT,                                                                                                                      
   YESTERDAYBIZDATE DATETIME                                                                                                                      
)                         
-- get business day AUEC wise for End Date                                                          
Declare @AUECBusinessDatesForEndDate TABLE                                                                          
(                                                                                         
   AUECID INT,                                                                         
   YESTERDAYBIZDATE DATETIME                                                                                                                                    
)                                                                                    
-- get Security Master Data in a Temp Table                                                            
Declare @SecMasterDataTempTable Table                                                                                                      
(                                                  
  AUECID int,                                                                                                         
  TickerSymbol Varchar(500),                                                                                    
  CompanyName  VarChar(500),                                                            
  AssetName Varchar(100),                                                                                                                                
  SecurityTypeName Varchar(200),                                                                                             
  SectorName Varchar(100),                                      
  SubSectorName Varchar(100),            
  CountryName  Varchar(100),                                                            
  PutOrCall Varchar(5),                      
  Multiplier Float,                                                            
  LeadCurrencyID int,                                                    
  VsCurrencyID int,                                    
  UnderlyingSymbol varchar(500),                  
  CurrencyID int,            
  AssetID int                              
)                                                            
                                            
Insert Into @SecMasterDataTempTable                                                            
Select                                                             
 AUECID ,                                         
 TickerSymbol ,                                                                                                        
 CompanyName  ,                                  
 AssetName,                                                            
 SecurityTypeName,                                                                                                                                                             
 SectorName ,                                                                                                                                                          
 SubSectorName ,                                                                                     
 CountryName ,                                                            
 PutOrCall ,                                                            
 Multiplier ,                                                            
 LeadCurrencyID ,                                                            
 VsCurrencyID,                                    
 UnderlyingSymbol,                  
 CurrencyID ,            
 AssetID                                                            
     From V_SecMasterData                                                            
                                                                                         
                                                                                                                              
  Set @MinTradeDate =(Select min(PT.AUECModifiedDate) as TradeDate                                                                                                                        
   from PM_Taxlots PT  where datediff(d,PT.AUECModifiedDate,@EndDate) >=0 and taxlotclosingid_fk is null)                                                                                             
                                                                             
  If (DateDiff(d,@StartDate,@MinTradeDate) >0)                                                                                             
Begin                                                                                                          
   Set @MinTradeDate = @StartDate                                                                         
 End                                                                                                                                  
 -- Insert statements for procedure here                            
                           
  INSERT INTO @MarkPriceForStartDate Select * From dbo.GetMarkPriceForBusinessDay(@StartDate)                                                                                                  
                                                                                    
  INSERT INTO @MarkPriceForEndDate Select * From dbo.GetMarkPriceForBusinessDay(DateAdd(d,1,@EndDate))                                                                                             
                                                                    
-- Yesterday business date                                                                    
   INSERT INTO @AUECYesterDates                                                                                             
     Select Distinct V_SymbolAUEC.AUECID, dbo.AdjustBusinessDays(@StartDate,-1, V_SymbolAUEC.AUECID)                                    
     from V_SymbolAUEC                                                           
                                     
   INSERT INTO @AUECBusinessDatesForEndDate                                                                             
     Select Distinct V_SymbolAUEC.AUECID, dbo.AdjustBusinessDays(DateAdd(d,1,@EndDate),-1, V_SymbolAUEC.AUECID)                                                                                                              
     from V_SymbolAUEC                                                                                                                        
  Insert into @FXConversionRates                                                                                                                  
  Select * from  dbo.GetAllFXConversionRatesForGivenDateRange(@MinTradeDate,@EndDate) as A                                                                                             
                                                                                                                                    
  Update @FXConversionRates                                                
  Set RateValue = 1.0/RateValue                                                                                          
  Where RateValue <> 0 and ConversionMethod = 1                                                                                          
                                                                                            
 Update @FXConversionRates                                                                                          
  Set RateValue = 0                                                                                          
  Where RateValue is Null                                  
                                    
Create Table #TempOutput                                    
(                                    
                                   
 Symbol varchar(500),                                    
 UnderlyingSymbol        varchar(500),                                    
 TaxLotOpenQty   float,                                                      
 AvgPrice    float,                                                          
 ClosingPrice   float,                                                       
 TotalOpenCommission  float,                                                
 TotalClosedCommission float,                                              
 Mark1     float,                                                              
 Mark2     float,                                                              
 MarketValue1   float,                                                    
 MarketValue2   float,                                                     
 PositionFrom   CHAR(1),                                                      
 ConversionRateTrade  float,                                                 
 ConversionRateStart  float,                                                 
 ConversionRateEnd  float,                                           
 CompanyName    varchar(max),                                       
 Dividend    float,                                    
 RealizedPNL    float,                                    
 UnRealizedPNL   float,                              
 TradeDate DateTime ,            
AssetID int                             
)                                     
                                    
Insert Into #TempOutput                                                                                                                                                   
 Select                                    
                                                                  
  PT.Symbol    as Symbol,                                  
  SM.UnderlyingSymbol As UnderlyingSymbol,                                                                              
  PT.TaxLotOpenQty  as TaxLotOpenQty ,                                         PT.AvgPrice    as AvgPrice ,                                                                                                                                            
  0      as ClosingPrice,                                                                                            
Case                                               
When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                                   
Then                                                                
 Case @BaseCurrencyID                                                                 
  When SM.VsCurrencyID                                                                 
  Then IsNull(PT.OpenTotalCommissionAndFees,0)                                                                
  Else                                                               
   Case                                                                                                    
    When (G.FXRate > 0 And G.FXConversionMethodOperator='M')                                              
    Then IsNull(PT.OpenTotalCommissionAndFees * G.FXRate,0)                                                                                                       
    When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                      
    Then IsNull(PT.OpenTotalCommissionAndFees * 1/G.FXRate,0)                                                                                                       
    Else IsNull(PT.OpenTotalCommissionAndFees * IsNull(FXDayRatesForTradeDateForForex.RateValue,0),0)                                                                      
   End                                                                
 End                                                                
Else                                                                                         
 Case                                                                                                                          
 When G.CurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                                                                                    
 Then IsNull(PT.OpenTotalCommissionAndFees,0)                                                                                                                  
 Else  --When Company and Traded Currency are different                                                                     
  Case                                                                                                    
   When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                    
   Then IsNull(PT.OpenTotalCommissionAndFees * G.FXRate,0)                                         
   When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                      
   Then IsNull(PT.OpenTotalCommissionAndFees * 1/G.FXRate,0)                                                              
   When G.FXRate <= 0 OR G.FXRate is null                                        
   Then  IsNull(PT.OpenTotalCommissionAndFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                    
  End              
 End                                                                                                  
End as TotalOpenCommission,                                                                                             
                                                                                                            
0 as TotalClosedCommission,                                                                      
--Case                                                         
-- When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                           
-- Then IsNull(FXMarkPriceForStartDate.RateValue,0)                                                          
-- Else  IsNull(MPS.Finalmarkprice,0)                                                             
--End Mark1,        
IsNull(MPS.Finalmarkprice,0) As Mark1,                                                           
        
--Case                                                       
--When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                           
--Then IsNull(FXMarkPriceForEndDate.RateValue,0)                                                           
--Else IsNull(MPE.Finalmarkprice,0)                                                          
--End Mark2,             
IsNull(MPE.Finalmarkprice,0) As Mark2,         
                                                           
Case                                                 
When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                                   
Then                                                                
Case @BaseCurrencyID                                           
 When SM.VsCurrencyID                                                                 
 Then                                                                 
  Case                                                                
   When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                                           
   Then IsNull(PT.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                                         
  
    
   Else IsNull(IsNull(MPS.FinalMarkPrice,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                      
  End                                                                 
 Else                                                                
  Case                                                                                                                           
   When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                                           
   Then                                                               
    Case                                                                                      
     When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                
     Then IsNull(PT.AvgPrice * G.FXRate * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)        
     When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                             
     Then IsNull(PT.AvgPrice * 1/G.FXRate * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                             
     Else IsNull(PT.AvgPrice * IsNull(FXDayRatesForTradeDateForForex.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                              
    End                               
   Else IsNull(IsNull(MPS.FinalMarkPrice,0) * TaxlotOpenQty * FXDayRatesForStartDateForForex.RateValue * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                              
  
   
  End                                                                      
 End                                                                
Else   -- if Not Forex Trade                                               
 Case                                                                                                                                     
  When G.CurrencyID =  @BaseCurrencyID                   
  Then                                                                                        
   Case                                                                                                        
    When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                                           
    Then IsNull(PT.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                          
    Else IsNull(IsNull(MPS.Finalmarkprice,0)* TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                     
   End                                                                                          
  Else                                                                                                                               
   Case                                                                                               
    When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                     
    Then                                                                                                      
     Case                                                                                      
      When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                
      Then IsNull(PT.AvgPrice * G.FXRate * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                      
      When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                     
      Then IsNull((PT.AvgPrice * 1/G.FXRate) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                      
      When G.FXRate <= 0 OR G.FXRate is null                                                                                                                                
      Then IsNull(PT.AvgPrice * IsNull(FXDayRatesForTradeDate.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                          
  
   
      
         
     End                                                                                                        
    Else IsNull(IsNull(MPS.Finalmarkprice,0) * IsNull(FXDayRatesForStartDate.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                    
   End                                                                                                                          
 End                                                                  
End  as  MarketValue1,                                                                                                                                   
                                                                                                                  
Case                                                                  
 When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                   
 Then                                                                
  Case @BaseCurrencyID                                                                 
   When SM.VsCurrencyID                                                                 
   Then IsNull(IsNull(MPE.Finalmarkprice,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)              
   Else IsNull(IsNull(MPE.Finalmarkprice,0) * TaxlotOpenQty * IsNull(FXDayRatesForEndDateForForex.RateValue,0) * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                              
  End                                                                
 Else                                                                
  Case                                                                                                
   When G.CurrencyID =  @BaseCurrencyID                                                                                                                            
   Then IsNull(IsNull(MPE.Finalmarkprice,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                 
   Else IsNull(IsNull(MPE.Finalmarkprice,0) * IsNull(FXDayRatesForEndDate.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                            
  End                                                      
End  as  MarketValue2 ,                                                                                                                     
                                                                                           
'O' as PositionFrom,                                                           
Case                                                          
 When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                          
 Then  IsNull(FXDayRatesForTradeDateForForex.RateValue,0)                                                          
 Else IsNull(FXDayRatesForTradeDate.RateValue,0)                                                          
End  as ConversionRateTrade,                                                                                    
Case                                                       
 When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                             
 Then IsNull(FXDayRatesForStartDateForForex.RateValue,0)                                                          
 Else IsNull(FXDayRatesForStartDate.RateValue,0)                                                          
End as ConversionRateStart,                                                          
Case                                                        
 When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                            
 Then IsNull(FXDayRatesForEndDateForForex.RateValue,0)                                                          
 Else IsNull(FXDayRatesForEndDate.RateValue,0)                                                 
End as ConversionRateEnd,                                                          
                                                                                                                             
SM.CompanyName,                                                                                                            
  0 as Dividend,                                    
0 As RealizedPNL,                                    
0 As UnRealizedPNL,                              
G.ProcessDate as TradeDate,            
G.AssetID                                                           
                                                          
 from PM_Taxlots PT                          
  Left Outer Join @MarkPriceForStartDate MPS on MPS.Symbol=PT.Symbol                                                                                                                                      
  Left Outer Join @MarkPriceForEndDate MPE on MPE.Symbol=PT.Symbol                                                                                            
 Inner Join T_Group G on G.GroupID = PT.GroupID                                                                    
  -- join to get yesterday business day                                                          
  LEFT OUTER JOIN @AUECYesterDates AUECYesterDates ON G.AUECID = AUECYesterDates.AUECID                          
  LEFT OUTER JOIN @AUECBusinessDatesForEndDate AUECBusinessDatesForEndDate ON G.AUECID = AUECBusinessDatesForEndDate.AUECID                                                              
  -- Forex Price for Trade Date other than FX Trade                                                                               
  Left outer join @FXConversionRates FXDayRatesForTradeDate                                                           
 on (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID                                                           
 And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                           
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDate.Date)=0)                                                               
  -- Forex Price for Start Date other than FX Trade                                                                  
 Left outer join @FXConversionRates FXDayRatesForStartDate                                       
 on (FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID                                                           
 And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                                           
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDate.Date)=0)                                                               
-- Forex Price for End Date other than FX Trade                                                                                           
  Left outer join @FXConversionRates FXDayRatesForEndDate                                                           
 on (FXDayRatesForEndDate.FromCurrencyID = G.CurrencyID                                                           
 And FXDayRatesForEndDate.ToCurrencyID = @BaseCurrencyID                                                           
 And DateDiff(d,AUECBusinessDatesForEndDate.YESTERDAYBIZDATE,FXDayRatesForEndDate.Date)=0)                                                             
-- Security Master DB Join                                                                                           
  Left outer join @SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                                   
-- Mark Price for Start Date for FX Trade                                                              
  Left outer  join @FXConversionRates FXMarkPriceforStartDate                                                           
 on (FXMarkPriceforStartDate.FromCurrencyID = SM.LeadCurrencyID                                                           
 And FXMarkPriceforStartDate.ToCurrencyID = SM.VsCurrencyID                                                           
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXMarkPriceforStartDate.Date)=0)                                                               
-- Mark Price for End Date for FX Trade                                                              
  Left outer  join @FXConversionRates FXMarkPriceforEndDate                                                            
 on (FXMarkPriceforEndDate.FromCurrencyID = SM.LeadCurrencyID                                                           
 And FXMarkPriceforEndDate.ToCurrencyID = SM.VsCurrencyID                                                           
 And DateDiff(d,AUECBusinessDatesForEndDate.YESTERDAYBIZDATE,FXMarkPriceforEndDate.Date)=0)                                                                   
--Forex Price for Trade Date for other than FX Trade                                                                
  Left outer  join @FXConversionRates FXDayRatesForTradeDateForForex                                                           
 on (FXDayRatesForTradeDateForForex.FromCurrencyID = SM.VsCurrencyID                                                           
 And FXDayRatesForTradeDateForForex.ToCurrencyID = @BaseCurrencyID                                                           
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDateForForex.Date)=0)               
--Forex Price for Start Date for other than FX Trade                                          
  Left outer  join @FXConversionRates FXDayRatesForStartDateForForex                                                           
 on (FXDayRatesForStartDateForForex.FromCurrencyID = SM.VsCurrencyID                                                           
 And FXDayRatesForStartDateForForex.ToCurrencyID = @BaseCurrencyID                                                           
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDateForForex.Date)=0)                                                                
--Forex Price for End Date for FX Trade                                                                 
  Left outer  join @FXConversionRates FXDayRatesForEndDateForForex                                                           
 on (FXDayRatesForEndDateForForex.FromCurrencyID = SM.VsCurrencyID                                                           
 And FXDayRatesForEndDateForForex.ToCurrencyID = @BaseCurrencyID                                                           
 And DateDiff(d,AUECBusinessDatesForEndDate.YESTERDAYBIZDATE,FXDayRatesForEndDateForForex.Date)=0)                   
                                    
Where TaxLotOpenQty<>0 and PT.FundID in (select FundID from @Fund)                                                                               
   and taxlot_PK in                                                                                                                                                                           
   (                                                   
    select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                                           
    where DateDiff(d,PM_Taxlots.AUECModifiedDate,@EndDate) >=0                                                       
    group by taxlotid                                                                                                                                                      
  )                                                                                                                                  
                                                                                                                                                     
                                                                                      
 Union All                                                                                        
                                
 select                                    
                                                                                             
  PT.Symbol as Symbol,                                    
  SM.UnderlyingSymbol As UnderlyingSymbol,        
  PTC.ClosedQty   as ClosedQty ,                                                                                                                     
  PT.AvgPrice    as AvgPrice ,                                                         
  IsNull(PT1.AvgPrice,0)as ClosingPrice ,                                                                
Case                                                                                         
When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                                   
Then                                                                
 Case @BaseCurrencyID                                 
  When SM.VsCurrencyID                                                                 
  Then IsNull(PT.ClosedTotalCommissionandFees,0)                                                              
  Else                                                              
Case                                                                                                    
    When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                  
    Then IsNull(PT.ClosedTotalCommissionandFees * G.FXRate,0)                                                      
    When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                
    Then IsNull(PT.ClosedTotalCommissionandFees * 1/G.FXRate,0)                                                                                                       
    Else IsNull(PT.ClosedTotalCommissionandFees * IsNull(FXDayRatesForTradeDateForForex.RateValue,0),0)                                                                      
   End                                       
 End                                                              
Else                                           
 Case                                                                                                                                     
  When G.CurrencyID = @BaseCurrencyID   --When Company and Traded Currency both are same                                                                                                                                 
  Then IsNull(PT.ClosedTotalCommissionandFees,0)                                                                                                                      
  Else  --When Company and Traded Currency are different                                                                     
   Case                                                                                                    
    When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                    
    Then IsNull(PT.ClosedTotalCommissionandFees * G.FXRate,0)                                                                                                       
    When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                      
    Then IsNull(PT.ClosedTotalCommissionandFees * 1/G.FXRate,0)                                                                                                       
    When G.FXRate <= 0 OR G.FXRate is null                                                                          
    Then  IsNull(PT.ClosedTotalCommissionandFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                     
   End                                                                  
 End                                                                                                          
End as TotalOpenCommission,                                                                                                                        
 --Closed Commission                                                         
Case                                                        
When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                    
Then                  
 Case @BaseCurrencyID                                                                 
  When SM.VsCurrencyID                                                                 
  Then IsNull(PT1.ClosedTotalCommissionandFees,0)                                             
  Else  --When Company and Traded Currency are different                                                  
   Case                                                                                  
    When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                                                                  
    Then IsNull(PT1.ClosedTotalCommissionandFees * G1.FXRate,0)                            
    When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                                             
    Then IsNull(PT1.ClosedTotalCommissionandFees * 1/G1.FXRate,0)                                              
    Else IsNull(PT1.ClosedTotalCommissionandFees * IsNull(FXDayRatesForClosingDateForForex.RateValue,0),0)                                                      
   End                                                                
 End                                                               
Else                                                                                                                  
 Case                                                                                                                                     
  When G.CurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                                                                                                 
  Then IsNull(PT1.ClosedTotalCommissionandFees,0)                                                                                                                      
  Else  --When Company and Traded Currency are different                                                                      
   Case                                                                                                    
    When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                                                                    
    Then IsNull(PT1.ClosedTotalCommissionandFees * G1.FXRate,0)                                                                                
    When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                                                                      
    Then IsNull(PT1.ClosedTotalCommissionandFees * 1/G1.FXRate,0)                                                                                                       
    When G1.FXRate <= 0 OR G1.FXRate is null                                                          
    Then  IsNull(PT1.ClosedTotalCommissionandFees * IsNull(FXDayRatesForClosingDate.RateValue,0),0)                                                                      
   End                                                                                                                         
 End                                                                                            
End as TotalClosedCommission,         
                                                            
--Case                                          
-- When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                           
-- Then IsNull(FXMarkPriceForStartDate.RateValue,0)                                                          
-- Else IsNull(MPS.FinalMarkPrice,0)                                                             
--End Mark1,          
IsNull(MPS.FinalMarkPrice,0) As Mark1,                                                         
--Case                                                        
-- When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                        
-- Then 0                                                           
-- Else IsNull(MPE.FinalMarkPrice,0)                                                       
--End Mark2,          
IsNull(MPE.FinalMarkPrice,0) As Mark2,        
                     
Case            
When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                              
Then                                                              
 Case @BaseCurrencyID                                                              
  When SM.VsCurrencyID                                                
  Then                                                               
   Case                                                                                     
    When DateDiff(d,G.ProcessDate,@StartDate) > 0              
    Then IsNull(MPS.FinalMarkPrice,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                                        
    Else PT.AvgPrice * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                                                                  
  
    
      
       
   End                                                                
  Else                                                               
   Case                                                                                                                                                       
    When  DateDiff(d,G.ProcessDate,@StartDate) > 0                                                                                           
    Then IsNull(MPS.FinalMarkPrice,0) * IsNull(FXDayRatesForStartDateForForex.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                  
    Else                                                                                                      
     Case                                                                                                                                    
      When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                      
      Then IsNull(PT.AvgPrice * G.FXRate * PTC.ClosedQty  * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                      
      When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                               
      Then IsNull((PT.AvgPrice * 1/G.FXRate) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                      
      Else IsNull(PT.AvgPrice * IsNull(FXDayRatesForTradeDateForForex.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                  
  
    
     End                                                                                             
   End                                                               
 End                                                              
Else  --- For Normal Trade                                                               
 Case                                                                         
  When G.CurrencyID =  @BaseCurrencyID                                                                                                                                    
  Then                                                                                                                              
   Case                
    When DateDiff(d,G.ProcessDate,@StartDate) >0        
    Then IsNull(MPS.FinalMarkPrice,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                                                  
   
       
       
    Else PT.AvgPrice * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                                                                                  
   
    
      
       
   End                                                            
  Else                                                                 
   Case                  
    When  DateDiff(d,G.ProcessDate,@StartDate) > 0                                                                                           
    Then IsNull(MPS.FinalMarkPrice,0) * IsNull(FXDayRatesForStartDate.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                                                                                     
  
    
     
         
    Else                                                                                          
     Case                                                                                                                                    
      When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                      
      Then IsNull(PT.AvgPrice * G.FXRate * PTC.ClosedQty  * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                      
      When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                       
      Then IsNull((PT.AvgPrice * 1/G.FXRate) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                      
      When G.FXRate <= 0 OR G.FXRate is null                                                                                                       
      Then IsNull(PT.AvgPrice * IsNull(FXDayRatesForTradeDate.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                          
  
    
      
          
     End                                                                                                                                    
   End                                                               
 End                                                                                                          
End as  MarketValue1 ,         
                                                                                                                         
Case                                              
 When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                             
 Then                                                              
  Case @BaseCurrencyID                                                              
   When SM.VsCurrencyID                                        
   Then IsNull(PT1.AvgPrice,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                              
   Else IsNull(PT1.AvgPrice,0) * IsNull(FXDayRatesForClosingDateForForex.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                          
  
    
  End                                                              
 Else                                                              
  Case                                                         When G.CurrencyID <> @BaseCurrencyID                                                                                         
   Then                                           
    Case                          
     When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                                          
     Then IsNull(PT1.AvgPrice,0)* G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                          
     When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                                          
     Then IsNull(PT1.AvgPrice,0)* 1/G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                          
     Else IsNull(PT1.AvgPrice,0)* IsNull(FXDayRatesForClosingDate.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                          
    End                                                                                          
   Else ISNULL(PT1.AvgPrice,0)* PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                                    
  END                                                              
End as  MarketValue2,         
                                                                                                                                           
'C' as PositionFrom,                                         
Case                                                        
 When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                         
 Then IsNull(FXDayRatesForTradeDateForForex.RateValue,0)                                                          
 Else IsNull(FXDayRatesForTradeDate.RateValue,0)                                                          
End as ConversionRateTrade,                                                          
Case                                                          
 When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                            
 Then IsNull(FXDayRatesForStartDateForForex.RateValue,0)                                                          
 Else IsNull(FXDayRatesForStartDate.RateValue,0)                                                          
End as ConversionRateStart,                                                          
Case                                                           
 When G.AssetID=5 OR G.AssetID=11 -- 11 for FX Forward  and 5 for FX Spot                                                            
 Then IsNull(FXDayRatesForClosingDateForForex.RateValue,0)                                                          
 Else IsNull(FXDayRatesForClosingDate.RateValue,0)                                                          
End as ConversionRateClosing,                                                          
                                                                       
  SM.CompanyName,                                                                                                      
  0 as Dividend,                                    
0 As RealizedPNL,                                    
0 As UnRealizedPNL,                              
G.ProcessDate   as TradeDate,            
G.AssetID                                                           
                                                           
                                                                           
  from PM_TaxlotClosing  PTC                                                                                                                        
  Inner Join PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                       
  Inner Join PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                                                                                                                          
  
   
       
       
  Inner Join T_Group G on G.GroupID = PT.GroupID                                                                                                                                                                              
  Inner Join T_Group G1 on G1.GroupID = PT1.GroupID                                     
  Left Outer Join @MarkPriceForStartDate MPS on MPS.Symbol=PT.Symbol                                                                                                 
  Left Outer Join @MarkPriceForEndDate MPE on MPE.Symbol=PT.Symbol                                                         
  --get yesterday business day                                                          
  LEFT OUTER JOIN @AUECYesterDates AUECYesterDates ON G.AUECID = AUECYesterDates.AUECID                                                            
  -- Security Master DB join                                                            
  LEFT OUTER JOIN @SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                                                               
  -- Mark Price for Start Date for FX Trade                                                             
  Left outer  join @FXConversionRates FXMarkPriceforStartDate                                                           
 on (FXMarkPriceforStartDate.FromCurrencyID = SM.LeadCurrencyID                                                    
 And FXMarkPriceforStartDate.ToCurrencyID = SM.VsCurrencyID                                                           
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXMarkPriceforStartDate.Date)=0)                                     
-- Forex Price for Trade Date for FX Trade                                                          
  Left outer  join @FXConversionRates FXDayRatesForTradeDateForForex                                                           
 on (FXDayRatesForTradeDateForForex.FromCurrencyID = SM.VsCurrencyID                                                           
 And FXDayRatesForTradeDateForForex.ToCurrencyID = @BaseCurrencyID                                                           
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDateForForex.Date)=0)                                                                                   
-- Forex Price for Start Date for FX Trade                                                          
  Left outer  join @FXConversionRates FXDayRatesForStartDateForForex                                                           
 on (FXDayRatesForStartDateForForex.FromCurrencyID = SM.VsCurrencyID                                                           
 And FXDayRatesForStartDateForForex.ToCurrencyID = @BaseCurrencyID                                                           
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDateForForex.Date)=0)                                                                                                                                  
-- Forex Price for Closing Date for FX Trade                                                          
  Left outer  join @FXConversionRates FXDayRatesForClosingDateForForex                                                           
 on (FXDayRatesForClosingDateForForex.FromCurrencyID = SM.VsCurrencyID                                                           
 And FXDayRatesForClosingDateForForex.ToCurrencyID = @BaseCurrencyID                                                           
 And DateDiff(d,G1.ProcessDate,FXDayRatesForClosingDateForForex.Date)=0)                                                             
  -- Forex Price for Trade Date for other than FX Trade                                                          
  Left outer  join @FXConversionRates FXDayRatesForTradeDate                         
 on (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID                                                           
 And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                         
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDate.Date)=0)                                                                           
  -- Forex Price for Start Date for other than FX Trade                                                               
  Left outer  join @FXConversionRates FXDayRatesForStartDate                                                           
 on (FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID                                                           
 And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID      
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDate.Date)=0)                                                          
  -- Forex Price for Closing Date for other than FX Trade                                                             
  Left outer  join @FXConversionRates FXDayRatesForClosingDate                                       
 on (FXDayRatesForClosingDate.FromCurrencyID = G.CurrencyID                              
 And FXDayRatesForClosingDate.ToCurrencyID = @BaseCurrencyID                                                           
 And DateDiff(d,G1.ProcessDate,FXDayRatesForClosingDate.Date)=0)                                                                  
Where DateDiff(d,@StartDate,PTC.AUECLocalDate) >=0                                                                                           
  and  DateDiff(d,PTC.AUECLocalDate,@EndDate)>=0                                          
  and  PTC.ClosingMode<>7 --7 means CoperateAction!                                       
  and PT.FundID in (select FundID from @Fund)             
                                    
Union All                                      
                                      
 Select                                   
                                                                                                                                                            
   CashDiv.Symbol,                                  
   MIN(SM.UnderlyingSymbol) As UnderlyingSymbol,                                                                 
   0  as TaxLotOpenQty ,                                                                                                                                                                        
   0  as AvgPrice ,                                               
   0  as ClosingPrice,                                                                                                                                                            
   0  as TotalOpenCommission,                                                                                                                          
   0  as TotalClosedCommission,                                                                                                                                                                           
   0  as Mark1,                                                                    
   0  as Mark2,                                                                                                                                                
   0  as  MarketValue1,                                                                                   
   0  as  MarketValue2 ,                                                                                                                                    
   'F' as PositionFrom,                                                                                                                           
   Max(IsNull(FXDayRatesForDiviDate.RateValue,0)) as ConversionRateTrade,                                                                                                                                      
   0 as ConversionRateStart,                                                                  
   0 as ConversionRateEnd,                                                                                            
   MIN(SM.CompanyName) as CompanyName,                                                                                                            
   Case                                                                                                                       
     When Min(SM.CurrencyID) <>  @BaseCurrencyID                                                                                                                       
     Then Max(IsNull(FXDayRatesForDiviDate.RateValue,0)) * Sum(CashDiv.Amount)                      
     Else Sum(CashDiv.Amount)                     
   End as Dividend,                                    
   0 As RealizedPNL,                                    
   0 As UnRealizedPNL,                              
   Min(CashDiv.ExDate) as TradeDate,            
Min(SM.AssetID) as AssetID                                                           
                                                          
  from T_CashTransactions CashDiv    
  inner JOIN T_ActivityType on (T_ActivityType.ActivityTypeId = CashDiv.ActivityTypeId and ActivitySource = 2)                                        
  --Inner Join V_Taxlots VT on CashDiv.TaxlotID = VT.TaxlotID                                                                                                            
  LEFT OUTER JOIN @SecMasterDataTempTable SM ON SM.TickerSymbol = CashDiv.Symbol                  
  Left outer  join @FXConversionRates FXDayRatesForDiviDate On (FXDayRatesForDiviDate.FromCurrencyID = SM.CurrencyID                                          
   And FXDayRatesForDiviDate.ToCurrencyID = @BaseCurrencyID And DateDiff(d,CashDiv.ExDate,FXDayRatesForDiviDate.Date)=0)                                                                                                 
  Where DateDiff(d,@StartDate,CashDiv.ExDate) >=0                                                                                         
  and  DateDiff(d,CashDiv.ExDate,@EndDate)>=0 and CashDiv.FundID in (select FundID from @Fund)                                          
  group by CashDiv.FundId,CashDiv.Symbol,CashDiv.ExDate                                    
  Order By PT.Symbol                                 
                                  
  --Select * from #TempOutput          
--Fixed Income Handling          
Update #TempOutput            
Set UnRealizedPNL =           
Case When AssetID = 8          
 Then (MarketValue2 - MarketValue1) / 100          
 Else (MarketValue2 - MarketValue1)          
End           
Where PositionFrom = 'O'          
           
            
Update #TempOutput           
Set RealizedPNL =           
Case When AssetID = 8          
 Then (MarketValue2 - MarketValue1) / 100          
 Else  (MarketValue2 - MarketValue1)          
End            
Where PositionFrom = 'C'          
                              
                            
Update #TempOutput                                    
Set RealizedPNL = (RealizedPNL - TotalOpenCommission - TotalClosedCommission)                                    
Where PositionFrom = 'C' and DateDiff(day,@StartDate,TradeDate)>=0                               
                
Update #TempOutput                                    
Set RealizedPNL = (RealizedPNL - TotalClosedCommission)                          
Where PositionFrom = 'C' and DateDiff(day,@StartDate,TradeDate)< 0                                    
                                    
Update #TempOutput                                    
Set UnRealizedPNL = (UnRealizedPNL - TotalOpenCommission)                                    
Where PositionFrom = 'O' and DateDiff(day,@StartDate,TradeDate)>=0                         
                              
--Update #TempOutput                                    
--Set UnRealizedPNL = (MarketValue2 - MarketValue1)                                    
--Where PositionFrom = 'O' and DateDiff(day,@StartDate,TradeDate)< 0             
            
                           
                                  
If @OrderBy = 'Winners'                                    
Begin                                    
 Set @OrderBy = ' DESC'                                    
End                                    
Else                            
Begin                                    
 Set @OrderBy = ' ASC'                                    
End                                    
                                    
Exec ('Select TOP(' +@TopX + ')                                       
  MIN(Symbol) As Symbol                                    
 ,MIN(UnderlyingSymbol) As UnderlyingSymbol                                    
 ,MIN(CompanyName) As CompanyName                                    
 ,SUM(RealizedPNL + UnRealizedPNL + Dividend) As TotalPNL                                    
from #TempOutput            
Group By ' + @TopBy + '                                    
Order By TotalPNL ' + @OrderBy)                                 
                                
--Select * from #TempOutput                                   
                                    
Drop Table #TempOutput                                                                                              
END 