              
              
/******************************************************************                
                                                                                                                                                                                                                                                       
Author:  <Author: Sandeep>                    
Create date: Create Date: 17-Oct-2008                                           
Description: Description: To get Realize and UnRealized PnL for the Date Ranges                                                                       
                                      
Modified Date: 24-April-2012                                                                            
Modified By : Sandeep Singh                                                                                                                                                
Description: FX Spot, FX Forward and Fixed Income related changes                 
                
Modified Date: 06-MAY-2013                                                                            
Modified By : Ankit                                                                                                                                                
Description: Removed all configurable parameters and added in Table T_ReportPreferences                
              
Modified Date: 03-April-2014                                                                            
Modified By : Ankit                                                                                                                                                
Description: Added Trade Attributed for both Closed and Open Trades              
1309,1310,1311,1312,1313,1314,1315,1316,1317,1318,1319,1320,1321            
[PMGetRealizeAndUnrealizePnL_Testing] '4/28/2014','8/25/2014','1312','1,2,3,4,5,6,7,8,9','MTM_v1'              
                                                                                                  
******************************************************************/                
                                 
CREATE PROCEDURE  [dbo].[PMGetRealizeAndUnrealizePnL_RJO]                                                                                     
(                                                                                                                                                                                                                                                          
 -- Add the parameters for the stored procedure here                                          
 @StartDate datetime,                 
 @EndDate datetime,                
 @Fund Varchar(max),                
 @Asset varchar(max),                
 @ReportId Varchar(100)                
)  with Recompile                                                                                                                                                                                                                                              
  
    
      
        
          
            
              
                
AS                                                                                                                                                                                                                                                          
BEGIN                                                                                                                                                                                                                                                          
 -- SET NOCOUNT ON added to prevent extra result sets from                                                
-- interfering with SELECT statements.                                                                 
 SET NOCOUNT ON;                                                     
--Declare @StartDate datetime                              
--Declare @EndDate datetime         
--Declare @Fund varchar(max)                                      
--Declare @Asset varchar(max)                       
--Declare @ReportId varchar(max)                                              
--Set @StartDate='10/01/2012'                                              
--Set @EndDate='10/10/2012'                        
--Set @Fund = '1182,1183'                      
--Set @Asset= '1,2,3'                      
--Set @ReportId = 'MTM_V0'                           
                    
-----------------------------------------------------------------------------------                      
                
Declare @ShowFutureMktValueAsUnrealizedOrZero int                         
Set  @ShowFutureMktValueAsUnrealizedOrZero = (Select FutureMV_ZeroOrEndingMVOrUnrealized from T_ReportPreferences where ReportId = @ReportId)                
                   
Declare @ShowInternationalFutureOptionMktValueAsMVorUnrealizedOrZero int                                                                    
Set @ShowInternationalFutureOptionMktValueAsMVorUnrealizedOrZero=(Select InternationalFutOptionsMV_ZeroOrEndingMVOrUnrealized from T_ReportPreferences where ReportId = @ReportId)                
                
Declare @ShowFXMktValueAsUnrealizedOrZero int                         
Set  @ShowFXMktValueAsUnrealizedOrZero = (Select FXMV_ZeroOrEndingMVOrUnrealized from T_ReportPreferences where ReportId = @ReportId)                
                
Declare @SwapMV_ZeroOrEndingMVOrUnrealized int                                                                    
Set @SwapMV_ZeroOrEndingMVOrUnrealized=(Select SwapMV_ZeroOrEndingMVOrUnrealized from T_ReportPreferences where ReportId = @ReportId)                
                
-----------------------------------------------------------------------------------                
Declare @IncludeFXPNLinEquity bit                                                                                
Set @IncludeFXPNLinEquity=(Select IncludeFXPNLinEquity from T_ReportPreferences where ReportId = @ReportId)                
                                                                                                              
Declare @IncludeFXPNLinEquityOption bit                                                              
Set @IncludeFXPNLinEquityOption=(Select IncludeFXPNLinEquityOption from T_ReportPreferences where ReportId = @ReportId)                
                
Declare @FuturePNLWithBothOrEndFXRate bit                                                                                
Set @FuturePNLWithBothOrEndFXRate=(Select IncludeFXPNLinFutures from T_ReportPreferences where ReportId = @ReportId)                
                                                                                                              
Declare @InternationalFutureOptionPNLWithBothOrEndFXRate bit                                                              
Set @InternationalFutureOptionPNLWithBothOrEndFXRate=(Select IncludeFXPNLinInternationalFutOptions from T_ReportPreferences where ReportId = @ReportId)                
                                                            
Declare @IncludeFXPNLinFX bit                                                                                
Set @IncludeFXPNLinFX=(Select IncludeFXPNLinFX from T_ReportPreferences where ReportId = @ReportId)                
                                                                                                          
Declare @IncludeFXPNLinSwaps bit                                                              
Set @IncludeFXPNLinSwaps=(Select IncludeFXPNLinSwaps from T_ReportPreferences where ReportId = @ReportId)                
                
Declare @IncludeFXPNLinOther bit                                                              
Set @IncludeFXPNLinOther =(Select IncludeFXPNLinOther from T_ReportPreferences where ReportId = @ReportId)                
                
-----------------------------------------------------------------------------------                
Declare @IncludeCommissionInPNL_Equity bit                
Set @IncludeCommissionInPNL_Equity =(Select IncludeCommissionInPNL_Equity from T_ReportPreferences where ReportId = @ReportId)         
                                                                                                              
Declare @IncludeCommissionInPNL_EquityOption bit                
Set @IncludeCommissionInPNL_EquityOption =(Select IncludeCommissionInPNL_EquityOption  from T_ReportPreferences where ReportId = @ReportId)                
                
Declare @IncludeCommissionInPNL_Futures bit                 
Set  @IncludeCommissionInPNL_Futures = (Select IncludeCommissionInPNL_Futures from T_ReportPreferences where ReportId = @ReportId)                
                                                            
Declare @IncludeCommissionInPNL_FutOptions bit                                                         
Set @IncludeCommissionInPNL_FutOptions=(Select IncludeCommissionInPNL_FutOptions from T_ReportPreferences where ReportId = @ReportId)                
                
Declare @IncludeCommissionInPNL_Swaps bit                                  
Set @IncludeCommissionInPNL_Swaps=(Select IncludeCommissionInPNL_Swaps from T_ReportPreferences where ReportId = @ReportId)                
                                                                                                              
Declare @IncludeCommissionInPNL_FX bit                                           
Set @IncludeCommissionInPNL_FX =(Select IncludeCommissionInPNL_FX from T_ReportPreferences where ReportId = @ReportId)                
                
Declare @IncludeCommissionInPNL_Other bit                                                              
Set @IncludeCommissionInPNL_Other =(Select IncludeCommissionInPNL_Other from T_ReportPreferences where ReportId = @ReportId)                
                
---------------------------------------------------------------------------------------------------------                
---------------------------------------------------------------------------------------------------------                
                
Declare @RecentDateForNonZeroCash DateTime                          
Set  @RecentDateForNonZeroCash = (Select dbo.[GetRecentDateForNonZeroCash](@EndDate))                          
                                                            
Declare @MinTradeDate DateTime                                                                                
Declare @BaseCurrencyID int                                                                            
Set @BaseCurrencyID=(select BaseCurrencyID from T_Company)                                                                 
                                                                                                              
Declare @DefaultAUECID int                                                                    
Set @DefaultAUECID=(select DefaultAUECID from T_Company)                                                                
                                                                                                                                           
-- get Mark Price for Start Date                                                                 
Create Table #MarkPriceForStartDate                                                                                   
(                                       
Finalmarkprice float ,                                                                                     
Symbol varchar(50)                                                                                                                                                    
)                                                                                                                                                                                                      
                                                                                                                                                                       
-- get Mark Price for End Date                                                                                                                          
Create Table #MarkPriceForEndDate                                                                                                                                           
(                                               
Finalmarkprice float ,            
Symbol varchar(50)                                                                                                                                                                                                              
)                                                                                      
-- get forex rates for 2 date ranges                   
                                                                                                                                                                                                                           
Create Table #FXConversionRates                                                                                                                                    
                 
(                                                                                                                                                                                                                    
 FromCurrencyID int,                                                                                                                                                                                          
 ToCurrencyID int,                                                                                              
 RateValue float,                                                                                                              
 ConversionMethod int,                                                                              
 Date DateTime,                                                                     
 eSignalSymbol varchar(max)                                                                        
)                                                                                                                                      
-- get yesterday business day AUEC wise                                             
Create Table #AUECYesterDates                                                                                                             
(                                                                                     
  AUECID INT,                                                      
   YESTERDAYBIZDATE DATETIME                                                                                                                                               
)                                                                                                                          
-- get business day AUEC wise for End Date                                                                                                    
Create Table #AUECBusinessDatesForEndDate                                                                                   
(                                                                                 
   AUECID INT,                                                                                                                                             
   YESTERDAYBIZDATE DATETIME                                           
)                                                                                                                                                                                          
-- get Security Master Data in a Temp Table                     
Create Table #SecMasterDataTempTable                                                                                                                                                                                                    
(                                                                                                                                                                                             
  AUECID int,                                                   
  TickerSymbol Varchar(100),                                                                                                        
  CompanyName  VarChar(500),                                                                                                                                                               
  AssetID int,                                                      
  AssetName Varchar(100),                                                                                                                           
  SecurityTypeName Varchar(200),                                                                                                                                                                        
  SectorName Varchar(100),                                                                                                                                                                                                                       
  SubSectorName Varchar(100),                                                                                                                                          
  CountryName  Varchar(100),                                                                                                                                                                
  PutOrCall Varchar(5),                                                                 
  Multiplier Float,                                                                                       
  LeadCurrencyID int,                                                                             
  VsCurrencyID int,                                                                                              
  CurrencyID int,                              
  ExpirationDate Datetime,                                                                              
  UnderlyingSymbol Varchar(100),                                          
  BloombergSymbol Varchar(100),                                          
  SedolSymbol Varchar(100),                                          
  ISINSymbol Varchar(100),                                          
  CusipSymbol Varchar(100),                                          
  OSISymbol Varchar(100),                                          
  IDCOSymbol Varchar(100)                                               
)                                                                       
                                                            
Insert Into #SecMasterDataTempTable                                                                                                                                             
Select                                                                                                                                 
 AUECID ,                                                                                                                 
 TickerSymbol ,                                                                                                                                                 
 CompanyName  ,                                                                                                   
 AssetID,                                                                                                                
 AssetName,                                  
SecurityTypeName,                                                                                 
 SectorName ,                                      
 SubSectorName ,                                                                                                                                                                                         
 CountryName ,                                                                      
 PutOrCall ,                                        
 Multiplier ,                                                                 
 LeadCurrencyID ,                                                       
 VsCurrencyID,          
 CurrencyID,                 
 ExpirationDate,                                                                                       
 UnderlyingSymbol,                                          
 BloombergSymbol,                               
 SedolSymbol,                                          
 ISINSymbol,                                          
 CusipSymbol,                                          
OSISymbol,                                          
IDCOSymbol                                                                                                                      
     From V_SecMasterData                                                          
                                                                                
                                            
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
                                                           
CREATE TABLE #PM_Taxlots                  
(                                                        
 [TaxLot_PK] [bigint] NOT NULL,                                                 
 [TaxLotID] [varchar](50) NOT NULL,                                         
 [Symbol] [varchar](100) NOT NULL,                                                        
 [TaxLotOpenQty] [float] NOT NULL,                                      
 [AvgPrice] [float] NOT NULL,                                                        
 [TimeOfSaveUTC] [datetime] NULL,                                                        
 [GroupID] [nvarchar](50) NULL,                                                        
 [AUECModifiedDate] [datetime] NULL,                                                    
 [FundID] [int] NULL,                                                        
 [Level2ID] [int] NULL,             
 [OpenTotalCommissionandFees] [float] NULL,                                                        
 [ClosedTotalCommissionandFees] [float] NULL,                                                        
 [PositionTag] [int] NULL,                                                        
 [OrderSideTagValue] [nchar](10) NULL,                                                
 [TaxLotClosingId_Fk] [uniqueidentifier] NULL,                                                        
 [ParentRow_Pk] [bigint] NULL,                                                  
 [AccruedInterest] [Float] Null,                                
 [FXRate] [Float] NULL,                                
 [FXConversionMethodOperator] [varchar](3) NULL                                                    
)                                                        
                                                        
Insert Into #PM_Taxlots                                                        
 Select                                       
 TaxLot_PK,                                                          
 TaxLotID,                                                          
 Symbol,                                                          
 TaxLotOpenQty,                                                          
 AvgPrice,                                                          
 TimeOfSaveUTC,                                                          
 GroupID,                                                          
 AUECModifiedDate,                                                          
 PM_Taxlots.FundID,                                                          
 Level2ID,                                                          
 OpenTotalCommissionandFees,                                                          
 ClosedTotalCommissionandFees,                                                          
 PositionTag,                                                          
 OrderSideTagValue,                                                          
 TaxLotClosingId_Fk,                                                          
 ParentRow_Pk,                                                    
 AccruedInterest,                                
 FXRate,                                
 FXConversionMethodOperator                                
                                
 From PM_Taxlots                                               
 INNER JOIN @T_FundIDs FundIDs ON PM_Taxlots.FundID = FundIDs.FundID                                               
 Inner Join #SecMasterDataTempTable SM On SM.TickerSymbol =  PM_Taxlots.Symbol                                        
 INNER JOIN @T_AssetIDs AssetIDs ON SM.AssetID = AssetIDs.AssetID                                                      
 Where DateDiff(Day,AUECModifiedDate,@EndDate) >= 0                                     
                     
                                          
CREATE TABLE #T_CorpActionData                                            
(                                           
 Symbol varchar(100),                                            
 SplitFactor float,                        
 EffectiveDate Datetime,                                            
 CorpActionID varchar(100),                                             
 IsApplied bit                                            
)                                            
                                             
Insert into #T_CorpActionData                            
(Symbol, SplitFactor, EffectiveDate,CorpActionID,IsApplied)                                            
select                                             
Symbol,                                             
SplitFactor,                                            
EffectiveDate,                                            
CorpActionID,                                            
IsApplied                                             
from V_CorpActionData                                              
where IsApplied = 1 and CorpActionTypeID=6                     
and Datediff(d,@StartDate, Effectivedate) >= 0 and Datediff(d,Effectivedate, @EndDate) >= 0                                                                                             
                                   
Create Table #TempSplitFactorForOpen                                                                                
(                                                                                
TaxlotID varchar(50),                                                                                
Symbol varchar(100),                                                                                
SplitFactor float,                                     
EffectiveDate Datetime                                                                                 
)                                                                                
                                                                                
Insert InTo #TempSplitFactorForOpen                                                                                
select                                 
NA.TaxlotID,                                
NA.Symbol,                   
IsNull(EXP(SUM(LOG(NA.splitFactor))),1) as SplitFactor,                                
NA.EffectiveDate  from                                                                                               
(                                                                                
  Select                                 
  A.Taxlotid,                                
  A.symbol,                                 
  VCA.SplitFactor,                   
  VCA.EffectiveDate                                 
  from                                              
 (                                                                                
  Select                                                                                 
  TaxlotId,                                                                                
  PT.Symbol as Symbol ,              
  G.ProcessDate as TradeDate                                                                                                                                                          
  from #PM_Taxlots PT                                 
    Inner Join T_Group G on G.GroupID = PT.GroupID                  
    Where TaxLotOpenQty <> 0                                              
     and Taxlot_PK in                                                                  
     (                                                                                                     
   Select max(Taxlot_PK) from #PM_Taxlots                                                                                                     
   Where DateDiff(d,#PM_Taxlots.AUECModifiedDate,@EndDate) >=0                                                                                               
    Group by TaxlotId                                                                                   
    )                                   
 ) as A                                                  
 Inner Join #T_CorpActionData VCA on A.Symbol = VCA.Symbol                                                                                             
 Where DateDiff(Day,A.TradeDate,VCA.EffectiveDate) >= 0                                                                                           
) as NA                                                            
Group by NA.TaxlotId,NA.symbol,NA.EffectiveDate                                                                                 
                                 
Create Table #TempSplitFactorForClosed_1                                                                                
(                                                 
TaxlotID varchar(50),                                                       
Symbol varchar(100),                                                                                
SplitFactor float,                                                                    
Effectivedate DateTime                                   
)                                                                  
                                                                
Insert InTo #TempSplitFactorForClosed_1                                                                      
Select                                 
A.Taxlotid,                                
A.symbol,                                 
VCA.SplitFactor,                                
VCA.Effectivedate as Effectivedate                                 
from                                                                                             
 (                                                                                
  Select                                                                                 
  PT.TaxlotId,                                                                                
  PT.Symbol as Symbol  ,                  
  G.ProcessDate as Tradedate                                                                                                                                                                                                                                  
  
    
      
        
          
            
              
                
                 
  from PM_TaxlotClosing  PTC                                                                                                                         
  Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                            
     Inner Join T_Group G on G.GroupID = PT.GroupID                                                                                 
     Where DateDiff(d,@StartDate,PTC.AUECLocalDate) >=0                                                                                                                                                                                               
      and DateDiff(d,PTC.AUECLocalDate,@EndDate)>=0                                                                                                              
      and PTC.ClosingMode<>7                                                                                
 ) as A                                          
 Inner Join #T_CorpActionData VCA on A.Symbol = VCA.Symbol                                                                                                                                            
   Where DateDiff(Day,A.TradeDate,VCA.EffectiveDate) >= 0                                                            
Select Distinct                                 
TaxlotID,                                 
Symbol,                                
SplitFactor,                                
Effectivedate                                 
into #TempSplitFactorForClosed_2                                  
from #TempSplitFactorForClosed_1               
                               
Create Table #TempSplitFactorForClosed                                                                       
(                                                            
TaxlotID varchar(50),                                                      
Symbol varchar(100),                                                                        
SplitFactor float,                                                      
Effectivedate DateTime                                                         
)                                                        
                                                        
Insert into #TempSplitFactorForClosed                                                        
Select                                                         
NA.TaxlotID,                                                         
NA.Symbol,                                                         
IsNull(EXP(SUM(LOG(NA.splitFactor))),1) as SplitFactor,                                                      
Max(Effectivedate) as Effectivedate                                                           
from #TempSplitFactorForClosed_2 NA Group by NA.TaxlotID,NA.symbol                        
                      
                                                               
Set @MinTradeDate =              
(              
select min(AuecLocalDate) from PM_Taxlots T              
inner join T_group G on G.groupID =  T.GroupID                
where               
T.taxlotopenqty <> 0               
and               
T.Taxlot_pk in (select max (Taxlot_pk) from PM_taxlots where datediff (d,Auecmodifieddate,@StartDate)>=0 group by Taxlotid)             
and               
DateDiff(D,AuecLocalDate,'1-1-1800') <> 0              
)              
--(              
--Select min(PT.AUECModifiedDate) as TradeDate                                                                                                         
--      from #PM_Taxlots PT  Where taxlot_PK in                                                                                                                 
--      (                                                                        
--        Select max(taxlot_PK) from #PM_Taxlots                                                                    
--        where Datediff(d,PT.AUECModifiedDate,@StartDate) >= 0                                                                                            
--        group by taxlotid                                                                                                                                         
--      )                                                                    
--      and TaxLotOpenQty<>0              
--)                                                                                                                                                                        
                           
If ( @MinTradeDate is not null And (DateDiff(d,@StartDate,@MinTradeDate)) > 0)                                                                                                                        
  Begin                                                                                                                                      
   Set @MinTradeDate = @StartDate                                                                                                                                                                                              
  End                                                
Else If (@MinTradeDate is null)                                                            
 Begin                                                            
  Set @MinTradeDate = @StartDate                                                             
 End                                                                               
                                                   
Set @MinTradeDate =  dbo.AdjustBusinessDays(@MinTradeDate,-1, @DefaultAUECID)                                     
                                                                                                                                        
Insert into #FXConversionRates Exec P_GetAllFXConversionRatesForGivenDateRange @MinTradeDate,@EndDate                                                                                
                                                                                                                          
 Update #FXConversionRates                                                                                                                                                        
 Set RateValue = 1.0/RateValue                                                                              
 Where RateValue <> 0 and ConversionMethod = 1                                                                              
                                                                                                                                                                                                                                   
                                                      
-- Create a Temp table and insert Data in that table and remove Union All                                                                                           
                            
Create Table #MTMDataTable                                                                                          
(                                                                                          
FundID int,                                                                                          
Symbol Varchar(100),                                                                                          
TaxLotOpenQty float,                                              
AvgPrice float,                                                                                          
ClosingPrice float,                                                                                          
AssetID int,                                                                                          
CurrencyID int,              
CurrencySymbol Varchar(50),                                                                                          
AUECID int,                                                                                          
TotalOpenCommission_Local Float,                                                                                               
TotalOpenCommission_Base float,                        
TotalClosedCommission_Local float,                                                                    
TotalClosedCommission_Base float,                                                                                                
AssetMultiplier float,                                           
TradeDate DateTime,                                                                                          
ClosingDate DateTime,                                                                                          
Mark1 float,                                                            
Mark2 float,                                                                                 
IsSwapped bit,                                                                                          
NotionalValue float,                                                                                         
BenchMarkRate float,                                                                                          
Differential float,                                         
OrigCostBasis float,                                                                                          
DayCount float,                                                                                          
FirstResetDate DateTime,                                                                                  
OrigTransDate DateTime,                                                                                          
SwapData float,                                                                                          
BeginningMarketValue_Local float,                         
BeginningMarketValue_Base float,                          
EndingMarketValue_Local float,                                           
EndingMarketValue_Base float,                                                                                                
Open_CloseTag Varchar(5),                                                                                   
ConversionRateTrade float,                                                                                          
ConversionRateStart float,                                                                               
ConversionRateEnd float,                                                                                          
CompanyName varchar(200),                                                                                                                                                       
FundName varchar(100),                                                                                          
StrategyName varchar(100),                                                                  
Side varchar(10),                                                            
Asset varchar(50),                          
UDAAsset varchar(100),                                                                                          
UDASecurityTypeName varchar(100),                                                   
UDASectorName varchar(100),                                                                                          
UDASubSectorName varchar(100),                                                                                          
UDACountryName varchar(100),                          
PutOrCall varchar(5),                                                              
MasterFundName varchar(100),                                                                                          
Dividend float,                                                                                    
MasterStrategyName varchar(100),                                                                                        
UnderlyingSymbol Varchar(100),                                                                
CashFXUnrealizedPNL float,                                    
NetTotalCost_Base float,                             
BloombergSymbol Varchar(100),                                          
SedolSymbol varchar(100),                                          
ISINSymbol varchar(100),                                          
CusipSymbol varchar(100),                                          
OSISymbol varchar(100),                                          
IDCOSymbol varchar(100),                      
UnrealizedPNLMTM_Local float,                      
RealizedPNLMTM_Local float,                      
UnrealizedPNLMTM_Base float,                      
RealizedPNLMTM_Base float  ,              
ExpirationDate datetime   ,              
 OpenTradeAttribute1 varchar(200),              
 OpenTradeAttribute2 varchar(200),              
 OpenTradeAttribute3 varchar(200),              
 OpenTradeAttribute4 varchar(200),              
 OpenTradeAttribute5 varchar(200),              
 OpenTradeAttribute6 varchar(200),                                                                                             
 ClosedTradeAttribute1 varchar(200),              
 ClosedTradeAttribute2 varchar(200),              
 ClosedTradeAttribute3 varchar(200),              
 ClosedTradeAttribute4 varchar(200),              
 ClosedTradeAttribute5 varchar(200),              
 ClosedTradeAttribute6 varchar(200)              
)                                                                                                      
                                                                                
 -- Insert statements for procedure here                                                                                                                            
  INSERT INTO #MarkPriceForStartDate Exec P_GetMarkPriceForBusinessDay @StartDate                                                                                          
 --Select * From dbo.GetMarkPriceForBusinessDay(@StartDate)                                        
  Declare @MarkEndDate DateTime                                                                             
  Set @MarkEndDate=DateAdd(d,1,@EndDate)                                                                                     
                                               
  INSERT INTO #MarkPriceForEndDate Exec P_GetMarkPriceForBusinessDay @MarkEndDate                                        
 --Select * From dbo.GetMarkPriceForBusinessDay(DateAdd(d,1,@EndDate))                                        
                                         
 -- Yesterday business date                                                                                                                                                                
   INSERT INTO #AUECYesterDates                                                                            
     Select Distinct V_SymbolAUEC.AUECID, dbo.AdjustBusinessDays(@StartDate,-1, V_SymbolAUEC.AUECID)                                                       
     from V_SymbolAUEC                                                                           
                                                                                                         
   INSERT INTO #AUECBusinessDatesForEndDate                                                                                                                                       
     Select Distinct V_SymbolAUEC.AUECID, dbo.AdjustBusinessDays(DateAdd(d,1,@EndDate),-1, V_SymbolAUEC.AUECID)                                                         
     from V_SymbolAUEC                              
                                                                                           
-----------------------------------------------------------------------------------------------------------                                
CREATE TABLE #TempClosingData                                          
(                                          
PTTaxLot_PK bigint ,                                   
PTTaxLotID varchar(50) ,                                          
PTSymbol varchar(100) ,                                          
PTTaxLotOpenQty float ,                                          
PTAvgPrice float ,                                          
PTTimeOfSaveUTC datetime ,                                          
PTGroupID nvarchar(50) ,                                          
PTAUECModifiedDate datetime ,                                          
PTFundID int ,                                      
PTLevel2ID int ,                                          
PTOpenTotalCommissionandFees float ,                                          
PTClosedTotalCommissionandFees float ,                                          
PTPositionTag int ,                                          
PTOrderSideTagValue nchar(10) ,                                          
PTTaxLotClosingId_Fk uniqueidentifier ,                                                        
PT1TaxLot_PK bigint ,                                          
PT1TaxLotID varchar(50) ,                                          
PT1Symbol varchar(100) ,                                          
PT1TaxLotOpenQty float ,                                          
PT1AvgPrice float ,                                          
PT1TimeOfSaveUTC datetime ,                                     
PT1GroupID nvarchar(50) ,                                          
PT1AUECModifiedDate datetime ,                                          
PT1FundID int ,                                          
PT1Level2ID int ,                                          
PT1OpenTotalCommissionandFees float ,                                          
PT1ClosedTotalCommissionandFees float ,                                          
PT1PositionTag int ,                                          
PT1OrderSideTagValue nchar(10) ,                                          
PT1TaxLotClosingId_Fk uniqueidentifier ,                                                             
TaxLotClosingID uniqueidentifier,                                          
PositionalTaxlotId varchar(50),                                         
ClosingTaxlotId varchar(50),                                          
ClosedQty float,                                          
ClosingMode int ,                                          
TimeOfSaveUTC datetime,                                          
AUECLocalDate datetime,                                          
PositionSide nchar(10),                                
G1Symbol varchar(100),                                          
G1AssetID int,                                   
G1CurrencyID int,                                         
G1FXRate float,                                          
G1FXConversionMethodOperator varchar(3),                                          
G1AUECLocalDate datetime,                                          
G1OrderSideTagValue varchar(3),                                                         
G1AvgPrice float,                                           
G1CumQty float,                                  
G1GroupID varchar(50),                                  
G1IsSwapped bit,                 
G1AllocationDate datetime,                                  
G1ProcessDate datetime,                                  
G1OriginalPurchaseDate datetime,                                
GSymbol varchar(100),                                                  
GAUECID int,                                  
GCurrencyID int,                                  
GUnderlyingID int,                                  
GExchangeID int,                                          
GAUECLocalDate datetime,                                          
GAvgPrice float,                                          
GCumQty float,                                          
GFXRate float,                                          
GFXConversionMethodOperator varchar(3),                                          
GAssetID int,                                          
GOrderSideTagValue varchar(3),                                                          
GCounterPartyID int,                                          
GGroupID varchar(50),                                          
GIsswapped bit,                                  
GAllocationDate datetime,                                  
GProcessDate datetime,                                  
GOriginalPurchaseDate datetime,                                  
OpeningFXRate float,                                   
EndingFXRate float,                                 
TradeDateFXRate float,                                 
StartDateFXRate float,                                  
ClosingDateFXRate float,              
GTradeAttribute1 varchar(200),              
GTradeAttribute2 varchar(200),              
GTradeAttribute3 varchar(200),              
GTradeAttribute4 varchar(200),              
GTradeAttribute5 varchar(200),              
GTradeAttribute6 varchar(200),              
G1TradeAttribute1 varchar(200),              
G1TradeAttribute2 varchar(200),      G1TradeAttribute3 varchar(200),              
G1TradeAttribute4 varchar(200),                                                               
G1TradeAttribute5 varchar(200),                                                               
G1TradeAttribute6 varchar(200)                                
)                                           
                                                        
Insert Into #TempClosingData                                 
Select                                            
PT.[TaxLot_PK] As [PTTaxLot_PK],                                          
PT.[TaxLotID]  As [PTTaxLotID],                                          
PT.[Symbol]   As [PTSymbol],                                          
PT.[TaxLotOpenQty]  As [PTTaxLotOpenQty],                                          
PT.[AvgPrice]  As [PTAvgPrice],                                          
PT.[TimeOfSaveUTC]  As [PTTimeOfSaveUTC],                                          
PT.[GroupID]  As [PTGroupID],                                          
PT.[AUECModifiedDate]  As [PTAUECModifiedDate],                                          
PT.[FundID]  As [PTFundID],                                          
PT.[Level2ID]  As [PTLevel2ID],                                          
PT.[OpenTotalCommissionandFees]  As [PTOpenTotalCommissionandFees],                                          
PT.[ClosedTotalCommissionandFees]  As [PTClosedTotalCommissionandFees],                                          
PT.[PositionTag] As [PTPositionTag],                                          
PT.[OrderSideTagValue] As [PTOrderSideTagValue],                                          
PT.[TaxLotClosingId_Fk] As [PTTaxLotClosingId_Fk],                                                       
PT1.[TaxLot_PK] As [PT1TaxLot_PK],                                          
PT1.[TaxLotID] As [PT1TaxLotID],                                          
PT1.[Symbol] As [PT1Symbol],                                          
PT1.[TaxLotOpenQty] As [PT1TaxLotOpenQty],                                          
PT1.[AvgPrice] As [PT1AvgPrice],                                   
PT1.[TimeOfSaveUTC] As [PT1TimeOfSaveUTC],                                      
PT1.[GroupID] As [PT1GroupID],                                          
PT1.[AUECModifiedDate] As [PT1AUECModifiedDate],                
PT1.[FundID] As [PT1FundID],                                          
PT1.[Level2ID] As [PT1Level2ID],                                          
PT1.[OpenTotalCommissionandFees] As [PT1OpenTotalCommissionandFees],                                          
PT1.[ClosedTotalCommissionandFees] As [PT1ClosedTotalCommissionandFees],                                          
PT1.[PositionTag] As [PT1PositionTag],                                          
PT1.[OrderSideTagValue] As [PT1OrderSideTagValue],                                          
PT1.[TaxLotClosingId_Fk] As [PT1TaxLotClosingId_Fk] ,                                                              
PTC.[TaxLotClosingID] AS [TaxLotClosingID],                                          
PTC.[PositionalTaxlotId] AS [PositionalTaxlotId],                           
PTC.[ClosingTaxlotId] AS [ClosingTaxlotId],                                          
PTC.[ClosedQty] AS [ClosedQty],                                          
PTC.[ClosingMode] AS [ClosingMode],                                          
PTC.[TimeOfSaveUTC] AS [TimeOfSaveUTC],                                          
PTC.[AUECLocalDate] AS [AUECLocalDate],                                          
PTC.[PositionSide] As [PositionSide],                                
G1.Symbol As G1Symbol,                                          
G1.AssetID As   G1AssetID,                                   
G1.CurrencyID As G1CurrencyID,                                                       
G1.FXRate As    G1FXRate,                                                         
G1.FXConversionMethodOperator As G1FXConversionMethodOperato,                                          
G1.AUECLocalDate As G1AUECLocalDate,                                          
G1.OrderSideTagValue As G1OrderSideTagValue,                                
G1.AvgPrice As  G1AvgPrice,                                          
G1.CumQty As G1CumQty,                                  
G1.GroupID As G1GroupID,                                  
G1.IsSwapped As G1IsSwapped,                        
G1.AllocationDate as G1AllocationDate,                                  
G1.ProcessDate as G1ProcessDate,                                  
G1.OriginalPurchaseDate as G1OriginalPurchaseDate,                                
G.Symbol As GSymbol,                                              
G.AUECID As GAUECID,                                  
G.CurrencyID As GCurrencyID,                                    
G.UnderlyingID As GUnderlyingID,                                  
G.ExchangeID As GExchangeID,                                             
G.AUECLocalDate As GAUECLocalDate,                                     
G.AvgPrice As GAvgPrice,                                           
G.CumQty As GCumQty,                                          
G.FXRate As GFXRate,                                          
G.FXConversionMethodOperator As GFXConversionMethodOperator,                                          
G.AssetID As GAssetID,                                          
G.OrderSideTagValue As  G1OrderSideTagValue,                                                      
G.CounterPartyID As GCounterPartyID,                                          
G.GroupID as GGroupID,                                          
G.IsSwapped as GIsSwapped,                                  
G.AllocationDate as GAllocationDate,                                  
G.ProcessDate as GProcessDate,                                  
G.OriginalPurchaseDate as GOriginalPurchaseDate,                                  
                                
Case                                                                                                                                                 
 When Isnull(PT.FXRate,G.FXrate) > 0 And Isnull(PT.FXConversionMethodOperator,G.FXConversionMethodOperator)='M'                                                                                                                                               
  
    
      
        
         
               
              
Then Isnull(PT.FXRate,G.FXrate)                                                      
 When Isnull(PT.FXRate,G.FXrate) > 0 And Isnull(PT.FXConversionMethodOperator,G.FXConversionMethodOperator)='D'                                                                                                           
 Then 1/Isnull(PT.FXRate,G.FXrate)                                                      
 Else IsNull(FXDayRatesForTradeDate.RateValue,0)                                     
End as OpeningFXRate,                                   
                                
Case                                                               
 When Isnull(PT1.FXRate,G1.FXrate) > 0 And Isnull(PT1.FXConversionMethodOperator,G1.FXConversionMethodOperator)='M'                                                                                                                                            
  
    
      
        
         
            
             
 Then Isnull(PT1.FXRate,G1.FXrate)                                                      
 When Isnull(PT1.FXRate,G1.FXrate) > 0 And Isnull(PT1.FXConversionMethodOperator,G1.FXConversionMethodOperator)='D'                                                                                                 
 Then 1/Isnull(PT1.FXRate,G1.FXrate)                                            
 Else IsNull(FXDayRatesForClosingDate.RateValue,0)                                             
End as EndingFXRate,                                                      
                                   
IsNull(FXDayRatesForTradeDate.RateValue,0) as TradeDateFXRate,                                                                                                                     
IsNull(FXDayRatesForStartDate.RateValue,0) as StartDateFXRate,                                                                      
IsNull(FXDayRatesForClosingDate.RateValue,0) as ClosingDateFXRate ,              
G.TradeAttribute1 as GTradeAttribute1,              
G.TradeAttribute2 as GTradeAttribute2,              
G.TradeAttribute3 as GTradeAttribute3,              
G.TradeAttribute4 as GTradeAttribute4,              
G.TradeAttribute5 as GTradeAttribute5,              
G.TradeAttribute6 as GTradeAttribute6,              
G1.TradeAttribute1 as G1TradeAttribute1,              
G1.TradeAttribute2 as G1TradeAttribute2,              
G1.TradeAttribute3 as G1TradeAttribute3,              
G1.TradeAttribute4 as G1TradeAttribute4,              
G1.TradeAttribute5 as G1TradeAttribute5,              
G1.TradeAttribute6 as G1TradeAttribute6                                            
                                          
from PM_TaxlotClosing  PTC                                                                                           
Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                        
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                                       
Inner Join T_Group G on G.GroupID = PT.GroupID                                                           
Inner Join T_Group G1 on G1.GroupID = PT1.GroupID                                 
--get yesterday business day                                                                                                                                
LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON G.AUECID = AUECYesterDates.AUECID                                   
                                
-- Forex Price for Trade Date                                                          
Left outer  join #FXConversionRates FXDayRatesForTradeDate                                                                                                                                                                             
on (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID                           
And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                               
And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDate.Date)=0)                                              
                         
-- Forex Price for Start Date                                                                                                                                         
Left outer  join #FXConversionRates FXDayRatesForStartDate                                                                                                                                                                       
on (FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID                               
And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                                                                                       
And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDate.Date)=0)                                              
                                                                                                     
-- Forex Price for Closing Date                                                                         
Left outer  join #FXConversionRates FXDayRatesForClosingDate                                                                                                                 
on (FXDayRatesForClosingDate.FromCurrencyID = G.CurrencyID                                                                                     
And FXDayRatesForClosingDate.ToCurrencyID = @BaseCurrencyID                                                                                                                    
And DateDiff(d,G1.ProcessDate,FXDayRatesForClosingDate.Date)=0)                                   
                                  
where DateDiff(d,@StartDate,PTC.AUECLocalDate) >=0                                                                                           
and  DateDiff(d,PTC.AUECLocalDate,@EndDate)>=0                       
                                 
-----------------------------------------------------------------------------------------------------------                                 
                                                                                            
                                                                                                         
Insert into #MTMDataTable                                                                                   
                                 
/**************************************************************************                                            
       OPEN POSITIONS HANDLING                                            
**************************************************************************/                                                                                                
 Select                                                                                                                            
  PT.FundID As FundID,                       
     
        
          
            
              
                
  PT.Symbol As Symbol,                               
  PT.TaxLotOpenQty As TaxLotOpenQty ,                                                                                                                                           
 PT.AvgPrice As AvgPrice ,                                                                                                              
  0 as ClosingPrice,                                                                                                                  
  G.AssetID As AssetID,                                                                                                                                                                                                                                       
  
     
      
        
          
           
  G.CurrencyID AS CurrencyID,                
  C.CurrencySymbol as CurrencySymbol,                                                                                                                     G.AUECID As AUECID ,                                                                                 
  
    
                                                                                    
                                                                         
IsNull(PT.OpenTotalCommissionAndFees,0) As TotalOpenCommission_Local,                          
                                                           
 Case                                                                                                                                                                                                        
  When G.CurrencyID =  @BaseCurrencyID  --When Company and Traded Currency both are same                                    
  Then IsNull(PT.OpenTotalCommissionAndFees,0)                                                                                                                                                             
  Else  --When Company and Traded Currency are different                                                                                                                                            
   Case                                                                                                    
    When Isnull(PT.FXRate,G.FXRate) > 0 And IsNull(PT.FXConversionMethodOperator,G.FXConversionMethodOperator)='M'                                                                                                                         
    Then IsNull(PT.OpenTotalCommissionAndFees * Isnull(PT.FXRate,G.FXRate),0)                                             
    When Isnull(PT.FXRate,G.FXRate) > 0 And IsNull(PT.FXConversionMethodOperator,G.FXConversionMethodOperator)='D'                                                                                                                                             
  
   
      
        
          
           
              
                
                  
                    
                       
    Then IsNull(PT.OpenTotalCommissionAndFees * 1/Isnull(PT.FXRate,G.FXRate),0)                                                                                                                                                                  
    Else  IsNull(PT.OpenTotalCommissionAndFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                                                                              
   End                               
End as TotalOpenCommission_Base,                                                                                                                                              
                                                                                                                       
0 as TotalClosedCommission_Local,                                                                                                                             
0 as TotalClosedCommission_Base,                                                                                          
SM.Multiplier as AssetMultiplier,                                                                                                                                                  
G.ProcessDate   as TradeDate,                                                                                                                                                                                                                          
null as ClosingDate,                                                                                                                          
IsNull(MPS.Finalmarkprice,0) As Mark1,                                                                                                                                 
IsNull(MPE.FinalMarkPrice,0) As Mark2,                                                                                                                                                                                                                         
 
     
      
       
           
            
              
       
                 
                    
                      
                           
G.IsSwapped as IsSwapped,                                                                                                  
Isnull( (PT.TaxLotOpenQty * SW.NotionalValue / Case When G.CumQty=0 Then 1 Else G.CumQty End) ,0) as NotionalValue,                                                                                                                                            
  
    
      
        
         
Isnull(SW.BenchMarkRate,0) as BenchMarkRate,                                                                                                               
Isnull(SW.Differential,0) as Differential,                                                                                                                                                                                      
Isnull(SW.OrigCostBasis,0) as OrigCostBasis,                                                                                                                                                                                        
Isnull(SW.DayCount,0) as DayCount,                                                                                   
Isnull(SW.FirstResetDate,'') as FirstResetDate,                                                                                                                                                  
Isnull(SW.OrigTransDate,'') as OrigTransDate,                                                                                                                                                                                                                  
  
    
      
        
          
           
              
                 
                  
                      
                         
--Case G.IsSwapped                                                                                                                                                                                                                                       
-- When 1                                                   
-- Then                                                                                                                                                                                        
--  Case                                                                                                                           
--   When  DateDiff(d,@StartDate,SW.OrigTransDate) >=0                                                                           
--   Then                                             
--  case                                             
--     when G.CurrencyID = @BaseCurrencyID                           
--     then (((isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty) ,0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,SW.OrigTransDate,@EndDate))/100)/SW.DayCount)* dbo.GetSideMultiplier(PT.OrderSideTagValue)                  
  
    
      
       
           
            
              
               
                  
                     
                     
--     else (((isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty) ,0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,SW.OrigTransDate,@EndDate))/100)/SW.DayCount)* dbo.GetSideMultiplier(PT.OrderSideTagValue)                  
  
    
      
        
          
            
              
                
                  
                    
                     
--*  Isnull(FXDayRatesForEndDate.RateValue,0)                                                                
--    end                                            
--   Else                                             
--    case                                             
--     when G.CurrencyID = @BaseCurrencyID                                            
--     then (((isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty) ,0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,@StartDate,@EndDate))/100)/SW.DayCount ) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                      
  
    
      
        
          
           
              
                
                  
                    
                      
--     else (((isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty) ,0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,@StartDate,@EndDate))/100)/SW.DayCount ) * dbo.GetSideMultiplier(PT.OrderSideTagValue)                      
  
    
      
        
          
            
              
                
                 
--*  Isnull(FXDayRatesForEndDate.RateValue,0)                                                                
--    end                                            
--  End                               
-- Else 0                                                                                                               
--End as SwapData,                              
                                 
Case G.IsSwapped                                                                                                                                                                                                                                               
 When 1                                                                                                                                           
 Then                                               
  Case                                                                                                             
   When G.CurrencyID = @BaseCurrencyID                                                                           
   Then (((isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty) ,0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,SW.OrigTransDate,@EndDate))/100)/SW.DayCount)* dbo.GetSideMultiplier(PT.OrderSideTagValue)                      
  
    
     
         
          
           
              
                 
                  
                    
                      
   Else (((isnull((PT.TaxLotOpenQty * SW.NotionalValue / G.CumQty) ,0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,SW.OrigTransDate,@EndDate))/100)/SW.DayCount)* dbo.GetSideMultiplier(PT.OrderSideTagValue)                      
  
    
      
       
* IsNull(FXDayRatesForEndDate.RateValue,0)                                                                            
  End                                                                                                                                                                                 
 Else 0                                                                                                 
End as SwapData,                                         
                                                                    
Case                                                                                                             
 When DateDiff(d,@StartDate,G.ProcessDate) >=0            
 Then                                                      
  Case                                                 
   When DateDiff(day,SplitTab.EffectiveDate,G.ProcessDate)=0                                                      
   Then IsNull(G.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                           
   Else IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                       
  End                                                      
 Else IsNull((IsNull(MPS.FinalMarkPrice,0) / IsNull(SplitTab.SplitFactor,1))* TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                         
 
     
      
       
           
            
              
               
                   
                    
                     
End As BeginningMarketValue_Local,                                              
                                                                                                                                                  
--Market Value in Base on Start Date                                                                       
Case                     
 When G.CurrencyID =  @BaseCurrencyID                                                 
 Then                                          
  Case                                                                                                       
   When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                 
   Then                                                
    Case                                                 
     When DateDiff(day,SplitTab.EffectiveDate,G.ProcessDate)=0                                                
     Then IsNull(G.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                     
     Else IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                      
  
   
       
        
         
             
    End                                                
   Else IsNull((IsNull(MPS.FinalMarkPrice,0) / IsNull(SplitTab.SplitFactor,1))* TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                 
    
      
        
          
            
              
                
  End                                                  
 Else                                                                                                                                                                                                       
 Case                                                                                                        
  When DateDiff(d,@StartDate,G.ProcessDate) >=0                                                                                                                                                                                           
  Then                                                 
   Case                                                 
    When DateDiff(day,SplitTab.EffectiveDate,G.ProcessDate)=0                                                
    Then                                                
     Case                                                                                                                                                
      When Isnull(PT.FXRate,G.FXRate) > 0 And IsNull(PT.FXConversionMethodOperator,G.FXConversionMethodOperator)='M'                                                                                                                                          
      Then IsNull(G.AvgPrice * Isnull(PT.FXRate,G.FXRate) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                       
      When Isnull(PT.FXRate,G.FXRate) > 0 And IsNull(PT.FXConversionMethodOperator,G.FXConversionMethodOperator)='D'                                                                                                                                          
   
    
      
       
           
            
              
               
      Then IsNull((G.AvgPrice * 1/Isnull(PT.FXRate,G.FXRate)) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                       
  
    
      
        
          
            
              
               
      ELSE IsNull(G.AvgPrice * IsNull(FXDayRatesForTradeDate.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                           
  
    
      
     End                                                     
    Else                                                                                          
     Case                                                                                                                                                                              
      When Isnull(PT.FXRate,G.FXRate) > 0 And IsNull(PT.FXConversionMethodOperator,G.FXConversionMethodOperator)='M'                                                                                                                                           
  
    
      
        
          
            
              
                
      Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * Isnull(PT.FXRate,G.FXRate) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                        
  
    
      
        
          
            
              
                
      When Isnull(PT.FXRate,G.FXRate) > 0 And IsNull(PT.FXConversionMethodOperator,G.FXConversionMethodOperator)='D'                                                                                                                                           
  
    
      
        
          
            
              
                
      Then IsNull(((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * 1/Isnull(PT.FXRate,G.FXRate)) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                              
      Else IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * IsNull(FXDayRatesForTradeDate.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                        
  
    
      
        
          
            
              
                
     End                                                 
   End                                  
  Else IsNull((IsNull(MPS.FinalMarkPrice,0) / IsNull(SplitTab.SplitFactor,1)) * FXDayRatesForStartDate.RateValue * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                    
  
    
      
        
          
            
              
                
 End                                                                                                                                                     
End As BeginningMarketValue_Base,                        
                        
IsNull(IsNull(MPE.Finalmarkprice,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0) As EndingMarketValue_Local ,                                     
                                                                                                                                    
Case                                          
   When G.CurrencyID =  @BaseCurrencyID                                                                                                                                                                                                                        
 
     
      
        
          
            
              
                
   Then IsNull(IsNull(MPE.Finalmarkprice,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                         
  
    
      
        
          
            
              
                
                  
                    
                      
 Else IsNull(IsNull(MPE.Finalmarkprice,0) * IsNull(FXDayRatesForEndDate.RateValue,0) * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                           
End As EndingMarketValue_Base ,                                                                                                                                                    
                                                                                                                
--Case                                             
--When G.AssetID=3 then 'I'                                                                                                                                                    
-- Else 'O'                                 
--End as Open_CloseTag,                     
'O' As Open_CloseTag,                                    
                                                                                                   
Case                                                                     
 When Isnull(PT.FXRate,G.FXRate) > 0 And IsNull(PT.FXConversionMethodOperator,G.FXConversionMethodOperator)='M'                                                      
 Then Isnull(PT.FXRate,G.FXRate)                                                      
 When Isnull(PT.FXRate,G.FXRate) > 0 And IsNull(PT.FXConversionMethodOperator,G.FXConversionMethodOperator)='D'                     
 Then 1 / Isnull(PT.FXRate,G.FXRate)                                               
Else IsNull(FXDayRatesForTradeDate.RateValue,0)                                                                                                                                                   
End As ConversionRateTrade,                                        
                        
IsNull(FXDayRatesForStartDate.RateValue,0) as ConversionRateStart,                                                                                                                                                            
                         
Case                                                                                                      
 When G.CurrencyID = @BaseCurrencyID                        
 Then 1                        
 Else IsNull(FXDayRatesForEndDate.RateValue,0)                         
End as ConversionRateEnd,                                                                              
                                                                                                              
SM.CompanyName,                                                                                                                                                                                                                                                
  
   
       
        
         
             
              
#T_CompanyFunds.FundName as FundName,                                                 
IsNull(CompanyStrategy.StrategyName,'Strategy Unallocated') AS StrategyName,                                                               
                
                 
                     
                     
Case dbo.GetSideMultiplier(PT.OrderSideTagValue)                                     
  When  1                                                                                                                                             
  Then  'Long'                                                                                                                                            
  Else  'Short'                                                                                                                                                                           
End as Side,                                                                              
#T_Asset.AssetName as Asset,                                                                  
IsNull(SM.AssetName,'Undefined') as UDAAsset,                                                             
IsNull(SM.SecurityTypeName,'Undefined') as UDASecurityTypeName,                                                 
IsNUll(SM.SectorName,'Undefined') as UDASectorName,                                                                                                                         
IsNull(SM.SubSectorName,'Undefined') as UDASubSectorName,                                                                                                                                                  
IsNull(SM.CountryName,'Undefined') as UDACountryName ,                                                                                  
IsNull(SM.PutOrCall,'') as PutOrCall,                                           
IsNull(CMF.MasterFundName,'Unassigned') as MasterFundName,                                                      
0 as Dividend,                                                                                              
IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                                                                                        
IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,                                                                
0 as CashFXUnrealizedPNL,                      
                     
                
                
                
                               
Case                                              
-- When Trade date FX Rate is used                
 When                 
(                
(G.AssetID=1 And G.IsSwapped = 1 and @IncludeFXPNLinSwaps = 1) Or                 
(G.AssetID=5 and @IncludeFXPNLinFX = 1) OR                 
(G.AssetID=11 and @IncludeFXPNLinFX = 1) or                 
(G.AssetID = 3  and @FuturePNLWithBothOrEndFXRate = 1 ) or                
(G.AssetID = 4 and @BaseCurrencyID <> G.CurrencyID and @InternationalFutureOptionPNLWithBothOrEndFXRate=1)                                      
)                                       
 Then                               
  Case                                                                          
   When G.CurrencyID = @BaseCurrencyID                             
   Then IsNull(PT.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)  + Isnull(PT.OpenTotalCommissionAndFees,0)                                                                                              
  
    
      
        
           
           
               
                                                    
   Else                                                                     
    Case                                                                                                                 
     When  Isnull(PT.FXRate,G.FXRate) > 0 And Isnull(PT.FXConversionMethodOperator,G.FXConversionMethodOperator)='M'                                                                                                                                           
  
    
     Then (IsNull(PT.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0) + Isnull(PT.OpenTotalCommissionAndFees,0)) *  Isnull(PT.FXRate,G.FXRate)                                          
                                                                     
     When Isnull(PT.FXRate,G.FXRate) > 0 And Isnull(PT.FXConversionMethodOperator,G.FXConversionMethodOperator)='D'                                              
     Then (IsNull(PT.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0) + Isnull(PT.OpenTotalCommissionAndFees,0)) * 1/Isnull(PT.FXRate,G.FXRate)                                                             
 
     
      
        
          
           
               
                                                                         
     Else (IsNull(PT.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0) + Isnull(PT.OpenTotalCommissionAndFees,0)) * IsNull(FXDayRatesForTradeDate.RateValue,0)                            
            
              
                                                       
    End                                                                                                                                                                                                              
  End                   
-- When No FX Rate is used                          
 When                 
(                
(G.AssetID=1 And G.IsSwapped = 1 and @IncludeFXPNLinSwaps = 0) Or                 
(G.AssetID=5 and @IncludeFXPNLinFX = 0) OR                 
(G.AssetID=11 and @IncludeFXPNLinFX = 0) or                 
(G.AssetID = 3  and @FuturePNLWithBothOrEndFXRate = 0 ) or                
(G.AssetID = 4 and @BaseCurrencyID <> G.CurrencyID and @InternationalFutureOptionPNLWithBothOrEndFXRate = 0)                                      
)                
 Then                                                                                              
  Case                                                                  
   When G.CurrencyID = @BaseCurrencyID                                                                                                                                                                        
 Then IsNull(PT.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0) + Isnull(PT.OpenTotalCommissionAndFees,0)                
 Else (IsNull(PT.AvgPrice * TaxlotOpenQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0) + Isnull(PT.OpenTotalCommissionAndFees,0)) * IsNull(FXDayRatesForEndDate.RateValue,0)                
                 
  End                    
                  
                                                                                   
 Else 0                 
End As NetTotalCost_Base,                      
                  
               
                                          
SM.BloombergSymbol,                                          
SM.SedolSymbol,                                          
SM.ISINSymbol,                                          
SM.CusipSymbol,                                          
SM.OSISymbol,                                          
SM.IDCOSymbol,                      
0 As UnrealizedPNLMTM_Local,                      
0 As RealizedPNLMTM_Local,          
0 As UnrealizedPNLMTM_Base,                      
0 As RealizedPNLMTM_Base   ,              
 CAST(FLOOR(CAST(SM.ExpirationDate AS FLOAT ) ) AS DATETIME ) as ExpirationDate,              
G.TradeAttribute1 as OpenTradeAttribute1,              
G.TradeAttribute2 as OpenTradeAttribute2,              
G.TradeAttribute3 as OpenTradeAttribute3,              
G.TradeAttribute4 as OpenTradeAttribute4,              
G.TradeAttribute5 as OpenTradeAttribute5,              
G.TradeAttribute6 as OpenTradeAttribute6,              
'' as ClosedTradeAttribute1 ,              
'' as ClosedTradeAttribute2 ,              
'' as ClosedTradeAttribute3 ,              
'' as ClosedTradeAttribute4 ,              
'' as ClosedTradeAttribute5 ,              
'' as ClosedTradeAttribute6               
                                                                                               
                                   
 From #PM_Taxlots PT                                                                                                                                                                                     
  Inner Join #T_CompanyFunds ON  PT.FundID= #T_CompanyFunds.CompanyFundID                                         
  Inner Join T_Group G on G.GroupID = PT.GroupID                       
  Inner Join #T_Asset On #T_Asset.AssetId=G.AssetID                                     
  Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=PT.Symbol                                                                          
  Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=PT.Symbol                                                                                                                                                            
  Left outer join  T_SwapParameters SW on G.GroupID=SW.GroupID                                                          
  Left OUTER JOIN T_Currency C on G.CurrencyID = C.CurrencyID              
  -- join to get yesterday business day                                                                                        
  LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON G.AUECID = AUECYesterDates.AUECID                                                                                                                                                                
  LEFT OUTER JOIN #AUECBusinessDatesForEndDate AUECBusinessDatesForEndDate ON G.AUECID = AUECBusinessDatesForEndDate.AUECID                                  
  -- Forex Price for Trade Date                                                                                                                                 
  Left outer join #FXConversionRates FXDayRatesForTradeDate                                                                                                                                                               
 on (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID                                                                                                    
 And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                                                                                                                               
 And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDate.Date)=0)                                                                 
                                                                                                                                                            
  -- Forex Price for Start Date                                                                              
 Left outer join #FXConversionRates FXDayRatesForStartDate                                                                        
 on (FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID                                                                                 
 And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                                                                                                                            
 And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDate.Date)=0)                                                                                                                                    
                                                                                                                                               
-- Forex Price for End Date                                                                                                                                             
  Left outer join #FXConversionRates FXDayRatesForEndDate                                                                                                                                                              
 on (FXDayRatesForEndDate.FromCurrencyID = G.CurrencyID                                      
 And FXDayRatesForEndDate.ToCurrencyID = @BaseCurrencyID                               
 And DateDiff(d,AUECBusinessDatesForEndDate.YESTERDAYBIZDATE,FXDayRatesForEndDate.Date)=0)                                                                                                                                                                     
 
    
       
                                                                                                                                                                                       
-- Security Master DB Join                                                                      
  Left outer join #SecMasterDataTempTable SM ON SM.TickerSymbol = PT.Symbol                                   
  Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=PT.Level2ID                                                                                                              
  LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                                                    
  
    
      
  LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                                         
  LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                              
  LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                                                                                                                                 
  
   
       
  Left Outer Join #TempSplitFactorForOpen SplitTab on SplitTab.TaxlotID=PT.TaxlotID                        
  where TaxLotOpenQty<>0  -- and   SSymbol.FutSymbol is null                                                                                                                                                        
  and taxlot_PK in                                                                                                                                                                                
  (                                   
    Select max(Taxlot_PK) from #PM_Taxlots                                                                                                                                        
    Where DateDiff(d,#PM_Taxlots.AUECModifiedDate,@EndDate) >=0                                                
    group by TaxlotId                                                                                                                                                                                     
  )                                 
                                                                                                                                                                                                                      
                                                                                                
/**************************************************************************                                            
       CLOSED POSITIONS HANDLING                                            
**************************************************************************/                                                                                                         
Insert into #MTMDataTable                                                          
                                                                                                                                                           
 select                                                                                                                            
  PTFundID    as FundID,                                                                                                                                               
  PTSymbol    as Symbol,                                                                                       
  ClosedQty   as ClosedQty ,                                                                                                                        
  PTAvgPrice    as AvgPrice ,            
  IsNull(PT1AvgPrice,0)as ClosingPrice ,                                        
  GAssetID    as AssetID,                                                                                                     
  GCurrencyID   as CurrencyID,                
  C.CurrencySymbol as CurrencySymbol,                                                                                                                                                    
  GAUECID    as AUECID,                                             
                                                                                                
IsNull(PTClosedTotalCommissionandFees,0) As  TotalOpenCommission_Local,                        
                                
   Case                                                                                                                                                                                               
 When GCurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                                                                               
        
          
             
              
               
                  
                    
                      
 Then IsNull(PTClosedTotalCommissionandFees,0)                                                                                                                                                           
 Else  ----When Company and Traded Currency are different                                                                                                                   
--  Case                                           
--   When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                        
--   Then IsNull(PTClosedTotalCommissionandFees * G.FXRate,0)                                                                                                                                        
              
                
                  
                    
                      
--   When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                          
--   Then IsNull(PTClosedTotalCommissionandFees * 1/G.FXRate,0)                                                                                                                                                                                               
  
    
      
         
         
             
              
                
                  
                    
                      
--   When G.FXRate <= 0 OR G.FXRate is null                                                                                                                                                                                                                    
  
    
      
--   Then  IsNull(PTClosedTotalCommissionandFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                                                                                                                                               
  
    
     
--  End                                  
ISnull(PTClosedTotalCommissionandFees * OpeningFXRate,0)                                       
End as TotalOpenCommission_Base,                                          
                                    
IsNull(PT1ClosedTotalCommissionandFees,0) as TotalClosedCommission_Local,                                                                                                                                                      
      
        
          
            
              
                
                  
                    
                                                                                                         
                                                                                                                                         
Case                                                                                                                                  
 When GCurrencyID =  @BaseCurrencyID --When Company and Traded Currency both are same                                                                               
 Then IsNull(PT1ClosedTotalCommissionandFees,0)                                                                                                                                                               
  Else  --When Company and Traded Currency are different                                                                                                                 
--  Case                                                             
--   When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                                                                                                                                               
--   Then IsNull(PT1.ClosedTotalCommissionandFees * G1.FXRate,0)                                                                                                                                                                                               
 
     
      
       
           
            
          
                
                  
                   
                       
--   When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                                                                                                                                                                                        
--   Then IsNull(PT1.ClosedTotalCommissionandFees * 1/G1.FXRate,0)                                                                                                                                                          
--  When G1.FXRate <= 0 OR G1.FXRate is null                                                                        
--   Then  IsNull(PT1.ClosedTotalCommissionandFees * IsNull(FXDayRatesForClosingDate.RateValue,0),0)                                                                                                    
--  End              
IsNull(PT1ClosedTotalCommissionandFees * EndingFXRate,0)                                                                                                                      
End as TotalClosedCommission_Base,                                                    
                                                                                                                           
SM.Multiplier As AssetMultiplier,                                                                                                                   
GProcessDate As TradeDate,                                                                                                                                                                                       
AUECLocalDate  as ClosingDate, --now closing taxlot Trade date is cloisng date                                                                                                                                                                                
   
IsNull(MPS.FinalMarkPrice,0) Mark1,                              
IsNull(MPE.FinalMarkPrice,0) Mark2,                                      
GIsSwapped,                                                                                                                                                                                      
isnull(SW.NotionalValue*((PT1TaxLotOpenQty+ClosedQty)/Case When G1CumQty=0 Then 1 Else G1CumQty End),0) as NotionalValue,                                  
                                      
                                        
isnull(SW.BenchMarkRate,0) as BenchMarkRate,                                                                                                                                      
isnull(SW.Differential,0) as Differential,                                                                                                             
isnull(SW.OrigCostBasis,0) as OrigCostBasis,                                                                                                                                                                                        
isnull(SW.DayCount,0) as DayCount,                                                             
SW1.FirstResetDate as FirstResetDate,                                                                                                                                                         
SW1.OrigTransDate as OrigTransDate ,                                                                   
                                                                               
--Case GIsSwapped                                                                      
-- When 1                                                                                                                                                                                     
-- Then                                                                                                               
--  Case                    
--   When DateDiff(d,SW.OrigTransDate,@StartDate) >=0                                                                                                                                         
--   Then                                             
--    case                                             
--     when GCurrencyID = @BaseCurrencyID                                            
--     then (((isnull(SW.NotionalValue*((PTTaxLotOpenQty+ClosedQty)/Case When GCumQty=0 Then 1 Else GCumQty End),0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,SW.OrigTransDate,SW1.OrigTransDate))/100)/SW.DayCount)             
  
   
      
        
          
            
               
           
                 
                     
                     
--*  dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                         
--     else (((isnull(SW.NotionalValue*((PTTaxLotOpenQty+ClosedQty)/Case When GCumQty=0 Then 1 Else GCumQty End),0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,SW.OrigTransDate,SW1.OrigTransDate))/100)/SW.DayCount)             
  
    
      
        
          
            
              
                
                  
                    
                     
--*  dbo.GetSideMultiplier(PTOrderSideTagValue) * ClosingDateFXRate                                                                                     
--    end                                            
--   Else                                             
--    case                                             
--     when GCurrencyID = @BaseCurrencyID                                            
--     then (((isnull(SW.NotionalValue*((PTTaxLotOpenQty + ClosedQty)/GCumQty),0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,SW.OrigTransDate,SW1.OrigTransDate))/100)/SW.DayCount ) *  dbo.GetSideMultiplier(PTOrderSideTagValue)
  
    
      
        
          
            
              
                
                  
                    
                      
--     else (((isnull(SW.NotionalValue*((PTTaxLotOpenQty + ClosedQty)/GCumQty),0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,SW.OrigTransDate,SW1.OrigTransDate))/100)/SW.DayCount )                                       
--*  dbo.GetSideMultiplier(PTOrderSideTagValue) * ClosingDateFXRate                                                                                     
-- end                                            
--  End                                           
-- Else 0                                                                                                                                                                         
--End as SwapData,                               
                              
Case GIsSwapped                                                                                                
 When 1                                                                                                                                                                       
 Then                                                                                       
  Case                                                                                                                                                             
  When GCurrencyID = @BaseCurrencyID                                                       
  Then (((isnull(SW.NotionalValue * ((PTTaxLotOpenQty + ClosedQty)/Case When GCumQty=0 Then 1 Else GCumQty End),0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,SW.OrigTransDate,AUECLocalDate))/100)/SW.DayCount)                  
 
     
      
* dbo.GetSideMultiplier(PTOrderSideTagValue) * ClosingDateFXRate                                                        
  Else (((isnull(SW.NotionalValue * ((PTTaxLotOpenQty + ClosedQty)/Case When GCumQty=0 Then 1 Else GCumQty End),0) * (isnull(SW.BenchMarkRate,0) + isnull(SW.Differential,0)) * DateDiff(d,SW.OrigTransDate,AUECLocalDate))/100)/SW.DayCount)                  
  
    
      
* dbo.GetSideMultiplier(PTOrderSideTagValue)                                                          
  End                                                                                                                            
 Else 0                                         
End as SwapData,                                        
                                                                                                                                                                    
Case                                                                                                   
 When DateDiff(d,GProcessDate,@StartDate) > 0                                                           
 Then (IsNull(MPS.FinalMarkPrice,0) / IsNull(SplitTab.SplitFactor,1)) * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                                     
 Else (GAvgPrice / IsNull(SplitTab.SplitFactor,1)) * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                                                         
   
   
       
        
          
           
               
                
End as BeginningMarketValue_Local ,                   
                                                                                                           
Case                                                            
 When GCurrencyID =  @BaseCurrencyID                                                                                                    
  Then                                                                                             
   Case                                                                                                                                                                                         
   When DateDiff(d,GProcessDate,@StartDate) > 0                                                                   
   Then (IsNull(MPS.FinalMarkPrice,0) / IsNull(SplitTab.SplitFactor,1)) * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                                     
  
    
      
        
   Else (GAvgPrice / IsNull(SplitTab.SplitFactor,1)) * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                                                        
  
    
      
        
         
             
              
               
                  
                    
                       
   End                                                                                                                          
  Else                                                                                                                                                   
   Case                        
   When  DateDiff(d,GProcessDate,@StartDate) > 0                                                                                                            
   Then (IsNull(MPS.FinalMarkPrice,0) / IsNull(SplitTab.SplitFactor,1)) * StartDateFXRate * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                   
  
    
      
        
          
    Else                                                       
--    Case                                                                                  
--     When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                                                                        
--     Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * G.FXRate * PTC.ClosedQty  * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue),0)                                                                                         
  
    
      
       
           
           
              
                 
                 
                     
                      
--     When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                           
--     Then IsNull(((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * 1/G.FXRate) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue),0)                                                                                      
  
    
      
        
          
            
              
                
                  
                    
                      
--     When G.FXRate <= 0 OR G.FXRate is null                                                                             
--     Then IsNull((G.AvgPrice / IsNull(SplitTab.SplitFactor,1)) * FXDayRatesForTradeDate.RateValue * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue),0)                                                                  
  
    
      
        
          
            
              
                
                  
                    
                      
--    End                                   
  IsNull((GAvgPrice / IsNull(SplitTab.SplitFactor,1)) * OpeningFXRate * ClosedQty  * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue),0)                                                                                                   
  
    
      
        
          
            
              
                
                  
                    
                           
 End                                          
End as BeginningMarketValue_Base ,                               
                                  
ISNULL(PT1AvgPrice,0)* ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(GOrderSideTagValue,G1OrderSideTagValue),1) as  EndingMarketValue_Local,                                                                                    
  
    
      
        
         
             
              
               
                   
                    
                                                                                                                 
 Case                                 
 When GCurrencyID <> @BaseCurrencyID                                                                       
  Then                        
--  Case                                                                                                                 
--   When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                                                                                 
--   Then IsNull(PT1.AvgPrice,0)* G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                                                    
 
     
      
        
          
            
              
                
                  
                    
                     
--   When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                                                                                                                           
--   Then IsNull(PT1.AvgPrice,0)* 1/G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                                                  
  
    
      
        
          
            
              
                
                 
                   
                      
--   Else IsNull(PT1.AvgPrice,0)* IsNull(FXDayRatesForClosingDate.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                 
  
    
      
        
          
            
              
                
                  
                    
                      
--  End                                 
 IsNull(PT1AvgPrice,0)* EndingFXRate * ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(GOrderSideTagValue,G1OrderSideTagValue),1)                                                                         
                  
                    
                          
 Else ISNULL(PT1AvgPrice,0)* ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(GOrderSideTagValue,G1OrderSideTagValue),1)                                                                                                           
  
    
      
        
          
            
             
                 
                  
                    
                      
End as  EndingMarketValue_Base,                                                                                             
                                                                                                                                                                                 
'C' as Open_CloseTag,                                      
                                                                                                                                                   
--Case                                                                                                                                                 
-- When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                                                                 
-- Then G.FXRate                                                      
-- When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                           
-- Then 1/G.FXRate                                                      
-- When G.FXRate <= 0 OR G.FXRate is null                                                                      
-- Then IsNull(FXDayRatesForTradeDate.RateValue,0)                              
--End                                 
OpeningFXRate as ConversionRateTrade,                                                       
                                                                                
--IsNull(FXDayRatesForStartDate.RateValue,0) as ConversionRateStart,                                                                      
--IsNull(FXDayRatesForClosingDate.RateValue,0) as ConversionRateClosing,                                       
                                             
StartDateFXRate as ConversionRateStart,                                
                               
Case                                                                                                      
 When GCurrencyID = @BaseCurrencyID                        
 Then 1                        
 Else ClosingDateFXRate                      
End as ConversionRateClosing,                               
                                                                                                                                
  SM.CompanyName,                                                                                                                                                                                                                                              
 
     
     
         
          
            
  #T_CompanyFunds.FundName  as FundName,                                                                                                                                     
  IsNull(CompanyStrategy.StrategyName,'Strategy Unallocated') AS StrategyName,                                                                                                                                                                                 
  
    
      
        
          
            
              
                
                  
                   
                                                                          
  Case dbo.GetSideMultiplierForClosing(GOrderSideTagValue,G1OrderSideTagValue)                                                                       
   When  1                                                                                                                                                                                             
   Then 'Long'                                                                            
   When  -1                                                          
   Then  'Short'                                                          
   Else  ''        
  End as Side,                                                             
                                                                                                                                                                                                       
  #T_Asset.AssetName as Asset,                                                                                                                                                                     
  IsNull(SM.AssetName,'Undefined') as UDAAsset,                                                                                                                                                                                   
  IsNull(SM.SecurityTypeName,'Undefined') as UDASecurityTypeName,                                                                                                                                         
  IsNull(SM.SectorName,'Undefined') as UDASectorName,                                                                                               
  IsNull(SM.SubSectorName,'Undefined') as UDASubSectorName,                                                                                                                                         
  IsNull(SM.CountryName,'Undefined') as UDACountryName,                                                          
  IsNUll(SM.PutOrCall,'') as PutOrCall,                                                                                                                                      
  IsNull(CMF.MasterFundName,'Unassigned') As MasterFundName,                                                                                        
  0 as Dividend,                                                           
  IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                                                                                        
  IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,                                                       
  0 as CashFXUnrealizedPNL,                                    
  0 As NetTotalCost_Base,                                            
SM.BloombergSymbol,                                          
SM.SedolSymbol,                                          
SM.ISINSymbol,                                          
SM.CusipSymbol,                                          
SM.OSISymbol,                                          
SM.IDCOSymbol,                      
0 As UnrealizedPNLMTM_Local,                      
0 As RealizedPNLMTM_Local,                      
0 As UnrealizedPNLMTM_Base,                      
0 As RealizedPNLMTM_Base     ,              
 CAST(FLOOR(CAST(SM.ExpirationDate AS FLOAT ) ) AS DATETIME ) as ExpirationDate ,              
GTradeAttribute1 as OpenTradeAttribute1,              
GTradeAttribute2 as OpenTradeAttribute2,              
GTradeAttribute3 as OpenTradeAttribute3,              
GTradeAttribute4 as OpenTradeAttribute4,              
GTradeAttribute5 as OpenTradeAttribute5,              
GTradeAttribute6 as OpenTradeAttribute6,                 
G1TradeAttribute1 As ClosedTradeAttribute1 ,              
G1TradeAttribute2 As ClosedTradeAttribute2 ,              
G1TradeAttribute3 As ClosedTradeAttribute3 ,              
G1TradeAttribute4 As ClosedTradeAttribute4 ,              
G1TradeAttribute5 As ClosedTradeAttribute5 ,              
G1TradeAttribute6 As ClosedTradeAttribute6                                            
                                                                         
--  from PM_TaxlotClosing  PTC                                                                                    
--  Inner Join #PM_Taxlots PT on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                    
                 
                  
                   
                      
--  Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID  and  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                                                                 
--  Inner Join T_Group G on G.GroupID = PT.GroupID                              
--  Inner Join T_Group G1 on G1.GroupID = PT1.GroupID                                  
from #TempClosingData                                                  
  Inner Join #T_CompanyFunds ON  #TempClosingData.PTFundID= #T_CompanyFunds.CompanyFundID                                         
  Inner Join #T_Asset On #T_Asset.AssetId=#TempClosingData.GAssetID                                                                                                                                                                      
  --Inner Join T_AUEC AUEC on G.AUECID = AUEC.AUECID                                                                                                                                                                           
  Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=#TempClosingData.PTSymbol                                                                                              
  Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=#TempClosingData.PTSymbol                                                                                    
--  --get yesterday business day                                                                                                                                              
--  LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON #TempClosingData.GAUECID = AUECYesterDates.AUECID                                                                                                                    
  -- Security Master DB join                                                                                                                                                                
  LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = #TempClosingData.PTSymbol                     
      
        
           
           
              
                 
                 
                     
                      
                                  
--  -- Forex Price for Trade Date                                                                                                     
--  Left outer  join #FXConversionRates FXDayRatesForTradeDate                                                                                                                                                                             
--  on (FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID                                                                                        
--  And FXDayRatesForTradeDate.ToCurrencyID = @BaseCurrencyID                                                                                                           
--  And DateDiff(d,G.ProcessDate,FXDayRatesForTradeDate.Date)=0)                                              
--                                                                                                                                                                                           
--  -- Forex Price for Start Date                                                                                                                                         
--  Left outer  join #FXConversionRates FXDayRatesForStartDate                                                                                                                                                                 
--  on (FXDayRatesForStartDate.FromCurrencyID = G.CurrencyID                                                                                  
--  And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                                                
--  And DateDiff(d,AUECYesterDates.YESTERDAYBIZDATE,FXDayRatesForStartDate.Date)=0)                                              
--                                                                                                             
--  -- Forex Price for Closing Date                                             
--  Left outer  join #FXConversionRates FXDayRatesForClosingDate                                                                                                                         
--  on (FXDayRatesForClosingDate.FromCurrencyID = G.CurrencyID                           
--  And FXDayRatesForClosingDate.ToCurrencyID = @BaseCurrencyID                                 
--  And DateDiff(d,G1.ProcessDate,FXDayRatesForClosingDate.Date)=0)                                      
                                    
  Left Outer Join  T_SwapParameters SW on SW.GroupID=#TempClosingData.GGroupID                                                                                                                                                                                
  
    
       
       
          
             
              
   Left OUTER JOIN T_Currency C on GCurrencyID = C.CurrencyID              
              
                  
                    
                      
  Left Outer Join  T_SwapParameters SW1 on SW1.GroupID=#TempClosingData.G1GroupID                                                                                                                        
  Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=#TempClosingData.PTLevel2ID                                                                                                                                        
  
    
      
        
          
            
              
                
                  
                    
                      
  LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                                                    
  
    
      
        
          
            
              
                
  LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                                                 
  LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                                                        
  
    
      
        
          
            
              
                        
                    
                      
  LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                                                                                                                
  Left Outer Join #TempSplitFactorForClosed SplitTab on SplitTab.TaxlotID=#TempClosingData.PTTaxlotID                                 
  And DateDiff(day,#TempClosingData.PTAUECModifiedDate,SplitTab.Effectivedate) <= 0                                                                                                                                        
  Where ClosingMode <> 7 --and SSymbol.FutSymbol is null --7 means CoperateAction!                                                                                                                                          
                                                                                           
                                                           
                                                                                                                          
/**************************************************************************                                            
       DIVIDEND HANDLING                                       
**************************************************************************/                                                                                                          
Insert into #MTMDataTable                                                                                                       
 Select                                                                                                                                                                                            
  IsNull(CashDiv.FundId,0) as FundID,                      
   CashDiv.Symbol,                                                                                                                                           
   0  as TaxLotOpenQty ,                                               
   0  as AvgPrice ,                                                                                                                                                                                   
   0  as ClosingPrice,                                                                                               
   min(Isnull(SM.AssetID,0)) as AssetID,                                                   
   Min(CashDiv.CurrencyID) as CurrencyID,               
   MIN(C.CurrencySymbol) as CurrencySymbol,                                                                                       
   Min(Isnull(SM.AUECID,0)) as AUECID ,                                                                                                                                                                                                                    
   0  As TotalOpenCommission_Local,                                                                                                                                                                                                            
   0  as TotalOpenCommission_Base,                                                                                   
   0  as TotalClosedCommission_Local,                         
   0  as TotalClosedCommission_Base,                                                                                                            
   0  as AssetMultiplier,                     
   min(CashDiv.ExDate) as TradeDate,                                        
   '1800-01-01 00:00:00.000' as ClosingDate,                                                                                                                  
   0  as Mark1,                                                                                                                                                                                                                                               
  
    
      
        
           
   0  as Mark2,                                                               
   0  as IsSwapped,                                                                                                   
   0  as NotionalValue,                                                                                                                                                            
   0  as BenchMarkRate,                                                              
   0  as Differential,                                                                                                                                                                       
   0  as OrigCostBasis,                                                                                                                                                                                                                                 
   0  as DayCount,                                                                                                                                                                              
   '1800-01-01 00:00:00.000' as FirstResetDate,                                    
   '1800-01-01 00:00:00.000' as OrigTransDate,                                                             
   0  as SwapData,                    
   0  as  BeginningMarketValue_Local,                                                                                                                                                                                            
   0  as  BeginningMarketValue_Base,                         
   0  as  EndingMarketValue_Local ,                                                                                                                
   0  as  EndingMarketValue_Base ,                                                                                   
 'D' as Open_CloseTag,                                                                   
   Max(IsNull(FXDayRatesForDiviDate.RateValue,0)) as ConversionRateTrade,                                                                                                                                     
   0 as ConversionRateStart,                                                                                                                                                                           
   0 as ConversionRateEnd,                                                                                          
   Min(SM.CompanyName) as CompanyName,                                                                                                                                                                                                                     
   Min(#T_CompanyFunds.FundName) as FundName,                                                                                                                                                                                       
   'Strategy Unallocated' as StrategyName,                                                                       
Case                                                                     
 When Sum(CashDiv.Dividend) >= 0                       
 Then 'Long'                                                                    
 Else 'Short'                                                                    
End as Side,                                                                                
Min(IsNull(#T_Asset.AssetName,'Undefined')) as Asset,                                                              
Min(IsNull(SM.AssetName,'Undefined')) as UDAAsset,                                                                         
Min(IsNull(SM.SecurityTypeName,'Undefined')) as UDASecurityTypeName,                                                                                                                                                               
Min(IsNull(SM.SectorName,'Undefined')) as UDASectorName,                                                                                                                   
Min(IsNull(SM.SubSectorName,'Undefined')) as UDASubSectorName,                                                                                                                                                                
Min(IsNull(SM.CountryName,'Undefined')) as UDACountryName,                                                                                   
Min(IsNull(SM.PutOrCall,'')) as PutOrCall,                                                                         
Min(IsNull(CMF.MasterFundName,'Unassigned')) as MasterFundName,                                                                     
Case                                                                                                                                                
  When Min(CashDiv.CurrencyID) =  @BaseCurrencyID                                                                                            
  Then Sum(CashDiv.Dividend)                                                                  
  Else Max(IsNull(FXDayRatesForDiviDate.RateValue,0)) * Sum(CashDiv.Dividend)                                                                  
End as Dividend,                                                                                                                
'Unassigned' as MasterStrategyName,                                                                                        
Min(IsNull(SM.UnderlyingSymbol,'')) as UnderlyingSymbol,                                                                
0 as CashFXUnrealizedPNL,                                    
0 As NetTotalCost_Base,                                            
max(SM.BloombergSymbol) as BloombergSymbol,                                          
max(SM.SedolSymbol) as SedolSymbol,                                          
max(SM.ISINSymbol) as ISINSymbol,                                          
max(SM.CusipSymbol) as CusipSymbol,                                          
max(SM.OSISymbol) as OSISymbol,                                          
max(SM.IDCOSymbol) as IDCOSymbol,                      
0 As UnrealizedPNLMTM_Local,                      
0 As RealizedPNLMTM_Local,                      
0 As UnrealizedPNLMTM_Base,                      
0 As RealizedPNLMTM_Base    ,              
 Min(CAST(FLOOR(CAST(SM.ExpirationDate AS FLOAT ) ) AS DATETIME )) as ExpirationDate,              
'' as OpenTradeAttribute1,              
'' as OpenTradeAttribute2,              
'' as OpenTradeAttribute3,              
'' as OpenTradeAttribute4,              
'' as OpenTradeAttribute5,              
'' as OpenTradeAttribute6,                 
'' As ClosedTradeAttribute1 ,              
'' As ClosedTradeAttribute2 ,              
'' As ClosedTradeAttribute3 ,              
'' As ClosedTradeAttribute4 ,              
'' As ClosedTradeAttribute5 ,              
'' As ClosedTradeAttribute6               
--min(SM.ExpirationDate) as ExpirationDate              
                                                                                          
  from T_TaxlotCashDividends CashDiv                        
   Inner Join #T_CompanyFunds ON  CashDiv.FundID= #T_CompanyFunds.CompanyFundID                                        
   LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = CashDiv.Symbol                                                                                                                                                 
   Left OUTER JOIN T_Currency C on CashDiv.CurrencyID = C.CurrencyID              
              
   Inner Join #T_Asset On #T_Asset.AssetID=SM.AssetId                                                           
   --Left outer Join T_AUEC AUEC On AUEC.AUECID=SM.AUECID                                             
   LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                                                   
   LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                   
   Left outer  join #FXConversionRates FXDayRatesForDiviDate On               
(FXDayRatesForDiviDate.FromCurrencyID = CashDiv.CurrencyID                                                             
   And FXDayRatesForDiviDate.ToCurrencyID = @BaseCurrencyID               
And DateDiff(d,CashDiv.ExDate,FXDayRatesForDiviDate.Date)=0)               
              
                                                            
 Where DateDiff(d,@StartDate,CashDiv.ExDate) >=0                                                                                
       and DateDiff(d,CashDiv.ExDate,@EndDate)>=0                                                                              
       Group By CashDiv.FundId,CashDiv.Symbol,CashDiv.ExDate,CashDiv.CurrencyID                                                                                                              
                                                                                                      
/**************************************************************************                                            
      NAME CHANGE HANDLING                                            
**************************************************************************/                                                                                                                   
Insert into #MTMDataTable                                                                                                        
select                                                                                                                    
  PTFundID as FundID,                                                                         
  PTSymbol as Symbol,                                                                                                                           
  ClosedQty as ClosedQty ,                                                            
  PTAvgPrice as AvgPrice ,                                                                                                                                                           
  IsNull(PT1AvgPrice,0)as ClosingPrice ,                                                     
  GAssetID as AssetID,                                                                                                                                                 
  GCurrencyID as CurrencyID,                   
  C.CurrencySymbol as CurrencySymbol,                                                                               
  GAUECID as AUECID,                                                                                                
                                                                                                                                      
IsNull(PTOpenTotalCommissionandFees,0) As  TotalOpenCommission_Local,                                                                                                                         
                      
Case                                            
 When GCurrencyID =  @BaseCurrencyID   --When Company and Traded Currency both are same                                                              
 Then IsNull(PTOpenTotalCommissionandFees,0)                                                                                          
 Else  ----When Company and Traded Currency are different                               
-- Case                                                                                
--  When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                         
--  Then IsNull(PTOpenTotalCommissionandFees * G.FXRate,0)                                           
--  When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                          
--  Then IsNull(PTOpenTotalCommissionandFees * 1/G.FXRate,0)                                                                                                         
--  When G.FXRate <= 0 OR G.FXRate is null                                                                                                                                                                                  
--  Then  IsNull(PTOpenTotalCommissionandFees * IsNull(FXDayRatesForTradeDate.RateValue,0),0)                                                                                                                                                       
-- End                                   
IsNull(PTOpenTotalCommissionandFees * OpeningFXRate,0)                                                                                                                                                                                                       
End as TotalOpenCommission_Base,                                   
                            
 --Closed Commission                                                                               
0 as TotalClosedCommission_Local,                                                                                
0 as TotalClosedCommission_Base,                                                                                    
SM.Multiplier as AssetMultiplier,                                                                                                                                                                                      
GProcessDate as TradeDate,                                                                                                                                                                     
AUECLocalDate  as ClosingDate, --now closing taxlot Trade date is cloisng date                                                                                    
IsNull(MPS.FinalMarkPrice,0) as Mark1,                                                                                 
IsNull(MPE.FinalMarkPrice,0) As Mark2,            
0 as IsSwapped,                                                                        
0 as NotionalValue,                                                                                                                                          
0 as BenchMarkRate,                                                                               
0 as Differential,                                                                                                                                                                        
0 as OrigCostBasis,                                                                                      
0 as DayCount,                                                                                                                     
'1800-01-01 00:00:00.000'  as FirstResetDate,                                                                                                                                                                                                                 
 
'1800-01-01 00:00:00.000'  as OrigTransDate ,                                                                                                                         
0 as SwapData,                                                                                                                                                                         
                                                                                                                                                                              
Case                                              
 When DateDiff(d,GProcessDate,@StartDate) > 0                                                                                                            
 Then IsNull(MPS.FinalMarkPrice,0) * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                                                                          
 
    
      
        
          
            
              
                
                 
 Else PTAvgPrice * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                 
End As BeginningMarketValue_Local ,                                   
                                                           
Case                                                                                                                                                                                 
 When GCurrencyID =  @BaseCurrencyID                                                                                                                                                                
Then                                          
 Case   
   When DateDiff(d,GProcessDate,@StartDate) > 0                                                             
   Then IsNull(MPS.FinalMarkPrice,0) * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                                                                       
   
    
      
        
          
            
             
                 
                 
                    
                      
   Else PTAvgPrice * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                                                                                                                          
  
    
      
        
          
            
              
                
                  
                    
                      
                          
 End                         
Else                                                               
 Case                                             
  When  DateDiff(d,GProcessDate,@StartDate) > 0                                                                            
  Then IsNull(MPS.FinalMarkPrice,0) * StartDateFXRate * ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue)                                                     
  Else                                                                                                                                             
--   Case                                                                                                                         
--    When G.FXRate > 0 And G.FXConversionMethodOperator='M'                                                                                                                          
                    
                      
--    Then IsNull(PT.AvgPrice * G.FXRate * PTC.ClosedQty  * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                                           
  
   
       
       
           
           
               
               
                   
                    
                      
--    When G.FXRate > 0 And G.FXConversionMethodOperator='D'                                                                                                                          
--    Then IsNull((PT.AvgPrice * 1/G.FXRate) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                    
--    When G.FXRate <= 0 OR G.FXRate is null                                                                                 
--    Then IsNull(PT.AvgPrice * FXDayRatesForTradeDate.RateValue * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PT.OrderSideTagValue),0)                                                                                                   
--   End                                 
 IsNull(PTAvgPrice * OpeningFXRate * ClosedQty  * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(PTOrderSideTagValue),0)                                                                                                                                      
 
     
      
        
          
            
              
                
                  
                    
                           
 End                                                                                                                                    
End as BeginningMarketValue_Base ,                                
                                                                                 
ISNULL(PT1AvgPrice,0)* ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(GOrderSideTagValue,G1OrderSideTagValue),1) as  EndingMarketValue_Local,       
                                    
Case                                                                                                                                                                                                                        
 When GCurrencyID <> @BaseCurrencyID                                                                                     
Then                                                                                                                                                 
-- Case                                                                                       
--  When G1.FXRate > 0 And G1.FXConversionMethodOperator='M'                                                                                                                                                              
--  Then IsNull(PT1.AvgPrice,0)* G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                          
        
          
            
              
                
                  
                    
                      
--  When G1.FXRate > 0 And G1.FXConversionMethodOperator='D'                                                                                                                                                              
--  Then IsNull(PT1.AvgPrice,0)* 1/G1.FXRate * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                                                 
 --  Else IsNull(PT1.AvgPrice,0)* IsNull(FXDayRatesForClosingDate.RateValue,0) * PTC.ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(G.OrderSideTagValue,G1.OrderSideTagValue),1)                                                
   
   
       
        
          
            
              
                
-- End                                   
IsNull(PT1AvgPrice,0)* EndingFXRate * ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(GOrderSideTagValue,G1OrderSideTagValue),1)                                                                                                  
  
    
      
        
          
            
              
                
                  
                    
                          
Else ISNULL(PT1AvgPrice,0)* ClosedQty * IsNUll(SM.Multiplier,0) * IsNull(dbo.GetSideMultiplierForClosing(GOrderSideTagValue,G1OrderSideTagValue),1)                                                                                                            
  
    
      
        
          
            
    
                
                  
                    
                      
End as  EndingMarketValue_Base,                                                                            
                                                                                                                
'O' as Open_CloseTag,                           
--IsNull(FXDayRatesForTradeDate.RateValue,0) As ConversionRateTrade,                                          
--IsNull(FXDayRatesForStartDate.RateValue,0) as ConversionRateStart,                                                                                                                                                                      
--IsNull(FXDayRatesForClosingDate.RateValue,0) as ConversionRateClosing,                                
                                 
TradeDateFXRate As ConversionRateTrade,                                                                                                                                                  
StartDateFXRate as ConversionRateStart,                                   
ClosingDateFXRate as ConversionRateClosing,                                
                                                                                                       
SM.CompanyName,                                                                                                                                                                                                                  
    
     
         
          
            
              
                
                 
                     
                                               
#T_CompanyFunds.FundName  as FundName,                                                                                                                                                                                                 
IsNull(CompanyStrategy.StrategyName,'Strategy Unallocated') AS StrategyName,                                                                                                          
                           
Case dbo.GetSideMultiplierForClosing(GOrderSideTagValue,G1OrderSideTagValue)                                                                                                                                                                                  
   
    
      
        
          
            
              
               
  When  1                       
  Then  'Long'                                                                                                                                       
  When  -1                                                                                                                        
  Then  'Short'                                                                                                                                                                                                    
  Else  ''                                                                                      
End as Side,                                                                                 
                                                                                                                                                  
#T_Asset.AssetName as Asset,                                                                   
IsNull(SM.AssetName,'Undefined') as UDAAsset,                                                         
IsNull(SM.SecurityTypeName,'Undefined') as UDASecurityTypeName,                                                                                           
IsNull(SM.SectorName,'Undefined') as UDASectorName,                                                                                                                                                                                                 
IsNull(SM.SubSectorName,'Undefined') as UDASubSectorName,                                        
IsNull(SM.CountryName,'Undefined') as UDACountryName,                            
IsNUll(SM.PutOrCall,'') as PutOrCall,                                                                                                                                                                                                        
IsNull(CMF.MasterFundName,'Unassigned') As MasterFundName,                      
0 as Dividend,                                                                                       
IsNull(CMS.MasterStrategyName,'Unassigned') as MasterStrategyName,                                                                                        
IsNull(SM.UnderlyingSymbol,'') as UnderlyingSymbol,                                                                
0 as CashFXUnrealizedPNL,                                    
0 As NetTotalCost_Base,                                
SM.BloombergSymbol,                                          
SM.SedolSymbol,                                          
SM.ISINSymbol,                                          
SM.CusipSymbol,                                          
SM.OSISymbol,                                          
SM.IDCOSymbol,                      
0 As UnrealizedPNLMTM_Local,                      
0 As RealizedPNLMTM_Local,                      
0 As UnrealizedPNLMTM_Base,                      
0 As RealizedPNLMTM_Base     ,              
 CAST(FLOOR(CAST(SM.ExpirationDate AS FLOAT ) ) AS DATETIME ) as ExpirationDate,              
GTradeAttribute1 as OpenTradeAttribute1,              
GTradeAttribute2 as OpenTradeAttribute2,              
GTradeAttribute3 as OpenTradeAttribute3,              
GTradeAttribute4 as OpenTradeAttribute4,              
GTradeAttribute5 as OpenTradeAttribute5,              
GTradeAttribute6 as OpenTradeAttribute6,                 
G1TradeAttribute1 As ClosedTradeAttribute1 ,              
G1TradeAttribute2 As ClosedTradeAttribute2 ,              
G1TradeAttribute3 As ClosedTradeAttribute3 ,              
G1TradeAttribute4 As ClosedTradeAttribute4 ,              
G1TradeAttribute5 As ClosedTradeAttribute5 ,              
G1TradeAttribute6 As ClosedTradeAttribute6               
--SM.ExpirationDate                                                                                                                              
                                                                                                                                                                                 
                                  
from #TempClosingData                                                                 
  Inner Join #T_CompanyFunds ON  #TempClosingData.PTFundID= #T_CompanyFunds.CompanyFundID                                         
  Inner Join #T_Asset On #T_Asset.AssetId=#TempClosingData.GAssetID                                                                                                                                                                                            
 
    
      
        
           
            
             
  Left OUTER JOIN T_Currency C on GCurrencyID = C.CurrencyID              
  Left Outer Join #MarkPriceForStartDate MPS on MPS.Symbol=#TempClosingData.PTSymbol                                                                           
  Left Outer Join #MarkPriceForEndDate MPE on MPE.Symbol=#TempClosingData.PTSymbol                                                                                                                                   
  --get yesterday business day                                                                                                                                  
  LEFT OUTER JOIN #AUECYesterDates AUECYesterDates ON #TempClosingData.GAUECID = AUECYesterDates.AUECID                                                                   
  -- Security Master DB join                                                                 
  LEFT OUTER JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = #TempClosingData.PTSymbol                                                                                                                                 
 Left Outer Join T_CompanyStrategy as CompanyStrategy On CompanyStrategy.CompanyStrategyID=#TempClosingData.PTLevel2ID                                                                                                                                         
  
    
      
       
           
           
               
                
 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON #T_CompanyFunds.CompanyFundID = CMFSSAA.CompanyFundID                                 
 LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                                                                      
 LEFT OUTER JOIN T_CompanyMasterStrategySubAccountAssociation CMSSSAA ON CompanyStrategy.CompanyStrategyID = CMSSSAA.CompanyStrategyID                                                                                                                         
  
    
     
         
          
            
              
                
 LEFT OUTER JOIN T_CompanyMasterStrategy CMS ON CMSSSAA.CompanyMasterStrategyID = CMS.CompanyMasterStrategyID                                   
 Where ClosingMode=7 --7 means CoperateAction!                                                                                                                    
                                                                
                                                                              
/**************************************************************************                                            
       CASH HANDLING                                            
**************************************************************************/                                                                           
Insert into #MTMDataTable                                                                                                                                        
 Select                                                                                                                                                                            
   IsNull(DailyCash.FundId,0) as FundID,                                                                                        
   MIN(CLocal.CurrencySymbol) AS Symbol,                                                      
   0  as TaxLotOpenQty ,                                                                                                                       
   0  as AvgPrice ,                                                                                                   
   0  as ClosingPrice,                                                                                             
   6  as AssetID,                                                                                                                                                                 
   MIN(DailyCash.LocalCurrencyID) as CurrencyID,                 
   MIN(CLocal.CurrencySymbol) as CurrencySymbol,                                                                                   
   0  as AUECID ,                                                                               
   0 As TotalOpenCommission_Local,                                                                                                                                                                                                         
   0  as TotalOpenCommission_Base,                        
   0  as TotalClosedCommission_Local,                                                                                                                                                                                                                         
   
   
       
       
          
   0  as TotalClosedCommission_Base,                                                                                                     
   1  as AssetMultiplier,                                                 
    Max(DailyCash.Date) as TradeDate,                                                                                      
   '1800-01-01 00:00:00.000' as ClosingDate,                                                                                                                                                                                                                   
 
   0  as Mark1,                                             
   0  as Mark2,                                                         
   0  as IsSwapped,                                                                                                 
   0  as NotionalValue,                         
   0  as BenchMarkRate,                                                                 
   0  as Differential,                                                                                                                                                                                                    
   0  as OrigCostBasis,                    
   0  as DayCount,                                                
   '1800-01-01 00:00:00.000' as FirstResetDate,                                                                                                                                   
   '1800-01-01 00:00:00.000' as OrigTransDate,                                                                                                              
   0  as SwapData,                                                                                                                                                                                    
   0 As BeginningMarketValue_Local,                                                                               
   0  as  BeginningMarketValue_Base,                        
   Sum(IsNull(DailyCash.CashValueLocal,0))  as  EndingMarketValue_Local ,                                                                                                              
   Sum(IsNull(DailyCash.CashValueBase,0))  as  EndingMarketValue_Base ,                                                                                                                                                                 
   'CASH' as Open_CloseTag,                                            
   0 as ConversionRateTrade,                                                                                                                                           
   Min(IsNull(FXDayRatesForStartDate.RateValue,0)) as ConversionRateStart,                       
   Min(IsNull(FXDayRatesForEndDate.RateValue,0)) as ConversionRateEnd,                                                                 
   'Undefined' as CompanyName,                                                                                                                                                                                                                                
  
    
      
         
         
            
              
                
                  
                    
                       
   Min(CF.FundName) as FundName,                                                                                                                                                                       
   'Strategy Unallocated' as StrategyName,                                                                     
  Case                                                                   
  When Sum(DailyCash.CashValueLocal) >= 0                                                                  
 Then 'Long'                                                                  
 Else 'Short'                                                                  
  End as Side,                                                                                                                                   
                                                                                                                     
 'Cash' as Asset,                                                                      
'Undefined' as UDAAsset,                                                        
'Undefined' as UDASecurityTypeName,                                                                                                                                                        
'Undefined' as UDASectorName,                                                                                                                 
'Undefined' as UDASubSectorName,                                                                                                                                                              
'Undefined' as UDACountryName,                                                                                 
'' as PutOrCall,                                                                                                                                                        
--'Undefined' as MasterFundName,                                           
IsNull(CMF.MasterFundName,'Unassigned') as MasterFundName,                                          
0 as Dividend,                                                                                                              
 'Unassigned' as MasterStrategyName,                                                                          
'' as UnderlyingSymbol,                                                      
Case                                                                 
 When DailyCash.LocalCurrencyID <> @BaseCurrencyID                                                                
 Then (Min(IsNull(FXDayRatesForEndDate.RateValue,0)) - Min(IsNull(FXDayRatesForStartDate.RateValue,0))) * Sum(DailyCash.CashValueLocal)                                                                
 Else 0                                                                
End as CashFXUnrealizedPNL,                                    
0 As NetTotalCost_Base,                                            
'' as BloombergSymbol,                                          
'' as SedolSymbol,                                          
'' as ISINSymbol,                                          
'' as CusipSymbol,                                          
'' as OSISymbol,                                          
'' as IDCOSymbol,                      
0 As UnrealizedPNLMTM_Local,                      
0 As RealizedPNLMTM_Local,                      
0 As UnrealizedPNLMTM_Base,                      
0 As RealizedPNLMTM_Base   ,              
 '1800-01-01'  as ExpirationDate,              
'' as OpenTradeAttribute1,              
'' as OpenTradeAttribute2,              
'' as OpenTradeAttribute3,           
'' as OpenTradeAttribute4,              
'' as OpenTradeAttribute5,              
'' as OpenTradeAttribute6,                 
'' As ClosedTradeAttribute1 ,              
'' As ClosedTradeAttribute2 ,              
'' As ClosedTradeAttribute3 ,              
'' As ClosedTradeAttribute4 ,              
'' As ClosedTradeAttribute5 ,              
'' As ClosedTradeAttribute6               
 --'1800-01-01 00:00:00.000' as ExpirationDate                                          
                                                                
 --From T_DayEndBalances DailyCash                       
 From PM_companyFundCashCurrencyValue DailyCash                       
 INNER JOIN #T_CompanyFunds CF ON DailyCash.FundID = CF.CompanyFundID                                                                      
   INNER JOIN T_Currency CLocal ON DailyCash.LocalCurrencyID = CLocal.CurrencyID                     
  INNER JOIN T_Currency CBase ON BaseCurrencyID = CBase.CurrencyID                                                                                        
   LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMFSSAA ON CF.CompanyFundID = CMFSSAA.CompanyFundID                                                                                                                                                
 
    
       
       
           
            
             
                
                   
             
                     
   LEFT OUTER JOIN T_CompanyMasterFunds CMF ON CMFSSAA.CompanyMasterFundID = CMF.CompanyMasterFundID                                                                 
                                                                
  -- Forex Price for Start Date                                                                            
 Left outer join #FXConversionRates FXDayRatesForStartDate                                                                                                                                        
 on (FXDayRatesForStartDate.FromCurrencyID = DailyCash.LocalCurrencyID                                                                     
 And FXDayRatesForStartDate.ToCurrencyID = @BaseCurrencyID                                                                                                                                                             
 And DateDiff(d,dbo.AdjustBusinessDays(@StartDate,-1,@DefaultAUECID),FXDayRatesForStartDate.Date)=0)                                                                                           
                                                                                                         
 -- Forex Price for End Date                                                                                                                                                                   
  Left outer join #FXConversionRates FXDayRatesForEndDate                                                                                                                                                            
 on (FXDayRatesForEndDate.FromCurrencyID = DailyCash.LocalCurrencyID                                                                                             
 And FXDayRatesForEndDate.ToCurrencyID = @BaseCurrencyID                                                                                                                  
 And DateDiff(d,dbo.AdjustBusinessDays(DateAdd(d,1,@EndDate),-1,@DefaultAUECID),FXDayRatesForEndDate.Date)=0)                                                                    
   WHERE DateDiff(Day,DailyCash.Date,@RecentDateForNonZeroCash)=0                                               
   GROUP BY CMF.MasterFundName,DailyCash.FundID,DailyCash.LocalCurrencyID                               
                                                                                                      
                      
-------------------------------------------------------------------------------------------------------------                      
Update #MTMDataTable              
Set               
              
ClosedTradeAttribute1=              
CASE              
WHEN (Open_CloseTag ='o')               
THEN OpenTradeAttribute1              
ELSE ClosedTradeAttribute1              
END,              
ClosedTradeAttribute2=              
CASE              
WHEN (Open_CloseTag ='o')               
THEN OpenTradeAttribute2              
ELSE ClosedTradeAttribute2              
END,              
ClosedTradeAttribute3=              
CASE              
WHEN (Open_CloseTag ='o')               
THEN OpenTradeAttribute3              
ELSE ClosedTradeAttribute3              
END,              
ClosedTradeAttribute4=              
CASE              
WHEN (Open_CloseTag ='o')               
THEN OpenTradeAttribute4              
ELSE ClosedTradeAttribute4              
END,              
ClosedTradeAttribute5=              
CASE              
WHEN (Open_CloseTag ='o')               
THEN OpenTradeAttribute5              
ELSE ClosedTradeAttribute5              
END,              
ClosedTradeAttribute6=              
CASE              
WHEN (Open_CloseTag ='o')               
THEN OpenTradeAttribute6              
ELSE ClosedTradeAttribute6      END              
              
---------------------------------------------------------------------------------------------------------------------                
              
Update #MTMDataTable              
Set               
ClosedTradeAttribute1=              
CASE              
WHEN ((ClosedTradeAttribute1='' OR ClosedTradeAttribute1 = NULL) and (Open_CloseTag = 'C'))              
THEN OpenTradeAttribute1              
ELSE ClosedTradeAttribute1              
END,              
ClosedTradeAttribute2=              
CASE              
WHEN ((ClosedTradeAttribute2='' OR ClosedTradeAttribute2 = NULL) and (Open_CloseTag = 'C'))              
THEN OpenTradeAttribute2              
ELSE ClosedTradeAttribute2              
END,              
ClosedTradeAttribute3=              
CASE              
WHEN ((ClosedTradeAttribute3='' OR ClosedTradeAttribute3 = NULL) and (Open_CloseTag = 'C'))              
THEN OpenTradeAttribute3              
ELSE ClosedTradeAttribute3              
END,              
ClosedTradeAttribute4=              
CASE              
WHEN ((ClosedTradeAttribute4='' OR ClosedTradeAttribute4 = NULL) and (Open_CloseTag = 'C'))              
THEN OpenTradeAttribute4              
ELSE ClosedTradeAttribute4              
END,              
ClosedTradeAttribute5=              
CASE              
WHEN ((ClosedTradeAttribute5='' OR ClosedTradeAttribute5 = NULL) and (Open_CloseTag = 'C'))              
THEN OpenTradeAttribute5              
ELSE ClosedTradeAttribute5              
END,              
ClosedTradeAttribute6=              
CASE              
WHEN ((ClosedTradeAttribute6='' OR ClosedTradeAttribute6 = NULL) and (Open_CloseTag = 'C'))              
THEN OpenTradeAttribute6              
ELSE ClosedTradeAttribute6              
END,              
OpenTradeAttribute1=              
CASE              
WHEN ((OpenTradeAttribute1='' OR OpenTradeAttribute1 = NULL) and (Open_CloseTag = 'C'))              
THEN ClosedTradeAttribute1              
ELSE OpenTradeAttribute1              
END,              
OpenTradeAttribute2=              
CASE              
WHEN ((OpenTradeAttribute2='' OR OpenTradeAttribute2 = NULL) and (Open_CloseTag = 'C'))              
THEN ClosedTradeAttribute2              
ELSE OpenTradeAttribute2              
END,              
OpenTradeAttribute3=              
CASE              
WHEN ((OpenTradeAttribute3='' OR OpenTradeAttribute3 = NULL) and (Open_CloseTag = 'C'))              
THEN ClosedTradeAttribute3              
ELSE OpenTradeAttribute3              
END,              
OpenTradeAttribute4=              
CASE              
WHEN ((OpenTradeAttribute4='' OR OpenTradeAttribute4 = NULL) and (Open_CloseTag = 'C'))              
THEN ClosedTradeAttribute4              
ELSE OpenTradeAttribute4              
END,              
OpenTradeAttribute5=              
CASE              
WHEN ((OpenTradeAttribute5='' OR OpenTradeAttribute5 = NULL) and (Open_CloseTag = 'C'))              
THEN ClosedTradeAttribute5              
ELSE OpenTradeAttribute5              
END,              
OpenTradeAttribute6=              
CASE              
WHEN ((OpenTradeAttribute6='' OR OpenTradeAttribute6 = NULL) and (Open_CloseTag = 'C'))              
THEN ClosedTradeAttribute6              
ELSE OpenTradeAttribute6              
END              
              
---------------------------------------------------------------------------------------------------------------------                
              
Update #MTMDataTable              
Set               
ClosedTradeAttribute1=              
CASE              
WHEN (ClosedTradeAttribute1='' OR ClosedTradeAttribute1 = NULL)              
THEN 'undefined'              
ELSE ClosedTradeAttribute1              
END,              
ClosedTradeAttribute2=              
CASE              
WHEN (ClosedTradeAttribute2='' OR ClosedTradeAttribute2 = NULL)              
THEN 'undefined'              
ELSE ClosedTradeAttribute2              
END,              
ClosedTradeAttribute3=              
CASE              
WHEN (ClosedTradeAttribute3='' OR ClosedTradeAttribute3 = NULL)              
THEN 'undefined'              
ELSE ClosedTradeAttribute3              
END,              
ClosedTradeAttribute4=              
CASE              
WHEN (ClosedTradeAttribute4='' OR ClosedTradeAttribute4 = NULL)              
THEN 'undefined'              
ELSE ClosedTradeAttribute4              
END,              
ClosedTradeAttribute5=              
CASE              
WHEN (ClosedTradeAttribute5='' OR ClosedTradeAttribute5 = NULL)              
THEN 'undefined'              
ELSE ClosedTradeAttribute5              
END,              
ClosedTradeAttribute6=              
CASE              
WHEN (ClosedTradeAttribute6='' OR ClosedTradeAttribute6 = NULL)              
THEN 'undefined'              
ELSE ClosedTradeAttribute6              
END,              
OpenTradeAttribute1=              
CASE              
WHEN (OpenTradeAttribute1='' OR OpenTradeAttribute1 = NULL)              
THEN 'undefined'              
ELSE OpenTradeAttribute1              
END,              
OpenTradeAttribute2=              
CASE              
WHEN (OpenTradeAttribute2='' OR OpenTradeAttribute2 = NULL)              
THEN 'undefined'              
ELSE OpenTradeAttribute2              
END,              
OpenTradeAttribute3=              
CASE              
WHEN (OpenTradeAttribute3='' OR OpenTradeAttribute3 = NULL)              
THEN 'undefined'              
ELSE OpenTradeAttribute3              
END,              
OpenTradeAttribute4=              
CASE              
WHEN (OpenTradeAttribute4='' OR OpenTradeAttribute4 = NULL)              
THEN 'undefined'              
ELSE OpenTradeAttribute4              
END,              
OpenTradeAttribute5=              
CASE              
WHEN (OpenTradeAttribute5='' OR OpenTradeAttribute5 = NULL)              
THEN 'undefined'              
ELSE OpenTradeAttribute5              
END,              
OpenTradeAttribute6=              
CASE              
WHEN (OpenTradeAttribute6='' OR OpenTradeAttribute6 = NULL)              
THEN 'undefined'              
ELSE OpenTradeAttribute6              
END              
---------------------------------------------------------------------------------------------------------------------                
              
              
Update #MTMDataTable                                                                    
Set                                                                     
BeginningMarketValue_Local = BeginningMarketValue_Local /100  ,                                              
BeginningMarketValue_Base = BeginningMarketValue_Base /100,                                                                   
EndingMarketValue_Local = EndingMarketValue_Local /100,                                                                  
EndingMarketValue_Base = EndingMarketValue_Base /100                                                                                 
Where Asset = 'FixedIncome'                       
                  
---------------------------------------------------------------------------------------------------------------------                
                    
Update #MTMDataTable                      
SET                      
                
UnrealizedPNLMTM_Local  =                                                     
CASE                 
--Local Unrealized PnL of Open Trade with Commission                                                          
 WHEN                  
 (                
 (Open_CloseTag = 'O' and  Asset = 'Future'  and @IncludeCommissionInPNL_Futures =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'FutureOption'  and @IncludeCommissionInPNL_FutOptions =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'FX'  and @IncludeCommissionInPNL_FX =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'Equity' and IsSwapped =1 and @IncludeCommissionInPNL_Swaps =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'Equity' and @IncludeCommissionInPNL_Equity =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption =1) or                     
 (Open_CloseTag = 'O' and Asset not in ('Future','FutureOption','FX','Equity','EquityOption') and @IncludeCommissionInPNL_Other =1)                      
 )                
 THEN                      
  CASE                      
  WHEN DateDiff(d,@StartDate,TradeDate) >= 0                 
  THEN (Isnull(EndingMarketValue_Local,0) - Isnull(BeginningMarketValue_Local,0)) - TotalOpenCommission_Local                 
  ELSE Isnull(EndingMarketValue_Local,0) - Isnull(BeginningMarketValue_Local,0)                 
  END                  
--Local Unrealized PnL of Open Trade without Commission                                                           
 WHEN          
 (                
 (Open_CloseTag = 'O' and  Asset = 'Future'  and @IncludeCommissionInPNL_Futures =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'FutureOption'  and @IncludeCommissionInPNL_FutOptions =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'FX'  and @IncludeCommissionInPNL_FX =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'Equity' and IsSwapped =1 and @IncludeCommissionInPNL_Swaps =0) or     
 (Open_CloseTag = 'O' and  Asset = 'Equity' and @IncludeCommissionInPNL_Equity =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption =0) or                    
 (Open_CloseTag = 'O' and Asset not in ('Future','FutureOption','FX','Equity','EquityOption') and @IncludeCommissionInPNL_Other =0)                       
 )                
 THEN                      
  Isnull(EndingMarketValue_Local,0) - Isnull(BeginningMarketValue_Local,0)                
 ELSE 0                 
END,                      
                      
                
                
UnrealizedPNLMTM_Base =                      
CASE                                            
--Unrealized PnL of Open Trade with Ending FX rate and Commission                 
 WHEN                  
 (                
 (Open_CloseTag = 'O' and  Asset = 'Future'  And  @FuturePNLWithBothOrEndFXRate = 0 and @IncludeCommissionInPNL_Futures =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'FutureOption'  And  @InternationalFutureOptionPNLWithBothOrEndFXRate = 0 and @IncludeCommissionInPNL_FutOptions =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'FX'  And  @IncludeFXPNLinFX = 0 and @IncludeCommissionInPNL_FX =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'Equity' and IsSwapped =1  And  @IncludeFXPNLinSwaps = 0 and @IncludeCommissionInPNL_Swaps =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'Equity'  And  @IncludeFXPNLinEquity = 0 and @IncludeCommissionInPNL_Equity =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'EquityOption'  And  @IncludeFXPNLinEquityOption = 0 and @IncludeCommissionInPNL_EquityOption =1)  or                 
 (Open_CloseTag = 'O' and Asset not in ('Future','FutureOption','FX','Equity','EquityOption') and @IncludeFXPNLinOther=0 and @IncludeCommissionInPNL_Other =1)                                 
 )                
 THEN                      
 CASE                      
  WHEN DateDiff(d,@StartDate,TradeDate) >= 0                  
  THEN ((Isnull(EndingMarketValue_Local,0) - Isnull(BeginningMarketValue_Local,0)) - TotalOpenCommission_Local) * ConversionRateEnd                                                
  ELSE (Isnull(EndingMarketValue_Local,0) - Isnull(BeginningMarketValue_Local,0)) * ConversionRateEnd                 
 END                 
                
--Unrealized PnL of Open Trade with Ending FX rate and no Commission                
 WHEN                  
 (                
 (Open_CloseTag = 'O' and  Asset = 'Future'  And  @FuturePNLWithBothOrEndFXRate = 0 and @IncludeCommissionInPNL_Futures =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'FutureOption'  And  @InternationalFutureOptionPNLWithBothOrEndFXRate = 0 and @IncludeCommissionInPNL_FutOptions =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'FX'  And  @IncludeFXPNLinFX = 0 and @IncludeCommissionInPNL_FX =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'Equity' and IsSwapped =1  And  @IncludeFXPNLinSwaps = 0 and @IncludeCommissionInPNL_Swaps =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'Equity'  And  @IncludeFXPNLinEquity = 0 and @IncludeCommissionInPNL_Equity =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'EquityOption'  And  @IncludeFXPNLinEquityOption = 0 and @IncludeCommissionInPNL_EquityOption =0)   or                  
 (Open_CloseTag = 'O' and Asset not in ('Future','FutureOption','FX','Equity','EquityOption') and @IncludeFXPNLinOther=0 and @IncludeCommissionInPNL_Other =0)                              
 )                
 THEN                      
  (Isnull(EndingMarketValue_Local,0) - Isnull(BeginningMarketValue_Local,0)) * ConversionRateEnd                 
                  
--Unrealized PnL of Open Trade with both FX rates and Commission                
 WHEN                  
 (                
 (Open_CloseTag = 'O' and  Asset = 'Future'  And  @FuturePNLWithBothOrEndFXRate = 1 and @IncludeCommissionInPNL_Futures =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'FutureOption'  And  @InternationalFutureOptionPNLWithBothOrEndFXRate = 1 and @IncludeCommissionInPNL_FutOptions =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'FX'  And  @IncludeFXPNLinFX = 1 and @IncludeCommissionInPNL_FX =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'Equity' and IsSwapped =1  And  @IncludeFXPNLinSwaps = 1 and @IncludeCommissionInPNL_Swaps =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'Equity'  And  @IncludeFXPNLinEquity = 1 and @IncludeCommissionInPNL_Equity =1) or                 
 (Open_CloseTag = 'O' and  Asset = 'EquityOption'  And  @IncludeFXPNLinEquityOption = 1 and @IncludeCommissionInPNL_EquityOption =1) or                 
 (Open_CloseTag = 'O' and Asset not in ('Future','FutureOption','FX','Equity','EquityOption') and @IncludeCommissionInPNL_Other =1 and @IncludeFXPNLinOther=1  )                        
 )                
 THEN                      
 CASE                      
  WHEN DateDiff(d,@StartDate,TradeDate) >= 0                 
  THEN (Isnull(EndingMarketValue_Base,0) - Isnull(BeginningMarketValue_Base,0)) - TotalOpenCommission_Base                                                
  ELSE Isnull(EndingMarketValue_Base,0) - Isnull(BeginningMarketValue_Base,0)                          
 END                  
                
--Unrealized PnL of Open Trade with both FX rates and no Commission                
 WHEN                  
 (                
 (Open_CloseTag = 'O' and  Asset = 'Future'  And  @FuturePNLWithBothOrEndFXRate = 1 and @IncludeCommissionInPNL_Futures =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'FutureOption'  And  @InternationalFutureOptionPNLWithBothOrEndFXRate = 1 and @IncludeCommissionInPNL_FutOptions =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'FX'  And  @IncludeFXPNLinFX = 1 and @IncludeCommissionInPNL_FX =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'Equity' and IsSwapped =1  And  @IncludeFXPNLinSwaps = 1 and @IncludeCommissionInPNL_Swaps =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'Equity'  And  @IncludeFXPNLinEquity = 1 and @IncludeCommissionInPNL_Equity =0) or                 
 (Open_CloseTag = 'O' and  Asset = 'EquityOption'  And  @IncludeFXPNLinEquityOption = 1 and @IncludeCommissionInPNL_EquityOption =0) or                 
 (Open_CloseTag = 'O' and Asset not in ('Future','FutureOption','FX','Equity','EquityOption') and @IncludeFXPNLinOther=1 and @IncludeCommissionInPNL_Other =0   )                        
 )                
 THEN                      
  Isnull(EndingMarketValue_Base,0) - Isnull(BeginningMarketValue_Base,0)                          
                    
                 
 WHEN Open_CloseTag = 'CASH'                      
 THEN CashFXUnrealizedPNL                      
ELSE 0                                               
END,                      
                      
                
                
                
RealizedPNLMTM_Local =                       
CASE                      
--Local Realized PnL of closed Trade with Commission            
 WHEN                  
 (                
 (Open_CloseTag = 'C' and  Asset = 'Future'  and @IncludeCommissionInPNL_Futures =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'FutureOption'  and @IncludeCommissionInPNL_FutOptions =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'FX'  and @IncludeCommissionInPNL_FX =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'Equity' and IsSwapped =1 and @IncludeCommissionInPNL_Swaps =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'Equity' and @IncludeCommissionInPNL_Equity =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption =1)     or            
 (Open_CloseTag = 'C' and Asset not in ('Future','FutureOption','FX','Equity','EquityOption') and @IncludeCommissionInPNL_Other =1)                      
 )                
 THEN                      
 CASE                      
  WHEN DateDiff(d,@StartDate,TradeDate) >= 0                        
  THEN (Isnull(EndingMarketValue_Local,0) - Isnull(BeginningMarketValue_Local,0)) - TotalOpenCommission_Local - TotalClosedCommission_Local                                                   
  ELSE (Isnull(EndingMarketValue_Local,0) - Isnull(BeginningMarketValue_Local,0)) - TotalClosedCommission_Local                        
 END                   
--Local Realized PnL of closed Trade without Commission                    
 WHEN                  
 (                
 (Open_CloseTag = 'C' and  Asset = 'Future'  and @IncludeCommissionInPNL_Futures =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'FutureOption'  and @IncludeCommissionInPNL_FutOptions =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'FX'  and @IncludeCommissionInPNL_FX =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'Equity' and IsSwapped =1 and @IncludeCommissionInPNL_Swaps =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'Equity' and @IncludeCommissionInPNL_Equity =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'EquityOption' and @IncludeCommissionInPNL_EquityOption =0)  or                   
 (Open_CloseTag = 'C' and Asset not in ('Future','FutureOption','FX','Equity','EquityOption') and @IncludeCommissionInPNL_Other =0)                       
 )                
 THEN                      
  (Isnull(EndingMarketValue_Local,0) - Isnull(BeginningMarketValue_Local,0))                   
                                            
 ELSE 0                      
END,                      
                      
                
                
                
                
RealizedPNLMTM_Base =                       
CASE                     
--Realized PnL of closed Trade with Ending FX rate and Commission                 
 WHEN                  
 (                
 (Open_CloseTag = 'C' and  Asset = 'Future'  And  @FuturePNLWithBothOrEndFXRate = 0 and @IncludeCommissionInPNL_Futures =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'FutureOption'  And  @InternationalFutureOptionPNLWithBothOrEndFXRate = 0 and @IncludeCommissionInPNL_FutOptions =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'FX'  And  @IncludeFXPNLinFX = 0 and @IncludeCommissionInPNL_FX =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'Equity' and IsSwapped =1  And  @IncludeFXPNLinSwaps = 0 and @IncludeCommissionInPNL_Swaps =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'Equity'  And  @IncludeFXPNLinEquity = 0 and @IncludeCommissionInPNL_Equity =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'EquityOption'  And  @IncludeFXPNLinEquityOption = 0 and @IncludeCommissionInPNL_EquityOption =1)  or                 
 (Open_CloseTag = 'C' and Asset not in ('Future','FutureOption','FX','Equity','EquityOption') and @IncludeFXPNLinOther=0 and @IncludeCommissionInPNL_Other =1)                                 
 )                
 THEN                      
 CASE                       
  WHEN DateDiff(d,@StartDate,TradeDate) >= 0                    
  THEN ((Isnull(EndingMarketValue_Local,0) - Isnull(BeginningMarketValue_Local,0)) - TotalOpenCommission_Local - TotalClosedCommission_Local) * ConversionRateEnd                                                 
  ELSE ((Isnull(EndingMarketValue_Local,0) - Isnull(BeginningMarketValue_Local,0)) - TotalClosedCommission_Local) * ConversionRateEnd                                                                            
 END                 
                
--Realized PnL of closed Trade with Ending FX rate and no Commission                
 WHEN                  
 (                
 (Open_CloseTag = 'C' and  Asset = 'Future'  And  @FuturePNLWithBothOrEndFXRate = 0 and @IncludeCommissionInPNL_Futures =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'FutureOption'  And  @InternationalFutureOptionPNLWithBothOrEndFXRate = 0 and @IncludeCommissionInPNL_FutOptions =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'FX'  And  @IncludeFXPNLinFX = 0 and @IncludeCommissionInPNL_FX =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'Equity' and IsSwapped =1  And  @IncludeFXPNLinSwaps = 0 and @IncludeCommissionInPNL_Swaps =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'Equity'  And  @IncludeFXPNLinEquity = 0 and @IncludeCommissionInPNL_Equity =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'EquityOption'  And  @IncludeFXPNLinEquityOption = 0 and @IncludeCommissionInPNL_EquityOption =0)  or                   
 (Open_CloseTag = 'C' and Asset not in ('Future','FutureOption','FX','Equity','EquityOption') and @IncludeFXPNLinOther=0 and @IncludeCommissionInPNL_Other =0)                              
 )                
 THEN                      
  (Isnull(EndingMarketValue_Local,0) - Isnull(BeginningMarketValue_Local,0)) * ConversionRateEnd                                                                            
                  
--Realized PnL of closed Trade with both FX rates and Commission                
 WHEN                  
 (                
 (Open_CloseTag = 'C' and  Asset = 'Future'  And  @FuturePNLWithBothOrEndFXRate = 1 and @IncludeCommissionInPNL_Futures =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'FutureOption'  And  @InternationalFutureOptionPNLWithBothOrEndFXRate = 1 and @IncludeCommissionInPNL_FutOptions =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'FX'  And  @IncludeFXPNLinFX = 1 and @IncludeCommissionInPNL_FX =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'Equity' and IsSwapped =1  And  @IncludeFXPNLinSwaps = 1 and @IncludeCommissionInPNL_Swaps =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'Equity'  And  @IncludeFXPNLinEquity = 1 and @IncludeCommissionInPNL_Equity =1) or                 
 (Open_CloseTag = 'C' and  Asset = 'EquityOption'  And  @IncludeFXPNLinEquityOption = 1 and @IncludeCommissionInPNL_EquityOption =1) or                 
 (Open_CloseTag = 'C' and Asset not in ('Future','FutureOption','FX','Equity','EquityOption') and @IncludeCommissionInPNL_Other =1 and @IncludeFXPNLinOther=1  )                        
 )                
 THEN                      
 CASE                      
 WHEN DateDiff(d,@StartDate,TradeDate) >= 0                  
  THEN (Isnull(EndingMarketValue_Base,0) - Isnull(BeginningMarketValue_Base,0)) - TotalOpenCommission_Base - TotalClosedCommission_Base                                                   
  ELSE (Isnull(EndingMarketValue_Base,0) - Isnull(BeginningMarketValue_Base,0)) - TotalClosedCommission_Base                                                                             
 END                  
                
--Realized PnL of closed Trade with both FX rates and no Commission                
 WHEN                  
 (                
 (Open_CloseTag = 'C' and  Asset = 'Future'  And  @FuturePNLWithBothOrEndFXRate = 1 and @IncludeCommissionInPNL_Futures =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'FutureOption'  And  @InternationalFutureOptionPNLWithBothOrEndFXRate = 1 and @IncludeCommissionInPNL_FutOptions =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'FX'  And  @IncludeFXPNLinFX = 1 and @IncludeCommissionInPNL_FX =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'Equity' and IsSwapped =1  And  @IncludeFXPNLinSwaps = 1 and @IncludeCommissionInPNL_Swaps =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'Equity'  And  @IncludeFXPNLinEquity = 1 and @IncludeCommissionInPNL_Equity =0) or                 
 (Open_CloseTag = 'C' and  Asset = 'EquityOption'  And  @IncludeFXPNLinEquityOption = 1 and @IncludeCommissionInPNL_EquityOption =0) or                 
 (Open_CloseTag = 'C' and Asset not in ('Future','FutureOption','FX','Equity','EquityOption') and @IncludeFXPNLinOther=1 and @IncludeCommissionInPNL_Other =0   )                        
 )                
 THEN                      
  (Isnull(EndingMarketValue_Base,0) - Isnull(BeginningMarketValue_Base,0))                                                                      
                                                                   
                    
 ELSE 0                      
END                      
                      
-------------------------------------------------------------------------------------------------------------------------------------------------------                  
                
                
Update #MTMDataTable                      
SET                      
EndingMarketValue_Base =                                           
CASE                      
                     
--Market Value with Commission when set to Unrealized P&L                               
WHEN Open_CloseTag = 'O'                 
 and                 
 (                
 (Asset in ('FX','FXForward') and @ShowFXMktValueAsUnrealizedOrZero =2 and @IncludeCommissionInPNL_FX = 1)or                
 (Asset = 'Equity' and IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized =2 and @IncludeCommissionInPNL_Swaps =1 ) or                 
 (Asset = 'Future' and @ShowFutureMktValueAsUnrealizedOrZero = 2 and @IncludeCommissionInPNL_Futures=1) or                 
 (Asset = 'FutureOption' and @BaseCurrencyID <> CurrencyID and @ShowInternationalFutureOptionMktValueAsMVorUnrealizedOrZero = 2 and @IncludeCommissionInPNL_FutOptions=1)                     
 )                
THEN                 
 Isnull(EndingMarketValue_Base,0) - Isnull(NetTotalCost_Base,0)                  
                    
--Market Value without Commission when set to Unrealized P&L                   
WHEN Open_CloseTag = 'O'                 
 and                 
 (                
 (Asset in ('FX','FXForward') and @ShowFXMktValueAsUnrealizedOrZero =2 and @IncludeCommissionInPNL_FX = 0)or                  
 (Asset = 'Equity' and IsSwapped = 1 and @SwapMV_ZeroOrEndingMVOrUnrealized =2 and @IncludeCommissionInPNL_Swaps =0 ) or                 
 (Asset = 'Future' and @ShowFutureMktValueAsUnrealizedOrZero = 2 and @IncludeCommissionInPNL_Futures=0) or                 
 (Asset = 'FutureOption' and @BaseCurrencyID <> CurrencyID and @ShowInternationalFutureOptionMktValueAsMVorUnrealizedOrZero = 2 and @IncludeCommissionInPNL_FutOptions=0)                     
 )                
THEN                 
-- CASE                          
--  WHEN DateDiff(d,@StartDate,TradeDate) >= 0                               
--  THEN (Isnull(EndingMarketValue_Base,0) - Isnull(NetTotalCost_Base,0)) + TotalOpenCommission_Base                                                    
--  ELSE Isnull(EndingMarketValue_Base,0) - Isnull(NetTotalCost_Base,0)                              
--END                     
(Isnull(EndingMarketValue_Base,0) - Isnull(NetTotalCost_Base,0)) + TotalOpenCommission_Base                 
                
                  
WHEN Open_CloseTag = 'C'                                        
THEN 0                      
ELSE EndingMarketValue_Base                                                 
END                      
                      
If @ShowFutureMktValueAsUnrealizedOrZero =3                      
BEGIN                      
UPdate #MTMDataTable                      
SET                      
EndingMarketValue_Base =  0                       
Where Asset = 'Future' and Open_CloseTag = 'O'                      
END                      
                      
-----------------------------------------------------------------------------------------------------------                                                                                                
                                              
Select             
FundID,            
Symbol,            
TaxLotOpenQty,            
AvgPrice,            
ClosingPrice,            
AssetID,            
CurrencyID,            
CurrencySymbol,            
AUECID,        
TotalOpenCommission_Local,            
TotalOpenCommission_Base,            
TotalClosedCommission_Local,            
TotalClosedCommission_Base,            
AssetMultiplier,            
TradeDate,            
ClosingDate,            
Mark1 ,            
Mark2 ,            
IsSwapped ,            
NotionalValue ,            
BenchMarkRate ,            
Differential ,            
OrigCostBasis ,            
DayCount ,                                                                                          
FirstResetDate ,                                                                                  
OrigTransDate ,                                                                                          
SwapData ,                                                                                          
BeginningMarketValue_Local ,                         
BeginningMarketValue_Base ,                          
EndingMarketValue_Local ,                                           
EndingMarketValue_Base ,                                                                                                
Open_CloseTag ,            
ConversionRateTrade,                                                                                          
ConversionRateStart ,                                                                                          
ConversionRateEnd ,                                                                                          
CompanyName ,            
FundName ,            
StrategyName ,            
Side ,            
Asset ,            
UDAAsset ,            
UDASecurityTypeName ,            
UDASectorName ,            
UDASubSectorName ,            
UDACountryName ,            
PutOrCall ,            
MasterFundName ,            
Dividend ,                                                                                    
MasterStrategyName ,            
UnderlyingSymbol ,            
CashFXUnrealizedPNL ,                                    
NetTotalCost_Base ,                             
BloombergSymbol ,            
SedolSymbol ,            
ISINSymbol ,            
CusipSymbol ,            
OSISymbol ,            
IDCOSymbol ,            
UnrealizedPNLMTM_Local ,                      
RealizedPNLMTM_Local ,                      
UnrealizedPNLMTM_Base ,                      
RealizedPNLMTM_Base ,              
ExpirationDate ,              
 OpenTradeAttribute1 ,            
 OpenTradeAttribute2 ,            
 OpenTradeAttribute3 ,            
 OpenTradeAttribute4 ,            
 OpenTradeAttribute5 ,            
 OpenTradeAttribute6 ,            
 ClosedTradeAttribute1 ,            
 ClosedTradeAttribute2 ,            
 ClosedTradeAttribute3 ,            
 ClosedTradeAttribute4 ,            
 ClosedTradeAttribute5 ,            
 ClosedTradeAttribute6 ,          
-- This Quantity is used for  avg price calculations. Since we need Quantity for only open positions          
 Case                 
  when Open_closeTag = 'O'       
  Then TaxLotOpenQty               
  Else                
  0                
 End as  TaxLotOpenQtyProxy            
 from #MTMDataTable      
             
Order By Symbol                
-------------------------------------------------------                                           
                                                                               
DROP TABLE #T_Asset, #PM_Taxlots, #T_CompanyFunds,#T_CorpActionData , #TempClosingData                                                                                                                                
DROP table #MarkPriceForStartDate, #MarkPriceForEndDate ,#SecMasterDataTempTable                      
DROP TABLE #TempSplitFactorForClosed_2,#TempSplitFactorForClosed_1,#TempSplitFactorForOpen,#TempSplitFactorForClosed                                                                                                                                          
  
    
      
         
          
           
              
                                               
DROP table #FXConversionRates, #AUECYesterDates, #AUECBusinessDatesForEndDate,#MTMDataTable                      
END               
              
