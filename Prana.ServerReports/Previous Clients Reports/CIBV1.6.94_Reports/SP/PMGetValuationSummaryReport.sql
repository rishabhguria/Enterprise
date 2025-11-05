/****************************************************************************                                                                                                                                                                                  
  
Name :   PMGetValuationSummaryReport                                                  
Date Created: 27-Nov-2008                                                                                                                                                                                     
Purpose:  Get the open positions till the date passed.                                                                                                                                                                                    
Module Name: PortfolioReports/Valuation Summary Report                                                                                                                                            
Author: Bhupesh Bareja                                                                                                                                                                                    
Parameters:                                                                                                                                                                                     
 @companyID int,                                                                                                                                                                                    
 @date datetime                                                                                                          
Date Modified:                                                   
Description:                                                                                                                                                                                       
Modified By:                                 
                                
Date Modified:  19-March-2009                                                 
Description:  FX related changes                                                                                                                                                                                     
Modified By:  Sandeep Singh         
        
Date Modified:  13-APRIL-2012                                                 
Description:  Description field added                                                                                                                                                                                    
Modified By:  Sandeep Singh                                              
                                                                
Execution StateMent:                                                                                                                                                                                     
   EXEC [PMGetValuationSummaryReport] 6, '04-12-2012'      
  
Date Modified:  21-AUG-2012                                               
Description:  ISIN field added                                                                                                                                                                                  
Modified By:  Ankit Gupta   
                                                                    
****************************************************************************/                                                                                                                                                                                  
  
CREATE PROCEDURE [dbo].[PMGetValuationSummaryReport]                                                   
(                                                                                                                                                                                    
 @companyID int,                                                                                                                                                                          
 @date datetime                                                  
)                                                          
AS              
BEGIN          
                                                  
 DECLARE @startingDate DateTime                                                                       
                                                                            
 Set @startingDate = DateAdd(yy, -20, @date)                                                                                  
 Create Table #TEMPGetConversionFactorForGivenDateRange               
 (                             
  FCID int,                      
  TCID int,                                   
  ConversionFactor float,                                                             
  ConversionMethod int,                        
 DateCC DateTime                                                                      
 )                                     
 INSERT INTO #TEMPGetConversionFactorForGivenDateRange                                                                                                                                           
 SELECT                                                                                               
  FromCurrencyID,                                                                                                                                                        
  ToCurrencyID,                                                            
  RateValue,                                                                                              
  ConversionMethod,                                                                                                              
  Date                                                                              
 FROM dbo.GetAllFXConversionRatesForGivenDateRange(@startingDate, @date)                                                  
                                                       
                                                  
 Declare @AllAUECDatesString VARCHAR(MAX)                                                                                
 Set @AllAUECDatesString = dbo.GetAUECDateString(@date)                                                                                                           
 Declare @AUECDatesTable Table                        
 (                        
   AUECID int,                        
   CurrentAUECDate DateTime                        
 )                                                                                                                                
 Insert Into @AUECDatesTable                         
 Select * From dbo.GetAllAUECDatesFromString(@AllAUECDatesString)                        
                         
 Update @AUECDatesTable                        
 Set CurrentAUECDate = dbo.AdjustBusinessDays(DateAdd(d,1,CurrentAUECDate),-1,AUECID)                        
                                                                                                                                        
 Create Table #TEMPFundPositionsForDate_ValReport                                                              
 (                                                                                                                      
  TaxLotID  varchar(50),                                                                                                                                                                              
  CreationDate datetime,                                                                                       
  OrderSideTagValue char(1),                                                                                      
  Symbol varchar(200),                                                                         
  OpenQuantity float, -- quantity is the net quantity of the position fetched i.e. the current quantity.       
      
       
  AveragePrice float,                                                                                  
  FundID int,                                                                                          
  AssetID int,                                                                                         
  UnderLyingID int,                                                                                                      
  ExchangeID int,                                                                                                        
  CurrencyID int,                                          
AUECID int,                                                                     
  OpenTotalCommissionandFees float,                                        
  Multiplier float,                                                                                                                                                           
  SettlementDate datetime,                                                                                                                                        
--  VsCurrencyID int,                                                                            
--  TradedCurrencyID  int,                                                                                                              
--  ExpirationDate datetime,                                                                                                
  Description varchar(max),                                                            
  Level2ID int,                                                                                            
  NotionalValue float,                                                                                
  BenchMarkRate float,                                                                                            
  Differential float,                                                                                            
  OrigCostBasis float,                                                           
  DC int,                                                                        
  SwapDescription varchar(max),                                                                                            
  FirstResetDate datetime,                                                                                   
  OrigTransDate datetime,                                                                                            
  IsSwapped bit,                                               
  AUECLocalDate DateTime,                                                                      
  GroupID Varchar(50),                                                                  
  PositionTag int,                                                                  
  FXRate float,                                                                  
  FXConversionMethodOperator varchar(5),                                                              
  FutureRootSymbol varchar(5)                                                            
 )                                                                
                                                                                    
 Insert Into #TEMPFundPositionsForDate_ValReport                                                              
  (                                                              
   TaxLotID,                                                                                                                    
   CreationDate,                                                                                       
   OrderSideTagValue,                                                    
   Symbol,                                                                                    
   OpenQuantity,                                                              
   AveragePrice,                                                                                              
   FundID,                                                                                           
   AssetID,                
   UnderLyingID,                                                                                                      
   ExchangeID,                                                                                                        
   CurrencyID,                                                                                                                                                                               
   AUECID,                           
   OpenTotalCommissionandFees,                                                                  
   Multiplier,                  
   SettlementDate,                                                                                        
--   VsCurrencyID,                                                                                     
--   TradedCurrencyID,                                                                                                                               
--   ExpirationDate,                                                                                          
   Description,                                                                                                  
   Level2ID,                         
   NotionalValue,                                                                                
   BenchMarkRate,                                                                                            
   Differential,                                                                                            
   OrigCostBasis,                                                                                                  
   DC,                                                                        
   SwapDescription,                                 
   FirstResetDate,                                                                                            
   OrigTransDate,                                                                                            
   IsSwapped,                                                                    
   AUECLocalDate,                                                                      
   GroupID,                                                                  
   PositionTag,                                                                  
   FXRate,                                                                  
   FXConversionMethodOperator                                                              
  )                                                   
 Select                                                                           
 PT.TaxLotID as TaxLotID,                                                                                              
 G.AUECLocalDate as TradeDate,                                                                      
 PT.OrderSideTagValue as SideID, -- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS                                                     
 PT.Symbol as Symbol ,                                                                        
 PT.TaxLotOpenQty as TaxLotOpenQty ,                                                                          
 PT.AvgPrice as AvgPrice ,                                                                                                                                                      
 PT.FundID as FundID,                                                                                                
 G.AssetID as AssetID,                                                                                              
G.UnderLyingID as UnderLyingID,                                                                                              
 G.ExchangeID as ExchangeID,                                                                                                
 G.CurrencyID as CurrencyID,                                                                                     
 G.AUECID as AUECID ,        
 PT.OpenTotalCommissionandFees,--this is open commission and closed commission sum is not necessarily equals to total commission                      
 AUEC.Multiplier as Multiplier,                                                                                                                                                                                                      
 G.SettlementDate as SettlementDate,                                                              
 --G.VsCurrencyID as VsCurrencyID ,                                                                                                                      
 --G.TradedCurrencyID as TradedCurrencyID,                                                
 --G.ExpirationDate as ExpirationDate,                                     
 G.Description as Description,                       
 PT.Level2ID as Level2ID,                                                                        
 isnull( (PT.TaxLotOpenQty * SW.NotionalValue / G.Quantity) ,0) as NotionalValue,                                                                                    
 isnull(SW.BenchMarkRate,0) as BenchMarkRate,                             
 isnull(SW.Differential,0) as Differential,                                                                                    
 isnull(SW.OrigCostBasis,0) as OrigCostBasis,                                                                                          
 isnull(SW.DayCount,0) as DayCount,                                                    
 isnull(SW.SwapDescription,'') as SwapDescription,                                                                                    
 SW.FirstResetDate as FirstResetDate,                                                                                    
 SW.OrigTransDate as OrigTransDate,                                                                
 G.IsSwapped as IsSwapped,                               
 G.AUECLocalDate as AUECLocalDate,                                                            
 G.GroupID,                                                          
 PT.PositionTag,                                                    
 G.FXRate,                                                    
 G.FXConversionMethodOperator                                                                  
                                                                                
from PM_Taxlots PT                                                                                         
Inner join  T_Group G on G.GroupID=PT.GroupID                                                                                    
Left outer  join  T_SwapParameters SW on G.GroupID=SW.GroupID                                                                                            
inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID                                                                                                                
and taxlot_PK in                                                                           
(                                                      
 select max(taxlot_PK) from PM_Taxlots                                                                           
 Inner join  T_Group G on G.GroupID=PM_Taxlots.GroupID                                                                           
 inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID                                                                                                                                  
 inner join @AUECDatesTable AUECDates on AUEC.AUECID = AUECDates.AUECID                                                                                       
 where datediff(d, PM_Taxlots.AUECModifiedDate,AUECDates.CurrentAUECDate) >= 0                          
 group by taxlotid                                         
)                                                                          
and TaxLotOpenQty<>0                                                              
                                                              
 --select * from #TEMPFundPositionsForDate_ValReport                               
                                                              
 Create Table #DayMarkPrices                                                              
 (                     
  Symbol varchar(200),                                                              
  YesterDayMarkPrice float,                                                              
  TodayMarkPrice float                                                            
 )                                                              
                                                            
                                                            
 Create Table #CompanyNamesAndUDAData                                                              
 (                                                              
  Symbol varchar(200),                                                              
  CompanyName varchar(500),                           
  UDAAssetName varchar(100),                                          
  UDASecurityTypeName varchar(100),                                                      
  UDASectorName varchar(100),                                                      
  UDASubSectorName varchar(100),                    
  UDACountryName varchar(100),                                                
  PutOrCall Varchar(10),                                      
  FutureMultiplier float,                                      
  ExpirationDate DateTime,                                  
  LeadCurrencyID int,                                  
  VsCurrencyID int,                  
  UnderlyingSymbol Varchar(100),            
  OSISymbol Varchar(100) ,    
 BloombergSymbol Varchar(100),                  
  SedolSymbol Varchar(100),                  
  ISINSymbol Varchar(100),                  
  CusipSymbol Varchar(100),                  
  IDCOSymbol Varchar(100)          
 )                                                              
                                                            
 INSERT INTO #CompanyNamesAndUDAData                                                              
Select                   
 TickerSymbol,                  
 CompanyName,                   
 AssetName,                   
 SecurityTypeName,                   
 SectorName,                   
 SubSectorName,                   
 CountryName,                   
 PutOrCall,                   
 Multiplier,                  
 ExpirationDate,                                  
 LeadCurrencyID,                  
 VsCurrencyID,                  
 UnderlyingSymbol,            
 OSISymbol,    
BloombergSymbol,                  
SedolSymbol,                  
ISINSymbol,                  
CusipSymbol,                  
IDCOSymbol                                  
     From V_SecMasterData                                  
                                                            
                                                            
                                                                            
-- IF(@reportMode = 0)       -- Cost basis mode                                                                                                   
-- Begin                                                                
--                                                              
  INSERT Into #DayMarkPrices                                                              
  Select                        
  DayMarkPrice.Symbol,                         
  0,    
  FinalMarkPrice                                                             
  From PM_DayMarkPrice DayMarkPrice                         
 Inner Join V_SymbolAUEC ON DayMarkPrice.Symbol = V_SymbolAUEC.Symbol                        
 Inner Join @AUECDatesTable AUECDatesTable ON AUECDatesTable.AUECID = V_SymbolAUEC.AUECID                        
 Where Datediff(d,DayMarkPrice.Date, AUECDatesTable.CurrentAUECDate) = 0                        
                       
--   Where Datediff(d,Date, @date) = 0                                                            
--                                                               
--  (                                                                  
  Select            
-- Case             
--  When AUEC.AssetID=2            
--  Then IsNull(SM.OSISymbol,'')            
--  Else Convert(varchar(200), TFPVR.Symbol)             
-- End as Symbol,                                                               
    Convert(varchar(200), TFPVR.Symbol) AS Symbol,                                                            
    OpenQuantity AS Quantity,                                                               
    AveragePrice AS CostPrice,         
    ISNULL(TFPVR.OpenTotalCommissionandFees, 0) AS TotalCommissionandFees,                                                              
    1 As TotalCostPerShare,                                                                                                                                                                      
    CASE AUEC.AssetID                                                                                                              
     When 5 THEN                                                                                                        
      0.0                                                  
     Else                                                                                                      
      0.0                                                  
    End                                                                       
    As TotalCost,                                                      
    0.0 AS MarketValue,                                                                           
    ((1 * OpenQuantity) - (OpenQuantity * AveragePrice)) AS Gain,                                                                                                               
    FundName,                                                                                                              
    CASE TFPVR.OrderSideTagValue                                                                                      
     WHEN '1' Then 'Long'                                                                                                                                
     WHEN '2' Then 'Short'                                               
     WHEN '5' Then 'Short'    --Sell Short                                                                                     
     WHEN 'A' Then 'Long'     --Buy To Open                                                                                                                                                                                  
     WHEN 'B' Then 'Long'    --Buy To Close                               
     WHEN 'C' Then 'Short'    --Sell To Open                                                                                                                  
     WHEN 'D' Then 'Short'   --Sell To Close                                                                                                                                                           
    END As [Position Type],                                                                                                                            
    C.CurrencySymbol,                                                                                            
    ' ' AS LanguageName,                         
    0.0 AS TotalCostInBaseCurrency,                                                                                                
    Comp.BaseCurrencyID AS BaseCurrencyID,                                                                                                             
    '' AS BaseCurrencyLanguageName,                                                                                  
    '' AS GroupParamaeter1,                                                                           '' AS GroupParamaeter2,                                                                                       
    '' AS GroupParamaeter3,                                                                                                                                                
    0.0 AS MarketValueInBaseCurrency,                                                                                                                                                
0.00 AS GainInBaseCurrency,                                                        
    1 AS AggregateOption, -- Selected as 1 by default so as not to use aggregate option for the first time load.                       
                              
    0.0 AS YesterdayMarketValue,                                                                                                            
   0.0 AS YesterdayMarketValueInBaseCurrency,                                                                                 
   CONVERT(VARCHAR(10), TFPVR.CreationDate, 101) AS TradeDate,                                                                                                      
    ISNULL(SM.FutureMultiplier, 0) As Multiplier,                                                                                       
    AUEC.AUECID AS AUECID,                                                                                                    
    AUEC.AssetID AS AssetID,                                                                                                    
    AUEC.BaseCurrencyID AS CurrencyID,                                                                                         
    ISNULL(SM.LeadCurrencyID, 0) AS TradedCurrencyID,                                                             
    ISNULL(SM.VsCurrencyID, 0) AS VsCurrencyID,                                   
    IsNull(CMF.MasterFundName,'Unassigned') as MasterFundName                                                                                                  
    , '' AS GroupParamaeter4                                                                                   
    ,isnull(BenchMarkRate + Differential,0) AS I1                                            
    ,DC                                                                                
    ,isnull(DayMark.TodayMarkPrice, 0.0) AS RRMarkPriceCost,                                  
    ISNULL(FXRRConvFactor.ConversionFactor, 0) AS FXRRMarkPriceCost,                           
                                           
 CASE Comp.BaseCurrencyID                                                                            
    WHEN TFPVR.CurrencyID                                                                               
THEN 1                                                                            
    ELSE                                
       CASE ISNULL(TFPVR.FXRate, 0)                                                              
        WHEN 0                                 
        THEN ISNULL(TDConvFactor.ConversionFactor, 0)                                                                            
        ELSE TFPVR.FXRate                                                               
       END                                                              
 END AS TDConversionFactorCost,                               
                                                        
 CASE ISNULL(TFPVR.FXRate, 0)                                
  WHEN 0                                                               
   THEN ISNULL(TDConvFactor.ConversionMethod, 0)                                                              
   ELSE                                                              
     CASE ISNULL(FXConversionMethodOperator, 'M')                                                     
       WHEN 'M'                                                               
       THEN 0                                                              
      ELSE 1                                                              
    END                                               
 END AS TDConversionMethodCost,                                
                                
CASE Comp.BaseCurrencyID                                                                           
  WHEN SM.VsCurrencyID                                      
  THEN 1                                                                            
  ELSE                                                                            
    CASE                                                               
     WHEN TFPVR.FXRate > 0 And (Comp.BaseCurrencyID = SM.LeadCurrencyID OR Comp.BaseCurrencyID = SM.VsCurrencyID)                                                               
     THEN TFPVR.FXRate                                                                           
     ELSE ISNULL(FXTDConvFactor.ConversionFactor, 0)                                                               
    END                                                                
END AS FXTDConversionFactorCost,                               
                                                         
CASE ISNULL(TFPVR.FXRate, 0)                                                              
   WHEN 0                                 
   THEN ISNULL(FXTDConvFactor.ConversionMethod, 0)                                                              
   ELSE                                                              
    CASE                                    
   WHEN TFPVR.FXConversionMethodOperator ='M' And (Comp.BaseCurrencyID = SM.LeadCurrencyID OR Comp.BaseCurrencyID = SM.VsCurrencyID)                                  
   THEN  0                            
   WHEN TFPVR.FXConversionMethodOperator ='D' And (Comp.BaseCurrencyID = SM.LeadCurrencyID OR Comp.BaseCurrencyID = SM.VsCurrencyID)                                  
   THEN  1                                                          
   ELSE ISNULL(FXTDConvFactor.ConversionMethod, 0)                                                              
  END                                                          
END AS FXTDConversionMethodCost,                                                              
                              
CASE Comp.BaseCurrencyID                                                  
      WHEN TFPVR.CurrencyID                                                                                 
      THEN 1                                                                            
      ELSE ISNULL(RRConvFactor.ConversionFactor, 0)                                                     
END AS RRConversionFactorCost,                                                                               
ISNULL(RRConvFactor.ConversionMethod, 0) AS RRConversionMethodCost,                                                              
 CASE Comp.BaseCurrencyID                                                                           
    WHEN SM.VsCurrencyID                                                                             
    THEN 1                                                                            
    ELSE ISNULL(FXRRConvFactorVsToBase.ConversionFactor, 0)                                                                           
 END AS FXRRConversionFactorCost,                                                                              
 ISNULL(FXRRConvFactorVsToBase.ConversionMethod, 0) AS FXRRConversionMethodCost,                                                           
                                                                                     
 dbo.GetSideMultiplier(TFPVR.OrderSideTagValue) AS SideMultiplier,                                                              
 isnull(NotionalValue, 0) AS NotionalValue,           
Case             
 When TFPVR.AssetID=2            
 Then IsNull(SM.CompanyName,'') + ', ' + CONVERT(VARCHAR(10),SM.ExpirationDate,101)           
 Else IsNull(SM.CompanyName,'')             
End as SymbolCompanyName,                                                                              
-- SM.CompanyName AS SymbolCompanyName,                                                                         
 TFPVR.TaxlotID AS TaxlotID,                            
 '01-01-1900' AS YesterdayBusinessDate,                             
 '01-01-1900' AS FXYesterdayBusinessDate,                                                                         
 CF.CompanyFundID As CompanyFundID,                                                              
 A.AssetName AS AssetName,                                                        
 TFPVR.Level2ID AS StrategyID,                                                        
 ISNULL(CS.StrategyName, 'Strategy Unallocated') AS StrategyName,                                                                            
 ISNULL(SM.Symbol, 'Undefined') AS UDATickerSymbol,                                
 ISNULL(SM.UDAAssetName, 'Undefined') AS UDAAssetName,                                                                     
 ISNULL(SM.UDASecurityTypeName, 'Undefined') AS UDASecurityTypeName,                                                                            
 ISNULL(SM.UDASectorName, 'Undefined') AS UDASectorName,                                                                       ISNULL(SM.UDASubSectorName, 'Undefined') AS UDASubSectorName,                                                                  
  
    
      
          
 ISNULL(SM.UDACountryName, 'Undefined') AS UDACountryName,                                                
 ISNULL(SM.PutOrCall, '-1') AS PutOrCall,                      
 IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                  
 IsNull(SM.UnderlyingSymbol,'') AS UnderlyingSymbol,        
TFPVR.Description,    
IsNull(SM.BloombergSymbol,'') as BloombergSymbol,                  
 IsNull(SM.SedolSymbol,'') as SedolSymbol,                  
 IsNull(SM.ISINSymbol,'') as ISINSymbol,                  
 IsNull(SM.CusipSymbol,'') as CusipSymbol,                  
 IsNull(SM.OSISymbol,'') as OSISymbol,                  
 IsNull(SM.IDCOSymbol,'') as IDCOSymbol                                                            
                                                                                
   FROM  #TEMPFundPositionsForDate_ValReport TFPVR                                       
   LEFT OUTER JOIN #CompanyNamesAndUDAData SM ON SM.Symbol = TFPVR.Symbol                                                                                                                 
   INNER JOIN T_Company Comp ON Comp.CompanyID = @companyID                                                                                                          
   INNER JOIN T_AUEC AUEC ON TFPVR.AUECID = AUEC.AUECID                                                                                                                                       
   INNER JOIN T_Asset A ON AUEC.AssetID = A.AssetID                                                                                                              
   INNER JOIN T_Currency C ON TFPVR.CurrencyID = C.CurrencyID                                                           
   INNER JOIN T_CompanyFunds CF ON TFPVR.FundID = CF.CompanyFundID                                                        
   LEFT JOIN T_CompanyStrategy CS ON TFPVR.Level2ID = CS.CompanyStrategyID                          
   Left Outer Join @AUECDatesTable AUECDates On AUECDates.AUECID = TFPVR.AUECID                                                                                     
   LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA                                                               
    ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                                        
   LEFT OUTER JOIN T_CompanyMasterFunds CMF                                                               
    ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                   
   LEFT OUTER JOIN #DayMarkPrices DayMark                                                               
    ON DayMark.Symbol = TFPVR.Symbol                                        
   LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange TDConvFactor                                                               
    ON TFPVR.CurrencyID = TDConvFactor.FCID                               
     AND Comp.BaseCurrencyID = TDConvFactor.TCID                                                               
     AND DATEDIFF(d,TDConvFactor.DateCC,TFPVR.CreationDate) = 0                                                              
   LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange RRConvFactor                                                               
    ON TFPVR.CurrencyID = RRConvFactor.FCID                                                               
     AND Comp.BaseCurrencyID = RRConvFactor.TCID                                                               
     AND DATEDIFF(d,RRConvFactor.DateCC,AUECDates.CurrentAUECDate) = 0                          
                                                 
 LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXTDConvFactor                                                               
    ON (FXTDConvFactor.FCID = SM.VsCurrencyID            
     And FXTDConvFactor.TCID = Comp.BaseCurrencyID)                                       
     AND DATEDIFF(d,FXTDConvFactor.DateCC,TFPVR.CreationDate) = 0                                 
                                                              
    LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXRRConvFactorVsToBase                                                               
    ON (FXRRConvFactorVsToBase.FCID = SM.VsCurrencyID                  
     And FXRRConvFactorVsToBase.TCID = Comp.BaseCurrencyID)                                              
     AND DATEDIFF(d,FXRRConvFactorVsToBase.DateCC,AUECDates.CurrentAUECDate) = 0                                  
                                                           
 LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXRRConvFactor                                                              
    ON (FXRRConvFactor.FCID = SM.LeadCurrencyID                                                               
     And FXRRConvFactor.TCID = SM.VsCurrencyID)                                                               
     AND DATEDIFF(d,FXRRConvFactor.DateCC,AUECDates.CurrentAUECDate) = 0                                  
                             
 LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA                       
 ON CS.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                  
 LEFT OUTER JOIN T_CompanyMasterStrategy CMS                       
 ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                      
                             
  WHERE TFPVR.OpenQuantity > 0                                                                              
--  )                                                              
                                                              
  UNION ALL                                                              
  (                                                              
   Select                                                              
   MIN(C.CurrencySymbol) AS Symbol,                                                                    
   SUM(CashValueLocal) AS Quantity,                                  
   0.0 AS CostPrice,                                                              
   0.0 AS TotalCommissionandFees,                                                              
   1 AS TotalCostPerShare,                                                              
   0.0 AS TotalCost,                                                              
   SUM(CashValueLocal) AS MarketValue,                                                                 
   0.0 AS Gain,                                                              
   MIN(CF.FundName) AS FundName,                                                              
   '' AS [Position Type],                                       
   MIN(C.CurrencySymbol) AS CurrencySymbol,                                                               
   ' ' AS LanguageName,                                                              
  0.0 AS TotalCostInBaseCurrency,                                                              
   MIN(BaseCurrencyID) AS BaseCurrencyID,                                                              
   '' AS BaseCurrencyLanguageName,                                                                                  
   '' AS GroupParamaeter1,                                                                                            
   '' AS GroupParamaeter2,                                                                          
   '' AS GroupParamaeter3,                                                              
                                                  
   SUM(CashValueBase) AS MarketValueInBaseCurrency,     --As the base value is directly picked from the DB so no need to convert into base market value.                                                                                                       
 
    
      
        
   0.00 AS GainInBaseCurrency,              
   1 AS AggregateOption, -- Selected as 1 by default so as not to use aggregate option for the first time load.                                                                  
   0.0 AS YesterdayMarketValue,                                                                                                            
   0.0 AS YesterdayMarketValueInBaseCurrency,                                                              
   MIN(CFCC.Date) AS TradeDate,                                                              
   1 AS Multiplier,                           
   0 AS AUECID,                                                                                                    
   6 AS AssetID,                                                                                     
   MIN(LocalCurrencyID) AS CurrencyID,                                                              
   0 AS TradedCurrencyID,                                                                                                    
   0 AS VsCurrencyID,                                                              
   MIN(IsNull(CMF.MasterFundName,'Unassigned')) AS MasterFundName,                                                              
   '' AS GroupParamaeter4,                                                              
   0.0 AS I1,                                                                 
   0 AS DC,                                                                         
   0.0 AS RRMarkPriceCost,                                    
   0.0 AS FXRRMarkPriceCost,                                                              
   0.0 AS TDConversionFactorCost,                                                              
   0 AS TDConversionMethodCost,                                                            
   0.0 AS FXTDConversionFactorCost,                                                              
   0 AS FXTDConversionMethodCost,                      
                                                        
   0 AS RRConversionFactorCost,      --As the base value is directly picked from the DB so no need to get coversion rate.                                                      
   MIN(RRConvFactor.ConversionMethod) AS RRConversionMethodCost,                                                              
   0.0 AS FXRRConversionFactorCost,                            
   0 AS FXRRConversionMethodCost,                                                              
                                                                                   
   0 AS SideMultiplier,                                                                                
   0.0 AS NotionalValue,                                                                             
   '' AS SymbolCompanyName,                               
   '' AS TaxlotID,                                                                                
   '01-01-1900' AS YesterdayBusinessDate,                                                                          
   '01-01-1900' AS FXYesterdayBusinessDate,                                                        
   MIN(CF.CompanyFundID) As CompanyFundID,                                                      
   'Cash' AS AssetName,                                                        
   0 AS StrategyID,                                        
   'Strategy Unallocated' AS StrategyName,                                                                     
 'Undefined' AS UDATickerSymbol,                                                  
 'Undefined' AS UDAAssetName,                                                                     
 'Undefined' AS UDASecurityTypeName,                                                                            
 'Undefined' AS UDASectorName,                                                                            
 'Undefined' AS UDASubSectorName,                                                                            
 'Undefined' AS UDACountryName,                                                
 '-1' AS PutOrCall,                      
 'Unassigned' as MasterStrategyName,                  
'' as UnderlyingSymbol,        
'Cash' As Description,    
'' as BloombergSymbol,                  
'' as SedolSymbol,                  
'' as ISINSymbol,                  
'' as CusipSymbol,                  
'' as OSISymbol,                  
'' as IDCOSymbol                  
                                                                
   From PM_CompanyFundCashCurrencyValue CFCC                                                               
   INNER JOIN T_Currency C                                                                  
    ON CFCC.LocalCurrencyID = C.CurrencyID                                                    
   INNER JOIN T_Currency CBase                                                               
    ON BaseCurrencyID = CBase.CurrencyID                                           
   INNER JOIN T_CompanyFunds CF                                                               
    ON CFCC.FundID = CF.CompanyFundID                                                              
   LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA                                                               
    ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                        
   LEFT OUTER JOIN T_CompanyMasterFunds CMF                                                               
    ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                              
   LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange RRConvFactor                                                      
    ON CFCC.LocalCurrencyID = RRConvFactor.FCID                                                               
     AND CFCC.BaseCurrencyID = RRConvFactor.TCID                                           
     And DATEDIFF(d,RRConvFactor.DateCC,CFCC.Date) = 0                                                      
   WHERE dbo.GetFormattedDatePart(Date) = dbo.GetFormattedDatePart(@date)                                                                  
    Group By CFCC.FundID, LocalCurrencyID                                             
  )                                                              
                                                                                                        
                                                                                                                  
 DROP TABLE #TEMPGetConversionFactorForGivenDateRange,#TEMPFundPositionsForDate_ValReport,#CompanyNamesAndUDAData,#DayMarkPrices                                                                               
--DROP TABLE #TEMPFundPositionsForDate_ValReport                     
END 