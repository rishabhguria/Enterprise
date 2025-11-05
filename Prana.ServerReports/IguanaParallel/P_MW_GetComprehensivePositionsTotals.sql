 alter Procedure [dbo].[P_MW_GetComprehensivePositionsTotals]                                                                                  
(                                                                                                             
 @Date datetime,                   
 @fund varchar(max),      
 @NAVSource bit      
 --@LocalCurrency bit                                                                                              
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
      
CREATE TABLE #t      
(      
DeltaAdjustedExposure float,      
--DeltaAdjustedExposureLocal float,      
NAV float,      
PreviousDayNAV float,      
PreviousDayPercent float,      
BeginningMonthNAV float,      
BeginningMonthPercent float      
)      
      
IF dbo.IsBusinessDay(@Date,1) = 0      
BEGIN      
SET @Date = dbo.AdjustBusinessDays(@Date,-1,1)      
END      
      
ELSE      
BEGIN      
SET @Date = @Date      
END      
      
DECLARE @prevDate datetime      
SET @prevDate = dbo.AdjustBusinessDays(@Date,-1,1)      
      
INSERT INTO #t(DeltaAdjustedExposure)      
SELECT DISTINCT SUM(DeltaExposureBase)      
FROM T_MW_GenericPNL      
WHERE RunDate = @Date       
AND Open_CloseTag = 'O'      
AND (Fund in (SELECT * FROM dbo.split(@fund, ',')))      
      
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
EXEC P_MW_GetUnrealizedPNL @date,'Unrealized_MW',@FundID,'1,2,3,4,5,6,7,8,9,10,11,12,13,14','','Symbol','Select','Select','Select','Select'     
  

 
CREATE TABLE #temp2(      
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
INSERT INTO #temp2      
EXEC P_MW_GetUnrealizedPNL @prevDate,'Unrealized_MW',@FundID,'1,2,3,4,5,6,7,8,9,10,11,12,13,14','','Symbol','Select','Select','Select','Select'  
      
DECLARE @BeginningMonthDate datetime      
DECLARE @EndPrevMonthDate datetime      
SET @BeginningMonthDate = CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@Date)-1),@Date),101)      
SET @EndPrevMonthDate = dbo.AdjustBusinessDays(@BeginningMonthDate,-1,1)      
      
CREATE TABLE #temp3(      
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
INSERT INTO #temp3      
EXEC P_MW_GetUnrealizedPNL @EndPrevMonthDate,'Unrealized_MW',@FundID,'1,2,3,4,5,6,7,8,9,10,11,12,13,14','','Symbol','Select','Select','Select','Select'   
      
UPDATE #t      
SET       
NAV =      
 CASE      
  WHEN @NAVSource = 1      
  THEN (SELECT SUM(NAVValue) FROM PM_NAVValue INNER JOIN T_CompanyFunds ON PM_NAVValue.FundID = T_CompanyFunds.CompanyFundID      
     WHERE Date = @Date      
     AND(FundName in (SELECT * FROM dbo.split(@fund, ','))))      
  ELSE (SELECT SUM(EndingMarketValueBase) FROM #temp      
        WHERE Fund IN (SELECT * FROM dbo.split(@fund, ',')))      
 END,      
PreviousDayNAV =      
 CASE      
  WHEN @NAVSource = 1      
  THEN (SELECT SUM(NAVValue) FROM PM_NAVValue INNER JOIN T_CompanyFunds ON PM_NAVValue.FundID = T_CompanyFunds.CompanyFundID      
     WHERE Date = @prevDate      
     AND(FundName in (SELECT * FROM dbo.split(@fund, ','))))      
  ELSE (SELECT SUM(EndingMarketValueBase) FROM #temp2      
        WHERE Fund IN (SELECT * FROM dbo.split(@fund, ',')))      
 END,      
BeginningMonthNAV =      
 CASE      
  WHEN @NAVSource = 1      
  THEN (SELECT SUM(NAVValue) FROM PM_NAVValue INNER JOIN T_CompanyFunds ON PM_NAVValue.FundID = T_CompanyFunds.CompanyFundID      
     WHERE Date = @EndPrevMonthDate      
     AND(FundName in (SELECT * FROM dbo.split(@fund, ','))))      
  ELSE (SELECT SUM(EndingMarketValueBase) FROM #temp3      
        WHERE Fund IN (SELECT * FROM dbo.split(@fund, ',')))      
 END      
      
UPDATE #t      
SET      
PreviousDayPercent = (NAV - PreviousDayNAV)/PreviousDayNAV,      
BeginningMonthPercent = (NAV - BeginningMonthNAV)/BeginningMonthNAV      
      
SELECT * FROM #t      
DROP TABLE #temp      
DROP TABLE #temp2      
DROP TABLE #temp3      
--      
RETURN;                                                                                    
End 