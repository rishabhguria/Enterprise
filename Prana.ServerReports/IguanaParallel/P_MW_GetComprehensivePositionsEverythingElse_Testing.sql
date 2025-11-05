           
alter Procedure [dbo].[P_MW_GetComprehensivePositionsEverythingElse_Testing]    
(                                                                                                                 
 @Date datetime,                       
 @fund varchar(max),          
 @IncludeCash bit,          
 @LocalCurrency bit,          
 @NAVSource bit                                                                                                     
)                                                         
As                                                                                                                                             
Begin           
        
Declare @entity table            
(            
 Fundname varchar(max),            
 Companyfundid int            
)            
            
insert into @entity            
select Fundname, companyfundid from t_companyfunds where fundname in (select * from dbo.split(@fund,','))            
            
declare @tmp varchar(250),@FundID varchar(max)            
SET @tmp = ''            
select @tmp = @tmp + cast (CompanyFundID as varchar(10))+ ',' from @entity            
select @FundID=SUBSTRING(@tmp, 0, LEN(@tmp))        
           
          
CREATE TABLE #closed          
(          
Symbol varchar(max),          
Quantity int          
)          
          
INSERT INTO #closed(Symbol, Quantity)          
SELECT DISTINCT Symbol, SUM(BeginningQuantity*SideMultiplier) FROM T_MW_GenericPNL          
WHERE RunDate = @Date          
AND Asset = 'Equity'          
AND Open_CloseTag = 'O'          
AND (Fund in (SELECT * FROM dbo.split(@fund, ',')))          
GROUP BY Symbol          
          
DELETE FROM #closed          
WHERE Quantity != 0          
          
          
CREATE TABLE #t          
(          
SecurityID varchar(max),          
SecurityDescription varchar(max),          
UnderlyingSecurity varchar(max),          
Fund varchar(max),          
Quantity int,          
SideMultiplier int,          
UnitCost float,          
UnitCostLocal float,          
TotalCost float,          
TotalCostLocal float,          
MarketPrice float,          
MarketPriceLocal float,          
MarketValue float,          
MarketValueLocal float,          
Delta float,          
DeltaAdjusted float,          
DeltaAdjustedLocal float,          
UnrealizedPNL float, --On Cost          
UnrealizedPNLLocal float,          
PercentofEquity float,          
AssetClass varchar(max),          
UDAAssetClass varchar(max),          
UDASector varchar(max),          
UDASubSector varchar(max)          
)          
          
IF dbo.IsBusinessDay(@Date,1) = 0          
BEGIN          
SET @Date = dbo.AdjustBusinessDays(@Date,-1,1)          
END          
          
ELSE          
BEGIN          
SET @Date = @Date          
END          
          
INSERT INTO #t(SecurityID,SecurityDescription,UnderlyingSecurity, Fund, Quantity, SideMultiplier, UnitCost, UnitCostLocal, TotalCost, TotalCostLocal,     
MarketPrice, MarketPriceLocal, MarketValue, MarketValueLocal, Delta, DeltaAdjusted, DeltaAdjustedLocal, UnrealizedPNL, UnrealizedPNLLocal, AssetClass,    
UDAAssetClass, UDASector, UDASubSector)          
SELECT DISTINCT Symbol,          
CASE          
when (Max(asset) in ('EquityOption','FutureOption'))          
Then Max(UnderlyingSymbol)+' '+convert(varchar,Max(ExpirationDate), 101)+' '+Max(PutOrCall)+' '+Cast(Max(StrikePrice) as Varchar(10))          
Else Max(SecurityName)          
End as SecurityDescription,          
 UnderlyingSymbol, MAX(Fund), SUM(BeginningQuantity*SideMultiplier), MAX(SideMultiplier), AVG(UnitCostBase), AVG(UnitCostLocal), SUM(TotalCost_Base), SUM(TotalCost_Local), AVG(EndingPriceBase), AVG(EndingPriceLocal), SUM(EndingMarketValueBase),     
SUM(EndingMarketValueLocal), MAX(Delta), SUM(DeltaExposureBase), SUM(DeltaExposureLocal), SUM(UnrealizedTotalGainOnCostD2_Base), SUM(UnrealizedTotalGainOnCostD2_Local), MAX(Asset), MAX(UDAAssetClass), MAX(UDASector), MAX(UDASubSector)          
FROM T_MW_GenericPNL          
WHERE RunDate = @Date      
AND Open_CloseTag = 'O'          
AND UDAAssetClass <> 'OUS'          
AND UDASector <> 'Indices'          
AND UDASubSector <> 'Warrants'          
AND UnderlyingSymbol IN (SELECT DISTINCT Symbol FROM T_MW_GenericPNL          
       WHERE RunDate = @Date          
       AND Open_CloseTag = 'O'           
       AND Fund in (SELECT * FROM dbo.split(@fund, ','))) --Filter out naked options          
AND (Fund in (SELECT * FROM dbo.split(@fund, ',')))          
GROUP BY Symbol,UnderlyingSymbol          
ORDER BY UnderlyingSymbol          
          
--DECLARE @NetEquity float --(Now NAV)          
--SET @NetEquity = (SELECT SUM(NAVValue) FROM PM_NAVValue INNER JOIN T_CompanyFunds ON PM_NAVValue.FundID = T_CompanyFunds.      CompanyFundID          
--     WHERE Date = @Date          
--     AND(FundName in (SELECT * FROM dbo.split(@fund, ','))))          
          
CREATE TABLE #temp(          
 [Rundate] [datetime],        
 [TradeDate] [datetime], 
 [ExpirationDate] [datetime],        
 [Symbol] [varchar](100),        
 [CUSIPSymbol] [varchar](50),        
 [ISINSymbol] [varchar](50),        
 [SEDOLSymbol] [varchar](50),        
 [BloombergSYmbol] [varchar](50),        
 [ReutersSYmbol] [varchar](50),        
 [IDCOSymbol] [varchar](50),        
 [OSISymbol] [varchar](50),        
 [UnderlyingSymbol] [varchar](100),        
 [Fund] [varchar](100),        
 [Asset] [varchar](100),        
 [TradeCurrency] [varchar](10),        
 [Side] [varchar](10),        
 [SecurityName] [varchar](500),        
 [MasterFund] [varchar](100),        
 [Strategy] [varchar](100),        
 [UDASector] [varchar](100),        
 [UDACountry] [varchar](100),        
 [UDASecurityType] [varchar](100),        
 [UDAAssetClass] [varchar](100),        
 [UDASubSector] [varchar](100),        
 [UnitCostLocal] [float],        
 [OpeningFXRate] [float],        
 [UnitCostBase] [float],        
 [EndingFXRate] [float],        
 [EndingPriceLocal] [float],        
 [EndingPriceBase] [float], 
OpenTradeAttribute1 [varchar](200),        
OpenTradeAttribute2 [varchar](200),        
OpenTradeAttribute3 [varchar](200),        
OpenTradeAttribute4 [varchar](200),        
OpenTradeAttribute5 [varchar](200),        
OpenTradeAttribute6 [varchar](200),
[BeginningQuantity] [float],        
 [Multiplier] [float],        
 [SideMultiplier] [varchar](5),        
 [PutOrCall] [varchar](10),        
 [ISSwapped] [bit],        
 [Open_CloseTag] [varchar](5),        
 [TotalOpenCommissionAndFees_Local] [float],        
 [TotalOpenCommissionAndFees_Base] [float],        
 [TotalCost_Local] [float],        
 [TotalCost_Base] [float],        
 [EndingMarketValueLocal] [float],        
 [EndingMarketValueBase] [float],        
 [UnrealizedTradingGainOnCostD2_Base] [float],        
 [UnrealizedFXGainOnCostD2_Base] [float],        
 [UnrealizedTotalGainOnCostD2_Local] [float],        
 [UnrealizedTotalGainOnCostD2_Base] [float],      
OriginalPurchaseDate datetime,            
BaseCurrency varchar(10),       
UnderlyingSymbolCompanyName nvarchar(50),      
PercentageAsset float           
)          
INSERT INTO #temp          
--EXEC P_MW_GetUnrealizedPNL @date,2,2,2,'1','0','0','0','0','0'          
EXEC P_MW_GetUnrealizedPNL @date,'Unrealized_MW',@FundID,'1,2,3,4,5,6,7,8,9,10,11,12,13,14','','Symbol','Select','Select','Select','Select'              
          
DECLARE @NetEquity float --(Now NAV)          
SET @NetEquity =          
 CASE          
  WHEN @NAVSource = 1          
  THEN (SELECT SUM(NAVValue) FROM PM_NAVValue INNER JOIN T_CompanyFunds ON PM_NAVValue.FundID = T_CompanyFunds.CompanyFundID          
     WHERE Date = @Date          
     AND(FundName in (SELECT * FROM dbo.split(@fund, ','))))          
  ELSE (SELECT SUM(EndingMarketValueBase) FROM #temp          
        WHERE Fund IN (SELECT * FROM dbo.split(@fund, ',')))          
 END          
          
UPDATE #t          
SET PercentOfEquity = MarketValue / @NetEquity          
          
DELETE FROM #t          
WHERE (Quantity = 0 OR Quantity IS NULL)          
          
DELETE FROM #t          
WHERE AssetClass = 'EquityOption'          
AND UnderlyingSecurity IN (SELECT DISTINCT Symbol from #closed)          
          
DROP TABLE #closed          
DROP TABLE #temp          
          
IF @IncludeCash = 0          
BEGIN          
 SELECT * FROM #t          
 WHERE AssetClass != 'Cash'          
END          
ELSE          
 SELECT * FROM #t          
          
RETURN;                                                                   
End 