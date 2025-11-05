/****************************************************************************                                                                                                                                                                                  
  
    
      
Name :   PMGetUnrealizedPNLReport                                                                                                      
Date Created: 20-Sep-2007                                                                                                                                                                                                                                      
  
   
Purpose:  Get the unrealized pnl report i.e. open position's PNL till the date passed.                                                                                                                                                                         
  
    
      
Module Name: PortfolioReports/Unrealized PNL Report                                                                                                                                                                                                
Author: Bhupesh Bareja                                                                                                                                                                                                                                        
Parameters:                                                                                                                                                                                                                                         
 @companyID int,                                                                                                                                                                                                                                        
 @date datetime,                                                                                                                                                                                                                                        
 @reportMode int                                                                                                                                                              
 Date Modified: 13-05-2008                                                                                                                                                                                                                                     
  
   
 Description:                                                                                                                                                                                                                                           
 Modified By:  Sandeep SIngh                                                                                                                    
                                                                                                                    
Date Modified: 24-10-2008                                                                                                                                                                                                                                      
  
Description: Execution Optimizations & Formatting                                                                                                                  
Modified By:  Sumit Kakra                                                                                                                  
                                                                                                                                                                                                                                     
Execution StateMent:                                  
   EXEC [PMGetUnrealizedPNLReport] 2,'10-12-2012',0,'1182,1183,1184','1,2,3,4,5'                  
select * from T_Group                      
                          
Modified Date: 24-April-2012                                                                
Modified By : Sandeep Singh                                                                                           
<Description: FX Spot, FX Forward and Fixed Income related changes>                   
                  
Modified Date: 27-Sep-2012                  
Modified By: Rahul Gupta                  
Description: FX level implementation at taxlot level            
      
Modified Date: 12-DEc-2012                  
Modified By: om shiv                  
Description: Added UDA columns        
      
Modified Date: 30-OCT-2014                  
Modified By: Sandeep Singh                  
Description: Transaction Type column added       
                                                                                                                     
           
Modified By : Ankit Gupta on 30 Oct, 2014    
Description : Fetch allocated quantity per taxlot in P_GetPositions.    
JIRA  : http://jira.nirvanasolutions.com:8080/browse/CHMW-1670    
*/        
                                       
Alter PROCEDURE [dbo].[PMGetUnrealizedPNLReport]                                                                                  
(                                                                                    
 @companyID int,                                                               
 @date datetime,                                                           
 @reportMode int,                  
 @Fund varchar(max),                              
 @Asset varchar(max)                                                                                      
)                                                                                                    
AS                                                                                  
BEGIN                       
                    
Select                     
PM_Taxlots.*                     
into #PM_Taxlots                    
from PM_Taxlots                     
Where taxlot_PK in                                                                                                 
(                                                                                                                                                
  Select max(taxlot_PK) from PM_Taxlots                                                    
  where Datediff(d,PM_Taxlots.AUECModifiedDate,@date) >= 0                                                                                          
  group by taxlotid                                                                                                                         
 )                                                                                                                                                 
and TaxLotOpenQty<>0                     
                                                                                     
 DECLARE @startingDate DateTime                                                                                             
 Set @startingDate =           
(          
select min(AuecLocalDate) from PM_Taxlots T          
inner join T_group G on G.groupID =  T.GroupID            
where           
T.taxlotopenqty <> 0           
and           
T.Taxlot_pk in (select max (Taxlot_pk) from PM_taxlots where datediff (d,Auecmodifieddate,@date)>=0 group by Taxlotid)          
and           
DateDiff(D,AuecLocalDate,'1-1-1800') <> 0          
--Select min(AUECModifiedDate) as TradeDate from #PM_Taxlots          
)               
          
--Select @startingDate          
              
If @startingDate is null              
Set @startingDate = @date                         
                    
                                                  
 Create Table #TEMPGetConversionFactorForGivenDateRange               
 (                                                                                                                                                   
  FCID int,                                                       
  TCID int,                                                                                                             
  ConversionFactor float,                    
  ConversionMethod int,                                                  
  DateCC DateTime,
  eSignalSymbol VARCHAR(max),
  fundID INT                                                                                                                                                                           
 )                                                                                                                     
 INSERT INTO #TEMPGetConversionFactorForGivenDateRange                                                                              
 SELECT                                            
  FromCurrencyID,                                                                                                                                         
  ToCurrencyID,                                                                                                                                                                                        
  RateValue,                                                                                                                                                                                        
  ConversionMethod,                                                                                                                                                                   
  Date,
  eSignalSymbol,
  fundID                                                                                                                                     
  FROM dbo.GetAllFXConversionRatesForGivenDateRange(@startingDate, @date)                                                
                                                                                                                     
 Declare @AllAUECDatesString VARCHAR(MAX)                                                                                                                                  
 Set @AllAUECDatesString = dbo.GetAUECDateString(@date)                   
                  
Declare @T_FundIDs Table                                        
(                                        
 FundId int                                        
)                                        
Insert Into @T_FundIDs Select * From dbo.Split(@Fund, ',')                               
                              
Declare @T_AssetIDs Table                                            
(                                            
 AssetId int                                            
)                                            
Insert Into @T_AssetIDs Select * From dbo.Split(@Asset, ',')                                  
                                           
 Select                                       
 T_Asset.*                                   
InTo #T_Asset                                         
  From T_Asset INNER JOIN @T_AssetIDs AssetIDs ON T_Asset.AssetID = AssetIDs.AssetId                                
                              
CREATE TABLE #T_CompanyFunds                                        
(                                        
 CompanyFundID int,                                        
 FundName varchar(50),                                        
 FundShortName varchar(50),                                        
 CompanyID int,                                        
 FundTypeID int,                                
 UIOrder int NULL                                       
)                               
                                     
Insert Into #T_CompanyFunds                                        
Select          
 CompanyFundID,                                        
 FundName,                                        
 FundShortName,                                        
 CompanyID,           
 FundTypeID,                                
 UIOrder                                          
 From T_CompanyFunds INNER JOIN @T_FundIDs FundIDs ON T_CompanyFunds.CompanyFundID = FundIDs.FundID                                                
                                                              
                                                                                                         
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
  VsCurrencyID int,                                                                                                                                                              
  TradedCurrencyID  int,                                                                                                     
  ExpirationDate datetime,                                                                                                                                                        
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
 CompanyName varchar(500),                                                                  
 UnderlyingSymbol varchar(50)                                                                                                            
 )                                                                                                                               
                                                            
Create Table #PositionTable                                                    
 (                                                 
 TaxLotID  varchar(50),                                                                                                                  
 AUECLocalDate datetime,                                                                              
 SideID char(1)  ,                                                                          
 Symbol varchar(200),                                                                                                                                                                                                                    
 OpenQuantity float,                                                                                                                                                                                                                     
 AveragePrice float,                                                                                                                                                                                                                                          
  
    
 FundID int,                                                                                                                                                                  
 AssetID int,                                                                                                                                                   
 UnderLyingID int,                                                                                                                                                                
 ExchangeID int,                                                                                                                                                                  
 CurrencyID int,             
 AUECID int,           
 TotalCommissionandFees float,                                                                                                                            
 Multiplier float,                             
 SettlementDate datetime,                                                                    
 LeadCurrencyID int,                                                        
 VsCurrencyID int,                                                            
 ExpirationDate datetime,                                                                                                                       
 Description varchar(max),                                    
 Level2ID int,                                                                                                            
 NotionalValue float,                                           
 BenchMarkRate float,                                                             
 Differential float,                                                                                                              
 OrigCostBasis float,                                                                                                                                                            
 DayCount int,                                                            
 SwapDescription varchar(max),                                                                     
 FirstResetDate datetime,                                         
 OrigTransDate datetime,                                                                                                                       
 IsSwapped bit,                                                               
 AllocationDate DateTime,                                                                                                                                
 GroupID Varchar(50),                                                          
 PositionTag int,                                                            
 FXRate float,                                                                                      
 FXConversionMethodOperator varchar(5),                                                                                                       
 CompanyName varchar(500),                                                                                         
 UnderlyingSymbol varchar(50),                                                                                      
 Delta float,                                                                                    
 PutOrCall Varchar(5),                                                                                  
 IsGrPreAllocated bit,                                                                                  
 GrCumQty float,                                                                                  
 GrAllocatedQty float,                                                                                  
 GrQuantity float ,                                                                        
 Taxlot_Pk  bigint,                                                                                 
 ParentRow_Pk  bigint ,                                                          
 StrikePrice float,                                                        
 UserID int,                                                        
 CounterPartyID int,                                                    
 CorpActionID uniqueidentifier,                                    
 Coupon float,                                    
 IssueDate Datetime,                                    
 MaturityDate DateTime,                                    
 FirstCouponDate DateTime,                                    
 CouponFrequencyID int,                                    
 AccrualBasisID int,                                    
 BondTypeID int,                                    
 IsZero bit,        
 ProcessDate DateTime,                                  
 OriginalPurchaseDate DateTime,        
 IsNDF bit,                              
 ReRateDate DateTime,              
 IDCOSymbol varchar(50),                                    
 OSISymbol varchar(50),                                     
 SEDOLSymbol varchar(50),                                     
 CUSIPSymbol varchar(50),                                      
 BloombergSymbol varchar(50),                    
 MasterFund varchar(50),            
 UnderlyingDelta float,          
 ISINSymbol varchar(50),          
 LotId varchar(200),          
 ExternalTransId varchar(100),           
 TradeAttribute1  Varchar(200),              
 TradeAttribute2  Varchar(200),            
 TradeAttribute3  Varchar(200),            
 TradeAttribute4  Varchar(200),            
 TradeAttribute5  Varchar(200),            
 TradeAttribute6  Varchar(200),          
 ProxySymbol varchar(100),      
      
 --Added UDA columns ,by Omshiv, Nov, 2013        
 AssetName Varchar(100),                                                                                                                                                                                                                                      
  
    
 SecurityTypeName Varchar(200),                                                                             
 SectorName Varchar(100),                                                                                                                                                                                                                               
 SubSectorName Varchar(100),                                                                                                                                                  
 CountryName  Varchar(100) ,        
 BBGID varchar(20),      
 TransactionType Varchar(200),    
    ExecutedQty float,    
 ClosingTaxlotId varchar(50),    
 ReutersSymbol varchar(50),
 InternalComments varchar(100),
 SettlCurrency  varchar(50),
 SettlCurrFxRate FLOAT, 
 SettlCurrAmt FLOAT,
 SettlCurrFxRateCalc Varchar(100),
 IsCurrencyFuture BIT,
 Symbol_PK BIGINT   
 )                                                     
Insert Into #PositionTable EXEC P_GetPositions @AllAUECDatesString,null,null,null                                                    
                                                    
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
   VsCurrencyID,                                                                                                                                                                    
   TradedCurrencyID,                                                                                                                                                                                   
    ExpirationDate,                                                                                                                                                        
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
   FXConversionMethodOperator,                                                                                                  
   CompanyName,                                                                                                   
   UnderlyingSymbol                                                                                                                                                                        
  )                                                                                              
 Select                                                                                   
TaxLotID,                                                                                                                            
 ProcessDate,--AUECLocalDate,                                                                                                                
 SideID,                                                                                   
 Symbol ,                                                                          
 OpenQuantity ,                                                                             
 AveragePrice ,                                                                           
 FundID,                                      
 AssetID,                                                                                                                                                        
 UnderLyingID,                                                                             
 ExchangeID,                                                                                                                                                          
 CurrencyID,                                                                                                          
 AUECID ,                                                                                                                                
 TotalCommissionandFees,--this is open commission and closed commission sum is not necessarily equals to total commission                                                  
 Multiplier,                                                                                                                                                                                                                     
 SettlementDate,                                                                                                    
 VsCurrencyID ,                                                                                                                                                                                
 LeadCurrencyID,                                                                                                                                                    
 ExpirationDate,                                                                                             
 Description,                                                                                                                                               
 Level2ID,                                                                                                       
 NotionalValue,                                                                                        
 BenchMarkRate,                                                                                             
 Differential,                                                                                                
 OrigCostBasis,                      
 DayCount,                                                                                                                        
 SwapDescription,                                                                                                            
 FirstResetDate,                                                                         
 OrigTransDate,                                                                                                                          
 IsSwapped,                                                                                                                       
 AllocationDate,                                               
 GroupID,                                                                                               
 PositionTag,                                                                                                 
 FXRate,                                                                                     
 FXConversionMethodOperator,                                                        
 CompanyName,                                                                                                                        
 UnderlyingSymbol                                                                  
 from  #PositionTable                                                                                       
                                               
                                                                                                                                                                                            
                               
 Create Table #DayMarkPrices                                     
 (                                                                                                                  
  Symbol varchar(200),                                 
  YesterDayMarkPrice float,                                                                                                                  
  TodayMarkPrice float                                                                                                                
 )                                                              
                                                            
Create Table #AUECYesterDates                                                    
(                                               
  AUECID INT,                                                                                                                                
YESTERDAYBIZDATE DATETIME                                                                                                                                
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
  BloombergSymbol Varchar(100),                                                                                       
  SedolSymbol Varchar(100),                       
  ISINSymbol Varchar(100),                              
  CusipSymbol Varchar(100),                              
  OSISymbol Varchar(100),                              
  IDCOSymbol Varchar(100),    
  ReutersSymbol varchar(100)                                                                                                          
 )                                                                       
                                                                                                                
 INSERT INTO #CompanyNamesAndUDAData                                                                             
  Select TickerSymbol,                       
  CompanyName,                       
  AssetName,                       
  SecurityTypeName,                       
  SectorName,                       
  SubSectorName,                       
  CountryName,                       
  PutOrCall,                       
  Multiplier,                      
  BloombergSymbol,                      
  SedolSymbol,                      
  ISINSymbol,                      
  CusipSymbol,                      
  OSISymbol,                      
  IDCOSymbol,    
  ReutersSymbol                                                                                
  From V_SecMasterData           
                               
Create Table #TempSplitFactorForOpen                                              
(                                              
TaxlotID varchar(50),                                              
Symbol varchar(100),                                              
SplitFactor float                      
)                                              
                                              
Insert InTo #TempSplitFactorForOpen                                              
Select                                             
PT.TaxlotID,                                            
PT.Symbol,                                            
SplitFactor                                        
from #PM_Taxlots PT                                                     
Inner Join V_CorpActionData VCA on PT.Symbol = VCA.Symbol                     
and DateDiff(day,VCA.EffectiveDate,@Date)=0 and VCA.IsApplied = 1                        
                                                                                                                
DECLARE @RecentDateForNonZeroCash datetime                    
SET @RecentDateForNonZeroCash = dbo.[GetRecentDateForNonZeroCash](@date)                                                                      
                           
-----------------------------------------------------------------------------------------------------                                                                                                                                                    
 IF(@reportMode = 0)   -- Cost basis mode                                                                                                                                                       
 Begin                                                                  
                                                            
 INSERT INTO #AUECYesterDates                                                                                                                              
 Select Distinct V_SymbolAUEC.AUECID, dbo.AdjustBusinessDays(DateAdd(d,1,@date),-1, V_SymbolAUEC.AUECID)                                                                                                                   
    from V_SymbolAUEC                                                                                                               
                      
INSERT Into #DayMarkPrices                                                                                        
 Select                                                             
 DayMarkPrice.Symbol,                                                            
 0,                                                            
 DayMarkPrice.FinalMarkPrice                                                                                                                 
  From PM_DayMarkPrice DayMarkPrice                                                            
  Inner Join V_SymbolAUEC ON DayMarkPrice.Symbol = V_SymbolAUEC.Symbol                                                            
  Inner Join #AUECYesterDates AUECDates ON AUECDates.AUECID = V_SymbolAUEC.AUECID                                                          
  Where Datediff(d,DayMarkPrice.Date, AUECDates.YESTERDAYBIZDATE) = 0                                                                                              
                  
------------------------------------------------------------------------------------------------                                                                                                               
   Select                                                                                                                   
    Convert(varchar(200), TFPVR.Symbol) AS Symbol,                                                                                                                                     
    OpenQuantity AS Quantity,                                                                                        
    AveragePrice AS CostPrice,                                                                                            
    ISNULL(TFPVR.OpenTotalCommissionandFees, 0) AS TotalCommissionandFees,                                                                                      
    1 As TotalCostPerShare,                                                                                                     
    CASE AUEC.AssetID                                                                                                                                                          
     When 5                             
  THEN 0.0                                               
     Else 0.0                            
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
    '' AS GroupParamaeter1,                                           
    '' AS GroupParamaeter2,                                                                   
    '' AS GroupParamaeter3,                                                                                                                              
    0.0 AS MarketValueInBaseCurrency,                                                                                                                                                                              
    0.00 AS GainInBaseCurrency,                                                                                
    1 AS AggregateOption, -- Selected as 1 by default so as not to use aggregate option for the first time load.                                                                             
   0.0 AS YesterdayMarketValue,                                                                                                                                                                
   0.0 AS YesterdayMarketValueInBaseCurrency,                                                                                                                                              
   CONVERT(VARCHAR(10), TFPVR.CreationDate, 101) AS TradeDate,                                                                             
    ISNULL(CompanyNames.FutureMultiplier, 0) As Multiplier,                                                                                                                                           
    AUEC.AUECID AS AUECID,                                                                                                                                                        
    AUEC.AssetID AS AssetID,                                                                                                         
    AUEC.BaseCurrencyID AS CurrencyID,                                                  
    ISNULL(TFPVR.TradedCurrencyID, 0) AS TradedCurrencyID,                                                                                                                            
    ISNULL(TFPVR.VsCurrencyID, 0) AS VsCurrencyID                                                                                                               
    ,IsNull(CMF.MasterFundName,'Unassigned') as MasterFundName                                                                                                                                                        
    , '' AS GroupParamaeter4                                                                                                                                       
    ,isnull(BenchMarkRate + Differential,0) AS I1                                                                                                               
    ,DC                                                                                                
    ,isnull(DayMark.TodayMarkPrice, 0.0) AS RRMarkPriceCost                                                                                                                                    
                                                                            
   --ISNULL(FXRRConvFactor.ConversionFactor, 0)AS FXRRMarkPriceCost  --this is mark price for forex                             
-- Previously  FX Spot and Forward Mark Price was picked from FX Rate Table, now it has been updated and Mark Price is picked from the PM_DayMarkPrice table                            
    ,isnull(DayMark.TodayMarkPrice, 0.0) AS FXRRMarkPriceCost                             
    ,0.0 AS RRYMarkPriceCost --Not required for cost.                                                  
    ,0.0 AS FXRRYMarkPriceCost, --Not required for cost.                                                                                    
                                                                                      
CASE Comp.BaseCurrencyID                                                                                                          
 WHEN TFPVR.CurrencyID                                                                                                                        
 THEN 1                                                                                                                                
 ELSE                                                                                                                                
  CASE ISNULL(TFPVR.FXRate, 0)                               
   WHEN 0                                                                           
   THEN IsNull(TDConvFactor.ConversionFactor, 0)                                                                                                                                
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
 WHEN TFPVR.VsCurrencyID                                                      
 THEN 1                                                                                                                                
 ELSE                                                               
  CASE                                                                                            
   WHEN TFPVR.FXRate > 0                                                                                                                  
   THEN TFPVR.FXRate                                                    
   ELSE ISNULL(TDConvFactor.ConversionFactor, 0)                                                                                                                           
  END                                          
END AS FXTDConversionFactorCost,                                                                                                                                            
                                                                                                                                          
CASE ISNULL(TFPVR.FXRate, 0)                                                                                                                  
 WHEN 0                                         
 THEN ISNULL(TDConvFactor.ConversionMethod, 0)                                                                                                                          
 ELSE                                                                                                                  
  CASE                                                                                                                   
   WHEN ISNULL(FXConversionMethodOperator, 'M')='M'                                                                        
   THEN 0                                                              
   WHEN ISNULL(FXConversionMethodOperator, 'D')='D'                                                                          
   THEN 1                                                      
  ELSE ISNULL(TDConvFactor.ConversionMethod, 0)                                                                                
  END                                                                                                      
END AS FXTDConversionMethodCost,                               
                                                                                                               
CASE Comp.BaseCurrencyID                                                                                                                               
 WHEN TFPVR.CurrencyID                                                           
 THEN 1                        
ELSE ISNULL(RRConvFactor.ConversionFactor, 0)                                                                                                                                
END AS RRConversionFactorCost,                                                                                                                                   
                                                                          
ISNULL(RRConvFactor.ConversionMethod, 0) AS RRConversionMethodCost,                                                                           
                                                                          
CASE Comp.BaseCurrencyID              
   WHEN TFPVR.VsCurrencyID                                                
   THEN 1                                                                                                                       
   ELSE ISNULL(RRConvFactor.ConversionFactor, 0)                                                                    
END AS FXRRConversionFactorCost,                                                                      
                                                                          
ISNULL(RRConvFactor.ConversionMethod, 0) AS FXRRConversionMethodCost,                                                                                                                      
     0.0 AS RRMarkPriceDay,                                                                                                                                    
     0.0 AS FXRRMarkPriceDay,                                                                                                                                    
     0.0 AS RRYMarkPriceDay,                                                             
     0.0 AS FXRRYMarkPriceDay,                                                                        
     0.0 AS RRConversionFactorDay,                                                                                                 
     0 AS RRConversionMethodDay,             
     0.0 AS FXRRConversionFactorDay,                                                                                                                                 
     0 AS FXRRConversionMethodDay,                                                                                                      
     0.0 AS RRYConversionFactorDay,                                                                        
     0 AS RRYConversionMethodDay,             
     0.0 AS FXRRYConversionFactorDay,                                                                                                                                 
     0 AS FXRRYConversionMethodDay,                                                                                                                             
     dbo.GetSideMultiplier(TFPVR.OrderSideTagValue) AS SideMultiplier,                                                                                           
     isnull(NotionalValue, 0) AS NotionalValue,                                                                                    
  CompanyNames.CompanyName AS SymbolCompanyName,                                 
     TFPVR.TaxlotID AS TaxlotID,                                                                                                                                    
     '01-01-1900' AS YesterdayBusinessDate,                                                                                                                              
     '01-01-1900' AS FXYesterdayBusinessDate,                                                                                                                              
     CF.CompanyFundID As CompanyFundID,                            
     A.AssetName AS AssetName,                                                            
 TFPVR.Level2ID AS StrategyID,                                                                                        
 ISNULL(CS.StrategyName, 'Strategy Unallocated') AS StrategyName,                                     
 ISNULL(CompanyNames.Symbol, 'Undefined') AS UDATickerSymbol,                                                                                                                                
 ISNULL(CompanyNames.UDAAssetName, 'Undefined') AS UDAAssetName,                                                       
 ISNULL(CompanyNames.UDASecurityTypeName, 'Undefined') AS UDASecurityTypeName,                                                                                 
 ISNULL(CompanyNames.UDASectorName, 'Undefined') AS UDASectorName,                                                                       
 ISNULL(CompanyNames.UDASubSectorName, 'Undefined') AS UDASubSectorName,                                              
 ISNULL(CompanyNames.UDACountryName, 'Undefined') AS UDACountryName,                                                                                                    
 ISNULL(CompanyNames.PutOrCall, '-1') AS PutOrCall,                                                                    
 0 FXRRConversionFactorDay2,                                                                    
 0 FXRRConversionMethodDay2,                                                        
 IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                                              
ISNull(TFPVR.UnderlyingSymbol,'') as UnderlyingSymbol,                        
TFPVR.IsSwapped,                      
Isnull(CompanyNames.BloombergSymbol,'') as BloombergSymbol,                                                                       
Isnull(CompanyNames.SedolSymbol,'') as SedolSymbol,                              
Isnull(CompanyNames.ISINSymbol,'') as ISINSymbol,                              
Isnull(CompanyNames.CusipSymbol,'') as CusipSymbol,                              
Isnull(CompanyNames.OSISymbol,'') as OSISymbol,                              
Isnull(CompanyNames.IDCOSymbol,'') as IDCOSymbol                                                                
                    
FROM  #TEMPFundPositionsForDate_ValReport TFPVR               
INNER JOIN #T_CompanyFunds CF ON TFPVR.FundID = CF.CompanyFundID                                                                                                            
INNER JOIN T_Company Comp ON Comp.CompanyID = @companyID                                                           
INNER JOIN T_AUEC AUEC ON TFPVR.AUECID = AUEC.AUECID                                                                   
INNER JOIN #T_Asset A ON AUEC.AssetID = A.AssetID                                                                                                        
INNER JOIN T_Currency C ON TFPVR.CurrencyID = C.CurrencyID                                                                                                                                                                                                
LEFT JOIN T_CompanyStrategy CS ON TFPVR.Level2ID = CS.CompanyStrategyID                                                                                                                                               
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                                              
LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                                    
LEFT OUTER JOIN #DayMarkPrices DayMark ON DayMark.Symbol = TFPVR.Symbol                                                                      
LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON TFPVR.AUECID = AUECYesterDates.AUECID                       
                                            
LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange TDConvFactor                                                                         
ON TFPVR.CurrencyID = TDConvFactor.FCID                                                                                                                   
AND Comp.BaseCurrencyID = TDConvFactor.TCID                                                                                                       
AND DATEDIFF(d,TDConvFactor.DateCC,TFPVR.CreationDate) = 0                                                              
                                                         
LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange RRConvFactor                                                                                                                   
ON TFPVR.CurrencyID = RRConvFactor.FCID                       
AND Comp.BaseCurrencyID = RRConvFactor.TCID                                                                                                                   
AND DATEDIFF(d,RRConvFactor.DateCC,AUECYesterDates.YESTERDAYBIZDATE) = 0                                                                 
                                                                   
--LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXTDConvFactor                                                  
--ON (FXTDConvFactor.FCID = TFPVR.VsCurrencyID                                                                                                                           
--And FXTDConvFactor.TCID = Comp.BaseCurrencyID)                                    
--AND DATEDIFF(d,FXTDConvFactor.DateCC,TFPVR.CreationDate) = 0                                                             
                                                                                                       
--LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXRRConvFactor                                                                            
--ON (FXRRConvFactor.FCID = TFPVR.TradedCurrencyID                                                 
--And FXRRConvFactor.TCID = TFPVR.VsCurrencyID)                                                                                                                           
--AND DATEDIFF(d,FXRRConvFactor.DateCC,AUECYesterDates.YESTERDAYBIZDATE) = 0                                                 
                                                              
--LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXRRConvFactorVsToBase                                                                                                               
--ON (FXRRConvFactorVsToBase.FCID = TFPVR.VsCurrencyID                                                                        
--And FXRRConvFactorVsToBase.TCID = Comp.BaseCurrencyID)                                                                                                                           
--AND DATEDIFF(d,FXRRConvFactorVsToBase.DateCC,AUECYesterDates.YESTERDAYBIZDATE) = 0                                                                                     
                                                     
LEFT OUTER JOIN #CompanyNamesAndUDAData CompanyNames ON CompanyNames.Symbol = TFPVR.Symbol                              
LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CS.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                                  
LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                                                                                                 
WHERE TFPVR.OpenQuantity > 0                                                                                                                                  
                        
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
--   CASE ISNULL(MIN(RRConvFactor.ConversionMethod), 0)                                                                   
--    WHEN 0 THEN --MULTILPY                                                                                         
--     ISNULL(MIN(RRConvFactor.ConversionFactor), 0) * SUM(ISNULL(CashValueLocal, 0))                                                                                                                
--    WHEN 1 THEN --DIVIDE                                                                                                           
--  CASE SUM(ISNULL(CashValueLocal, 0))                                                                                                              
--   WHEN 0 THEN                                            
--    0                          
--   ELSE                                                                                          
--       CASE ISNULL(MIN(RRConvFactor.ConversionFactor), 1)                                                                                                              
--     WHEN 0 THEN                                                                                                              
--      0                                                                                                              
--     ELSE                     
--      (1 / ISNULL(MIN(RRConvFactor.ConversionFactor), 1)) * SUM(ISNULL(CashValueLocal, 0))                                                                                                                
--   END                                                                                                              
--  END                                                                         
--   END                                                        
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
   0.0 AS RRYMarkPriceCost,              
   0.0 AS FXRRYMarkPriceCost,                                                                                                                  
   0.0 AS TDConversionFactorCost,                                                                                                                  
   0 AS TDConversionMethodCost,                                                                                                                  
  0.0 AS FXTDConversionFactorCost,                                                                                                  
   0 AS FXTDConversionMethodCost,                                                                                                                  
   0 AS RRConversionFactorCost,      --As the base value is directly picked from the DB so no need to get coversion rate.                                      
   MIN(RRConvFactor.ConversionMethod) AS RRConversionMethodCost,                                                                                                                  
   0.0 AS FXRRConversionFactorCost,                                                                                                   
   0 AS FXRRConversionMethodCost,                                                                                                                  
   0.0 AS RRMarkPriceDay        ,                                                                                                                            
   0.0 AS FXRRMarkPriceDay  ,                                                                                                          
   0.0 AS RRYMarkPriceDay  ,                                                                                                                               
   0.0 AS FXRRYMarkPriceDay  ,                                                                                                                   
   0.0 AS RRConversionFactorDay ,                                                                                                                                   
   0 AS RRConversionMethodDay ,                                                                                                                          
   0.0 AS FXRRConversionFactorDay  ,                                                          
   0 AS FXRRConversionMethodDay ,                                                                             
   0.0 AS RRYConversionFactorDay  ,                                                                                                               
   0 AS RRYConversionMethodDay,                                                                                                                                    
   0.0 AS FXRRYConversionFactorDay,                                                                                                      
   0 AS FXRRYConversionMethodDay,                                      
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
 0 FXRRConversionFactorDay2,                                                                    
 0 FXRRConversionMethodDay2,                                
 'Unassigned' as MasterStrategyName,                                              
 '' as UnderlyingSymbol,                        
'' as IsSwapped,                      
'' as BloombergSymbol,                                                                       
 '' as SedolSymbol,                              
 '' as ISINSymbol,                              
 '' as CusipSymbol,                              
 '' as OSISymbol,                              
 '' as IDCOSymbol                                                                                                   
From     
--T_DayEndBalances CFCC       
PM_CompanyFundCashCurrencyValue CFCC     
    
INNER JOIN #T_CompanyFunds CF ON CFCC.FundID = CF.CompanyFundID                                                                                                                  
INNER JOIN T_Currency C ON CFCC.LocalCurrencyID = C.CurrencyID  --AND CFCC.BalanceType=1                                                                                                                   
INNER JOIN T_Currency CBase ON BaseCurrencyID = CBase.CurrencyID                                                                                            
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                             
LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                                                  
LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange RRConvFactor                                                                                                
ON CFCC.LocalCurrencyID = RRConvFactor.FCID                                                                                                                   
AND CFCC.BaseCurrencyID = RRConvFactor.TCID                                                      
And DATEDIFF(d,RRConvFactor.DateCC,CFCC.Date) = 0                                                                                         
                                                                                      
WHERE DateDiff(Day,Date,@RecentDateForNonZeroCash)=0                                                                                                         
Group By CFCC.FundID, LocalCurrencyID                                                                                                   
)                                                                                                                  
End                    
                                                  
--------------------------------------------------------------------------------------------------------                                                         
ElSE                                                                                                                                                      
 Begin                                                     
                                                                                                           
INSERT INTO #AUECYesterDates                                                                                                       
Select Distinct                                                   
V_SymbolAUEC.AUECID,                                                  
dbo.AdjustBusinessDays(@date,-1, V_SymbolAUEC.AUECID)                                                                                                                   
from V_SymbolAUEC                                                                                                           
                                                                                                                             
  INSERT INTO #DayMarkPrices                
  Select Symbol, [YesterDayMarkPrice], [TodayMarkPrice]                                                                                              
  From (Select Symbol, [Day], FinalMarkPrice From                                   
    (                                                                                                             
  Select                                                                                                                   
   PM_DayMarkPrice.Symbol Symbol,                                                                                                            
   Case                                                                                           
    When Datediff(d,Date,AUECYesterDates.YESTERDAYBIZDATE) = 0                                                                                                     
    THEN 'YesterDayMarkPrice'                                                               
    ELSE 'TodayMarkPrice'                                                                      
   End As [Day],               
   FinalMarkPrice                                                                                                                
   From PM_DayMarkPrice                                                                                          
   INNER JOIN V_SymbolAuec ON PM_DayMarkPrice.Symbol = V_SymbolAuec.Symbol                                                                                                                  
   INNER JOIN #AUECYesterDates AUECYesterDates ON AUECYesterDates.AUECID = V_SymbolAuec.AUECID                                                                                                   
  Where (DATEDIFF(d,PM_DayMarkPrice.Date,@date) = 0                                                                                                                   
  Or DATEDIFF(d,PM_DayMarkPrice.Date,AUECYesterDates.YESTERDAYBIZDATE) = 0)                                                                                                                  
    ) AS TempDayMarkPrices                                                                  
   ) As MarkPrices                                                                                                                  
  PIVOT                                                                                                                   
   (                                                                                                                  
    MAX(FinalMarkPrice)                                                                                                                   
 For [Day] IN ([YesterDayMarkPrice],[TodayMarkPrice])                                                                                       
   ) AS Pvt;                                                 
                                                
                                   
                             
  Select                                                                                                                                                        
   TFPVR.Symbol AS Symbol,                              
   TFPVR.OpenQuantity AS Quantity,                        
   TFPVR.AveragePrice AS CostPrice,                                                                                       
   ISNULL(TFPVR.OpenTotalCommissionandFees, 0) AS TotalCommissionandFees,                                              
   1 As TotalCostPerShare,                                                                                               
   CASE AUEC.AssetID                                                                         
   When 5                                                                                                                                                            
    THEN 0.0                
   Else                                                                                                                                                          
    0.0                                                                                          
   End As TotalCost,                                                                                                               
   0.0 AS MarketValue,                                                                                                                      
   ((1 * TFPVR.OpenQuantity) - (TFPVR.OpenQuantity * TFPVR.AveragePrice)) AS Gain,                                                              
   FundName,                                                           
   CASE TFPVR.OrderSideTagValue                                                                                                                                                                                               
    WHEN '1' Then 'Long'                                                                                                   
    WHEN '2' Then 'Short'                                                                                                                                                                   
    WHEN '5' Then 'Short'    --Sell Short                                                                                 
    WHEN 'A' Then 'Long'     --Buy To Open                                                                                             
    WHEN 'B' Then 'Long'    --Buy To Close                                                                                                                                                       
    WHEN 'C' Then 'Short'    --Sell To Open                                                                       
    WHEN 'D' Then 'Short'   --Sell To Close                                                                                                                                    
   END As [Position Type],                                                                                                                                                                     C.CurrencySymbol,                                              
  
    
      
        
          
            
   ' ' AS LanguageName,                                                                                                                
   0.0 AS TotalCostInBaseCurrency,                                                                                    
   Comp.BaseCurrencyID AS BaseCurrencyID,                                
   '' AS BaseCurrencyLanguageName,                                                                                                                                                                        
   '' AS GroupParamaeter1,                                                                                                                                                                                                                              
   '' AS GroupParamaeter2,                                    
   '' AS GroupParamaeter3,                                                                                                                                  
   0.0 AS MarketValueInBaseCurrency,                                                                                                                                          
   0.00 AS GainInBaseCurrency,                                                                                                    
   1 AS AggregateOption, -- Selected as 1 by default so as not to use aggregate option for the first time load.                                                                                                                                          
   0.0 AS YesterdayMarketValue,                                                                           
   0.0 AS YesterdayMarketValueInBaseCurrency,                                        
   CONVERT(VARCHAR(10), TFPVR.CreationDate, 101) AS TradeDate,                                                                                                                                                          
   ISNULL(CompanyNames.FutureMultiplier, 0) AS Multiplier,                                                                                                                 
   AUEC.AUECID AS AUECID,                                         
   AUEC.AssetID AS AssetID,                                                        
   AUEC.BaseCurrencyID AS CurrencyID,                                                                                                                           
   ISNULL(TFPVR.TradedCurrencyID, 0) AS TradedCurrencyID,                                                                                    
   ISNULL(TFPVR.VsCurrencyID, 0) AS VsCurrencyID,                        
   IsNull(CMF.MasterFundName,'Unassigned') AS MasterFundName,                                                                                              
   '' AS GroupParamaeter4,                                                                                                                  
   isnull(BenchMarkRate + Differential,0) AS I1,                                                                                                                                    
   DC,                                                                                                         
   0.0 AS RRMarkPriceCost,                                                                                                        
   0.0 AS FXRRMarkPriceCost,                                                                                                                                     
   0.0 AS RRYMarkPriceCost,                                                                                                                                    
   0.0 AS FXRRYMarkPriceCost,                                                                                             
   0.0 AS TDConversionFactorCost,                                                                                                 
  0 AS TDConversionMethodCost,                                                                
   0.0 AS FXTDConversionFactorCost,                                                                                                                 
   0 AS FXTDConversionMethodCost,                                                                                                                                    
   0.0 AS RRConversionFactorCost,                                                                                                              
   0 AS RRConversionMethodCost,                                                                                                       
   0.0 AS FXRRConversionFactorCost,                                                                                                            
   0 AS FXRRConversionMethodCost,                                                              
   ISNULL(PMDMP.TodayMarkPrice, 0.0) AS RRMarkPriceDay,                                                                                               
   ISNULL(FXRRConvFactor.ConversionFactor, 0) AS FXRRMarkPriceDay,                               
   ISNULL(PMDMP.YesterdayMarkPrice, 0.0) / IsNull(SplitTab.SplitFactor,1) AS RRYMarkPriceDay,                                                                         
                         
   ISNULL(FXDayMarkYest.ConversionFactor, 0)  AS FXRRYMarkPriceDay,                                                                                                                              
   CASE Comp.BaseCurrencyID                                                                                                                  
    WHEN TFPVR.CurrencyID                                                               
    THEN 1                                                                                                
    ELSE ISNULL(RRConvFactor.ConversionFactor, 0)                                               
   END AS RRConversionFactorDay,                                                                                                                                
                                                                                                                                      
   isnull(RRConvFactor.ConversionMethod, 0) AS RRConversionMethodDay,                                                                     
                                                                    
 CASE Comp.BaseCurrencyID                                                                                                                   
     WHEN TFPVR.VsCurrencyID                                                                           
     THEN 1                                                                                                                                
 ELSE ISNULL(RRConvFactor.ConversionFactor, 0)                                                                                             
 END AS FXRRConversionFactorDay,                                                                          
                                                  
   isnull(RRConvFactor.ConversionMethod, 0) AS FXRRConversionMethodDay,                     
                                                                                                                         
   CASE Comp.BaseCurrencyID                                                                                                                    
    WHEN TFPVR.CurrencyID                                                         
     THEN 1                                                                                      
    ELSE                                                                              
     ISNULL(RRYConvFactor.ConversionFactor, 0)                                                                                                                                 
   END AS RRYConversionFactorDay,                                                                     
                                                                         
   ISNULL(RRYConvFactor.ConversionMethod, 0) AS RRYConversionMethodDay,                                                                                                                               
                                                                                                            
   CASE Comp.BaseCurrencyID                                                                                                           
     WHEN TFPVR.VsCurrencyID                       
     THEN 1                                                                                                             
    ELSE ISNULL(RRYConvFactor.ConversionFactor, 0)                                                      
   END AS FXRRYConversionFactorDay,                                                                                                                          
                           
   isnull(RRYConvFactor.ConversionMethod, 0) AS FXRRYConversionMethodDay,                      
              
   dbo.GetSideMultiplier(TFPVR.OrderSideTagValue) AS SideMultiplier,                                                      
   isnull(NotionalValue, 0) AS NotionalValue,                                                                                                                                    
   CompanyNames.CompanyName AS SymbolCompanyName,                                                                                                                                  
   TFPVR.TaxlotID AS TaxlotID,                                                                                                                              
   dbo.GetFormattedDatePart(dbo.AdjustBusinessDays(@date, -1, TFPVR.AUECID)) AS YesterdayBusinessDate,                                    
   dbo.GetFormattedDatePart(dateadd(d, -1, @date)) AS FXYesterdayBusinessDate,                                                                                       
   CF.CompanyFundID As CompanyFundID,                                                                       
   A.AssetName AS AssetName,                                                                                                            
   TFPVR.Level2ID AS StrategyID,                                                                                                  
 ISNULL(CS.StrategyName, 'Strategy Unallocated') AS StrategyName,                                                                                                                                
 ISNULL(CompanyNames.Symbol, 'Undefined') AS UDATickerSymbol,                                                                                                                    
 ISNULL(CompanyNames.UDAAssetName, 'Undefined') AS UDAAssetName,                                                                                                                  
 ISNULL(CompanyNames.UDASecurityTypeName, 'Undefined') AS UDASecurityTypeName,                                                                
 ISNULL(CompanyNames.UDASectorName, 'Undefined') AS UDASectorName,                                                                                                                                
 ISNULL(CompanyNames.UDASubSectorName, 'Undefined') AS UDASubSectorName,                                                            
 ISNULL(CompanyNames.UDACountryName, 'Undefined') AS UDACountryName,                                                                                                    
 ISNULL(CompanyNames.PutOrCall, '-1') AS PutOrCall,                                                             
                                                                    
CASE Comp.BaseCurrencyID                                                                                                                   
 WHEN TFPVR.VsCurrencyID                    
 THEN 1                                                                                                                                
 ELSE                                                                    
  CASE                                                                     
   WHEN DateDiff(d,TFPVR.CreationDate,@date)=0                                                                    
   THEN                                                         
    CASE                                                                                            
     WHEN TFPVR.FXRate > 0                     
     THEN TFPVR.FXRate                                                                                                                                
     ELSE ISNULL(RRConvFactor.ConversionFactor, 0)                                                                      
    END                                                                    
   ELSE ISNULL(RRConvFactor.ConversionFactor, 0)                                                                             
  END                                                                    
END AS FXRRConversionFactorDay2,                                                                     
                                                                    
CASE                                             
 WHEN DateDiff(d,TFPVR.CreationDate,@date)=0                                              
 THEN                                                                     
  CASE ISNULL(TFPVR.FXRate, 0)                                                                                                                  
   WHEN 0                                                                           
   THEN ISNULL(RRConvFactor.ConversionMethod, 0)                                                                    
   ELSE                                                              
    CASE                                                                                                 
     WHEN ISNULL(FXConversionMethodOperator, 'M')='M' And TFPVR.FXRate > 0                                                                        
     THEN 0                                                                 
     WHEN ISNULL(FXConversionMethodOperator, 'D')='D' And TFPVR.FXRate > 0                                                          
     THEN 1                                                                                              
     ELSE ISNULL(RRConvFactor.ConversionMethod, 0)                                                                                                                          
    END                                        
  END                                                                    
 ELSE ISNULL(RRConvFactor.ConversionMethod, 0)                                                                
END AS FXRRConversionMethodDay2,                                                        
IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                                              
ISNull(TFPVR.UnderlyingSymbol,'') as UnderlyingSymbol,                        
TFPVR.IsSwapped,                      
Isnull(CompanyNames.BloombergSymbol,'') as BloombergSymbol,                                                                       
Isnull(CompanyNames.SedolSymbol,'') as SedolSymbol,                              
Isnull(CompanyNames.ISINSymbol,'') as ISINSymbol,                              
Isnull(CompanyNames.CusipSymbol,'') as CusipSymbol,                              
Isnull(CompanyNames.OSISymbol,'') as OSISymbol,                              
Isnull(CompanyNames.IDCOSymbol,'') as IDCOSymbol                                                                     
                                                                 
FROM  #TEMPFundPositionsForDate_ValReport TFPVR                                                                                          
INNER JOIN T_Company Comp ON Comp.CompanyID = @companyID                                                                                                                                                                          
INNER JOIN T_AUEC AUEC ON TFPVR.AUECID = AUEC.AUECID                                                                                                                                                                                                  
INNER JOIN #T_Asset A ON AUEC.AssetID = A.AssetID                                                                                       
INNER JOIN T_Currency C ON TFPVR.CurrencyID = C.CurrencyID                                                                                                                            
INNER JOIN #T_CompanyFunds CF ON TFPVR.FundID = CF.CompanyFundID                 
LEFT JOIN T_CompanyStrategy CS ON TFPVR.Level2ID = CS.CompanyStrategyID                                                                                          
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                                                                   
 
LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                    
LEFT OUTER JOIN #DayMarkPrices PMDMP ON TFPVR.Symbol = PMDMP.Symbol                            
LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON TFPVR.AUECID = AUECYesterDates.AUECID                       
                                                                                                              
LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXDayMarkYest                                                                                                                  
ON (FXDayMarkYest.FCID = TFPVR.TradedCurrencyID                                                                                                                   
And FXDayMarkYest.TCID = TFPVR.VsCurrencyID)                                                                                                                   
AND DATEDIFF(d,FXDayMarkYest.DateCC,AUECYesterDates.YESTERDAYBIZDATE) = 0                                                                      
              
LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange RRYConvFactor                                                                                                                           
ON TFPVR.CurrencyID = RRYConvFactor.FCID                                                                          
AND Comp.BaseCurrencyID = RRYConvFactor.TCID                                                                      
AND DATEDIFF(d,RRYConvFactor.DateCC,AUECYesterDates.YESTERDAYBIZDATE) = 0                      
                                                                 
LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange RRConvFactor                                                                                                                   
ON TFPVR.CurrencyID  = RRConvFactor.FCID                                                                                               
AND Comp.BaseCurrencyID = RRConvFactor.TCID                                                                                                        
AND DATEDIFF(d,RRConvFactor.DateCC,@date) = 0                                                                                                                  
                                           
LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXRRConvFactor                                                                                                                   
ON (FXRRConvFactor.FCID = TFPVR.TradedCurrencyID                                                                                      
And FXRRConvFactor.TCID = TFPVR.VsCurrencyID)                                                                                                                   
AND DATEDIFF(d,FXRRConvFactor.DateCC,@date) = 0                                                                           
                                                                 
--new code 4 lines                                                                          
--LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXRRConvFactorVsToBase                                                                             
--ON (FXRRConvFactorVsToBase.FCID = TFPVR.VsCurrencyID                         
--And FXRRConvFactorVsToBase.TCID = Comp.BaseCurrencyID)                                                                                                                           
--AND DATEDIFF(d,FXRRConvFactorVsToBase.DateCC,@date) = 0                                                                             
                                        
                              
--LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange FXRRYConvFactor                                                                                                                           
--ON (FXRRYConvFactor.FCID = TFPVR.VsCurrencyID                                                                                                                           
--And FXRRYConvFactor.TCID = Comp.BaseCurrencyID)                                                                           
--AND DATEDIFF(d,FXRRYConvFactor.DateCC, AUECYesterDates.YESTERDAYBIZDATE) = 0                                                           
                                            
LEFT OUTER JOIN #CompanyNamesAndUDAData CompanyNames ON CompanyNames.Symbol = TFPVR.Symbol                                                                 
LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CS.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                                  
LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                             
Left Outer Join #TempSplitFactorForOpen SplitTab on SplitTab.TaxlotID= TFPVR.TaxlotID                                                          
WHERE TFPVR.OpenQuantity > 0                                                                                                                  
                                                                                                                    
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
   0.0 AS RRYMarkPriceCost,                                                   
   0.0 AS FXRRYMarkPriceCost,                                                                                                                  
   0.0 AS TDConversionFactorCost,                                                                           
   0 AS TDConversionMethodCost,                 
   0.0 AS FXTDConversionFactorCost,                                       
   0 AS FXTDConversionMethodCost,                                                           
   0.0 AS RRConversionFactorCost,                                                                                                                  
   0 AS RRConversionMethodCost,                                                                                                                  
   0.0 AS FXRRConversionFactorCost,                                                                                                                  
   0 AS FXRRConversionMethodCost,                                                                                                            
   0.0 AS RRMarkPriceDay ,                                                                                                                            
   0.0 AS FXRRMarkPriceDay  ,                                                                                                                                  
   0.0 AS RRYMarkPriceDay  ,                                                                                                
   0.0 AS FXRRYMarkPriceDay  ,                                                             
   0 AS RRConversionFactorDay,      --As the base value is directly picked from the DB so no need to get coversion rate.                                                                                                          
   MIN(RRConvFactor.ConversionMethod) AS RRConversionMethodDay,                                                                                                      
   0.0 AS FXRRConversionFactorDay  ,                                                                                                                                  
   0 AS FXRRConversionMethodDay ,                                                                                                         
   0.0 AS RRYConversionFactorDay  ,                                                                                          
   0 AS RRYConversionMethodDay,                                                                                                              
   0.0 AS FXRRYConversionFactorDay,                                                                                                                                    
   0 AS FXRRYConversionMethodDay,                                                                                         
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
 0 FXRRConversionFactorDay2,                                                                    
 0 FXRRConversionMethodDay2,                                                        
 'Unassigned' as MasterStrategyName,                                              
 '' as UnderlyingSymbol,                        
'' as IsSwapped,                      
 '' as BloombergSymbol,                                                                       
 '' as SedolSymbol,                              
 '' as ISINSymbol,                 
 '' as CusipSymbol,                              
 '' as OSISymbol,                              
 '' as IDCOSymbol                                                         
                                                                                                     
From T_DayEndBalances CFCC                          
INNER JOIN T_Currency C ON CFCC.LocalCurrencyID = C.CurrencyID and CFCC.BalanceType = 1                                                                                                                               
INNER JOIN T_Currency CBase ON BaseCurrencyID = CBase.CurrencyID                                                           
INNER JOIN #T_CompanyFunds CF ON CFCC.FundID = CF.CompanyFundID                                                                                                                          
LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                     
LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                     
                                                                                                 
LEFT OUTER JOIN #TEMPGetConversionFactorForGivenDateRange RRConvFactor                                                                                                                   
ON CFCC.LocalCurrencyID = RRConvFactor.FCID                                                                                                                   
AND CFCC.BaseCurrencyID = RRConvFactor.TCID                                        
AND DATEDIFF(d,RRConvFactor.DateCC,CFCC.Date) = 0                                                                 
                                                                                                             
WHERE DateDiff(Day,Date,@RecentDateForNonZeroCash)=0                                                                                                                     
GROUP BY CFCC.FundID, LocalCurrencyID                                                                                                              
)                                                                                                            
END                                                                                                                                                              
------------------------------------------------------------------------------------------------------              
              
--Dropping temporary tables                                                                                                                                                                      
DROP TABLE #TEMPGetConversionFactorForGivenDateRange                    
DROP TABLE #TEMPFundPositionsForDate_ValReport,#PositionTable,#DayMarkPrices                     
DROP TABLE #AUECYesterDates ,#CompanyNamesAndUDAData ,#TempSplitFactorForOpen                  
DROP TABLE #PM_Taxlots,#T_CompanyFunds, #T_Asset                                                                                       
END           